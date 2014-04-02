using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public abstract class YAxis : Axis
    {
        public YAxis()
        {
            this.AxisPosition=AxisPosition.Left;
        }

        public override float PointFromValue(double value)
        {
            float point = (float)(this.PositionedRect.Height - ((value - this.DesiredMinValue) * this.PositionedRect.Height / (this.DesiredMaxValue - this.DesiredMinValue)));
            point += PositionedRect.Top;
            return point;
        }
    }
}
