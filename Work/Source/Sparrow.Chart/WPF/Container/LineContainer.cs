#if !WINRT
using System.Windows;
using System.Windows.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

#if WPF

#endif

#if DIRECTX2D
using Sparrow.Directx2D;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
#endif
namespace Sparrow.Chart
{
    /// <summary>
    /// LineSeries Container
    /// </summary>
    public class LineContainer : SeriesContainer
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
            if (series is LineSeries || series is HiLoSeries || series is HiLoOpenCloseSeries)
            {
                var points = new PointCollection();
                var lowPoints = new PointCollection();
                var pointCount = 0;
                PartsCollection partsCollection = new PartsCollection();
                if (series is LineSeries)
                {
                    LineSeries lineSeries = series as LineSeries;
                    points = lineSeries.LinePoints;
                    pointCount = lineSeries.LinePoints.Count;
                    partsCollection = lineSeries.Parts;
                }
                else if(series is HiLoSeries)
                {
                    HiLoSeries lineSeries = series as HiLoSeries;
                    points = lineSeries.HighPoints;
                    lowPoints = lineSeries.LowPoints;
                    pointCount = lineSeries.HighPoints.Count;
                    partsCollection = lineSeries.Parts;
                }
                else if (series is HiLoOpenCloseSeries)
                {
                    HiLoOpenCloseSeries lineSeries = series as HiLoOpenCloseSeries;
                    points = lineSeries.HighPoints;
                    lowPoints = lineSeries.LowPoints;
                    pointCount = lineSeries.HighPoints.Count;
                    partsCollection = lineSeries.Parts;
                }
                if (RenderingMode == RenderingMode.Default)
                {
                    for (int i = 0; i < partsCollection.Count; i++)
                    {
                        UIElement renderElement = partsCollection[i].CreatePart();
                        if (renderElement != null && !PartsCanvas.Children.Contains(renderElement))
                            PartsCanvas.Children.Add(renderElement);
                    }
                }
                else
                {
                    if (series is LineSeries)
                    {
                        for (int i = 0; i < pointCount - 1; i++)
                        {

                            switch (RenderingMode)
                            {
                                case RenderingMode.GDIRendering:
                                    GDIGraphics.DrawLine(pen, points[i].AsDrawingPointF(), points[i + 1].AsDrawingPointF());

                                    break;
                                case RenderingMode.Default:
                                    break;
                                case RenderingMode.WritableBitmap:
                                    this.WritableBitmap.Lock();
                                    WritableBitmapGraphics.DrawLine(pen, points[i].AsDrawingPointF(), points[i + 1].AsDrawingPointF());
                                    this.WritableBitmap.Unlock();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < pointCount; i++)
                        {

                            switch (RenderingMode)
                            {
                                case RenderingMode.GDIRendering:
                                    GDIGraphics.DrawLine(pen, points[i].AsDrawingPointF(), lowPoints[i].AsDrawingPointF());

                                    break;
                                case RenderingMode.Default:
                                    break;
                                case RenderingMode.WritableBitmap:
                                    this.WritableBitmap.Lock();
                                    WritableBitmapGraphics.DrawLine(pen, points[i].AsDrawingPointF(), lowPoints[i].AsDrawingPointF());
                                    this.WritableBitmap.Unlock();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    this.Collection.InvalidateBitmap();
                }                
            }        
        }
#else
        protected override void DrawPath(SeriesBase series, Brush brush, double strokeThickness)
        {
            if (series is LineSeries || series is HiLoSeries)
            {
                
                PartsCollection partsCollection = new PartsCollection();
                if (series is LineSeries)
                {
                    LineSeries lineSeries = series as LineSeries;                  
                    partsCollection = lineSeries.Parts;
                }
                else if (series is HiLoSeries)
                {
                    HiLoSeries lineSeries = series as HiLoSeries;                   
                    partsCollection = lineSeries.Parts;
                }
                if (RenderingMode == RenderingMode.Default)
                {
                    for (int i = 0; i < partsCollection.Count; i++)
                    {
                        UIElement element = partsCollection[i].CreatePart();
                        if (element != null)
                        PartsCanvas.Children.Add(element);
                    }
                }
            }
        }
#endif
    }
}
