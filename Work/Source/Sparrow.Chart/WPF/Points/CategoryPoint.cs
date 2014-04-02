namespace Sparrow.Chart
{
    /// <summary>
    /// Category Point for Series.Points
    /// </summary>
    public class CategoryPoint : ChartPoint
    {
        private string _mCategory;
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category
        {
            get
            {
                return _mCategory;
            }
            set
            {
                _mCategory = value;
                if (!SparrowChart.ActualCategoryValues.Contains(_mCategory))
                    SparrowChart.ActualCategoryValues.Add(_mCategory);
                this.XValue = SparrowChart.ActualCategoryValues.IndexOf(_mCategory);
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
