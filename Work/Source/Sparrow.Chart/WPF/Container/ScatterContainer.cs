using System.Collections.Generic;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
#else
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
#endif

namespace Sparrow.Chart
{
    /// <summary>
    /// Scatter Series Container
    /// </summary>
    public class ScatterContainer : SeriesContainer
    {
#if WPF
        override protected void DrawPath(SeriesBase series, System.Drawing.Pen pen)
        {
            if (series is ScatterSeries || series is BubbleSeries)
            {
                var points = new PointCollection();
                var sizes=new List<double>();
                var pointCount = 0;
                float size = 0;
                var brush=new SolidColorBrush(Colors.Blue).AsDrawingBrush();
                PartsCollection partsCollection = new PartsCollection();
                if(series is ScatterSeries)   
                {
                   ScatterSeries scatterSeries = series as ScatterSeries;
                   points = scatterSeries.ScatterPoints;
                   pointCount = scatterSeries.ScatterPoints.Count;
                   size = (float)scatterSeries.ScatterSize;
                   brush = (this.Series as ScatterSeries).Fill.AsDrawingBrush();
                   partsCollection = scatterSeries.Parts;
                }
                else if (series is BubbleSeries)
                {
                    BubbleSeries scatterSeries = series as BubbleSeries;
                    points = scatterSeries.BubblePoints;
                    pointCount = scatterSeries.BubblePoints.Count;
                    //size = (float)scatterSeries.;
                    sizes=scatterSeries.SizeValues;
                    brush = (this.Series as FillSeriesBase).Fill.AsDrawingBrush();
                    partsCollection = scatterSeries.Parts;
                }
               
                if (RenderingMode == RenderingMode.Default)
                {
                    for (int i = 0; i < partsCollection.Count; i++)
                    {
                        Ellipse element = (Ellipse)partsCollection[i].CreatePart();
                        if (element != null && !PartsCanvas.Children.Contains(element))
                            PartsCanvas.Children.Add(element);                        
                    }
                }
                else
                {
                    for (int i = 0; i < pointCount; i++)
                    {
                        if (series is ScatterSeries)
                        {
                            switch (RenderingMode)
                            {
                                case RenderingMode.GDIRendering:
                                    GDIGraphics.DrawEllipse(pen, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
                                    GDIGraphics.FillEllipse(brush, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
                                    break;
                                case RenderingMode.Default:
                                    break;
                                case RenderingMode.WritableBitmap:
                                    this.WritableBitmap.Lock();
                                    WritableBitmapGraphics.DrawEllipse(pen, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
                                    WritableBitmapGraphics.FillEllipse(brush, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
                                    this.WritableBitmap.Unlock();
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            size=(int)sizes[i];
                            switch (RenderingMode)
                            {
                                case RenderingMode.GDIRendering:
                                    GDIGraphics.DrawEllipse(pen, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
                                    GDIGraphics.FillEllipse(brush, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
                                    break;
                                case RenderingMode.Default:
                                    break;
                                case RenderingMode.WritableBitmap:
                                    this.WritableBitmap.Lock();
                                    WritableBitmapGraphics.DrawEllipse(pen, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
                                    WritableBitmapGraphics.FillEllipse(brush, points[i].AsDrawingPointF().X - size / 2, points[i].AsDrawingPointF().Y - size / 2, size, size);
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
        /// <summary>
        /// Draws the path.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        protected override void DrawPath(SeriesBase series, Brush brush, double strokeThickness)
        {
            if (series is ScatterSeries || series is BubbleSeries)
            {
                var points = new PointCollection();
                var pointCount = 0;
                float size = 0;
                PartsCollection partsCollection = new PartsCollection();
                if (series is ScatterSeries)
                {
                    ScatterSeries scatterSeries = series as ScatterSeries;
                    points = scatterSeries.ScatterPoints;
                    pointCount = scatterSeries.ScatterPoints.Count;
                    size = (float)scatterSeries.ScatterSize;
                    partsCollection = scatterSeries.Parts;
                }
                else if (series is BubbleSeries)
                {
                    BubbleSeries scatterSeries = series as BubbleSeries;
                    points = scatterSeries.BubblePoints;
                    pointCount = scatterSeries.BubblePoints.Count;
                    partsCollection = scatterSeries.Parts;
                }

                if (RenderingMode == RenderingMode.Default)
                {
                    for (int i = 0; i < partsCollection.Count; i++)
                    {
                        Ellipse element = (Ellipse)partsCollection[i].CreatePart();
                        Canvas.SetLeft(element, points[i].X - (element.Width / 2));
                        Canvas.SetTop(element, points[i].Y - (element.Height / 2));
                        PartsCanvas.Children.Add(element);
                    }
                }
            }
        }
#endif
    }
}
