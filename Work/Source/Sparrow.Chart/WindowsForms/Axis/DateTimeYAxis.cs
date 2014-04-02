using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class DateTimeYAxis : YAxis
    {
        public TimeSpan? Interval
        {
            get;
            set;
        }

        public DateTime? MaxValue
        {
            get;
            set;
        }

        public DateTime? MinValue
        {
            get;
            set;
        }
    }
}
