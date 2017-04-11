using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebRocks.Data
{
    [DataContract]
    public class OrbitalData
    {
        [DataMember(Name = "orbit_id")]
        public int OrbitId { get; set; }

        [DataMember(Name = "orbit_determination_date")]
        public string OrbitDeterminationDateString { get; set; }

        public DateTime OrbitDeterminationDateTime
        {
            get
            {
                return DateTime.ParseExact(OrbitDeterminationDateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
        }

        [DataMember(Name = "orbit_uncertainty")]
        public int OrbitUncertainty { get; set; }

        [DataMember(Name = "minimum_orbit_intersection")]
        public float MinimumOrbitIntersection { get; set; }

        [DataMember(Name = "jupiter_tisserand_invariant")]
        public float JupiterTisserandInvariant { get; set; }

        [DataMember(Name = "epoch_osculation")]
        public float EpochOsculation { get; set; }

        [DataMember(Name = "eccentricity")]
        public float Eccentricity { get; set; }

        [DataMember(Name = "semi_major_axis")]
        public float SemiMajorAxis { get; set; }

        [DataMember(Name = "inclination")]
        public float Inclination { get; set; }

        [DataMember(Name = "ascending_node_longitude")]
        public float AscendingNodeLongitude { get; set; }

        [DataMember(Name = "orbital_period")]
        public float OrbitalPeriod { get; set; }

        [DataMember(Name = "perihelion_distance")]
        public float PerihelionDistance { get; set; }

        [DataMember(Name = "perihelion_argument")]
        public float PerihelionArgument { get; set; }

        [DataMember(Name = "aphelion_distance")]
        public float AphelionDistance { get; set; }

        [DataMember(Name = "perihelion_time")]
        public float PerihelionTime { get; set; }

        [DataMember(Name = "mean_anomaly")]
        public float MeanAnomaly { get; set; }

        [DataMember(Name = "mean_motion")]
        public float MeanMotion { get; set; }

        [DataMember(Name = "equinox")]
        public string Equinox { get; set; }

    }
}
