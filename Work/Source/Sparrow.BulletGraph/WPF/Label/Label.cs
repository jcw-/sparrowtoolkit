using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Data;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
#endif

namespace Sparrow.BulletGraph
{
    public class Label : FrameworkElement
    {
        public ContentControl LabelElement { get; set; }

        public Size Size 
        {
            get 
            { 
                return this.LabelElement.DesiredSize;
            } 
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Label), new PropertyMetadata(string.Empty));



        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Label), new PropertyMetadata(0d));



        public DataTemplate LabelTemplate
        {
            get { return (DataTemplate)GetValue(LabelTemplateProperty); }
            set { SetValue(LabelTemplateProperty, value); }
        }

        public static readonly DependencyProperty LabelTemplateProperty =
            DependencyProperty.Register("LabelTemplate", typeof(DataTemplate), typeof(Label), new PropertyMetadata(null));



        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Label), new PropertyMetadata(0d));



        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Label), new PropertyMetadata(0d));



        public LabelPosition LabelPosition
        {
            get { return (LabelPosition)GetValue(LabelPositionProperty); }
            set { SetValue(LabelPositionProperty, value); }
        }

        public static readonly DependencyProperty LabelPositionProperty =
            DependencyProperty.Register("LabelPosition", typeof(LabelPosition), typeof(Label), new PropertyMetadata(LabelPosition.Default));



        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Label), new PropertyMetadata(Orientation.Horizontal));


        internal ContentControl GetLabelElement()
        {
            LabelElement = new ContentControl();
            Binding templateBinding = new Binding();
            templateBinding.Source = this;
            templateBinding.Path = new PropertyPath("LabelTemplate");
            LabelElement.SetBinding(ContentControl.ContentTemplateProperty, templateBinding);
            Binding contentBinding = new Binding();
            contentBinding.Source = this;
            contentBinding.Path = new PropertyPath("Text");
            LabelElement.SetBinding(ContentControl.ContentProperty, contentBinding);
            return LabelElement;
        }

        internal Rect GetLabelRect()
        {
            Rect labelBounds = new Rect();
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    switch (LabelPosition)
                    {
                        case LabelPosition.Opposed:
                            labelBounds = new Rect(new Point(X - (Size.Width / 2), Y), Size);
                            break;
                        case LabelPosition.Cross:
                            labelBounds = new Rect(new Point(X - (Size.Width / 2), Y - (Size.Height / 2)), Size);
                            break;
                        case LabelPosition.Default:
                            labelBounds = new Rect(new Point(X - (Size.Width / 2), Y), Size);
                            break;
                        default:
                            break;
                    }
                    break;
                case Orientation.Vertical:
                    switch (LabelPosition)
                    {
                        case LabelPosition.Opposed:
                            labelBounds = new Rect(new Point(X , Y - (Size.Height / 2)), Size);
                            break;
                        case LabelPosition.Cross:
                            labelBounds = new Rect(new Point(X - (Size.Width / 2), Y - (Size.Height / 2)), Size);
                            break;
                        case LabelPosition.Default:
                            labelBounds = new Rect(new Point(X, Y - (Size.Height / 2)), Size);
                            break;
                        default:
                            break;
                    }
                    break;
            }
            return labelBounds;
        }

    }
}

