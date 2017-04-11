using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRocks.Data;

namespace WebRocks
{
    public static class OrbitalDataExtensions
    {
        public static IEnumerable<NearEarthObject> FlattenNearEarthObjects(this IEnumerable<ApiBrowsePage> pages)
        {
            return pages.SelectMany(p => p.NearEarthObjects);
        }

        public static IEnumerable<CloseApproachDate> FlattenCloseApproaches(this IEnumerable<NearEarthObject> neos)
        {
            return neos.SelectMany(p => p.CloseApproaches);
        }

        public static DiameterMeasurement[] MeasurementsToArray(this EstimatedDiameter estimatedDiameter)
        {
            return new DiameterMeasurement[] {
                estimatedDiameter.Feet,
                estimatedDiameter.Kilometers,
                estimatedDiameter.Miles,
                estimatedDiameter.Meters
            };
        }
    }
}
