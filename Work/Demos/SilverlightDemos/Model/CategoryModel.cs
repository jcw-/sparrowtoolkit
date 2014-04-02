using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Sparrow.Chart.Demos
{
    public class CategoryModel : INotifyPropertyChanged
    {
        private string categoryName;
         public string CategoryName 
        {
            get { return categoryName; }
            set { categoryName = value; RaisePropertyChanged("CategoryName"); }
        }

        private List<SampleModel> samples;
        public List<SampleModel> Samples
        {
            get { return samples; }
            set { samples = value; RaisePropertyChanged("Samples"); }
        }

        private bool isHeader;
        public bool IsHeader
        {
            get { return isHeader; }
            set { isHeader = value; RaisePropertyChanged("IsHeader"); }
        }

        private SampleModel selectedModel;
        public SampleModel SelectedModel
        {
            get { return selectedModel; }
            set { selectedModel = value; RaisePropertyChanged("SelectedModel"); }
        }

        public CategoryModel(string categoryName)
        {
            this.CategoryName = categoryName;
            this.SelectedModel = new SampleModel("", "", "");
            this.Samples = new List<SampleModel>();
        }

        public CategoryModel()
        {

        }



        public CategoryModel(string categoryName, List<SampleModel> samples)
        {
            this.CategoryName = categoryName;
            this.SelectedModel=new SampleModel("","","");
            this.Samples = samples;           
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
