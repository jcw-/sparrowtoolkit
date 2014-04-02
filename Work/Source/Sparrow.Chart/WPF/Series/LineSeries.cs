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
    /// Line Series for Sparrow Chart
    /// </summary>
    public class LineSeries : LineSeriesBase
    {
        /// <summary>
        /// Generates the datas.
        /// </summary>
        override public void GenerateDatas()
        {            
            if (Points != null && SeriesContainer != null)
            {
                LinePoints = new PointCollection();
                if (!IsPointsGenerated)
                    Parts.Clear();
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint { XValue = double.MinValue, YValue = double.MinValue };
                IntializePoints();
                foreach (ChartPoint point in Points)
                {
                    if (CheckValuePoint(oldPoint,point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        LinePoints.Add(linePoint);
                        oldPoint = point;
                    }
                }
                if (RenderingMode == RenderingMode.Default)
                {
                    if (!IsPointsGenerated)
                    {
                        if (!UseSinglePart)
                        {
                            for (int i = 0; i < LinePoints.Count - 1; i++)
                            {
                                if (CheckValue(LinePoints[i].X) && CheckValue(LinePoints[i].Y) && CheckValue(LinePoints[i + 1].X) && CheckValue(LinePoints[i + 1].Y))
                                {
                                    LinePart linePart = new LinePart(LinePoints[i], LinePoints[i + 1]);
                                    SetBindingForStrokeandStrokeThickness(linePart);
                                    Parts.Add(linePart);
                                }
                            }
                        }
                        else
                        {
                            LineSinglePart singlePart = new LineSinglePart(LinePoints);
                            SetBindingForStrokeandStrokeThickness(singlePart);
                            Parts.Add(singlePart);
                        }
                        IsPointsGenerated = true;
                    }
                    else
                    {
                        int i=0;
                        if (!UseSinglePart)
                        {
                            foreach (LinePart part in Parts)
                            {
                                if (CheckValue(LinePoints[i].X) && CheckValue(LinePoints[i].Y) && CheckValue(LinePoints[i + 1].X) && CheckValue(LinePoints[i + 1].Y))
                                {
                                    part.X1 = LinePoints[i].X;
                                    part.Y1 = LinePoints[i].Y;
                                    part.X2 = LinePoints[i + 1].X;
                                    part.Y2 = LinePoints[i + 1].Y;
                                    part.Refresh();
                                }
                                i++;
                            }
                        }
                        else
                        {
                            foreach (LineSinglePart part in Parts)
                            {
                                part.LinePoints = LinePoints;
                                part.Refresh();
                                i++;
                            }
                        }
                    }
                }
            }

            if (this.SeriesContainer != null)
                this.SeriesContainer.Invalidate();
            IsRefreshed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries"/> class.
        /// </summary>
        public LineSeries()
        {
            LinePoints = new PointCollection();
        }

        internal override SeriesContainer CreateContainer()
        {
            return new LineContainer();
        }


        public PointCollection LinePoints
        {
            get { return (PointCollection)GetValue(LinePointsProperty); }
            set { SetValue(LinePointsProperty, value); }
        }

        public static readonly DependencyProperty LinePointsProperty =
            DependencyProperty.Register("LinePoints", typeof(PointCollection), typeof(LineSeries), new PropertyMetadata(null));

    }

}
