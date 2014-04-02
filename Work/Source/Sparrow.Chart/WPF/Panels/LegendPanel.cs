using System.Windows;
#if !WINRT
using System.Windows.Controls;

#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
#endif

namespace Sparrow.Chart
{
    public class LegendPanel : StackPanel
    {


        /// <summary>
        /// Gets or sets the dock position.
        /// </summary>
        /// <value>
        /// The dock position.
        /// </value>
        public Dock DockPosition
        {
            get { return (Dock)GetValue(DockPositionProperty); }
            set { SetValue(DockPositionProperty, value); }
        }

        /// <summary>
        /// The dock position property
        /// </summary>
        public static readonly DependencyProperty DockPositionProperty =
            DependencyProperty.Register("DockPosition", typeof(Dock), typeof(LegendPanel), new PropertyMetadata(Dock.Top,OnDockChanged));

        /// <summary>
        /// Called when [dock changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnDockChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var legendPanel = sender as LegendPanel;
            if (legendPanel != null) legendPanel.DockChanged(args);
        }

        /// <summary>
        /// Docks the changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void DockChanged(DependencyPropertyChangedEventArgs args)
        {
            switch (DockPosition)
            {

                case Dock.Top:
                case Dock.Bottom:
                    this.Orientation = Orientation.Horizontal;
                    break;
                case Dock.Left:
                case Dock.Right:
                    this.Orientation = Orientation.Vertical;
                    break;
                default:
                    break;
            }
            this.InvalidateMeasure();
            this.InvalidateArrange();
        }        
    }
}
