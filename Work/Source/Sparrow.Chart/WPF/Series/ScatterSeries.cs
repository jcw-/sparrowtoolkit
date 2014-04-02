using System.Windows;
#if !WINRT
using System.Windows.Data;
using System.Windows.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
#endif


namespace Sparrow.Chart
{
    /// <summary>
    /// ScatterSeries for SparrowChart
    /// </summary>
    public class ScatterSeries : FillSeriesBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterSeries"/> class.
        /// </summary>
        public ScatterSeries()
        {
            ScatterPoints = new PointCollection();
            IsFill = true;
        }

        /// <summary>
        /// Generates the datas.
        /// </summary>
        public override void GenerateDatas()
        {
            ScatterPoints.Clear();
            if (!IsPointsGenerated)
                Parts.Clear();
            Point endPoint = new Point(0, 0);
            Point startPoint = new Point(0, 0);

            if (this.Points != null && this.SeriesContainer != null && this.Points.Count > 0)
            {
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint() { XValue = double.MinValue, YValue = double.MinValue };
                IntializePoints();                
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint, point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));                       
                        ScatterPoints.Add(linePoint);
                        oldPoint = point;
                    }
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!IsPointsGenerated)
                    {
                        for (int i = 0; i < ScatterPoints.Count; i++)
                        {
                            ScatterPart scatterPart = new ScatterPart(ScatterPoints[i]);
                            Binding sizeBinding = new Binding();
                            sizeBinding.Path = new PropertyPath("ScatterSize");
                            sizeBinding.Source = this;
                            scatterPart.SetBinding(ScatterPart.SizeProperty, sizeBinding);
                            SetBindingForStrokeandStrokeThickness(scatterPart);
                            this.Parts.Add(scatterPart);
                        }
                        IsPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (ScatterPart part in this.Parts)
                        {
                            part.X1 = ScatterPoints[i].X;
                            part.Y1 = ScatterPoints[i].Y;
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
            return new ScatterContainer();
        }

        /// <summary>
        /// Gets or sets the scatter points.
        /// </summary>
        /// <value>
        /// The scatter points.
        /// </value>
        public PointCollection ScatterPoints
        {
            get { return (PointCollection)GetValue(ScatterPointsProperty); }
            set { SetValue(ScatterPointsProperty, value); }
        }

        /// <summary>
        /// The scatter points property
        /// </summary>
        public static readonly DependencyProperty ScatterPointsProperty =
            DependencyProperty.Register("ScatterPoints", typeof(PointCollection), typeof(ScatterSeries), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the size of the scatter.
        /// </summary>
        /// <value>
        /// The size of the scatter.
        /// </value>
        public double ScatterSize
        {
            get { return (double)GetValue(ScatterSizeProperty); }
            set { SetValue(ScatterSizeProperty, value); }
        }

        /// <summary>
        /// The scatter size property
        /// </summary>
        public static readonly DependencyProperty ScatterSizeProperty =
            DependencyProperty.Register("ScatterSize", typeof(double), typeof(ScatterSeries), new PropertyMetadata(30d));

           
    }
}
