using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace NEOTracker.Data
{
    public sealed class EsaDataManager
    {
        public static async Task<IEnumerable<EsaData>> GetEsaDataAsync()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/esa-list.csv"));
            using (var stream = await file.OpenReadAsync())
            {
                using (var streamReader = new StreamReader(stream.AsStreamForRead()))
                {
                    var csvReader = new CsvReader(streamReader);

                    var esaData = new List<EsaData>();

                    while (csvReader.Read())
                    {
                        var name = csvReader.GetField<string>(0);
                        var size = float.Parse(csvReader.GetField<string>(1).Split('*')[0]);
                        var closeApproachDateTime = csvReader.GetField<DateTime>(2);
                        var impactProbability = double.Parse(csvReader.GetField<string>(3).Split('/')[0]) / double.Parse(csvReader.GetField<string>(3).Split('/')[1]);
                        var palermoScale = csvReader.GetField<float>(4);
                        var torinoScale = csvReader.GetField<string>(5) == "n/a" ? null : csvReader.GetField<float?>(5);
                        var velocity = csvReader.GetField<float>(6);
                        var daysInList = csvReader.GetField<int>(7);

                        esaData.Add(new EsaData()
                        {
                            Name = name,
                            Size = size,
                            CloseApproachDateTime = closeApproachDateTime,
                            IP = impactProbability,
                            PS = palermoScale,
                            TS = torinoScale,
                            Velocity = velocity,
                            DaysInList = daysInList
                        });
                    }

                    return esaData;
                }
            }
        }
    }

    public sealed class EsaData
    {
        public string Name { get; set; }
        public float Size { get; set; }
        public DateTime CloseApproachDateTime { get; set; }
        public double IP { get; set; }
        public float PS { get; set; }
        public float? TS { get; set; }
        public float Velocity { get; set; }
        public int DaysInList { get; set; }
    }
}