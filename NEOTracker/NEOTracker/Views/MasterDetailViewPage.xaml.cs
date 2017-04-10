using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NEOTracker.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MasterDetailViewPage : Page
    {
        ObservableCollection<WebRocks.Data.NearEarthObject> Items = new ObservableCollection<WebRocks.Data.NearEarthObject>();

        public MasterDetailViewPage()
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

            foreach (var result in results.NearEarthObjects) Items.Add(result);
        }
    }
}
