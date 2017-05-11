using NEOTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.UI.Xaml.Controls.Chart;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NEOTracker.Views
{
    public sealed partial class ChartViewPage : Page
    {
        public ChartViewPage()
        {
            this.InitializeComponent();

            Loaded += async (s, e) =>
            {
                var neos = await Data.Data.GetNEOs();
                var random = new Random();

                var neoData = new List<NeoDataPoint>();
                foreach (var neo in neos)
                {
                    neoData.Add(new NeoDataPoint()
                    {
                        CloseApproachDateTime = neo.CloseApproaches.First().CloseApproachDateTime,
                        EstimatedDiameter = neo.EstimatedDiameter.Meters.EstimatedDiameterMax,
                        MissDistance = neo.CloseApproaches.First().MissDistance.Kilometers,
                        Angle = random.Next(0, 360)
                    });
                }

                chart.DataContext = neoData;
            };
        }
    }

    public class NeoDataPoint
    {
        public DateTime CloseApproachDateTime { get; set; }
        public float? EstimatedDiameter { get; set; }
        public float MissDistance { get; set; }
        public int Angle { get; set;  }
    }
}
