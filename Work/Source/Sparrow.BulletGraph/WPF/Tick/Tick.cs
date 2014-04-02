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
    public class Tick : FrameworkElement
    {
        public Line TickElement { get; set; }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Tick), new PropertyMetadata(0d));



        public double TickSize
        {
            get { return (double)GetValue(TickSizeProperty); }
            set { SetValue(TickSizeProperty, value); }
        }

        public static readonly DependencyProperty TickSizeProperty =
            DependencyProperty.Register("TickSize", typeof(double), typeof(Tick), new PropertyMetadata(0d));



        public Style LineStyle
        {
            get { return (Style)GetValue(LineStyleProperty); }
            set { SetValue(LineStyleProperty, value); }
        }

        public static readonly DependencyProperty LineStyleProperty =
            DependencyProperty.Register("LineStyle", typeof(Style), typeof(Tick), new PropertyMetadata(null));


        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(Tick), new PropertyMetadata(0d));



        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(Tick), new PropertyMetadata(0d));



        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(Tick), new PropertyMetadata(0d));



        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(Tick), new PropertyMetadata(0d));


        public ScalePosition TickPosition
        {
            get { return (ScalePosition)GetValue(TickPositionProperty); }
            set { SetValue(TickPositionProperty, value); }
        }

        public static readonly DependencyProperty TickPositionProperty =
            DependencyProperty.Register("TickPosition", typeof(ScalePosition), typeof(Tick), new PropertyMetadata(ScalePosition.Default));

        internal Line GetTickLine()
        {
            TickElement = new Line();
            
            Binding styleBinding = new Binding();
            styleBinding.Source = this;
            styleBinding.Path = new PropertyPath("LineStyle");
            TickElement.SetBinding(Line.StyleProperty, styleBinding);
            
            return TickElement;
        }

        internal void Refresh()
        {
            if (this.TickElement != null)
            {
                TickElement.X1 = this.X1;
                TickElement.X2 = this.X2;
                TickElement.Y1 = this.Y1;
                TickElement.Y2 = this.Y2;
            }
        }

    }
}
