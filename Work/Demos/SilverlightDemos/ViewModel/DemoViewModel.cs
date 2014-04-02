using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace Sparrow.Chart.Demos
{
    public class DemoViewModel
    {       
        
        public ObservableCollection<SampleModel> Samples 
        {
            get;
            set; 
        }

        public ObservableCollection<CategoryModel> Categories { get; set; }

        public DemoViewModel()
        {
            Samples=new ObservableCollection<SampleModel>();
            Categories = new ObservableCollection<CategoryModel>();
            AddSamples();        
        }

        private void AddSamples()
        {
            //CPUPerformance.CPUView cpuView = new CPUPerformance.CPUView();

            //Sparrow.Chart.Demos.Demos.PerformanceDemo.PerformanceDemo performanceView = new Demos.PerformanceDemo.PerformanceDemo();
            CategoryModel category = new CategoryModel();
            XmlReader reader = null;
             try
             {
                 reader = XmlReader.Create("Demos.xml");

                 while (reader.Read())
                 {
                     if (reader.IsStartElement())
                     {
                         switch (reader.Name)
                         {
                             case "Category":                                 
                                 category = new CategoryModel(reader["Name"]);
                                 this.Categories.Add(category);
                                 break;
                             case "Sample":
                                 category.Samples.Add(new SampleModel(reader["Name"], "", reader["Class"],reader["Image"]));
                                 break;
                         }
                     }

                 }
             }
             catch (Exception)
             {
             }
             finally
             {
                 if (reader != null)
                     reader.Close();
             }

             foreach (var cat in Categories)
             {
                 foreach (var sample in cat.Samples)
                 {
                     sample.IsHeader = false;
                     Samples.Add(sample);
                 }
             }
            //SampleModel performanceDemo = new SampleModel("Performance Demo", "", "Sparrow.Chart.Demos.Demos.PerformanceDemo.PerformanceDemo");
            //performanceDemo.IsHeader = false;

            //SampleModel liveDataDemo = new SampleModel("Live Datas Demo", "", "Sparrow.Chart.Demos.Demos.LiveDatasDemo.LiveDatasDemo");
            //liveDataDemo.IsHeader = false;

            //Samples.Add(liveDataDemo);
            //Samples.Add(performanceDemo);

        }
       
    }
}
