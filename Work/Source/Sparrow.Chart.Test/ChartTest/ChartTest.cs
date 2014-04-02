using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Threading;

namespace Sparrow.Chart.Test
{
    [TestClass]
    public class ChartTest
    {
        [TestMethod]
        public void TestDefaultRenderingMode()
        {            
            Sparrow.Chart.SparrowChart Chart = new Sparrow.Chart.SparrowChart();
            Chart.XAxis = new Sparrow.Chart.LinearXAxis();
            Chart.YAxis = new Sparrow.Chart.LinearYAxis();
            Chart.Series.Add(new Sparrow.Chart.LineSeries());
            Assert.AreEqual(Sparrow.Chart.RenderingMode.Default, Chart.RenderingMode);
        }

        [TestMethod]
        public void TestDefaultAxisHeight()
        {
            Sparrow.Chart.SparrowChart Chart = new Sparrow.Chart.SparrowChart();
            Chart.XAxis = new Sparrow.Chart.LinearXAxis();
            Chart.YAxis = new Sparrow.Chart.LinearYAxis();
            Chart.Series.Add(new Sparrow.Chart.LineSeries());   
            Assert.AreEqual(30d, Chart.AxisHeight);
        }

        [TestMethod]
        public void TestDefaultAxisWidth()
        {
            Sparrow.Chart.SparrowChart Chart = new Sparrow.Chart.SparrowChart();
            Chart.XAxis = new Sparrow.Chart.LinearXAxis();
            Chart.YAxis = new Sparrow.Chart.LinearYAxis();
            Chart.Series.Add(new Sparrow.Chart.LineSeries());
            Assert.AreEqual(30d, Chart.AxisWidth);
        }

        [TestMethod]
        public void TestDefaultSmoothingMode()
        {
            Sparrow.Chart.SparrowChart Chart = new Sparrow.Chart.SparrowChart();
            Chart.XAxis = new Sparrow.Chart.LinearXAxis();
            Chart.YAxis = new Sparrow.Chart.LinearYAxis();
            Chart.Series.Add(new Sparrow.Chart.LineSeries());
            Assert.AreEqual(Sparrow.Chart.SmoothingMode.HighQuality, Chart.SmoothingMode);
        }
    }
}
