using System;

namespace Sparrow.Chart
{
    /// <summary>
    /// TimePoint for Series.Points
    /// </summary>
    public class TimePoint : ChartPoint
    {
        private DateTime _mTime;
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public DateTime Time
        {
            get
            {
                return _mTime;
            }
            set
            {
                _mTime = value;
                this.XValue = _mTime.ToOADate();
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
