using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.ProfileResource;
using Xbim.Ifc4.TopologyResource;

namespace ifc2geojson
{
    public static class Ifc2GeoJSON
    {
        public static void Convert(IfcStore model)
        {
            var ifcProject = model.FederatedInstances.OfType<IfcProject>().FirstOrDefault();
            var buildings = ifcProject.Buildings;
            Console.WriteLine("Buildings: " + ifcProject.Buildings.Count());
            HandleBuildings(ifcProject, buildings);
        }

        private static void HandleBuildings(IfcProject ifcProject, IEnumerable<IIfcBuilding> buildings)
        {
            foreach (var building in buildings)
            {
                // Console.WriteLine("description: " + building.BuildingAddress.Description);
                var storeys = building.BuildingStoreys;
                var site = ifcProject.Sites.FirstOrDefault();
                HandleStoreys(storeys, site.RefLongitude.Value.AsDouble, site.RefLatitude.Value.AsDouble);
            }
        }

        private static void HandleStoreys(IEnumerable<IIfcBuildingStorey> storeys, double longitude, double latitude)
        {
            foreach (var storey in storeys)
            {
                Console.WriteLine("Processing storey " + storey.LongName);
                HandleSpaces(storey.Spaces, storey.Name, longitude, latitude);
            }
        }

        private static void HandleSpaces(IEnumerable<IIfcSpace> spaces, string storey_name, double longitude, double latitude)
        {
            var storey_data = new FeatureCollection();

            foreach (var space in spaces)
            {
                HandleSpace(space, storey_data, longitude, latitude);
            }

            var serializedData = JsonConvert.SerializeObject(storey_data);
            File.WriteAllText($"{storey_name}.geojson", serializedData);
        }

        private static void HandleSpace(IIfcSpace space, FeatureCollection features, double longitude, double latitude)
        {
            var representation = space.Representation.Representations[0].Items[0];
            if (representation is IfcExtrudedAreaSolid)
            {
                HandleExtrudedAreaSolid((IfcExtrudedAreaSolid)representation, features, longitude, latitude);
            }
            else if(representation is IfcFacetedBrep)
            {
                HandleFacetedBrep((IfcFacetedBrep)representation, features, longitude, latitude);
            }
            // todo: handle IfcPolyline?
            // todo: handle IfcBooleanClippingResult?

        }

        private static void HandleFacetedBrep(IfcFacetedBrep facetedBrep, FeatureCollection features, double longitude, double latitude)
        {
            var outer = facetedBrep.Outer;

            foreach(var face in outer.CfsFaces)
            {
                foreach (var bound in face.Bounds)
                {
                    var polyloop = (IfcPolyLoop)bound.Bound;
                    var polygon = polyloop.Polygon;

                    var points = new List<IPosition>();

                    foreach (var pnt in polygon)
                    {
                        var newp = AddDelta(longitude, latitude, pnt.X / 1000, pnt.Y / 1000);
                        points.Add(new Position(newp.y, newp.x));

                    }

                    var featureProperties = new Dictionary<string, object> { };
                    var ls = new LineString(points);
                    var feat = new Feature(ls, featureProperties);
                    features.Features.Add(feat);

                }
            }

        }

        private static void HandleExtrudedAreaSolid(IfcExtrudedAreaSolid extrudedAreaSolid, FeatureCollection features, double longitude, double latitude)
        {
            var depth = extrudedAreaSolid.Depth;

            if (extrudedAreaSolid.SweptArea is IfcArbitraryClosedProfileDef)
            {
                var sweptArea = (IfcArbitraryClosedProfileDef)extrudedAreaSolid.SweptArea;
                if (sweptArea.OuterCurve is IfcPolyline)
                {
                    var outercurve = (IfcPolyline)sweptArea.OuterCurve;
                    var line = outercurve.Points;

                    var points = new List<IPosition>();
                    foreach (var pnt in line)
                    {
                        var newp = AddDelta(longitude, latitude, pnt.X / 1000, pnt.Y / 1000);
                        points.Add(new Position(newp.y, newp.x));
                    }
                    var featureProperties = new Dictionary<string, object> { };
                    var ls = new LineString(points);
                    var feat = new Feature(ls, featureProperties);
                    features.Features.Add(feat);
                }
            }
        }

        private static (double x, double y) AddDelta(double longitude, double latitude, double dx, double dy)
        {
            var lat = latitude + (180 / Math.PI) * (dy / 6378137);
            var lon = longitude + (180 / Math.PI) * (dx / 6378137) / Math.Cos(latitude);
            return (lon, lat);
        }
    }
}
