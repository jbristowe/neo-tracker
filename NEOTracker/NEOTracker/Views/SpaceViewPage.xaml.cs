using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class SpaceViewPage : Page
    {
        private ObservableCollection<NEO> Items = new ObservableCollection<NEO>();

        public SpaceViewPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            var neos = await Data.Data.GetNEOs();

            var minDistance = neos.Min(neo => neo.CloseApproaches.First().MissDistance.Kilometers);
            var maxDistance = neos.Max(neo => neo.CloseApproaches.First().MissDistance.Kilometers);

            var minDiameter = neos.Min(neo => neo.EstimatedDiameter.Kilometers.EstimatedDiameterMin);
            var maxDiameter = neos.Max(neo => neo.EstimatedDiameter.Kilometers.EstimatedDiameterMax);

            foreach (var item in neos)
            {
                var neo = new NEO()
                {
                    Distance = (item.CloseApproaches.First().MissDistance.Kilometers - minDistance) / (maxDistance - minDistance),
                    Diameter = (item.EstimatedDiameter.Kilometers.EstimatedDiameterMin - minDiameter) / (maxDiameter - minDiameter),
                    Label = item.Name,
                    Item = item
                };
                Items.Add(neo);
            }

            Visualizer.ItemsSource = Items;
        }

        public static SolidColorBrush DangerZoneColor(WebRocks.Data.NearEarthObject item)
        {
            if (item.IsPotentiallyHazardousAsteroid)
                return new SolidColorBrush(Colors.Red);
            else
                return new SolidColorBrush(Colors.White);
        }
    }
}
