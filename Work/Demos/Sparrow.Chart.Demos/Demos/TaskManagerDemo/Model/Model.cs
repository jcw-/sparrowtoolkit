using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPUPerformance
{
    public class Model
    {
        private DateTime time;
        public DateTime Time 
        {
            get{ return time;}
            set { time = value; }
        }

        private double percentage;
        public double Percentage
        {
            get { return percentage; }
            set { percentage = value; }
        }

        private double memoryUsage;
        public double MemoryUsage
        {
            get { return memoryUsage; }
            set { memoryUsage = value; }
        }

        public Model()
        {
        }
        public Model(DateTime time, double percentage, double memoryUsage)
        {
            this.Time = time;
            this.Percentage = percentage;
            this.MemoryUsage = memoryUsage;
        }

    }
}
