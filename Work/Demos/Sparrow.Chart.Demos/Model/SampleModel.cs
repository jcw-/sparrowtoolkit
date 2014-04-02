using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Sparrow.Chart.Demos
{
    public class SampleModel : INotifyPropertyChanged
    {
        private string sampleName;
        public string SampleName 
        {
            get { return sampleName; }
            set { sampleName = value; RaisePropertyChanged("SampleName"); }
        }

        private bool isHeader;
        public bool IsHeader
        {
            get { return isHeader; }
            set { isHeader = value; RaisePropertyChanged("IsHeader"); }
        }

        private bool isIntialized;
        public bool IsIntialized
        {
            get { return isIntialized; }
            set { isIntialized = value; RaisePropertyChanged("IsIntialized"); }
        }

        private UserControl view;
        public UserControl View
        {
            get { return view; }
            set { view = value;IsIntialized=true ;RaisePropertyChanged("View"); }
        }

        private string viewClass;
        public string ViewClass
        {
            get { return viewClass; }
            set { viewClass = value; RaisePropertyChanged("ViewClass"); }
        }

        private string sampleLocation;
        public string SampleLocation
        {
            get { return sampleLocation; }
            set { sampleLocation = value; RaisePropertyChanged("SampleLocation"); }
        }

        public static bool GetIsBusy(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsBusyProperty);
        }

        public static void SetIsBusy(DependencyObject obj, bool value)
        {
            obj.SetValue(IsBusyProperty, value);
        }
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.RegisterAttached("IsBusy", typeof(bool), typeof(SampleModel), new PropertyMetadata(false));


        public SampleModel(string sampleName, string sampleLocation, string viewClass)
        {
            this.SampleName = sampleName;
            this.SampleLocation = sampleLocation;
            this.ViewClass = viewClass;
        }
        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        protected void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null)
            {
                changed(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
