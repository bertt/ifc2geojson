using ifc2geojson.core;
using NUnit.Framework;
using System.Linq;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.Ifc2x3.Kernel;
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
            Assert.IsTrue(project.FriendlyName == "Project Number");
            Assert.IsTrue(project.Walls.Count == 1);
            Assert.IsTrue(project.Walls[0].GlobalId == "3uPlCH7wP2fu4If6Q86Sno");
            Assert.IsTrue(project.Walls[0].Name == "Basic Wall:Wall-Ext_102Bwk-75Ins-100LBlk-12P:285323");
            Assert.IsTrue(project.Walls[0].Properties["Reference"].ToString() == "Wall-Ext_102Bwk-75Ins-100LBlk-12P");
            Assert.IsTrue((bool)project.Walls[0].Properties["IsExternal"]);
            Assert.IsTrue(!(bool)project.Walls[0].Properties["LoadBearing"]);
            Assert.IsTrue(project.Walls[0].ObjectType=="Basic Wall:Wall-Ext_102Bwk-75Ins-100LBlk-12P:45419");
            Assert.IsTrue(project.Walls[0].Properties.Count == 5);
            Assert.IsTrue(project.Windows.Count == 2);
            Assert.IsTrue(project.Windows[0].Properties.Count == 4);
            Assert.IsTrue(project.Windows[0].Properties["Manufacturer"].ToString() == "Revit");
            Assert.IsTrue((bool)project.Windows[0].Properties["IsExternal"]);
            Assert.IsTrue(project.Windows[0].Properties["Reference"].ToString()== "910x910mm");
            Assert.IsTrue(project.Windows[0].Properties["ThermalTransmittance"].ToString() == "5.5617");
            Assert.IsTrue(project.Windows[0].Name == "Windows_Sgl_Plain:910x910mm:285546");




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
            var walls = model.FederatedInstances.OfType<IIfcWall>().ToList();
            Assert.IsTrue(walls.Count == 1);
            var wall = walls.FirstOrDefault();
            Assert.IsTrue(wall.Name == "Basic Wall:Wall-Ext_102Bwk-75Ins-100LBlk-12P:285323");
            Assert.IsTrue(wall.GlobalId == "3uPlCH7wP2fu4If6Q86Sno");

            var windows = model.FederatedInstances.OfType<IIfcWindow>().ToList();
            Assert.IsTrue(windows.Count == 2);

            var ifcProject = model.FederatedInstances.OfType<IfcProject>().FirstOrDefault();
            var site = ifcProject.Sites.FirstOrDefault();
            Assert.IsTrue(site.Name == "Default");
            Assert.IsTrue(ifcProject.FriendlyName == "Project Number");
            // q: how to get from IIfcSite for Location: ProjectLocation, Top Elevation, Bottom Elevation, Global Top Elevation, Global Bottom Elevation ?
            // q: how to get from IIfcSite for geometry: Has Own Geometry, GllobalX, GlobalY, GlobalZ. BundingBox (length, width, height) ?
        }
    }
}
