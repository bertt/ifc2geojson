using ifc2geojson.core;
using NUnit.Framework;

namespace ifc2geojson.tests
{
    public class LonLatTests
    {
        [Test]
        public void AddDeltaTest()
        {
            var (x,y) = LonLat.AddDelta(4.5, 51, 100, 200);
            // Assert.IsTrue();
            Assert.IsTrue(x == 4.5012104159593465);
            Assert.IsTrue(y == 51.001796630568236);
        }

    }
}
