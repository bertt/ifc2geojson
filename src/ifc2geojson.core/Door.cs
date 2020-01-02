﻿
using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Door: IElement
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string GlobalId { get; set; }
        public Dictionary<string, object> Properties { get; set; }


        public double Height { get; set; }

        public double Width { get; set; }
    }
}
