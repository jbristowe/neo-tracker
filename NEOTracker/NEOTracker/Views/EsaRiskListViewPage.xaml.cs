using Microsoft.Toolkit.Uwp.UI;
using NEOTracker.Data;
using System.Collections.Generic;
using System.Linq;
using Telerik.UI.Xaml.Controls.Chart;
using Telerik.UI.Xaml.Controls.Grid;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace NEOTracker.Views
{
    public sealed partial class EsaRiskListViewPage : Page
    {
        public EsaRiskListViewPage()
        {
            InitializeComponent();
            grid.SelectionChanged += GridSelectionChanged; 
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void GridSelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
        {
            var esaData = (IEnumerable<EsaData>) DataContext;

            foreach (EsaData item in e.AddedItems)
            {
                var index = esaData.ToList<EsaData>().IndexOf(item);
                var pointSeries = chart.FindDescendant<PointSeries>();

                FindVisualChildren<Ellipse>(chart).ToList<Ellipse>()[index].Fill = new SolidColorBrush(Colors.Red);
            }

            foreach (EsaData item in e.RemovedItems)
            {
                var index = esaData.ToList<EsaData>().IndexOf(item);
                var pointSeries = chart.FindDescendant<PointSeries>();

                FindVisualChildren<Ellipse>(chart).ToList<Ellipse>()[index].Fill = new SolidColorBrush(Color.FromArgb(178, 30, 152, 228));
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var esaData = await EsaDataManager.GetEsaDataAsync();
            DataContext = esaData;
        }
    }
}