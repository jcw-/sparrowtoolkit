using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class LinearXAxis : XAxis
    {
        private double? interval;
        public double? Interval
        {
            get { return interval; }
            set { interval = value; ActualInterval = CalculateActualInterval(interval); DesiredInterval = ActualInterval; }
        }

        private double? maxValue;
        public double? MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; ActualMaxValue = CalculateActualMaxValue(maxValue); DesiredMaxValue = ActualMaxValue; }
        }

        private double? minValue;
        public double? MinValue
        {
            get { return minValue; }
            set { minValue = value; ActualMinValue = CalculateActualMinValue(minValue); DesiredMinValue = ActualMinValue; }
        }

        protected override double CalculateActualInterval(object interval)
        {
            if (interval != null)
                return Convert.ToDouble(interval.ToString());
            else
                return 0;
        }

        protected override double CalculateActualMaxValue(object maxValue)
        {
            if (maxValue != null)
                return Convert.ToDouble(maxValue.ToString());
            else
                return 0;
        }

        protected override double CalculateActualMinValue(object minValue)
        {
            if (minValue != null)
                return Convert.ToDouble(minValue.ToString());
            else
                return 0;
        }

        protected override void CalculateAxisRange()
        {
            this.DesiredMinValue = ((MinValue != null)) ? ActualMinValue : this.CalculateMinimumFromSeries();
            this.DesiredMaxValue = ((MaxValue != null)) ? ActualMaxValue : this.CalculateMaximumFromSeries();
            this.DesiredInterval = ((Interval != null)
                                        ? ActualInterval
                                        : ((DesiredMaxValue - DesiredMinValue)/IntervalCount));
        }
    }
}
