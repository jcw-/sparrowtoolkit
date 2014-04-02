using System;
using System.Diagnostics;
#if WPF
using System.Drawing;
#endif
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Windows.Point;
using Image = System.Windows.Controls.Image;
using System.Windows.Threading;
using Size = System.Windows.Size;
using System.Windows.Data;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
#endif

#if DIRECTX2D
using Sparrow.Directx2D;
using D2D = Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using D3D10 = Microsoft.WindowsAPICodePack.DirectX.Direct3D10;
using DirectX = Microsoft.WindowsAPICodePack.DirectX;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
#endif
namespace Sparrow.Chart
{
    /// <summary>
    /// Container Panel
    /// </summary>
    public class ContainerCollection : Panel
    {
        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public SeriesCollection Series
        {
            get { return (SeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// The series property
        /// </summary>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(SeriesCollection), typeof(ContainerCollection), new PropertyMetadata(null));

        Canvas _containerCanvas;
#if DIRECTX2D
        protected Directx2DBitmap DirectxBimap;
        protected D3D10Image Directx2DGraphics;
        private readonly D2D.D2DFactory factory;
        private D3D10.D3DDevice device;
        private D2D.RenderTarget renderTarget;
        private D3D10.Texture2D texture;
       
#endif
#if WPF
        private bool disposed;
        protected bool isDirectXInitialized;
        private int bpp = PixelFormats.Bgra32.BitsPerPixel / 8;
        private IntPtr MapViewPointer;
        protected Graphics GDIGraphics;
        protected InteropBitmap InteropBitmap;
        internal double dpiFactor;
        protected WriteableBitmap WritableBitmap;
        protected Bitmap ImageBitmap;
        protected Graphics WritableBitmapGraphics;
#endif
        private Canvas _partsCanvas;
        private bool _isLegendUpdate;

        

        internal AxisCrossLinesContainer AxisLinesconatiner;
        bool _isIntialized;

        private const uint FileMapAllAccess = 0xF001F;
        private const uint PageReadwrite = 0x04;



        /// <summary>
        /// Gets or sets the containers.
        /// </summary>
        /// <value>
        /// The containers.
        /// </value>
        public Containers Containers
        {
            get { return (Containers)GetValue(ContainersProperty); }
            set { SetValue(ContainersProperty, value); }
        }

        Image bitmapImage;

        /// <summary>
        /// The containers property
        /// </summary>
        public static readonly DependencyProperty ContainersProperty =
            DependencyProperty.Register("Containers", typeof(Containers), typeof(ContainerCollection), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerCollection"/> class.
        /// </summary>
        public ContainerCollection()
        {
            Containers = new Containers();
            this.SizeChanged += ContainerCollection_SizeChanged;
            bitmapImage = new Image();
            bitmapImage.Stretch = Stretch.None;
#if DIRECTX2D
             this.factory = D2D.D2DFactory.CreateFactory(D2D.D2DFactoryType.Multithreaded);
            this.Directx2DGraphics = new D3D10Image();
#endif            
            
        }

        /// <summary>
        /// Handles the SizeChanged event of the ContainerCollection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        void ContainerCollection_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if !WPF
            RectangleGeometry clipRectGeometry = new RectangleGeometry();
            clipRectGeometry.Rect = new Rect(new Point(0, 0), new Size(this.ActualWidth,this.ActualHeight));
            this.Clip = clipRectGeometry;
#endif
            
            Refresh();
        }
       
#if DIRECTX2D
        protected void InitializeDirectX()
        {
            this.Directx2DGraphics.Lock();
            try
            {
                this.CreateResources();

                // Resize to the size of this control, if we have a size

                width = Math.Max(1, (int)this.ActualWidth);
                height = Math.Max(1, (int)this.ActualHeight);
                this.Resize(width, height);

                this.Directx2DGraphics.SetBackBuffer(this.Texture);
                if (this.Directx2DGraphics.IsFrontBufferAvailable)
                {
                    this.Directx2DGraphics.Lock();
                    this.Directx2DGraphics.AddDirtyRect(new Int32Rect(0, 0, this.Directx2DGraphics.PixelWidth, this.Directx2DGraphics.PixelHeight));
                    this.Directx2DGraphics.Unlock();
                }
                 
            }
            finally
            {
                this.Directx2DGraphics.Unlock();
            }
            isDirectXInitialized = true;
        }
#endif
        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
#if DIRECTX2D
            if (RenderingMode == RenderingMode.DirectX2D)
                InitializeDirectX();
#endif
            switch (RenderingMode)
            {
#if WPF
                case RenderingMode.GDIRendering:
                    if (this.InteropBitmap != null && this.GDIGraphics != null)
                    {
                        this.GDIGraphics.Dispose();
                        this.GDIGraphics = null;
                        this.InteropBitmap = null;
                        GC.Collect();
                    }
                    break;
               case RenderingMode.WritableBitmap:
                    if (this.WritableBitmap != null && this.WritableBitmapGraphics != null)
                    {
                        this.WritableBitmapGraphics.Dispose();
                        this.WritableBitmapGraphics = null;
                        this.WritableBitmap = null;
                        GC.Collect();
                    }
                    break;
#endif
                case RenderingMode.Default:
                    break;
                
                default:
                    break;
            }

            Initialize();
            if (!_isIntialized)
                GenerateConatiners();
            else
                UpdateContainers();            

            if (this.XAxis != null)
            {
                this.XAxis.MMinValue = 0;
                this.XAxis.MMaxValue = 1;
                this.XAxis.CalculateIntervalFromSeriesPoints();
                this.XAxis.Refresh();
            }
            if (this.YAxis != null)
            {
                this.YAxis.MMinValue = 0;
                this.YAxis.MMaxValue = 1;
                this.YAxis.CalculateIntervalFromSeriesPoints();
                this.YAxis.Refresh();
            }

            if (this.Series != null)
            {
                foreach (SeriesBase series in this.Series)
                {
                    if (Containers.Count > 0 && (this.Series.Count == Containers.Count))
                        series.SeriesContainer = Containers[series.Index];
                    series.Refresh();
                }
            }

        }

        internal void Refresh(bool invalidate)
        {
#if DIRECTX2D
            if (RenderingMode == RenderingMode.DirectX2D)
                InitializeDirectX();
#endif
            switch (RenderingMode)
            {
#if WPF
                case RenderingMode.GDIRendering:
                    if (this.InteropBitmap != null && this.GDIGraphics != null)
                    {
                        this.GDIGraphics.Dispose();
                        this.GDIGraphics = null;
                        this.InteropBitmap = null;
                        GC.Collect();
                    }
                    break;
                case RenderingMode.WritableBitmap:
                    if (this.WritableBitmap != null && this.WritableBitmapGraphics != null)
                    {
                        this.WritableBitmapGraphics.Dispose();
                        this.WritableBitmapGraphics = null;
                        this.WritableBitmap = null;
                        GC.Collect();
                    }
                    break;
#endif
                case RenderingMode.Default:
                    break;

                default:
                    break;
            }

            Initialize();
            if (!_isIntialized || invalidate)
                GenerateConatiners();
            else
                UpdateContainers();

            if (this.XAxis != null)
            {
                this.XAxis.MMinValue = 0;
                this.XAxis.MMaxValue = 1;
                this.XAxis.CalculateIntervalFromSeriesPoints();
                this.XAxis.Refresh();
            }
            if (this.YAxis != null)
            {
                this.YAxis.MMinValue = 0;
                this.YAxis.MMaxValue = 1;
                this.YAxis.CalculateIntervalFromSeriesPoints();
                this.YAxis.Refresh();
            }
            if (this.Series != null)
            {
                foreach (SeriesBase series in this.Series)
                {
                    if (Containers.Count > 0 && (this.Series.Count == Containers.Count))
                        series.SeriesContainer = Containers[series.Index];
                    series.Refresh();
                }
            }

        }
        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            try
            {
                switch (this.RenderingMode)
                {
#if WPF
                    case RenderingMode.GDIRendering:
                        GDIGraphics.Clear(System.Windows.Media.Colors.Transparent.AsDrawingColor());
                        break;
                    case RenderingMode.WritableBitmap:
                        WritableBitmapGraphics.Clear(System.Windows.Media.Colors.Transparent.AsDrawingColor());
                        break;
#endif
                    case RenderingMode.Default:
                        break;
                    
                    default:
                        break;
                }
                
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception Occured : " + e.Message);
            }
        }

        private bool _isBitmapInitialized;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is bitmap initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is bitmap initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsBitmapInitialized
        {
            get { return _isBitmapInitialized; }
            set { _isBitmapInitialized = value; }
        }

        public void Invalidate()
        {
#if WPF
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {
                Draw();
            }));
#elif WINRT
           IAsyncAction action= this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
            {
                Draw();
            });
#else

            DispatcherOperation dispatcherOperation = this.Dispatcher.BeginInvoke(new Action(Draw));
#endif
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public virtual void Draw()
        {
        }
 #if WPF
        public void DrawPath(System.Windows.Media.PathGeometry seriesData, System.Drawing.Pen gdiPen, System.Drawing.Brush gdiBrush)
        {
            foreach (PathFigure figure in seriesData.Figures)
            {
                var points = figure.Segments.Cast<LineSegment>().Select(s => s.Point.AsDrawingPoint());

                var nextPoint = points.First();

                foreach (var point in points)
                {
                    var lastPoint = nextPoint;
                    nextPoint = point;
                    GDIGraphics.DrawLine(gdiPen, lastPoint, nextPoint);
                }
            }
        }
#endif
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
#if WPF && !PUPLISH
            if (ActualHeight > 1 && ActualWidth > 1)
            {
                switch (RenderingMode)
                {

                    case RenderingMode.GDIRendering:
                        uint byteCount = (uint)(ActualWidth * ActualHeight * bpp);

                        //Allocate and create the InteropBitmap
                        var fileMappingPointer = CreateFileMapping(new IntPtr(-1), IntPtr.Zero, PageReadwrite, 0, byteCount, null);
                        this.MapViewPointer = MapViewOfFile(fileMappingPointer, FileMapAllAccess, 0, 0, byteCount);
                        var format = PixelFormats.Bgra32;
                        var stride = (int)((int)ActualWidth * (int)format.BitsPerPixel / 8);
                        this.InteropBitmap = Imaging.CreateBitmapSourceFromMemorySection(fileMappingPointer,
                                                                                    (int)ActualWidth,
                                                                                    (int)ActualHeight,
                                                                                    format,
                                                                                    stride,
                                                                                    0) as InteropBitmap;
                        this.GDIGraphics = GetGdiGraphics(MapViewPointer);
                        break;

                    case RenderingMode.WritableBitmap:
                        //Allocate and create the WritableBitmap
                        WritableBitmap = new WriteableBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Bgra32, null);
                        ImageBitmap = new Bitmap((int)ActualWidth, (int)ActualHeight, ((int)ActualWidth * 4), System.Drawing.Imaging.PixelFormat.Format32bppPArgb, WritableBitmap.BackBuffer);
                        WritableBitmapGraphics = System.Drawing.Graphics.FromImage(ImageBitmap);
                        WritableBitmapGraphics.CompositingMode = this.CompositingMode.AsDrawingCompositingMode();
                        WritableBitmapGraphics.CompositingQuality = this.CompositingQuality.AsDrawingCompositingQuality();
                        WritableBitmapGraphics.SmoothingMode = this.SmoothingMode.AsDrawingSmoothingMode();
                        break;
                    default:
                        break;
                }

                Clear();
                this.IsBitmapInitialized = true;
            } 
#endif

        }

        /// <summary>
        /// Invalidates the bitmap.
        /// </summary>
        public void InvalidateBitmap()
        {      
 #if WPF
            switch (RenderingMode)
            {
                case RenderingMode.Default:
                    break;
#if DIRECTX2D
                case RenderingMode.DirectX2D:
                    if (!isDirectXInitialized)
                        InitializeDirectX();
                    this.bitmapImage.Source = Directx2DGraphics;
                    break;
#endif

                case RenderingMode.GDIRendering:
                    this.bitmapImage.Source = (BitmapSource)InteropBitmap.GetAsFrozen();
                    break;
                case RenderingMode.WritableBitmap:
                    this.bitmapImage.Source = (BitmapSource)WritableBitmap.GetAsFrozen();
                    break;
                default:
                    break;
            }
            this.bitmapImage.InvalidateVisual();  
#endif
        }
        private int _index;
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        internal int Index
        {
            get { return _index; }
            set { _index = value; }
        }
#if WPF
        private Graphics GetGdiGraphics(IntPtr mapViewPointer)
        {
            Graphics gdiGraphics;
            //create the GDI Bitmap pointing to the same part of memory.
            System.Drawing.Bitmap gdiBitmap;
            gdiBitmap = new System.Drawing.Bitmap((int)ActualWidth,
                                                  (int)ActualHeight,
                                                  (int)ActualWidth * bpp,
                                                  System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                                  mapViewPointer);          
            // Get GDI Graphics 
            gdiGraphics = System.Drawing.Graphics.FromImage(gdiBitmap);
          
            gdiGraphics.CompositingMode = this.CompositingMode.AsDrawingCompositingMode();
            gdiGraphics.CompositingQuality = this.CompositingQuality.AsDrawingCompositingQuality();
            gdiGraphics.SmoothingMode = this.SmoothingMode.AsDrawingSmoothingMode();

            return gdiGraphics;
        }
#endif

        /// <summary>
        /// Measures the override.
        /// </summary>
        /// <param name="constraint">The constraint.</param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            Size desiredSize = new Size(0, 0);
            int count = 0;

            foreach (UIElement child in Children)
            {
                //child.   
                Canvas.SetZIndex(child, count);
                count++;
                child.Measure(constraint);
                desiredSize.Width += child.DesiredSize.Width;
                desiredSize.Height += child.DesiredSize.Height;
            }
            if (Double.IsInfinity(constraint.Height))
                constraint.Height = desiredSize.Height;
            if (Double.IsInfinity(constraint.Width))
                constraint.Width = desiredSize.Width;
            return constraint;
        }

        /// <summary>
        /// Arranges the override.
        /// </summary>
        /// <param name="arrangeSize">Size of the arrange.</param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(new Point(0, 0), arrangeSize));
            }           
            return arrangeSize;
        }
#if WPF
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "5"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass"), DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFileMapping(IntPtr hFile,
                                                       IntPtr lpFileMappingAttributes,
                                                       uint flProtect,
                                                       uint dwMaximumSizeHigh,
                                                       uint dwMaximumSizeLow,
                                                       string lpName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "4"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass"), DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject,
                                                   uint dwDesiredAccess,
                                                   uint dwFileOffsetHigh,
                                                   uint dwFileOffsetLow,
                                                   uint dwNumberOfBytesToMap);
#endif
        /// <summary>
        /// Updates the containers.
        /// </summary>
        private void UpdateContainers()
        {
            if (XAxis != null && YAxis != null)
            {
                AxisLinesconatiner.Height = this.ActualHeight;
                AxisLinesconatiner.Width = this.ActualWidth;
            }
            foreach (var container in Containers)
            {               
                container.RenderingMode = this.RenderingMode;
#if WPF
                container.dpiFactor = this.dpiFactor;
#endif
                //container.Height = this.ActualHeight;
                //container.Width = this.ActualWidth;
                switch (RenderingMode)
                {
#if WPF
                    case RenderingMode.GDIRendering:
                        container.InteropBitmap = this.InteropBitmap;
                        container.GDIGraphics = this.GDIGraphics;
                        break;
                    case RenderingMode.Default:
                        break;
                   case RenderingMode.WritableBitmap:
                        container.WritableBitmap = this.WritableBitmap;
                        container.WritableBitmapGraphics = this.WritableBitmapGraphics;
                        container.ImageBitmap = this.ImageBitmap;
                        break;
#endif
#if DIRECTX2D
                    case Chart.RenderingMode.DirectX2D:
                        container.Directx2DGraphics = this.Directx2DGraphics;
                        container.RenderTarget = this.RenderTarget;
                        break;
#endif

                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Generates the conatiners.
        /// </summary>
        private void GenerateConatiners()
        {
            this.Children.Clear();
            this.Containers.Clear();
            if (XAxis != null && YAxis != null)
            {
                AxisLinesconatiner = new AxisCrossLinesContainer {Height = this.ActualHeight, Width = this.ActualWidth};
                Binding xAxisBinding = new Binding {Path = new PropertyPath("XAxis"), Source = this};
                Binding yAxisBinding = new Binding {Source = this, Path = new PropertyPath("YAxis")};
                AxisLinesconatiner.SetBinding(AxisCrossLinesContainer.XAxisProperty, xAxisBinding);
                AxisLinesconatiner.SetBinding(AxisCrossLinesContainer.YAxisProperty, yAxisBinding);

                if (this.Chart != null && this.Chart.OverlayMode == OverlayMode.SeriesFirst)
                    this.Children.Add(AxisLinesconatiner);
            }
            if(this.Series!=null)
                foreach (var seriesBase in Series)
                {
                    SeriesContainer container = seriesBase.CreateContainer();
                    seriesBase.Height = this.ActualHeight;
                    seriesBase.Width = this.ActualWidth;
                    container.Series = seriesBase;
                    container.Container = this;
                    container.RenderingMode = this.RenderingMode;
#if WPF
                    container.dpiFactor = this.dpiFactor;
#endif
                    container.Collection = this;
                    switch (RenderingMode)
                    {
#if WPF
                        case RenderingMode.GDIRendering:
                            container.InteropBitmap = this.InteropBitmap;
                            container.GDIGraphics = this.GDIGraphics;
                            break;
                        case RenderingMode.WritableBitmap:
                            container.WritableBitmap = this.WritableBitmap;
                            container.WritableBitmapGraphics = this.WritableBitmapGraphics;
                            container.ImageBitmap = this.ImageBitmap;
                            break;
#endif
                        case RenderingMode.Default:
                            this.Children.Add(container.PartsCanvas);
                            break;
#if DIRECTX2D
                    case Chart.RenderingMode.DirectX2D:
                        container.Directx2DGraphics = this.Directx2DGraphics;
                        container.RenderTarget = this.RenderTarget;
                        break;
#endif

                        default:
                            break;
                    }

                    this.Containers.Add(container);
                }
          
            this.Children.Add(bitmapImage);
            if (XAxis != null && YAxis != null)
            {
                if (this.Chart != null && this.Chart.OverlayMode == OverlayMode.AxisFirst)
                    this.Children.Add(AxisLinesconatiner);
            }
            _isIntialized = true;
        }
#if WPF
        internal SmoothingMode SmoothingMode
        {
            get { return (SmoothingMode)GetValue(SmoothingModeProperty); }
            set { SetValue(SmoothingModeProperty, value); }
        }

        public static readonly DependencyProperty SmoothingModeProperty =
            DependencyProperty.Register("SmoothingMode", typeof(SmoothingMode), typeof(ContainerCollection), new PropertyMetadata(SmoothingMode.HighQuality));



        internal CompositingQuality CompositingQuality
        {
            get { return (CompositingQuality)GetValue(CompositingQualityProperty); }
            set { SetValue(CompositingQualityProperty, value); }
        }

        public static readonly DependencyProperty CompositingQualityProperty =
            DependencyProperty.Register("CompositingQuality", typeof(CompositingQuality), typeof(ContainerCollection), new PropertyMetadata(CompositingQuality.HighQuality));

         internal CompositingMode CompositingMode
        {
            get { return (CompositingMode)GetValue(CompositingModeProperty); }
            set { SetValue(CompositingModeProperty, value); }
        }

        public static readonly DependencyProperty CompositingModeProperty =
            DependencyProperty.Register("CompositingMode", typeof(CompositingMode), typeof(ContainerCollection), new PropertyMetadata(CompositingMode.SourceOver));

#endif
        /// <summary>
        /// Gets or sets the rendering mode.
        /// </summary>
        /// <value>
        /// The rendering mode.
        /// </value>
        public RenderingMode RenderingMode
        {
            get { return (RenderingMode)GetValue(RenderingModeProperty); }
            set { SetValue(RenderingModeProperty, value); }
        }

        /// <summary>
        /// The rendering mode property
        /// </summary>
        public static readonly DependencyProperty RenderingModeProperty =
            DependencyProperty.Register("RenderingMode", typeof(RenderingMode), typeof(ContainerCollection), new PropertyMetadata(RenderingMode.Default));



        /// <summary>
        /// Gets or sets the chart.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public SparrowChart Chart
        {
            get { return (SparrowChart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        /// <summary>
        /// The chart property
        /// </summary>
        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(SparrowChart), typeof(ContainerCollection), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the X axis.
        /// </summary>
        /// <value>
        /// The X axis.
        /// </value>
        public XAxis XAxis
        {
            get { return (XAxis)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        /// <summary>
        /// The X axis property
        /// </summary>
        public static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register("XAxis", typeof(XAxis), typeof(ContainerCollection), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the Y axis.
        /// </summary>
        /// <value>
        /// The Y axis.
        /// </value>
        public YAxis YAxis
        {
            get { return (YAxis)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        /// <summary>
        /// The Y axis property
        /// </summary>
        public static readonly DependencyProperty YAxisProperty =
            DependencyProperty.Register("YAxis", typeof(YAxis), typeof(ContainerCollection), new PropertyMetadata(null));
#if DIRECTX2D
        public Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SolidColorBrush GetDirectXBrush(Color color)
        {
            Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SolidColorBrush brush = this.RenderTarget.CreateSolidColorBrush(new ColorF(((float)color.R / 255), ((float)color.G / 255), ((float)color.B / 255), ((float)color.A / 255)));
            return brush;
        }

        public Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush GetDirectXBrush(System.Windows.Media.Brush mediaBrush)
        {
            if (mediaBrush is System.Windows.Media.SolidColorBrush)
                return GetDirectXBrush(((System.Windows.Media.SolidColorBrush)mediaBrush).Color);
            else if (mediaBrush is System.Windows.Media.RadialGradientBrush)
            {
                System.Windows.Media.RadialGradientBrush mediaRadialBrush = (mediaBrush as System.Windows.Media.RadialGradientBrush);
                RadialGradientBrushProperties properties = new RadialGradientBrushProperties(mediaRadialBrush.Center.AsDirectX2DPoint(), mediaRadialBrush.GradientOrigin.AsDirectX2DPoint(), (float)mediaRadialBrush.RadiusX, (float)mediaRadialBrush.RadiusY);
                List<Microsoft.WindowsAPICodePack.DirectX.Direct2D1.GradientStop> stopCollection = new List<Microsoft.WindowsAPICodePack.DirectX.Direct2D1.GradientStop>();
                foreach (var stop in mediaRadialBrush.GradientStops)
                {
                    stopCollection.Add(new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.GradientStop((float)stop.Offset, new ColorF((float)stop.Color.R, ((float)stop.Color.G / 255), ((float)stop.Color.B / 255), ((float)stop.Color.A / 255))));
                }
                Microsoft.WindowsAPICodePack.DirectX.Direct2D1.GradientStopCollection stops = this.RenderTarget.CreateGradientStopCollection(stopCollection, Gamma.StandardRgb, ExtendMode.Wrap);

                Microsoft.WindowsAPICodePack.DirectX.Direct2D1.RadialGradientBrush radialBrush = this.RenderTarget.CreateRadialGradientBrush(properties, stops);
                return radialBrush;
            }
            return null;

        }

        /// <summary>
        /// Raised when the content of the Scene has changed.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>Gets the surface this instance draws to.</summary>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Dispose()"/> has been called on this instance.
        /// </exception>
        public D3D10.Texture2D Texture
        {
            get
            {
                this.ThrowIfDisposed();
                return this.texture;
            }
        }

        /// <summary>
        /// Gets the <see cref="D2D.D2DFactory"/> used to create the resources.
        /// </summary>
        protected D2D.D2DFactory Factory
        {
            get { return this.factory; }
        }


        /// <summary>
        /// Gets the <see cref="D2D.RenderTarget"/> used for drawing.
        /// </summary>
        protected D2D.RenderTarget RenderTarget
        {
            get { return this.renderTarget; }
        }

        /// <summary>
        /// Immediately frees any system resources that the object might hold.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a DirectX 10 device and related device specific resources.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// A previous call to CreateResources has not been followed by a call to
        /// <see cref="FreeResources"/>.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Dispose()"/> has been called on this instance.
        /// </exception>
        /// <exception cref="DirectX.DirectXException">
        /// Unable to create a DirectX 10 device or an error occured creating
        /// device dependent resources.
        /// </exception>
        public void CreateResources()
        {
            FreeResources();
            this.ThrowIfDisposed();
            if (this.device != null)
            {
                throw new InvalidOperationException("A previous call to CreateResources has not been followed by a call to FreeResources.");
            }

            // Try to create a hardware device first and fall back to a
            // software (WARP doens't let us share resources)
            var device1 = TryCreateDevice1(D3D10.DriverType.Hardware);
            if (device1 == null)
            {
                device1 = TryCreateDevice1(D3D10.DriverType.Software);
                if (device1 == null)
                {
                    throw new DirectX.DirectXException("Unable to create a DirectX 10 device.");
                }
            }
            this.device = device1.QueryInterface<D3D10.D3DDevice>();
            device1.Dispose();
        }

        /// <summary>
        /// Releases the DirectX device and any device dependent resources.
        /// </summary>
        /// <remarks>
        /// This method is safe to be called even if the instance has been disposed.
        /// </remarks>
        public void FreeResources()
        {
            this.OnFreeResources();

            if (this.texture != null)
            {
                this.texture.Dispose();
                this.texture = null;
            }
            if (this.renderTarget != null)
            {
                this.renderTarget.Dispose();
                this.renderTarget = null;
            }
            if (this.device != null)
            {
                this.device.Dispose();
                this.device = null;
            }
        }

        /// <summary>
        /// Causes the scene to redraw its contents.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Resize"/> has not been called.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Dispose()"/> has been called on this instance.
        /// </exception>
        public void Render()
        {
            this.ThrowIfDisposed();
            if (this.renderTarget == null)
            {
                throw new InvalidOperationException("Resize has not been called.");
            }

            this.OnRender();
            this.device.Flush();
            this.OnUpdated();
        }

        /// <summary>Resizes the scene.</summary>
        /// <param name="width">The new width for the scene.</param>
        /// <param name="height">The new height for the scene.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// width/height is less than zero.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="CreateResources"/> has not been called.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Dispose()"/> has been called on this instance.
        /// </exception>
        /// <exception cref="DirectX.DirectXException">
        /// An error occured creating device dependent resources.
        /// </exception>
        public void Resize(int width, int height)
        {
            this.ThrowIfDisposed();
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException("width", "Value must be positive.");
            }
            if (height < 0)
            {
                throw new ArgumentOutOfRangeException("height", "Value must be positive.");
            }
            if (this.device == null)
            {
                throw new InvalidOperationException("CreateResources has not been called.");
            }

            // Recreate the render target
            this.CreateTexture(width, height);
            using (var surface = this.texture.QueryInterface<DirectX.Graphics.Surface>())
            {
                this.CreateRenderTarget(surface);
            }

            // Resize our viewport
            var viewport = new D3D10.Viewport();
            viewport.Height = (uint)height;
            viewport.MaxDepth = 1;
            viewport.MinDepth = 0;
            viewport.TopLeftX = 0;
            viewport.TopLeftY = 0;
            viewport.Width = (uint)width;
            this.device.RS.Viewports = new D3D10.Viewport[] { viewport };

            // Destroy and recreate any dependent resources declared in a
            // derived class only (i.e don't destroy our resources).
            this.OnFreeResources();
            this.OnCreateResources();
        }

        /// <summary>
        /// Immediately frees any system resources that the object might hold.
        /// </summary>
        /// <param name="disposing">
        /// Set to true if called from an explicit disposer; otherwise, false.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            this.FreeResources();
            if (disposing)
            {
                this.factory.Dispose();
            }

            this.disposed = true;
        }

        /// <summary>
        /// When overriden in a derived class, creates device dependent resources.
        /// </summary>
        internal void OnCreateResources()
        {
        }

        /// <summary>
        /// When overriden in a deriven class, releases device dependent resources.
        /// </summary>
        internal void OnFreeResources()
        {
        }

        /// <summary>
        /// When overriden in a derived class, renders the Direct2D content.
        /// </summary>
        protected virtual void OnRender()
        {
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if
        /// <see cref="Dispose()"/> has been called on this instance.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        private static D3D10.D3DDevice1 TryCreateDevice1(D3D10.DriverType type)
        {
            // We'll try to create the device that supports any of these feature levels
            DirectX.Direct3D.FeatureLevel[] levels =
            {
                DirectX.Direct3D.FeatureLevel.Ten,
                DirectX.Direct3D.FeatureLevel.NinePointThree,
                DirectX.Direct3D.FeatureLevel.NinePointTwo,
                DirectX.Direct3D.FeatureLevel.NinePointOne
            };

            foreach (var level in levels)
            {
                try
                {
                    return D3D10.D3DDevice1.CreateDevice1(null, type, null, D3D10.CreateDeviceOptions.SupportBgra, level);
                }
                catch (ArgumentException) // E_INVALIDARG
                {
                    continue; // Try the next feature level
                }
                catch (OutOfMemoryException) // E_OUTOFMEMORY
                {
                    continue; // Try the next feature level
                }
                catch (DirectX.DirectXException) // D3DERR_INVALIDCALL or E_FAIL
                {
                    continue; // Try the next feature level
                }
            }
            return null; // We failed to create a device at any required feature level
        }

        private void CreateRenderTarget(DirectX.Graphics.Surface surface)
        {
            // Create a D2D render target which can draw into our offscreen D3D
            // surface. D2D uses device independant units, like WPF, at 96/inch
            var properties = new D2D.RenderTargetProperties();
            properties.DpiX = 96;
            properties.DpiY = 96;
            properties.MinLevel = DirectX.Direct3D.FeatureLevel.Default;
            properties.PixelFormat = new D2D.PixelFormat(DirectX.Graphics.Format.Unknown, D2D.AlphaMode.Premultiplied);
            properties.RenderTargetType = D2D.RenderTargetType.Default;
            properties.Usage = D2D.RenderTargetUsages.None;           
            // Assign result to temporary variable in case CreateGraphicsSurfaceRenderTarget throws
            var target = this.factory.CreateGraphicsSurfaceRenderTarget(surface, properties);
            target.AntiAliasMode = AntiAliasMode.Aliased;
            if (this.renderTarget != null)
            {
                this.renderTarget.Dispose();
            }
            this.renderTarget = target;
        }

        private void CreateTexture(int width, int height)
        {
            var description = new D3D10.Texture2DDescription();
            description.ArraySize = 1;
            description.BindingOptions = D3D10.BindingOptions.RenderTarget | D3D10.BindingOptions.ShaderResource;
            description.CpuAccessOptions = D3D10.CpuAccessOptions.None;
            description.Format = DirectX.Graphics.Format.B8G8R8A8UNorm;
            description.MipLevels = 1;
            description.MiscellaneousResourceOptions = D3D10.MiscellaneousResourceOptions.Shared;
            description.SampleDescription = new DirectX.Graphics.SampleDescription(1, 0);
            description.Usage = D3D10.Usage.Default;

            description.Height = (uint)height;
            description.Width = (uint)width;

            // Assign result to temporary variable in case CreateTexture2D throws
            var texture = this.device.CreateTexture2D(description);
            if (this.texture != null)
            {
                this.texture.Dispose();
            }
            this.texture = texture;
        }

        private void OnUpdated()
        {
            var callback = this.Updated;
            if (callback != null)
            {
                callback(this, EventArgs.Empty);
            }
        }
#endif
    }
}
