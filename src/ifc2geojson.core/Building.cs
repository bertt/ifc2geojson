﻿using GeoJSON.Net.Geometry;
using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Building: IElement
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string GlobalId { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        public List<Storey> Storeys { get; set; }

        public int YearOfConstruction { get; set; }

        public Position Location { get; set; }
    }
}
