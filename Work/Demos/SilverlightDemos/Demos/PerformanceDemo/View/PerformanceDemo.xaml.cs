using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sparrow.Chart.Demos.Demos.PerformanceDemo
{
    public partial class PerformanceDemo : UserControl
    {
        private Stopwatch stopWatch;
        private PointsCollection Collection;
        private DataGenerator _viewModel;

        public PerformanceDemo()
        {
            stopWatch = new Stopwatch();
            InitializeComponent();
            Chart.LayoutUpdated += Chart_LayoutUpdated;
        }

        void Chart_LayoutUpdated(object sender, EventArgs e)
        {
            if (stopWatch != null)
            {
                stopWatch.Stop();
                //text.Text = stopWatch.ElapsedMilliseconds.ToString() + " ms";
            }
        }

        private void SparrowButton_Click_1(object sender, RoutedEventArgs e)
        {           
            _viewModel = new DataGenerator();
            Collection = _viewModel.Generate();            
            stopWatch.Start();
            ((LineSeries)(Chart.Series[0])).Points = Collection;                               
        }       
    }
}
