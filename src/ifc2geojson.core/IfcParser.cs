using System.Collections.Generic;
using System.Linq;
using Wkx;
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
            site.ReferencePoint = new Point(ifcSite.RefLongitude.Value.AsDouble, ifcSite.RefLatitude.Value.AsDouble, ifcSite.RefElevation.Value);
            site.Building = ParseBuilding(ifcSite.Buildings.FirstOrDefault(), LengthUnitPower, site.ReferencePoint);
            return site;
        }

        private static Building ParseBuilding(IIfcBuilding ifcBuilding, double LengthUnitPower, Point SiteLocation)
        {
            var building = new Building();
            building.Name = ifcBuilding.Name;
            building.Description = ifcBuilding.Description;
            building.Storeys = ParseStoreys(ifcBuilding.BuildingStoreys, LengthUnitPower, SiteLocation);
            // todo: how to read properties like BuildingID, grossPlannedArea, NetAreaPlanned?
            return building;
        }

        private static List<Storey> ParseStoreys(IEnumerable<IIfcBuildingStorey> ifcStoreys, double LengthUnitPower, Point SiteLocation)
        {
            var storeys = new List<Storey>();
            foreach (var ifcStorey in ifcStoreys)
            {
                storeys.Add(ParseStoreys(ifcStorey, LengthUnitPower, SiteLocation));
            }
            return storeys;
        }

        private static Storey ParseStoreys(IIfcBuildingStorey ifcStorey, double LengthUnitPower, Point SiteLocation)
        {
            var storey = new Storey();
            storey.Name = ifcStorey.Name;
            storey.Elevation = ifcStorey.Elevation.Value;
            storey.GrossFloorArea = ifcStorey.GrossFloorArea.Value;
            storey.Spaces = ParseSpaces(ifcStorey.Spaces, LengthUnitPower, SiteLocation);
            // todo: parse walls, stairs, doors
            return storey;
        }

        private static List<Space> ParseSpaces(IEnumerable<IIfcSpace> ifcSpaces, double LengthUnitPower, Point SiteLocation)
        {
            var spaces = new List<Space>();

            foreach (var ifcSpace in ifcSpaces)
            {
                spaces.Add(ParseSpace(ifcSpace, LengthUnitPower, SiteLocation));
            }
            return spaces;
        }

        private static Space ParseSpace(IIfcSpace ifcSpace, double LengthUnitPower, Point SiteLocation)
        {
            var space = new Space();
            space.Name = ifcSpace.Name;
            space.LongName = ifcSpace.LongName;
            space.Geometry = HandleGeometry(ifcSpace, LengthUnitPower, SiteLocation);
            return space;
        }

        private static Polygon HandleGeometry(IIfcSpace ifcSpace, double LengthUnitPower, Point SiteLocation)
        {
            Polygon polygon = null; 
            var representation = ifcSpace.Representation.Representations[0].Items[0];
            if (representation is IfcFacetedBrep)
            {
                polygon = HandleFacetedBrep((IfcFacetedBrep)representation, LengthUnitPower, SiteLocation);
            }

            return polygon;
        }

        private static Polygon HandleFacetedBrep(IfcFacetedBrep representation, double lengthUnitPower, Point SiteLocation)
        {
            // todo: rewrite this method for correct handling faceted brep
            var polygon = new Polygon();
            var outer = representation.Outer;

            foreach (var face in outer.CfsFaces)
            {
                foreach (var bound in face.Bounds)
                {
                    var polyloop = (IfcPolyLoop)bound.Bound;
                    var xbimPolygon = polyloop.Polygon;

                    foreach (var pnt in xbimPolygon)
                    {
                        var newp = LonLat.AddDelta(SiteLocation.X.Value, SiteLocation.Y.Value, pnt.X * lengthUnitPower, pnt.Y * lengthUnitPower);
                        var point = new Point(newp.y, newp.x);
                        polygon.ExteriorRing.Points.Add(point);
                    }
                }
            }
            return polygon;
        }
    }
}
