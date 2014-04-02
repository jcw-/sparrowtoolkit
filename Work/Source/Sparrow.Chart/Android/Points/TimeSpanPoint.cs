using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
	/// <summary>
	/// TimeSpan Point for Series.Points
	/// </summary>
	public class TimeSpanPoint : ChartPoint
	{
		private TimeSpan m_timeSpan;
		public TimeSpan TimeSpan
		{
			get
			{
				return m_timeSpan;
			}
			set
			{
				m_timeSpan = value;
				this.XValue = m_timeSpan.TotalMilliseconds;
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

