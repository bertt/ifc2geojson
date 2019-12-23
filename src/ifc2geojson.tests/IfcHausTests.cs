using ifc2geojson.core;
using NUnit.Framework;
using Wkx;
using Xbim.Ifc;

namespace ifc2geojson.tests
{
    public class IfcHausTests
    {
        IfcStore model;

        [SetUp]
        public void Setup()
        {
            model = IfcStore.Open("AC20-FZK-Haus.ifc");
        }

        [Test]
        public void ConvertModel()
        {
            var project = IfcParser.ParseModel(model);
            Assert.IsTrue(project.LengthUnitPower == 1);
            Assert.IsTrue(project.Name == "Projekt-FZK-Haus");
            Assert.IsTrue(project.Description == "Projekt FZK-House create by KHH Forschuungszentrum Karlsruhe");
            Assert.IsTrue(project.Site.Name == "Gelaende");
            Assert.IsTrue(project.Site.Description == "Ebenes Gelaende");
            Assert.IsTrue(project.Site.ReferencePoint.X == 8.436539);
            Assert.IsTrue(project.Site.ReferencePoint.Y == 49.100435);
            Assert.IsTrue(project.Site.ReferencePoint.Z == 110);
            Assert.IsTrue(project.Site.Building.Name == "FZK-Haus");
            Assert.IsTrue(project.Site.Building.Storeys.Count == 2);

            var erdSurvey = project.Site.Building.Storeys[0];
            var dachSurvey = project.Site.Building.Storeys[1];
            Assert.IsTrue(erdSurvey.Name == "Erdgeschoss");
            Assert.IsTrue(erdSurvey.Elevation == 0);
            Assert.IsTrue(erdSurvey.GrossFloorArea== 119.824049906);
            Assert.IsTrue(erdSurvey.Spaces.Count == 6);
            Assert.IsTrue(erdSurvey.Spaces[0].Name == "4");
            Assert.IsTrue(erdSurvey.Spaces[0].LongName == "Schlafzimmer");

            Assert.IsTrue(dachSurvey.Name == "Dachgeschoss");
            Assert.IsTrue(dachSurvey.Elevation == 2.7);
            Assert.IsTrue(dachSurvey.GrossFloorArea == 121.673906052);
            Assert.IsTrue(dachSurvey.Spaces.Count == 1);
            Assert.IsTrue(dachSurvey.Spaces[0].Name == "7");
            Assert.IsTrue(dachSurvey.Spaces[0].LongName == "Galerie");

            var schlafZimmer = erdSurvey.Spaces[0];
            var geometry = (Polygon)schlafZimmer.Geometry;
            // todo: is this correct? 24 points? 
            Assert.IsTrue(geometry.ExteriorRing.Points.Count == 24);
            // todo: check coordinates
        }
    }
}