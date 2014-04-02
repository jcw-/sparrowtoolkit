using System;
using System.Collections.Generic;
#if !WINRT
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
#endif

namespace Sparrow.Chart
{
    /// <summary>
    /// Base for Axis
    /// </summary>
    public abstract class AxisBase : Canvas, IAxis
    {


        /// <summary>
        /// Invalidates the visuals.
        /// </summary>
        internal virtual void InvalidateVisuals()
        {
        }

        protected ResourceDictionary Styles;
        protected Line AxisLine;
        protected List<ContentControl> Labels;
        protected List<Line> MajorTickLines;
        protected List<Line> MinorTickLines;
        protected ContentControl header;
        protected bool IsInitialized;

        private bool _isIntervalCountZero;

        internal ActualType ActualType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisBase"/> class.
        /// </summary>
        public AxisBase()
        {
            this.MLabels = new List<string>();
            this.MLabelValues = new List<double>();
            this.ActualType = ActualType.Double;
            GetStyles();
        }

        /// <summary>
        /// Gets the styles.
        /// </summary>
        protected virtual void GetStyles()
        {
            Styles = new ResourceDictionary()
            {
#if X86
                Source = new Uri(@"/Sparrow.Chart.DirectX2D_x86;component/Themes/Styles.xaml", UriKind.Relative)
#elif X64
                Source = new Uri(@"/Sparrow.Chart.DirectX2D_x64;component/Themes/Styles.xaml", UriKind.Relative)
#elif WPF
#if NET45
                Source = new Uri(@"/Sparrow.Chart.Wpf.45;component/Themes/Styles.xaml", UriKind.Relative)
#elif NET40
                Source = new Uri(@"/Sparrow.Chart.Wpf.40;component/Themes/Styles.xaml", UriKind.Relative)
#else
                Source = new Uri(@"/Sparrow.Chart.Wpf.35;component/Themes/Styles.xaml", UriKind.Relative)
#endif
#elif SILVERLIGHT
#if SL5
                Source = new Uri(@"/Sparrow.Chart.Silverlight.50;component/Themes/Styles.xaml", UriKind.Relative)
#else
                Source = new Uri(@"/Sparrow.Chart.Silverlight.40;component/Themes/Styles.xaml", UriKind.Relative)
#endif
#elif WINRT
                Source = new Uri(@"ms-appx:///Sparrow.Chart.WinRT.45/Themes/Styles.xaml")
#elif WP7
#if NET45
                Source = new Uri(@"/Sparrow.Chart.WP7.45;component/Themes/Styles.xaml", UriKind.Relative)
#else
                Source = new Uri(@"/Sparrow.Chart.WP7.40;component/Themes/Styles.xaml", UriKind.Relative)
#endif
#elif WP8
                Source = new Uri(@"/Sparrow.Chart.WP8.45;component/Themes/Styles.xaml", UriKind.Relative)
#endif
            };

            this.AxisLineStyle = (Style)Styles["axisLineStyle"];
            this.MajorLineStyle = (Style)Styles["majorLineStyle"];
            this.MinorLineStyle = (Style)Styles["minorLineStyle"];
            this.LabelTemplate = (DataTemplate)Styles["axisLabelTemplate"];
            this.CrossLineStyle = (Style)Styles["crossLineStyle"];
            this.MinorCrossLineStyle = (Style)Styles["minorCrossLineStyle"];
        }



        /// <summary>
        /// Gets or sets the zoom offset.
        /// </summary>
        /// <value>
        /// The zoom offset.
        /// </value>
        public double ZoomOffset
        {
            get { return (double)GetValue(ZoomOffsetProperty); }
            set { SetValue(ZoomOffsetProperty, value); }
        }


        /// <summary>
        /// The zoom offset property
        /// </summary>
        public static readonly DependencyProperty ZoomOffsetProperty =
            DependencyProperty.Register("ZoomOffset", typeof(double), typeof(AxisBase), new PropertyMetadata(0d,OnZoomOffsetChanged));

        /// <summary>
        /// Called when [zoom offset changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnZoomOffsetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as AxisBase).ZoomPropertyChanged(args);
        }

        /// <summary>
        /// Gets or sets the zoom coefficient.
        /// </summary>
        /// <value>
        /// The zoom coefficient.
        /// </value>
        public double ZoomCoefficient
        {
            get { return (double)GetValue(ZoomCoefficientProperty); }
            set { SetValue(ZoomCoefficientProperty, value); }
        }

        /// <summary>
        /// The zoom coefficient property
        /// </summary>
        public static readonly DependencyProperty ZoomCoefficientProperty =
            DependencyProperty.Register("ZoomCoefficient", typeof(double), typeof(AxisBase), new PropertyMetadata(1d,OnZoomCoefficientChanged));

        /// <summary>
        /// Called when [zoom coefficient changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnZoomCoefficientChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as AxisBase).ZoomPropertyChanged(args);
        }
        internal void ZoomPropertyChanged(DependencyPropertyChangedEventArgs args)
        {           
            Refresh();
        }

        /// <summary>
        /// Gets or sets the min value.
        /// </summary>
        /// <value>
        /// The min value.
        /// </value>
        public object MinValue
        {
            get { return (object)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        /// The min value property
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(object), typeof(AxisBase), new PropertyMetadata(null,OnMinValueChanged));

        /// <summary>
        /// Called when [min value changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnMinValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as AxisBase).MinValueChnaged(args);
        }

        /// <summary>
        /// Mins the value chnaged.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void MinValueChnaged(DependencyPropertyChangedEventArgs args)
        {
            this.MMinValue =Double.Parse(this.MinValue.ToString());
            this.MStartValue = Double.Parse(this.MinValue.ToString()); 
            if (this.Chart != null && this.Chart.Containers != null)
                Chart.Containers.Refresh();
            RefreshSeries();
        }

        /// <summary>
        /// Called when [max value changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnMaxValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as AxisBase).MaxValueChanged(args);
        }

        /// <summary>
        /// Maxes the value changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void MaxValueChanged(DependencyPropertyChangedEventArgs args)
        {
            this.MMaxValue = Double.Parse(this.MaxValue.ToString());
            if (this.Chart != null && this.Chart.Containers != null)
                Chart.Containers.Refresh();
            RefreshSeries();
        }

        /// <summary>
        /// Called when [interval changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIntervalChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as AxisBase).IntervalChanged(args);
        }

        /// <summary>
        /// Intervals the changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void IntervalChanged(DependencyPropertyChangedEventArgs args)
        {
            switch (ActualType)
            {                 
                case ActualType.Double:
                case ActualType.Category:                    
                    if (!this.Interval.ToString().Contains(":"))
                        this.MInterval = Double.Parse(this.Interval.ToString());
                    else
                        this.MInterval = (DateTime.Now + (TimeSpan)TimeSpan.Parse(Interval.ToString())).ToOADate() - DateTime.Now.ToOADate();
                    break;
                case ActualType.DateTime:
                    this.MInterval = (DateTime.Now + (TimeSpan)TimeSpan.Parse(Interval.ToString())).ToOADate() - DateTime.Now.ToOADate();
                    break;
                default:
                    break;
            }

            if (this.Chart != null && this.Chart.Containers != null)
            {
                Chart.Containers.Refresh();
                Chart.Containers.AxisLinesconatiner.Refresh();
            }
            RefreshSeries();
        }

        /// <summary>
        /// Gets or sets the max value.
        /// </summary>
        /// <value>
        /// The max value.
        /// </value>
        public object MaxValue
        {
            get { return (object)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(object), typeof(AxisBase), new PropertyMetadata(null,OnMaxValueChanged));


        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public object Interval
        {
            get { return (object)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(object), typeof(AxisBase), new PropertyMetadata(null,OnIntervalChanged));

        /// <summary>
        /// Adds the min max.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        internal void AddMinMax(double min, double max)
        {
            if (this.MinValue == null)
            {
                MMinValue = min;
                
                if (!_isStartSet)
                {
                    MStartValue = min;
                    _isStartSet = true;
                }
                ActualMinvalue = min;
            }
            else
            {
                ActualMinvalue = Double.Parse(this.MinValue.ToString());
            }
            
            if (this.MaxValue == null)
            {
                MMaxValue = max;
                ActualMaxvalue = max;
            }
            else
            {
                ActualMaxvalue = Double.Parse(this.MaxValue.ToString());
            }
            
        }   
    
        /// <summary>
        /// Add Interval to Axis
        /// </summary>
        /// <param name="interval">Interval to be added for Axis</param>
        public void AddInterval(double interval)
        {
            if (this.Interval == null)
                MInterval = interval;            
        }

        /// <summary>
        /// Generates the labels.
        /// </summary>
        internal void GenerateLabels()
        {           
            MLabels.Clear();
            MLabelValues.Clear();
            double value = MMinValue;
            for (int i = 0; i <= MIntervalCount; i++)
            {
                //if (value >= m_MinValue && value <= m_MaxValue)
                //{
                    MLabels.Add(GetOriginalLabel(value));
                    MLabelValues.Add(value);                    
                //}                
                value += MInterval;
                if (_isIntervalCountZero)
                    break;
            }           
        }

        /// <summary>
        /// Determines whether [is rect intersect] [the specified source].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="relativeTo">The relative to.</param>
        /// <returns>
        ///   <c>true</c> if [is rect intersect] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsRectIntersect(Rect source, Rect relativeTo)
        {
            return !(relativeTo.Left > source.Right || relativeTo.Right < source.Left || relativeTo.Top > source.Bottom || relativeTo.Bottom < source.Top);
        }


        /// <summary>
        /// Datas to point.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual double DataToPoint(double value)
        {
            return 0;            
        }

        /// <summary>
        /// Calculates the interval from series points.
        /// </summary>
        public virtual void CalculateIntervalFromSeriesPoints()
        {

        }
        /// <summary>
        /// Gets the original label.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual string GetOriginalLabel(double value)
        {
            return string.Empty;
        }

        /// <summary>
        /// Calculates the auto interval.
        /// </summary>
        protected void CalculateAutoInterval()
        {
            _isIntervalCountZero = false;
            bool isZoomig = false;
            if (CheckType())
            {
               
                if (ZoomCoefficient < 1 || (ZoomOffset > 0 && ZoomCoefficient < 1))
                {
                    MMinValue = ActualMinvalue + ZoomOffset * (ActualMaxvalue - ActualMinvalue); ;
                    MMaxValue = MMinValue + ZoomCoefficient * (ActualMaxvalue - ActualMinvalue);
                    if (MMinValue < ActualMinvalue)
                    {
                        MMaxValue = MMaxValue + (ActualMinvalue - MMinValue);
                        MMinValue = ActualMinvalue;
                    }

                    if (MMaxValue > ActualMaxvalue)
                    {
                        MMinValue = MMinValue - (MMaxValue - ActualMaxvalue);
                        MMaxValue = ActualMaxvalue;
                    }
                    MStartValue = MMinValue;
                    isZoomig = true;
                }
                else
                {
                    MMaxValue = ActualMaxvalue;
                    MMinValue = ActualMinvalue;
                    MStartValue = ActualMinvalue;
                }
                if (this.Interval == null)
                {
                    switch (ActualType)
                    {
                        case ActualType.Double:
                            //m_MinValue = Math.Floor(m_MinValue);                           
                            this.MInterval = AxisUtil.CalculateInetrval(MMinValue, MMaxValue, MIntervalCount);
                            //m_MaxValue = m_MinValue + (m_IntervalCount * this.m_Interval);
                            break;
                        case ActualType.DateTime:
                            this.MInterval = (MMaxValue - MMinValue) / MIntervalCount;
                            break;
                        default:
                            break;
                    }
                   
                    this.MInterval = (MMaxValue - MMinValue) / MIntervalCount;
                }
                else if(!isZoomig)
                {
                    switch (ActualType)
                    {
                        case ActualType.Double:
                            this.MIntervalCount = (int)Math.Abs((MMaxValue - MMinValue) / MInterval);
                            double tempMax = (this.MIntervalCount * this.MInterval) + MMinValue;
                            if (tempMax >= this.MMaxValue)
                                this.MMaxValue = tempMax;
                            if(tempMax<this.MMaxValue)
                            {
                                this.MMaxValue = tempMax + this.MInterval;
                                MIntervalCount++;
                            }
                            
                            break;
                        case ActualType.DateTime:
                            this.MIntervalCount = (int)Math.Abs((MMaxValue - MMinValue) / MInterval);
                            break;
                        default:
                            break;
                    }
                    if (MIntervalCount <= 1)
                        _isIntervalCountZero = true;
                    MIntervalCount = (MIntervalCount > 0) ? MIntervalCount : 1;

                }
                
                //if ((m_MinValue >= m_startValue + m_Interval))
                //    m_startValue = m_MinValue + (m_MinValue % m_Interval);
                
                
            }
            else
            {
                if (this.Interval == null)
                {
                    this.MInterval = 1;
                    MStartValue = MMinValue;
                }
                else
                {
                    MIntervalCount = (int)(((int)MMaxValue - (int)MMinValue) / (int)MInterval);
                    double tempMax = (this.MIntervalCount * this.MInterval) + MMinValue;
                    if (tempMax >= this.MMaxValue)
                        this.MMaxValue = tempMax;
                    else
                    if (tempMax < this.MMaxValue)
                    {
                        this.MMaxValue = tempMax + this.MInterval;
                        MIntervalCount++;
                    }                    
                    if ((MMinValue >= MStartValue + MInterval))
                        MStartValue = MStartValue + MInterval;
                }
                this.MIntervalCount = (int)Math.Abs(((int)MMaxValue - (int)MMinValue) / (int)MInterval);
                MIntervalCount = (MIntervalCount > 0) ? MIntervalCount : 1;
                
            }
            
            RefreshSeries();
            if (this.Chart != null && this.Chart.Containers != null && this.Chart.Containers.AxisLinesconatiner != null)
                this.Chart.Containers.AxisLinesconatiner.Refresh();
        }

        /// <summary>
        /// Checks the type.
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckType()
        {
            return true;
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public virtual void Refresh()
        {
            if (this.Chart != null)
                InvalidateVisuals();
        }

        /// <summary>
        /// Refreshes the series.
        /// </summary>
        internal void RefreshSeries()
        {
            if (Series != null)
                foreach (SeriesBase series in this.Series)
                {
                    series.RefreshWithoutAxis(this);
                }
        }

        /// <summary>
        /// Gets the rotated rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="rotate">The rotate.</param>
        /// <returns></returns>
        public Rect GetRotatedRect(Rect rect, RotateTransform rotate)
        {
#if !WINRT
            Point leftTop = rotate.Transform(new Point(rect.Left, rect.Top));
            Point rightTop = rotate.Transform(new Point(rect.Right, rect.Top));
            Point leftBottom = rotate.Transform(new Point(rect.Left, rect.Bottom));
            Point rightBottom = rotate.Transform(new Point(rect.Right, rect.Bottom));
            double left = Math.Min(Math.Min(leftTop.X, rightTop.X), Math.Min(leftBottom.X, rightBottom.X));
            double top = Math.Min(Math.Min(leftTop.Y, rightTop.Y), Math.Min(leftBottom.Y, rightBottom.Y));
            double right = Math.Max(Math.Max(leftTop.X, rightTop.X), Math.Max(leftBottom.X, rightBottom.X));
            double bottom = Math.Max(Math.Max(leftTop.Y, rightTop.Y), Math.Max(leftBottom.Y, rightBottom.Y));
            return new Rect(left, top, right - left, bottom - top);
#else
            Point leftTop = rotate.TransformPoint(new Point(rect.Left, rect.Top));
            Point rightTop = rotate.TransformPoint(new Point(rect.Right, rect.Top));
            Point leftBottom = rotate.TransformPoint(new Point(rect.Left, rect.Bottom));
            Point rightBottom = rotate.TransformPoint(new Point(rect.Right, rect.Bottom));
            double left = Math.Min(Math.Min(leftTop.X, rightTop.X), Math.Min(leftBottom.X, rightBottom.X));
            double top = Math.Min(Math.Min(leftTop.Y, rightTop.Y), Math.Min(leftBottom.Y, rightBottom.Y));
            double right = Math.Max(Math.Max(leftTop.X, rightTop.X), Math.Max(leftBottom.X, rightBottom.X));
            double bottom = Math.Max(Math.Max(leftTop.Y, rightTop.Y), Math.Max(leftBottom.Y, rightBottom.Y));
            return new Rect(left, top, right - left, bottom - top);
#endif
        }

        internal double MInterval;
        internal double MMaxValue = 1;
        internal double MMinValue = 0;
        internal double ActualMaxvalue = 1;
        internal double ActualMinvalue = 0;
        internal double MIntervalCount = 5;
        
        internal List<string> MLabels;
        internal List<double> MLabelValues;
        internal double MOffset = 0;
        internal double MStartValue = 0;

        bool _isStartSet;

        protected bool IsAxisHeightSet;
        protected bool IsAxisWidthSet;

        /// <summary>
        /// Gets or sets the axis line style.
        /// </summary>
        /// <value>
        /// The axis line style.
        /// </value>
        public Style AxisLineStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string format.
        /// </summary>
        /// <value>
        /// The string format.
        /// </value>
        public string StringFormat
        {
            get { return (string)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }

        /// <summary>
        /// The string format property
        /// </summary>
        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register("StringFormat", typeof(string), typeof(AxisBase), new PropertyMetadata(string.Empty));


        /// <summary>
        /// Gets or sets the size of the minor line.
        /// </summary>
        /// <value>
        /// The size of the minor line.
        /// </value>
        public double MinorLineSize
        {
            get { return (double)GetValue(MinorLineSizeProperty); }
            set { SetValue(MinorLineSizeProperty, value); }
        }

        /// <summary>
        /// The minor line size property
        /// </summary>
        public static readonly DependencyProperty MinorLineSizeProperty =
            DependencyProperty.Register("MinorLineSize", typeof(double), typeof(AxisBase), new PropertyMetadata(6d));


        /// <summary>
        /// Gets or sets the minor line style.
        /// </summary>
        /// <value>
        /// The minor line style.
        /// </value>
        public Style MinorLineStyle
        {
            get { return (Style)GetValue(MinorLineStyleProperty); }
            set { SetValue(MinorLineStyleProperty, value); }
        }

        /// <summary>
        /// The minor line style property
        /// </summary>
        public static readonly DependencyProperty MinorLineStyleProperty =
            DependencyProperty.Register("MinorLineStyle", typeof(Style), typeof(AxisBase), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the major line style.
        /// </summary>
        /// <value>
        /// The major line style.
        /// </value>
        public Style MajorLineStyle
        {
            get { return (Style)GetValue(MajorLineStyleProperty); }
            set { SetValue(MajorLineStyleProperty, value); }
        }

        /// <summary>
        /// The major line style property
        /// </summary>
        public static readonly DependencyProperty MajorLineStyleProperty =
            DependencyProperty.Register("MajorLineStyle", typeof(Style), typeof(AxisBase), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the size of the major line.
        /// </summary>
        /// <value>
        /// The size of the major line.
        /// </value>
        public double MajorLineSize
        {
            get { return (double)GetValue(MajorLineSizeProperty); }
            set { SetValue(MajorLineSizeProperty, value); }
        }

        /// <summary>
        /// The major line size property
        /// </summary>
        public static readonly DependencyProperty MajorLineSizeProperty =
            DependencyProperty.Register("MajorLineSize", typeof(double), typeof(AxisBase), new PropertyMetadata(10d));


        /// <summary>
        /// Gets or sets the major ticks position.
        /// </summary>
        /// <value>
        /// The major ticks position.
        /// </value>
        public TickPosition MajorTicksPosition
        {
            get { return (TickPosition)GetValue(MajorTicksPositionProperty); }
            set { SetValue(MajorTicksPositionProperty, value); }
        }

        /// <summary>
        /// The major ticks position property
        /// </summary>
        public static readonly DependencyProperty MajorTicksPositionProperty =
            DependencyProperty.Register("MajorTicksPosition", typeof(TickPosition), typeof(AxisBase), new PropertyMetadata(TickPosition.Outside));


        /// <summary>
        /// Gets or sets the minor ticks position.
        /// </summary>
        /// <value>
        /// The minor ticks position.
        /// </value>
        public TickPosition MinorTicksPosition
        {
            get { return (TickPosition)GetValue(MinorTicksPositionProperty); }
            set { SetValue(MinorTicksPositionProperty, value); }
        }

        /// <summary>
        /// The minor ticks position property
        /// </summary>
        public static readonly DependencyProperty MinorTicksPositionProperty =
            DependencyProperty.Register("MinorTicksPosition", typeof(TickPosition), typeof(AxisBase), new PropertyMetadata(TickPosition.Outside));



        /// <summary>
        /// Gets or sets a value indicating whether [show major ticks].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show major ticks]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowMajorTicks
        {
            get { return (bool)GetValue(ShowMajorTicksProperty); }
            set { SetValue(ShowMajorTicksProperty, value); }
        }

        /// <summary>
        /// The show major ticks property
        /// </summary>
        public static readonly DependencyProperty ShowMajorTicksProperty =
            DependencyProperty.Register("ShowMajorTicks", typeof(bool), typeof(AxisBase), new PropertyMetadata(true));


        /// <summary>
        /// Gets or sets the minor ticks count.
        /// </summary>
        /// <value>
        /// The minor ticks count.
        /// </value>
        public int MinorTicksCount
        {
            get { return (int)GetValue(MinorTicksCountProperty); }
            set { SetValue(MinorTicksCountProperty, value);}
        }

        /// <summary>
        /// The minor ticks count property
        /// </summary>
        public static readonly DependencyProperty MinorTicksCountProperty =
            DependencyProperty.Register("MinorTicksCount", typeof(int), typeof(AxisBase), new PropertyMetadata(0));



        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// The header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(AxisBase), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the header template.
        /// </summary>
        /// <value>
        /// The header template.
        /// </value>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// The header template property
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(AxisBase), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the chart.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public SparrowChart Chart
        {
            get { return (SparrowChart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        /// <summary>
        /// The chart property
        /// </summary>
        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(SparrowChart), typeof(AxisBase), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the label template.
        /// </summary>
        /// <value>
        /// The label template.
        /// </value>
        public DataTemplate LabelTemplate
        {
            get { return (DataTemplate)GetValue(LabelTemplateProperty); }
            set { SetValue(LabelTemplateProperty, value); }
        }

        /// <summary>
        /// The label template property
        /// </summary>
        public static readonly DependencyProperty LabelTemplateProperty =
            DependencyProperty.Register("LabelTemplate", typeof(DataTemplate), typeof(AxisBase), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the label angle.
        /// </summary>
        /// <value>
        /// The label angle.
        /// </value>
        public double LabelAngle
        {
            get { return (double)GetValue(LabelAngleProperty); }
            set { SetValue(LabelAngleProperty, value); }
        }

        /// <summary>
        /// The label angle property
        /// </summary>
        public static readonly DependencyProperty LabelAngleProperty =
            DependencyProperty.Register("LabelAngle", typeof(double), typeof(AxisBase), new PropertyMetadata(0d));



        /// <summary>
        /// Gets or sets a value indicating whether [show cross lines].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show cross lines]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowCrossLines
        {
            get { return (bool)GetValue(ShowCrossLinesProperty); }
            set { SetValue(ShowCrossLinesProperty, value); }
        }

        /// <summary>
        /// The show cross lines property
        /// </summary>
        public static readonly DependencyProperty ShowCrossLinesProperty =
            DependencyProperty.Register("ShowCrossLines", typeof(bool), typeof(AxisBase), new PropertyMetadata(true));



        /// <summary>
        /// Gets or sets a value indicating whether [show minor cross lines].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show minor cross lines]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowMinorCrossLines
        {
            get { return (bool)GetValue(ShowMinorCrossLinesProperty); }
            set { SetValue(ShowMinorCrossLinesProperty, value); }
        }

        /// <summary>
        /// The show minor cross lines property
        /// </summary>
        public static readonly DependencyProperty ShowMinorCrossLinesProperty =
            DependencyProperty.Register("ShowMinorCrossLines", typeof(bool), typeof(AxisBase), new PropertyMetadata(true));




        /// <summary>
        /// Gets or sets the minor cross line style.
        /// </summary>
        /// <value>
        /// The minor cross line style.
        /// </value>
        public Style MinorCrossLineStyle
        {
            get { return (Style)GetValue(MinorCrossLineStyleProperty); }
            set { SetValue(MinorCrossLineStyleProperty, value); }
        }

        public static readonly DependencyProperty MinorCrossLineStyleProperty =
            DependencyProperty.Register("MinorCrossLineStyle", typeof(Style), typeof(AxisBase), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the cross line style.
        /// </summary>
        /// <value>
        /// The cross line style.
        /// </value>
        public Style CrossLineStyle
        {
            get { return (Style)GetValue(CrossLineStyleProperty); }
            set { SetValue(CrossLineStyleProperty, value); }
        }

        /// <summary>
        /// The cross line style property
        /// </summary>
        public static readonly DependencyProperty CrossLineStyleProperty =
            DependencyProperty.Register("CrossLineStyle", typeof(Style), typeof(AxisBase), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public SeriesCollection Series
        {
            get { return (SeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// The series property
        /// </summary>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(SeriesCollection), typeof(AxisBase), new PropertyMetadata(null));
    }
}
