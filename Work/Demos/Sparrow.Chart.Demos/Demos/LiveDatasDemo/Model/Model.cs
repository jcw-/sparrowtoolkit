using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart.Demos.Demos.LiveDatasDemo
{
    public class Data
    {
        public Data()
        {

        }
        public Data(DateTime date, double value, double value1, double value2)
        {
            Date = date;
            Value = value;
            Value1 = value1;
            Value2 = value2;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public double Value
        {
            get;
            set;
        }
        public double Value1
        {
            get;
            set;
        }
        public double Value2
        {
            get;
            set;
        }
    }   
}
