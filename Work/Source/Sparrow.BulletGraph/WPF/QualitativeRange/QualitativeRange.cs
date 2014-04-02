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
    public class QualitativeRange : FrameworkElement
    {
        internal Rectangle RangeRect
        {
            get;
            set;
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(QualitativeRange), new PropertyMetadata(1d,OnMaximumChanged));

        private static void OnMaximumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as QualitativeRange).MaximumChanged(args);
        }
        internal void MaximumChanged(DependencyPropertyChangedEventArgs args)
        {
            if (this.BulletGraph != null && this.BulletGraph.isRendered)
            {
                this.Refresh(this.BulletGraph.RangeRect);
            }
        }

        internal BulletGraph BulletGraph
        {
            get { return (BulletGraph)GetValue(BulletGraphProperty); }
            set { SetValue(BulletGraphProperty, value); }
        }

        internal static readonly DependencyProperty BulletGraphProperty =
            DependencyProperty.Register("BulletGraph", typeof(BulletGraph), typeof(QualitativeRange), new PropertyMetadata(null));



        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(QualitativeRange), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        

        internal Rectangle GetRangeRectangle()
        {
            RangeRect = new Rectangle();
            Binding fillBinding = new Binding();
            fillBinding.Source = this;
            fillBinding.Path = new PropertyPath("Fill");
            RangeRect.SetBinding(Rectangle.FillProperty, fillBinding);
            return RangeRect;
        }

        internal void Refresh(Rect renderRect)
        {
            
            switch (this.BulletGraph.Orientation)
            {
                case Orientation.Horizontal:
                    RangeRect.Height = renderRect.Height;
                    RangeRect.Width = this.GetPointFromValue(this.Maximum,renderRect);
                    Canvas.SetTop(RangeRect, renderRect.Top);
                    Canvas.SetLeft(RangeRect, renderRect.Left);
                    break;
                case Orientation.Vertical:
                    RangeRect.Height = this.BulletGraph.GetPointFromValue(this.BulletGraph.Minimum, renderRect) - this.BulletGraph.GetPointFromValue(this.Maximum, renderRect);
                    RangeRect.Width = renderRect.Width;
                    Canvas.SetTop(RangeRect, this.BulletGraph.GetPointFromValue(this.Maximum, renderRect));
                    Canvas.SetLeft(RangeRect, renderRect.Left);
                    break;
            }
            
        }


        internal double GetPointFromValue(double value,Rect renderedRect)
        {
            double maximumCoefficient = this.BulletGraph.CalculateCoefficient(this.Maximum);
            switch (this.BulletGraph.Orientation)
            {
                case Orientation.Horizontal:
                    return (this.Maximum - this.BulletGraph.Minimum) * (renderedRect.Width / maximumCoefficient);
                case Orientation.Vertical:
                    break;
            }
            return 0;
        }
    }
}
