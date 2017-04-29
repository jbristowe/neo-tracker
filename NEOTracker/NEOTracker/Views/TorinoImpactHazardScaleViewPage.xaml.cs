using NEOTracker.Data;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NEOTracker.Views
{
    public sealed partial class TorinoImpactHazardScaleViewPage : Page
    {
        public TorinoImpactHazardScaleViewPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var esaData = await EsaDataManager.GetEsaDataAsync();
            DataContext = esaData;
        }
    }
}
