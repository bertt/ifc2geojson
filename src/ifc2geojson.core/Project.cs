using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Project: Element
    {
        public Site Site { get; set; }

        public string FriendlyName { get; set; }

        public double LengthUnitPower { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Window> Windows { get; set; }
    }
}
