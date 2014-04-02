using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

#if !WINRT
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
#endif

namespace Sparrow.Chart
{
    /// <summary>
    /// Base for All Series
    /// </summary>
    public abstract class SeriesBase : Control
    {
        protected double XMin { get; set; }
        protected double XMax { get; set; }
        protected double YMin { get; set; }
        protected double YMax { get; set; }
        double _xDifference;
        double _yDifference;
        double _xAbs;
        double _yAbs;
        internal List<double> XValues;
        internal List<double> YValues;
        internal SeriesContainer SeriesContainer;
        internal bool IsFill;
        protected bool IsRefreshed;
        internal bool IsPointsGenerated;

        /// <summary>
        /// Generates the datas.
        /// </summary>
        public virtual void GenerateDatas()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesBase"/> class.
        /// </summary>
        public SeriesBase()
        {
            this.DefaultStyleKey = typeof(SeriesBase);
            this.Parts = new PartsCollection();
            this.Points = new PointsCollection();
        }

        /// <summary>
        /// Checks the min maxand interval.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="axis">The axis.</param>
        internal void CheckMinMaxandInterval(Double value, AxisBase axis)
        {
            axis.MMinValue = Math.Min(axis.MMinValue, value);
            axis.MMaxValue = Math.Max(axis.MMaxValue, value);
        }

        /// <summary>
        /// Calculates the min and max.
        /// </summary>
        public virtual void CalculateMinAndMax()
        {
            if (this.XAxis != null)
            {                
                XMin = this.XAxis.MMinValue;
                XMax = this.XAxis.MMaxValue;
            }
            if (this.YAxis != null)
            {               
                YMin = this.YAxis.MMinValue;
                YMax = this.YAxis.MMaxValue;
            }
        }

        /// <summary>
        /// Normalizes the point.
        /// </summary>
        /// <param name="pt">The pt.</param>
        /// <returns></returns>
        public Point NormalizePoint(Point pt)
        {
            Point result = new Point();
            //if (this.XAxis != null)
            //    result.X = this.XAxis.DataToPoint(pt.X);
            //if (this.YAxis != null)
            //    result.Y = this.YAxis.DataToPoint(pt.Y);
            result.X = ((pt.X - XMin) * (SeriesContainer.Collection.ActualWidth / (XMax - XMin)));
            result.Y = (SeriesContainer.Collection.ActualHeight - ((pt.Y - YMin) * SeriesContainer.Collection.ActualHeight) / (YMax - YMin));
            return result;
        }

        /// <summary>
        /// Checks the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected bool CheckValue(double value)
        {
            bool isOk = !(double.IsNaN(value) || double.IsInfinity(value) || double.IsNegativeInfinity(value) || double.IsPositiveInfinity(value) || value == double.MaxValue || value == double.MinValue);
            return isOk;
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public virtual void Refresh()
        {
            if (this.XAxis != null)
            {
                this.XAxis.MMinValue = 0;
                this.XAxis.MMaxValue = 1;
                this.XAxis.CalculateIntervalFromSeriesPoints();
                this.XAxis.Refresh();
            }
            if (this.YAxis != null)
            {
                this.YAxis.MMinValue = 0;
                this.YAxis.MMaxValue = 1;
                this.YAxis.CalculateIntervalFromSeriesPoints();
               this.YAxis.Refresh();
            }
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <returns></returns>
        internal virtual SeriesContainer CreateContainer()
        {
            return null;
        }

        /// <summary>
        /// Refreshes the without axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        public void RefreshWithoutAxis(AxisBase axis)
        {           
            if (!IsRefreshed && IsRefresh)
            {
#if WPF
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(this.GenerateDatas));
#elif WINRT
                IAsyncAction action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, this.GenerateDatas);
#else
                Dispatcher.BeginInvoke(new Action(this.GenerateDatas));
#endif
                IsRefreshed = true;
            }
        }

        /// <summary>
        /// Gets the points from values.
        /// </summary>
        /// <param name="xValues">The x values.</param>
        /// <param name="yValues">The y values.</param>
        /// <returns></returns>
        protected PointsCollection GetPointsFromValues(List<double> xValues,List<double> yValues)
        {
            PointsCollection tempPoints = new PointsCollection();
            for (int i = 0; (i < xValues.Count && i < yValues.Count); i++)
            {
                ChartPoint point = new ChartPoint() { XValue = xValues[i], YValue = yValues[i] };
                tempPoints.Add(point);
            }
            return tempPoints;
        }

        /// <summary>
        /// Intializes the points.
        /// </summary>
        protected void IntializePoints()
        {
            _xDifference = XMax - XMin;
            _yDifference = YMax - YMin;
            _xAbs = Math.Abs(_xDifference / Width);
            _yAbs = Math.Abs(_yDifference / Height);
        }

        /// <summary>
        /// Checks the value point.
        /// </summary>
        /// <param name="oldPoint">The old point.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        protected bool CheckValuePoint(ChartPoint oldPoint,ChartPoint point)
        {
            //point.YValue <= yMax && point.YValue >= yMin && point.XValue <= xMax && point.XValue >= xMin && 
            if ((Math.Abs(oldPoint.XValue - point.XValue) >= _xAbs || Math.Abs(oldPoint.YValue - point.YValue) >= _yAbs))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the reflection value from item.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public double GetReflectionValueFromItem(string path, Object item)
        {
#if !WINRT
            PropertyInfo propertInfo = item.GetType().GetProperty(path);
#else
            PropertyInfo propertInfo = item.GetType().GetRuntimeProperty(path);
#endif
            FastProperty fastPropertInfo = new FastProperty(propertInfo);

            if (propertInfo != null)
            {
                object value = fastPropertInfo.Get(item);
                if (value is Double || value is int)
                {
                    return Double.Parse(value.ToString());
                }
                else if (value is DateTime)
                {
                    return ((DateTime)value).ToOADate();
                }
                else if (value is String)
                {
                    if (!SparrowChart.ActualCategoryValues.Contains(value.ToString()))
                        SparrowChart.ActualCategoryValues.Add(value.ToString());
                    return SparrowChart.ActualCategoryValues.IndexOf(value.ToString());
                }
                else
                    throw new NotSupportedException(String.Format("The {0} type is Not Supported by Chart", path));
            }
            return 0d;
        }

        /// <summary>
        /// Gets the reflection value.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="source">The source.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public double GetReflectionValue(string path, IEnumerable source, int position)
        {
            if (!String.IsNullOrEmpty(path))
            {
                IEnumerator enumerator = source.GetEnumerator();
                double index = 0;
                for (int i = 0; i < position - 1; i++)
                {
                    enumerator.MoveNext();
                }

                if (enumerator.MoveNext())
                {
#if !WINRT
                    PropertyInfo propertInfo = enumerator.Current.GetType().GetProperty(path);
#else
                    PropertyInfo propertInfo = enumerator.Current.GetType().GetRuntimeProperty(path);
#endif
                    FastProperty fastPropertInfo = new FastProperty(propertInfo);
                    if (propertInfo != null)
                    {
                        object value = fastPropertInfo.Get(enumerator.Current);
                        if (value is Double || value is int)
                        {
                            return Double.Parse(value.ToString());
                        }
                        else if (value is DateTime)
                        {
                            return ((DateTime)value).ToOADate();
                        }
                        else if (value is String)
                        {
                            if (!SparrowChart.ActualCategoryValues.Contains(value.ToString()))
                                SparrowChart.ActualCategoryValues.Add(value.ToString());
                            return SparrowChart.ActualCategoryValues.IndexOf(value.ToString());
                        }
                        else
                            throw new NotSupportedException(String.Format("The {0} type is Not Supported by Chart", path));
                        
                    }
                }
            }
            return 0d;
        }

        /// <summary>
        /// Gets the reflection values.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="source">The source.</param>
        /// <param name="oldValues">The old values.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public List<double> GetReflectionValues(string path,IEnumerable source, List<double> oldValues,bool isUpdate)
        {
            List<double> values;
            if(isUpdate)
                values=oldValues;
            else
                values = new List<double>();
            bool notifyCollectionChanged=false;
            if (!string.IsNullOrEmpty(path) && (source != null))
            {
                IEnumerator enumerator = source.GetEnumerator();
                double index = 0d;
                
                if (enumerator.MoveNext())
                {                    
                    if (enumerator.Current is INotifyPropertyChanged)
                        notifyCollectionChanged = true;
#if !WINRT
                    PropertyInfo xPropertInfo = enumerator.Current.GetType().GetProperty(path);
#else
                    PropertyInfo xPropertInfo = enumerator.Current.GetType().GetRuntimeProperty(path);
#endif
                    FastProperty fastPropertInfo = new FastProperty(xPropertInfo);
                    do
                    {                        
                        if (xPropertInfo != null)
                        {
                            object value = fastPropertInfo.Get(enumerator.Current);
                            if (value is Double || value is int)
                            {
                                values.Add(Double.Parse(value.ToString()));
                            }
                            else if (value is DateTime)
                            {
                                values.Add(((DateTime)value).ToOADate());
                            }
                            else if (value is String)
                            {
                                if (!SparrowChart.ActualCategoryValues.Contains(value.ToString()))
                                    SparrowChart.ActualCategoryValues.Add(value.ToString());
                                values.Add(SparrowChart.ActualCategoryValues.IndexOf(value.ToString()));
                            }
                            else
                                throw new NotSupportedException(String.Format("The {0} type is Not Supported by Chart", path));
                            index++;
                        }
                    } while (enumerator.MoveNext());
                    if (notifyCollectionChanged)
                    {
                        enumerator.Reset();
                        while (enumerator.MoveNext())
                        {
                            (enumerator.Current as INotifyPropertyChanged).PropertyChanged -= Collection_PropertyChanged;
                            (enumerator.Current as INotifyPropertyChanged).PropertyChanged += Collection_PropertyChanged;
                        }

                    }
                }

            }
            return values;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the Collection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        virtual protected void Collection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Gets or sets the X axis.
        /// </summary>
        /// <value>
        /// The X axis.
        /// </value>
        public XAxis XAxis
        {
            get { return (XAxis)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        /// <summary>
        /// The X axis property
        /// </summary>
        public static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register("XAxis", typeof(XAxis), typeof(SeriesBase), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets a value indicating whether this instance is refresh.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is refresh; otherwise, <c>false</c>.
        /// </value>
        public bool IsRefresh
        {
            get { return (bool)GetValue(IsRefreshProperty); }
            set { SetValue(IsRefreshProperty, value); }
        }

        /// <summary>
        /// The is refresh property
        /// </summary>
        public static readonly DependencyProperty IsRefreshProperty =
            DependencyProperty.Register("IsRefresh", typeof(bool), typeof(SeriesBase), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the Y axis.
        /// </summary>
        /// <value>
        /// The Y axis.
        /// </value>
        public YAxis YAxis
        {
            get { return (YAxis)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        /// <summary>
        /// The Y axis property
        /// </summary>
        public static readonly DependencyProperty YAxisProperty =
            DependencyProperty.Register("YAxis", typeof(YAxis), typeof(SeriesBase), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        /// <value>
        /// The parts.
        /// </value>
        public PartsCollection Parts
        {
            get { return (PartsCollection)GetValue(PartsProperty); }
            set { SetValue(PartsProperty, value); }
        }

        /// <summary>
        /// The parts property
        /// </summary>
        public static readonly DependencyProperty PartsProperty =
            DependencyProperty.Register("Parts", typeof(PartsCollection), typeof(SeriesBase), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets a value indicating whether [use single part].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use single part]; otherwise, <c>false</c>.
        /// </value>
        public bool UseSinglePart
        {
            get { return (bool)GetValue(UseSinglePartProperty); }
            set { SetValue(UseSinglePartProperty, value); }
        }

        /// <summary>
        /// The use single part property
        /// </summary>
        public static readonly DependencyProperty UseSinglePartProperty =
            DependencyProperty.Register("UseSinglePart", typeof(bool), typeof(SeriesBase), new PropertyMetadata(false));



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
            DependencyProperty.Register("Chart", typeof(SparrowChart), typeof(SeriesBase), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// The stroke property
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(SeriesBase), new PropertyMetadata(null));

#if !WINRT && !WP7 && !WP8
        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        [TypeConverter(typeof(StringToChartPointConverter))]
#endif
        public PointsCollection Points
        {
            get { return (PointsCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointsCollection), typeof(SeriesBase), new PropertyMetadata(null, OnPointsChanged));

        private static void OnPointsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SeriesBase series = sender as SeriesBase;
            if (series != null) series.PointsChanged(args);
        }

        /// <summary>
        /// Pointses the changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void PointsChanged(DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue is INotifyCollectionChanged)
            {
                (args.OldValue as INotifyCollectionChanged).CollectionChanged -= Points_CollectionChanged;
            }

            if (args.NewValue is INotifyCollectionChanged)
            {
                (args.NewValue as INotifyCollectionChanged).CollectionChanged += Points_CollectionChanged;
            }
            IsPointsGenerated = false;
            
            if (this.IsRefresh)
                Refresh();            
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Points control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        void Points_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsPointsGenerated = false;
            if (this.IsRefresh)
                Refresh();
        }

        /// <summary>
        /// Gets or sets the X path.
        /// </summary>
        /// <value>
        /// The X path.
        /// </value>
        public string XPath
        {
            get { return (string)GetValue(XPathProperty); }
            set { SetValue(XPathProperty, value); }
        }



        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public object Label
        {
            get { return (object)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// The label property
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(object), typeof(SeriesBase), new PropertyMetadata(null));



        /// <summary>
        /// The X path property
        /// </summary>
        public static readonly DependencyProperty XPathProperty =
            DependencyProperty.Register("XPath", typeof(string), typeof(SeriesBase), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the rendering mode.
        /// </summary>
        /// <value>
        /// The rendering mode.
        /// </value>
        internal RenderingMode RenderingMode
        {
            get { return (RenderingMode)GetValue(RenderingModeProperty); }
            set { SetValue(RenderingModeProperty, value); }
        }

        /// <summary>
        /// The rendering mode property
        /// </summary>
        internal static readonly DependencyProperty RenderingModeProperty =
            DependencyProperty.Register("RenderingMode", typeof(RenderingMode), typeof(SeriesBase), new PropertyMetadata(RenderingMode.Default));


        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        /// <summary>
        /// The stroke thickness property
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
          DependencyProperty.Register("StrokeThickness", typeof(double), typeof(SeriesBase), new PropertyMetadata(1d));

        /// <summary>
        /// Sets the binding for strokeand stroke thickness.
        /// </summary>
        /// <param name="part">The part.</param>
        protected virtual void SetBindingForStrokeandStrokeThickness(SeriesPartBase part)
        {
            Binding strokeBinding = new Binding {Path = new PropertyPath("Stroke"), Source = this};
            Binding strokeThicknessBinding = new Binding {Path = new PropertyPath("StrokeThickness")};
            strokeThicknessBinding.Source = this;
            part.SetBinding(SeriesPartBase.StrokeProperty, strokeBinding);
            part.SetBinding(SeriesPartBase.StrokeThicknessProperty, strokeThicknessBinding);
        }

        private int _index;
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        internal int Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }   
}
