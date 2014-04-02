using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class LineSeriesPart : SeriesPart
    {

        public LineSeriesPart(PointF point1, PointF point2,Pen pen)
        {
            this.X1 = point1.X;
            this.X2 = point2.X;
            this.Y1 = point1.Y;
            this.Y2 = point2.Y;
            this.Pen = pen;
        }

        public LineSeriesPart(float x1,float y1,float x2,float y2,Pen pen)
        {
            this.X1 = x1;
            this.X2 = x2;
            this.Y1 = y1;
            this.Y2 = y2;
            this.Pen = pen;
        }

        public float X1
        {
            get;
            set;
        }

        public float X2
        {
            get;
            set;
        }

        public float Y1
        {
            get;
            set;
        }

        public float Y2
        {
            get;
            set;
        }

        public Pen Pen
        {
            get;           
            set;
        }

        public override void DrawSeriesPart()
        {                        
            this.RootGraphics.DrawLine(this.Pen, this.X1, this.Y1, this.X2, this.Y2);            
            //this.RootGraphics.DrawEllipse(this.Pen, new RectangleF(new PointF(this.X1 - 5, this.Y1 - 5), new SizeF(10f, 10f)));
            //this.RootGraphics.FillEllipse(this.Pen.Brush, new RectangleF(new PointF(this.X1 - 5, this.Y1 - 5), new SizeF(10f, 10f)));
        }
    }
}
