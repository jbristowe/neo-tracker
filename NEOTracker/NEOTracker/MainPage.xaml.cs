using NEOTracker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NEOTracker
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            List<View> views = new List<View>();
            views.Add(new View() { Title = "Details", Icon = "ms-appx:///Assets/icons/noun_966844_cc_modified.png", PageType = typeof(MasterDetailViewPage) });
            views.Add(new View() { Title = "Space View", Icon = "ms-appx:///Assets/icons/noun_923839_cc_modified.png", PageType = typeof(SpaceViewPage) });
            views.Add(new View() { Title = "Grid View", Icon = "ms-appx:///Assets/icons/noun_923839_cc_modified.png", PageType = typeof(GridViewPage) });
            views.Add(new View() { Title = "Chart View", Icon = "ms-appx:///Assets/icons/noun_923839_cc_modified.png", PageType = typeof(ChartViewPage) });
            views.Add(new View() { Title = "ESA Risk List", Icon = "ms-appx:///Assets/icons/noun_923839_cc_modified.png", PageType = typeof(EsaRiskListViewPage) });
            views.Add(new View() { Title = "Torino Scale", Icon = "ms-appx:///Assets/icons/noun_923839_cc_modified.png", PageType = typeof(TorinoImpactHazardScaleViewPage) });
            Menu.ItemsSource = views;

            Menu.SelectedIndex = 0;
            MainFrame.Navigate(views.First().PageType);
        }

        private void Menu_ItemClick(object sender, ItemClickEventArgs e)
        {
            var view = e.ClickedItem as View;
            MainFrame.Navigate(view.PageType);
            MainFrame.BackStack.Clear();
        }

        private void Menu_OptionsItemClick(object sender, ItemClickEventArgs e)
        {
            MainFrame.Navigate(typeof(About));
            MainFrame.BackStack.Clear();
        }
    }

    public class View
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type PageType { get; set; }
    }
}
