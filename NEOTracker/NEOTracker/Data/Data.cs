using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRocks.Data;

namespace NEOTracker.Data
{
    public class Data
    {
        private static ApiFeedPage results;

        private static async Task LoadData()
        {
            if (results == null)
            {
                var config = new WebRocks.WebRocksConfiguration(apiKey: Keys.NASAAPIKEY);
                var provider = new WebRocks.Requests.HttpClientNeoRequestProvider();
                var client = new WebRocks.WebRocksClient(config, provider);
                results = await client.GetFeedPageAsync(DateTime.Now);
            }
        }

        public static async Task<IEnumerable<NearEarthObject>> GetNEOs()
        {
            await LoadData();
            return results.NearEarthObjects.Select(kv => kv.Value).SelectMany(n => n).Where(n => n.CloseApproaches.Count() > 0);
        }

        public static async Task<IEnumerable<IGrouping<DateTime, NearEarthObject>>> GetGroupedNEOs()
        {
            await LoadData();
            return from neo in (from kv in (from kv in results.NearEarthObjects
                                            orderby DateTime.Parse(kv.Key)
                                            select kv)
                                from neo in kv.Value
                                select neo)
                   group neo by DateTime.Parse(neo.CloseApproaches.First().CloseApproachDateString);
        }
    }
}
