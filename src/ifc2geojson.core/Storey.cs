using System.Collections.Generic;
using Xbim.Ifc4.MeasureResource;

namespace ifc2geojson.core
{
    public class Storey
    {
        public string Name { get; set; }

        public double Elevation { get; set; }

        public double GrossFloorArea { get; set; }
        public List<Space> Spaces { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Stair> Stairs { get; set; }
        public List<Door> Doors { get; set; }

    }
}
