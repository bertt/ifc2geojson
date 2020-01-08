using ifc2geojson.core;
using NUnit.Framework;
using Xbim.Ifc;

namespace ifc2geojson.tests
{
    public class IfcHausTests
    {
        IfcStore model;

        [SetUp]
        public void Setup()
        {
            IfcStore.ModelProviderFactory.UseMemoryModelProvider();
            model = IfcStore.Open("assets/AC20-FZK-Haus.ifc");
            Assert.IsTrue(model.SchemaVersion == Xbim.Common.Step21.XbimSchemaVersion.Ifc4);
        }

        [Test]
        public void ConvertModel()
        {
            var project = IfcParser.ParseModel(model);

            Assert.IsTrue(project.LengthUnitPower == 1);
            Assert.IsTrue(project.Name == "Projekt-FZK-Haus");
            Assert.IsTrue(project.Description == "Projekt FZK-House create by KHH Forschuungszentrum Karlsruhe");
            Assert.IsTrue(project.GlobalId == "0lY6P5Ur90TAQnnnI6wtnb");
            Assert.IsTrue(project.Properties.Count == 4);
            Assert.IsTrue(project.Properties["GS_TimeStamp"].ToString() == "9685146");
            Assert.IsTrue(project.Properties["BuildingPermitId"].ToString() == "1234");
            Assert.IsTrue(project.Properties["ConstructionMode"].ToString() == "Massivhaus");
            Assert.IsTrue(project.Properties["GrossAreaPlanned"].ToString() == "288");
            Assert.IsTrue(project.Site.Name == "Gelaende");
            Assert.IsTrue(project.Site.Description == "Ebenes Gelaende");
            Assert.IsTrue(project.Site.ReferencePoint.Longitude== 8.436539);
            Assert.IsTrue(project.Site.ReferencePoint.Latitude == 49.100435);
            Assert.IsTrue(project.Site.ReferencePoint.Altitude == 110);
            Assert.IsTrue(project.Site.BoundingBoxLength == 18);
            Assert.IsTrue(project.Site.BoundingBoxWidth == 16);
            Assert.IsTrue(project.Site.BoundingBoxHeight == 1);
            Assert.IsTrue(project.Site.HasOwnGeometry == true);

            Assert.IsTrue(project.Site.GlobalId == "0KMpiAlnb52RgQuM1CwVfd");
            Assert.IsTrue(project.Site.Properties.Count == 4);
            Assert.IsTrue(project.Site.Properties["GrossArea"].ToString()=="0");
            Assert.IsTrue(project.Site.Properties["GrossPerimeter"].ToString() == "0");
            Assert.IsTrue(project.Site.Properties["BuildingHeightLimit"].ToString() == "9");
            Assert.IsTrue(project.Site.Properties["GrossAreaPlanned"].ToString() == "0");
            Assert.IsTrue(project.Site.GlobalX == -3);
            Assert.IsTrue(project.Site.GlobalY == -3);
            Assert.IsTrue(project.Site.GlobalZ == -1);
            Assert.IsTrue(project.Site.Building.Name == "FZK-Haus");
            Assert.IsTrue(project.Site.Building.GlobalId == "2hQBAVPOr5VxhS3Jl0O47h");
            Assert.IsTrue(project.Site.Building.HasOwnGeometry == false);

            // todo: get following tests working...
            //Assert.IsTrue(project.Site.Building.GlobalX == -0.5);
            //Assert.IsTrue(project.Site.Building.GlobalY == -0.5);
            //Assert.IsTrue(project.Site.Building.GlobalZ == 0.2);

            Assert.IsTrue(project.Site.Building.Storeys.Count == 2);
            Assert.IsTrue(project.Site.Building.Location == project.Site.ReferencePoint);
            Assert.IsTrue(project.Site.Building.Properties.Count == 10);
            Assert.IsTrue(project.Site.Building.Properties["GrossFloorArea"].ToString() == "241.497955958");
            Assert.IsTrue(project.Site.Building.Properties["BuildingID"].ToString() == "5678");
            Assert.IsTrue(project.Site.Building.Properties["GrossPlannedArea"].ToString() == "120");
            Assert.IsTrue((bool)project.Site.Building.Properties["IsPermanentID"]);
            Assert.IsTrue(project.Site.Building.Properties["NetAreaPlanned"].ToString() == "120");
            Assert.IsTrue(project.Site.Building.Properties["NumberOfStoreys"].ToString() == "2");
            Assert.IsTrue(project.Site.Building.Properties["OccupancyType"].ToString() == "citygml:1000 (residential building)");
            Assert.IsFalse((bool)project.Site.Building.Properties["SprinklerProtection"]);
            Assert.IsFalse((bool)project.Site.Building.Properties["SprinklerProtectionAutomatic"]);
            Assert.IsTrue(project.Site.Building.Properties["YearOfConstruction"].ToString() == "2008");

            var erdStorey = project.Site.Building.Storeys[0];
            Assert.IsTrue(erdStorey.Name == "Erdgeschoss");
            Assert.IsTrue(erdStorey.Elevation == 0);
            Assert.IsTrue(erdStorey.GrossFloorArea== 119.824049906);
            Assert.IsTrue(erdStorey.HasOwnGeometry == false);
            Assert.IsTrue(project.Walls.Count == 13);

            // todo: get following tests working...
            //Assert.IsTrue(erdSurvey.GlobalX == 0);
            //Assert.IsTrue(erdSurvey.GlobalY == 0);
            //Assert.IsTrue(erdSurvey.GlobalZ == -0.2);

            Assert.IsTrue(erdStorey.Spaces.Count == 6);
            Assert.IsTrue(erdStorey.Spaces[0].Name == "4");
            Assert.IsTrue(erdStorey.Spaces[0].LongName == "Schlafzimmer");
            Assert.IsTrue(erdStorey.Spaces[0].Properties.Count > 50 );
            Assert.IsTrue(erdStorey.Spaces[0].Properties["ClearHeight"].ToString() == "2.5");
            Assert.IsTrue(erdStorey.Spaces[0].HasOwnGeometry == true);
            Assert.IsTrue(erdStorey.Spaces[0].GrossFloorArea == 22.0725);
            Assert.IsTrue(erdStorey.Spaces[0].GrossPerimeter == 19);
            Assert.IsTrue(erdStorey.Spaces[0].Height == 2.5);

            // todo: get following tests working...
            //Assert.IsTrue(erdSurvey.Spaces[0].GlobalX == 7.65);
            //Assert.IsTrue(erdSurvey.Spaces[0].GlobalY == 4.25);
            //Assert.IsTrue(erdSurvey.Spaces[0].GlobalZ == 0);

            Assert.IsTrue(erdStorey.Properties.Count == 4);
            Assert.IsTrue(erdStorey.Properties["GrossFloorArea"].ToString() == "119.824049906");
            Assert.IsTrue(erdStorey.Properties["GrossHeight"].ToString() == "2.7");
            Assert.IsTrue(erdStorey.Properties["Height"].ToString() == "2.7");
            Assert.IsTrue(erdStorey.Properties["NetHeight"].ToString() == "2.7");

            Assert.IsTrue(erdStorey.Spaces[0].Location.Longitude == 8.43671310669212 && erdStorey.Spaces[0].Location.Latitude == 49.100522136582555 && erdStorey.Spaces[0].Location.Altitude == 110);
            var schlafZimmer = erdStorey.Spaces[0];
            Assert.IsTrue(schlafZimmer.Polygon.Coordinates[0].Coordinates.Count ==5);
        }
    }
}