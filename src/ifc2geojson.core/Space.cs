using Wkx;

namespace ifc2geojson.core
{
    public class Space
    {
        public string Name { get; set; }
        public string LongName { get; set; }
        public Wkx.Polygon Polygon { get; set; }

        public Point Location { get; set; }
    }
}
