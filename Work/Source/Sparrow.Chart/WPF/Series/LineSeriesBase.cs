using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
#if !WINRT
using System.Windows.Threading;

#else
using Windows.UI.Xaml;
using Windows.Foundation;
#endif


namespace Sparrow.Chart
{
    /// <summary>
    /// LineSeries Based Series
    /// </summary>
    public class LineSeriesBase : SeriesBase
    {

        /// <summary>
        /// Generates the points from source.
        /// </summary>
        public void GeneratePointsFromSource()
        {
            XValues = GetReflectionValues(XPath, PointsSource, XValues, false);
            YValues = new List<double>();

            YValues = GetReflectionValues(YPath, PointsSource, YValues, false);

            if (XValues != null && XValues.Count > 0)
            {
              Points = GetPointsFromValues(XValues, YValues);
            }
            else
            {
              Points.Clear();
            }
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        override public void Refresh()
        {
            if (!IsPointsGenerated)
                base.Refresh();
            if (IsRefresh)
            {
#if WPF               
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(this.GenerateDatas));               
#elif WINRT
                IAsyncAction action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, this.GenerateDatas);
#else
                Dispatcher.BeginInvoke(GenerateDatas);
#endif
                //isRefreshed = true;
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
            DependencyProperty.Register("PointsSource", typeof(IEnumerable), typeof(LineSeriesBase), new PropertyMetadata(null, OnPointsSourceChanged));

        /// <summary>
        /// Called when [points source changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnPointsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            LineSeriesBase series = sender as LineSeriesBase;
            if (series != null) series.PointsSourceChanged(sender, args);
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
                double yValue = GetReflectionValue(YPath, PointsSource, YValues.Count + 1);
                XValues.Add(xValue);                
                YValues.Add(xValue);
                Points.Add(new ChartPoint() { XValue = xValue, YValue = yValue });
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                IList oldItems = e.OldItems;
                double oldXValue = GetReflectionValueFromItem(XPath, oldItems[0]);
                int index = XValues.IndexOf(oldXValue);
                XValues.RemoveAt(index);
                YValues.RemoveAt(index);
                Points.RemoveAt(index);
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Points.Clear();
                GeneratePointsFromSource();
            }

        }

        /// <summary>
        /// Gets or sets the Y path.
        /// </summary>
        /// <value>
        /// The Y path.
        /// </value>
        public string YPath
        {
            get { return (string)GetValue(YPathProperty); }
            set { SetValue(YPathProperty, value); }
        }

        /// <summary>
        /// The Y path property
        /// </summary>
        public static readonly DependencyProperty YPathProperty =
            DependencyProperty.Register("YPath", typeof(string), typeof(LineSeriesBase), new PropertyMetadata(null));
       
    }
}
