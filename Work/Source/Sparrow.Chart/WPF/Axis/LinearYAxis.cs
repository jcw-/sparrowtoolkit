namespace Sparrow.Chart
{
    /// <summary>
    /// Linear YAxis
    /// </summary>
    public class LinearYAxis : YAxis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearYAxis"/> class.
        /// </summary>
        public LinearYAxis() :
            base()
        {
            this.Type = YType.Double;
        }
    }
}
