namespace Sparrow.Chart
{
    /// <summary>
    /// DoublePoint for Series.Points
    /// </summary>
    public class DoublePoint : ChartPoint
    {
        private double _mData;
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public double Data
        {
            get
            {
                return _mData;
            }
            set
            {
                _mData = value;
                this.XValue = value;
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
