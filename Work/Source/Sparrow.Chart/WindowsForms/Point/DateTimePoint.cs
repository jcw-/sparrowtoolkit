using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class DateTimePoint : ChartPoint
    {
        private DateTime date;
        private double yvalue;
    
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value; this.XValue = date.ToOADate();
            }
        }

        public double Value
        {
            get
            {
                return yvalue;
            }
            set
            {
                yvalue = value; this.YValue = value;
            }
        }
    }
}
