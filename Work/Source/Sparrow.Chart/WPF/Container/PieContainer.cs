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
    public class PieContainer : SeriesContainer
    {
#if WPF
        override protected void DrawPath(SeriesBase series, System.Drawing.Pen pen)
        {
            if (series is PieSeries)
            {
                var points = new PointCollection();
                var pointCount = 0;
                var partsCollection = new PartsCollection();
                if (series is PieSeries)
                {
                    var pieSeries = series as PieSeries;
                    points = pieSeries.PiePoints;
                    pointCount = pieSeries.PiePoints.Count;
                    partsCollection = pieSeries.Parts;
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
                    if (series is PieSeries)
                    {
                        for (int i = 0; i < pointCount; i++)
                        {

                            switch (RenderingMode)
                            {
                                case RenderingMode.GDIRendering:
                                    //GDIGraphics.DrawLine(pen, points[i].AsDrawingPointF(), points[i + 1].AsDrawingPointF());

                                    break;
                                case RenderingMode.Default:
                                    break;
                                case RenderingMode.WritableBitmap:
                                    //this.WritableBitmap.Lock();
                                    //WritableBitmapGraphics.DrawLine(pen, points[i].AsDrawingPointF(), points[i + 1].AsDrawingPointF());
                                   // this.WritableBitmap.Unlock();
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
            if (series is PieSeries)
            {
                var points = new PointCollection();
                var pointCount = 0;
                var partsCollection = new PartsCollection();
                if (series is PieSeries)
                {
                    var pieSeries = series as PieSeries;
                    points = pieSeries.PiePoints;
                    pointCount = pieSeries.PiePoints.Count;
                    partsCollection = pieSeries.Parts;
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
            }
        }
#endif
    }
}
