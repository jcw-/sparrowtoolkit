#if WPF
#endif
using System.Windows;
#if !WINRT
using System.Windows.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace Sparrow.Chart
{
    /// <summary>
    /// Axes Panel
    /// </summary>
    public class AxesPanel : StackPanel
    {

        /// <summary>
        /// Gets or sets the axes.
        /// </summary>
        /// <value>
        /// The axes.
        /// </value>
        public Axes Axes
        {
            get { return (Axes)GetValue(AxesProperty); }
            set { SetValue(AxesProperty, value); }
        }

        /// <summary>
        /// The axes property
        /// </summary>
        public static readonly DependencyProperty AxesProperty =
            DependencyProperty.Register("Axes", typeof(Axes), typeof(AxesPanel), new PropertyMetadata(null));


    }
}
