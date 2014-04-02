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
    public class FillPartBase : LinePartBase
    {
        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// The fill property
        /// </summary>
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(FillPartBase), new PropertyMetadata(null));

        /// <summary>
        /// </summary>
        /// <param name="shape"></param>
        protected override void SetBindingForStrokeandStrokeThickness(Shape shape)
        {
            Binding fillBinding = new Binding {Path = new PropertyPath("Fill"), Source = this};
            shape.SetBinding(Shape.FillProperty, fillBinding);
            base.SetBindingForStrokeandStrokeThickness(shape);
        }

    }
}
