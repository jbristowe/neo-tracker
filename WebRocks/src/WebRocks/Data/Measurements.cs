using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebRocks.Data
{
    [DataContract]
    public class EstimatedDiameter
    {
        [DataMember(Name = "kilometers")]
        public DiameterMeasurementKilometers Kilometers { get; set; }

        [DataMember(Name = "meters")]
        public DiameterMeasurementMeters Meters { get; set; }

        [DataMember(Name = "miles")]
        public DiameterMeasurementMiles Miles { get; set; }

        [DataMember(Name = "feet")]
        public DiameterMeasurementFeet Feet { get; set; }
    }

    [DataContract]
    public class RelativeVelocity
    {
        [DataMember(Name = "kilometers_per_second")]
        public float KilometersPerSecond { get; set; }

        [DataMember(Name = "kilometers_per_hour")]
        public float KilometersPerHour { get; set; }

        [DataMember(Name = "miles_per_hour")]
        public float MilesPerHour { get; set; }
    }

    [DataContract]
    public class MissDistance
    {
        [DataMember(Name = "astronomical")]
        public float Astronomical { get; set; }

        [DataMember(Name = "lunar")]
        public float Lunar { get; set; }

        [DataMember(Name = "kilometers")]
        public float Kilometers { get; set; }

        [DataMember(Name = "miles")]
        public float Miles { get; set; }
    }

    [DataContract]
    public abstract class DiameterMeasurement
    {
        public abstract MeasurementUnit Units { get; }

        [DataMember(Name = "estimated_diameter_min")]
        public float EstimatedDiameterMin { get; set; }

        [DataMember(Name = "estimated_diameter_max")]
        public float EstimatedDiameterMax { get; set; }
    }

    public enum MeasurementUnit
    {
        Kilometers,
        Meters,
        Miles,
        Feet
    }

    public class DiameterMeasurementKilometers : DiameterMeasurement
    {
        public override MeasurementUnit Units { get; } = MeasurementUnit.Kilometers;
    }

    public class DiameterMeasurementMeters : DiameterMeasurement
    {
        public override MeasurementUnit Units { get; } = MeasurementUnit.Meters;
    }

    public class DiameterMeasurementMiles : DiameterMeasurement
    {
        public override MeasurementUnit Units { get; } = MeasurementUnit.Miles;
    }

    public class DiameterMeasurementFeet : DiameterMeasurement
    {
        public override MeasurementUnit Units { get; } = MeasurementUnit.Feet;
    }


}
