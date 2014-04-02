using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
#endif
namespace Sparrow.Chart
{
    /// <summary>
    /// YAxis for Sparrow Chart
    /// </summary>
    public abstract class YAxis : AxisBase
    {
        /// <summary>
        /// Gets the styles.
        /// </summary>
        protected override void GetStyles()
        {
            base.GetStyles();
            this.HeaderTemplate = (DataTemplate)Styles["yAxisHeaderTemplate"];
        }

        /// <summary>
        /// Checks the type.
        /// </summary>
        /// <returns></returns>
        protected override bool CheckType()
        {
            return true; 
        }

        /// <summary>
        /// Invalidates the visuals.
        /// </summary>
        internal override void InvalidateVisuals()
        {
            if (!IsInitialized)
                Initialize();
            else
                Update();
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        internal void Update()
        {
            double desiredWidth = 0;
            CalculateAutoInterval();
            GenerateLabels();
            if (this.ActualHeight > 0 && this.ActualWidth > 0)
            {
                double yAxisHeightStep = this.ActualHeight / ((MIntervalCount > 0) ? MIntervalCount : 1);
                double yAxisHeightPosition = 0;
                Rect oldRect = new Rect(0, 0, 0, 0);
                AxisLine.X1 = this.ActualWidth;
                AxisLine.X2 = this.ActualWidth;
                AxisLine.Y1 = 0;
                AxisLine.Y2 = this.ActualHeight;
                Binding binding = new Binding();
                binding.Path = new PropertyPath("AxisLineStyle");
                binding.Source = this;
                AxisLine.SetBinding(Line.StyleProperty, binding);
                double labelSize = 0;
                int minorCount = 0;
                if (this.MLabels.Count == Labels.Count)
                {
                    for (int i = this.MLabels.Count - 1; i >=0; i--)
                    {
                        yAxisHeightPosition = this.DataToPoint(MLabelValues[i]);                        
                        ContentControl label = Labels[i];
                        label.Content = MLabels[i];
                        label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        RotateTransform labelRotation = new RotateTransform();
                        labelRotation.Angle = LabelAngle;
                        Rect rotatedRect = GetRotatedRect(new Rect(0, 0, label.DesiredSize.Width, label.DesiredSize.Height), labelRotation);
                        label.RenderTransform = labelRotation;  
                        Line tickLine = MajorTickLines[i];
                        double labelPadding = 0;
                        tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        //tickLine.X1 = this.ActualWidth;
                        tickLine.Y1 = yAxisHeightPosition;
                        tickLine.Y2 = yAxisHeightPosition;
                        //tickLine.X2 = tickLine.X1 - MajorLineSize;
                        switch (MajorTicksPosition)
                        {
                            case TickPosition.Inside:
                                tickLine.X1 = this.ActualWidth;
                                tickLine.X2 = tickLine.X1 + MajorLineSize;
                                if (this.ShowMajorTicks)
                                {
                                    labelPadding = 0;
                                    desiredWidth = 0;
                                }
                                break;
                            case TickPosition.Cross:
                                tickLine.X1 = this.ActualWidth - (MajorLineSize / 2);
                                tickLine.X2 = this.ActualWidth + (MajorLineSize / 2);
                                if (this.ShowMajorTicks)
                                {
                                    labelPadding = tickLine.X2;
                                    desiredWidth = this.MajorLineSize / 2;
                                }
                                break;
                            case TickPosition.Outside:
                                tickLine.X1 = this.ActualWidth;
                                tickLine.X2 = tickLine.X1 - MajorLineSize;
                                if (this.ShowMajorTicks)
                                {
                                    labelPadding = tickLine.X2;
                                    desiredWidth = this.MajorLineSize;
                                }
                                break;
                            default:
                                break;
                        }
                        //desiredWidth = 0;
                        if (i != 0)
                        {
                            double minorWidth = yAxisHeightStep;
                            double minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int j = 0; j < this.MinorTicksCount; j++)
                            {
                                Line minorLine = MinorTickLines[minorCount];
                                minorLine.Y1 = (yAxisHeightPosition + minorstep * (j + 1));
                                minorLine.Y2 = (yAxisHeightPosition + minorstep * (j + 1));
                                //minorLine.X1 = this.ActualWidth - (minorLine.StrokeThickness);
                               // minorLine.X2 = minorLine.X1 - MinorLineSize;
                                switch (MinorTicksPosition)
                                {
                                    case TickPosition.Inside:
                                        minorLine.X1 = this.ActualWidth;
                                        minorLine.X2 = minorLine.X1 + MinorLineSize;
                                        break;
                                    case TickPosition.Cross:
                                        minorLine.X1 = this.ActualWidth - (MinorLineSize / 2);
                                        minorLine.X2 = this.ActualWidth + (MinorLineSize / 2);
                                        break;
                                    case TickPosition.Outside:
                                        minorLine.X1 = this.ActualWidth;
                                        minorLine.X2 = minorLine.X1 - MinorLineSize;
                                        break;
                                    default:
                                        break;
                                }
                                minorCount++;
                            }
                        }

                        if (this.LabelAngle == 0)
                        {
                            Canvas.SetLeft(label, this.ActualWidth - (label.DesiredSize.Width) - desiredWidth - 1);
                            Canvas.SetTop(label, yAxisHeightPosition - (label.DesiredSize.Height / 2));
                            labelSize = Math.Max(labelSize, label.DesiredSize.Width);
                        }
                        else
                        {
                            Canvas.SetLeft(label, this.ActualWidth - (rotatedRect.Width)-rotatedRect.X - desiredWidth - 1);
                            Canvas.SetTop(label, yAxisHeightPosition - (rotatedRect.Height / 2)-rotatedRect.Y);
                            labelSize = Math.Max(labelSize, rotatedRect.Width);
                        }           
                    }
                }
                else
                {
                    if (this.MLabels.Count > Labels.Count)
                    {
                        int offset = this.MLabels.Count - Labels.Count;
                        for (int j = 0; j < offset; j++)
                        {
                            ContentControl label = new ContentControl();
                            label.Content = MLabels[this.MLabels.Count - offset - 1];
                            //label.ContentTemplate = this.LabelTemplate;
                            Binding labelTemplateBinding = new Binding();
                            labelTemplateBinding.Path = new PropertyPath("LabelTemplate");
                            labelTemplateBinding.Source = this;
                            label.SetBinding(ContentControl.ContentTemplateProperty, labelTemplateBinding);
                            label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                            RotateTransform labelRotation = new RotateTransform();
                            labelRotation.Angle = LabelAngle;
                            Rect rotatedRect = GetRotatedRect(new Rect(0, 0, label.DesiredSize.Width, label.DesiredSize.Height), labelRotation);
                            label.RenderTransform = labelRotation;  
                            Labels.Add(label);
                            Line tickLine = new Line();
                            Binding styleBinding = new Binding();
                            styleBinding.Path = new PropertyPath("MajorLineStyle");
                            styleBinding.Source = this;
                            tickLine.SetBinding(Line.StyleProperty, styleBinding);
                            tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));                           
                            tickLine.Y1 = yAxisHeightPosition;
                            tickLine.Y2 = yAxisHeightPosition;
                            //tickLine.X2 = tickLine.X1 - MajorLineSize; 
                            switch (MajorTicksPosition)
                            {
                                case TickPosition.Inside:
                                    tickLine.X1 = this.ActualWidth;
                                    tickLine.X2 = tickLine.X1 + MajorLineSize;
                                    break;
                                case TickPosition.Cross:
                                    tickLine.X1 = this.ActualWidth - (MajorLineSize / 2);
                                    tickLine.X2 = this.ActualWidth + (MajorLineSize / 2);
                                    break;
                                case TickPosition.Outside:
                                    tickLine.X1 = this.ActualWidth;
                                    tickLine.X2 = tickLine.X1 - MajorLineSize;
                                    break;
                                default:
                                    break;
                            }
                            //Binding tickSizeBinding = new Binding();
                            //tickSizeBinding.Path = new PropertyPath("MajorLineSize");
                            //tickSizeBinding.Source = this;
                            //tickSizeBinding.Converter = new MajorSizeThicknessConverter();
                            //tickSizeBinding.ConverterParameter = tickLine.X1;
                            //tickLine.SetBinding(Line.X2Property, tickSizeBinding);
                            Binding ticklineVisibilityBinding = new Binding();
                            ticklineVisibilityBinding.Path = new PropertyPath("ShowMajorTicks");
                            ticklineVisibilityBinding.Source = this;
                            ticklineVisibilityBinding.Converter = new BooleanToVisibilityConverter();
                            tickLine.SetBinding(Line.VisibilityProperty, ticklineVisibilityBinding);
                            double minorWidth = yAxisHeightStep;
                            double minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int k = 0; k < this.MinorTicksCount; j++)
                            {
                                Line minorLine = new Line();
                                Binding minorstyleBinding = new Binding();
                                minorstyleBinding.Path = new PropertyPath("MinorLineStyle");
                                minorstyleBinding.Source = this;
                                minorLine.SetBinding(Line.StyleProperty, minorstyleBinding);
                                minorLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                                minorLine.Y1 = (yAxisHeightPosition + minorstep * (j + 1));
                                minorLine.Y2 = (yAxisHeightPosition + minorstep * (j + 1));                             
                                switch (MinorTicksPosition)
                                {
                                    case TickPosition.Inside:
                                        minorLine.X1 = this.ActualWidth;
                                        minorLine.X2 = minorLine.X1 + MinorLineSize;
                                        break;
                                    case TickPosition.Cross:
                                        minorLine.X1 = this.ActualWidth - (MinorLineSize / 2);
                                        minorLine.X2 = this.ActualWidth + (MinorLineSize / 2);
                                        break;
                                    case TickPosition.Outside:
                                        minorLine.X1 = this.ActualWidth;
                                        minorLine.X2 = minorLine.X1 - MinorLineSize;
                                        break;
                                    default:
                                        break;
                                }
                                MinorTickLines.Add(minorLine);
                                this.Children.Add(minorLine);
                            }
                            this.Children.Add(label);
                            MajorTickLines.Add(tickLine);
                            this.Children.Add(tickLine);
                        }
                    }
                    else
                    {
                        int offset = Labels.Count - this.MLabels.Count;
                        for (int j = 0; j < offset; j++)
                        {
                            this.Children.Remove(Labels[Labels.Count - 1]);
                            Labels.RemoveAt(Labels.Count - 1);
                            for (int k = 0; k < this.MinorTicksCount; k++)
                            {
                                this.Children.Remove(MinorTickLines[MinorTickLines.Count - 1]);
                                MinorTickLines.RemoveAt(MinorTickLines.Count - 1);
                            }
                            this.Children.Remove(MajorTickLines[MajorTickLines.Count - 1]);
                            MajorTickLines.RemoveAt(MajorTickLines.Count - 1);
                        }
                    }
                    for (int i = this.MLabels.Count - 1; i >= 0; i--)
                    {
                        ContentControl label = Labels[i];
                        label.Content = MLabels[i];
                        label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        RotateTransform labelRotation = new RotateTransform();
                        labelRotation.Angle = LabelAngle;
                        Rect rotatedRect = GetRotatedRect(new Rect(0, 0, label.DesiredSize.Width, label.DesiredSize.Height), labelRotation);
                        label.RenderTransform = labelRotation;  
                        Line tickLine = MajorTickLines[i];
                        double labelPadding = 0;
                       
                        tickLine.Y1 = yAxisHeightPosition;
                        tickLine.Y2 = yAxisHeightPosition;
                        switch (MajorTicksPosition)
                        {
                            case TickPosition.Inside:
                                tickLine.X1 = this.ActualWidth;
                                tickLine.X2 = tickLine.X1 + MajorLineSize;
                                if (this.ShowMajorTicks)
                                {
                                    labelPadding = 0;
                                    desiredWidth = 0;
                                }
                                break;
                            case TickPosition.Cross:
                                tickLine.X1 = this.ActualWidth - (MajorLineSize / 2);
                                tickLine.X2 = this.ActualWidth + (MajorLineSize / 2);
                                if (this.ShowMajorTicks)
                                {
                                    labelPadding = tickLine.X2;
                                    desiredWidth = this.MajorLineSize / 2;
                                }
                                break;
                            case TickPosition.Outside:
                                tickLine.X1 = this.ActualWidth;
                                tickLine.X2 = tickLine.X1 - MajorLineSize;
                                if (this.ShowMajorTicks)
                                {
                                    labelPadding = tickLine.X2;
                                    desiredWidth = this.MajorLineSize;
                                }
                                break;
                            default:
                                break;
                        }
                        tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        //desiredWidth = 0;
                        double minorstep = 0;
                        if (!(i == 0))
                        {
                            double minorWidth = yAxisHeightStep;
                            minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int j = 0; j < this.MinorTicksCount; j++)
                            {
                                Line minorLine = MinorTickLines[minorCount];
                                minorLine.Y1 = (yAxisHeightPosition + minorstep * (j + 1));
                                minorLine.Y2 = (yAxisHeightPosition + minorstep * (j + 1));
                                //minorLine.X1 = this.ActualWidth - (minorLine.StrokeThickness);
                                //minorLine.X2 = minorLine.X1 - MinorLineSize;
                                switch (MinorTicksPosition)
                                {
                                    case TickPosition.Inside:
                                        minorLine.X1 = this.ActualWidth;
                                        minorLine.X2 = minorLine.X1 + MinorLineSize;
                                        break;
                                    case TickPosition.Cross:
                                        minorLine.X1 = this.ActualWidth - (MinorLineSize / 2);
                                        minorLine.X2 = this.ActualWidth + (MinorLineSize / 2);
                                        break;
                                    case TickPosition.Outside:
                                        minorLine.X1 = this.ActualWidth;
                                        minorLine.X2 = minorLine.X1 - MinorLineSize;
                                        break;
                                    default:
                                        break;
                                }
                                minorCount++;
                            }
                        }

                        if (this.LabelAngle == 0)
                        {
                            Canvas.SetLeft(label, this.ActualWidth - (label.DesiredSize.Width) - desiredWidth - 1);
                            Canvas.SetTop(label, yAxisHeightPosition - (label.DesiredSize.Height / 2));
                            labelSize = Math.Max(labelSize, label.DesiredSize.Width);
                        }
                        else
                        {
                            Canvas.SetLeft(label, this.ActualWidth - (rotatedRect.Width - rotatedRect.X) - desiredWidth - 1);
                            Canvas.SetTop(label, yAxisHeightPosition - (rotatedRect.Height / 2) - rotatedRect.Y);
                            labelSize = Math.Max(labelSize, rotatedRect.Width);
                        }                        
                        yAxisHeightPosition += yAxisHeightStep;
                        //desiredWidth = desiredWidth + labelSize;
                    }
                }
                header.Measure(new Size(this.ActualHeight, this.ActualWidth));
                Canvas.SetLeft(header, this.ActualWidth - labelSize - header.DesiredSize.Height - this.MajorLineSize - 1);
                Canvas.SetTop(header, this.ActualHeight / 2);
                desiredWidth = desiredWidth+ header.DesiredSize.Height + labelSize;
            }
            if (this.Chart.AxisWidth < desiredWidth)
                this.Chart.AxisWidth = desiredWidth + 1;
        }
        private void Initialize()
        {
            double desiredWidth = 0;
            CalculateAutoInterval();
            GenerateLabels();            
            if (this.ActualHeight > 0 && this.ActualWidth > 0)
            {
               
                this.Children.Clear();
                double yAxisHeightStep = this.ActualHeight / ((MIntervalCount > 0) ? MIntervalCount : 1);
                double yAxisHeightPosition = this.DataToPoint(MStartValue);
                Rect oldRect = new Rect(0, 0, 0, 0);
                AxisLine = new Line();
                Binding binding = new Binding();
                binding.Path = new PropertyPath("AxisLineStyle");
                binding.Source = this;
                AxisLine.SetBinding(Line.StyleProperty, binding);
                AxisLine.X1 = this.ActualWidth;
                AxisLine.X2 = this.ActualWidth;
                AxisLine.Y1 = 0;
                AxisLine.Y2 = this.ActualHeight;
                AxisLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                Labels = new List<ContentControl>();
                MajorTickLines = new List<Line>();
                MinorTickLines = new List<Line>();
                double labelSize = 0;
                for (int i = this.MLabels.Count - 1; i >= 0; i--)
                {
                    ContentControl label = new ContentControl();
                    label.Content = MLabels[i];
                    //label.ContentTemplate = this.LabelTemplate;
                    Binding labelTemplateBinding = new Binding();
                    labelTemplateBinding.Path = new PropertyPath("LabelTemplate");
                    labelTemplateBinding.Source = this;
                    label.SetBinding(ContentControl.ContentTemplateProperty, labelTemplateBinding);
                    label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                    RotateTransform labelRotation = new RotateTransform();
                    labelRotation.Angle = LabelAngle;
                    Rect rotatedRect = GetRotatedRect(new Rect(0, 0, label.DesiredSize.Width, label.DesiredSize.Height), labelRotation);
                    label.RenderTransform = labelRotation;      
                    Labels.Add(label);
                    Line tickLine = new Line();
                    double labelPadding = 0;
                    Binding styleBinding = new Binding();
                    styleBinding.Path = new PropertyPath("MajorLineStyle");
                    styleBinding.Source = this;
                    tickLine.SetBinding(Line.StyleProperty, styleBinding);
                    //tickLine.Style = MajorLineStyle;
                    tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                    
                    tickLine.Y1 = yAxisHeightPosition;
                    tickLine.Y2 = yAxisHeightPosition;
                    
                    switch (MajorTicksPosition)
                    {
                        case TickPosition.Inside:
                            tickLine.X1 = this.ActualWidth;
                            tickLine.X2 = tickLine.X1 + MajorLineSize;
                            if (this.ShowMajorTicks)
                            {
                                labelPadding = 0;
                                desiredWidth = 0;
                            }
                            break;
                        case TickPosition.Cross:
                            tickLine.X1 = this.ActualWidth -(MajorLineSize / 2);
                            tickLine.X2 = this.ActualWidth + (MajorLineSize / 2);
                            if (this.ShowMajorTicks)
                            {
                                labelPadding = tickLine.X2;
                                desiredWidth = this.MajorLineSize / 2;
                            }
                            break;
                        case TickPosition.Outside:
                            tickLine.X1 = this.ActualWidth;
                            tickLine.X2 = tickLine.X1 - MajorLineSize;
                            if (this.ShowMajorTicks)
                            {
                                labelPadding = tickLine.X2;
                                desiredWidth = this.MajorLineSize;
                            }
                            break;
                        default:
                            break;
                    }
                    Binding ticklineVisibilityBinding = new Binding();
                    ticklineVisibilityBinding.Path = new PropertyPath("ShowMajorTicks");
                    ticklineVisibilityBinding.Source = this;
                    ticklineVisibilityBinding.Converter = new BooleanToVisibilityConverter();
                    tickLine.SetBinding(Line.VisibilityProperty, ticklineVisibilityBinding);
                    MajorTickLines.Add(tickLine);
                    this.Children.Add(tickLine);
                    //desiredWidth = 0;
                    double minorstep = 0;
                    if (!(i == 0))
                    {
                        double minorWidth = yAxisHeightStep;
                        minorstep = minorWidth / (MinorTicksCount + 1);
                        for (int j = 0; j < this.MinorTicksCount; j++)
                        {
                            Line minorLine = new Line();
                            Binding minorstyleBinding = new Binding();
                            minorstyleBinding.Path = new PropertyPath("MinorLineStyle");
                            minorstyleBinding.Source = this;
                            minorLine.SetBinding(Line.StyleProperty, minorstyleBinding);
                            minorLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                            minorLine.Y1 = (yAxisHeightPosition + minorstep * (j + 1));
                            minorLine.Y2 = (yAxisHeightPosition + minorstep * (j + 1));                           
                           
                            switch (MinorTicksPosition)
                            {
                                case TickPosition.Inside:
                                    minorLine.X1 = this.ActualWidth;
                                    minorLine.X2 = minorLine.X1 + MinorLineSize;
                                    break;
                                case TickPosition.Cross:
                                    minorLine.X1 = this.ActualWidth - (MinorLineSize / 2);
                                    minorLine.X2 = this.ActualWidth + (MinorLineSize / 2);
                                    break;
                                case TickPosition.Outside:
                                    minorLine.X1 = this.ActualWidth;
                                    minorLine.X2 = minorLine.X1 - MinorLineSize;
                                    break;
                                default:
                                    break;
                            }
                            MinorTickLines.Add(minorLine);
                            this.Children.Add(minorLine);
                        }
                    }

                    if (this.LabelAngle == 0)
                    {
                        Canvas.SetLeft(label, this.ActualWidth - (label.DesiredSize.Width) - desiredWidth - 1);
                        Canvas.SetTop(label, yAxisHeightPosition - (label.DesiredSize.Height / 2));
                        labelSize = Math.Max(labelSize, label.DesiredSize.Width);
                    }
                    else
                    {
                        Canvas.SetLeft(label, this.ActualWidth - (rotatedRect.Width-rotatedRect.X) - desiredWidth - 1);
                        Canvas.SetTop(label, yAxisHeightPosition - (rotatedRect.Height / 2) - rotatedRect.Y);
                        labelSize = Math.Max(labelSize, rotatedRect.Width);
                    }
                    
                    this.Children.Add(label);
                    yAxisHeightPosition += yAxisHeightStep;
                }
                header = new ContentControl();
                header.DataContext = null;
                header.Content = this.Header;
                //header.ContentTemplate = this.HeaderTemplate;
                Binding contentBinding = new Binding();
                contentBinding.Path = new PropertyPath("Header");
                contentBinding.Source = this;
                header.SetBinding(ContentControl.ContentProperty, contentBinding);
                Binding headerTemplateBinding = new Binding();
                headerTemplateBinding.Path = new PropertyPath("HeaderTemplate");
                headerTemplateBinding.Source = this;
                header.SetBinding(ContentControl.ContentTemplateProperty, headerTemplateBinding);
                header.Measure(new Size(this.ActualHeight, this.ActualWidth));
                desiredWidth += labelSize;
                Canvas.SetLeft(header, this.ActualWidth - labelSize - header.DesiredSize.Height - this.MajorLineSize - 1);
                Canvas.SetTop(header, this.ActualHeight / 2);
                desiredWidth = desiredWidth + header.DesiredSize.Height ;
                this.Children.Add(header);
                this.Children.Add(AxisLine);
                IsInitialized = true;
                this.Chart.AxisWidth = desiredWidth;
            }
                           
        }

        /// <summary>
        /// Datas to point.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override double DataToPoint(double value)
        {
            if (!(MMinValue == MMaxValue))
                return (this.ActualHeight - ((value - MMinValue) * this.ActualHeight) / (MMaxValue - MMinValue));
            else
                return 0;
        }

        /// <summary>
        /// Calculates the interval from series points.
        /// </summary>
        public override void CalculateIntervalFromSeriesPoints()
        {
            bool isContainsColumn = false;
            List<double> yValues = new List<double>();
            if (this.Series != null)
                foreach (SeriesBase series in Series)
                {
                    if (series.Points != null)
                    {
                        yValues.AddRange(series.Points.Select(point => point.YValue));
                    }
                    if (series is ColumnSeries || series is AreaSeries)
                        isContainsColumn = true;
                }
            if (yValues.Count > 1)
            {
                double min, max;
                min = yValues.ToArray().Min();
                max = yValues.ToArray().Max();
                if (isContainsColumn && min > 0)
                {
                    min = 0;
                }
                this.AddMinMax(min, max); 
            }
        }

        /// <summary>
        /// Gets the original label.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        override public string GetOriginalLabel(double value)
        {
            switch (Type)
            {
                case YType.Double:
                    return value.ToString(this.StringFormat);
                case YType.DateTime:
#if !WINRT
                    return DateTime.FromOADate(value).ToString(this.StringFormat);
#else
                    return value.FromOADate().ToString(this.StringFormat);
#endif
                default:
                    return value.ToString(this.StringFormat);
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        internal YType Type
        {
            get { return (YType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// The type property
        /// </summary>
        internal static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(YType), typeof(YAxis), new PropertyMetadata(YType.Double,OnTypeChanged));

        /// <summary>
        /// Called when [type changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var yAxis = sender as YAxis;
            if (yAxis != null) yAxis.TypeChanged(args);
        }

        /// <summary>
        /// Types the changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void TypeChanged(DependencyPropertyChangedEventArgs args)
        {
            switch (Type)
            {
                case YType.Double:
#if WPF
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), YType.Double.ToString());
#else
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), YType.Double.ToString(),false);
#endif
                    break;
                case YType.DateTime:
#if WPF
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), YType.DateTime.ToString());
#else
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), YType.DateTime.ToString(),false);
#endif
                    break;  
            }
        }

    }
}
