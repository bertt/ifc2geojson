using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Building
    {
        public List<Storey> Storeys { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int YearOfConstruction { get; set; }

    }
}
