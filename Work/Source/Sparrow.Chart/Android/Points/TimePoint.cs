using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
	/// <summary>
	/// TimePoint for Series.Points
	/// </summary>
	public class TimePoint : ChartPoint
	{
		private DateTime m_time;
		public DateTime Time
		{
			get
			{
				return m_time;
			}
			set
			{
				m_time = value;
				this.XValue = m_time.ToOADate();
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

