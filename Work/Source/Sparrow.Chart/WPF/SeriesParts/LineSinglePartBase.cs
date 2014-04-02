#if !WINRT
using System.Windows.Media;

#else
using Windows.UI.Xaml.Media;
#endif

namespace Sparrow.Chart
{
    public class LineSinglePartBase : SeriesPartBase
    {
        /// <summary>
        /// Gets or sets the line points.
        /// </summary>
        /// <value>
        /// The line points.
        /// </value>
        public PointCollection LinePoints { get; set; }
    }
}
