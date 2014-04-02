using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
	/// <summary>
	/// DoublePoint for Series.Points
	/// </summary>
	public class DoublePoint : ChartPoint
	{
		public DoublePoint()
		{
			
		}
		private double m_data;
		public double Data
		{
			get
			{
				return m_data;
			}
			set
			{
				m_data = value;
				this.XValue = value;
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

