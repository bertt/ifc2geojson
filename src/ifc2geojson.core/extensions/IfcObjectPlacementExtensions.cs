using GeoJSON.Net.Geometry;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.Interfaces;

namespace ifc2geojson.core.extensions
{
    public static class IfcObjectPlacementExtensions
    {
        public static Position ToAbsoluteLocation(this IIfcObjectPlacement objectPlacement, Position referencePoint, double LengthUnitPower)
        {
            var relativeLocation = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)objectPlacement).RelativePlacement).Location;
            var (x,y) = LonLat.AddDelta((double)referencePoint.Longitude, (double)referencePoint.Latitude, relativeLocation.X * LengthUnitPower, relativeLocation.Y * LengthUnitPower);
            var point = new Position(y,x,referencePoint.Altitude + relativeLocation.Z);
            return point;
        }
    }
}
