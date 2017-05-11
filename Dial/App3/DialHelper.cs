using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.DataVisualization;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App3
{
    public class DialHelper
    {
        static RadialBarGaugeIndicator _control;

        private static RadialController _controller;

        public static RadialController Controller
        {
            get
            {
                return _controller;
            }

            set
            {
                _controller = value;
            }
        }
        
        /// <summary>
        /// The amount the TextBox will be modified for each rotation step on the Surface Dial. This can be any double value.
        /// </summary>
        public static readonly DependencyProperty StepValueProperty =
            DependencyProperty.RegisterAttached("StepValue", typeof(double), typeof(DialHelper), new PropertyMetadata(0d, new PropertyChangedCallback(StepValueChanged)));

        /// <summary>
        /// Setter of the StepValue.
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <returns>Return value of property</returns>
        public static double GetStepValue(DependencyObject obj)
        {
            return (double)obj.GetValue(StepValueProperty);
        }

        /// <summary>
        /// Getter of the StepValue
        /// </summary>
        /// <param name="obj">The Depenency Object we are dealing with, like a TextBox.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetStepValue(DependencyObject obj, double value)
        {
            obj.SetValue(StepValueProperty, value);
        }


        private static void StepValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _control = d as RadialBarGaugeIndicator;

            if (_control == null)
            {
                return;
            }

            _controller = _controller ?? RadialController.CreateForCurrentView();

            var menuItem = RadialControllerMenuItem.CreateFromKnownIcon("Radial Bar", RadialControllerMenuKnownIcon.Scroll);
            _controller.Menu.Items.Add(menuItem);
            _controller.Menu.SelectMenuItem(menuItem);

            _controller.UseAutomaticHapticFeedback = true;
            _controller.RotationResolutionInDegrees = 1;
            _controller.RotationChanged += Controller_RotationChanged;
        }

        private static void Controller_RotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            if (_control == null)
            {
                return;
            }

            _control.Value += args.RotationDeltaInDegrees * GetStepValue(_control);
        }
    }
}
