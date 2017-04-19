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
    public sealed class NEOVisualizer : ContentControl
    {
        private Canvas _canvas;
        private double _animationDuration = 200;

        public NEOVisualizer()
        {
            this.DefaultStyleKey = typeof(NEOVisualizer);

            // NIKO:CONTROLER
            this.IsTabStop = false;
            TabNavigation = KeyboardNavigationMode.Once;
            KeyDown += NEOVisualizer_KeyDown;

            // NIKO:DESIGNER
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var items = new List<NEO>();
                items.Add(new NEO() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                items.Add(new NEO() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                items.Add(new NEO() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                ItemsSource = items;
            }
        }

        // NIKO:CONTROLER
        private void NEOVisualizer_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Left)
            {
                e.Handled = true;
                var currentEllement = FocusManager.GetFocusedElement() as ContentControl;
                if (currentEllement != null)
                {
                    var index = _canvas.Children.IndexOf(currentEllement);
                    var nextIndex = (index + 1) % _canvas.Children.Count;

                    (_canvas.Children.ElementAt(nextIndex) as ContentControl).Focus(FocusState.Keyboard);
                }
            }
            else if (e.Key == Windows.System.VirtualKey.Right)
            {
                e.Handled = true;
                var currentEllement = FocusManager.GetFocusedElement() as ContentControl;
                if (currentEllement != null)
                {
                    var index = _canvas.Children.IndexOf(currentEllement);
                    var nextIndex = index > 0 ? index - 1 : _canvas.Children.Count - 1;

                    (_canvas.Children.ElementAt(nextIndex) as ContentControl).Focus(FocusState.Keyboard);
                }
            }
        }

        protected override void OnApplyTemplate()
        {
            _canvas = (Canvas)GetTemplateChild("Canvas");
            if (_canvas == null)
            {
                return;
            }

            _canvas.SizeChanged += (s, e) => PositionNEOs();
        }

        #region dp
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

            NEO newItem;
            int newIndex; ;

            if (e.NewValue != null)
            {
                newItem = e.NewValue as NEO;
                newIndex = control.ItemsSource.ToList().IndexOf(newItem);
                var newElement = control._canvas.Children.ElementAt(newIndex);
                newElement.Fade(1f, control._animationDuration).Start();

                foreach (var element in control._canvas.Children)
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
        #endregion

        private void CreateNEOs()
        {
            if (_canvas == null) return;
            _canvas.Children.Clear();

            if (ItemsSource == null || ItemsSource.Count() == 0) return;

            foreach (var item in ItemsSource)
            {
                // NIKO:DATATEMPLTE
                FrameworkElement element = ItemTemplate?.LoadContent() as FrameworkElement;
                if (element == null)
                {
                    var neoEllipse = new Ellipse();
                    neoEllipse.Fill = this.Foreground;
                    element = neoEllipse;
                }

                element.Height = element.Width = (item.Diameter) * (MaxItemSize - MinItemSize) + MinItemSize;

                var control = new ContentControl();
                control.Content = element;
                control.DataContext = item;

                // NIKO:INPUT
                control.IsTabStop = true;
                control.UseSystemFocusVisuals = true;
                control.GotFocus += Control_GotFocus;
                control.LostFocus += Control_LostFocus; ;
                control.PointerEntered += Control_PointerEntered;
                control.PointerExited += Control_PointerExited;
                control.PointerPressed += Control_PointerPressed;
                control.PointerReleased += Control_PointerReleased;
                control.KeyDown += Control_KeyDown;
                control.KeyUp += Control_KeyUp;

                // NIKO:ACCESIBILITY
                control.SetValue(AutomationProperties.NameProperty, item.Label);

                _canvas.Children.Add(control);
            }

            PositionNEOs();
        }
        
        private void PositionNEOs()
        {
            if (_canvas == null || _canvas.Children.Count == 0) return;
            var controlWidth = _canvas.ActualWidth;
            var controlHeight = _canvas.ActualHeight;

            double centerLeft(FrameworkElement element, double x) => (controlWidth / 2) + x - element.ActualWidth / 2;
            double centerTop(FrameworkElement element, double y) => (controlHeight / 2) - y - element.ActualHeight / 2;

            var centerElement = Content as FrameworkElement;
            var maxDistance = Math.Min(controlWidth, controlHeight) / 2;
            var minDistance = Math.Min(maxDistance,
                                (centerElement != null ? Math.Max(centerElement.ActualHeight, centerElement.ActualWidth) : 0) + MaxItemSize) ;
            
            var count = ItemsSource.Count();
            var angle = 2 * Math.PI / count;

            for (var i = 0; i < count; i++)
            {
                var item = ItemsSource.ElementAt(i);
                var element = _canvas.Children.ElementAt(i) as FrameworkElement;

                var distance = (item.Distance) * (maxDistance - minDistance) + minDistance; 

                var x = distance * Math.Cos(angle * i);
                var y = distance * Math.Sin(angle * i);

                Canvas.SetTop(element, centerTop(element, y));
                Canvas.SetLeft(element, centerLeft(element, x));
            }
        }

        #region Input
        private void Control_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space || e.Key == Windows.System.VirtualKey.GamepadA)
            {
                var item = sender as ContentControl;
                item.Scale(2f, 2f, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
            }
        }

        private void Control_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space || e.Key == Windows.System.VirtualKey.GamepadA)
            {
                var item = sender as ContentControl;
                item.Scale(1.1f, 1.1f, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
                SelectedItem = item.DataContext as NEO;
            }
        }

        private void Control_LostFocus(object sender, RoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(1, 1, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
        }

        private void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(2f, 2f, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
        }

        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(1, 1, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
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
        static Random r = new Random();

        private void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(1.5f, 1.5f, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();

            if (_canvas.Children.IndexOf(item) == 0)
                item.Offset(r.Next(-50, 50), r.Next(-50, 50), 200).Start();
        }
        #endregion
    }
}
