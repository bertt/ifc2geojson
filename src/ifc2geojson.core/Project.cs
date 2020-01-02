using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Project: IElement
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string GlobalId { get; set; }

        public Dictionary<string, object> Properties { get; set; }
        public Site Site { get; set; }

        public double LengthUnitPower { get; set; }

        public double BoundingBoxLength { get; set; }
        public double BoundingBoxWidth { get; set; }
        public double BoundingBoxHeight { get; set; }

        public double GlobalX { get; set; }
        public double GlobalY { get; set; }

        public double GlobalZ { get; set; }


    }
}
