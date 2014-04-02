namespace Sparrow.Chart
{
    /// <summary>
    /// DateTime XAxis
    /// </summary>
    public class DateTimeXAxis : XAxis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeXAxis"/> class.
        /// </summary>
        public DateTimeXAxis() :
            base()
        {
            this.Type = XType.DateTime;
        }
    }
}
