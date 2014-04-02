using System;

namespace Sparrow.Chart
{
    /// <summary>
    /// TimeSpan Point for Series.Points
    /// </summary>
    public class TimeSpanPoint : ChartPoint
    {
        private TimeSpan _mTimeSpan;
        /// <summary>
        /// Gets or sets the time span.
        /// </summary>
        /// <value>
        /// The time span.
        /// </value>
        public TimeSpan TimeSpan
        {
            get
            {
                return _mTimeSpan;
            }
            set
            {
                _mTimeSpan = value;
                this.XValue = _mTimeSpan.TotalMilliseconds;
            }
        }
        private double _mValue;
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value
        {
            get
            {
                return _mValue;
            }
            set
            {
                _mValue = value;
                this.YValue = value;
            }
        }
    }
}
