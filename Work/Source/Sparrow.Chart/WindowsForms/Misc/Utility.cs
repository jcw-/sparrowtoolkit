using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace Sparrow.Chart
{       
    /// <summary>
    /// BezierSpline
    /// </summary>
    public static class BezierSpline
    {
        /// <summary>
        /// Get open-ended Bezier Spline Control Points.
        /// </summary>
        /// <param name="knots">Input Knot Bezier spline points.</param>
        /// <param name="firstControlPoints">Output First Control points array of knots.Length - 1 length.</param>
        /// <param name="secondControlPoints">Output Second Control points array of knots.Length - 1 length.</param>
        /// <exception cref="ArgumentNullException"><paramref name="knots"/> parameter must be not null.</exception>
        /// <exception cref="ArgumentException"><paramref name="knots"/> array must containg at least two points.</exception>
        public static void GetCurveControlPoints(Point[] knots, out Point[] firstControlPoints, out Point[] secondControlPoints)
        {
            if (knots == null)
                throw new ArgumentNullException("knots");
            int n = knots.Length - 1;
            if (n < 1)
                throw new ArgumentException("At least two knot points required", "knots");
            if (n == 1)
            { // Special case: Bezier curve should be a straight line.
                firstControlPoints = new Point[1];
                // 3P1 = 2P0 + P3
                firstControlPoints[0].X = (2 * knots[0].X + knots[1].X) / 3;
                firstControlPoints[0].Y = (2 * knots[0].Y + knots[1].Y) / 3;

                secondControlPoints = new Point[1];
                // P2 = 2P1 – P0
                secondControlPoints[0].X = 2 * firstControlPoints[0].X - knots[0].X;
                secondControlPoints[0].Y = 2 * firstControlPoints[0].Y - knots[0].Y;
                return;
            }

            // Calculate first Bezier control points
            // Right hand side vector
            double[] rhs = new double[n];

            // Set right hand side X values
            for (int i = 1; i < n - 1; ++i)
                rhs[i] = 4 * knots[i].X + 2 * knots[i + 1].X;
            rhs[0] = knots[0].X + 2 * knots[1].X;
            rhs[n - 1] = (8 * knots[n - 1].X + knots[n].X) / 2.0;
            // Get first control points X-values
            double[] x = GetFirstControlPoints(rhs);

            // Set right hand side Y values
            for (int i = 1; i < n - 1; ++i)
                rhs[i] = 4 * knots[i].Y + 2 * knots[i + 1].Y;
            rhs[0] = knots[0].Y + 2 * knots[1].Y;
            rhs[n - 1] = (8 * knots[n - 1].Y + knots[n].Y) / 2.0;
            // Get first control points Y-values
            double[] y = GetFirstControlPoints(rhs);

            // Fill output arrays.
            firstControlPoints = new Point[n];
            secondControlPoints = new Point[n];
            for (int i = 0; i < n; ++i)
            {
                // First control point
                firstControlPoints[i] = new Point(x[i], y[i]);
                // Second control point
                if (i < n - 1)
                    secondControlPoints[i] = new Point(2 * knots[i + 1].X - x[i + 1], 2 * knots[i + 1].Y - y[i + 1]);
                else
                    secondControlPoints[i] = new Point((knots[n].X + x[n - 1]) / 2, (knots[n].Y + y[n - 1]) / 2);
            }
        }

        /// <summary>
        /// Solves a tridiagonal system for one of coordinates (x or y) of first Bezier control points.
        /// </summary>
        /// <param name="rhs">Right hand side vector.</param>
        /// <returns>Solution vector.</returns>
        private static double[] GetFirstControlPoints(double[] rhs)
        {
            int n = rhs.Length;
            double[] x = new double[n]; // Solution vector.
            double[] tmp = new double[n]; // Temp workspace.

            double b = 2.0;
            x[0] = rhs[0] / b;
            for (int i = 1; i < n; i++) // Decomposition and forward substitution.
            {
                tmp[i] = 1 / b;
                b = (i < n - 1 ? 4.0 : 3.5) - tmp[i];
                x[i] = (rhs[i] - x[i - 1]) / b;
            }
            for (int i = 1; i < n; i++)
                x[n - i - 1] -= tmp[n - i] * x[n - i]; // Backsubstitution.

            return x;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Themes
    {

        /// <summary>
        /// Metroes the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> MetroBrushes()
        {
            List<Brush> metroBrushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                metroBrushes.Add(new SolidBrush(Color.Black));
            }
            metroBrushes[0] = new SolidBrush(Color.FromArgb(0xFF,0x8E, 0xC4, 0x41));
            metroBrushes[4] = new SolidBrush(Color.FromArgb(0xFF,0x1B, 0x9D, 0xDE));
            metroBrushes[2] = new SolidBrush(Color.FromArgb(0xFF,0xF5, 0x97, 0x00));
            metroBrushes[1] = new SolidBrush(Color.FromArgb(0xFF,0xD4, 0xDF, 0x32));
            metroBrushes[3] = new SolidBrush(Color.FromArgb(0xFF,0x33, 0x99, 0x33)); 
            metroBrushes[5] = new SolidBrush(Color.FromArgb(0xFF,0x00, 0xAB, 0xA9));
            metroBrushes[6] = new SolidBrush(Color.FromArgb(0xFF,0xDC, 0x5B, 0x20));
            metroBrushes[7] = new SolidBrush(Color.FromArgb(0xFF,0xE8, 0xBC, 0x34));            
            return metroBrushes;
        }

        /// <summary>
        /// Arctics the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> ArcticBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0x8F, 0xCA, 0xEE));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0x4C, 0xA9, 0xD7));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0x1E, 0x7B, 0xA9));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0xBD, 0xC1, 0xC5));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0x8C, 0x8C, 0x8C));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0xCF, 0x1C, 0x1F));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0xF0, 0x48, 0x4B));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xFF, 0x86, 0x7F));
            return brushes;
        }

        /// <summary>
        /// Autmns the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> AutmnBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0x97, 0xA0, 0x35));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0xBF, 0xAD, 0x61));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0xE2, 0xCB, 0x70));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0xEC, 0xB5, 0x52));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0xDD, 0x7F, 0x33));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0xD1, 0x5B, 0x27));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0xB5, 0x48, 0x24));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0x7D, 0x3B, 0x25));
            return brushes;
        }

        /// <summary>
        /// Colds the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> ColdBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0x7D, 0x3B, 0x25));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0x4B, 0xAE, 0xDB));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0x28, 0x8D, 0xBA));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0x7C, 0xCC, 0xD6));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0x4B, 0xAD, 0xB9));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0x9C, 0xCF, 0xE5));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0x63, 0xB2, 0xCF));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0x45, 0x93, 0xAC));
            return brushes;
        }

        /// <summary>
        /// Flowers the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> FlowerBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0xDD, 0x78, 0x9B));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0xAA, 0xC2, 0x71));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0x9F, 0x8E, 0x7C));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0xE0, 0xD2, 0x6D));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0xE4, 0x94, 0xBB));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0x85, 0xB3, 0x79));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0xAA, 0xA1, 0xA1));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xDE, 0xB3, 0xB7));
            return brushes;
        }

        /// <summary>
        /// Forests the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> ForestBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0xAD, 0xB8, 0x27));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0xF1, 0xA5, 0x4C));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0xA6, 0x69, 0x40));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0xDB, 0x63, 0x40));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0xDC, 0xA0, 0x63));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0x8B, 0xA0, 0x41));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0xC0, 0x93, 0x66));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xF2, 0xBE, 0x4B));
            return brushes;
        }

        /// <summary>
        /// Grayscales the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> GrayscaleBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0x40, 0x40, 0x40));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0x80, 0x80, 0x80));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0xB3, 0xB3, 0xB3));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0x59, 0x59, 0x59));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0x99, 0x99, 0x99));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0xC0, 0xC0, 0xC0));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0x66, 0x66, 0x66));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xA6, 0xA6, 0xA6));
            return brushes;
        }

        /// <summary>
        /// Grounds the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> GroundBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0xFF, 0xB3, 0x8C));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0xF2, 0x87, 0x5A));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0xCC, 0x43, 0x43));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0x6C, 0x52, 0x4E));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0x8A, 0x71, 0x61));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0xB0, 0x91, 0x76));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0xDA, 0xB8, 0x9B));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xB6, 0x58, 0x45));
            return brushes;
        }

        /// <summary>
        /// Lialacs the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> LialacBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0x88, 0xC9, 0x7F));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0x80, 0xD5, 0xCF));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0xC3, 0xAA, 0xEF));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0x6E, 0x8F, 0xD4));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0xA4, 0xD3, 0x86));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0xA4, 0xB5, 0xEF));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0x61, 0xBB, 0xD1));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xA0, 0xA0, 0xD2));
            return brushes;
        }

        /// <summary>
        /// Naturals the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> NaturalBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0xEF, 0x75, 0x13));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0xF0, 0xA1, 0x3F));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0xEF, 0xD1, 0x59));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0xC1, 0xCA, 0x5F));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0x97, 0xA0, 0x31));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0xAD, 0xA1, 0x7E));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0x8B, 0x47, 0x3A));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xCE, 0x42, 0x26));
            return brushes;
        }

        /// <summary>
        /// Pastels the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> PastelBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0xC9, 0xA2, 0xC8));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0x98, 0xBE, 0xD9));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0x9C, 0xD0, 0x84));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0x71, 0x9B, 0xAE));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0x80, 0xC4, 0xB2));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0x94, 0xA0, 0xBC));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0xA0, 0xCB, 0xD3));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xD9, 0xCA, 0xD1));
            return brushes;
        }

        /// <summary>
        /// Rainbows the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> RainbowBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0x11, 0x71, 0xCA));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0x32, 0x94, 0xDA));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0x45, 0xB5, 0xB2));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0x8C, 0xC2, 0x21));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0xAF, 0xDA, 0x3D));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0xE3, 0xDA, 0x20));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0xF5, 0x8E, 0x13));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xE0, 0x3D, 0x0B));
            return brushes;
        }

        /// <summary>
        /// Springs the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> SpringBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0xBE, 0xD7, 0x81));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0xF8, 0xA6, 0xC4));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0x84, 0xC3, 0xBA));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0xC0, 0xB2, 0xB2));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0x9D, 0xCE, 0x67));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0xF4, 0x8E, 0xAD));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0xE0, 0xCC, 0xCA));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xA0, 0xCB, 0xDA));
            return brushes;
        }

        /// <summary>
        /// Summers the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> SummerBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0x3F, 0xA7, 0xC4));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0x9E, 0xD5, 0x52));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0xE1, 0xDB, 0x63));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0xC4, 0x9F, 0xD9));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0xC5, 0xDF, 0x5E));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0x55, 0xC0, 0xCD));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0x86, 0xD8, 0xA6));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xAD, 0xAD, 0xD7));
            return brushes;
        }

        /// <summary>
        /// Warms the brushes.
        /// </summary>
        /// <returns></returns>
        public static List<Brush> WarmBrushes()
        {
            List<Brush> brushes = new List<Brush>();
            for (int i = 0; i < 8; i++)
            {
                brushes.Add(new SolidBrush(Color.Black));
            }
            brushes[0] = new SolidBrush(Color.FromArgb(0xFF, 0xF3, 0xD2, 0x4F));
            brushes[4] = new SolidBrush(Color.FromArgb(0xFF, 0xFA, 0xC2, 0x4D));
            brushes[2] = new SolidBrush(Color.FromArgb(0xFF, 0xF9, 0xAC, 0x3B));
            brushes[1] = new SolidBrush(Color.FromArgb(0xFF, 0xF5, 0x8E, 0x13));
            brushes[3] = new SolidBrush(Color.FromArgb(0xFF, 0xFA, 0x68, 0x00));
            brushes[5] = new SolidBrush(Color.FromArgb(0xFF, 0xF0, 0x46, 0x00));
            brushes[6] = new SolidBrush(Color.FromArgb(0xFF, 0xDE, 0x2E, 0x05));
            brushes[7] = new SolidBrush(Color.FromArgb(0xFF, 0xCB, 0x0C, 0x0C));
            return brushes;
        }
    } 
}
