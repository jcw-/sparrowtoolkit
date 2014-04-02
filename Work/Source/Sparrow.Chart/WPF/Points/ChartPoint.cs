using System.ComponentModel;

namespace Sparrow.Chart
{
    /// <summary>
    /// ChartPoint
    /// </summary>
    public class ChartPoint : INotifyPropertyChanged
    {
        private double _xValue;
        /// <summary>
        /// Gets or sets the X value.
        /// </summary>
        /// <value>
        /// The X value.
        /// </value>
        internal double XValue
        {
            get { return _xValue;}
            set { _xValue = value; OnPropertyChanged("XValue"); }
        }
        private double _yValue;
        /// <summary>
        /// Gets or sets the Y value.
        /// </summary>
        /// <value>
        /// The Y value.
        /// </value>
        internal double YValue
        {
            get { return _yValue; }
            set { _yValue = value; OnPropertyChanged("YValue"); }
        }
        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
