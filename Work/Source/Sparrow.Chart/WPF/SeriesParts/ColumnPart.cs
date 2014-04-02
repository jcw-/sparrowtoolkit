using System.Windows;
#if !WINRT
using System.Windows.Shapes;
using System.Windows.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
#endif


namespace Sparrow.Chart
{
    public class ColumnPart : FillPartBase
    {       
        internal double PartWidth;
        internal double PartHeight;
        internal double ColumnMargin;
        internal Rectangle RectPart;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnPart"/> class.
        /// </summary>
        public ColumnPart()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnPart"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        public ColumnPart(double x, double y,double x2,double y2)
        {
            this.X1 = x;
            this.Y1 = y;
            this.X2 = x2;
            this.Y2 = y2;
        }


        /// <summary>
        /// Create a visual for single Series Part
        /// </summary>
        /// <returns>
        /// UIElement
        /// </returns>
        public override UIElement CreatePart()
        {
            Rect rect = new Rect(new Point(X1, Y1), new Point(X2, Y2));
            RectPart = new Rectangle {Height = rect.Height, Width = rect.Width};
            RectPart.SetValue(Canvas.LeftProperty, rect.X);
            RectPart.SetValue(Canvas.TopProperty, rect.Y);

            SetBindingForStrokeandStrokeThickness(RectPart);
            return RectPart;
        }

        /// <summary>
        /// Refresh the Series Part
        /// </summary>
        public override void Refresh()
        {
            if (RectPart != null)
            {
                Rect rect = new Rect(new Point(X1, Y1), new Point(X2, Y2));
                RectPart.Height = rect.Height;
                RectPart.Width = rect.Width;
                RectPart.SetValue(Canvas.LeftProperty, rect.X);
                RectPart.SetValue(Canvas.TopProperty, rect.Y);
            }
        }
    }
}
