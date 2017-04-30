using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public SpaceView()
        {
            DefaultStyleKey = typeof(SpaceView);

            IsTabStop = false;
            TabNavigation = KeyboardNavigationMode.Once;
            KeyDown += SpaceView_KeyDown;

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var items = new List<SpaceViewItem>();
                items.Add(new SpaceViewItem() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                items.Add(new SpaceViewItem() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                items.Add(new SpaceViewItem() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                ItemsSource = items;
            }
        }

        private Canvas _itemCanvas;
        private Canvas _orbitsCanvas;
        private Canvas _anchorCanvas;
        private ContentPresenter _centerContent;

        private Compositor _compositor;
        private const double _animationDuration = 200;

        public event EventHandler<SpaceViewItemClickedEventArgs> ItemClicked;

        private void SpaceView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Left)
            {
                e.Handled = true;
                var currentEllement = FocusManager.GetFocusedElement() as ContentControl;
                if (currentEllement != null)
                {
                    var index = _itemCanvas.Children.IndexOf(currentEllement);
                    var nextIndex = (index + 1) % _itemCanvas.Children.Count;

                    (_itemCanvas.Children.ElementAt(nextIndex) as ContentControl).Focus(FocusState.Keyboard);
                }
            }
            else if (e.Key == Windows.System.VirtualKey.Right)
            {
                e.Handled = true;
                var currentEllement = FocusManager.GetFocusedElement() as ContentControl;
                if (currentEllement != null)
                {
                    var index = _itemCanvas.Children.IndexOf(currentEllement);
                    var nextIndex = index > 0 ? index - 1 : _itemCanvas.Children.Count - 1;

                    (_itemCanvas.Children.ElementAt(nextIndex) as ContentControl).Focus(FocusState.Keyboard);
                }
            }
        }

        protected override void OnApplyTemplate()
        {
            if (_itemCanvas != null)
            {
                _itemCanvas.SizeChanged -= _canvas_SizeChanged;
            }

            _itemCanvas = (Canvas)GetTemplateChild("ItemCanvas");
            if (_itemCanvas == null)
            {
                return;
            }

            _centerContent = (ContentPresenter)GetTemplateChild("CenterContent");
            if (_centerContent == null)
            {
                return;
            }

            _compositor = ElementCompositionPreview.GetElementVisual(_itemCanvas).Compositor;

            CreateItems();
            _itemCanvas.SizeChanged += _canvas_SizeChanged;
            
            base.OnApplyTemplate();
        }

        private void _canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PositionItems();
        }

        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(SpaceView), new PropertyMetadata(null, OnItemsChanged));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(SpaceView), new PropertyMetadata(null));
        

        public bool OrbitsEnabled
        {
            get { return (bool)GetValue(OrbitsEnabledProperty); }
            set { SetValue(OrbitsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrbitsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrbitsEnabledProperty =
            DependencyProperty.Register("OrbitsEnabled", typeof(bool), typeof(SpaceView), new PropertyMetadata(false, OnOrbitsEnabledChanged));



        public bool IsItemClickEnabled
        {
            get { return (bool)GetValue(IsItemClickEnabledProperty); }
            set { SetValue(IsItemClickEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsItemClickEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsItemClickEnabledProperty =
            DependencyProperty.Register("IsItemClickEnabled", typeof(bool), typeof(SpaceView), new PropertyMetadata(false, OnItemClickEnabledChanged));

        public bool AnchorsEnabled
        {
            get { return (bool)GetValue(AnchorsEnabledProperty); }
            set { SetValue(AnchorsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnchorsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnchorsEnabledProperty =
            DependencyProperty.Register("AnchorsEnabled", typeof(bool), typeof(SpaceView), new PropertyMetadata(false, OnAchorsEnabledChanged));



        public double MinItemSize
        {
            get { return (double)GetValue(MinItemSizeProperty); }
            set { SetValue(MinItemSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinItemSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinItemSizeProperty =
            DependencyProperty.Register("MinItemSize", typeof(double), typeof(SpaceView), new PropertyMetadata(20d));



        public double MaxItemSize
        {
            get { return (double)GetValue(MaxItemSizeProperty); }
            set { SetValue(MaxItemSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxItemSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxItemSizeProperty =
            DependencyProperty.Register("MaxItemSize", typeof(double), typeof(SpaceView), new PropertyMetadata(50d));



        public Brush AnchorColor
        {
            get { return (Brush)GetValue(AnchorColorProperty); }
            set { SetValue(AnchorColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnchorColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnchorColorProperty =
            DependencyProperty.Register("AnchorColor", typeof(Brush), typeof(SpaceView), new PropertyMetadata(new SolidColorBrush(Colors.Black)));



        public Brush OrbitColor
        {
            get { return (Brush)GetValue(OrbitColorProperty); }
            set { SetValue(OrbitColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrbitColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrbitColorProperty =
            DependencyProperty.Register("OrbitColor", typeof(Brush), typeof(SpaceView), new PropertyMetadata(new SolidColorBrush(Colors.Black)));




        public DoubleCollection OrbitDashArray
        {
            get { return (DoubleCollection)GetValue(OrbitDashArrayProperty); }
            set { SetValue(OrbitDashArrayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrbitDashArray.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrbitDashArrayProperty =
            DependencyProperty.Register("OrbitDashArray", typeof(DoubleCollection), typeof(SpaceView), new PropertyMetadata(null));


        public double AnchorThickness
        {
            get { return (double)GetValue(AnchorThicknessProperty); }
            set { SetValue(AnchorThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnchorThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnchorThicknessProperty =
            DependencyProperty.Register("AnchorThickness", typeof(double), typeof(double), new PropertyMetadata(2d));


        public double OrbitThickness
        {
            get { return (double)GetValue(OrbitThicknessProperty); }
            set { SetValue(OrbitThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrbitThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrbitThicknessProperty =
            DependencyProperty.Register("OrbitThickness", typeof(double), typeof(SpaceView), new PropertyMetadata(2d));




        private static void OnAchorsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SpaceView;

            if (e.NewValue == e.OldValue) return;

            if ((bool)e.NewValue)
            {
                sv.PositionItems();
            }
            else
            {
                sv.ClearAnchors();
                sv.PositionItems();
            }
        }

        private static void OnOrbitsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SpaceView;

            if (e.NewValue == e.OldValue) return;

            if (!(bool)e.NewValue)
            { 
                sv.ClearOrbits();
            }
            else
            {
                sv.CreateOrbits();
            }

            sv.PositionItems();
        }

        private static void OnItemClickEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SpaceView;

            if (sv._itemCanvas == null || sv._itemCanvas.Children.Count == 0) return;

            foreach (var control in sv._itemCanvas.Children)
            {
                if ((bool)e.NewValue)
                {
                    sv.EnableItemInteraction(control as ContentControl);
                }
                else
                {
                    sv.DisableItemInteraction(control as ContentControl);
                }
            }
        }

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;

            var control = d as SpaceView;
            if (control == null) return;

            if (e.NewValue == null) return;

            control.HandleNewItemsSource(e.OldValue, e.NewValue);
        }

        private void HandleNewItemsSource(object oldValue, object newValue)
        {
            var oldValueObservable = oldValue as INotifyCollectionChanged;
            if (oldValueObservable != null)
            {
                oldValueObservable.CollectionChanged -= ObservableList_CollectionChanged;
            }

            var items = newValue as IEnumerable<object>;
            if (items == null) return;

            var observableList = items as INotifyCollectionChanged;
            if (observableList != null)
            {
                observableList.CollectionChanged += ObservableList_CollectionChanged;
            }

            CreateItems();
        }

        private void ObservableList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var control = CreateItem(item as SpaceViewItem);
                        ApplyImplicitOffsetAnimation(control);
                        _itemCanvas.Children.Add(control);
                    }

                    ClearAnchors();

                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    CreateItems();
                    break;
            }

            PositionItems();
        }

        private void CreateItems()
        {
            if (_itemCanvas == null) return;
            _itemCanvas.Children.Clear();

            var itemSource = ItemsSource as IEnumerable<object>;

            if (ItemsSource != null && itemSource.Count() > 0)
            {
                foreach (var item in itemSource)
                {
                    var control = CreateItem(item);
                    ApplyImplicitOffsetAnimation(control);
                    _itemCanvas.Children.Add(control);
                }
            }

            if (OrbitsEnabled) CreateOrbits();

            PositionItems();
        }

        private void PositionItems()
        {
            if (_itemCanvas == null) return;
            _itemCanvas.InvalidateMeasure();

            var controlWidth = _itemCanvas.ActualWidth;
            var controlHeight = _itemCanvas.ActualHeight;

            double centerLeft(FrameworkElement element, double x) => (controlWidth / 2) + x - element.ActualHeight / 2;
            double centerTop(FrameworkElement element, double y) => (controlHeight / 2) - y - element.ActualWidth / 2;

            var angle = 2 * Math.PI / _itemCanvas.Children.Count;

            var minDistance = 100;
            var maxDistance = Math.Max(minDistance, (Math.Min(controlWidth, controlHeight) - MaxItemSize) / 2);

            var random = new Random();

            var positionOrbits = OrbitsEnabled && _orbitsCanvas != null && _orbitsCanvas.Children.Count == _itemCanvas.Children.Count;
            var createAnchors = ShouldAnchorsBeCreated();
            
            for (var i = 0; i < _itemCanvas.Children.Count; i++)
            {
                var control = _itemCanvas.Children.ElementAt(i) as ContentControl;

                var item = control.DataContext as SpaceViewItem;

                var distance = (item.Distance) * (maxDistance - minDistance) + minDistance;

                var x = distance * Math.Cos(angle * i + angle/2);
                var y = distance * Math.Sin(angle * i + angle/2);

                Canvas.SetTop(control, centerTop(control, y));
                Canvas.SetLeft(control, centerLeft(control, x));

                if (positionOrbits)
                {
                    var orbit = _orbitsCanvas.Children.ElementAt(i) as FrameworkElement;
                    orbit.Height = orbit.Width = 2 * distance;
                    Canvas.SetTop(orbit, centerTop(orbit, 0));
                    Canvas.SetLeft(orbit, centerLeft(orbit, 0));
                }

                // must be created in layout stage due to expression animations need to know x and y
                if (createAnchors)
                {
                    var anchor = CreateAnchor(control, x, y);
                    _anchorCanvas.Children.Add(anchor);
                }
            }
        }

        private ContentControl CreateItem(object item)
        {
            var control = new ContentControl();
            control.DataContext = item;

            var spaceViewItem = item as SpaceViewItem;
            

            FrameworkElement element = ItemTemplate?.LoadContent() as FrameworkElement;
            if (element == null)
            {
                var itemEllipse = new Ellipse()
                {
                    Fill = Foreground,
                };

                if (spaceViewItem != null && spaceViewItem.Image != null)
                {
                    itemEllipse.Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri(spaceViewItem.Image)) };
                }

                element = itemEllipse;
            }

            if (spaceViewItem != null)
            {
                control.SetValue(AutomationProperties.NameProperty, spaceViewItem.Label);
                element.Width = element.Height = (spaceViewItem.Diameter) * (MaxItemSize - MinItemSize) + MinItemSize;
            }
            else
            {
                control.SetValue(AutomationProperties.NameProperty, item.ToString());
            }

            control.Content = element;


            if (IsItemClickEnabled)
            {
                EnableItemInteraction(control);
            }

            return control;
        }

        private void EnableItemInteraction(ContentControl control)
        {
            DisableItemInteraction(control);

            control.IsTabStop = true;
            control.UseSystemFocusVisuals = true;
            control.PointerEntered += Control_PointerEntered;
            control.PointerExited += Control_PointerExited;
            control.PointerPressed += Control_PointerPressed;
            control.PointerReleased += Control_PointerReleased;
            control.KeyDown += Control_KeyDown;
            control.KeyUp += Control_KeyUp;
        }

        private void DisableItemInteraction(ContentControl control)
        {
            control.IsTabStop = false;
            control.UseSystemFocusVisuals = false;
            control.PointerEntered -= Control_PointerEntered;
            control.PointerExited -= Control_PointerExited;
            control.PointerPressed -= Control_PointerPressed;
            control.PointerReleased -= Control_PointerReleased;
            control.KeyDown -= Control_KeyDown;
            control.KeyUp -= Control_KeyUp;
        }

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
                ItemClicked?.Invoke(this, new SpaceViewItemClickedEventArgs(item, item.DataContext));
            }
        }

        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(1, 1, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
            ItemClicked?.Invoke(this, new SpaceViewItemClickedEventArgs(item, item.DataContext));
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

        private void ClearOrbits()
        {
            if (_orbitsCanvas == null)
            {
                return;
            }

            _orbitsCanvas.Children.Clear();
        }

        private void CreateOrbits()
        {
            if (_orbitsCanvas == null)
            {
                _orbitsCanvas = (Canvas)GetTemplateChild("OrbitCanvas");
                if (_orbitsCanvas == null)
                {
                    return;
                }
            }

            _orbitsCanvas.Children.Clear();

            foreach (var item in _itemCanvas.Children)
            {
                var ellipse = new Ellipse()
                {
                    StrokeDashArray = OrbitDashArray,
                    Stroke = OrbitColor,
                    StrokeThickness = OrbitThickness
                };
                _orbitsCanvas.Children.Add(ellipse);
            }
        }

        private bool ShouldAnchorsBeCreated()
        {
            if (!AnchorsEnabled)
            {
                return false;
            }

            if (_anchorCanvas == null)
            {
                _anchorCanvas = (Canvas)GetTemplateChild("AnchorCanvas");
                if (_anchorCanvas == null)
                {
                    return false;
                }
            }

            return _anchorCanvas.Children.Count != _itemCanvas.Children.Count;
        }

        private Line CreateAnchor(UIElement element, double x, double y)
        {
            var anchor = new Line()
            {
                Stroke = AnchorColor,
                StrokeThickness = AnchorThickness,
                X1 = 0,
                Y1 = 0,
                X2 = 100,
                Y2 = 0
            };

            var anchorVisual = ElementCompositionPreview.GetElementVisual(anchor);
            var elementVisual = ElementCompositionPreview.GetElementVisual(element);
            var centerVisual = ElementCompositionPreview.GetElementVisual(_centerContent);

            string expression = "";
            var elementY = "(elementVisual.Offset.Y + elementVisual.Size.Y / 2)";
            var centerY = "(centerVisual.Offset.Y + centerVisual.Size.Y / 2)";
            var elementX = "(elementVisual.Offset.X + elementVisual.Size.X / 2)";
            var centerX = "(centerVisual.Offset.X + centerVisual.Size.X / 2)";

            var startingAngle = Math.Atan2(y, x);

            if (startingAngle > Math.PI / 4 && startingAngle < 3 * Math.PI / 4)
                expression = $"Atan((-1 * ({elementX} - {centerX})) / ( {elementY} - {centerY})) - PI / 2";
            else if (startingAngle >= 3 * Math.PI / 4 || startingAngle < -3 * Math.PI / 4)
                expression = $"Atan(({elementY} - {centerY}) / ({elementX} - {centerX})) + PI";
            else if (startingAngle >= -3 * Math.PI / 4 && startingAngle < Math.PI / -4)
                expression = $"Atan(({elementX} - {centerX}) / (-1 * ({elementY} - {centerY}))) + PI  / 2";
            else
                expression = $"Atan(({elementY} - {centerY}) / ({elementX} - {centerX}))";
            
            anchorVisual.CenterPoint = new Vector3(0);
            var rotationExpression = _compositor.CreateExpressionAnimation();
            rotationExpression.Expression = expression;
            rotationExpression.SetReferenceParameter("centerVisual", centerVisual);
            rotationExpression.SetReferenceParameter("elementVisual", elementVisual);
            anchorVisual.StartAnimation(nameof(anchorVisual.RotationAngle), rotationExpression);

            var offsetExpression = _compositor.CreateExpressionAnimation();
            offsetExpression.Expression = "Vector3(centerVisual.Offset.X + centerVisual.Size.X / 2, centerVisual.Offset.Y + centerVisual.Size.Y / 2, 0)";
            offsetExpression.SetReferenceParameter("centerVisual", centerVisual);
            anchorVisual.StartAnimation(nameof(anchorVisual.Offset), offsetExpression);

            var scaleExpression = _compositor.CreateExpressionAnimation();
            scaleExpression.Expression = $"Vector3(Pow(Pow({elementX} - {centerX}, 2) + Pow({elementY} - {centerY}, 2), 0.5)/100, 1, 1)";
            scaleExpression.SetReferenceParameter("centerVisual", centerVisual);
            scaleExpression.SetReferenceParameter("elementVisual", elementVisual);
            anchorVisual.StartAnimation(nameof(anchorVisual.Scale), scaleExpression);

            return anchor;
        }

        private void ClearAnchors()
        {
            if (_anchorCanvas == null)
            {
                return;
            }

            _anchorCanvas.Children.Clear();
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
