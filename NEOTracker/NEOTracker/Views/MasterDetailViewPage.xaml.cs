using Microsoft.Toolkit.Uwp.Services.Bing;
using Microsoft.Toolkit.Uwp.Services.Facebook;
using Microsoft.Toolkit.Uwp.Services.Twitter;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace NEOTracker.Views
{
    public sealed partial class MasterDetailViewPage : Page
    {
        public MasterDetailViewPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            cvs.Source = await Data.Data.GetGroupedNEOs();
        }

        public static IEnumerable<BingResult> GetNewsItems(string query)
        {
            var searchConfig = new BingSearchConfig
            {
                Query = query,
                QueryType = BingQueryType.Search
            };

            return BingService.GetAsIncrementalLoading(searchConfig, 50);
        }

        private async void TwitterClicked(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as WebRocks.Data.NearEarthObject;

            TwitterService.Instance.Initialize(Keys.TwitterConsumerKey, Keys.TwitterSecretKey, Keys.TwitterCallbackUri);
            if (!await TwitterService.Instance.LoginAsync()) { return; }
            await TwitterService.Instance.TweetStatusAsync($"Check out {item.Name} #dontpanic #TheWorldIsAboutToEnd {item.NasaJPLUrl}");
        }

        private async void FacebookClicked(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as WebRocks.Data.NearEarthObject;

            FacebookService.Instance.Initialize(Keys.FBAppId);
            if (!await FacebookService.Instance.LoginAsync()) { return; }
            await FacebookService.Instance.PostToFeedWithDialogAsync(item.Name, $"Check out {item.Name} #dontpanic #TheWorldIsAboutToEnd", item.NasaJPLUrl);

        }

        private async void MasterDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems.First() is WebRocks.Data.NearEarthObject item)
            {
                await Danger.Scale(0, 1, duration: 300).StartAsync();

                if (item.IsPotentiallyHazardousAsteroid)
                {
                    Danger.Fill = new SolidColorBrush(Colors.Red);
                    Danger.Fade(1, duration: 200).Scale(1, 1, duration: 200).Then()
                          .Fade(0, duration: 200).Then()
                          .Fade(1, duration: 200).Then()
                          .Fade(0, duration: 200).Then()
                          .Fade(1, duration: 500)
                          .Start();
                }
                else
                {
                    Danger.Fill = new SolidColorBrush(Colors.Green);
                    Danger.Fade(1, duration: 300).Scale(1, 1, duration: 300).Start();
                }
            }
        }
    }
}
