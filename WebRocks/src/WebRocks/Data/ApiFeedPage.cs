using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebRocks.Data
{
    [DataContract]
    public class ApiFeedPage
    {
        [DataMember(Name = "links")]
        public Links Links { get; set; }

        [DataMember(Name = "element_count")]
        public int ElementCount { get; set; }

        [DataMember(Name = "near_earth_objects")]
        public Dictionary<string, NearEarthObject[]> NearEarthObjects { get; set; }
    }
}
