using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sparrow.Chart.Demos
{
    public class Datas
    {
        public static ObservableCollection<Data> FourierSeries(double amplitude, double phaseShift, int count = 5000)
        {
            ObservableCollection<Data> doublePoints = new ObservableCollection<Data>();

            for (int i = 0; i < count; i++)
            {
                var doublePoint = new Data();

                double time = 10 * i / (double)count;
                double wn = 2 * Math.PI / (count / 10);

                doublePoint.Value = time;
                doublePoint.Value1 = Math.PI * amplitude *
                            (Math.Sin(i * wn + phaseShift) +
                             0.33 * Math.Sin(i * 3 * wn + phaseShift) +
                             0.20 * Math.Sin(i * 5 * wn + phaseShift) +
                             0.14 * Math.Sin(i * 7 * wn + phaseShift) +
                             0.11 * Math.Sin(i * 9 * wn + phaseShift) +
                             0.09 * Math.Sin(i * 11 * wn + phaseShift));
                doublePoints.Add(doublePoint);
            }

            return doublePoints;
        }

        public static ObservableCollection<DoublePoint> EEG(int count, ref double startPhase, double phaseStep)
        {
            var doublePoints = new ObservableCollection<DoublePoint>();
            var rand = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < count; i++)
            {
                var doublePoint = new DoublePoint();

                var time = i / (double)count;
                doublePoint.Data = time;
                doublePoint.Value = 0.05 * (rand.NextDouble() - 0.5) + 1.0;

                doublePoints.Add(doublePoint);
                startPhase += phaseStep;
            }

            return doublePoints;
        }

        public static ObservableCollection<DoublePoint> SquirlyWave()
        {
            var doublePoints = new ObservableCollection<DoublePoint>();
            var rand = new Random((int)DateTime.Now.Ticks);

            const int COUNT = 1000;
            for (int i = 0; i < COUNT; i++)
            {
                var doublePoint = new DoublePoint();

                var time = i / (double)COUNT;
                doublePoint.Data = time;
                doublePoint.Value = time * Math.Sin(2 * Math.PI * i / (double)COUNT) +
                             0.2 * Math.Sin(2 * Math.PI * i / (COUNT / 7.9)) +
                             0.05 * (rand.NextDouble() - 0.5) +
                             1.0;

                doublePoints.Add(doublePoint);
            }

            return doublePoints;
        }
    }

    public class Data
    {
        public Data()
        {

        }
        public Data(DateTime date, double value, double value1, double value2)
        {
            Date = date;
            Value = value;
            Value1 = value1;
            Value2 = value2;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public double Value
        {
            get;
            set;
        }
        public double Value1
        {
            get;
            set;
        }
        public double Value2
        {
            get;
            set;
        }
    }   
}
