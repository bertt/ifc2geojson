using System;

namespace ifc2geojson.core
{
    public static class LonLat
    {
        public static (double x, double y) AddDelta(double longitude, double latitude, double dx, double dy)
        {
            var lat = latitude + (180 / Math.PI) * (dy / 6378137);
            var lon = longitude + (180 / Math.PI) * (dx / 6378137) / Math.Cos(latitude);
            return (lon, lat);
        }
    }
}
