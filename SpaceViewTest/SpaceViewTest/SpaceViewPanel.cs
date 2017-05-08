using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SpaceViewTest
{
    internal class SpaceViewPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var controlWidth = finalSize.Width;
            var controlHeight = finalSize.Height;

            double centerLeft(FrameworkElement element, double x) => (controlWidth / 2) + x - element.DesiredSize.Height / 2;
            double centerTop(FrameworkElement element, double y) => (controlHeight / 2) - y - element.DesiredSize.Width / 2;

            var angle = 2 * Math.PI / Children.Count;

            var maxItemSize = Children.Max(c => c.DesiredSize.Width);

            var minDistance = 100;
            var maxDistance = Math.Max(minDistance, (Math.Min(controlWidth, controlHeight) - maxItemSize) / 2);

            var random = new Random();

            //var positionOrbits = OrbitsEnabled && _orbitsCanvas != null && _orbitsCanvas.Children.Count == _itemCanvas.Children.Count;
            //var createAnchors = ShouldAnchorsBeCreated();

            for (var i = 0; i < Children.Count; i++)
            {
                var control = Children.ElementAt(i) as ContentControl;

                var item = control.DataContext as SpaceViewItem;

                var distance = (item.Distance) * (maxDistance - minDistance) + minDistance;

                var x = distance * Math.Cos(angle * i + angle / 2);
                var y = distance * Math.Sin(angle * i + angle / 2);

                control.Arrange(new Rect(centerLeft(control, x), centerTop(control, y), control.DesiredSize.Width, control.DesiredSize.Height));

                //Canvas.SetTop(control, centerTop(control, y));
                //Canvas.SetLeft(control, centerLeft(control, x));

                //if (positionOrbits)
                //{
                //    var orbit = _orbitsCanvas.Children.ElementAt(i) as FrameworkElement;
                //    orbit.Height = orbit.Width = 2 * distance;
                //    Canvas.SetTop(orbit, centerTop(orbit, 0));
                //    Canvas.SetLeft(orbit, centerLeft(orbit, 0));
                //}

                //// must be created in layout stage due to expression animations need to know x and y
                //if (createAnchors)
                //{
                //    var anchor = CreateAnchor(control, x, y);
                //    _anchorCanvas.Children.Add(anchor);
                //}
            }

            return finalSize;
        }


    }
}
