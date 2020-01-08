using GeoJSON.Net.Geometry;

namespace ifc2geojson.core
{
    public class Space: Element
    {
        public string LongName { get; set; }
        public Polygon Polygon { get; set; }

        public Position Location { get; set; }

        public double Height { get; set; }

        public double NetfloorArea { get; set; }
        public double GrossFloorArea { get; set; }
        public double GrossPerimeter { get; set; }
    }
}
