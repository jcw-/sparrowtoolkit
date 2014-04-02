using System;

namespace Sparrow.Chart
{
    /// <summary>
    /// Axis Utility for Sparrow Chart
    /// </summary>
    public static class AxisUtil
    {
        /// <summary>
        /// Calculate Nice Interval From
        /// http://stackoverflow.com/questions/361681/algorithm-for-nice-grid-line-intervals-on-a-graph
        /// </summary>
        /// <param name="min">MinValue</param>
        /// <param name="max">MaxValue</param>
        /// <param name="intervalCount">IntervalCount</param>
        /// <returns>Interval</returns>
        public static double CalculateInetrval(double min,double max, double intervalCount)
        {
            double range = max - min;
            // calculate an initial guess at step size
            double tempInterval = range / intervalCount;

            // get the magnitude of the step size
            double mag = Math.Floor(Math.Log10(tempInterval));
            double magPow = Math.Pow(10, mag);

            // calculate most significant digit of the new step size
            double magMsd = (int)(tempInterval / magPow + 0.5);

            // promote the MSD to either 1, 2, or 5
            if (magMsd > 5.0)
            { }
            else if (magMsd > 3.0)
                magMsd = 3.0f;
            else if (magMsd > 2.0)
                magMsd = 5.0f;
            else if (magMsd > 1.0)
                magMsd = 2.0f;


            return magMsd * magPow;
        }
    }
}
