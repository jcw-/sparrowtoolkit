using System.Windows;
#if !WINRT
using System.Windows.Media;

#else
using Windows.UI.Xaml.Media;
using Windows.Foundation;
#endif

namespace Sparrow.Chart
{
    public class HiLoSeries : StockChartBase
    {
        /// <summary>
        /// Generates the datas.
        /// </summary>
        override public void GenerateDatas()
        {
            LowPoints.Clear();
            HighPoints.Clear();
            if (!IsPointsGenerated)
                Parts.Clear();
            if (Points != null && SeriesContainer != null)
            {
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint { XValue = double.MinValue, YValue = double.MinValue };
                IntializePoints();
                int index = 0;
                foreach (ChartPoint point in Points)
                {
                    if (CheckValuePoint(oldPoint,point))
                    {
                        Point highPoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        Point lowPoint = NormalizePoint(new Point(lowPoints[index].XValue, lowPoints[index].YValue));
                        HighPoints.Add(highPoint);
                        LowPoints.Add(lowPoint);
                        oldPoint = point;
                    }
                    index++;
                }
                if (RenderingMode == RenderingMode.Default)
                {
                    if (!IsPointsGenerated)
                    {
                        for (int i = 0; i < HighPoints.Count; i++)
                        {
                            LinePart linePart = new LinePart(HighPoints[i], LowPoints[i]);
                            SetBindingForStrokeandStrokeThickness(linePart);
                            Parts.Add(linePart);
                        }
                        IsPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (LinePart part in Parts)
                        {
                            part.X1 = HighPoints[i].X;
                            part.Y1 = HighPoints[i].Y;
                            part.X2 = LowPoints[i].X;
                            part.Y2 = LowPoints[i].Y;
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
        /// Initializes a new instance of the <see cref="HiLoSeries"/> class.
        /// </summary>
        public HiLoSeries()
        {
            HighPoints = new PointCollection();
            LowPoints = new PointCollection();
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <returns></returns>
        internal override SeriesContainer CreateContainer()
        {
            return new LineContainer();
        }
    }
}
