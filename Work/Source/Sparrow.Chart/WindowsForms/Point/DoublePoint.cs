using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class DoublePoint : ChartPoint
    {
        private double data;
        private double yvalue;

        public double Data
        {
            get { return data; }
            set { data = value; this.XValue = data; }
        }

        public double Value
        {
            get { return yvalue; }
            set { yvalue = value; this.YValue = value; }
        }
    }
}
