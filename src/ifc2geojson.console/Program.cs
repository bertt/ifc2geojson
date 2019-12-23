using CommandLine;
using ifc2geojson.core;
using System;
using System.Diagnostics;
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
                stopwatch.Stop();
                Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            });
        }
    }
}
