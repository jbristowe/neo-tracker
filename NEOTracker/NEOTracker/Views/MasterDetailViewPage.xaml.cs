using Microsoft.Toolkit.Uwp.Services.Bing;
using Microsoft.Toolkit.Uwp.UI.Controls;
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
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.Toolkit.Uwp.Services.Facebook;
using Microsoft.Toolkit.Uwp.Services.Twitter;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NEOTracker.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MasterDetailViewPage : Page
    {
        ObservableCollection<WebRocks.Data.NearEarthObject> Items = new ObservableCollection<WebRocks.Data.NearEarthObject>();
        static Dictionary<string, ObservableCollection<BingResult>> NewsItems = new Dictionary<string, ObservableCollection<BingResult>>();

        public MasterDetailViewPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataTransferManager.GetForCurrentView().DataRequested += DataTransferManager_DataRequested;

            var config = new WebRocks.WebRocksConfiguration(apiKey: Keys.NASAAPIKEY);
            var provider = new WebRocks.Requests.HttpClientNeoRequestProvider();
            var client = new WebRocks.WebRocksClient(config, provider);

            var results = (await client.GetFeedPageAsync(DateTime.Now));

            var group = from neo in (from kv in (from kv in results.NearEarthObjects
                                                 orderby DateTime.Parse(kv.Key)
                                                 select kv)
                                     from neo in kv.Value
                                     select neo)
                        group neo by DateTime.Parse(neo.CloseApproaches.First().CloseApproachDateString);
                        
                        

            //foreach (var result in results.NearEarthObjects) foreach (var item in result.Value) Items.Add(item);
            
          //  var groups = from item in Items group item by item.CloseApproaches.First().CloseApproachDateString;
            cvs.Source = group;
            //MasterDetails.SelectedItem = Items.First();
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var item = (MasterDetails.SelectedItem as WebRocks.Data.NearEarthObject);
            args.Request.Data.SetWebLink(new Uri(item.NasaJPLUrl));

            args.Request.Data.Properties.Title = item.Name;
            args.Request.Data.Properties.Description = $"Share details about {item.Name}";

        }

        private static async Task LoadData(string query, ObservableCollection<BingResult> results)
        {
            var searchConfig = new BingSearchConfig
            {
                Query = query,
                QueryType = BingQueryType.Search
            };

            var newsItems = await BingService.Instance.RequestAsync(searchConfig, 10);

            foreach (var result in newsItems) results.Add(result);
            NewsItems.Add(query, results);
        }

        private static Visibility CloseApproachesToVisibility(WebRocks.Data.CloseApproachDate[] data)
        {
            return data != null && data.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public static IEnumerable<BingResult> GetNewsItems(string query)
        {
            if (NewsItems.TryGetValue(query, out var results))
            {
                return results;
            }

            results = new ObservableCollection<BingResult>();
            LoadData(query, results);

            return results;
        }

        private void ShareClicked(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private async void TwitterClicked(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as WebRocks.Data.NearEarthObject;

            TwitterService.Instance.Initialize(Keys.TwitterConsumerKey, Keys.TwitterSecretKey, Keys.TwitterCallbackUri);
            if (!await TwitterService.Instance.LoginAsync()) { return; }
            await TwitterService.Instance.TweetStatusAsync($"Check out this Near Earth Object #dontpanic #TheWorldIsAboutToEnd {item.NasaJPLUrl}");
        }

        private async void FacebookClicked(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as WebRocks.Data.NearEarthObject;

            FacebookService.Instance.Initialize(Keys.FBAppId);
            if (!await FacebookService.Instance.LoginAsync()) { return; }
            await FacebookService.Instance.PostToFeedWithDialogAsync(item.Name, "Shared with NEO Tracker for UWP", item.NasaJPLUrl);

        }
    }
}
