using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace NEOTracker.Data
{
    public sealed class CneosDataManager
    {
        static float? ConvertDistance(string distance)
        {
            var numericValue = float.Parse(distance.Split(' ')[0]);

            if (distance.Contains("m")) return numericValue;
            return numericValue * 1000;
        }

        public static async Task<IEnumerable<CneosData>> GetCneosDataAsync()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/cneos-data.csv"));
            using (var stream = await file.OpenReadAsync())
            {
                using (var streamReader = new StreamReader(stream.AsStreamForRead()))
                {
                    var csvReader = new CsvReader(streamReader);
                    csvReader.Configuration.HasHeaderRecord = true;
                    csvReader.Configuration.IgnoreHeaderWhiteSpace = true;

                    var cneos = new List<CneosData>();

                    while (csvReader.Read())
                    {
                        var name = csvReader.GetField<string>(0);
                        var closeApproachDateTime = DateTime.Parse(csvReader.GetField<string>(1).Split('±')[0].Trim());
                        var closeApproachNominalDistance = float.Parse(csvReader.GetField<string>(2).Split('|')[1].Trim());
                        var closeApproachMinimumDistance = float.Parse(csvReader.GetField<string>(3).Split('|')[1].Trim());
                        if (csvReader.TryGetField<float>(4, out var velocityRelative)) { }
                        if (csvReader.TryGetField<float>(5, out var velocityInfinity)) { }
                        if (csvReader.TryGetField<float>(6, out var magnitude)) { }
                        var diameterMinimum = csvReader.GetField(7) == "n/a" ? null : ConvertDistance(csvReader.GetField<string>(7).Split('-')[0].Trim());
                        var diameterMaximum = csvReader.GetField(7) == "n/a" ? null : ConvertDistance(csvReader.GetField<string>(7).Split('-')[1].Trim());

                        cneos.Add(new CneosData()
                        {
                            Name = name,
                            CloseApproachDateTime = closeApproachDateTime,
                            CloseApproachNominalDistance = closeApproachNominalDistance,
                            CloseApproachMinimumDistance = closeApproachMinimumDistance,
                            VelocityRelative = velocityRelative,
                            VelocityInfinity = velocityInfinity,
                            Magnitude = magnitude,
                            DiameterMinimum = diameterMinimum,
                            DiameterMaximum = diameterMaximum,
                        });
                    }

                    return cneos;
                }
            }
        }
    }

    public sealed class CneosData
    {
        public string Name { get; set; }
        public DateTime CloseApproachDateTime { get; set; }
        public float CloseApproachNominalDistance { get; set; }
        public float CloseApproachMinimumDistance { get; set; }
        public float? VelocityRelative { get; set; }
        public float? VelocityInfinity { get; set; }
        public float? Magnitude { get; set; }
        public float? DiameterMinimum { get; set; }
        public float? DiameterMaximum { get; set; }
    }
}
