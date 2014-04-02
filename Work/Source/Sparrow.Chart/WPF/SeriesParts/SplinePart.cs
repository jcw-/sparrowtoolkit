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
    
    public class SplinePart : LinePartBase
    {
        internal Point StartPoint;
        internal Point FirstControlPoint;
        internal Point EndControlPoint;
        internal Point EndPoint;
        internal Path SplinePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplinePart"/> class.
        /// </summary>
        public SplinePart()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplinePart"/> class.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="firstControlPoint">The first control point.</param>
        /// <param name="endControlPoint">The end control point.</param>
        /// <param name="endPoint">The end point.</param>
        public SplinePart(Point startPoint,Point firstControlPoint,Point endControlPoint,Point endPoint)
        {
            this.X1 = startPoint.X;
            this.Y1 = startPoint.Y;
            this.X2 = endPoint.X;
            this.Y2 = endPoint.Y;
            this.StartPoint = startPoint;
            this.FirstControlPoint = firstControlPoint;
            this.EndControlPoint = endControlPoint;
            this.EndPoint = endPoint;
        }

        /// <summary>
        /// Creates the part.
        /// </summary>
        /// <returns>
        /// UIElement
        /// </returns>
        public override UIElement CreatePart()
        {
            SplinePath = new Path();
            PathFigure figure = new PathFigure();
            BezierSegment bezierPoints = new BezierSegment();
            PathGeometry pathGeometry = new PathGeometry();
            figure.StartPoint = StartPoint;
            bezierPoints.Point1 = FirstControlPoint;
            bezierPoints.Point2 = EndControlPoint;
            bezierPoints.Point3 = EndPoint;
            figure.Segments.Add(bezierPoints);
            pathGeometry.Figures = new PathFigureCollection() { figure };
            SplinePath.Data = pathGeometry;
            SetBindingForStrokeandStrokeThickness(SplinePath);
            return SplinePath;
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public override void Refresh()
        {
            if (SplinePath != null)
            {
                PathFigure figure = new PathFigure();
                BezierSegment bezierPoints = new BezierSegment();
                PathGeometry pathGeometry = new PathGeometry();
                figure.StartPoint = StartPoint;
                bezierPoints.Point1 = FirstControlPoint;
                bezierPoints.Point2 = EndControlPoint;
                bezierPoints.Point3 = EndPoint;
                figure.Segments.Add(bezierPoints);
                pathGeometry.Figures = new PathFigureCollection() { figure };
                SplinePath.Data = pathGeometry;
            }
        }
    }
}
