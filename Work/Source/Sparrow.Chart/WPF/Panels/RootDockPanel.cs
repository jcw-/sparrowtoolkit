using System;
using System.Windows;
#if !WINRT
using System.Windows.Controls;

#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
#endif
namespace Sparrow.Chart
{
    /// <summary>
    /// Chart Root DockPanel
    /// </summary>
    public class RootPanel : DockPanel
    {

        /// <summary>
        /// Gets or sets the legend dock.
        /// </summary>
        /// <value>
        /// The legend dock.
        /// </value>
        public Dock LegendDock
        {
            get { return (Dock)GetValue(LegendDockProperty); }
            set { SetValue(LegendDockProperty, value); }
        }

        /// <summary>
        /// The legend dock property
        /// </summary>
        public static readonly DependencyProperty LegendDockProperty =
            DependencyProperty.Register("LegendDock", typeof(Dock), typeof(RootPanel), new PropertyMetadata(Dock.Top));
        
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //ContainerCollection container = (ContainerCollection)this.FindName("PART_containers");
            base.ArrangeOverride(arrangeSize);            
            return arrangeSize;
        }

        /// <summary>
        /// Measures the child elements of a
        /// <see cref="T:System.Windows.Controls.DockPanel" /> in preparation
        /// for arranging them during the
        /// <see cref="M:System.Windows.Controls.DockPanel.ArrangeOverride(System.Windows.Size)" />
        /// pass.
        /// </summary>
        /// <param name="constraint">The area available to the
        /// <see cref="T:System.Windows.Controls.DockPanel" />.</param>
        /// <returns>
        /// The desired size of the
        /// <see cref="T:System.Windows.Controls.DockPanel" />.
        /// </returns>
        protected override Size MeasureOverride(Size constraint)
        {
            DockPanel.SetDock(this.Children[0], LegendDock);
            Size desiredSize = new Size(0, 0);
            int count = 0;
            
            foreach (UIElement child in Children)
            {                
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
                        
            return base.MeasureOverride(constraint);
        }
    }
}
