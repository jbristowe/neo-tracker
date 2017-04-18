using Microsoft.Toolkit.Uwp.UI.Animations;
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
    public sealed class NEOVisualizer : ContentControl
    {
        private Canvas _canvas;
        List<FrameworkElement> _elements = new List<FrameworkElement>();

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

        public event EventHandler<ItemClickedEventArgs> ItemClicked;

        protected override void OnApplyTemplate()
        {
            _canvas = (Canvas)GetTemplateChild("Canvas");

            if (_canvas == null)
            {
                return;
            }

            _canvas.SizeChanged += (s, e) => PositionNEOs();
        }

        public IEnumerable<NEO> ItemsSource
        {
            get { return (IEnumerable<NEO>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<NEO>), typeof(NEOVisualizer), new PropertyMetadata(null, OnItemsChanged));

        public double MaxItemSize
        {
            get { return (double)GetValue(MaxItemSizeProperty); }
            set { SetValue(MaxItemSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxItemSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxItemSizeProperty =
            DependencyProperty.Register("MaxItemSize", typeof(double), typeof(NEOVisualizer), new PropertyMetadata(60d));

        public double MinItemSize
        {
            get { return (double)GetValue(MinItemSizeProperty); }
            set { SetValue(MinItemSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinItemSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinItemSizeProperty =
            DependencyProperty.Register("MinItemSize", typeof(double), typeof(NEOVisualizer), new PropertyMetadata(10d));



        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(NEOVisualizer), new PropertyMetadata(null));

        public NEO SelectedItem
        {
            get { return (NEO)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(NEO), typeof(NEOVisualizer), new PropertyMetadata(null, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NEOVisualizer;

            NEO newItem, oldItem;
            int newIndex, oldIndex;

            if (e.NewValue != null)
            {
                newItem = e.NewValue as NEO;
                newIndex = control.ItemsSource.ToList().IndexOf(newItem);
                var newElement = control._elements.ElementAt(newIndex);
                newElement.Fade(1f, control._animationDuration).Start();

                foreach (var element in control._elements)
                {
                    if (element != newElement) element.Fade(0.3f, control._animationDuration).Start();
                }
            }
            
        }

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            if (e.NewValue == e.OldValue) return;

            var control = d as NEOVisualizer;
            if (control == null) return;

            var items = e.NewValue as IEnumerable<NEO>;
            if (items == null) return;

            control.CreateNEOs();
        }

        private void CreateNEOs()
        {
            if (_canvas == null) return;
            _canvas.Children.Clear();
            _elements.Clear();

            var centerElement = Content as FrameworkElement;
            _canvas.Children.Add(centerElement);


            if (ItemsSource == null || ItemsSource.Count() <= 0) return;

            foreach (var item in ItemsSource)
            {
                FrameworkElement element = ItemTemplate?.LoadContent() as FrameworkElement;
                
                if (element == null)
                {
                    var neoEllipse = new Ellipse();
                    neoEllipse.Fill = new SolidColorBrush(Colors.LightGray);

                    element = neoEllipse;
                }

                //var viewbox = new Viewbox();
                //viewbox.Height = viewbox.Width = (item.DiameterWidth) * (MaxItemSize - MinItemSize) + MinItemSize;
                //viewbox.Stretch = Stretch.Uniform;
                //viewbox.Child = element;

                element.Height = element.Width = (item.DiameterWidth) * (MaxItemSize - MinItemSize) + MinItemSize;


                var control = new ContentControl();
                control.Content = element;
                control.DataContext = item;
                control.IsTabStop = true;
                control.UseSystemFocusVisuals = true;

                control.PointerEntered += Control_PointerEntered;
                control.PointerExited += Control_PointerExited;
                control.PointerPressed += Control_PointerPressed;
                control.PointerReleased += Control_PointerReleased;
                control.KeyDown += Control_KeyDown;
                control.KeyUp += Control_KeyUp;

                control.SetValue(AutomationProperties.NameProperty, item.Label);
                _canvas.Children.Add(control);
                _elements.Add(control);
            }

            PositionNEOs();
        }

        private void PositionNEOs()
        {
            if (_canvas == null) return;

            var controlWidth = _canvas.ActualWidth;
            var controlHeight = _canvas.ActualHeight;

            double centerLeft(FrameworkElement element, double x) => (controlWidth / 2) + x - element.ActualWidth / 2;
            double centerTop(FrameworkElement element, double y) => (controlHeight / 2) - y - element.ActualHeight / 2;

            var centerElement = Content as FrameworkElement;
            Canvas.SetTop(centerElement, centerTop(centerElement, 0));
            Canvas.SetLeft(centerElement, centerLeft(centerElement, 0));

            if (_elements.Count() <= 0) return;

            var count = ItemsSource.Count();
            var angle = 2 * Math.PI / count;

            var minDistance = Math.Max(centerElement.ActualHeight, centerElement.ActualWidth) + 50;
            var maxDistance = Math.Min(controlWidth, controlHeight) / 2;

            var random = new Random();

            for (var i = 0; i < count; i++)
            {
                var item = ItemsSource.ElementAt(i);
                var element = _elements.ElementAt(i);

                var distance = (item.Distance) * (maxDistance - minDistance) + minDistance; 

                var x = distance * Math.Cos(angle * i);
                var y = distance * Math.Sin(angle * i);

                Canvas.SetTop(element, centerTop(element, y));
                Canvas.SetLeft(element, centerLeft(element, x));
            }
        }

        private double _animationDuration = 200;

        private void Control_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space || e.Key == Windows.System.VirtualKey.GamepadA)
            {
                var item = sender as ContentControl;
                item.Scale(1, 1, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
            }
        }

        private void Control_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space || e.Key == Windows.System.VirtualKey.GamepadA)
            {
                var item = sender as ContentControl;
                item.Scale(0.9f, 0.9f, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
                ItemClicked?.Invoke(this, new ItemClickedEventArgs(item, item.DataContext));
                SelectedItem = item.DataContext as NEO;
            }
        }

        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(1, 1, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
            ItemClicked?.Invoke(this, new ItemClickedEventArgs(item, item.DataContext));
            SelectedItem = item.DataContext as NEO;
        }

        private void Control_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(0.9f, 0.9f, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
        }

        private void Control_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(1, 1, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
        }

        private void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(1.1f, 1.1f, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
        }

    }

    public class ItemClickedEventArgs
    {
        internal ItemClickedEventArgs(ContentControl container, object item)
        {
            Container = container;
            Item = item;
        }

        public ContentControl Container { get; set; }
        public object Item { get; set; }
    }
}
