using System.Collections.Generic;
using System.Windows;
#if !WINRT
using System.Windows.Media;
using System.Windows.Shapes;

#else
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
#endif


#if DIRECTX2D

using Sparrow.Directx2D;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
#endif

namespace Sparrow.Chart
{
    public class ColumnContainer : SeriesContainer
    {
#if DIRECTX2D
        override protected void OnRender()
        {
            if (this.Directx2DGraphics.IsFrontBufferAvailable)
            {
                this.Directx2DGraphics.Lock();
                this.Directx2DGraphics.AddDirtyRect(new Int32Rect(0, 0, this.Directx2DGraphics.PixelWidth, this.Directx2DGraphics.PixelHeight));
                this.Directx2DGraphics.Unlock();
            }

            if (this.RenderTarget != null)
            {
                this.RenderTarget.BeginDraw();
                if (this.Series.Index == 0)
                    this.RenderTarget.Clear(new ColorF(0, 0, 0, 0.0f));
                var pointCount = ((LineSeries)Series).LinePoints.Count;
                var points = ((LineSeries)Series).LinePoints;
                for (int i = 0; i < pointCount - 1; i++)
                {
                    directXBrush = collection.GetDirectXBrush((this.Series as LineSeries).Stroke);
                    this.RenderTarget.DrawLine(points[i].AsDirectX2DPoint(), points[i + 1].AsDirectX2DPoint(), directXBrush, thickness);
                }
                this.RenderTarget.EndDraw();
            }
        }
        
        protected void OnFreeResources()
        {
            collection.OnFreeResources();
            if (this.directXBrush != null)
            {
                this.directXBrush.Dispose();
                this.directXBrush = null;
            }            
        }

       
#endif
#if WPF
        override protected void DrawPath(SeriesBase series, System.Drawing.Pen pen)
        {
            if (series is ColumnSeries)
            {
                var points = new PointCollection();
                var pointCount = 0;
                var rects = new List<Rect>();
                ColumnSeries columnSeries = series as ColumnSeries;
                points = columnSeries.ColumnPoints;
                pointCount = columnSeries.ColumnPoints.Count;
                rects = columnSeries.Rects;
                System.Drawing.Brush fill = columnSeries.Fill.AsDrawingBrush();
                System.Drawing.Pen fillPen = new System.Drawing.Pen(fill);
                if (RenderingMode == RenderingMode.Default)
                {
                    for (int i = 0; i < columnSeries.Parts.Count; i++)
                    {
                        System.Windows.Shapes.Rectangle element = (columnSeries.Parts[i] as ColumnPart).CreatePart() as System.Windows.Shapes.Rectangle;
                        if (element != null && !PartsCanvas.Children.Contains(element))
                            PartsCanvas.Children.Add(element);
                    }
                }
                else
                {
                    for (int i = 0; i < rects.Count; i++)
                    {
                        Rect rect=rects[i];
                        switch (RenderingMode)
                        {
                            case RenderingMode.GDIRendering:
                                GDIGraphics.DrawRectangle(pen, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
                                GDIGraphics.FillRectangle(fill, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
                                break;
                            case RenderingMode.Default:
                                break;
                            case RenderingMode.WritableBitmap:
                                this.WritableBitmap.Lock();
                                WritableBitmapGraphics.DrawRectangle(pen, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
                                WritableBitmapGraphics.FillRectangle(fill, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
                                this.WritableBitmap.Unlock();
                                break;
                            default:
                                break;
                        }

                    }
                    this.Collection.InvalidateBitmap();
                }
            }
        }
#else
        /// <summary>
        /// Draws the path.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        protected override void DrawPath(SeriesBase series, Brush brush, double strokeThickness)
        {
            if (series is ColumnSeries)
            {                
                ColumnSeries columnSeries = series as ColumnSeries;                
                if (RenderingMode == RenderingMode.Default)
                {
                    for (int i = 0; i < columnSeries.Parts.Count; i++)
                    {
                        var columnPart = columnSeries.Parts[i] as ColumnPart;
                        if (columnPart != null)
                        {
                            Rectangle element = columnPart.CreatePart() as Rectangle;                        
                            PartsCanvas.Children.Add(element);
                        }
                    }
                }
            }
        }
#endif
    }
}
