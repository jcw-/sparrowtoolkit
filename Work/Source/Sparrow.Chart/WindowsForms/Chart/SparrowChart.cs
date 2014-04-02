using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Sparrow.Chart
{
    public class SparrowChart : Control
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SparrowChart"/> class.
        /// </summary>
        public SparrowChart()
        {
            this.SetDefaultValues();
            this.GetBrushes();
        }
       
        internal RectF PositionedRect
        {
            get;
            set;
        }        

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Brush> Brushes
        {
            get;
            set;
        }

         [DefaultValue(false)]
        public SmoothingMode SmoothingMode
        {
            get;
            set;
        }

        [DefaultValue(false)]
        public CompositingQuality CompositingQuality
        {
            get; 
            set;
        }

         [DefaultValue(false)]
        public CompositingMode CompositingMode
        {
            get; 
            set; 
        }


        private SeriesCollection series;
        [DefaultValue(false)]
        public SeriesCollection Series
        {
            get { return series; }
            set
            {
                if(series != null)
                    series.CollectionChanged -= OnSeriesCollectionChanged;
                series = value;
                if(value != null)
                    series.CollectionChanged += OnSeriesCollectionChanged;
            }
        }

        private void OnSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                    case NotifyCollectionChangedAction.Add:
                    foreach (Series newSeries in e.NewItems)
                    {
                        newSeries.Chart = this;
                        if (newSeries.XAxis == null)
                            newSeries.XAxis = this.XAxis;
                        else
                        {
                            if (!this.Axes.Contains(newSeries.XAxis))
                                this.Axes.Add(newSeries.XAxis);                                                        
                        }
                        if (newSeries.YAxis == null)
                            newSeries.YAxis = this.YAxis;
                        else
                        {
                            if (!this.Axes.Contains(newSeries.YAxis))
                                this.Axes.Add(newSeries.YAxis);  
                        }
                        if (Brushes.Count > 1)
                            newSeries.Stroke = newSeries.Stroke ?? Brushes[this.Series.IndexOf(newSeries) % (Brushes.Count)];
                        else
                            newSeries.Stroke = newSeries.Stroke ?? Brushes[Brushes.Count];                                              
                    }
                    break;
                    case NotifyCollectionChangedAction.Move:
                    break;
                    case NotifyCollectionChangedAction.Remove:
                    break;
                    case NotifyCollectionChangedAction.Replace:
                    break;
                    case NotifyCollectionChangedAction.Reset:
                    break;
                     
            }            
            Invalidate();
        }


        internal Axes Axes
        {
            get;
            set;
        }

        [DefaultValue(false)]
        public Legend Legend
        {
            get; 
            set;
        }

        protected override void OnPaint(PaintEventArgs e)
        {            
            this.RootGraphics = e.Graphics;
            this.RootGraphics.CompositingMode = this.CompositingMode;
            this.RootGraphics.CompositingQuality = this.CompositingQuality;
            this.RootGraphics.SmoothingMode = this.SmoothingMode;
            this.RootGraphics.Clear(BackColor);
            Pen borderPen = this.BorderPen ?? new Pen(this.BorderBrush,this.BorderThickness);
            
            this.PositionedRect = new RectF(new PointF(borderPen.Width, borderPen.Width),
                                            new SizeF(this.Width - (borderPen.Width * 2), this.Height - (borderPen.Width * 2)));
            this.PositionedRect=RenderTitle(this.PositionedRect, borderPen.Width);
            if (borderPen.Width > 0)
                this.RootGraphics.DrawRectangle(borderPen, (borderPen.Width > 2) ? borderPen.Width/2 : 0,
                                                (borderPen.Width > 2) ? borderPen.Width/2 : 0,
                                                this.Width - (borderPen.Width), this.Height - (borderPen.Width));
            foreach (Axis axis in this.Axes)
            {
                axis.PositionedRect = this.PositionedRect;
                axis.RootGraphics = this.RootGraphics;
                axis.Chart = this;
                axis.Refresh();
                this.PositionedRect = axis.PreRenderAxis();
            }
            foreach (Axis axis in Axes)
            {
                axis.PositionedRect = this.PositionedRect;
                axis.RenderAxis();
            }
            
            foreach (var series in Series)
            {
                series.RootGraphics = this.RootGraphics;
                series.Refresh();
            }
            
        }

        private RectF RenderTitle(RectF positionedRect,float borderWidth)
        {
            SizeF textSize = this.RootGraphics.MeasureString(this.Title, this.TitleStyle.Font);
            float x = 0, y = 0;
            switch (this.TitleStyle.HorizontalAlignment)
            {
                    case HorizontalAlignment.Center:
                    x = (positionedRect.X + (this.PositionedRect.Width/2)) - (textSize.Width/2);                                       
                    break;
                    case  HorizontalAlignment.Left:
                    x = positionedRect.X;
                    break;
                    case  HorizontalAlignment.Right:
                    x = (positionedRect.X + (this.PositionedRect.Width)) - (textSize.Width);
                    break;
            }
            switch (this.TitleStyle.VerticalAlignment)
            {
                case VerticalAlignment.Center:
                    y = (positionedRect.Y) + (this.TitleMargin/2);
                    break;
                case VerticalAlignment.Bottom:
                    y = (positionedRect.Y) + this.TitleMargin;
                    break;
                case VerticalAlignment.Top:
                    y = positionedRect.Y;
                    break;
            }
            if (!String.IsNullOrEmpty(this.Title))
            {
                this.RootGraphics.DrawString(this.Title, this.TitleStyle.Font, this.TitleStyle.Foreground, x,
                                             y);
            }
            positionedRect =
                new RectF(new PointF(positionedRect.X, positionedRect.Y + (this.TitleMargin + textSize.Height)),
                          new SizeF(positionedRect.Width, positionedRect.Height - textSize.Height - this.TitleMargin));
            return positionedRect;
        }

        internal Graphics RootGraphics
        {
            get; 
            set;
        }

        private Theme theme;
        [DefaultValue(false)]
        public Theme Theme
        {
            get
            {
                return theme;
            }
            set 
            { 
                theme = value;
                GetBrushes();
                BrushTheme();
            }
        }


        public TextStyle TitleStyle
        {
            get; 
            set;
        }

        public string Title
        {
            get; 
            set;
        }

        public float TitleMargin { get; set; }

        private XAxis xAxis;
        [DefaultValue(false)]
        public XAxis XAxis
        {
            get { return xAxis; }
            set
            {
                if (this.Axes.Contains(xAxis))
                    this.Axes.Remove(xAxis);
                xAxis = value;
                if (!this.Axes.Contains(xAxis))
                    this.Axes.Add(xAxis);
            }
        }

        private YAxis yAxis;
        [DefaultValue(false)]
        public YAxis YAxis
        {
            get { return yAxis; }
            set
            {
                if (yAxis!=null && this.Axes.Contains(yAxis))
                    this.Axes.Remove(yAxis);
                yAxis = value;
                if (yAxis != null && !this.Axes.Contains(yAxis))
                    this.Axes.Add(yAxis);
            }
        }


        private void SetDefaultValues()
        {
            this.Series=new SeriesCollection();
            this.Theme=Theme.Metro;
            this.Axes=new Axes();
            this.Axes.CollectionChanged += OnAxesCollectionChanged;
            this.SmoothingMode = SmoothingMode.HighQuality;
            this.CompositingMode=CompositingMode.SourceOver;
            this.CompositingQuality=CompositingQuality.HighQuality;
            this.Brushes=new List<Brush>();
            this.XAxis=new CategoryXAxis();
            this.YAxis = new LinearYAxis();
            this.BorderBrush=new SolidBrush(Color.Black);
            this.BorderThickness = 1.0f;
            this.ContainerBorderPen = new Pen(Color.Black, 0f);
            TextStyle textStyle=new TextStyle();
            textStyle.Font = new Font("Arial",16f);
            textStyle.Foreground=new SolidBrush(Color.Black);
            textStyle.HorizontalAlignment=HorizontalAlignment.Center;
            textStyle.VerticalAlignment=VerticalAlignment.Center;
            this.TitleStyle=textStyle;
            this.TitleMargin = 5f;
        }

        void OnAxesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                    case NotifyCollectionChangedAction.Add:
                    foreach (Axis axis in e.NewItems)
                    {
                        axis.Chart = this;
                    }
                    break;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
           Invalidate();
        }

        private void GetBrushes()
        {
            switch (Theme)
            {
                case Theme.Arctic:
                    Brushes = Themes.ArcticBrushes();
                    break;
                case Theme.Autmn:
                    Brushes = Themes.AutmnBrushes();
                    break;
                case Theme.Cold:
                    Brushes = Themes.ColdBrushes();
                    break;
                case Theme.Flower:
                    Brushes = Themes.FlowerBrushes();
                    break;
                case Theme.Forest:
                    Brushes = Themes.ForestBrushes();
                    break;
                case Theme.Grayscale:
                    Brushes = Themes.GrayscaleBrushes();
                    break;
                case Theme.Ground:
                    Brushes = Themes.GroundBrushes();
                    break;
                case Theme.Lialac:
                    Brushes = Themes.LialacBrushes();
                    break;
                case Theme.Natural:
                    Brushes = Themes.NaturalBrushes();
                    break;
                case Theme.Pastel:
                    Brushes = Themes.PastelBrushes();
                    break;
                case Theme.Rainbow:
                    Brushes = Themes.RainbowBrushes();
                    break;
                case Theme.Spring:
                    Brushes = Themes.SpringBrushes();
                    break;
                case Theme.Summer:
                    Brushes = Themes.SummerBrushes();
                    break;
                case Theme.Warm:
                    Brushes = Themes.WarmBrushes();
                    break;
                case Theme.Metro:
                    Brushes = Themes.MetroBrushes();
                    break;
                case Theme.Custom:
                    break;
            }    
        }
        private void BrushTheme()
        {
            if (this.Series != null)
                foreach (var series in Series)
                {
                    if (Brushes.Count > 1)
                        series.Stroke = Brushes[Series.IndexOf(series)%(Brushes.Count)];
                    else
                        series.Stroke = Brushes[Brushes.Count];
                }
        }

        [DefaultValue(false)]
        public Brush BorderBrush
        {
            get; 
            set;
        }

        [DefaultValue(false)]
        public float BorderThickness
        {
            get; 
            set;
        }

        [DefaultValue(false)]
        public Pen BorderPen
        {
            get; 
            set;
        }

        [DefaultValue(false)]
        public Pen ContainerBorderPen
        {
            get;
            set;
        }        
        
    }
}
