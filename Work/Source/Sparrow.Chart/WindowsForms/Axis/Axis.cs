using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Sparrow.Chart
{
    public abstract class Axis
    {

        public Axis()
        {
            TextStyle textStyle = new TextStyle();
            textStyle.Font = new Font("Arial", 10f);
            textStyle.Foreground = new SolidBrush(Color.Black);
            textStyle.HorizontalAlignment = HorizontalAlignment.Center;
            textStyle.VerticalAlignment = VerticalAlignment.Center;
            this.LabelStyle = textStyle;
            textStyle.Font = new Font("Arial", 12f);
            textStyle.Foreground = new SolidBrush(Color.Black);
            textStyle.HorizontalAlignment = HorizontalAlignment.Center;
            textStyle.VerticalAlignment = VerticalAlignment.Center;
            this.HeaderStyle = textStyle;
            this.Header = "";
            this.MajorLineSize = 10f;
            this.MinorLineSize = 5f;
            this.ShowMajorCrossLines = true;
            this.ShowMinorCrossLines = true;
            this.MinorTicksCount = 0;
            this.MajorLinePen=new Pen(Color.Black,0.75f);
            this.MinorLinePen=new Pen(Color.Black,0.5f);
            this.AxisLinePen=new Pen(Color.Black,1.0f);
        }
        private int intervalCount = 5;
        internal int IntervalCount
        {
            get { return intervalCount; }
            set { intervalCount = value; }
        }

        public TextStyle HeaderStyle
        {
            get;
            set;
        }

        public string Header
        { 
            get; 
            set;
        }

        public TextStyle LabelStyle
        {
            get;
            set;
        }

        public string LabetStringFormat
        {
            get; 
            set;
        }

        public float MajorLineSize { get; set; }

        public float MinorLineSize { get; set; }

        public bool ShowMajorCrossLines { get; set; }

        public bool ShowMinorCrossLines { get; set; }

        public int MinorTicksCount { get; set; }

        public Pen MajorLinePen { get; set; }

        public Pen MinorLinePen { get; set; }

        public Pen AxisLinePen { get; set; }

        internal SparrowChart Chart
        {
            get;
            set;
        }
       
        internal RectF PositionedRect
        {
            get; 
            set;
        }

        internal RectF Area { get; set; }

        internal double ActualInterval
        {
            get; 
            set;
        }

        internal double ActualMinValue
        {
            get;
            set;
        }

        internal double ActualMaxValue
        {
            get; 
            set;
        }

        internal double DesiredInterval
        {
            get;
            set;
        }

        internal double DesiredMaxValue
        {
            get;
            set;
        }

        internal double DesiredMinValue
        {
            get;
            set;
        }

        internal AxisPosition AxisPosition
        {
            get; 
            set;
        }

        internal Graphics RootGraphics
        {
            get;
            set;
        }

        public virtual float PointFromValue(double value)
        {
           return 0;
        }

        public bool CheckValue(double value)
        {
            if (value >= DesiredMinValue && value <= DesiredMaxValue)
                return true;
            else
                return false;
        }

        protected virtual double CalculateActualInterval(object interval)
        {
            return 0;
        }

        protected virtual double CalculateActualMinValue(object minValue)
        {
            return 0;
        }

        protected virtual double CalculateActualMaxValue(object maxValue)
        {
            return 0;
        }

        protected double CalculateMinimumFromSeries()
        {
            if (this.Chart != null && this.Chart.Series.Count > 0)
            {
                var result = this.Chart.Series.Where(series => ((series.XAxis == this) || (series.YAxis == this)))
                                 .Select(series => series.GetMinimumFromPoints(this));
                return (result.Any()) ? result.Min() : 0;
            }
            else
                return 0;
        }

        protected double CalculateMaximumFromSeries()
        {
            if (this.Chart != null && this.Chart.Series.Count > 0)
            {
                var result = this.Chart.Series.Where(series => ((series.XAxis == this) || (series.YAxis == this))).Select(series => series.GetMaximumFromPoints(this));
                return (result.Any()) ? result.Max() : 1;
            }
            else
                return 1;
        }

        protected virtual void CalculateAxisRange()
        {
            
        }

        internal void Refresh()
        {
            CalculateAxisRange();
        }


        protected double MaxPixelsCount = 100;
        protected double MaximumLabels = 2;

        protected double GetIntervalsCount(Size availableSize)
        {
            double size = (AxisPosition == AxisPosition.Left) || (AxisPosition == AxisPosition.Right)
                              ? availableSize.Width
                              : availableSize.Height;
           
            double adjustedIntervalsCount = ((AxisPosition == AxisPosition.Left) ||
                                                    (AxisPosition == AxisPosition.Right)
                                                        ? 0.8
                                                        : 1.0)*
                                                   MaximumLabels;
            intervalCount = (int) Math.Max(size*adjustedIntervalsCount/MaxPixelsCount, 1.0);

            return intervalCount;
        }

        internal RectF PreRenderAxis()
        {
            switch (AxisPosition)
            {
                    case AxisPosition.Left:
                    this.Area = new RectF(this.PositionedRect.X, this.PositionedRect.Y, CalculateAxisSize().Width, this.PositionedRect.Height);
                    this.PositionedRect = new RectF(this.PositionedRect.X + CalculateAxisSize().Width, this.PositionedRect.Y,
                                                    this.PositionedRect.Width-CalculateAxisSize().Width, this.PositionedRect.height);
                    
                    break;
                    case AxisPosition.Bottom:
                    this.Area = new RectF(this.PositionedRect.X, this.PositionedRect.Y + (this.PositionedRect.Height - CalculateAxisSize().Height), this.PositionedRect.Width, CalculateAxisSize().Height);
                    this.PositionedRect = new RectF(this.PositionedRect.X, this.PositionedRect.Y,
                                                    this.PositionedRect.Width,this.PositionedRect.Height- CalculateAxisSize().Height);
                    
                    break;
                    case AxisPosition.Top:
                    this.Area = new RectF(this.PositionedRect.X, this.PositionedRect.Y, this.PositionedRect.Width, CalculateAxisSize().Height);
                    this.PositionedRect = new RectF(this.PositionedRect.X,
                                                    this.PositionedRect.Y + CalculateAxisSize().Height,
                                                    this.PositionedRect.Width,
                                                    this.PositionedRect.height - CalculateAxisSize().Height);
                    break;
                    case AxisPosition.Right:
                    this.Area = new RectF(this.PositionedRect.X + (this.PositionedRect.Width - CalculateAxisSize().Width), this.PositionedRect.Y, CalculateAxisSize().Width, this.PositionedRect.Height);
                    this.PositionedRect = new RectF(this.PositionedRect.X, this.PositionedRect.Y,
                                                    this.PositionedRect.Width - CalculateAxisSize().Width, this.PositionedRect.height);
                    break;
            }
            return this.PositionedRect;
        }

        internal void RenderAxis()
        {
            switch (AxisPosition)
            {
                case AxisPosition.Left:
                    //this.RootGraphics.DrawRectangle(new Pen(Color.Black, 1), this.Area.X, this.PositionedRect.Y, CalculateAxisSize().Width, this.PositionedRect.Height);
                    this.RootGraphics.DrawLine(this.AxisLinePen, this.Area.X + CalculateAxisSize().Width, this.PositionedRect.Y, this.Area.X + CalculateAxisSize().Width, this.PositionedRect.Y+ this.PositionedRect.Height);                   
                    if (!string.IsNullOrEmpty(this.Header))
                    {
                        
                    }
                    break;
                case AxisPosition.Bottom:
                    //this.RootGraphics.DrawRectangle(new Pen(Color.Black, 1), this.PositionedRect.X, this.Area.Y, this.PositionedRect.Width, CalculateAxisSize().Height);
                    this.RootGraphics.DrawLine(this.AxisLinePen, this.PositionedRect.X, this.Area.Y, this.PositionedRect.X + this.PositionedRect.Width, this.Area.Y);                    
                    break;
                case AxisPosition.Top:
                    //this.RootGraphics.DrawRectangle(new Pen(Color.Black, 1), this.PositionedRect.X, this.Area.Y, this.PositionedRect.Width, CalculateAxisSize().Height);                    
                    this.RootGraphics.DrawLine(this.AxisLinePen, this.PositionedRect.X, this.Area.Y, this.PositionedRect.X + this.PositionedRect.Width, this.Area.Y);                    
                    break;
                case AxisPosition.Right:
                   // this.RootGraphics.DrawRectangle(new Pen(Color.Black, 1), this.Area.X, this.PositionedRect.Y, CalculateAxisSize().Width, this.PositionedRect.Height);                    
                    this.RootGraphics.DrawLine(this.AxisLinePen, this.Area.X + CalculateAxisSize().Width, this.PositionedRect.Y, this.Area.X + CalculateAxisSize().Width, this.PositionedRect.Y + this.PositionedRect.Height);                    
                    break;
            }           
        }

        private SizeF CalculateAxisSize()
        {
            float tickLineSize = Math.Max(this.MinorLineSize, this.MajorLineSize);
            SizeF labelSize = new SizeF(20,20);
            SizeF headerSize = this.RootGraphics.MeasureString(this.Header, this.HeaderStyle.Font);
            switch (AxisPosition)
            {
                case AxisPosition.Right:
                case AxisPosition.Left:
                    return new SizeF(tickLineSize + labelSize.Width + headerSize.Height, this.PositionedRect.Height);
                    break;
                case AxisPosition.Top:
                case AxisPosition.Bottom:
                    return new SizeF(this.PositionedRect.Width, tickLineSize + labelSize.Height + headerSize.Height);
                    break;
            }
            return new SizeF(this.PositionedRect.Width,this.PositionedRect.Height);
        }
    }
}
