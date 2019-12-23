using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Project
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Site Site { get; set; }

        public double LengthUnitPower { get; set; }
    }
}
