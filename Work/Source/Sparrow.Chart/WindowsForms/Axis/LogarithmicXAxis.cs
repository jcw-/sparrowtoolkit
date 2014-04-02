using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class LogarithmicXAxis : XAxis
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

        private double? logBase;
        public double? LogBase
        {
            get { return logBase; }
            set { logBase = value; }
        }

        protected override double CalculateActualInterval(object interval)
        {
            return Math.Log(Convert.ToDouble(interval.ToString()), LogBase ?? 10d);
        }

        protected override double CalculateActualMaxValue(object maxValue)
        {
            return Math.Log(Convert.ToDouble(maxValue.ToString()), LogBase ?? 10d);
        }

        protected override double CalculateActualMinValue(object minValue)
        {
            return Math.Log(Convert.ToDouble(minValue.ToString()), LogBase ?? 10d);
        }
    }
}
