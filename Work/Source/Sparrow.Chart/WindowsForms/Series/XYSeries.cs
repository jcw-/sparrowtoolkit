using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public abstract class XYSeries : Series
    {
        public XYSeries()
        {
            this.Points=new PointCollection();            
        }

        public string YPath
        {
            get;
            set;
        }

        private PointCollection points;
        public PointCollection Points
        {
            get { return points; }
            set
            {
                if(points!=null)
                    points.CollectionChanged -= OnPointsCollectionChanged;
                points = value;
                if (points != null)
                    points.CollectionChanged += OnPointsCollectionChanged;
            }
        }

        void OnPointsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this.Chart != null)
                this.Chart.Invalidate();
        }


        internal override double GetMaximumFromPoints(Axis axis)
        {
            if (Points.Count > 0)
            {
                if (axis is XAxis)
                {
                    double maximum = this.Points[0].XValue;
                    foreach (var point in Points)
                    {
                        maximum = Math.Max(maximum, point.XValue);
                    }
                    return maximum;
                }
                else
                {
                    double maximum = this.Points[0].YValue;
                    foreach (var point in Points)
                    {
                        maximum = Math.Max(maximum, point.YValue);
                    }
                    return maximum;
                }
            }
            else
                return 0;
        }

        internal override double GetMinimumFromPoints(Axis axis)
        {
            if (Points.Count > 0)
            {
                if (axis is XAxis)
                {
                    double maximum = this.Points[0].XValue;
                    foreach (var point in Points)
                    {
                        maximum = Math.Min(maximum, point.XValue);
                    }
                    return maximum;
                }
                else
                {
                    double maximum = this.Points[0].YValue;
                    foreach (var point in Points)
                    {
                        maximum = Math.Min(maximum, point.YValue);
                    }
                    return maximum;
                }
            }
            else
                return 0;
        }
    }
}
