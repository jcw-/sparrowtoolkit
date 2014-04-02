using System.Windows;
#if WINRT
using Windows.UI.Xaml;
#endif

namespace Sparrow.Chart
{

    /// <summary>
    /// IAxis
    /// </summary>
    public interface IAxis
    {        

        object MinValue
        {
            get;
            set;
        }

        object MaxValue
        {
            get;
            set;
        }

        object Interval
        {
            get;
            set;
        }

        Style AxisLineStyle
        {
            get;
            set;
        }

        SeriesCollection Series
        {
            get;
            set;
        }

        
    }
}
