using Wkx;
using Xbim.Ifc4.Interfaces;

namespace ifc2geojson.core.extensions
{
    public static class IfcCartesianPointExtensions
    {
        public static Point ToPoint(this IIfcCartesianPoint ifcPoint)
        {
            return new Point(ifcPoint.X, ifcPoint.Y, ifcPoint.Z);
        }
    }
}
