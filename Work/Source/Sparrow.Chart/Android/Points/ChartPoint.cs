
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.ComponentModel;

namespace Sparrow.Chart
{
	/// <summary>
	/// ChartPoint for Series.Points
	/// </summary>
	public class ChartPoint : INotifyPropertyChanged
	{
		private double xValue;
		internal double XValue
		{
			get { return xValue;}
			set { xValue = value; OnPropertyChanged("XValue"); }
		}
		private double yValue;
		internal double YValue
		{
			get { return yValue; }
			set { yValue = value; OnPropertyChanged("YValue"); }
		}
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}

