using GeoJSON.Net.Geometry;
using System.Collections.Generic;

namespace ifc2geojson.core
{
    public class Site:Element
    {
        public Building Building { get; set; }

        public Position ReferencePoint { get; set; }

    }
}
