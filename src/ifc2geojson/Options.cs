using CommandLine;

namespace ifc2geojson
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file")]
        public string Input { get; set; }
    }
}
