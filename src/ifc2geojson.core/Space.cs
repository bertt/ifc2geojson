
using GeoJSON.Net.Geometry;

namespace ifc2geojson.core
{
    public class Space
    {
        public string Name { get; set; }
        public string LongName { get; set; }
        public Polygon Polygon { get; set; }

        public Position Location { get; set; }
    }
}
