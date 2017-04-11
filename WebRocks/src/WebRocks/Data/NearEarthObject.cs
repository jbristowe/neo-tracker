using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebRocks.Data
{
    [DataContract]
    public class NearEarthObject
    {
        [DataMember(Name = "links")]
        public Links Links { get; set; }

        [DataMember(Name = "neo_reference_id")]
        public string NEOReferenceId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "nasa_jpl_url")]
        public string NasaJPLUrl { get; set; }

        [DataMember(Name = "absolute_magnitude_h")]
        public float AbsoluteMagnitudeH { get; set; }

        [DataMember(Name = "estimated_diameter")]
        public EstimatedDiameter EstimatedDiameter { get; set; }

        [DataMember(Name = "is_potentially_hazardous_asteroid")]
        public bool IsPotentiallyHazardousAsteroid { get; set; }

        [DataMember(Name = "close_approach_data")]
        public CloseApproachDate[] CloseApproaches { get; set; }

        [DataMember(Name = "orbital_data")]
        public OrbitalData OrbitalData { get; set; }

    }
}
