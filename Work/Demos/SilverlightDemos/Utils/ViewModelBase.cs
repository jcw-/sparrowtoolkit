using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sparrow.Chart.Demos
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, args);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Stopwatch
    {

        private long _start;

        private long _end;



        public void Start()

        {

            _start = DateTime.Now.Ticks;

        }



        public void Stop()

        {

            _end = DateTime.Now.Ticks;

        }



        public TimeSpan ElapsedTime
        {
            get
            {
                return new TimeSpan(_end - _start);
            }
        }


        public static Stopwatch StartNew()
        {

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            return stopwatch;

        }
    }

}
