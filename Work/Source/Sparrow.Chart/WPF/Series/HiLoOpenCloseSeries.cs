using System;
using System.Collections.Generic;
using System.Windows;
#if !WINRT
using System.Windows.Media;
using System.Windows.Data;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
#endif

namespace Sparrow.Chart
{
    public class HiLoOpenCloseSeries : StockChartBase
    {

        internal List<double> OpenValues;
        internal List<double> CloseValues;
        internal PointsCollection openPoints;
        internal PointCollection OpenOffPoints;
        internal PointCollection CloseOffPoints;
        internal PointsCollection closePoints;

        /// <summary>
        /// Generates the points from source.
        /// </summary>
        public override void GeneratePointsFromSource()
        {
            base.GeneratePointsFromSource();
            OpenValues = this.GetReflectionValues(this.OpenPath, PointsSource, OpenValues, false);
            CloseValues = this.GetReflectionValues(this.ClosePath, PointsSource, CloseValues, false);
            if (OpenValues != null && OpenValues.Count > 0)
            {
                this.openPoints = GetPointsFromValues(XValues, OpenValues);
            }
            if (CloseValues != null && CloseValues.Count > 0)
            {
                this.closePoints = GetPointsFromValues(XValues, CloseValues);
            }
        }

        /// <summary>
        /// Generates the datas.
        /// </summary>
        public override void GenerateDatas()
        {
            LowPoints.Clear();
            HighPoints.Clear();
            OpenPoints.Clear();
            ClosePoints.Clear();
            if (!IsPointsGenerated)
                Parts.Clear();
            if (this.Points != null && this.SeriesContainer != null)
            {
                CalculateMinAndMax();
                OpenOffPoints = new PointCollection();
                CloseOffPoints = new PointCollection();
                ChartPoint oldPoint = new ChartPoint() { XValue = double.MinValue, YValue = double.MinValue };
                IntializePoints();
                int index = 0;
                Point startAndEndPoint = CalculateSeriesInfo();
                foreach (ChartPoint point in this.Points)
                {
                    if (CheckValuePoint(oldPoint, point))
                    {
                        Point highPoint = NormalizePoint(new Point(point.XValue, point.YValue));
                        Point lowPoint = NormalizePoint(new Point(lowPoints[index].XValue, lowPoints[index].YValue));
                        Point openPoint = NormalizePoint(new Point(openPoints[index].XValue, openPoints[index].YValue));
                        Point closePoint = NormalizePoint(new Point(closePoints[index].XValue, closePoints[index].YValue));
                        Point openOffPoint = NormalizePoint(new Point(openPoints[index].XValue + startAndEndPoint.X, openPoints[index].YValue));
                        Point closeOffPoint = NormalizePoint(new Point(closePoints[index].XValue - startAndEndPoint.X, closePoints[index].YValue));
                        HighPoints.Add(highPoint);
                        LowPoints.Add(lowPoint);
                        OpenPoints.Add(openPoint);
                        ClosePoints.Add(closePoint);
                        OpenOffPoints.Add(openOffPoint);
                        CloseOffPoints.Add(closeOffPoint);
                        oldPoint = point;
                    }
                    index++;
                }
                if (this.RenderingMode == RenderingMode.Default)
                {
                    if (!IsPointsGenerated)
                    {
                        for (int i = 0; i < this.HighPoints.Count; i++)
                        {
                            HiLoOpenClosePart hiLoOpenClosePart = new HiLoOpenClosePart(this.HighPoints[i],this.LowPoints[i],this.ClosePoints[i],this.CloseOffPoints[i],this.OpenPoints[i],this.OpenOffPoints[i]);
                            if (this.openPoints[i].YValue <= this.closePoints[i].YValue)
                                hiLoOpenClosePart.IsBearfill = true;
                            else
                                hiLoOpenClosePart.IsBearfill = false;
                            SetBindingForStrokeandStrokeThickness(hiLoOpenClosePart);
                            this.Parts.Add(hiLoOpenClosePart);
                        }
                        IsPointsGenerated = true;
                    }
                    else
                    {
                        int i = 0;
                        foreach (HiLoOpenClosePart part in this.Parts)
                        {
                            part.Point1 = this.HighPoints[i];
                            part.Point2 = this.LowPoints[i];
                            part.Point3 = this.ClosePoints[i];
                            part.Point4 = this.CloseOffPoints[i];                            
                            part.Point5 = this.OpenPoints[i];
                            part.Point6 = this.OpenOffPoints[i];
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
        /// Initializes a new instance of the <see cref="HiLoOpenCloseSeries"/> class.
        /// </summary>
        public HiLoOpenCloseSeries()
        {
            HighPoints = new PointCollection();
            LowPoints = new PointCollection();
            OpenPoints = new PointCollection();
            ClosePoints = new PointCollection();
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <returns></returns>
        internal override SeriesContainer CreateContainer()
        {
            return new HiLoOpenCloseContainer();
        }

        /// <summary>
        /// Sets the binding for strokeand stroke thickness.
        /// </summary>
        /// <param name="part">The part.</param>
        protected override void SetBindingForStrokeandStrokeThickness(SeriesPartBase part)
        {
            HiLoOpenClosePart hiLoOpenClosePart = part as HiLoOpenClosePart;
            Binding strokeBinding = new Binding {Source = this};
            if (hiLoOpenClosePart.IsBearfill)
                strokeBinding.Path = new PropertyPath("BearFill");
            else
                strokeBinding.Path = new PropertyPath("BullFill");
            Binding strokeThicknessBinding = new Binding {Path = new PropertyPath("StrokeThickness"), Source = this};
            part.SetBinding(SeriesPartBase.StrokeProperty, strokeBinding);
            part.SetBinding(SeriesPartBase.StrokeThicknessProperty, strokeThicknessBinding);            
        }

        /// <summary>
        /// Calculates the series info.
        /// </summary>
        /// <returns></returns>
        internal Point CalculateSeriesInfo()
        {
            double width = 1 - SparrowChart.GetSeriesMarginPercentage(this);
            double mininumWidth = double.MaxValue;
            int position = Chart.HiLoOpenCloseSeries.IndexOf(this) + 1;
            int count = Chart.HiLoOpenCloseSeries.Count;
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
            return new Point(start, end);
            //}
        }

        /// <summary>
        /// Gets or sets the open points.
        /// </summary>
        /// <value>
        /// The open points.
        /// </value>
        public PointCollection OpenPoints
        {
            get { return (PointCollection)GetValue(OpenPointsProperty); }
            set { SetValue(OpenPointsProperty, value); }
        }

        /// <summary>
        /// The open points property
        /// </summary>
        public static readonly DependencyProperty OpenPointsProperty =
            DependencyProperty.Register("OpenPoints", typeof(PointCollection), typeof(HiLoOpenCloseSeries), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the close points.
        /// </summary>
        /// <value>
        /// The close points.
        /// </value>
        public PointCollection ClosePoints
        {
            get { return (PointCollection)GetValue(ClosePointsProperty); }
            set { SetValue(ClosePointsProperty, value); }
        }

        public static readonly DependencyProperty ClosePointsProperty =
            DependencyProperty.Register("ClosePoints", typeof(PointCollection), typeof(HiLoOpenCloseSeries), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the open path.
        /// </summary>
        /// <value>
        /// The open path.
        /// </value>
        public string OpenPath
        {
            get { return (string)GetValue(OpenPathProperty); }
            set { SetValue(OpenPathProperty, value); }
        }

        /// <summary>
        /// The open path property
        /// </summary>
        public static readonly DependencyProperty OpenPathProperty =
            DependencyProperty.Register("OpenPath", typeof(string), typeof(HiLoOpenCloseSeries), new PropertyMetadata(string.Empty));


        /// <summary>
        /// Gets or sets the close path.
        /// </summary>
        /// <value>
        /// The close path.
        /// </value>
        public string ClosePath
        {
            get { return (string)GetValue(ClosePathProperty); }
            set { SetValue(ClosePathProperty, value); }
        }

        /// <summary>
        /// The close path property
        /// </summary>
        public static readonly DependencyProperty ClosePathProperty =
            DependencyProperty.Register("ClosePath", typeof(string), typeof(HiLoOpenCloseSeries), new PropertyMetadata(null));


    }
}

