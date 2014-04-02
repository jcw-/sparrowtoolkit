using System;
using System.Windows;
using System.Linq;
#if !WINRT
using System.Windows.Media;
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
    public class PieSeries : PieSeriesBase
    {
        /// <summary>
        /// Generates the datas.
        /// </summary>
        override public void GenerateDatas()
        {            
            if (Points != null && SeriesContainer != null)
            {
                PiePoints = new PointCollection();
                var centerPoint = (SeriesContainer.Collection.ActualHeight <= SeriesContainer.Collection.ActualWidth)
                    ? new Point(SeriesContainer.Collection.ActualHeight/2, SeriesContainer.Collection.ActualHeight/2)
                    : new Point(SeriesContainer.Collection.ActualWidth/2, SeriesContainer.Collection.ActualWidth/2);
                PiePoints.Add(centerPoint);
                if (PointsSource == null && Points.Count > 0 )
                {
                    TotalYValues = Points.Sum(point => point.YValue);
                }
                
                if (!IsPointsGenerated)
                    Parts.Clear();
                 double rad = (SeriesContainer.Collection.ActualHeight <= SeriesContainer.Collection.ActualWidth)
                    ? SeriesContainer.Collection.ActualHeight
                    : SeriesContainer.Collection.ActualWidth;
                double width = SeriesContainer.Collection.ActualWidth;
                double height = SeriesContainer.Collection.ActualHeight;
                if (RenderingMode == RenderingMode.Default)
                {
                    var angle = -Math.PI / 2;  
                    if (!IsPointsGenerated)
                    {
                                              
                        foreach (var point in Points)
                        {
                            var path = new Path();
                            Geometry geometry = new PathGeometry();
                            var x = Math.Cos(angle) * rad / 2 + rad / 2;
                            var y = Math.Sin(angle) * rad / 2 + rad / 2;
                            var lingeSeg = new LineSegment(new Point(x, y), true);
                            double angleShare = (point.YValue/TotalYValues)*360;
                            angle += DegreeToRadian(angleShare);
                            x = Math.Cos(angle) * rad / 2 + rad / 2;
                            y = Math.Sin(angle) * rad / 2 + rad / 2;
                            var arcSeg = new ArcSegment(new Point(x, y), new Size(rad / 2, rad / 2), angleShare,
                                angleShare > 180, SweepDirection.Clockwise, false);
                            var lingeSeg2 = new LineSegment(new Point(rad / 2, rad / 2), true);
                            var fig = new PathFigure(new Point(rad / 2, rad / 2),
                                new PathSegment[] {lingeSeg, arcSeg, lingeSeg2}, false);
                            ((PathGeometry) geometry).Figures.Add(fig);
                             var brush   = Brushes[Points.IndexOf(point) % (Brushes.Count)];

                            path.Data = geometry;
                            Parts.Add(new PiePart(path) { Stroke = brush });
                        }
                        
                        IsPointsGenerated = true;
                    }
                    else
                    {
                        foreach (var point in Points)
                        {
                            Geometry geometry = new PathGeometry();
                            var x = Math.Cos(angle) * rad / 2 + rad / 2;
                            var y = Math.Sin(angle) * rad / 2 + rad / 2;
                            var lingeSeg = new LineSegment(new Point(x, y), true);
                            double angleShare = (point.YValue / TotalYValues) * 360;
                            angle += DegreeToRadian(angleShare);
                            x = Math.Cos(angle) * rad / 2 + rad / 2;
                            y = Math.Sin(angle) * rad / 2 + rad / 2;
                            var arcSeg = new ArcSegment(new Point(x, y), new Size(rad / 2, rad / 2), angleShare,
                                angleShare > 180, SweepDirection.Clockwise, false);
                            var lingeSeg2 = new LineSegment(new Point(rad / 2, rad / 2), true);
                            var fig = new PathFigure(new Point(rad / 2, rad / 2),
                                new PathSegment[] { lingeSeg, arcSeg, lingeSeg2 }, false);
                            ((PathGeometry)geometry).Figures.Add(fig);
                            ((Parts[Points.IndexOf(point)] as PiePart).Slice as Path).Data = geometry;
                        }
                    }
                }
            }
            else
            {
              Parts.Clear();
            }

            if (this.SeriesContainer != null)
                this.SeriesContainer.Invalidate();
            IsRefreshed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries"/> class.
        /// </summary>
        public PieSeries()
        {
            PiePoints = new PointCollection();
        }

        internal override SeriesContainer CreateContainer()
        {
            return new PieContainer();
        }


        public PointCollection PiePoints
        {
            get { return (PointCollection)GetValue(PiePointsProperty); }
            set { SetValue(PiePointsProperty, value); }
        }

        public static readonly DependencyProperty PiePointsProperty =
            DependencyProperty.Register("PiePoints", typeof(PointCollection), typeof(PieSeries), new PropertyMetadata(null));
    }
}
