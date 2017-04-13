using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Hosting;
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
        private Compositor _compositor;

        List<ContentControl> _elements;
        List<Ellipse> _orbits;
        List<Ellipse> _anchors;

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

            _compositor = ElementCompositionPreview.GetElementVisual(_canvas).Compositor;

            CreateItems();
            _canvas.SizeChanged += _canvas_SizeChanged;

            base.OnApplyTemplate();
        }

        private void _canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PositionItems();
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



        public bool AreOrbitsEnabled
        {
            get { return (bool)GetValue(AreOrbitsEnabledProperty); }
            set { SetValue(AreOrbitsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AreOrbitsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AreOrbitsEnabledProperty =
            DependencyProperty.Register("AreOrbitsEnabled", typeof(bool), typeof(SpaceView), new PropertyMetadata(false, OnOrbitsEnabledChanged));



        public bool AreAnchorsEnabled
        {
            get { return (bool)GetValue(AreAnchorsEnabledProperty); }
            set { SetValue(AreAnchorsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AreAnchorsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AreAnchorsEnabledProperty =
            DependencyProperty.Register("AreAnchorsEnabled", typeof(bool), typeof(SpaceView), new PropertyMetadata(false));



        private static void OnOrbitsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SpaceView;

            if (e.NewValue == e.OldValue) return;

            if ((bool)e.NewValue)
            {
                sv.CreateOrbits();
                sv.PositionItems();
            }
            else
            {
                sv.ClearOrbits();
                sv.PositionItems();
            }
        }

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

            var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);

            var delayCount = 0;

            ApplyImplicitOffsetAnimation(Content as FrameworkElement, delayCount++ * 40);
            _canvas.Children.Add(Content as FrameworkElement);

            if (_elements != null)
            {
                _elements.Clear();
            }
            else
            {
                _elements = new List<ContentControl>();
            }

            if (_orbits != null) _orbits.Clear();
            else _orbits = new List<Ellipse>();

            if (ItemsSource != null && ItemsSource.Count() > 0)
            {
                foreach (var item in ItemsSource)
                {
                    var control = CreateItem(item);
                    control.SetValue(AutomationProperties.NameProperty, item.Label);
                    ApplyImplicitOffsetAnimation(control, delayCount++ * 40);
                    _canvas.Children.Add(control);
                    _elements.Add(control);
                }
            }

            if (AreOrbitsEnabled)
            {
                CreateOrbits();
            }

            PositionItems();
        }

        private void PositionItems()
        {
            if (_canvas == null) return;
            _canvas.InvalidateMeasure();

            var controlWidth = _canvas.ActualWidth;
            var controlHeight = _canvas.ActualHeight;

            double centerLeft(FrameworkElement element, double x) => (controlWidth / 2) + x - element.ActualHeight / 2;
            double centerTop(FrameworkElement element, double y) => (controlHeight / 2) - y - element.ActualWidth / 2;

            Canvas.SetTop(Content as FrameworkElement, centerTop(Content as FrameworkElement, 0));
            Canvas.SetLeft(Content as FrameworkElement, centerLeft(Content as FrameworkElement, 0));

            var count = _elements.Count();
            var angle = 2 * Math.PI / count;

            var minDiameter = 10;
            var maxDiameter = 40;

            var minDistance = 100;
            var maxDistance = (Math.Min(controlWidth, controlHeight) - maxDiameter) / 2;

            var random = new Random();

            for (var i = 0; i < count; i++)
            {
                var control = _elements.ElementAt(i);

                var item = control.DataContext as SpaceViewItem;

                var distance = (item.Distance) * (maxDistance - minDistance) + minDistance;

                var x = distance * Math.Cos(angle * i + angle/2);
                var y = distance * Math.Sin(angle * i + angle/2);

                Canvas.SetTop(control, centerTop(control, y));
                Canvas.SetLeft(control, centerLeft(control, x));

                if (AreOrbitsEnabled)
                {
                    var orbit = _orbits.ElementAt(i);
                    orbit.Height = orbit.Width = 2 * distance;
                    Canvas.SetTop(orbit, centerTop(orbit, 0));
                    Canvas.SetLeft(orbit, centerLeft(orbit, 0));
                }
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

        private Ellipse CreateOrbit()
        {
            var ellipse = new Ellipse()
            {
                StrokeDashArray = { 5, 5 },
                Stroke = new SolidColorBrush(Colors.LightGray),
                StrokeThickness = 2
            };

            Canvas.SetZIndex(ellipse, -1);

            return ellipse;
        }

        private void CreateOrbits()
        {
            if (_canvas == null) return;

            if (ItemsSource != null && ItemsSource.Count() > 0)
            {
                foreach (var item in ItemsSource)
                {
                    var orbit = CreateOrbit();
                    //ApplyImplicitOffsetAnimation(orbit, delayCount++ * 40);
                    _canvas.Children.Add(orbit);
                    _orbits.Add(orbit);
                }
            }
        }

        private void ClearOrbits()
        {
            if (_canvas == null || _orbits == null) return;

            foreach(var orbit in _orbits)
            {
                _canvas.Children.Remove(orbit);
            }

            _orbits.Clear();
        }

        private void ApplyImplicitOffsetAnimation(UIElement element, double delay = 0)
        {
            // don't use animations if running in designer
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;

            if (_compositor == null) return;

            var easeIn = _compositor.CreateCubicBezierEasingFunction(new Vector2(0.03f, 1.11f), new Vector2(.66f, 1.11f));

            var offsetAnimation = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.Target = nameof(Visual.Offset);
            offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue", easeIn);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(200);
            offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(delay);

            var implicitAnimations = _compositor.CreateImplicitAnimationCollection();
            implicitAnimations[nameof(Visual.Offset)] = offsetAnimation;

            ElementCompositionPreview.GetElementVisual(element).ImplicitAnimations = implicitAnimations;
        }
    }
}
