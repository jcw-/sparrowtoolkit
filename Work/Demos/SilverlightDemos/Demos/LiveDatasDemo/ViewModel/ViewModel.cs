using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Sparrow.Chart.Demos.Demos.LiveDatasDemo
{
    public class DataGenerator : ViewModelBase
    {
        private ObservableCollection<Data> collection;
        public ObservableCollection<Data> Collection 
        {
            get { return collection; }
            set { collection = value; OnPropertyChanged("Collection");}
        }

        private ObservableCollection<Data> collection1;
        public ObservableCollection<Data> Collection1
        {
            get { return collection1; }
            set { collection1 = value; OnPropertyChanged("Collection1"); }
        }

        private DispatcherTimer timer;
        private double phase;
        static double increment = Math.PI * 0.1;
        public DataGenerator()
        {           
            Collection = Datas.FourierSeries(2.0, phase, 1000);
            Collection1 = Datas.FourierSeries(1.0, phase, 1000);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += OnTick;
            timer.Start();
        }

        void OnTick(object sender, EventArgs e)
        {
          Collection1 = Datas.FourierSeries(2.0, phase, 1000);
          Collection = Datas.FourierSeries(1.0, phase, 1000);
          phase += increment;          
        }
    }
}
