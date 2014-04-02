using System.Windows;
#if !WINRT
using System.Windows.Shapes;
#else
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
#endif

namespace Sparrow.Chart
{
    public class LinePart : LinePartBase
    {
        internal Line linePart;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinePart"/> class.
        /// </summary>
        public LinePart()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinePart"/> class.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="y2">The y2.</param>
        public LinePart(double x1,double x2,double y1,double y2)
        {
            this.X1 = x1;
            this.X2 = x2;
            this.Y1 = y1;
            this.Y2 = y2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinePart"/> class.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        public LinePart(Point startPoint,Point endPoint)
        {
            this.X1 = startPoint.X;
            this.X2 = endPoint.X;
            this.Y1 = startPoint.Y;
            this.Y2 = endPoint.Y;
        }

        /// <summary>
        /// Create a visual for single Series Part
        /// </summary>
        /// <returns>
        /// UIElement
        /// </returns>
        public override UIElement CreatePart()
        {
            linePart = new Line {X1 = this.X1, X2 = this.X2, Y1 = this.Y1, Y2 = this.Y2};
            SetBindingForStrokeandStrokeThickness(linePart);
            return linePart;
        }

        /// <summary>
        /// Refresh the Series Part
        /// </summary>
        public override void Refresh()
        {
            if (this.linePart != null)
            { 
                linePart.X1 = this.X1;
                linePart.X2 = this.X2;
                linePart.Y1 = this.Y1;
                linePart.Y2 = this.Y2;
            }
        }
    }
}
