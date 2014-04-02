using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
#else
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Foundation;
#endif

namespace Sparrow.BulletGraph
{
    public class ElementPanel : Panel
    {

        public BulletGraph BulletGraph
        {
            get { return (BulletGraph)GetValue(BulletGraphProperty); }
            set { SetValue(BulletGraphProperty, value); }
        }

        public static readonly DependencyProperty BulletGraphProperty =
            DependencyProperty.Register("BulletGraph", typeof(BulletGraph), typeof(ElementPanel), new PropertyMetadata(null));



        public QualitativeRangeCollection QualitativeRanges
        {
            get { return (QualitativeRangeCollection)GetValue(QualitativeRangesProperty); }
            set { SetValue(QualitativeRangesProperty, value); }
        }

        public static readonly DependencyProperty QualitativeRangesProperty =
            DependencyProperty.Register("QualitativeRanges", typeof(QualitativeRangeCollection), typeof(ElementPanel), new PropertyMetadata(null));

        
        protected override Size MeasureOverride(Size constraint)
        {
            Size desiredSize = new Size(0, 0);
            foreach (UIElement item in Children)
            {
                item.Measure(constraint);
                desiredSize.Height += item.DesiredSize.Height;
                desiredSize.Width += item.DesiredSize.Width;
            }
            if (double.IsPositiveInfinity(constraint.Width))
                constraint.Width = desiredSize.Width;
            if (double.IsPositiveInfinity(constraint.Height))
                constraint.Height = desiredSize.Height;
            return constraint;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement item in Children)
            {
                item.Arrange(new Rect(0, 0, arrangeSize.Width, arrangeSize.Height));
            }

            this.BulletGraph.CalculateRects(arrangeSize);
            this.BulletGraph.Update();
            return arrangeSize;
        }
    }
}
