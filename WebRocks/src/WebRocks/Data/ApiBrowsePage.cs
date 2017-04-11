using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebRocks.Data
{
    [DataContract]
    public class ApiBrowsePage
    {
        [DataMember(Name = "links")]
        public Links Links { get; set; }

        [DataMember(Name = "page")]
        public Page Page { get; set; }

        [DataMember(Name = "near_earth_objects")]
        public NearEarthObject[] NearEarthObjects { get; set; }
    }

    [DataContract]
    public class Links
    {
        [DataMember(Name = "next")]
        public string Next { get; set; }

        [DataMember(Name = "prev")]
        public string Prev { get; set; }

        [DataMember(Name = "self")]
        public string Self { get; set; }
    }

    [DataContract]
    public class Page
    {
        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "total_elements")]
        public int TotalElements { get; set; }

        [DataMember(Name = "total_pages")]
        public int TotalPages { get; set; }

        [DataMember(Name = "number")]
        public int Number { get; set; }
    }
}
