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

            var config = new WebRocks.WebRocksConfiguration();
            var provider = new WebRocks.Requests.HttpClientNeoRequestProvider();
            var client = new WebRocks.WebRocksClient(config, provider);

            var results = await client.GetBrowsePageAsync(1, 20);
            var neos = results.NearEarthObjects.Where(n => n.CloseApproaches.Count() > 0);

            telerikGrid.ItemsSource = neos;
        }
    }
}
