using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public abstract class Series 
    {
        public Series()
        {            
            this.StrokeThickness = 2.0f;
        }
        public string XPath
        {
            get;
            set;
        }

        public Brush Stroke
        {
            get; 
            set;
        }

        public float StrokeThickness
        {
            get; 
            set;
        }

        public Pen StrokePen
        {
            get;
            set;
        }

        internal Graphics RootGraphics
        {
            get;
            set;
        }

        public string Label
        {
            get; 
            set; 
        }

        private XAxis xAxis;
        [DefaultValue(false)]
        public XAxis XAxis
        {
            get { return xAxis; }
            set
            {
                if (this.Chart!=null && xAxis!=null && Chart.Axes.Contains(xAxis))
                    this.Chart.Axes.Remove(xAxis);
                xAxis = value;
                if (this.Chart!=null && xAxis!=null && !this.Chart.Axes.Contains(xAxis))
                    this.Chart.Axes.Add(xAxis);
            }
        }

        private YAxis yAxis;
        [DefaultValue(false)]
        public YAxis YAxis
        {
            get { return yAxis; }
            set
            {
                if (this.Chart != null && yAxis != null && this.Chart.Axes.Contains(yAxis))
                    this.Chart.Axes.Remove(yAxis);
                yAxis = value;
                if (this.Chart!=null && yAxis!=null && !this.Chart.Axes.Contains(yAxis))
                    this.Chart.Axes.Add(yAxis);
            }
        }

        public PartCollection Parts
        {
            get; 
            set;
        }

        public IEnumerable PointsSource
        {
            get; 
            set;
        }

        public SparrowChart Chart
        {
            get; 
            set;
        }

        protected void DrawAllSeriesParts()
        {
            if (this.RootGraphics != null)
                foreach (var part in Parts)
                {
                    part.DrawSeriesPart();
                }
        }

        protected virtual void GeneratePoints()
        {
           
        }

        internal void Refresh()
        {
            this.GeneratePoints();
        }

        internal virtual double GetMinimumFromPoints(Axis axis)
        {
            return 0;
        }

        internal virtual double GetMaximumFromPoints(Axis axis)
        {
            return 0;
        }
       
    }
}
