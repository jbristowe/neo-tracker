using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace NEOTracker
{
    [TemplatePart(Name = "Canvas", Type = typeof(Canvas))]
    public sealed class NEOVisualizer : Control
    {
        private Canvas _canvas;

        public NEOVisualizer()
        {
            this.DefaultStyleKey = typeof(NEOVisualizer);

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var items = new List<NEO>();
                items.Add(new NEO() { Distance = 0.1, DiameterHeight = 0.5, DiameterWidth = 0.5, Label = "test" });
                items.Add(new NEO() { Distance = 0.1, DiameterHeight = 0.5, DiameterWidth = 0.5, Label = "test" });
                items.Add(new NEO() { Distance = 0.1, DiameterHeight = 0.5, DiameterWidth = 0.5, Label = "test" });
                ItemsSource = items;
            }
        }

        protected override void OnApplyTemplate()
        {
            if (_canvas != null)
            {
                _canvas.SizeChanged -= _canvas_SizeChanged;
            }

            _canvas = (Canvas)GetTemplateChild("Canvas");

            if (_canvas == null)
            {
                return;
            }

            _canvas.SizeChanged += _canvas_SizeChanged;

            base.OnApplyTemplate();
        }

        private void _canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateNEOs();
        }

        public IEnumerable<NEO> ItemsSource
        {
            get { return (IEnumerable<NEO>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<NEO>), typeof(NEOVisualizer), new PropertyMetadata(null, OnItemsChanged));

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            if (e.NewValue == e.OldValue) return;

            var control = d as NEOVisualizer;
            if (control == null) return;

            var items = e.NewValue as IEnumerable<NEO>;
            if (items == null) return;

            control.UpdateNEOs();
        }

        private void UpdateNEOs()
        {
            if (_canvas == null) return;

            var controlWidth = _canvas.ActualWidth;
            var controlHeight = _canvas.ActualHeight;

            double centerLeft(FrameworkElement element, double x) => (controlWidth / 2) + x - element.ActualWidth / 2;
            double centerTop(FrameworkElement element, double y) => (controlHeight / 2) - y - element.ActualHeight / 2;


            _canvas.Children.Clear();

            var earth = new Ellipse()
            {
                Height = 100,
                Width = 100,
                Fill = new SolidColorBrush(Colors.LightBlue)
            };

            Canvas.SetTop(earth, centerTop(earth, 0));
            Canvas.SetLeft(earth, centerLeft(earth, 0));
            _canvas.Children.Add(earth);

            if (ItemsSource == null || ItemsSource.Count() <= 0) return;

            var count = ItemsSource.Count();
            var angle = 2 * Math.PI / count;

            var minDiameter = 10;
            var maxDiameter = 40;

            var minDistance = 150;
            var maxDistance = (Math.Min(controlWidth, controlHeight) - maxDiameter) / 2;

            var random = new Random();

            for (var i = 0; i < count; i++)
            {
                var item = ItemsSource.ElementAt(i);

                //var image = new Image()
                //{
                //    Source = new BitmapImage(new Uri("ms-appx:///Assets/icons/noun_705445_cc_modified.png")),
                //    Width = (item.DiameterWidth) * (maxDiameter - minDiameter) + minDiameter,
                //    Height = (item.DiameterHeight) * (maxDiameter - minDiameter) + minDiameter,
                //    Stretch = Stretch.Fill
                //};

                //image.RenderTransform = new RotateTransform()
                //{
                //    CenterX = image.Width / 2,
                //    CenterY = image.Height / 2,
                //    Angle = 45 * random.Next(0, 8)
                //};

                var neoEllipse = new Ellipse()
                {
                    Width = (item.DiameterWidth) * (maxDiameter - minDiameter) + minDiameter,
                    Height = (item.DiameterHeight) * (maxDiameter - minDiameter) + minDiameter,
                    Fill = new SolidColorBrush(Colors.LightGray),
                };

                var control = new ContentControl();
                control.Content = neoEllipse;
                control.DataContext = item;
                control.IsTabStop = true;
                control.UseSystemFocusVisuals = true;

                control.SetValue(AutomationProperties.NameProperty, item.Label);

                var distance = (item.Distance) * (maxDistance - minDistance) + minDistance; 

                var x = distance * Math.Cos(angle * i);
                var y = distance * Math.Sin(angle * i);

                Canvas.SetTop(control, centerTop(control, y));
                Canvas.SetLeft(control, centerLeft(control, x));
                _canvas.Children.Add(control);
            }
        }

    }
}
