using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace NEOTracker.Data
{
    public sealed class EsaDataClassMap : CsvClassMap<EsaData>
    {
        public EsaDataClassMap()
        {
            Map(m => m.Name).Index(0);
            Map(m => m.Size).Index(1);
            Map(m => m.CloseApproachDateTime).Index(2);
            Map(m => m.IP).Index(3);
            Map(m => m.PS).Index(4);
            Map(m => m.TS).Index(5);
            Map(m => m.Velocity).Index(6);
            Map(m => m.DaysInList).Index(7);
        }
    }

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
                    csvReader.Configuration.RegisterClassMap<EsaDataClassMap>();
                    return csvReader.GetRecords<EsaData>().ToList();
                }
            }
        }
    }

    public sealed class EsaData
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public DateTime CloseApproachDateTime { get; set; }
        public string IP { get; set; }
        public float PS { get; set; }
        public string TS { get; set; }
        public float Velocity { get; set; }
        public int DaysInList { get; set; }
    }
}