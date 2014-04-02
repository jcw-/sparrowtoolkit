using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class CategoryXAxis : XAxis
    {

        internal List<string> ActualCategoryValues
        {
            get;
            set;
        }

        public double? Interval
        {
            get;
            set;
        }

        public double? MaxValue
        {
            get;
            set;
        }

        public double? MinValue
        {
            get;
            set;
        }
    }
}
