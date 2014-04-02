using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class Point
    {
        internal double _x;
        internal double _y;

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point()
        {
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

    }
}
