using CommandLine;
using System;
using Xbim.Ifc;

namespace ifc2geojson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("tool ifc2gltf");
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                Console.WriteLine("Input file: " + o.Input);

                var model = IfcStore.Open(o.Input);

                Ifc2GeoJSON.Convert(model);

            });
        }
    }
}
