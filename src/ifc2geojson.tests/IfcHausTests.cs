using GeoJSON.Net.Geometry;
using ifc2geojson.core;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            Assert.IsTrue(project.Site.ReferencePoint.Longitude== 8.436539);
            Assert.IsTrue(project.Site.ReferencePoint.Latitude == 49.100435);
            Assert.IsTrue(project.Site.ReferencePoint.Altitude == 110);
            Assert.IsTrue(project.Site.Building.Name == "FZK-Haus");
            Assert.IsTrue(project.Site.Building.Storeys.Count == 2);
            Assert.IsTrue(project.Site.Building.Location == project.Site.ReferencePoint);
            var erdSurvey = project.Site.Building.Storeys[0];
            Assert.IsTrue(erdSurvey.Name == "Erdgeschoss");
            Assert.IsTrue(erdSurvey.Elevation == 0);
            Assert.IsTrue(erdSurvey.GrossFloorArea== 119.824049906);
            Assert.IsTrue(erdSurvey.Spaces.Count == 6);
            Assert.IsTrue(erdSurvey.Spaces[0].Name == "4");
            Assert.IsTrue(erdSurvey.Spaces[0].LongName == "Schlafzimmer");
            Assert.IsTrue(erdSurvey.Spaces[0].Location.Longitude == 8.43671310669212 && erdSurvey.Spaces[0].Location.Latitude == 49.100522136582555 && erdSurvey.Spaces[0].Location.Altitude == 110);

            var schlafZimmer = erdSurvey.Spaces[0];
            Assert.IsTrue(schlafZimmer.Polygon.Coordinates[0].Coordinates.Count ==5);
        }
    }
}