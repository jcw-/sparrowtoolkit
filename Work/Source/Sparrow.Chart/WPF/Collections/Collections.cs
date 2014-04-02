using System.Collections.ObjectModel;

namespace Sparrow.Chart
{
    /// <summary>
    /// Series Parts Collection
    /// </summary>
    public class PartsCollection : ObservableCollection<SeriesPartBase>
    {
    }

    /// <summary>
    /// ChartPoint Collection
    /// </summary>
    public class PointsCollection : ObservableCollection<ChartPoint>
    {
    }

    /// <summary>
    /// Series Collection
    /// </summary>
    public class SeriesCollection : ObservableCollection<SeriesBase>
    {
    }


    /// <summary>
    /// Container Collection
    /// </summary>
    public class Containers : ObservableCollection<SeriesContainer>
    {
    }

    /// <summary>
    /// Axis Collection
    /// </summary>
    public class Axes : ObservableCollection<AxisBase>
    {
    }

}
