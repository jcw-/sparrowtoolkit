using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Linq;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Shapes;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Devices.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;
using Rectangle = Windows.UI.Xaml.Shapes.Rectangle;
using Line = Windows.UI.Xaml.Shapes.Line;
using Windows.Foundation;
using Windows.UI;
#endif

namespace Sparrow.BulletGraph
{
    [TemplatePart(Name = "PART_ElementCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_ScaleCanvas", Type = typeof(Canvas))]
#if !WINRT
    [ContentProperty("QualitativeRanges")]
#else
    [ContentProperty(Name = "QualitativeRanges")]
#endif
    public class BulletGraph : Control
    {

        #region PrivateFields

        private Canvas elementCanvas;
        private Canvas scaleCanvas;
        internal bool isRendered { get; set; }

        internal static string ElementCanvasName = "PART_ElementCanvas";
        internal static string ScaleCanvasName = "PART_ScaleCanvas";

        #endregion

        #region Constructor
        

        public BulletGraph()
        {
            this.DefaultStyleKey = typeof(BulletGraph);
            this.QualitativeRanges = new QualitativeRangeCollection();
            this.Labels = new LabelCollection();
            this.MinorTicks = new TickCollection();
            this.MajorTicks = new TickCollection();
            CalculateMeasures();
        }


        #endregion

        private void CalculateMeasures()
        {
            this.ComparativeManager = new ComparativeMeasure();
            this.ComparativeManager.BulletGraph = this;
            Binding comparativeMeasureBinding = new Binding();
            comparativeMeasureBinding.Source = this;
            comparativeMeasureBinding.Path = new PropertyPath("ComparativeMeasure");
            ComparativeManager.SetBinding(Sparrow.BulletGraph.ComparativeMeasure.MeasureProperty, comparativeMeasureBinding);
            Binding comparativeMeasureSpacing = new Binding();
            comparativeMeasureSpacing.Source = this;
            comparativeMeasureSpacing.Path = new PropertyPath("ComparativeMeasureSpacing");
            ComparativeManager.SetBinding(Sparrow.BulletGraph.ComparativeMeasure.ComparativeMeasureSpacingProperty, comparativeMeasureSpacing);
            Binding MeasureThicknessBinding = new Binding();
            MeasureThicknessBinding.Source = this;
            MeasureThicknessBinding.Path = new PropertyPath("ComparativeMeasureThickness");
            ComparativeManager.SetBinding(Sparrow.BulletGraph.ComparativeMeasure.MeasureThicknessProperty, MeasureThicknessBinding);
            Binding OrientationBinding = new Binding();
            OrientationBinding.Source = this;
            OrientationBinding.Path = new PropertyPath("Orientation");
            ComparativeManager.SetBinding(Sparrow.BulletGraph.ComparativeMeasure.OrientationProperty, OrientationBinding);
            Binding fillBinding = new Binding();
            fillBinding.Source = this;
            fillBinding.Path = new PropertyPath("ComparativeMeasureFill");
            ComparativeManager.SetBinding(Sparrow.BulletGraph.ComparativeMeasure.FillProperty, fillBinding);

            this.PerformanceManager = new PerformanceMeasure();
            this.PerformanceManager.BulletGraph = this;
            Binding PerformancecomparativeMeasureBinding = new Binding();
            PerformancecomparativeMeasureBinding.Source = this;
            PerformancecomparativeMeasureBinding.Path = new PropertyPath("PerformanceMeasure");
            PerformanceManager.SetBinding(Sparrow.BulletGraph.PerformanceMeasure.MeasureProperty, PerformancecomparativeMeasureBinding);
            Binding PerformanceMeasureThicknessBinding = new Binding();
            PerformanceMeasureThicknessBinding.Source = this;
            PerformanceMeasureThicknessBinding.Path = new PropertyPath("PerformanceMeasureThickness");
            PerformanceManager.SetBinding(Sparrow.BulletGraph.PerformanceMeasure.MeasureThicknessProperty, PerformanceMeasureThicknessBinding);
            Binding PerformanceOrientationBinding = new Binding();
            PerformanceOrientationBinding.Source = this;
            PerformanceOrientationBinding.Path = new PropertyPath("Orientation");
            PerformanceManager.SetBinding(Sparrow.BulletGraph.PerformanceMeasure.OrientationProperty, PerformanceOrientationBinding);
            Binding PerformanceFillBinding = new Binding();
            PerformanceFillBinding.Source = this;
            PerformanceFillBinding.Path = new PropertyPath("PerformanceMeasureFill");
            PerformanceManager.SetBinding(Sparrow.BulletGraph.PerformanceMeasure.FillProperty, PerformanceFillBinding);
        }


        #region OnApplyTemplate

#if !WINRT
        public override void OnApplyTemplate()
#else
        protected override void OnApplyTemplate()
#endif
        {
            elementCanvas = this.GetTemplateChild(ElementCanvasName) as Canvas;
            scaleCanvas = this.GetTemplateChild(ScaleCanvasName) as Canvas;
            var sortedRanges = this.QualitativeRanges.OrderByDescending(range => range.Maximum).ToList();
            foreach (var range in sortedRanges)
            {
                Rectangle rect = range.RangeRect;
                if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(rect))
                {
                    this.elementCanvas.Children.Add(rect);
                }
            }
            CalculateLabels();
            CalculateMajorAndMinorTicks();
            foreach (var label in Labels)
            {
                ContentControl content = label.GetLabelElement();
                if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(content))
                {
                    this.elementCanvas.Children.Add(content);
                }
            }
            foreach (var tick in this.MajorTicks)
            {
                Line majorLine = tick.GetTickLine();
                if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(majorLine))
                {
                    this.elementCanvas.Children.Add(majorLine);
                }
            }
            foreach (var minorTick in this.MinorTicks)
            {
                Line minorLine = minorTick.GetTickLine();
                if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(minorLine))
                {
                    this.elementCanvas.Children.Add(minorLine);
                }
            }
            Rectangle comapartiveRectangle = this.ComparativeManager.GetMeasureElement();
            Rectangle performanceRectange = this.PerformanceManager.GetMeasureElement();
            if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(comapartiveRectangle))
            {
                this.elementCanvas.Children.Add(comapartiveRectangle);
            }
            if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(performanceRectange))
            {
                this.elementCanvas.Children.Add(performanceRectange);
            }
            isRendered = true;
        }

        #endregion

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(BulletGraph), new PropertyMetadata(null));



        internal LabelCollection Labels
        {
            get { return (LabelCollection)GetValue(LabelsProperty); }
            set { SetValue(LabelsProperty, value); }
        }

        internal static readonly DependencyProperty LabelsProperty =
            DependencyProperty.Register("Labels", typeof(LabelCollection), typeof(BulletGraph), new PropertyMetadata(null));




        internal TickCollection MajorTicks
        {
            get { return (TickCollection)GetValue(MajorTicksProperty); }
            set { SetValue(MajorTicksProperty, value); }
        }

        internal static readonly DependencyProperty MajorTicksProperty =
            DependencyProperty.Register("MajorTicks", typeof(TickCollection), typeof(BulletGraph), new PropertyMetadata(null));



        internal TickCollection MinorTicks
        {
            get { return (TickCollection)GetValue(MinorTicksProperty); }
            set { SetValue(MinorTicksProperty, value); }
        }

        internal static readonly DependencyProperty MinorTicksProperty =
            DependencyProperty.Register("MinorTicks", typeof(TickCollection), typeof(BulletGraph), new PropertyMetadata(null));



        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(BulletGraph), new PropertyMetadata(Orientation.Horizontal,OnPropertyChangedChanged));

        private static void OnPropertyChangedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as BulletGraph).PropertyChanged(args);
        }
        internal void PropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if (isRendered)
            {              
                this.InvalidateMeasure();
            }
        }

        private static void OnComparativeMeasurePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as BulletGraph).ComparativeMeasurePropertyChanged(args);
        }
        internal void ComparativeMeasurePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if (isRendered)
            {
                this.ComparativeManager.Refresh(this.RangeRect);
            }
        }

        private static void OnPerformanceMeasurePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as BulletGraph).PerformanceMeasurePropertyChanged(args);
        }
        internal void PerformanceMeasurePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if (isRendered)
            {
                this.PerformanceManager.Refresh(this.RangeRect);
            }
        }

        private static void OnUpdatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as BulletGraph).UpdatePropertyChanged(args);
        }
        internal void UpdatePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if (isRendered)
            {
                var sortedRanges = this.QualitativeRanges.OrderByDescending(range => range.Maximum).ToList();
                foreach (var range in sortedRanges)
                {
                    Rectangle rect = range.RangeRect;
                    if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(rect))
                    {
                        this.elementCanvas.Children.Add(rect);
                    }
                }
                CalculateLabels();
                CalculateMajorAndMinorTicks();
                foreach (var label in Labels)
                {
                    ContentControl content = label.GetLabelElement();
                    if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(content))
                    {
                        this.elementCanvas.Children.Add(content);
                    }
                }
                foreach (var tick in this.MajorTicks)
                {
                    Line majorLine = tick.GetTickLine();
                    if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(majorLine))
                    {
                        this.elementCanvas.Children.Add(majorLine);
                    }
                }
                foreach (var minorTick in this.MinorTicks)
                {
                    Line minorLine = minorTick.GetTickLine();
                    if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(minorLine))
                    {
                        this.elementCanvas.Children.Add(minorLine);
                    }
                }
                Rectangle comapartiveRectangle = this.ComparativeManager.GetMeasureElement();
                Rectangle performanceRectange = this.PerformanceManager.GetMeasureElement();
                if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(comapartiveRectangle))
                {
                    this.elementCanvas.Children.Add(comapartiveRectangle);
                }
                if (this.elementCanvas != null && !this.elementCanvas.Children.Contains(performanceRectange))
                {
                    this.elementCanvas.Children.Add(performanceRectange);
                }
                this.Update();
            }
        }

        public HorizontalAlignment HeaderHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(HeaderHorizontalAlignmentProperty); }
            set { SetValue(HeaderHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty HeaderHorizontalAlignmentProperty =
            DependencyProperty.Register("HeaderHorizontalAlignment", typeof(HorizontalAlignment), typeof(BulletGraph), new PropertyMetadata(HorizontalAlignment.Center));



        public VerticalAlignment HeaderVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(HeaderVerticalAlignmentProperty); }
            set { SetValue(HeaderVerticalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty HeaderVerticalAlignmentProperty =
            DependencyProperty.Register("HeaderVerticalAlignment", typeof(VerticalAlignment), typeof(BulletGraph), new PropertyMetadata(VerticalAlignment.Center));



        internal PerformanceMeasure PerformanceManager
        {
            get { return (PerformanceMeasure)GetValue(PerformanceManagerProperty); }
            set { SetValue(PerformanceManagerProperty, value); }
        }

        internal static readonly DependencyProperty PerformanceManagerProperty =
            DependencyProperty.Register("PerformanceManager", typeof(PerformanceMeasure), typeof(BulletGraph), new PropertyMetadata(null));




        internal ComparativeMeasure ComparativeManager
        {
            get { return (ComparativeMeasure)GetValue(ComparativeManagerProperty); }
            set { SetValue(ComparativeManagerProperty, value); }
        }

        internal static readonly DependencyProperty ComparativeManagerProperty =
            DependencyProperty.Register("ComparativeManager", typeof(ComparativeMeasure), typeof(BulletGraph), new PropertyMetadata(null));



        internal Dock HeaderDock
        {
            get { return (Dock)GetValue(HeaderDockProperty); }
            set { SetValue(HeaderDockProperty, value); }
        }

        internal static readonly DependencyProperty HeaderDockProperty =
            DependencyProperty.Register("HeaderDock", typeof(Dock), typeof(BulletGraph), new PropertyMetadata(Dock.Left));



        public Dock HeaderPosition
        {
            get { return (Dock)GetValue(HeaderPositionProperty); }
            set { SetValue(HeaderPositionProperty, value); }
        }

        public static readonly DependencyProperty HeaderPositionProperty =
            DependencyProperty.Register("HeaderPosition", typeof(Dock), typeof(BulletGraph), new PropertyMetadata(Dock.Left, OnHeaderPositionChanged));

        private static void OnHeaderPositionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is BulletGraph)
                (sender as BulletGraph).HeaderDock = (sender as BulletGraph).HeaderPosition;
        }

        public string LabelFormat
        {
            get { return (string)GetValue(LabelFormatProperty); }
            set { SetValue(LabelFormatProperty, value); }
        }

        public static readonly DependencyProperty LabelFormatProperty =
            DependencyProperty.Register("LabelFormat", typeof(string), typeof(BulletGraph), new PropertyMetadata(string.Empty));



        public DataTemplate LabelTemplate
        {
            get { return (DataTemplate)GetValue(LabelTemplateProperty); }
            set { SetValue(LabelTemplateProperty, value); }
        }

        public static readonly DependencyProperty LabelTemplateProperty =
            DependencyProperty.Register("LabelTemplate", typeof(DataTemplate), typeof(BulletGraph), new PropertyMetadata(null));



        public LabelPosition LabelPosition
        {
            get { return (LabelPosition)GetValue(LabelPositionProperty); }
            set { SetValue(LabelPositionProperty, value); }
        }

        public static readonly DependencyProperty LabelPositionProperty =
            DependencyProperty.Register("LabelPosition", typeof(LabelPosition), typeof(BulletGraph), new PropertyMetadata(LabelPosition.Default,OnPropertyChangedChanged));



        public ScalePosition ScalePosition
        {
            get { return (ScalePosition)GetValue(ScalePositionProperty); }
            set { SetValue(ScalePositionProperty, value); }
        }

        public static readonly DependencyProperty ScalePositionProperty =
            DependencyProperty.Register("ScalePosition", typeof(ScalePosition), typeof(BulletGraph), new PropertyMetadata(ScalePosition.Default,OnPropertyChangedChanged));



        public double MajorTickSize
        {
            get { return (double)GetValue(MajorTickSizeProperty); }
            set { SetValue(MajorTickSizeProperty, value); }
        }

        public static readonly DependencyProperty MajorTickSizeProperty =
            DependencyProperty.Register("MajorTickSize", typeof(double), typeof(BulletGraph), new PropertyMetadata(10d,OnPropertyChangedChanged));



        public Style MajorTickLineSyle
        {
            get { return (Style)GetValue(MajorTickLineSyleProperty); }
            set { SetValue(MajorTickLineSyleProperty, value); }
        }

        public static readonly DependencyProperty MajorTickLineSyleProperty =
            DependencyProperty.Register("MajorTickLineSyle", typeof(Style), typeof(BulletGraph), new PropertyMetadata(null));



        public double MinorTickSize
        {
            get { return (double)GetValue(MinorTickSizeProperty); }
            set { SetValue(MinorTickSizeProperty, value); }
        }

        public static readonly DependencyProperty MinorTickSizeProperty =
            DependencyProperty.Register("MinorTickSize", typeof(double), typeof(BulletGraph), new PropertyMetadata(5d));



        public Style MinorTickLineStyle
        {
            get { return (Style)GetValue(MinorTickLineStyleProperty); }
            set { SetValue(MinorTickLineStyleProperty, value); }
        }

        public static readonly DependencyProperty MinorTickLineStyleProperty =
            DependencyProperty.Register("MinorTickLineStyle", typeof(Style), typeof(BulletGraph), new PropertyMetadata(null));


        internal Rect RangeRect
        {
            get;
            set;
        }

        internal Rect ScaleRect
        {
            get;
            set;
        }

        internal Rect LabelRect
        {
            get;
            set;
        }



        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(BulletGraph), new PropertyMetadata(0d,OnUpdatePropertyChanged));



        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(BulletGraph), new PropertyMetadata(1d,OnUpdatePropertyChanged));


        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(double), typeof(BulletGraph), new PropertyMetadata(0d,OnUpdatePropertyChanged));




        public double PerformanceMeasureThickness
        {
            get { return (double)GetValue(PerformanceMeasureThicknessProperty); }
            set { SetValue(PerformanceMeasureThicknessProperty, value); }
        }

        public static readonly DependencyProperty PerformanceMeasureThicknessProperty =
            DependencyProperty.Register("PerformanceMeasureThickness", typeof(double), typeof(BulletGraph), new PropertyMetadata(6d,OnPerformanceMeasurePropertyChanged));



        public double ComparativeMeasureThickness
        {
            get { return (double)GetValue(ComparativeMeasureThicknessProperty); }
            set { SetValue(ComparativeMeasureThicknessProperty, value); }
        }

        public static readonly DependencyProperty ComparativeMeasureThicknessProperty =
            DependencyProperty.Register("ComparativeMeasureThickness", typeof(double), typeof(BulletGraph), new PropertyMetadata(3d, OnComparativeMeasurePropertyChanged));



        public double PerformanceMeasure
        {
            get { return (double)GetValue(PerformanceMeasureProperty); }
            set { SetValue(PerformanceMeasureProperty, value); }
        }

        public static readonly DependencyProperty PerformanceMeasureProperty =
            DependencyProperty.Register("PerformanceMeasure", typeof(double), typeof(BulletGraph), new PropertyMetadata(0d, OnPerformanceMeasurePropertyChanged));



        public double ComparativeMeasure
        {
            get { return (double)GetValue(ComparativeMeasureProperty); }
            set { SetValue(ComparativeMeasureProperty, value); }
        }

        public static readonly DependencyProperty ComparativeMeasureProperty =
            DependencyProperty.Register("ComparativeMeasure", typeof(double), typeof(BulletGraph), new PropertyMetadata(0d, OnComparativeMeasurePropertyChanged));



        public Brush ComparativeMeasureFill
        {
            get { return (Brush)GetValue(ComparativeMeasureFillProperty); }
            set { SetValue(ComparativeMeasureFillProperty, value); }
        }

        public static readonly DependencyProperty ComparativeMeasureFillProperty =
            DependencyProperty.Register("ComparativeMeasureFill", typeof(Brush), typeof(BulletGraph), new PropertyMetadata(new SolidColorBrush(Colors.Black)));



        public Brush PerformanceMeasureFill
        {
            get { return (Brush)GetValue(PerformanceMeasureFillProperty); }
            set { SetValue(PerformanceMeasureFillProperty, value); }
        }

        public static readonly DependencyProperty PerformanceMeasureFillProperty =
            DependencyProperty.Register("PerformanceMeasureFill", typeof(Brush), typeof(BulletGraph), new PropertyMetadata(new SolidColorBrush(Colors.Black)));



        public double ComparativeMeasureSpacing
        {
            get { return (double)GetValue(ComparativeMeasureSpacingProperty); }
            set { SetValue(ComparativeMeasureSpacingProperty, value); }
        }

        public static readonly DependencyProperty ComparativeMeasureSpacingProperty =
            DependencyProperty.Register("ComparativeMeasureSpacing", typeof(double), typeof(BulletGraph), new PropertyMetadata(0.1d, OnComparativeMeasurePropertyChanged));



        public int MinorTickStep
        {
            get { return (int)GetValue(MinorTickStepProperty); }
            set { SetValue(MinorTickStepProperty, value); }
        }

        public static readonly DependencyProperty MinorTickStepProperty =
            DependencyProperty.Register("MinorTickStep", typeof(int), typeof(BulletGraph), new PropertyMetadata(0,OnUpdatePropertyChanged));


        public QualitativeRangeCollection QualitativeRanges
        {
            get { return (QualitativeRangeCollection)GetValue(QualitativeRangesProperty); }
            set { SetValue(QualitativeRangesProperty, value); }
        }

        public static readonly DependencyProperty QualitativeRangesProperty =
            DependencyProperty.Register("QualitativeRanges", typeof(QualitativeRangeCollection), typeof(BulletGraph), new PropertyMetadata(null, OnQualitativeRangesChanged));

        private static void OnQualitativeRangesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as BulletGraph).QualitativeRangesChanged(args);
        }
        internal void QualitativeRangesChanged(DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue != null)
            {
                foreach (var oldqualitativeRange in (args.OldValue as QualitativeRangeCollection))
                {
                    Rectangle rect = oldqualitativeRange.RangeRect;
                    if (rect != null)
                    {
                        if (this.elementCanvas!=null && this.elementCanvas.Children.Contains(rect))
                        {
                            this.elementCanvas.Children.Remove(rect);
                        }
                    }                    
                }
                (args.OldValue as QualitativeRangeCollection).CollectionChanged -= OnQualitativeRangeCollectionChanged;
            }
            if (args.NewValue != null)
            {
                foreach (var qualitativeRange in (args.NewValue as QualitativeRangeCollection))
                {
                    qualitativeRange.BulletGraph = this;
                    Rectangle rect = qualitativeRange.GetRangeRectangle();
                    if (this.elementCanvas!=null && !this.elementCanvas.Children.Contains(rect))
                    {
                        this.elementCanvas.Children.Add(rect);
                        if (!RangeRect.IsEmpty)
                            qualitativeRange.Refresh(RangeRect);
                    }
                }
                (args.NewValue as QualitativeRangeCollection).CollectionChanged += OnQualitativeRangeCollectionChanged;
            }
        }

        void OnQualitativeRangeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (!this.RangeRect.IsEmpty)
                    {
                        foreach (var range in e.NewItems)
                        {
                            QualitativeRange qualitativeRange = range as QualitativeRange;
                            qualitativeRange.BulletGraph = this;
                            Rectangle rect = qualitativeRange.GetRangeRectangle();
                            if (this.elementCanvas !=null && !this.elementCanvas.Children.Contains(rect))
                            {
                                this.elementCanvas.Children.Add(rect);
                                if (!RangeRect.IsEmpty)
                                    qualitativeRange.Refresh(RangeRect);
                            }
                        }
                    }
                    break;               
                case NotifyCollectionChangedAction.Remove:
                    foreach (var range in e.NewItems)
                    {
                        QualitativeRange qualitativeRange = range as QualitativeRange;
                        qualitativeRange.BulletGraph = this;
                        Rectangle rect = qualitativeRange.RangeRect;
                        if (this.elementCanvas!=null && !this.elementCanvas.Children.Contains(rect))
                        {
                            this.elementCanvas.Children.Add(rect);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (this.elementCanvas != null)
                        this.elementCanvas.Children.Clear();
                    break;
                default:
                    break;
            }
            
        }

        internal double CalculateCoefficient(double value)
        {
            return ((Maximum - Minimum));
        }

        internal void CalculateLabels()
        {
            this.Labels.Clear();
            if (this.Interval > 0)
                for (double i = Minimum; i <= Maximum; i += Interval)
                {
                    Label label = new Label() { Value = i, Text = i.ToString(this.LabelFormat) };
                    Binding templateBinding = new Binding();
                    templateBinding.Source = this;
                    templateBinding.Path = new PropertyPath("LabelTemplate");
                    Binding orientationBinding = new Binding();
                    orientationBinding.Source = this;
                    orientationBinding.Path = new PropertyPath("Orientation");
                    Binding positionBinding = new Binding();
                    positionBinding.Source = this;
                    positionBinding.Path = new PropertyPath("LabelPosition");
                    label.SetBinding(Label.LabelPositionProperty, positionBinding);
                    label.SetBinding(Label.OrientationProperty, orientationBinding);
                    label.SetBinding(Label.LabelTemplateProperty, templateBinding);
                    this.Labels.Add(label);
                }
        }

        

        internal void CalculateMajorAndMinorTicks()
        {
            this.MajorTicks.Clear();
            this.MinorTicks.Clear();
            foreach (var label in Labels)
            {
                Tick majorTick = new Tick() { Value = label.Value };
                Binding tickSizeBinding = new Binding();
                tickSizeBinding.Source = this;
                tickSizeBinding.Path = new PropertyPath("MajorTickSize");
                Binding tickStyleBinding = new Binding();
                tickStyleBinding.Source = this;
                tickStyleBinding.Path = new PropertyPath("MajorTickLineSyle");
                Binding tickPositionBinding = new Binding();
                tickPositionBinding.Source = this;
                tickPositionBinding.Path = new PropertyPath("ScalePosition");
                majorTick.SetBinding(Tick.LineStyleProperty, tickStyleBinding);
                majorTick.SetBinding(Tick.TickSizeProperty, tickSizeBinding);
                majorTick.SetBinding(Tick.TickPositionProperty, tickPositionBinding);
                this.MajorTicks.Add(majorTick);
            }
            double minorTickInterval = (this.Minimum + this.Interval) / (this.MinorTickStep + 1);
            foreach (var majorTick in MajorTicks)
            {                
                if(this.MajorTicks.IndexOf(majorTick)!=this.MajorTicks.Count-1)
                    for (int i = 0; i < this.MinorTickStep; i++)
                    {
                        Tick minorTick = new Tick() { Value = majorTick.Value + (minorTickInterval * (i + 1)) };
                        Binding tickSizeBinding = new Binding();
                        tickSizeBinding.Source = this;
                        tickSizeBinding.Path = new PropertyPath("MinorTickSize");
                        Binding tickStyleBinding = new Binding();
                        tickStyleBinding.Source = this;
                        tickStyleBinding.Path = new PropertyPath("MinorTickLineStyle");
                        Binding tickPositionBinding = new Binding();
                        tickPositionBinding.Source = this;
                        tickPositionBinding.Path = new PropertyPath("ScalePosition");
                        minorTick.SetBinding(Tick.LineStyleProperty, tickStyleBinding);
                        minorTick.SetBinding(Tick.TickSizeProperty, tickSizeBinding);
                        minorTick.SetBinding(Tick.TickPositionProperty, tickPositionBinding);
                        this.MinorTicks.Add(minorTick);
                    }
            }
        }

        internal void Update()
        {
            double firstOffset = 0d;
            foreach (var label in Labels)
            {
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        label.X = this.GetPointFromValue(label.Value, RangeRect);
                        label.Y = this.LabelRect.Y;
                        Rect labelBounds = label.GetLabelRect();
                        ContentControl labelElement = label.LabelElement;
                        if (Labels.IndexOf(label) == 0)
                        {
                            Canvas.SetLeft(labelElement, 0);
                            firstOffset = label.Size.Width / 2;
                        }
                        else
                            Canvas.SetLeft(labelElement, labelBounds.Left);
                        Canvas.SetTop(labelElement, labelBounds.Top);                        
                        break;
                    case Orientation.Vertical:
                        label.X = this.LabelRect.X;
                        label.Y = this.GetPointFromValue(label.Value, RangeRect);
                        Rect labelBounds1 = label.GetLabelRect();
                        ContentControl labelElement1 = label.LabelElement;
                        if (Labels.IndexOf(label) == Labels.Count-1)
                            Canvas.SetTop(labelElement1, 0);
                        else
                            Canvas.SetTop(labelElement1, labelBounds1.Top);
                        if (!(this.LabelPosition == LabelPosition.Default))
                            Canvas.SetLeft(labelElement1, labelBounds1.Left);
                        else
                            Canvas.SetLeft(labelElement1, labelBounds1.Left + LabelRect.Width - label.Size.Width);
                        break;
                }                                                

            }

            foreach (var tick in this.MajorTicks)
            {
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        tick.X1 = this.GetPointFromValue(tick.Value, this.ScaleRect);
                        tick.X2 = tick.X1;
                        tick.Y1 = this.ScaleRect.Top;
                        tick.Y2 = this.ScaleRect.Top + this.MajorTickSize;                       
                        break;
                    case Orientation.Vertical:
                        tick.Y1 = this.GetPointFromValue(tick.Value, this.ScaleRect);
                        tick.Y2 = tick.Y1;
                        tick.X1=this.ScaleRect.Left;
                        tick.X2=this.ScaleRect.Left+this.MajorTickSize;
                        break;
                }
                tick.Refresh();
            }

            foreach (var minorTick in this.MinorTicks)
            {
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        minorTick.X1 = this.GetPointFromValue(minorTick.Value, this.ScaleRect);
                        minorTick.X2 = minorTick.X1;
                        switch (this.ScalePosition)
                        {
                            case ScalePosition.Default:
                                minorTick.Y1 = this.ScaleRect.Top;
                                minorTick.Y2 = this.ScaleRect.Top + this.MinorTickSize;
                                break;
                            case ScalePosition.Cross:
                                minorTick.Y1 = this.ScaleRect.Top + (this.ScaleRect.Height / 2) - (this.MinorTickSize / 2);
                                minorTick.Y2 = (this.ScaleRect.Top + (this.ScaleRect.Height / 2) - (this.MinorTickSize / 2)) + this.MinorTickSize;
                                break;
                            case ScalePosition.Opposed:
                                minorTick.Y1 = this.ScaleRect.Top + (this.ScaleRect.Height - this.MinorTickSize);
                                minorTick.Y2 = (this.ScaleRect.Top + (this.ScaleRect.Height - this.MinorTickSize)) + this.MinorTickSize;
                                break;
                            default:
                                break;
                        }
                        
                        break;
                    case Orientation.Vertical:
                        minorTick.Y1 = this.GetPointFromValue(minorTick.Value, this.ScaleRect);
                        minorTick.Y2 = minorTick.Y1;
                        switch (this.ScalePosition)
                        {
                            case ScalePosition.Opposed:
                                minorTick.X1 = this.ScaleRect.Left;
                                minorTick.X2 = this.ScaleRect.Left + this.MinorTickSize;
                                break;
                            case ScalePosition.Cross:
                                minorTick.X1 = this.ScaleRect.Left + (this.ScaleRect.Width / 2) - (this.MinorTickSize / 2);
                                minorTick.X2 = (this.ScaleRect.Left + (this.ScaleRect.Width / 2) - (this.MinorTickSize / 2)) + this.MinorTickSize;
                                break;
                            case ScalePosition.Default:
                                minorTick.X1 = this.ScaleRect.Left + (this.ScaleRect.Width - this.MinorTickSize);
                                minorTick.X2 = (this.ScaleRect.Left + (this.ScaleRect.Width - this.MinorTickSize)) + this.MinorTickSize;
                                break;
                            default:
                                break;
                        }
                        break;
                }

                minorTick.Refresh();
            }

            foreach (var range in QualitativeRanges)
            {
                range.Refresh(this.RangeRect);
            }
            this.PerformanceManager.Refresh(this.RangeRect);
            this.ComparativeManager.Refresh(this.RangeRect);
        }

        internal double GetPointFromValue(double value, Rect renderedRect)
        {
            double coefficient = this.CalculateCoefficient(this.Maximum);
            switch (this.Orientation)
            {
                case Orientation.Horizontal:
                    return ((value - this.Minimum) * (renderedRect.Width / coefficient)) + renderedRect.Left;
                case Orientation.Vertical:
                    return (renderedRect.Height - ((value - this.Minimum) * renderedRect.Height) / coefficient) + renderedRect.Top;
            }
            return 0;
        }

        internal void CalculateRects(Size arrangeSize)
        {
            double maxWidth = 0d;
            double maxHeight = 0d;
            double firstWidth = 0d;
            double firstHeight = 0d;
            double lastHeight = 0d;
            double lastWidth = 0d;
            double tickSize = 0;
            foreach (var label in Labels)
            {
                if (Labels.IndexOf(label) == 0)
                {
                    firstWidth = label.Size.Width;
                    firstHeight = label.Size.Height;
                }
                if (Labels.IndexOf(label) == Labels.Count - 1)
                {
                    lastHeight = label.Size.Height;
                    lastWidth = label.Size.Width;
                }
                maxWidth = Math.Max(maxWidth, label.Size.Width);
                maxHeight = Math.Max(maxHeight, label.Size.Height);
            }
            if (this.MajorTicks.Count > 0 || this.MinorTicks.Count > 0)
            {
                tickSize = Math.Max(this.MajorTickSize, this.MinorTickSize);
            }

            switch (Orientation)
            {
                case Orientation.Horizontal:
                    switch (LabelPosition)
                    {
                        case LabelPosition.Default:
                            switch (this.ScalePosition)
                            {
                                case ScalePosition.Default:
                                    this.RangeRect = new Rect((firstWidth / 2), 0, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, (arrangeSize.Height - maxHeight - tickSize) >= 0 ? (arrangeSize.Height - maxHeight - tickSize) : 0d);
                                    this.LabelRect = new Rect((firstWidth / 2), (arrangeSize.Height - maxHeight) >= 0 ? (arrangeSize.Height - maxHeight) : 0d, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, maxHeight);
                                    this.ScaleRect = new Rect((firstWidth / 2), (arrangeSize.Height - maxHeight - tickSize) >= 0 ? (arrangeSize.Height - maxHeight - tickSize) : 0d, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, tickSize);
                                    break;
                                case ScalePosition.Cross:
                                    this.RangeRect = new Rect((firstWidth / 2), 0, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, (arrangeSize.Height - maxHeight) >= 0 ? (arrangeSize.Height - maxHeight) : 0d);
                                    this.LabelRect = new Rect((firstWidth / 2), (arrangeSize.Height - maxHeight) >= 0 ? (arrangeSize.Height - maxHeight) : 0d, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, maxHeight);
                                    this.ScaleRect = new Rect((firstWidth / 2), (((arrangeSize.Height - maxHeight) >= 0 ? (arrangeSize.Height - maxHeight) : 0d) / 2) - (tickSize / 2), arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, tickSize);
                                    break;
                                case ScalePosition.Opposed:
                                    this.RangeRect = new Rect((firstWidth / 2), tickSize, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, (arrangeSize.Height - maxHeight - tickSize) >= 0 ? (arrangeSize.Height - maxHeight - tickSize) : 0d);
                                    this.LabelRect = new Rect((firstWidth / 2), (arrangeSize.Height - maxHeight) >= 0 ? (arrangeSize.Height - maxHeight) : 0d, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, maxHeight);
                                    this.ScaleRect = new Rect((firstWidth / 2), 0d, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, tickSize);
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case LabelPosition.Cross:
                            switch (this.ScalePosition)
                            {
                                case ScalePosition.Default:
                                    this.RangeRect = new Rect((firstWidth / 2), 0, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, arrangeSize.Height - tickSize);
                                    this.LabelRect = new Rect((firstWidth / 2), ((arrangeSize.Height - tickSize) / 2), arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, maxHeight);
                                    this.ScaleRect = new Rect((firstWidth / 2), (arrangeSize.Height - tickSize) >= 0 ? (arrangeSize.Height - tickSize) : 0d, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, tickSize);
                                    break;
                                case ScalePosition.Cross:
                                    this.RangeRect = new Rect((firstWidth / 2), 0, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, arrangeSize.Height);
                                    this.LabelRect = new Rect((firstWidth / 2), ((arrangeSize.Height) / 2), arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, maxHeight);
                                    this.ScaleRect = new Rect((firstWidth / 2), ((arrangeSize.Height / 2) - (tickSize / 2)) >= 0 ? ((arrangeSize.Height / 2) - (tickSize / 2)) : 0d, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, tickSize);
                                    break;
                                case ScalePosition.Opposed:
                                    this.RangeRect = new Rect((firstWidth / 2), tickSize, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, arrangeSize.Height - tickSize);
                                    this.LabelRect = new Rect((firstWidth / 2), ((arrangeSize.Height) / 2) + tickSize, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, maxHeight);
                                    this.ScaleRect = new Rect((firstWidth / 2), 0d, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, tickSize);
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case LabelPosition.Opposed:
                            switch (this.ScalePosition)
                            {
                                case ScalePosition.Default:
                                    this.RangeRect = new Rect((firstWidth / 2), maxHeight, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, (arrangeSize.Height - maxHeight - tickSize) >= 0 ? (arrangeSize.Height - maxHeight - tickSize) : 0d);
                                    this.LabelRect = new Rect((firstWidth / 2), 0, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, maxHeight);
                                    this.ScaleRect = new Rect((firstWidth / 2), (arrangeSize.Height - tickSize), arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, tickSize);
                                    break;
                                case ScalePosition.Cross:
                                    this.RangeRect = new Rect((firstWidth / 2), maxHeight, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, (arrangeSize.Height - maxHeight) >= 0 ? (arrangeSize.Height - maxHeight) : 0d);
                                    this.LabelRect = new Rect((firstWidth / 2), 0, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, maxHeight);
                                    this.ScaleRect = new Rect((firstWidth / 2), this.RangeRect.Top + (this.RangeRect.Height / 2) - (tickSize / 2), arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, tickSize);
                                    break;
                                case ScalePosition.Opposed:
                                    this.RangeRect = new Rect((firstWidth / 2), maxHeight + tickSize, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, (arrangeSize.Height - maxHeight - tickSize) >= 0 ? (arrangeSize.Height - maxHeight - tickSize) : 0d);
                                    this.LabelRect = new Rect((firstWidth / 2), 0, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, maxHeight);
                                    this.ScaleRect = new Rect((firstWidth / 2), maxHeight, arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) > 0 ? arrangeSize.Width - ((lastWidth / 2) + ((firstWidth / 2))) : 0d, tickSize);
                                    break;
                            }
                            break;
                    }
                    break;
                case Orientation.Vertical:
                    double commonY = (lastHeight / 2);
                    double commonHeight = arrangeSize.Height - (commonY + (firstHeight/2)) > 0 ? arrangeSize.Height - (commonY + (firstHeight/2)) : 0d;

                    switch (LabelPosition)
                    {
                        case LabelPosition.Default:
                            switch (this.ScalePosition)
                            {
                                case ScalePosition.Default:
                                    this.RangeRect = new Rect(maxWidth + tickSize, commonY, (arrangeSize.Width - maxWidth - tickSize > 0 ? arrangeSize.Width - maxWidth - tickSize : 0d), commonHeight);
                                    this.LabelRect = new Rect(0, commonY, maxWidth, commonHeight);
                                    this.ScaleRect = new Rect(maxWidth, commonY, tickSize, commonHeight);
                                    break;
                                case ScalePosition.Cross:
                                    this.RangeRect = new Rect(maxWidth , commonY, (arrangeSize.Width - maxWidth  > 0 ? arrangeSize.Width - maxWidth  : 0d), commonHeight);
                                    this.LabelRect = new Rect(0, commonY, maxWidth, commonHeight);
                                    this.ScaleRect = new Rect(this.RangeRect.Left + (this.RangeRect.Width / 2) - (tickSize / 2), commonY, tickSize, commonHeight);
                                    break;
                                case ScalePosition.Opposed:
                                    this.RangeRect = new Rect(maxWidth, commonY, (arrangeSize.Width - tickSize-maxWidth > 0 ? arrangeSize.Width - tickSize -maxWidth : 0d), commonHeight);
                                    this.LabelRect = new Rect(0, commonY, maxWidth, commonHeight);
                                    this.ScaleRect = new Rect(this.RangeRect.Left + this.RangeRect.Width, commonY, tickSize, commonHeight);
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case LabelPosition.Cross:
                            switch (this.ScalePosition)
                            {
                                case ScalePosition.Default:
                                    this.RangeRect = new Rect(tickSize, commonY, (arrangeSize.Width - tickSize > 0 ? arrangeSize.Width - tickSize  : 0d), commonHeight);
                                    this.LabelRect = new Rect((this.RangeRect.Width) / 2 + tickSize, commonY, maxWidth, commonHeight);
                                    this.ScaleRect = new Rect(0, commonY, tickSize, commonHeight);                                 
                                    break;
                                case ScalePosition.Cross:
                                    this.RangeRect = new Rect(0, commonY, (arrangeSize.Width > 0 ? arrangeSize.Width : 0d), commonHeight);
                                    this.LabelRect = new Rect((this.RangeRect.Width) / 2, commonY, maxWidth, commonHeight);
                                    this.ScaleRect = new Rect((this.RangeRect.Width) / 2 - (tickSize / 2), commonY, tickSize, commonHeight);                                 
                                    break;
                                case ScalePosition.Opposed:
                                   this.RangeRect = new Rect(0, commonY, (arrangeSize.Width - tickSize > 0 ? arrangeSize.Width - tickSize : 0d), commonHeight);
                                   this.LabelRect = new Rect((this.RangeRect.Width) / 2 , commonY, maxWidth, commonHeight);
                                   this.ScaleRect = new Rect((this.RangeRect.Width), commonY, tickSize, commonHeight);                                 
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case LabelPosition.Opposed:
                            switch (this.ScalePosition)
                            {
                                case ScalePosition.Default:
                                    this.RangeRect = new Rect(tickSize, commonY, (arrangeSize.Width - maxWidth - tickSize > 0 ? arrangeSize.Width - maxWidth - tickSize : 0d), commonHeight);
                                    this.LabelRect = new Rect(this.RangeRect.Width+tickSize, commonY, maxWidth, commonHeight);
                                    this.ScaleRect = new Rect(0, commonY, tickSize, commonHeight);
                                    break;
                                case ScalePosition.Cross:
                                    this.RangeRect = new Rect(0, commonY, (arrangeSize.Width - maxWidth  > 0 ? arrangeSize.Width - maxWidth : 0d), commonHeight);
                                    this.LabelRect = new Rect(this.RangeRect.Width, commonY, maxWidth, commonHeight);
                                    this.ScaleRect = new Rect((this.RangeRect.Width / 2) - (tickSize / 2), commonY, tickSize, commonHeight);
                                    break;
                                case ScalePosition.Opposed:
                                    this.RangeRect = new Rect(0, commonY, (arrangeSize.Width - maxWidth - tickSize > 0 ? arrangeSize.Width - maxWidth - tickSize : 0d), commonHeight);
                                    this.LabelRect = new Rect(this.RangeRect.Width + tickSize, commonY, maxWidth, commonHeight);
                                    this.ScaleRect = new Rect(this.RangeRect.Width, commonY, tickSize, commonHeight);
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

    }
}
