using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
#if !WINRT
using System.Windows.Media;
using System.Windows.Threading;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI;
#endif

namespace Sparrow.Chart
{
    /// <summary>
    /// 
    /// </summary>
    public class StockChartBase : SeriesBase
    {
        internal PointsCollection lowPoints;
        internal List<double> HighValues;
        internal List<double> LowValues;

        /// <summary>
        /// Generates the points from source.
        /// </summary>
        public virtual void GeneratePointsFromSource()
        {
            XValues = this.GetReflectionValues(this.XPath, PointsSource, XValues, false);
            YValues = this.GetReflectionValues(this.LowPath, PointsSource, YValues, false);
            LowValues = YValues;
            lowPoints = new PointsCollection();
            if (XValues != null && XValues.Count > 0)
            {
                this.lowPoints = GetPointsFromValues(XValues, YValues);
            }
            XValues = this.GetReflectionValues(this.XPath, PointsSource, XValues, false);
            YValues = this.GetReflectionValues(this.HighPath, PointsSource, YValues, false);
            if (XValues != null && XValues.Count > 0)
            {
                this.Points = GetPointsFromValues(XValues, YValues);               
            }
            HighValues = YValues;
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        override public void Refresh()
        {
            base.Refresh();
            if (IsRefresh)
            {
#if WPF
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(this.GenerateDatas));
#elif WINRT
                IAsyncAction action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, this.GenerateDatas);
#else
                Dispatcher.BeginInvoke(new Action(this.GenerateDatas));
#endif
               // isRefreshed = true;
            }
        }

        /// <summary>
        /// Gets or sets the points source.
        /// </summary>
        /// <value>
        /// The points source.
        /// </value>
        public IEnumerable PointsSource
        {
            get { return (IEnumerable)GetValue(PointsSourceProperty); }
            set { SetValue(PointsSourceProperty, value); }
        }

        /// <summary>
        /// The points source property
        /// </summary>
        public static readonly DependencyProperty PointsSourceProperty =
            DependencyProperty.Register("PointsSource", typeof(IEnumerable), typeof(StockChartBase), new PropertyMetadata(null, OnPointsSourceChanged));

        /// <summary>
        /// Called when [points source changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnPointsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            StockChartBase series = sender as StockChartBase;
            series.PointsSourceChanged(sender, args);

        }
        /// <summary>
        /// Pointses the source changed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void PointsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue is INotifyCollectionChanged)
            {
                (args.OldValue as INotifyCollectionChanged).CollectionChanged -= PointsSource_CollectionChanged;
            }

            if (args.NewValue is INotifyCollectionChanged)
            {
                (args.NewValue as INotifyCollectionChanged).CollectionChanged += PointsSource_CollectionChanged;
            }
            GeneratePointsFromSource();
        }

        /// <summary>
        /// Handles the CollectionChanged event of the PointsSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        void PointsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                double xValue = GetReflectionValue(XPath, PointsSource, XValues.Count + 1);
                double yValue = GetReflectionValue(HighPath, PointsSource, YValues.Count + 1);
                this.XValues.Add(xValue);
                this.YValues.Add(yValue);
                this.Points.Add(new ChartPoint() { XValue = xValue, YValue = yValue });
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                IList oldItems = e.OldItems;
                double oldXValue = GetReflectionValueFromItem(XPath, oldItems[0]);
                int index = this.XValues.IndexOf(oldXValue);
                this.XValues.RemoveAt(index);
                this.YValues.RemoveAt(index);
                this.Points.RemoveAt(index);
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Points.Clear();
                GeneratePointsFromSource();
            }
        }

        /// <summary>
        /// Gets or sets the high path.
        /// </summary>
        /// <value>
        /// The high path.
        /// </value>
        public string HighPath
        {
            get { return (string)GetValue(HighPathProperty); }
            set { SetValue(HighPathProperty, value); }
        }

        /// <summary>
        /// The high path property
        /// </summary>
        public static readonly DependencyProperty HighPathProperty =
            DependencyProperty.Register("HighPath", typeof(string), typeof(StockChartBase), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the low path.
        /// </summary>
        /// <value>
        /// The low path.
        /// </value>
        public string LowPath
        {
            get { return (string)GetValue(LowPathProperty); }
            set { SetValue(LowPathProperty, value); }
        }

        /// <summary>
        /// The low path property
        /// </summary>
        public static readonly DependencyProperty LowPathProperty =
            DependencyProperty.Register("LowPath", typeof(string), typeof(StockChartBase), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the bear fill.
        /// </summary>
        /// <value>
        /// The bear fill.
        /// </value>
        public Brush BearFill
        {
            get { return (Brush)GetValue(BearFillProperty); }
            set { SetValue(BearFillProperty, value); }
        }

        /// <summary>
        /// The bear fill property
        /// </summary>
        public static readonly DependencyProperty BearFillProperty =
            DependencyProperty.Register("BearFill", typeof(Brush), typeof(StockChartBase), new PropertyMetadata(new SolidColorBrush(Colors.Green)));



        /// <summary>
        /// Gets or sets the bull fill.
        /// </summary>
        /// <value>
        /// The bull fill.
        /// </value>
        public Brush BullFill
        {
            get { return (Brush)GetValue(BullFillProperty); }
            set { SetValue(BullFillProperty, value); }
        }

        /// <summary>
        /// The bull fill property
        /// </summary>
        public static readonly DependencyProperty BullFillProperty =
            DependencyProperty.Register("BullFill", typeof(Brush), typeof(StockChartBase), new PropertyMetadata(new SolidColorBrush(Colors.Red)));


        /// <summary>
        /// Gets or sets the high points.
        /// </summary>
        /// <value>
        /// The high points.
        /// </value>
        public PointCollection HighPoints
        {
            get { return (PointCollection)GetValue(HighPointsProperty); }
            set { SetValue(HighPointsProperty, value); }
        }

        /// <summary>
        /// The high points property
        /// </summary>
        public static readonly DependencyProperty HighPointsProperty =
            DependencyProperty.Register("HighPoints", typeof(PointCollection), typeof(StockChartBase), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the low points.
        /// </summary>
        /// <value>
        /// The low points.
        /// </value>
        public PointCollection LowPoints
        {
            get { return (PointCollection)GetValue(LowPointsProperty); }
            set { SetValue(LowPointsProperty, value); }
        }

        /// <summary>
        /// The low points property
        /// </summary>
        public static readonly DependencyProperty LowPointsProperty =
            DependencyProperty.Register("LowPoints", typeof(PointCollection), typeof(StockChartBase), new PropertyMetadata(null));



    }
}
