using System.Linq;
using System.Windows;
#if !WINRT
using System.Windows.Media;

#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
#endif

namespace Sparrow.Chart
{
    /// <summary>
    /// Spline Series for Sparrow Charts
    /// </summary>
    public class SplineSeries : LineSeriesBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SplineSeries"/> class.
        /// </summary>
        public SplineSeries()
        {           
            SplinePoints = new PointCollection();
            ControlPoints = new PointCollection();
        }

        /// <summary>
        /// The first control points
        /// </summary>
        internal Point[] FirstControlPoints;
        /// <summary>
        /// The second control points
        /// The second control points
        /// </summary>
        internal Point[] SecondControlPoints;

        /// <summary>
        /// Generates the datas.
        /// </summary>
        override public void GenerateDatas()
        {
            CalculateMinAndMax();
            ChartPoint oldPoint = new ChartPoint() { XValue = double.MinValue, YValue = double.MinValue };
            IntializePoints();
            SplinePoints.Clear();
            if (!IsPointsGenerated)
                Parts.Clear();
            if (this.Points != null && this.SeriesContainer != null)
            {
                CalculateMinAndMax();
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint,point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        this.SplinePoints.Add(linePoint);
                    }
                }
                if (this.SplinePoints.Count > 1)
                    BezierSpline.GetCurveControlPoints(this.SplinePoints.ToArray(), out FirstControlPoints, out SecondControlPoints);
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!IsPointsGenerated)
                    {
                        for (int i = 0; i < this.SplinePoints.Count - 1; i++)
                        {
                            SplinePart splinePart = new SplinePart(SplinePoints[i], FirstControlPoints[i], SecondControlPoints[i], SplinePoints[i + 1]);
                            SetBindingForStrokeandStrokeThickness(splinePart);
                            this.Parts.Add(splinePart);
                        }
                        IsPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (SplinePart part in this.Parts)
                        {
                            part.StartPoint = SplinePoints[i];
                            part.FirstControlPoint = FirstControlPoints[i];
                            part.EndControlPoint = SecondControlPoints[i];
                            part.EndPoint = SplinePoints[i + 1];
                            part.Refresh();
                            i++;
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
        /// Creates the container.
        /// </summary>
        /// <returns></returns>
        internal override SeriesContainer CreateContainer()
        {
            return new SplineContainer();
        }

        /// <summary>
        /// Gets or sets the spline points.
        /// </summary>
        /// <value>
        /// The spline points.
        /// </value>
        public PointCollection SplinePoints
        {
            get { return (PointCollection)GetValue(SplinePointsProperty); }
            set { SetValue(SplinePointsProperty, value); }
        }

        /// <summary>
        /// The spline points property
        /// </summary>
        public static readonly DependencyProperty SplinePointsProperty =
            DependencyProperty.Register("SplinePoints", typeof(PointCollection), typeof(SplineSeries), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the control points.
        /// </summary>
        /// <value>
        /// The control points.
        /// </value>
        public PointCollection ControlPoints
        {
            get { return (PointCollection)GetValue(ControlPointsProperty); }
            set { SetValue(ControlPointsProperty, value); }
        }

        /// <summary>
        /// The control points property
        /// </summary>
        public static readonly DependencyProperty ControlPointsProperty =
            DependencyProperty.Register("ControlPoints", typeof(PointCollection), typeof(SplineSeries), new PropertyMetadata(null));
       
    }
}
