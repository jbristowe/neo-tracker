using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebRocks.Data
{
    [DataContract]
    public class CloseApproachDate
    {
        [DataMember(Name = "close_approach_date")]
        public string CloseApproachDateString { get; set; }

        public DateTime CloseApproachDateTime
        {
            get
            {
                return DateTime.ParseExact(CloseApproachDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
        }

        [DataMember(Name = "epoch_date_close_approach")]
        public long EpochDateCloseApproachDate { get; set; }

        [DataMember(Name = "relative_velocity")]
        public RelativeVelocity RelativeVelocity { get; set; }

        [DataMember(Name = "miss_distance")]
        public MissDistance MissDistance { get; set; }

        [DataMember(Name = "orbiting_body")]
        public string OrbitingBody { get; set; }
    }
}
