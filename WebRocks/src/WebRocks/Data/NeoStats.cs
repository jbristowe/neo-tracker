using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebRocks.Data
{
    [DataContract]
    public class NeoStats
    {
        [DataMember(Name = "near_earth_object_count")]
        public int NearEarthObjectCount { get; set; }

        [DataMember(Name = "close_approach_count")]
        public int CloseApproachCount { get; set; }

        [DataMember(Name = "last_updated")]
        public DateTime LastUpdated { get; set; }

        [DataMember(Name = "source")]
        public string Source { get; set; }

        [DataMember(Name = "nasa_jpl_url")]
        public string NasaJplUrl { get; set; }
    }
}
