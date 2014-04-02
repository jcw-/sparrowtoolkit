using System.Windows;
#if !WINRT
using System.Windows.Data;
using System.Windows.Shapes;
using System.Windows.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
#endif

namespace Sparrow.Chart
{
    public class ScatterPart : FillPartBase
    {
        internal Ellipse Ellipse;

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        /// <summary>
        /// The size property
        /// </summary>
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(ScatterPart), new PropertyMetadata(0d));


        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterPart"/> class.
        /// </summary>
        public ScatterPart()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterPart"/> class.
        /// </summary>
        /// <param name="centerPoint">The center point.</param>
        public ScatterPart(Point centerPoint)
        {
            this.X1 = centerPoint.X;
            this.X2 = centerPoint.X;
            this.Y1 = centerPoint.Y;
            this.Y2 = centerPoint.Y;
        }

        /// <summary>
        /// Create a visual for single Series Part
        /// </summary>
        /// <returns>
        /// UIElement
        /// </returns>
        public override UIElement CreatePart()
        {
            Ellipse = new Ellipse();            
            Binding heightBinding = new Binding {Path = new PropertyPath("Size"), Source = this};
            Ellipse.SetBinding(Ellipse.HeightProperty, heightBinding);
            Binding widthBinding = new Binding {Path = new PropertyPath("Size"), Source = this};
            Ellipse.SetBinding(Ellipse.WidthProperty, widthBinding);

            Canvas.SetLeft(Ellipse, X1 - (Ellipse.Width / 2));
            Canvas.SetTop(Ellipse, Y1 - (Ellipse.Height / 2));

            SetBindingForStrokeandStrokeThickness(Ellipse);
            return Ellipse;
        }

        /// <summary>
        /// Refresh the Series Part
        /// </summary>
        public override void Refresh()
        {
            if (Ellipse != null)
            {
                Canvas.SetLeft(Ellipse, X1 - (Ellipse.Width / 2));
                Canvas.SetTop(Ellipse, Y1 - (Ellipse.Height / 2));
            }
        }

    }
}
