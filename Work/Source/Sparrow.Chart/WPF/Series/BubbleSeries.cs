using System.Collections.Generic;
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
    public class BubbleSeries : FillSeriesBase
    {
        internal List<double> SizeValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="BubbleSeries"/> class.
        /// </summary>
        public BubbleSeries()
        {
            BubblePoints = new PointCollection();
            IsFill = true;
            SizeValues = new List<double>();
        }

        /// <summary>
        /// Generates the datas.
        /// </summary>
        public override void GenerateDatas()
        {
            BubblePoints.Clear();
            if (!IsPointsGenerated)
                Parts.Clear();
            Point endPoint = new Point(0, 0);
            Point startPoint = new Point(0, 0);            
            if (PointsSource != null)
                SizeValues = this.GetReflectionValues(this.SizePath, PointsSource, SizeValues, false);

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
                        BubblePoints.Add(linePoint);
                        oldPoint = point;
                    }
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!IsPointsGenerated)
                    {
                        for (int i = 0; i < BubblePoints.Count; i++)
                        {
                            ScatterPart scatterPart = new ScatterPart(BubblePoints[i]);
                            scatterPart.Size = SizeValues[i];
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
                            part.X1 = BubblePoints[i].X;
                            part.Y1 = BubblePoints[i].Y;
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
        /// Gets or sets the bubble points.
        /// </summary>
        /// <value>
        /// The bubble points.
        /// </value>
        public PointCollection BubblePoints
        {
            get { return (PointCollection)GetValue(BubblePointsPointsProperty); }
            set { SetValue(BubblePointsPointsProperty, value); }
        }

        /// <summary>
        /// The bubble points points property
        /// </summary>
        public static readonly DependencyProperty BubblePointsPointsProperty =
            DependencyProperty.Register("BubblePointsPoints", typeof(PointCollection), typeof(BubbleSeries), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the size path.
        /// </summary>
        /// <value>
        /// The size path.
        /// </value>
        public string SizePath
        {
            get { return (string)GetValue(SizePathProperty); }
            set { SetValue(SizePathProperty, value); }
        }

        /// <summary>
        /// The size path property
        /// </summary>
        public static readonly DependencyProperty SizePathProperty =
            DependencyProperty.Register("SizePath", typeof(string), typeof(BubbleSeries), new PropertyMetadata(string.Empty));

    }
}
