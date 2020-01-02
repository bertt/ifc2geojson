using GeoJSON.Net.Geometry;
using ifc2geojson.core.extensions;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.PropertyResource;
using Xbim.Ifc4.TopologyResource;

namespace ifc2geojson.core
{
    public static class IfcParser
    {
        public static void ParseElement(Element element, IIfcRoot root)
        {
            element.Name = root.Name;
            element.GlobalId = root.GlobalId;
            element.Description = root.Description;
        }

        public static Project ParseModel(IfcStore model)
        {
            var ifcProject = model.FederatedInstances.OfType<IfcProject>().FirstOrDefault();
            var project = new Project();
            project.LengthUnitPower = ifcProject.UnitsInContext.LengthUnitPower;
            project.Properties = GetPropertiesFromContext(ifcProject);
            ParseElement(project, ifcProject);
            project.Site = ParseSite(ifcProject.Sites.FirstOrDefault(), project.LengthUnitPower);
            return project;
        }

        private static Dictionary<string, object> GetPropertiesFromObject(IIfcObject obj)
        {
            var relations = obj.IsDefinedBy.OfType<IfcRelDefinesByProperties>();
            var dict = GetProperties(relations);
            return dict;
        }

        private static Dictionary<string, object> GetPropertiesFromContext(IfcContext context)
        {
            var relations = context.IsDefinedBy.OfType<IfcRelDefinesByProperties>();
            var dict = GetProperties(relations);
            return dict;
        }

        private static Dictionary<string, object> GetProperties(IEnumerable<IfcRelDefinesByProperties> relations)
        {
            var dict = new Dictionary<string, object>();
            foreach (var rel in relations)
            {
                var relatedPropdef = rel.RelatingPropertyDefinition;

                if(relatedPropdef is IfcPropertySet)
                {
                    var propset = (IfcPropertySet)rel.RelatingPropertyDefinition;
                    var properties = propset.PropertySetDefinitions;
                    foreach (var property in properties)
                    {
                        var props = ((IIfcPropertySet)property).HasProperties;
                        foreach (var prop in props)
                        {
                            var key = ((IfcPropertySingleValue)prop).Name;
                            var val = ((IfcPropertySingleValue)prop).NominalValue.Value;
                            if (!dict.ContainsKey(key))
                            {
                                dict.Add(key, val);
                            }
                        }
                    }
                }
                else
                {
                    var elQuantity = (IIfcElementQuantity)relatedPropdef;
                    foreach(var q in elQuantity.Quantities)
                    {
                        if(q is IIfcQuantityLength)
                        {
                            var length = (IIfcQuantityLength)q;
                            if (!dict.ContainsKey(length.Name)){
                                dict.Add(length.Name, length.LengthValue);
                            }
                        }
                        else if(q is IIfcQuantityArea)
                        {
                            var area = (IIfcQuantityArea)q;
                            if (!dict.ContainsKey(area.Name))
                            {
                                dict.Add(area.Name, area.AreaValue);
                            }
                        }
                    }
                }
            }
            return dict;
        }

        private static Site ParseSite(IIfcSite ifcSite, double LengthUnitPower)
        {
            var site = new Site();
            ParseElement(site, ifcSite);
            site.Properties = GetPropertiesFromObject(ifcSite);
            ParseGeometry(ifcSite, site);
            site.ReferencePoint = new Position(ifcSite.RefLatitude.Value.AsDouble, ifcSite.RefLongitude.Value.AsDouble, ifcSite.RefElevation.Value);
            site.Building = ParseBuilding(ifcSite.Buildings.FirstOrDefault(), LengthUnitPower, site.ReferencePoint);
            return site;
        }

        private static void ParseGeometry(IIfcProduct ifcProduct, Element element)
        {
            var representation_box = ifcProduct.Representation.Representations.Where(x => x.RepresentationIdentifier == "Box").FirstOrDefault(); // envelope
            var bb = (IfcBoundingBox)representation_box.Items.FirstOrDefault();
            element.GlobalX = bb.Corner.X;
            element.GlobalY = bb.Corner.Y;
            element.GlobalZ = bb.Corner.Z;
            element.BoundingBoxLength = bb.XDim;
            element.BoundingBoxWidth = bb.YDim;
            element.BoundingBoxHeight = bb.ZDim;
        }

        private static Building ParseBuilding(IIfcBuilding ifcBuilding, double LengthUnitPower, Position SiteLocation)
        {
            var building = new Building();
            ParseElement(building, ifcBuilding);
            // in case of building representation is null, todo search other method
            // ParseGeometry(ifcBuilding, building);
            building.Properties = GetPropertiesFromObject(ifcBuilding);
            building.Location = ifcBuilding.ObjectPlacement.ToAbsoluteLocation(SiteLocation, LengthUnitPower);
            building.Storeys = ParseStoreys(ifcBuilding.BuildingStoreys, LengthUnitPower, building.Location);
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
            ParseElement(storey, ifcStorey);
            // ParseGeometry(ifcStorey, storey);
            storey.Properties = GetPropertiesFromObject(ifcStorey);
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
            ParseElement(space, ifcSpace);
            ParseGeometry(ifcSpace, space);

            space.Properties = GetPropertiesFromObject(ifcSpace);
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
