using System.Windows;
#if !WINRT
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Shapes;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
#endif

namespace Sparrow.Chart
{
    /// <summary>
    /// Legend Item
    /// </summary>
    public class LegendItem : DependencyObject
    {
        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        /// <value>
        /// The shape.
        /// </value>
        public Shape Shape
        {
            get { return (Shape)GetValue(ShapeProperty); }
            set { SetValue(ShapeProperty, value); }
        }

        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.Register("Shape", typeof(Shape), typeof(LegendItem), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public SeriesBase Series
        {
            get { return (SeriesBase)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// The series property
        /// </summary>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(SeriesBase), typeof(LegendItem), new PropertyMetadata(null,OnSeriesChanged));

        /// <summary>
        /// Called when [series changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnSeriesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var legendItem = sender as LegendItem;
            if (legendItem != null) legendItem.SeriesChanged(args);
        }

        /// <summary>
        /// Serieses the changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void SeriesChanged(DependencyPropertyChangedEventArgs args)
        {
            Binding binding = new Binding {Source = Series};
            if (Series is FillSeriesBase)
                binding.Path = new PropertyPath("Fill");
            else
                binding.Path = new PropertyPath("Stroke");                    
            BindingOperations.SetBinding(this, LegendItem.IconColorProperty, binding);

            binding = new Binding {Source = Series, Path = new PropertyPath("Label")};
            BindingOperations.SetBinding(this, LegendItem.LabelProperty, binding);
          
        }

        /// <summary>
        /// Gets or sets the color of the icon.
        /// </summary>
        /// <value>
        /// The color of the icon.
        /// </value>
        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        /// <summary>
        /// The icon color property
        /// </summary>
        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register("IconColor", typeof(Brush), typeof(LegendItem), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public object Label
        {
            get { return (object)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// The label property
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(object), typeof(LegendItem), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets a value indicating whether [show icon].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show icon]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        /// <summary>
        /// The show icon property
        /// </summary>
        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(LegendItem), new PropertyMetadata(null));


        
    }
}
