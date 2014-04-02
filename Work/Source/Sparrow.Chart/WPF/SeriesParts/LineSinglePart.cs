using System.Windows;
#if !WINRT
using System.Windows.Media;
using System.Windows.Shapes;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
#endif

namespace Sparrow.Chart
{
    public class LineSinglePart : LineSinglePartBase
    {
        internal Polyline LinePart;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSinglePart"/> class.
        /// </summary>
        public LineSinglePart()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSinglePart"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        public LineSinglePart(PointCollection points)
        {
            this.LinePoints = points;
        }

        /// <summary>
        /// Create a visual for single Series Part
        /// </summary>
        /// <returns>
        /// UIElement
        /// </returns>
        public override UIElement CreatePart()
        {
            if (this.LinePoints.Count > 0)
            {
                LinePart = new Polyline();
                foreach (var linePoint in LinePoints)
                {
                    LinePart.Points.Add(linePoint);
                }
                SetBindingForStrokeandStrokeThickness(LinePart);
            }
            return LinePart;
        }

        /// <summary>
        /// Refresh the Series Part
        /// </summary>
        public override void Refresh()
        {
            if (LinePart != null)
                LinePart.Points = this.LinePoints;     
        }
    }
}
