using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sparrow.Chart.Demos.Demos.PerformanceDemo
{
    public class ViewModel
    {
    }

    public class DataGenerator
    {
        public int DataCount = 100000;

        private Random randomNumber;

        public DataGenerator()
        {
            randomNumber = new Random();
        }

        public PointsCollection Generate()
        {
            PointsCollection collection = new PointsCollection();
            DateTime date = new DateTime(2009, 1, 1);
            double value = 1000;
            for (int i = 0; i < this.DataCount; i++)
            {
                collection.Add(new DoublePoint() { Data = i, Value = value });
                
                if (randomNumber.NextDouble() > .5)
                {
                    value += randomNumber.NextDouble();
                }
                else
                {
                    value -= randomNumber.NextDouble();
                }
            }
            return collection;
        }

        public ObservableCollection<PerformanceModel> GenerateData()
        {
            ObservableCollection<PerformanceModel> datas = new ObservableCollection<PerformanceModel>();

            DateTime date = new DateTime(2009, 1, 1);
            double value = 1000;
            for (int i = 0; i < this.DataCount; i++)
            {
                datas.Add(new PerformanceModel(date, value));
                date = date.Add(TimeSpan.FromSeconds(5));

                if (randomNumber.NextDouble() > .5)
                {
                    value += randomNumber.NextDouble();
                }
                else
                {
                    value -= randomNumber.NextDouble();
                }
            }

            return datas;
        }
    }
}
