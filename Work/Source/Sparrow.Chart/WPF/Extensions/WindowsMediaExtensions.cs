#if DIRECTX2D
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
#endif
using System;
using System.Windows.Media;

namespace Sparrow.Chart
{
    /// <summary>
    /// WindowsMedia Extensions for Conversion
    /// </summary>
    public static class WindowsMediaExtensions
    {
        public static System.Drawing.Drawing2D.SmoothingMode AsDrawingSmoothingMode(this SmoothingMode smoothingMode)
        {
            System.Drawing.Drawing2D.SmoothingMode drawingSmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            switch (smoothingMode)
            {
                case SmoothingMode.AntiAlias:
                    drawingSmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    break;
                case SmoothingMode.Default:
                    drawingSmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    break;
                case SmoothingMode.HighQuality:
                    drawingSmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    break;
                case SmoothingMode.HighSpeed:
                    drawingSmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    break;
                case SmoothingMode.Invalid:
                    drawingSmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Invalid;
                    break;
                case SmoothingMode.None:
                    drawingSmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    break;               
            }
            return drawingSmoothingMode;
        }

        public static System.Drawing.Drawing2D.CompositingMode AsDrawingCompositingMode(this CompositingMode compositingMode)
        {
            System.Drawing.Drawing2D.CompositingMode drawingcompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            switch (compositingMode)
            {
                case CompositingMode.SourceCopy:
                    drawingcompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    break;
                case CompositingMode.SourceOver:
                    drawingcompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    break;
            }
            return drawingcompositingMode;
        }

        public static System.Drawing.Drawing2D.CompositingQuality AsDrawingCompositingQuality(this CompositingQuality compositingQuality)
        {
            System.Drawing.Drawing2D.CompositingQuality drawingcompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            switch (compositingQuality)
            {
                case CompositingQuality.AssumeLinear:
                    drawingcompositingQuality = System.Drawing.Drawing2D.CompositingQuality.AssumeLinear;
                    break;
                case CompositingQuality.Default:
                    drawingcompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                    break;
                case CompositingQuality.GammaCorrected:
                    drawingcompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
                    break;
                case CompositingQuality.HighQuality:
                    drawingcompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    break;
                case CompositingQuality.HighSpeed:
                    drawingcompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                    break;
                case CompositingQuality.Invalid:
                    drawingcompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Invalid;
                    break;
                default:
                    break;
            }
            return drawingcompositingQuality;
        }

        public static System.Drawing.SolidBrush AsDrawingBrush(this System.Windows.Media.Color color)
        {
            var gdiBrush = new System.Drawing.SolidBrush(color.AsDrawingColor());
            return gdiBrush;
        }

        public static System.Drawing.Brush AsDrawingBrush(this System.Windows.Media.Brush brush)
        {
            System.Drawing.Brush drawingBrush = System.Drawing.Brushes.Black;
            if (brush is System.Windows.Media.LinearGradientBrush)
            {
                System.Windows.Media.LinearGradientBrush linearBrush = brush as System.Windows.Media.LinearGradientBrush;
                drawingBrush = new System.Drawing.Drawing2D.LinearGradientBrush(linearBrush.StartPoint.AsDrawingPoint(), linearBrush.EndPoint.AsDrawingPoint(), linearBrush.GradientStops[0].Color.AsDrawingColor(), linearBrush.GradientStops[1].Color.AsDrawingColor());
                return drawingBrush;
            }
            else if (brush is System.Windows.Media.SolidColorBrush)
            {
                drawingBrush = (brush as System.Windows.Media.SolidColorBrush).Color.AsDrawingBrush();
                return drawingBrush;
            }
            return drawingBrush;
        }

        public static System.Drawing.Color AsDrawingColor(this System.Windows.Media.Color color)
        {
            System.Drawing.Color drawingColor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            return drawingColor;
        }

        public static System.Drawing.Pen AsDrawingPen(this System.Windows.Media.Pen pen)
        {
            var brush = pen.Brush as System.Windows.Media.SolidColorBrush;
            brush = brush ?? Brushes.Transparent;
            System.Drawing.Color color = brush.Color.AsDrawingColor();
            var drawingPen = new System.Drawing.Pen(color, (float)pen.Thickness);

            return drawingPen;
        }

        public static System.Drawing.PointF AsDrawingPointF(this System.Windows.Point point)
        {
            var drawingPoint = new System.Drawing.PointF((float)point.X, (float)point.Y);
            return drawingPoint;
        }

        public static System.Drawing.Point AsDrawingPoint(this System.Windows.Point point)
        {
            var drawingPoint = new System.Drawing.Point(Convert.ToInt32(Math.Round(point.X)), Convert.ToInt32(Math.Round(point.Y)));
            return drawingPoint;
        }
#if DIRECTX2D
        public static Point2F AsDirectX2DPoint(this System.Windows.Point point)
        {
            var drawingPoint = new Point2F((float)point.X, (float)point.Y);
            return drawingPoint;
        }
#endif

    }
}
