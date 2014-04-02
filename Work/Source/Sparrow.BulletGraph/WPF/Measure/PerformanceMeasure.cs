using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.Foundation;
#endif


namespace Sparrow.BulletGraph
{
    public class PerformanceMeasure : FrameworkElement
    {
        public Rectangle MeasureElement { get; set; }

        public BulletGraph BulletGraph { get; set; }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(PerformanceMeasure), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public double Measure
        {
            get { return (double)GetValue(MeasureProperty); }
            set { SetValue(MeasureProperty, value); }
        }

        public static readonly DependencyProperty MeasureProperty =
            DependencyProperty.Register("Measure", typeof(double), typeof(PerformanceMeasure), new PropertyMetadata(0d));
        

        public double MeasureThickness
        {
            get { return (double)GetValue(MeasureThicknessProperty); }
            set { SetValue(MeasureThicknessProperty, value); }
        }

        public static readonly DependencyProperty MeasureThicknessProperty =
            DependencyProperty.Register("MeasureThickness", typeof(double), typeof(PerformanceMeasure), new PropertyMetadata(0d));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(PerformanceMeasure), new PropertyMetadata(Orientation.Horizontal));

        internal Rectangle GetMeasureElement()
        {
            MeasureElement = new Rectangle();
            Binding fillBinding = new Binding();
            fillBinding.Source = this;
            fillBinding.Path = new PropertyPath("Fill");
            MeasureElement.SetBinding(Rectangle.FillProperty, fillBinding);
            return MeasureElement;
        }

        internal void Refresh(Rect renderedRect)
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    this.MeasureElement.Width = this.BulletGraph.GetPointFromValue(this.Measure, renderedRect) - renderedRect.Left;
                    this.MeasureElement.Height = this.MeasureThickness;
                    Canvas.SetTop(this.MeasureElement, renderedRect.Top + ((renderedRect.Height / 2) - (this.MeasureThickness / 2)));
                    Canvas.SetLeft(this.MeasureElement, renderedRect.Left);
                    break;
                case Orientation.Vertical:
                    MeasureElement.Height = this.BulletGraph.GetPointFromValue(this.BulletGraph.Minimum, renderedRect) - this.BulletGraph.GetPointFromValue(this.Measure, renderedRect);
                    MeasureElement.Width = this.MeasureThickness;
                    Canvas.SetTop(MeasureElement, this.BulletGraph.GetPointFromValue(this.Measure, renderedRect));
                    Canvas.SetLeft(MeasureElement, renderedRect.Left + (renderedRect.Width / 2) - (this.MeasureThickness / 2));
                    break;
                default:
                    break;
            }
        }
    }
}
