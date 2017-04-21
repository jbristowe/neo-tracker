using NEOTracker.Data;
using System.Collections.Generic;
using Telerik.UI.Xaml.Controls.Grid;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NEOTracker.Views
{
    public sealed partial class EsaRiskListViewPage : Page
    {
        public EsaRiskListViewPage()
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