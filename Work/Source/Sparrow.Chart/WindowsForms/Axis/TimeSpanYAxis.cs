using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class TimeSpanYAxis : YAxis
    {
        public TimeSpan? Interval
        {
            get;
            set;
        }

        public TimeSpan? MaxValue
        {
            get;
            set;
        }

        public TimeSpan? MinValue
        {
            get;
            set;
        }
    }
}
