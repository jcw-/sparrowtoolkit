using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    /// <summary>
    /// Series Parts Collection
    /// </summary>
    public class PartsCollection : ObservableCollection<SeriesPartBase>
    {
        public PartsCollection()
        {

        }
    }

    /// <summary>
    /// ChartPoint Collection
    /// </summary>
    public class PointsCollection : ObservableCollection<ChartPoint>
    {
        public PointsCollection()
        {

        }
    }

    /// <summary>
    /// Series Collection
    /// </summary>
    public class SeriesCollection : ObservableCollection<SeriesBase>
    {
        public SeriesCollection()
        {

        }
    }


    /// <summary>
    /// Container Collection
    /// </summary>
    public class Containers : ObservableCollection<SeriesContainer>
    {      
        public Containers()
        {
        }
    }

	/// <summary>
	/// User interface element collecion.
	/// </summary>
	public class UIElementCollecion : ObservableCollection<UIElement>
	{
		public UIElementCollecion ()
		{
			
		}
	}

}
