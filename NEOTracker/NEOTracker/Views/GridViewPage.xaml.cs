using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NEOTracker.Views
{
    public sealed partial class GridViewPage : Page
    {
        public GridViewPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var config = new WebRocks.WebRocksConfiguration(apiKey: Keys.NASAAPIKEY);
            var provider = new WebRocks.Requests.HttpClientNeoRequestProvider();
            var client = new WebRocks.WebRocksClient(config, provider);
            var results = await client.GetFeedPageAsync(DateTime.Now);
            var neos = results.NearEarthObjects.Select(kv => kv.Value).SelectMany(n => n).Where(n => n.CloseApproaches.Count() > 0).OrderBy(n => n.CloseApproaches[0].CloseApproachDateTime);

            telerikGrid.ItemsSource = neos;
        }
    }
}