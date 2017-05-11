using System;
using Telerik.UI.Xaml.Controls.Chart;
using Windows.UI.Xaml.Data;

namespace NEOTracker.Converters
{
    class DataToPointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var chart = (RadCartesianChart) value;
            var data = new Tuple<object, object>(((string) parameter).Split(',')[0], ((string) parameter).Split(',')[1]);
            return chart.ConvertDataToPoint(data);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
