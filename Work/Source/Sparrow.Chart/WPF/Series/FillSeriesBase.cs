using System.Windows;
#if !WINRT
using System.Windows.Data;
using System.Windows.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
#endif


namespace Sparrow.Chart
{
    /// <summary>
    /// Fill Based Series Such as Area,Scatter and Column Series
    /// </summary>
    public class FillSeriesBase : LineSeriesBase
    {
        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// The fill property
        /// </summary>
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(FillSeriesBase), new PropertyMetadata(null));


        /// <summary>
        /// Sets the binding for strokeand stroke thickness.
        /// </summary>
        /// <param name="part">The part.</param>
        protected override void SetBindingForStrokeandStrokeThickness(SeriesPartBase part)
        {
            Binding fillBinding = new Binding {Path = new PropertyPath("Fill"), Source = this};
            part.SetBinding(FillPartBase.FillProperty, fillBinding);
            base.SetBindingForStrokeandStrokeThickness(part);
        }
    }
}
