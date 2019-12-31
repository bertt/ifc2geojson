using Wkx;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.Interfaces;

namespace ifc2geojson.core.extensions
{
    public static class IfcObjectPlacementExtensions
    {
        public static Point ToAbsoluteLocation(this IIfcObjectPlacement objectPlacement, Point referencePoint, double LengthUnitPower)
        {
            var relativeLocation = ((IIfcAxis2Placement3D)((IfcLocalPlacement)objectPlacement).RelativePlacement).Location;
            var (x,y) = LonLat.AddDelta((double)referencePoint.X, (double)referencePoint.Y, relativeLocation.X * LengthUnitPower, relativeLocation.Y * LengthUnitPower);
            var point = new Point(x,y,referencePoint.Z + relativeLocation.Z);
            return point;
        }
    }
}
