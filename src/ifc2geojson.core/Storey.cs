using GeoJSON.Net.Geometry;
using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Storey: IElement
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string GlobalId { get; set; }

        public Dictionary<string, object> Properties { get; set; }


        public double Elevation { get; set; }

        public double GrossFloorArea { get; set; }
        public List<Space> Spaces { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Stair> Stairs { get; set; }
        public List<Door> Doors { get; set; }

        public Position Location { get; set; }
    }
}
