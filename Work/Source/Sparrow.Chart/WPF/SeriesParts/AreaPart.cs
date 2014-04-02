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
    public class AreaPart : FillPartBase
    {
        internal Point StartPoint;
        internal Point AreaStartPoint;
        internal Point AreaEndPoint;
        internal Point EndPoint;
        internal Path AreaPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaPart"/> class.
        /// </summary>
        public AreaPart()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaPart"/> class.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="areaStartPoint">The area start point.</param>
        /// <param name="areaEndPoint">The area end point.</param>
        /// <param name="endPoint">The end point.</param>
        public AreaPart(Point startPoint, Point areaStartPoint, Point areaEndPoint, Point endPoint)
        {
            this.StartPoint = startPoint;
            this.AreaStartPoint = areaStartPoint;
            this.AreaEndPoint = areaEndPoint;
            this.EndPoint = endPoint;
            this.X1 = startPoint.X;
            this.Y1 = startPoint.Y;
            this.X2 = endPoint.X;
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
            AreaPath = new Path();
            PathFigure figure = new PathFigure();
            LineSegment startLineSegment = new LineSegment();
            LineSegment areaEndLineSegment = new LineSegment();
            LineSegment endLineSegment = new LineSegment();
            PathGeometry pathGeometry = new PathGeometry();
            figure.StartPoint = StartPoint;            
            startLineSegment.Point = AreaStartPoint;
            endLineSegment.Point = EndPoint;
            areaEndLineSegment.Point = AreaEndPoint;
            figure.Segments.Add(startLineSegment);
            figure.Segments.Add(areaEndLineSegment);
            figure.Segments.Add(endLineSegment);
            pathGeometry.Figures = new PathFigureCollection() { figure };
            AreaPath.Data = pathGeometry;
            SetBindingForStrokeandStrokeThickness(AreaPath);
            return AreaPath;
        }

        /// <summary>
        /// Refresh the Series Part
        /// </summary>
        public override void Refresh()
        {
            if (AreaPath != null)
            {
                PathFigure figure = new PathFigure();
                LineSegment startLineSegment = new LineSegment();
                LineSegment areaEndLineSegment = new LineSegment();
                LineSegment endLineSegment = new LineSegment();
                PathGeometry pathGeometry = new PathGeometry();
                figure.StartPoint = StartPoint;
                startLineSegment.Point = AreaStartPoint;
                endLineSegment.Point = EndPoint;
                areaEndLineSegment.Point = AreaEndPoint;
                figure.Segments.Add(startLineSegment);
                figure.Segments.Add(areaEndLineSegment);
                figure.Segments.Add(endLineSegment);
                pathGeometry.Figures = new PathFigureCollection() { figure };
                AreaPath.Data = pathGeometry;
            }
        }

    }
}
