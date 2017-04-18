using NEOTracker.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NEOTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
    }

    public class View
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type PageType { get; set; }
    }
}
