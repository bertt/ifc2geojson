using GeoJSON.Net.Geometry;
using ifc2geojson.core.extensions;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.TopologyResource;

namespace ifc2geojson.core
{
    public static class IfcParser
    {
        public static Project ParseModel(IfcStore model)
        {
            var ifcProject = model.FederatedInstances.OfType<IfcProject>().FirstOrDefault();

            var project = new Project();
            project.LengthUnitPower = ifcProject.UnitsInContext.LengthUnitPower;
            project.Name = ifcProject.Name;
            project.Description = ifcProject.Description;
            project.Site = ParseSite(ifcProject.Sites.FirstOrDefault(), project.LengthUnitPower);
            return project;
        }

        private static Site ParseSite(IIfcSite ifcSite, double LengthUnitPower)
        {
            var site = new Site();
            site.Name = ifcSite.Name;
            site.Description = ifcSite.Description;
            site.ReferencePoint = new Position(ifcSite.RefLatitude.Value.AsDouble, ifcSite.RefLongitude.Value.AsDouble, ifcSite.RefElevation.Value);
            site.Building = ParseBuilding(ifcSite.Buildings.FirstOrDefault(), LengthUnitPower, site.ReferencePoint);
            return site;
        }

        private static Building ParseBuilding(IIfcBuilding ifcBuilding, double LengthUnitPower, Position SiteLocation)
        {
            var building = new Building();
            building.Location = ifcBuilding.ObjectPlacement.ToAbsoluteLocation(SiteLocation, LengthUnitPower);
            building.Name = ifcBuilding.Name;
            building.Description = ifcBuilding.Description;
            building.Storeys = ParseStoreys(ifcBuilding.BuildingStoreys, LengthUnitPower, building.Location);
            // todo: how to read properties like BuildingID, grossPlannedArea, NetAreaPlanned?
            return building;
        }

        private static List<Storey> ParseStoreys(IEnumerable<IIfcBuildingStorey> ifcStoreys, double LengthUnitPower, Position BuildingLocation)
        {
            var storeys = new List<Storey>();
            foreach (var ifcStorey in ifcStoreys)
            {
                storeys.Add(ParseStoreys(ifcStorey, LengthUnitPower, BuildingLocation));
            }
            return storeys;
        }

        private static Storey ParseStoreys(IIfcBuildingStorey ifcStorey, double LengthUnitPower, Position BuildingLocation)
        {
            var storey = new Storey();
            storey.Name = ifcStorey.Name;
            storey.Elevation = ifcStorey.Elevation.Value;
            storey.GrossFloorArea = ifcStorey.GrossFloorArea.Value;
            storey.Location = ifcStorey.ObjectPlacement.ToAbsoluteLocation(BuildingLocation, LengthUnitPower);
            storey.Spaces = ParseSpaces(ifcStorey.Spaces, LengthUnitPower, storey.Location);
            // todo: parse walls, stairs, doors
            return storey;
        }

        private static List<Space> ParseSpaces(IEnumerable<IIfcSpace> ifcSpaces, double LengthUnitPower, Position StoreyLocation)
        {
            var spaces = new List<Space>();

            foreach (var ifcSpace in ifcSpaces)
            {
                spaces.Add(ParseSpace(ifcSpace, LengthUnitPower, StoreyLocation));
            }
            return spaces;
        }

        private static Space ParseSpace(IIfcSpace ifcSpace, double LengthUnitPower, Position StoreyLocation)
        {
            var space = new Space();
            space.Name = ifcSpace.Name;
            space.LongName = ifcSpace.LongName;
            space.Location = ifcSpace.ObjectPlacement.ToAbsoluteLocation(StoreyLocation, LengthUnitPower);
            space.Polygon = HandleGeometry(ifcSpace, LengthUnitPower, space.Location);
            return space;
        }

        private static Polygon HandleGeometry(IIfcSpace ifcSpace, double LengthUnitPower, Position SpaceLocation)
        {
            Polygon polygon = null; 
            var representation = ifcSpace.Representation.Representations[0].Items[0];
            if (representation is IfcFacetedBrep)
            {
                polygon = HandleFacetedBrep((IfcFacetedBrep)representation, LengthUnitPower, SpaceLocation);
            }

            return polygon;
        }

        private static Polygon HandleFacetedBrep(IfcFacetedBrep representation, double lengthUnitPower, Position SpaceLocation)
        {
            var points = new List<IPosition>();
            var outer = representation.Outer;

            foreach (var face in outer.CfsFaces)
            {
                foreach (var bound in face.Bounds)
                {
                    var polyloop = (IfcPolyLoop)bound.Bound;
                    var xbimPolygon = polyloop.Polygon;

                    foreach (var pnt in xbimPolygon)
                    {
                        var newp = LonLat.AddDelta(SpaceLocation.Longitude, SpaceLocation.Latitude, pnt.X * lengthUnitPower, pnt.Y * lengthUnitPower);
                        var position = new Position(newp.y, newp.x); // todo: add z? 
                        if (!points.Contains(position))
                        {
                            points.Add(position);
                        }
                    }
                }
            }
            var ls = new LineString(points);
            if (!ls.IsClosed())
            {
                points.Add(points[0]);
                ls = new LineString(points);
            }
            var polygon = new Polygon(new List<LineString> { ls });
            return polygon;
        }
    }
}
