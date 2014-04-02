namespace Sparrow.Chart
{
    /// <summary>
    /// Category XAxis
    /// </summary>
    public class CategoryXAxis : XAxis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryXAxis"/> class.
        /// </summary>
        public CategoryXAxis()
            : base()
        {
            this.Type = XType.Category;
        }
    }
}
