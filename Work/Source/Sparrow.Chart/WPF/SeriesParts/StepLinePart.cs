using System.Windows;
#if !WINRT
using System.Windows.Media;
using System.Windows.Shapes;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
#endif

namespace Sparrow.Chart
{
    
    public class StepLinePart : LinePartBase
    {
        internal Point StartPoint;
        internal Point EndPoint;
        internal Point StepPoint;
        internal Polyline Lines;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepLinePart"/> class.
        /// </summary>
        public StepLinePart()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepLinePart"/> class.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="stepPoint">The step point.</param>
        /// <param name="endPoint">The end point.</param>
        public StepLinePart(Point startPoint, Point stepPoint, Point endPoint)
        {
            this.X1 = startPoint.X;
            this.Y1 = startPoint.Y;
            this.X2 = endPoint.X;
            this.Y2 = endPoint.Y;
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.StepPoint = stepPoint;
        }

        /// <summary>
        /// Creates the part.
        /// </summary>
        /// <returns>
        /// UIElement
        /// </returns>
        public override UIElement CreatePart()
        {            
            Lines = new Polyline();
            PointCollection pointsCollection=new PointCollection {StartPoint, StepPoint, EndPoint};
            Lines.Points = pointsCollection;
            SetBindingForStrokeandStrokeThickness(Lines);
            UiElement = Lines;
            return Lines;
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public override void Refresh()
        {
            if (Lines != null)
            {
                PointCollection pointsCollection = new PointCollection {StartPoint, StepPoint, EndPoint};
                Lines.Points = pointsCollection;
            }
        }
        
    }
}
