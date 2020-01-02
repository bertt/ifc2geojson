
using GeoJSON.Net.Geometry;
using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Space: Element
    {
        public string LongName { get; set; }
        public Polygon Polygon { get; set; }

        public Position Location { get; set; }
    }
}
