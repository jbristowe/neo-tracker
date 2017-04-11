using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRocks.Data;
using Xunit;
using WebRocks;

namespace WebRocksTests
{
    public class ExtensionTests
    {
        [Fact]
        public void OrbitalDataExtensions_FlattenNearEarthObjects_ReturnsFlattenedList()
        {
            var neo1 = new NearEarthObject { NEOReferenceId = "1001" };
            var neo2 = new NearEarthObject { NEOReferenceId = "1002" };
            var neo3 = new NearEarthObject { NEOReferenceId = "1003" };
            var neo4 = new NearEarthObject { NEOReferenceId = "1004" };
            var page1 = new ApiBrowsePage()
            {
                NearEarthObjects = new NearEarthObject[] { neo1, neo2 }
            };

            var page2 = new ApiBrowsePage()
            {
                NearEarthObjects = new NearEarthObject[] { neo3, neo4 }
            };

            var pages = new[] { page1, page2 };

            var flatList = pages.FlattenNearEarthObjects().ToList();

            Assert.Equal(4, flatList.Count);
            Assert.Contains(neo1, flatList);
            Assert.Contains(neo2, flatList);
            Assert.Contains(neo3, flatList);
            Assert.Contains(neo4, flatList);
        }

        [Fact]
        public void OrbitalDataExtensions_FlattenCloseApproaches_ReturnsFlattenedList()
        {
            var close1 = new CloseApproachDate();
            var close2 = new CloseApproachDate();
            var close3 = new CloseApproachDate();
            var close4 = new CloseApproachDate();
            var neo1 = new NearEarthObject { NEOReferenceId = "1001", CloseApproaches = new CloseApproachDate[] { close1, close2 } };
            var neo2 = new NearEarthObject { NEOReferenceId = "1002", CloseApproaches = new CloseApproachDate[] { close3, close4 } };

            var items = new[] { neo1, neo2 };

            var flatList = items.FlattenCloseApproaches().ToList();

            Assert.Equal(4, flatList.Count);
            Assert.Contains(close1, flatList);
            Assert.Contains(close2, flatList);
            Assert.Contains(close3, flatList);
            Assert.Contains(close4, flatList);
        }

        [Fact]
        public void OrbitalDataExtensions_MeasurementsToArray_ReturnsArrayOfDiameters()
        {
            var estimatedDiameter = new EstimatedDiameter()
            {
                Feet = new DiameterMeasurementFeet() { EstimatedDiameterMin = 1f, EstimatedDiameterMax = 100f },
                Kilometers = new DiameterMeasurementKilometers() { EstimatedDiameterMin = 1f, EstimatedDiameterMax = 100f },
                Meters = new DiameterMeasurementMeters() { EstimatedDiameterMin = 1f, EstimatedDiameterMax = 100f },
                Miles = new DiameterMeasurementMiles() { EstimatedDiameterMin = 1f, EstimatedDiameterMax = 100f },
            };

            var array = estimatedDiameter.MeasurementsToArray();

            Assert.Equal(4, array.Length);
        }
    }
}

