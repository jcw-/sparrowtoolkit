using System.Windows;
#if !WINRT
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
#endif

namespace Sparrow.Chart
{
    public class PiePart : SeriesPartBase
    {
        public Shape Slice { get; set; }

        public PiePart(Shape slice)
        {
            var strokeBinding = new Binding {Source = this, Path = new PropertyPath("Stroke")};
            slice.SetBinding(Shape.FillProperty, strokeBinding);
            Slice = slice;
        }

        /// <summary>
        /// Create a visual for single Series Part
        /// </summary>
        /// <returns>
        /// UIElement
        /// </returns>
        public override UIElement CreatePart()
        {
            return Slice;
        }
    }
}
