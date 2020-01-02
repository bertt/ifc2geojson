using CommandLine;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using ifc2geojson.core;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using Xbim.Ifc;

namespace ifc2geojson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("tool ifc2geojson");
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                Console.WriteLine("Input file: " + o.Input);

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var model = IfcStore.Open(o.Input);

                var project = IfcParser.ParseModel(model);
                Console.WriteLine(project.Name);
                var storeys = project.Site.Building.Storeys;
                foreach(var storey in storeys)
                {
                    Console.WriteLine(storey.Name);
                }

                var fc = ToGeoJson(storeys[0]);
                var serializedData = JsonConvert.SerializeObject(fc);
                File.WriteAllText($"{storeys[0].Name}.geojson", serializedData);

                stopwatch.Stop();
                Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            });
        }

        private static FeatureCollection ToGeoJson(Storey storey)
        {
            var fc = new FeatureCollection();

            foreach(var space in storey.Spaces) {

                // var poly = space.Location;
                var point = new Point(space.Location);
                var f = new Feature(point);
                f.Properties.Add("name", space.LongName);
                fc.Features.Add(f);
            }

            return fc;
        }
    }
}
