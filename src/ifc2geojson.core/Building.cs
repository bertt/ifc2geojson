using System.Collections.Generic;
using Wkx;

namespace ifc2geojson.core
{
    public class Building
    {
        public List<Storey> Storeys { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int YearOfConstruction { get; set; }

        public Point Location { get; set; }
    }
}
