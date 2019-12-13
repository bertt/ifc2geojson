using CommandLine;
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

                Ifc2GeoJSON.Convert(model);

                stopwatch.Stop();
                Console.WriteLine("Converting to GeoJSON per storey finished.");
                Console.WriteLine("Elapsed: " + stopwatch.Elapsed);


            });
        }
    }
}
