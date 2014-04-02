using System;
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
    public class ColumnSeries : FillSeriesBase
    {        
        internal PointCollection StartEndPoints;
        internal List<Rect> Rects;

        /// <summary>
        /// Generates the datas.
        /// </summary>
        override public void GenerateDatas()
        {
            ColumnPoints.Clear();

            if (this.Points != null && this.SeriesContainer != null)
            {
                if (!IsPointsGenerated)
                    Parts.Clear();
                StartEndPoints = new PointCollection();
                Rects = new List<Rect>();
                CalculateMinAndMax();
                ChartPoint oldPoint = new ChartPoint() { XValue = double.MinValue, YValue = double.MinValue };
                IntializePoints();
                Point startAndEndPoint = CalculateColumnSeriesInfo();
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint, point))
                    {
                        Point linePoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        Point startPoint = NormalizePoint(new Point(point.XValue + startAndEndPoint.X, point.YValue));
                        Point endPoint = NormalizePoint(new Point(point.XValue + startAndEndPoint.Y, YMin));
                        StartEndPoints.Add(startPoint);
                        StartEndPoints.Add(endPoint);
                        ColumnPoints.Add(linePoint);
                        Rects.Add(new Rect(startPoint, endPoint));
                        oldPoint = point;
                    }
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    //if (!UseSinglePart)
                    //{
                    if (!IsPointsGenerated)
                    {
                        for (int i = 0; i <= this.StartEndPoints.Count - 2; i += 2)
                        {
                            if (CheckValue(StartEndPoints[i].X) && CheckValue(StartEndPoints[i].Y) && CheckValue(StartEndPoints[i + 1].X) && CheckValue(StartEndPoints[i + 1].Y))
                            {
                                ColumnPart columnPart = new ColumnPart(StartEndPoints[i].X, StartEndPoints[i].Y, StartEndPoints[i + 1].X, StartEndPoints[i + 1].Y);
                                SetBindingForStrokeandStrokeThickness(columnPart);
                                this.Parts.Add(columnPart);
                            }
                            //}
                            //else
                            //{
                            //    LineSinglePart singlePart = new LineSinglePart(this.ColumnPoints);
                            //    SetBindingForStrokeandStrokeThickness(singlePart);
                            //    this.Parts.Add(singlePart);
                            //}
                        }
                        IsPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (ColumnPart part in this.Parts)
                        {
                            if (CheckValue(StartEndPoints[i].X) && CheckValue(StartEndPoints[i].Y) && CheckValue(StartEndPoints[i + 1].X) && CheckValue(StartEndPoints[i + 1].Y))
                            {
                                part.X1 = StartEndPoints[i].X;
                                part.Y1 = StartEndPoints[i].Y;
                                part.X2 = StartEndPoints[i + 1].X;
                                part.Y2 = StartEndPoints[i + 1].Y;
                                part.Refresh();
                            }
                            i += 2;
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
        /// Initializes a new instance of the <see cref="ColumnSeries"/> class.
        /// </summary>
        public ColumnSeries()
        {
            ColumnPoints = new PointCollection();
            IsFill = true;
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <returns></returns>
        internal override SeriesContainer CreateContainer()
        {
            return new ColumnContainer();
        }

        /// <summary>
        /// Calculates the column series info.
        /// </summary>
        /// <returns></returns>
        public Point CalculateColumnSeriesInfo()
        {
            double width = 1 - SparrowChart.GetSeriesMarginPercentage(this);
            double mininumWidth = double.MaxValue;
            int position = Chart.ColumnSeries.IndexOf(this) + 1;
            int count = Chart.ColumnSeries.Count;
            foreach (SeriesBase series in Chart.Series)
            {
                List<double> values = series.XValues as List<double>;
                if (values != null)
                {
                    for (int i = 1; i < values.Count; i++)
                    {
                        double delta = values[i] - values[i - 1];
                        if (delta != 0)
                        {
                            mininumWidth = Math.Min(mininumWidth, delta);
                        }
                    }
                }
            }
            mininumWidth = ((mininumWidth == double.MaxValue || mininumWidth >= 1 || mininumWidth < 0) ? 1 : mininumWidth);
            double per = mininumWidth * width / count;
            double start = per * (position - 1) - mininumWidth * width / 2;
            double end = start + per;
            return new Point(start,end);
            //}
        }


        /// <summary>
        /// Gets or sets the column points.
        /// </summary>
        /// <value>
        /// The column points.
        /// </value>
        public PointCollection ColumnPoints
        {
            get { return (PointCollection)GetValue(ColumnPointsProperty); }
            set { SetValue(ColumnPointsProperty, value); }
        }

        /// <summary>
        /// The column points property
        /// </summary>
        public static readonly DependencyProperty ColumnPointsProperty =
            DependencyProperty.Register("ColumnPoints", typeof(PointCollection), typeof(ColumnSeries), new PropertyMetadata(null));
    }
}
