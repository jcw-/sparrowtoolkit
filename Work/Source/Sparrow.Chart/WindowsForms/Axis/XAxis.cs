using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public abstract class XAxis : Axis
    {
        public XAxis()
        {
            this.AxisPosition = AxisPosition.Bottom;
        }
        public override float PointFromValue(double value)
        {
            float point= (float)((value - this.DesiredMinValue) * (this.PositionedRect.Width / (this.DesiredMaxValue - this.DesiredMinValue)));
            point += PositionedRect.Left;
            return point;
        }
    }
}
