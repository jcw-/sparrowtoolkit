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
    /// AreaSeries for SparrowChart
    /// </summary>
    public class AreaSeries : FillSeriesBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaSeries"/> class.
        /// </summary>
        public AreaSeries()
        {
            this.AreaPoints = new PointCollection();            
            this.IsFill = true;
        }

        /// <summary>
        /// Generates the datas.
        /// </summary>
        override public void GenerateDatas()
        {
            AreaPoints.Clear();
            if (!IsPointsGenerated)
                Parts.Clear();
            Point endPoint;
            Point startPoint=new Point(0,0);
            int index = 0;
            if (this.Points != null && this.SeriesContainer != null && this.Points.Count > 1)
            {
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint() { XValue = double.MinValue, YValue = double.MinValue };
                IntializePoints();
                AreaPoints.Add(startPoint);
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint,point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        if (index == 0)
                            linePoint.X = linePoint.X - this.StrokeThickness;
                        AreaPoints.Add(linePoint);
                    }
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!IsPointsGenerated)
                    {
                        for (int i = 0; i < AreaPoints.Count - 2; i++)
                        {
                            startPoint = NormalizePoint(new Point(this.Points[i].XValue, YMin));
                            endPoint = NormalizePoint(new Point(this.Points[i + 1].XValue, YMin));
                            AreaPart areaPart = new AreaPart(AreaPoints[i + 1], startPoint, endPoint, AreaPoints[i + 2]);
                            SetBindingForStrokeandStrokeThickness(areaPart);
                            this.Parts.Add(areaPart);

                        }
                        IsPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (AreaPart part in this.Parts)
                        {
                            startPoint = NormalizePoint(new Point(this.Points[i].XValue, YMin));
                            endPoint = NormalizePoint(new Point(this.Points[i + 1].XValue, YMin));
                            part.StartPoint = AreaPoints[i + 1];
                            part.AreaStartPoint = startPoint;
                            part.AreaEndPoint = endPoint;
                            part.EndPoint = AreaPoints[i + 2];
                            part.Refresh();
                            i++;
                        }
                    }
                }
                endPoint = NormalizePoint(new Point(this.Points[this.Points.Count - 1].XValue, YMin));
                startPoint = NormalizePoint(new Point(this.Points[0].XValue, YMin));
                startPoint.X = startPoint.X - this.StrokeThickness;                
                if (AreaPoints.Count > 0)
                {
                    AreaPoints[0] = startPoint;
                    AreaPoints.Add(endPoint);
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
            return new AreaContainer();
        }


        /// <summary>
        /// Gets or sets the area points.
        /// </summary>
        /// <value>
        /// The area points.
        /// </value>
        public PointCollection AreaPoints
        {
            get { return (PointCollection)GetValue(AreaPointsProperty); }
            set { SetValue(AreaPointsProperty, value); }
        }

        /// <summary>
        /// The area points property
        /// </summary>
        public static readonly DependencyProperty AreaPointsProperty =
            DependencyProperty.Register("AreaPoints", typeof(PointCollection), typeof(AreaSeries), new PropertyMetadata(null));    

    }

   
}

