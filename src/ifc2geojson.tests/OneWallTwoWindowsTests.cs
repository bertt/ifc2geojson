using ifc2geojson.core;
using NUnit.Framework;
using System.Linq;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace ifc2geojson.tests
{
    public class OneWallTwoWindowsTests
    {
        IfcStore model;

        [SetUp]
        public void Setup()
        {
            model = IfcStore.Open("assets/OneWallTwoWindows.ifc");
            Assert.IsTrue(model.SchemaVersion == XbimSchemaVersion.Ifc2X3);
        }

        [Test]
        public void ReadTest()
        {
            var project = IfcParser.ParseModel(model);

            Assert.IsTrue(project.Name == "Project Number");
            Assert.IsTrue(project.Site.Name == "Default");
            Assert.IsTrue(project.Site.ReferencePoint.Altitude == 0);
            Assert.IsTrue(project.Site.ReferencePoint.Longitude== -0.12623620027777779);
            Assert.IsTrue(project.Site.ReferencePoint.Latitude== 51.500152587777777);
            Assert.IsTrue(project.Site.Building.GlobalId == "0gkbD86_XBgQrYZ6tJlPrf");
            Assert.IsTrue(project.Site.Building.Properties.Count == 1);
            Assert.IsTrue(project.Site.Building.Properties["NumberOfStoreys"].ToString() == "1");  // q: where is BuildingAdress property?? There should be 2 properties
            Assert.IsTrue(project.Site.Building.BuildingAdress.Town == "Westminster");
            Assert.IsTrue(project.Site.Building.Storeys.Count == 1);
            Assert.IsTrue(project.Site.Building.Storeys[0].Name == "Level 0");
            Assert.IsTrue(project.Site.Building.Storeys[0].Properties.Count == 1);
            Assert.IsTrue(project.Site.Building.Storeys[0].Properties["AboveGround"] == null);

            // todo: fix following values
            // Assert.IsTrue(project.Site.GlobalX == -3603.117269);
            // Assert.IsTrue(project.Site.GlobalY == 305.257796);
            // Assert.IsTrue(project.Site.BoundingBoxWidth == 352.5);
            // Assert.IsTrue(project.Site.BoundingBoxHeight == 3000);
            // Assert.IsTrue(project.Site.BoundingBoxLength == 6000);
            Assert.IsTrue(project.Site.GlobalZ == 0);


        }
        [Test]
        public void GlobalXTest()
        {
            var ifcProject = model.FederatedInstances.OfType<IIfcProject>().FirstOrDefault();
            var site = ifcProject.Sites.FirstOrDefault();
            Assert.IsTrue(site.Name == "Default");
            // q: how to get from IIfcSite for Location: ProjectLocation, Top Elevation, Bottom Elevation, Global Top Elevation, Global Bottom Elevation ?
            // q: how to get from IIfcSite for geometry: Has Own Geometry, GllobalX, GlobalY, GlobalZ. BundingBox (length, width, height) ?
        }
    }
}
