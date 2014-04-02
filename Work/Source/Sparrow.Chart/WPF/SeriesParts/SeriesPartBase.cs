using System.Windows;
#if !WINRT
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
#endif


namespace Sparrow.Chart
{    
    public class SeriesPartBase : FrameworkElement
    {

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// The stroke property
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(SeriesPartBase), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// The stroke thickness property
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
          DependencyProperty.Register("StrokeThickness", typeof(double), typeof(SeriesPartBase), new PropertyMetadata(1d));

        /// <summary>
        /// Sets the binding for strokeand stroke thickness.
        /// </summary>
        /// <param name="shape">The shape.</param>
        protected virtual void SetBindingForStrokeandStrokeThickness(Shape shape)
        {
            Binding strokeBinding = new Binding();
            strokeBinding.Source = this;
            strokeBinding.Path = new PropertyPath("Stroke");
            Binding strokeThicknessBinding = new Binding();
            strokeThicknessBinding.Path = new PropertyPath("StrokeThickness");
            strokeThicknessBinding.Source = this;
            shape.SetBinding(Shape.StrokeProperty, strokeBinding);
            shape.SetBinding(Shape.StrokeThicknessProperty, strokeThicknessBinding);
        }


        public UIElement UiElement { get; set; }

        /// <summary>
        /// Creates the part.
        /// </summary>
        /// <returns>UIElement</returns>
        public virtual UIElement CreatePart()
        {
            return null;
        }


        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public virtual void Refresh()
        {
        }
    }
}
