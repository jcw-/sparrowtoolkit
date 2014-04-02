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
    /// StepLine Series for Sparrow Chart
    /// </summary>
    public class StepLineSeries : LineSeriesBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="StepLineSeries"/> class.
        /// </summary>
        public StepLineSeries()
        {            
            LinePoints = new PointCollection(); 
        }

        /// <summary>
        /// Generates the datas.
        /// </summary>
        override public void GenerateDatas()
        {
            LinePoints.Clear();
            if (!IsPointsGenerated)
                Parts.Clear();
            if (this.Points != null && this.SeriesContainer != null)
            {
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint() { XValue = double.MinValue, YValue = double.MinValue };
                IntializePoints();
                for (int i = 0; i < this.Points.Count; i++)
                {
                    ChartPoint point = this.Points[i];
                    ChartPoint step = new ChartPoint();
                    if (!(i == this.Points.Count - 1))
                        step = this.Points[i + 1];
                    if (CheckValuePoint(oldPoint, point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        LinePoints.Add(linePoint);
                        if (!(i == this.Points.Count - 1))
                        {
                            Point stepPoint = NormalizePoint(new Point(point.XValue, step.YValue));
                            LinePoints.Add(stepPoint);
                        }
                    }
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!IsPointsGenerated)
                    {
                        if (!this.UseSinglePart)
                        {
                            for (int i = 0; i < LinePoints.Count - 2; i++)
                            {
                                StepLinePart stepLinePart = new StepLinePart(LinePoints[i], LinePoints[i + 1], LinePoints[i + 2]);
                                SetBindingForStrokeandStrokeThickness(stepLinePart);
                                this.Parts.Add(stepLinePart);
                            }
                        }
                        else
                        {
                            LineSinglePart stepLinePart = new LineSinglePart();
                            stepLinePart.LinePoints = this.LinePoints;
                            SetBindingForStrokeandStrokeThickness(stepLinePart);
                            this.Parts.Add(stepLinePart);
                        }
                        IsPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        if (!this.UseSinglePart)
                        {
                            foreach (StepLinePart part in this.Parts)
                            {
                                part.StartPoint = LinePoints[i];
                                part.StepPoint = LinePoints[i + 1];
                                part.EndPoint = LinePoints[i + 2];
                                part.Refresh();
                                i++;
                            }
                        }
                        else
                        {
                            foreach (LineSinglePart part in this.Parts)
                            {
                                part.LinePoints = this.LinePoints;
                                part.Refresh();
                                i++;
                            }
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
            return new StepLineContainer();
        }

        /// <summary>
        /// Gets or sets the line points.
        /// </summary>
        /// <value>
        /// The line points.
        /// </value>
        public PointCollection LinePoints
        {
            get { return (PointCollection)GetValue(LinePointsProperty); }
            set { SetValue(LinePointsProperty, value); }
        }

        /// <summary>
        /// The line points property
        /// </summary>
        public static readonly DependencyProperty LinePointsProperty =
            DependencyProperty.Register("LinePoints", typeof(PointCollection), typeof(StepLineSeries), new PropertyMetadata(null));
    
    }      
}
