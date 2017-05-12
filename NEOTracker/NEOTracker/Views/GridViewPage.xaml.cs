using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NEOTracker.Views
{
    public sealed partial class GridViewPage : Page
    {
        public GridViewPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var neos = await Data.Data.GetNEOs();
            telerikGrid.ItemsSource = neos;
        }
    }
}