using NEOTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NEOTracker.Views
{
    public sealed partial class ChartViewPage : Page
    {
        public ChartViewPage()
        {
            this.InitializeComponent();

            Loaded += async (s, e) =>
            {
                //var results = await CneosDataManager.GetCneosDataAsync();
                //var neos = results.Where(n => (n.CloseApproachDateTime - DateTime.Now).TotalDays < 30);
                var neos = await Data.Data.GetNEOs();

                //var config = new WebRocks.WebRocksConfiguration(apiKey: Keys.NASAAPIKEY);
                //var provider = new WebRocks.Requests.HttpClientNeoRequestProvider();
                //var client = new WebRocks.WebRocksClient(config, provider);
                //var results = await client.GetFeedPageAsync(DateTime.Now);
                //var neos = results.NearEarthObjects.Select(kv => kv.Value).SelectMany(n => n).Where(n => n.CloseApproaches.Count() > 0);

                var neoData = new List<NeoDataPoint>();
                foreach (var neo in neos)
                {
                    neoData.Add(new NeoDataPoint()
                    {
                        CloseApproachDateTime = neo.CloseApproaches.First().CloseApproachDateTime,
                        EstimatedDiameter = neo.EstimatedDiameter.Meters.EstimatedDiameterMax,
                        MissDistance = neo.CloseApproaches.First().MissDistance.Kilometers
                    });
                }

                chart.DataContext = neoData;
            };
        }
    }
    public class NeoDataPoint
    {
        public DateTime CloseApproachDateTime { get; set; }
        public float? EstimatedDiameter { get; set; }
        public float MissDistance { get; set; }
    }
}
