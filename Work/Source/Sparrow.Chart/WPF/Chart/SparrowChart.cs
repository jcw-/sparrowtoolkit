using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.Devices.Input;
using Windows.UI.Xaml.Markup;
#endif

namespace Sparrow.Chart
{
    /// <summary>
    /// SparrowChart Control
    /// </summary>
#if !WINRT
    [ContentProperty("Series")]
#else
    [ContentProperty(Name = "Series")]
#endif
    public class SparrowChart : Control
    {        
        internal ContainerCollection Containers;
        internal int IndexCount = 0;
        bool _isMouseDragging;
        bool _isMouseClick;
        private bool _isLegendUpdate;
        internal RootPanel RootDockPanel;
        internal Legend legend;
        internal Grid InnerChartPanel;
        internal Grid OuterChartPanel;
        internal List<ColumnSeries> ColumnSeries;
        internal List<HiLoOpenCloseSeries> HiLoOpenCloseSeries;
        internal List<ColumnSeries> CandleStickSeries;
        internal double MAxisheight;
        internal double MAxiswidth;

        ResourceDictionary styles;
        internal ObservableCollection<LegendItem> LegendItems;

        private static List<string> _actualCategoryValues;

        /// <summary>
        /// Gets or sets the actual category values.
        /// </summary>
        /// <value>
        /// The actual category values.
        /// </value>
        internal static List<string> ActualCategoryValues
        {
            get { return _actualCategoryValues; }
            set { _actualCategoryValues = value; }
        }

#if !WINRT
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" />. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        public override void OnApplyTemplate()
        {            
            Containers = (ContainerCollection)this.GetTemplateChild("PART_containers");
            if (Containers != null)
                Containers.Chart = this;
            RootDockPanel = (RootPanel)this.GetTemplateChild("Part_rootDockPanel");
            
            //containers.MouseMove += OnMouseMove;
            //containers.MouseLeave += OnMouseLeave;
            //containers.MouseLeftButtonDown += OnMousePress;
            //containers.MouseLeftButtonUp += OnMouseRelease;
#else
        protected override void OnApplyTemplate()
        {
            Containers = this.GetTemplateChild("PART_containers") as ContainerCollection;
            if (Containers != null)
                Containers.Chart = this;
            RootDockPanel = this.GetTemplateChild("Part_rootDockPanel") as RootPanel;            
#endif
#if WPF
            Containers.ClipToBounds = true;  
#else
            foreach (SeriesBase series in Series)
            {
                Binding dataContextBinding = new Binding {Path = new PropertyPath("DataContext"), Source = this};
                BindingOperations.SetBinding(series, SeriesBase.DataContextProperty, dataContextBinding);
            }
#endif                       
            BrushTheme();       
            base.OnApplyTemplate();
            legend = (Legend)this.GetTemplateChild("Part_Legend");
            InnerChartPanel = (Grid)this.GetTemplateChild("Part_InnerChartPanel");
            OuterChartPanel = (Grid)this.GetTemplateChild("Part_OuterChartPanel");
            RefreshLegend();  
        }

        /// <summary>
        /// Refreshes the legend.
        /// </summary>
        public void RefreshLegend()
        {
            //if (!isLegendUpdate)
            //{
                if (this.Legend != null)
                {
                    this.LegendItems.Clear();
                    if(this.Series!=null)
                        foreach (SeriesBase series in this.Series)
                        {
                            //if (!(series is PieSeriesBase))
                            //{
                                var legendItem = new LegendItem {Series = series};
                                var showIconBinding = new Binding
                                {
                                    Path = new PropertyPath("ShowIcon"),
                                    Source = this.Legend
                                };
                                BindingOperations.SetBinding(legendItem, LegendItem.ShowIconProperty, showIconBinding);
                                this.LegendItems.Add(legendItem);
                            //}
                            //else
                            //{
                            //    LegendItems = (series as PieSeriesBase).LegendItems;
                            //}
                        }
                    this.Legend.ItemsSource = this.LegendItems;                   
                    _isLegendUpdate = true;
                    switch (this.Legend.LegendPosition)
                    {
                        case LegendPosition.Inside:
                            if (this.OuterChartPanel != null && this.OuterChartPanel.Children.Contains(this.Legend))
                            {
                                this.OuterChartPanel.Children.Remove(this.Legend);
                            }
                            if (this.InnerChartPanel != null && !this.InnerChartPanel.Children.Contains(this.Legend))
                            {
                                this.InnerChartPanel.Children.Add(this.Legend);
                            }
                            break;
                        case LegendPosition.Outside:
                            if (this.InnerChartPanel != null && this.InnerChartPanel.Children.Contains(this.Legend))
                            {
                                this.InnerChartPanel.Children.Remove(this.Legend);
                            }
                            if (this.OuterChartPanel != null && !this.OuterChartPanel.Children.Contains(this.Legend))
                            {
                                this.OuterChartPanel.Children.Add(this.Legend);
                            }
                            break;
                    }
                    
                }

            //}
        }

        void OnMouseRelease(object sender, MouseEventArgs e)
        {

        }

        void OnMousePress(object sender, MouseEventArgs e)
        {                         
           
        }
        void OnMouseLeave(object sender, MouseEventArgs e)
        {

        }
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparrowChart"/> class.
        /// </summary>
        public SparrowChart()
        {            
            this.DefaultStyleKey = typeof(SparrowChart);
            this.Series = new SeriesCollection();
            this.XAxes = new Axes();
            this.XAxes.CollectionChanged += OnAxesCollectionChanged;
            this.YAxes = new Axes();
            this.YAxes.CollectionChanged += OnAxesCollectionChanged;            
            _brushes = Themes.MetroBrushes();
            LegendItems = new ObservableCollection<LegendItem>();
            ActualCategoryValues = new List<string>();
            ColumnSeries = new List<ColumnSeries>();
            HiLoOpenCloseSeries = new List<HiLoOpenCloseSeries>();
            CandleStickSeries = new List<ColumnSeries>();
            styles = new ResourceDictionary()
            {
#if X86
                Source = new Uri(@"/Sparrow.Chart.DirectX2D_x86;component/Themes/Styles.xaml", UriKind.Relative)
#elif X64
                Source = new Uri(@"/Sparrow.Chart.DirectX2D_x64;component/Themes/Styles.xaml", UriKind.Relative)
#else
#if WPF
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
                Source=new Uri(@"/Sparrow.Chart.WP7.45;component/Themes/Styles.xaml", UriKind.Relative)
#else
                Source=new Uri(@"/Sparrow.Chart.WP7.40;component/Themes/Styles.xaml", UriKind.Relative)
#endif
#elif WP8
                Source = new Uri(@"/Sparrow.Chart.WP8.45;component/Themes/Styles.xaml", UriKind.Relative)
#endif
#endif
            };
            this.ContainerBorderStyle = styles["containerBorderStyle"] as Style;
           
        }

        /// <summary>
        /// Called when [axes collection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        void OnAxesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// Called when [series collection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        void OnSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    
                    foreach (SeriesBase series in e.NewItems)
                    {
                        Binding dataContextBinding = new Binding {Path = new PropertyPath("DataContext"), Source = this};
                        BindingOperations.SetBinding(series, SeriesBase.DataContextProperty, dataContextBinding);
                        Binding renderingModeBinding = new Binding
                            {
                                Path = new PropertyPath("RenderingMode"),
                                Source = this
                            };
                        BindingOperations.SetBinding(series, SeriesBase.RenderingModeProperty, renderingModeBinding);
                        series.Chart = this;
                        series.Index = this.Series.IndexOf(series);
                        Binding xAxisBinding = new Binding {Source = this, Path = new PropertyPath("XAxis")};
                        BindingOperations.SetBinding(series, SeriesBase.XAxisProperty, xAxisBinding);
                        Binding yAxisBinding = new Binding {Source = this, Path = new PropertyPath("YAxis")};
                        BindingOperations.SetBinding(series, SeriesBase.YAxisProperty, yAxisBinding);
                        IndexCount = this.Series.Count;
                        if (series is ColumnSeries)
                        {
                            ColumnSeries.Add(series as ColumnSeries);                            
                        }
                        else if (series is HiLoOpenCloseSeries)
                        {
                            HiLoOpenCloseSeries.Add(series as HiLoOpenCloseSeries);
                        }
                        _isLegendUpdate = false;
                        RefreshLegend();
                    }                   
                    break;
#if WPF
                case NotifyCollectionChangedAction.Move:
                    break;
#endif
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                    
            }
            if (this.Containers != null)
                this.Containers.Refresh();
        }
#if WPF
        /// <summary>
        /// Export the Sparrow Chart as Image
        /// </summary>
        /// <param name="fileName">Output File Name to Export the Chart as Image</param>
        public void ExportAsImage(string fileName)
        {
            string fileExtension = new FileInfo(fileName).Extension.ToLower(CultureInfo.InvariantCulture);

            BitmapEncoder imageEncoder = null;
            switch (fileExtension)
            {
                case ".bmp":
                    imageEncoder = new BmpBitmapEncoder();
                    break;

                case ".jpg":
                case ".jpeg":
                    imageEncoder = new JpegBitmapEncoder();
                    break;

                case ".png":
                    imageEncoder = new PngBitmapEncoder();
                    break;

                case ".gif":
                    imageEncoder = new GifBitmapEncoder();
                    break;

                case ".tif":
                case ".tiff":
                    imageEncoder = new TiffBitmapEncoder();
                    break;

                case ".wdp":
                    imageEncoder = new WmpBitmapEncoder();
                    break;

                default:
                    imageEncoder = new BmpBitmapEncoder();
                    break;
            }

            RenderTargetBitmap bmpSource =new RenderTargetBitmap((int)this.ActualWidth,(int)this.ActualHeight, 96, 96,PixelFormats.Pbgra32);
            bmpSource.Render(this);

            imageEncoder.Frames.Add(BitmapFrame.Create(bmpSource));
            using (Stream stream = File.Create(fileName))
            {
                imageEncoder.Save(stream);
                stream.Close();
            }
            
        }

        /// <summary>
        /// Export the Sparrow Chart as Image
        /// </summary>
        /// <param name="fileName">Output File Name to Export the Chart as Image<</param>
        /// <param name="imageEncoder">Image Encoder Format for output image<</param>
        public void ExportAsImage(string fileName,BitmapEncoder imageEncoder)
        {
            string fileExtension = new FileInfo(fileName).Extension.ToLower(CultureInfo.InvariantCulture);                  

            RenderTargetBitmap bmpSource = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bmpSource.Render(this);

            imageEncoder.Frames.Add(BitmapFrame.Create(bmpSource));
            using (Stream stream = File.Create(fileName))
            {
                imageEncoder.Save(stream);
                stream.Close();
            }
        }
#endif
        /// <summary>
        /// Brushes the theme.
        /// </summary>
        private void BrushTheme()
        {
            if (this.Series != null)
                foreach (var series in Series)
                {
                    //if (!(series is PieSeriesBase))
                    //{
                        if (series.Stroke == null)
                        {
                            if (_brushes.Count > 1)
                                series.Stroke = _brushes[series.Index%(_brushes.Count)];
                            else
                                series.Stroke = _brushes[_brushes.Count];
                        }
                        var fillSeriesBase = series as FillSeriesBase;
                        if (fillSeriesBase != null && (series.IsFill && fillSeriesBase.Fill == null))
                        {
                            if (_brushes.Count > 1)
                                fillSeriesBase.Fill = _brushes[series.Index%(_brushes.Count)];
                            else
                                fillSeriesBase.Fill = _brushes[_brushes.Count];
                        }
                    //}
                    //else
                    //{
                    //    (series as PieSeriesBase).BrushTheme(_brushes);
                    //}
                }
        }

        //protected override Size MeasureOverride(Size constraint)
        //{                                
        //    if (rootDockPanel != null)
        //    {                
        //        rootDockPanel.Measure(constraint);               
        //    }
           
        //    return base.MeasureOverride(constraint);
        //}


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
            DependencyProperty.Register("XAxis", typeof(XAxis), typeof(SparrowChart), new PropertyMetadata(null,OnXAxisChanged));

        /// <summary>
        /// Called when [X axis changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnXAxisChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as SparrowChart).XAxisChanged(args);
        }

        /// <summary>
        /// Xs the axis changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void XAxisChanged(DependencyPropertyChangedEventArgs args)
        {
            Binding seriesBinding = new Binding {Path = new PropertyPath("Series"), Source = this};
            this.XAxis.SetBinding(AxisBase.SeriesProperty, seriesBinding);
            this.XAxis.Chart = this;
            this.XAxes.Add(this.XAxis);
        }


        /// <summary>
        /// Gets or sets the X axes.
        /// </summary>
        /// <value>
        /// The X axes.
        /// </value>
        public Axes XAxes
        {
            get { return (Axes)GetValue(XAxesProperty); }
            set { SetValue(XAxesProperty, value); }
        }

        /// <summary>
        /// The X axes property
        /// </summary>
        public static readonly DependencyProperty XAxesProperty =
            DependencyProperty.Register("XAxes", typeof(Axes), typeof(SparrowChart), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the Y axes.
        /// </summary>
        /// <value>
        /// The Y axes.
        /// </value>
        public Axes YAxes
        {
            get { return (Axes)GetValue(YAxesProperty); }
            set { SetValue(YAxesProperty, value); }
        }

        public static readonly DependencyProperty YAxesProperty =
            DependencyProperty.Register("YAxes", typeof(Axes), typeof(SparrowChart), new PropertyMetadata(null));



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
            DependencyProperty.Register("YAxis", typeof(YAxis), typeof(SparrowChart), new PropertyMetadata(null,OnYAxisChanged));

        /// <summary>
        /// Called when [Y axis changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnYAxisChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var sparrowChart = sender as SparrowChart;
            if (sparrowChart != null) sparrowChart.YAxisChanged(args);
        }

        /// <summary>
        /// Ys the axis changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void YAxisChanged(DependencyPropertyChangedEventArgs args)
        {
            Binding seriesBinding = new Binding();
            seriesBinding.Path = new PropertyPath("Series");
            seriesBinding.Source = this;
            this.YAxis.SetBinding(AxisBase.SeriesProperty, seriesBinding);
            this.YAxis.Chart = this;
            this.YAxes.Add(this.YAxis);
        }

        /// <summary>
        /// Gets or sets the height of the axis.
        /// </summary>
        /// <value>
        /// The height of the axis.
        /// </value>
        public Double AxisHeight
        {
            get { return (Double)GetValue(AxisHeightProperty); }
            set { SetValue(AxisHeightProperty, value); }
        }


        /// <summary>
        /// The axis height property
        /// </summary>
        public static readonly DependencyProperty AxisHeightProperty =
            DependencyProperty.Register("AxisHeight", typeof(Double), typeof(SparrowChart), new PropertyMetadata(30d));


        /// <summary>
        /// Gets or sets the width of the axis.
        /// </summary>
        /// <value>
        /// The width of the axis.
        /// </value>
        public Double AxisWidth
        {
            get { return (Double)GetValue(AxisWidthProperty); }
            set { SetValue(AxisWidthProperty, value); }
        }

        /// <summary>
        /// The axis width property
        /// </summary>
        public static readonly DependencyProperty AxisWidthProperty =
            DependencyProperty.Register("AxisWidth", typeof(Double), typeof(SparrowChart), new PropertyMetadata(30d));



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
            DependencyProperty.Register("IsRefresh", typeof(bool), typeof(SparrowChart), new PropertyMetadata(true,OnIsRefreshChanged));

        /// <summary>
        /// Called when [is refresh changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsRefreshChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SparrowChart sender = obj as SparrowChart;
            if (sender != null) sender.IsRefreshChanged(args);
        }

        /// <summary>
        /// Determines whether [is refresh changed] [the specified args].
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void IsRefreshChanged(DependencyPropertyChangedEventArgs args)
        {
            foreach (SeriesBase series in Series)
            {
                series.IsRefresh = (bool)args.NewValue;
                if ((bool)args.NewValue)
                    series.Refresh();
            }           

        }

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
            DependencyProperty.Register("Series", typeof(SeriesCollection), typeof(SparrowChart), new PropertyMetadata(null,OnSeriesChanged));

        private static void OnSeriesChanged(DependencyObject sender,DependencyPropertyChangedEventArgs args)
        {
            (sender as SparrowChart).SeriesChanged(args);
        }

        internal void SeriesChanged(DependencyPropertyChangedEventArgs args)
        {
           if(args.OldValue!=null && (args.OldValue is SeriesCollection))
           {
               (args.OldValue as SeriesCollection).CollectionChanged -= OnSeriesCollectionChanged;
           }
           if (args.NewValue != null && (args.NewValue is SeriesCollection))
           {
               IndexCount = 0;
               (args.NewValue as SeriesCollection).CollectionChanged += OnSeriesCollectionChanged;
               foreach (var series in (args.NewValue as SeriesCollection))
               {
                   Binding dataContextBinding = new Binding { Path = new PropertyPath("DataContext"), Source = this };
                   BindingOperations.SetBinding(series, SeriesBase.DataContextProperty, dataContextBinding);
                   Binding renderingModeBinding = new Binding
                   {
                       Path = new PropertyPath("RenderingMode"),
                       Source = this
                   };
                   BindingOperations.SetBinding(series, SeriesBase.RenderingModeProperty, renderingModeBinding);
                   series.Chart = this;
                   series.Index = IndexCount;
                   Binding xAxisBinding = new Binding { Source = this, Path = new PropertyPath("XAxis") };
                   BindingOperations.SetBinding(series, SeriesBase.XAxisProperty, xAxisBinding);
                   Binding yAxisBinding = new Binding { Source = this, Path = new PropertyPath("YAxis") };
                   BindingOperations.SetBinding(series, SeriesBase.YAxisProperty, yAxisBinding);
                   IndexCount++;
                   if (series is ColumnSeries)
                   {
                       ColumnSeries.Add(series as ColumnSeries);
                   }
                   else if (series is HiLoOpenCloseSeries)
                   {
                       HiLoOpenCloseSeries.Add(series as HiLoOpenCloseSeries);
                   }
                   _isLegendUpdate = false;
                   RefreshLegend();
               }
               if (this.Containers != null)
               {
                   this.Containers.Refresh(true);
               }
           }
        }

        /// <summary>
        /// Gets or sets the legend.
        /// </summary>
        /// <value>
        /// The legend.
        /// </value>
        public Legend Legend
        {
            get { return (Legend)GetValue(LegendProperty); }
            set { SetValue(LegendProperty, value); }
        }

        /// <summary>
        /// The legend property
        /// </summary>
        public static readonly DependencyProperty LegendProperty =
            DependencyProperty.Register("Legend", typeof(Legend), typeof(SparrowChart), new PropertyMetadata(null,OnLegendChanged));

        /// <summary>
        /// Called when [legend changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLegendChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var sparrowChart = sender as SparrowChart;
            if (sparrowChart != null) sparrowChart.LegendChanged(args);
        }

        /// <summary>
        /// Legends the changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void LegendChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.Legend != null)
            {
                this.Legend.Chart = this;
                this.Legend.DataContext = null;
            }
        }

        /// <summary>
        /// Gets or sets the legend dock.
        /// </summary>
        /// <value>
        /// The legend dock.
        /// </value>
        internal Dock LegendDock
        {
            get { return (Dock)GetValue(LegendDockProperty); }
            set { SetValue(LegendDockProperty, value); }
        }

        /// <summary>
        /// The legend dock property
        /// </summary>
        public static readonly DependencyProperty LegendDockProperty =
            DependencyProperty.Register("LegendDock", typeof(Dock), typeof(SparrowChart), new PropertyMetadata(Dock.Top));


        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        /// <value>
        /// The theme.
        /// </value>
        public Theme Theme
        {
            get { return (Theme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        /// <summary>
        /// The theme property
        /// </summary>
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(Theme), typeof(SparrowChart), new PropertyMetadata(Theme.Metro,OnThemeChanged));

        /// <summary>
        /// Called when [theme changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnThemeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var sparrowChart = sender as SparrowChart;
            if (sparrowChart != null) sparrowChart.ThemeChanged(args);
        }

        /// <summary>
        /// Themes the changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void ThemeChanged(DependencyPropertyChangedEventArgs args)
        {
            switch (Theme)
            {
                case Theme.Arctic:
                    _brushes=Themes.ArcticBrushes();
                    break;
                case Theme.Autmn:
                    _brushes = Themes.AutmnBrushes();
                    break;
                case Theme.Cold:
                    _brushes = Themes.ColdBrushes();
                    break;
                case Theme.Flower:
                    _brushes = Themes.FlowerBrushes();
                    break;
                case Theme.Forest:
                    _brushes = Themes.ForestBrushes();
                    break;
                case Theme.Grayscale:
                    _brushes = Themes.GrayscaleBrushes();
                    break;
                case Theme.Ground:
                    _brushes = Themes.GroundBrushes();
                    break;
                case Theme.Lialac:
                    _brushes = Themes.LialacBrushes();
                    break;
                case Theme.Natural:
                    _brushes = Themes.NaturalBrushes();
                    break;
                case Theme.Pastel:
                    _brushes = Themes.PastelBrushes();
                    break;
                case Theme.Rainbow:
                    _brushes = Themes.RainbowBrushes();
                    break;
                case Theme.Spring:
                    _brushes = Themes.SpringBrushes();
                    break;
                case Theme.Summer:
                    _brushes = Themes.SummerBrushes();
                    break;
                case Theme.Warm:
                    _brushes = Themes.WarmBrushes();
                    break;
                case Theme.Metro:
                    _brushes = Themes.MetroBrushes();
                    break;
                case Theme.Custom:
                    break;
                default:
                    break;
            }                                
            BrushTheme();
        }


        /// <summary>
        /// The overlay mode property
        /// </summary>
        public static readonly DependencyProperty OverlayModeProperty =
            DependencyProperty.Register("OverlayMode", typeof (OverlayMode), typeof (SparrowChart), new PropertyMetadata(OverlayMode.SeriesFirst));

        /// <summary>
        /// Gets or sets the overlay mode.
        /// </summary>
        /// <value>
        /// The overlay mode.
        /// </value>
        public OverlayMode OverlayMode
        {
            get { return (OverlayMode) GetValue(OverlayModeProperty); }
            set { SetValue(OverlayModeProperty, value); }
        }

        private List<Brush> _brushes;

        /// <summary>
        /// Gets or sets the brushes.
        /// </summary>
        /// <value>
        /// The brushes.
        /// </value>
        public List<Brush> Brushes
        {
            get { return (List<Brush>)GetValue(BrushesProperty); }
            set { SetValue(BrushesProperty, value); }
        }

        /// <summary>
        /// The brushes property
        /// </summary>
        public static readonly DependencyProperty BrushesProperty =
            DependencyProperty.Register("Brushes", typeof(List<Brush>), typeof(SparrowChart), new PropertyMetadata(new List<Brush>()));       
#if WPF
        public SmoothingMode SmoothingMode
        {
            get { return (SmoothingMode)GetValue(SmoothingModeProperty); }
            set { SetValue(SmoothingModeProperty, value); }
        }

        public static readonly DependencyProperty SmoothingModeProperty =
            DependencyProperty.Register("SmoothingMode", typeof(SmoothingMode), typeof(SparrowChart), new PropertyMetadata(SmoothingMode.HighQuality));
      

        public CompositingQuality CompositingQuality
        {
            get { return (CompositingQuality)GetValue(CompositingQualityProperty); }
            set { SetValue(CompositingQualityProperty, value); }
        }

        public static readonly DependencyProperty CompositingQualityProperty =
            DependencyProperty.Register("CompositingQuality", typeof(CompositingQuality), typeof(SparrowChart), new PropertyMetadata(CompositingQuality.HighQuality));

         public CompositingMode CompositingMode
        {
            get { return (CompositingMode)GetValue(CompositingModeProperty); }
            set { SetValue(CompositingModeProperty, value); }
        }

        public static readonly DependencyProperty CompositingModeProperty =
            DependencyProperty.Register("CompositingMode", typeof(CompositingMode), typeof(SparrowChart), new PropertyMetadata(CompositingMode.SourceOver));

#endif
        /// <summary>
        /// Gets or sets the container border style.
        /// </summary>
        /// <value>
        /// The container border style.
        /// </value>
        public Style ContainerBorderStyle
        {
            get { return (Style)GetValue(ContainerBorderStyleProperty); }
            set { SetValue(ContainerBorderStyleProperty, value); }
        }

        /// <summary>
        /// The container border style property
        /// </summary>
        public static readonly DependencyProperty ContainerBorderStyleProperty =
            DependencyProperty.Register("ContainerBorderStyle", typeof(Style), typeof(SparrowChart), new PropertyMetadata(null));



        /// <summary>
        /// Gets the series margin percentage.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static double GetSeriesMarginPercentage(DependencyObject obj)
        {
            return (double)obj.GetValue(SeriesMarginPercentageProperty);
        }

        /// <summary>
        /// Sets the series margin percentage.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetSeriesMarginPercentage(DependencyObject obj, double value)
        {
            obj.SetValue(SeriesMarginPercentageProperty, value);
        }

        /// <summary>
        /// The series margin percentage property
        /// </summary>
        public static readonly DependencyProperty SeriesMarginPercentageProperty =
            DependencyProperty.RegisterAttached("SeriesMarginPercentage", typeof(double), typeof(SparrowChart), new PropertyMetadata(0.3d));


        /// <summary>
        /// Gets or sets the rendering mode.
        /// </summary>
        /// <value>
        /// The rendering mode.
        /// </value>
        public RenderingMode RenderingMode
        {
            get { return (RenderingMode)GetValue(RenderingModeProperty); }
            set { SetValue(RenderingModeProperty, value); }
        }

        /// <summary>
        /// The rendering mode property
        /// </summary>
        public static readonly DependencyProperty RenderingModeProperty =
            DependencyProperty.Register("RenderingMode", typeof(RenderingMode), typeof(SparrowChart), new PropertyMetadata(RenderingMode.Default));
       
    }
}
