
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

namespace Sparrow.Chart
{
	/// <summary>
	/// Category Point for Series.Points
	/// </summary>
	public class CategoryPoint : ChartPoint
	{
		public CategoryPoint()
		{
			
		}
		private string m_category;
		public string Category
		{
			get
			{
				return m_category;
			}
			set
			{
				m_category = value;
				if (!SparrowChart.ActualCategoryValues.Contains(m_category))
					SparrowChart.ActualCategoryValues.Add(m_category);
				this.XValue = SparrowChart.ActualCategoryValues.IndexOf(m_category);
			}
		}
		
		private double m_value;
		public double Value
		{
			get
			{
				return m_value;
			}
			set
			{
				m_value = value;
				this.YValue = value;
			}
		}
	}
}

