using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class LineSeries : XYSeries
    {
        protected override void GeneratePoints()
        {
            this.Parts = new PartCollection();
            for (int i = 0; i < Points.Count - 1;i++ )
            {
                ChartPoint point1=Points[i];
                ChartPoint point2=Points[i+1];
                PointF startPoint=new PointF(this.XAxis.PointFromValue(point1.XValue),this.YAxis.PointFromValue(point1.YValue));
                PointF endPoint = new PointF(this.XAxis.PointFromValue(point2.XValue), this.YAxis.PointFromValue(point2.YValue));
                LineSeriesPart linePart = new LineSeriesPart(startPoint, endPoint,this.StrokePen ?? new Pen(this.Stroke, this.StrokeThickness){EndCap = LineCap.Round,StartCap = LineCap.Round});
                linePart.RootGraphics = this.RootGraphics;
                this.Parts.Add(linePart);
            }            
            this.DrawAllSeriesParts();
        }

    }    
}