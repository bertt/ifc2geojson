using GeoJSON.Net.Geometry;
using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Storey: Element
    {
        public double Elevation { get; set; }

        public double GrossFloorArea { get; set; }
        public List<Space> Spaces { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Stair> Stairs { get; set; }
        public List<Door> Doors { get; set; }

        public Position Location { get; set; }
    }
}
