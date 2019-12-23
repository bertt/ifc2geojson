using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Site
    {
        public string Name { get; set; }

        public string Description{ get; set; }

        public Building Building { get; set; }

        public Wkx.Point ReferencePoint { get; set; }
    }
}
