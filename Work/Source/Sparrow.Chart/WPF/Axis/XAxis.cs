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
    /// XAxis
    /// </summary>
    public abstract class XAxis : AxisBase
    {
        /// <summary>
        /// Checks the Axis type.
        /// </summary>
        /// <returns>Axis Value Type</returns>
        protected override bool CheckType()
        {
            if (this.Type == XType.Category)
                return false;
            return true;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        internal void Update()
        {
            double desiredHeight = 0;
            double labelSize = 0;
            CalculateAutoInterval();
            GenerateLabels();
            if (this.ActualHeight > 0 && this.ActualWidth > 0)
            {
                double xAxisWidthStep = this.ActualWidth / ((MIntervalCount > 0) ? MIntervalCount : 1);
                double xAxisWidthPosition = 0;
                double minorstep = 0;
                AxisLine.X2 = this.ActualWidth;                
                Rect oldRect = new Rect(0, 0, 0, 0);
               
                if (this.MLabels.Count == Labels.Count)
                {
                    int k = 0;
                    int minorCount = 0;
                    for (int i = 0; i < this.MLabels.Count; i++)
                    {
                        xAxisWidthPosition = this.DataToPoint(MStartValue + (MInterval * k));
                        ContentControl label = Labels[k];
                        label.Content = MLabels[k];
                        label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        RotateTransform labelRotation = new RotateTransform();
                        labelRotation.Angle = LabelAngle;                        
                        Rect originalRect=new Rect(0, 0, label.DesiredSize.Width, label.DesiredSize.Height);
                        
                        Rect rotatedRect = GetRotatedRect(originalRect, labelRotation);
                        //label.RenderTransformOrigin = new Point(0.5, 0.5);
                        label.RenderTransform = labelRotation;  
                        Line tickLine = MajorTickLines[k];
                        double labelPadding = 0;
                        tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        tickLine.X1 = xAxisWidthPosition ;
                        tickLine.X2 = xAxisWidthPosition ;
                        switch (MajorTicksPosition)
                        {
                            case TickPosition.Inside:
                                if (this.ShowMajorTicks)
                                {
                                    labelPadding = 0;
                                    desiredHeight = 0;
                                }
                                break;
                            case TickPosition.Cross:
                                if (this.ShowMajorTicks)
                                {
                                    labelPadding = tickLine.Y2;
                                    desiredHeight = tickLine.Y2;
                                }
                                break;
                            case TickPosition.Outside:
                                if (this.ShowMajorTicks)
                                {
                                    labelPadding = tickLine.Y2;
                                    desiredHeight = tickLine.Y2;
                                }
                                break;
                            default:
                                break;
                        }
                        if (!(i == this.MLabels.Count - 1))
                        {
                            double minorWidth = xAxisWidthStep;
                            minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int j = 0; j < this.MinorTicksCount; j++)
                            {
                                Line minorLine = MinorTickLines[minorCount];
                                minorLine.X1 = (xAxisWidthPosition + minorstep * (j + 1));
                                minorLine.X2 = (xAxisWidthPosition + minorstep * (j + 1));
                                switch (MinorTicksPosition)
                                {
                                    case TickPosition.Inside:
                                        minorLine.Y1 = 0;  
                                        break;
                                    case TickPosition.Cross:
                                        //minorLine.Y1 = MinorLineSize / 2;  
                                        break;
                                    case TickPosition.Outside:
                                        minorLine.Y1 = 0;  
                                        break;
                                    default:
                                        break;
                                }
                                       
                                minorCount++;
                            }
                        }
                        
                        //Canvas.SetLeft(label, xAxisWidthPosition - (label.DesiredSize.Width / 2));
                        //Canvas.SetTop(label, desiredHeight);                        
                        if (this.LabelAngle == 0)
                        {
                            Canvas.SetLeft(label, xAxisWidthPosition - (label.DesiredSize.Width / 2));
                            Canvas.SetTop(label, desiredHeight);
                            labelSize = Math.Max(labelSize, label.DesiredSize.Height);  
                        }
                        else
                        {
                            Canvas.SetLeft(label, xAxisWidthPosition - (rotatedRect.Width / 2) - rotatedRect.X);
                            Canvas.SetTop(label, desiredHeight - rotatedRect.Y);
                            labelSize = Math.Max(labelSize, rotatedRect.Height);  
                           
                        }
                        k++;
                       
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
                            Binding labelTemplateBinding = new Binding();
                            labelTemplateBinding.Path = new PropertyPath("LabelTemplate");
                            labelTemplateBinding.Source = this;
                            label.SetBinding(ContentControl.ContentTemplateProperty, labelTemplateBinding);
                            label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                            RotateTransform labelRotation = new RotateTransform();
                            labelRotation.Angle = LabelAngle;
                            Rect rotatedRect = GetRotatedRect(new Rect(0, 0, label.DesiredSize.Width, label.DesiredSize.Height), labelRotation);
                           // label.RenderTransformOrigin = new Point(0.5, 0.5);
                            label.RenderTransform = labelRotation;      
                            Labels.Add(label);
                            Line tickLine = new Line();
                            Binding styleBinding = new Binding();
                            styleBinding.Path = new PropertyPath("MajorLineStyle");
                            styleBinding.Source = this;
                            tickLine.SetBinding(Line.StyleProperty, styleBinding);
                            tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                            tickLine.X1 = xAxisWidthPosition - (tickLine.DesiredSize.Width / 2);
                            tickLine.X2 = xAxisWidthPosition - (tickLine.DesiredSize.Width / 2);

                            switch (this.MajorTicksPosition)
                            {
                                case TickPosition.Inside:
                                    tickLine.Y1 = 0;
                                    Binding tickSizeInsideBinding = new Binding();
                                    tickSizeInsideBinding.Path = new PropertyPath("MajorLineSize");
                                    tickSizeInsideBinding.Source = this;
                                    tickSizeInsideBinding.Converter = new NegativeConverter();
                                    tickLine.SetBinding(Line.Y2Property, tickSizeInsideBinding);
                                    if (this.ShowMajorTicks)
                                    {
                                        desiredHeight = 0;
                                    }
                                    break;
                                case TickPosition.Cross:
                                    Binding tickSizeNegativeCrossBinding = new Binding();
                                    tickSizeNegativeCrossBinding.Path = new PropertyPath("MajorLineSize");
                                    tickSizeNegativeCrossBinding.Source = this;
                                    tickSizeNegativeCrossBinding.Converter = new NegativeHalfConverter();
                                    tickLine.SetBinding(Line.Y1Property, tickSizeNegativeCrossBinding);
                                    Binding tickSizeCrossBinding = new Binding();
                                    tickSizeCrossBinding.Path = new PropertyPath("MajorLineSize");
                                    tickSizeCrossBinding.Source = this;
                                    tickSizeCrossBinding.Converter = new HalfValueConverter();
                                    tickLine.SetBinding(Line.Y2Property, tickSizeCrossBinding);
                                    if (this.ShowMajorTicks)
                                    {
                                        desiredHeight = MajorLineSize / 2;
                                    }
                                    break;
                                case TickPosition.Outside:
                                    tickLine.Y1 = 0;
                                    Binding tickSizeBinding = new Binding();
                                    tickSizeBinding.Path = new PropertyPath("MajorLineSize");
                                    tickSizeBinding.Source = this;
                                    tickLine.SetBinding(Line.Y2Property, tickSizeBinding);
                                    if (this.ShowMajorTicks)
                                    {
                                        desiredHeight = MajorLineSize;
                                    }
                                    break;                               
                            }
                            
                            Binding ticklineVisibilityBinding = new Binding();
                            ticklineVisibilityBinding.Path = new PropertyPath("ShowMajorTicks");
                            ticklineVisibilityBinding.Source = this;
                            ticklineVisibilityBinding.Converter = new BooleanToVisibilityConverter();
                            tickLine.SetBinding(Line.VisibilityProperty, ticklineVisibilityBinding);
                            double minorWidth = xAxisWidthStep;
                            minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int k = 0; k < this.MinorTicksCount; k++)
                            {
                                Line minorLine = new Line();
                                Binding minorstyleBinding = new Binding();
                                minorstyleBinding.Path = new PropertyPath("MinorLineStyle");
                                minorstyleBinding.Source = this;
                                minorLine.SetBinding(Line.StyleProperty, minorstyleBinding);
                                minorLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                                minorLine.X1 = (xAxisWidthPosition + minorstep);
                                minorLine.X2 = (xAxisWidthPosition + minorstep);
                                switch (MinorTicksPosition)
                                {
                                    case TickPosition.Inside:
                                        minorLine.Y1 = 0;
                                        Binding minortickInsideSizeBinding = new Binding();
                                        minortickInsideSizeBinding.Path = new PropertyPath("MinorLineSize");
                                        minortickInsideSizeBinding.Source = this;
                                        minortickInsideSizeBinding.Converter = new NegativeConverter();
                                        minorLine.SetBinding(Line.Y2Property, minortickInsideSizeBinding);
                                        break;
                                    case TickPosition.Cross:
                                        Binding minortickNegativeCrossSizeBinding = new Binding();
                                        minortickNegativeCrossSizeBinding.Path = new PropertyPath("MinorLineSize");
                                        minortickNegativeCrossSizeBinding.Source = this;
                                        minortickNegativeCrossSizeBinding.Converter = new NegativeHalfConverter();
                                        minorLine.SetBinding(Line.Y1Property, minortickNegativeCrossSizeBinding);

                                        Binding minortickCrossSizeBinding = new Binding();
                                        minortickCrossSizeBinding.Path = new PropertyPath("MinorLineSize");
                                        minortickCrossSizeBinding.Source = this;
                                        minortickCrossSizeBinding.Converter = new HalfValueConverter();
                                        minorLine.SetBinding(Line.Y2Property, minortickCrossSizeBinding);
                                        break;
                                    case TickPosition.Outside:
                                        minorLine.Y1 = 0;
                                        Binding minortickSizeBinding = new Binding();
                                        minortickSizeBinding.Path = new PropertyPath("MinorLineSize");
                                        minortickSizeBinding.Source = this;
                                        minorLine.SetBinding(Line.Y2Property, minortickSizeBinding);
                                        break;                                    
                                }
                                MinorTickLines.Add(minorLine);
                                this.Children.Add(minorLine);
                                minorstep += minorstep;
                            }
                            MajorTickLines.Add(tickLine);
                            this.Children.Add(label);
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
                    for (int i = 0; i < this.MLabels.Count; i++)
                    {
                        ContentControl label = Labels[i];
                        label.Content = MLabels[i];
                        label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        RotateTransform labelRotation = new RotateTransform();
                        labelRotation.Angle = LabelAngle;
                        Rect rotatedRect = GetRotatedRect(new Rect(0, 0, label.DesiredSize.Width, label.DesiredSize.Height), labelRotation);
                        //label.RenderTransformOrigin = new Point(0.5, 0.5);
                        label.RenderTransform = labelRotation;      
                        Line tickLine = MajorTickLines[i];
                        double labelPadding = 0;
                        int minorCount = 0;
                        tickLine.X1 = xAxisWidthPosition;
                        tickLine.X2 = xAxisWidthPosition;                        
                        tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                        
                        if (!(i == this.MLabels.Count - 1))
                        {
                            double minorWidth = xAxisWidthStep;
                            minorstep = minorWidth / (MinorTicksCount + 1);
                            for (int j = 0; j < this.MinorTicksCount; j++)
                            {
                                Line minorLine = MinorTickLines[minorCount];
                                minorLine.X1 = (xAxisWidthPosition + minorstep * (j + 1));
                                minorLine.X2 = (xAxisWidthPosition + minorstep * (j + 1));                                                                
                                minorCount++;
                            }
                        }

                        if (this.LabelAngle == 0)
                        {
                            Canvas.SetLeft(label, xAxisWidthPosition - (label.DesiredSize.Width / 2));
                            Canvas.SetTop(label, desiredHeight);
                            labelSize = Math.Max(labelSize, label.DesiredSize.Height);
                        }
                        else
                        {
                            Canvas.SetLeft(label, xAxisWidthPosition - (rotatedRect.Width / 2) - rotatedRect.X);
                            Canvas.SetTop(label, desiredHeight - rotatedRect.Y);
                            labelSize = Math.Max(labelSize, rotatedRect.Height);

                        }
                        xAxisWidthPosition += xAxisWidthStep;
                    }  
                }
                desiredHeight += labelSize;   
                header.Measure(new Size(this.ActualHeight, this.ActualWidth));
                Canvas.SetLeft(header, (this.ActualWidth / 2) - (header.DesiredSize.Width / 2));
                Canvas.SetTop(header, desiredHeight);
                desiredHeight += header.DesiredSize.Height;
                this.Chart.AxisHeight = desiredHeight;
            }
            //if (this.Chart.AxisHeight < desiredHeight)
                
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            double desiredHeight = 0;
            double labelSize = 0;
            //if (m_MinValue == m_startValue + m_Interval)
            CalculateAutoInterval();
            GenerateLabels();
            if (this.ActualHeight > 0 && this.ActualWidth > 0)
            {
               
                this.Children.Clear();
                double xAxisWidthStep = this.ActualWidth / ((MIntervalCount > 0) ? MIntervalCount : 1);
                double xAxisWidthPosition = this.DataToPoint(MStartValue);
                double minorstep = 0;
                //m_offset = this.DataToPoint(m_MinValue + m_Interval);
                Rect oldRect = new Rect(0, 0, 0, 0);
                AxisLine = new Line();
                AxisLine.X1 = 0;
                AxisLine.X2 = this.ActualWidth;
                AxisLine.Y1 = 0;
                AxisLine.Y2 = 0;
                Binding binding = new Binding();
                binding.Path = new PropertyPath("AxisLineStyle");
                binding.Source = this;
                AxisLine.SetBinding(Line.StyleProperty, binding);
                Labels = new List<ContentControl>();
                MajorTickLines = new List<Line>();
                MinorTickLines = new List<Line>();
                for (int i = 0; i < this.MLabels.Count; i++)
                {
                    ContentControl label = new ContentControl();
                    label.Content = MLabels[i];
                    Binding labelTemplateBinding = new Binding();
                    labelTemplateBinding.Path = new PropertyPath("LabelTemplate");
                    labelTemplateBinding.Source = this;
                    label.SetBinding(ContentControl.ContentTemplateProperty, labelTemplateBinding);
                    label.Measure(new Size(this.ActualHeight, this.ActualWidth));
                    RotateTransform labelRotation = new RotateTransform();
                    labelRotation.Angle = LabelAngle;
                    Rect rotatedRect = GetRotatedRect(new Rect(0, 0, label.DesiredSize.Width, label.DesiredSize.Height), labelRotation);
                    //label.RenderTransformOrigin = new Point(0.5, 0.5);
                    label.RenderTransform = labelRotation;                    
                    Labels.Add(label);
                    
                    Line tickLine = new Line();
                    double labelPadding = 0;
                    Binding styleBinding = new Binding();
                    styleBinding.Path = new PropertyPath("MajorLineStyle");
                    styleBinding.Source = this;
                    tickLine.SetBinding(Line.StyleProperty, styleBinding);
                    tickLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                    tickLine.X1 = xAxisWidthPosition - (tickLine.DesiredSize.Width / 2);
                    tickLine.X2 = xAxisWidthPosition - (tickLine.DesiredSize.Width / 2);                   
                    
                    switch (this.MajorTicksPosition)
                    {
                        case TickPosition.Inside:
                            tickLine.Y1 = 0;
                            Binding tickSizeInsideBinding = new Binding();
                            tickSizeInsideBinding.Path = new PropertyPath("MajorLineSize");
                            tickSizeInsideBinding.Source = this;
                            tickSizeInsideBinding.Converter = new NegativeConverter();
                            tickLine.SetBinding(Line.Y2Property, tickSizeInsideBinding);
                            if (this.ShowMajorTicks)
                            {
                                labelPadding = 0;
                                desiredHeight = 0;
                            }
                            break;
                        case TickPosition.Cross:
                            Binding tickSizeNegativeCrossBinding = new Binding();
                            tickSizeNegativeCrossBinding.Path = new PropertyPath("MajorLineSize");
                            tickSizeNegativeCrossBinding.Source = this;
                            tickSizeNegativeCrossBinding.Converter = new NegativeHalfConverter();
                            tickLine.SetBinding(Line.Y1Property, tickSizeNegativeCrossBinding);
                       
                            Binding tickSizeCrossBinding = new Binding();
                            tickSizeCrossBinding.Path = new PropertyPath("MajorLineSize");
                            tickSizeCrossBinding.Source = this;
                            tickSizeCrossBinding.Converter = new HalfValueConverter();
                            tickLine.SetBinding(Line.Y2Property, tickSizeCrossBinding);
                            if (this.ShowMajorTicks)
                            {
                                labelPadding = tickLine.Y2;
                                desiredHeight = MajorLineSize / 2;
                            }
                            break;
                        case TickPosition.Outside:
                            tickLine.Y1 = 0;
                            Binding tickSizeBinding = new Binding();
                            tickSizeBinding.Path = new PropertyPath("MajorLineSize");
                            tickSizeBinding.Source = this;
                            tickLine.SetBinding(Line.Y2Property, tickSizeBinding);
                            if (this.ShowMajorTicks)
                            {
                                labelPadding = tickLine.Y2;
                                desiredHeight = MajorLineSize;
                            }
                            break;                       
                    }
                    
                    Binding ticklineVisibilityBinding = new Binding();
                    ticklineVisibilityBinding.Path = new PropertyPath("ShowMajorTicks");
                    ticklineVisibilityBinding.Source = this;
                    ticklineVisibilityBinding.Converter = new BooleanToVisibilityConverter();
                    tickLine.SetBinding(Line.VisibilityProperty, ticklineVisibilityBinding);
                    MajorTickLines.Add(tickLine);
                    this.Children.Add(tickLine);
                    if (!(i == this.MLabels.Count - 1))
                    {
                        double minorWidth = xAxisWidthStep;
                        minorstep = minorWidth / (MinorTicksCount + 1);
                        for (int j = 0; j < this.MinorTicksCount; j++)
                        {
                            Line minorLine = new Line();
                            Binding minorstyleBinding = new Binding();
                            minorstyleBinding.Path = new PropertyPath("MinorLineStyle");
                            minorstyleBinding.Source = this;
                            minorLine.SetBinding(Line.StyleProperty, minorstyleBinding);
                            minorLine.Measure(new Size(this.ActualHeight, this.ActualWidth));
                            minorLine.X1 = (xAxisWidthPosition + minorstep * (j + 1));
                            minorLine.X2 = (xAxisWidthPosition + minorstep * (j + 1));

                            switch (MinorTicksPosition)
                            {
                                case TickPosition.Inside:
                                    minorLine.Y1 = 0;
                                    Binding minortickInsideSizeBinding = new Binding();
                                    minortickInsideSizeBinding.Path = new PropertyPath("MinorLineSize");
                                    minortickInsideSizeBinding.Source = this;
                                    minortickInsideSizeBinding.Converter = new NegativeConverter();
                                    minorLine.SetBinding(Line.Y2Property, minortickInsideSizeBinding);
                                    break;
                                case TickPosition.Cross:
                                    Binding minortickNegativeCrossSizeBinding = new Binding();
                                    minortickNegativeCrossSizeBinding.Path = new PropertyPath("MinorLineSize");
                                    minortickNegativeCrossSizeBinding.Source = this;
                                    minortickNegativeCrossSizeBinding.Converter = new NegativeHalfConverter();
                                    minorLine.SetBinding(Line.Y1Property, minortickNegativeCrossSizeBinding);

                                    Binding minortickCrossSizeBinding = new Binding();
                                    minortickCrossSizeBinding.Path = new PropertyPath("MinorLineSize");
                                    minortickCrossSizeBinding.Source = this;
                                    minortickCrossSizeBinding.Converter = new HalfValueConverter();
                                    minorLine.SetBinding(Line.Y2Property, minortickCrossSizeBinding);
                                    break;
                                case TickPosition.Outside:
                                    minorLine.Y1 = 0;
                                    Binding minortickSizeBinding = new Binding();
                                    minortickSizeBinding.Path = new PropertyPath("MinorLineSize");
                                    minortickSizeBinding.Source = this;
                                    minorLine.SetBinding(Line.Y2Property, minortickSizeBinding);
                                    break;
                            }
                                                        
                            MinorTickLines.Add(minorLine);
                            this.Children.Add(minorLine);                           
                        }
                    }
                    if (this.LabelAngle == 0)
                    {
                        Canvas.SetLeft(label, xAxisWidthPosition - (label.DesiredSize.Width / 2));
                        Canvas.SetTop(label, desiredHeight);
                        labelSize = Math.Max(labelSize, label.DesiredSize.Height);
                    }
                    else
                    {
                        Canvas.SetLeft(label, xAxisWidthPosition - (rotatedRect.Width / 2) - rotatedRect.X);
                        Canvas.SetTop(label, desiredHeight - rotatedRect.Y);
                        labelSize = Math.Max(labelSize, rotatedRect.Height);

                    }
                    //labelSize = Math.Max(labelSize, label.ActualHeight);  
                    this.Children.Add(label);                   
                    xAxisWidthPosition += xAxisWidthStep;
                }
                header = new ContentControl();
                header.DataContext = null;
                Binding contentBinding = new Binding();
                contentBinding.Path = new PropertyPath("Header");
                contentBinding.Source = this;
                header.SetBinding(ContentControl.ContentProperty, contentBinding);
                Binding headerTemplateBinding = new Binding();
                headerTemplateBinding.Path = new PropertyPath("HeaderTemplate");
                headerTemplateBinding.Source = this;
                header.SetBinding(ContentControl.ContentTemplateProperty, headerTemplateBinding);
                header.Measure(new Size(this.ActualHeight, this.ActualWidth));
                desiredHeight+=labelSize;
                Canvas.SetLeft(header, (this.ActualWidth / 2) - (header.DesiredSize.Width / 2));
                Canvas.SetTop(header, (this.ActualHeight / 2) - (header.DesiredSize.Height / 2) + desiredHeight);
                desiredHeight += header.DesiredSize.Height ;
                this.Children.Add(header);
                this.Children.Add(AxisLine);
                IsInitialized = true;
                this.Chart.AxisHeight = desiredHeight;
            }
           // if (this.Chart.AxisHeight < desiredHeight)
                
        }

        /// <summary>
        /// Gets the styles.
        /// </summary>
        protected override void GetStyles()
        {
            base.GetStyles();
            this.HeaderTemplate = (DataTemplate)Styles["xAxisHeaderTemplate"];
        }

#if WPf
        public static Rect BoundsRelativeTo(FrameworkElement element, Visual relativeTo)
        {
            return element.TransformToVisual(relativeTo).TransformBounds(LayoutInformation.GetLayoutSlot(element));
        }
#endif

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
        /// Datas to point.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override double DataToPoint(double value)
        {
            if (!(MMinValue == MMaxValue))
                return ((value - MMinValue) * (this.ActualWidth / (MMaxValue - MMinValue)));
            else
                return 0;
        }

        /// <summary>
        /// Calculates the interval from series points.
        /// </summary>
        public override void CalculateIntervalFromSeriesPoints()
        {
            List<double> xValues = new List<double>();
            
            if (this.Series != null)
            {
                var series = Series.Where(ser => ser.XAxis == this);
                foreach (SeriesBase seriesBase in series)
                {
                    if (seriesBase.Points != null)
                        foreach (var point in seriesBase.Points)
                        {
                            xValues.Add(point.XValue);
                        }
                    
                }
            }
            if (xValues.Count > 0)
            {
                double min, max;
                min = xValues.ToArray().Min();
                max = xValues.ToArray().Max();
                
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
                case XType.Double:
                    return value.ToString(this.StringFormat);                    
                case XType.DateTime:
#if !WINRT
                    return DateTime.FromOADate(value).ToString(this.StringFormat);
#else
                    return value.FromOADate().ToString(this.StringFormat);
#endif
                case XType.Category:
                    if (SparrowChart.ActualCategoryValues.Count > (int)value)
                        return SparrowChart.ActualCategoryValues[(int)value];
                    else
                        return "";
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
        internal XType Type
        {
            get { return (XType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// The type property
        /// </summary>
        internal static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(XType), typeof(XAxis), new PropertyMetadata(XType.Double,OnTypeChanged));

        /// <summary>
        /// Called when [type changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var xAxis = sender as XAxis;
            if (xAxis != null) xAxis.TypeChanged(args);
        }

        /// <summary>
        /// Types the changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        internal void TypeChanged(DependencyPropertyChangedEventArgs args)
        {           
            switch (Type)
            {
                case XType.Double:
#if WPF
                    this.ActualType =(ActualType)Enum.Parse(typeof(ActualType),XType.Double.ToString());
#else
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), XType.Double.ToString(),false);
#endif
                    break;
                case XType.DateTime:
#if WPF
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), XType.DateTime.ToString());
#else
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), XType.DateTime.ToString(),false);
#endif
                    break;
                case XType.Category:
#if WPF
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), XType.Category.ToString());
#else
                    this.ActualType = (ActualType)Enum.Parse(typeof(ActualType), XType.Category.ToString(),false);
#endif
                    break;
            }
        }

    }
}
