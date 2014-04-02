using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class ChartPoint :  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double XValue
        {
            get; 
            set;
        }

        public double YValue
        {
            get; 
            set;
        }
    }
}
