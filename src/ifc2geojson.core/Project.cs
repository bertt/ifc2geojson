using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Project: Element
    {
        public Site Site { get; set; }

        public double LengthUnitPower { get; set; }
    }
}
