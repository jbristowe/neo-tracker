using Telerik.UI.Xaml.Controls.Grid;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NEOTracker.Controls
{
    public class DeviationStyleSelector : StyleSelector
    {
        public Style NegativeStyle { get; set; }
        public Style PositiveStyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var cellInfo = item as DataGridCellInfo;
            var neo = cellInfo.Item as WebRocks.Data.NearEarthObject;

            return neo.IsPotentiallyHazardousAsteroid ? NegativeStyle : PositiveStyle;
        }
    }
}