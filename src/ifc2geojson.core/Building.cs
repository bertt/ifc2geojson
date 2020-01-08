using GeoJSON.Net.Geometry;
using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Building: Element
    {
        public List<Storey> Storeys { get; set; }

        public int YearOfConstruction { get; set; }

        public Position Location { get; set; }

        public BuildingAddress BuildingAdress { get; set; }

    }
}
