using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.UI.Xaml.Controls.Chart;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NEOTracker.Views
{
    public sealed partial class ChartViewPage : Page
    {
        public ChartViewPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var config = new WebRocks.WebRocksConfiguration();
            var provider = new WebRocks.Requests.HttpClientNeoRequestProvider();
            var client = new WebRocks.WebRocksClient(config, provider);

            var results = await client.GetBrowsePageAsync(1, 20);
            var neos = results.NearEarthObjects.Where(n => n.CloseApproaches.Count() > 0);

            var neoData = new List<NeoDataPoint>();
            foreach (var neo in neos)
            {
                neoData.Add(new NeoDataPoint() { CloseApproachDateTime = neo.CloseApproaches.First().CloseApproachDateTime, EstimatedDiameter = neo.EstimatedDiameter.Meters.EstimatedDiameterMax });
            }

            chart.DataContext = neoData;
        }
    }
    public class NeoDataPoint
    {
        public DateTime CloseApproachDateTime { get; set; }
        public float EstimatedDiameter { get; set; }
    }
}
