using System;
using System.Collections.Generic;
using System.Text;

namespace ifc2geojson.core
{
    public class BuildingAddress
    {
        public string AddressLines { get; set; }
        public string Country { get; set; }

        public string Region { get; set; }
        public string Town { get; set; }

    }
}
