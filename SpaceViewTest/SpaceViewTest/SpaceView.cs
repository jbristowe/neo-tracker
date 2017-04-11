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

namespace SpaceViewTest
{
    [TemplatePart(Name = "Canvas", Type = typeof(Canvas))]
    public sealed class SpaceView : ContentControl
    {
        private Canvas _canvas;

        public SpaceView()
        {
            this.DefaultStyleKey = typeof(SpaceView);

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var items = new List<SpaceViewItem>();
                items.Add(new SpaceViewItem() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                items.Add(new SpaceViewItem() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                items.Add(new SpaceViewItem() { Distance = 0.1, Diameter = 0.5, Label = "test" });
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

            CreateItems();
            _canvas.SizeChanged += _canvas_SizeChanged;

            base.OnApplyTemplate();
        }

        private void _canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateItems();
        }

        public IEnumerable<SpaceViewItem> ItemsSource
        {
            get { return (IEnumerable<SpaceViewItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<SpaceViewItem>), typeof(SpaceView), new PropertyMetadata(null, OnItemsChanged));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(SpaceView), new PropertyMetadata(null));

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            if (e.NewValue == e.OldValue) return;

            var control = d as SpaceView;
            if (control == null) return;

            var items = e.NewValue as IEnumerable<SpaceViewItem>;
            if (items == null) return;

            control.CreateItems();
        }

        List<ContentControl> elements;

        private void CreateItems()
        {
            if (_canvas == null) return;
            _canvas.Children.Clear();

            if (Content == null)
            {
                Content = new Ellipse()
                {
                    Height = 100,
                    Width = 100,
                    Fill = new SolidColorBrush(Colors.LightBlue)
                };
            }
            _canvas.Children.Add(Content as FrameworkElement);

            if (elements != null)
            {
                elements.Clear();
            }
            else
            {
                elements = new List<ContentControl>();
            }

            if (ItemsSource != null && ItemsSource.Count() > 0)
            {
                foreach (var item in ItemsSource)
                {
                    var control = CreateItem(item);
                    control.SetValue(AutomationProperties.NameProperty, item.Label);
                    _canvas.Children.Add(control);
                    elements.Add(control);
                }
            }

            UpdateItems();
        }

        private void UpdateItems()
        {
            if (_canvas == null) return;
            _canvas.InvalidateMeasure();

            var controlWidth = _canvas.ActualWidth;
            var controlHeight = _canvas.ActualHeight;

            double centerLeft(FrameworkElement element, double x) => (controlWidth / 2) + x - element.ActualHeight / 2;
            double centerTop(FrameworkElement element, double y) => (controlHeight / 2) - y - element.ActualWidth / 2;

            Canvas.SetTop(Content as FrameworkElement, centerTop(Content as FrameworkElement, 0));
            Canvas.SetLeft(Content as FrameworkElement, centerLeft(Content as FrameworkElement, 0));

            var count = elements.Count();
            var angle = 2 * Math.PI / count;

            var minDiameter = 10;
            var maxDiameter = 40;

            var minDistance = 100;
            var maxDistance = (Math.Min(controlWidth, controlHeight) - maxDiameter) / 2;

            var random = new Random();

            for (var i = 0; i < count; i++)
            {
                var control = elements.ElementAt(i);
                var item = control.DataContext as SpaceViewItem;

                var distance = (item.Distance) * (maxDistance - minDistance) + minDistance;

                var x = distance * Math.Cos(angle * i);
                var y = distance * Math.Sin(angle * i);

                Canvas.SetTop(control, centerTop(control, y));
                Canvas.SetLeft(control, centerLeft(control, x));
            }
        }

        private ContentControl CreateItem(SpaceViewItem item)
        {
            var control = new ContentControl();
            control.IsTabStop = true;
            control.UseSystemFocusVisuals = true;
            control.DataContext = item;

            FrameworkElement element = ItemTemplate?.LoadContent() as FrameworkElement;
            if (element == null)
            {
                var minDiameter = 5;
                var maxDiameter = 40;

                var itemEllipse = new Ellipse()
                {
                    Width = (item.Diameter) * (maxDiameter - minDiameter) + minDiameter,
                    Height = (item.Diameter) * (maxDiameter - minDiameter) + minDiameter,
                    Fill = new SolidColorBrush(Colors.LightGray),
                };

                if (item.Image != null)
                {
                    itemEllipse.Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri(item.Image)) };
                }

                control.Height = control.Width = itemEllipse.Height;
                control.Content = itemEllipse;
            }
            else
            {
                control.Content = element;
            }

            return control;
        }

    }
}
