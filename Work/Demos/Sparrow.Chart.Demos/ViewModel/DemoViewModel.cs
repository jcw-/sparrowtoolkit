using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Sparrow.Chart.Demos
{
    public class DemoViewModel
    {
        public ICommand AboutCommand 
        {
            get; 
            set; 
        }
        
        public ObservableCollection<CategoryModel> Categories 
        {
            get;
            set; 
        }               

        public DemoViewModel()
        {
            Categories = new ObservableCollection<CategoryModel>();
            AddCategories();
            AboutCommand = new AboutCommand();
        }

        private void AddCategories()
        {
            //CPUPerformance.CPUView cpuView = new CPUPerformance.CPUView();
            SampleModel cpuDemo = new SampleModel("Task Manager Demo", "", "CPUPerformance.CPUView");
            cpuDemo.IsHeader = false;

            //Sparrow.Chart.Demos.Demos.PerformanceDemo.PerformanceDemo performanceView = new Demos.PerformanceDemo.PerformanceDemo();
            SampleModel performanceDemo = new SampleModel("Performance Demo", "", "Sparrow.Chart.Demos.Demos.PerformanceDemo.PerformanceDemo");
            cpuDemo.IsHeader = false;

            SampleModel liveDataDemo = new SampleModel("Live Datas Demo", "", "Sparrow.Chart.Demos.Demos.LiveDatasDemo.LiveDatasDemo");
            liveDataDemo.IsHeader = false;

            List<SampleModel> showCaseSamples = new List<SampleModel>();
            showCaseSamples.Add(cpuDemo);
            showCaseSamples.Add(performanceDemo);
            showCaseSamples.Add(liveDataDemo);

            CategoryModel showCase = new CategoryModel("Showcase", showCaseSamples);
            showCase.IsHeader = true;
            Categories.Add(showCase);
        }

       
    }
}
