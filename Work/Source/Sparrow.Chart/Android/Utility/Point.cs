
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Sparrow.Chart
{
	/// <summary>
	/// Represents a point defined in the screen coordinate system.
	/// </summary>
	/// <remarks>
	/// The rendering methods transforms <see cref="DataPoint"/>s to <see cref="ScreenPoint"/>s.
	/// </remarks>
	public struct Point
	{
		/// <summary>
		/// The undefined point.
		/// </summary>
		public static readonly Point Undefined = new Point(double.NaN, double.NaN);
		
		/// <summary>
		/// The x-coordinate.
		/// </summary>
		internal double x;
		
		/// <summary>
		/// The y-coordinate.
		/// </summary>
		internal double y;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ScreenPoint"/> struct.
		/// </summary>
		/// <param name="x">
		/// The x-coordinate.
		/// </param>
		/// <param name="y">
		/// The y-coordinate.
		/// </param>
		public Point(double x, double y)
		{
			this.x = x;
			this.y = y;
		}
		
		/// <summary>
		/// Gets or sets the x-coordinate.
		/// </summary>
		/// <value> The x-coordinate. </value>
		public double X
		{
			get
			{
				return this.x;
			}
			
			set
			{
				this.x = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the y-coordinate.
		/// </summary>
		/// <value> The y-coordinate. </value>
		public double Y
		{
			get
			{
				return this.y;
			}
			
			set
			{
				this.y = value;
			}
		}
		
		/// <summary>
		/// Determines whether the specified point is undefined.
		/// </summary>
		/// <param name="point">
		/// The point.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified point is undefined; otherwise, <c>false</c> .
		/// </returns>
		public static bool IsUndefined(Point point)
		{
			return double.IsNaN(point.X) && double.IsNaN(point.Y);
		}
		
		/// <summary>
		/// Gets the distance to the specified point.
		/// </summary>
		/// <param name="point">
		/// The point.
		/// </param>
		/// <returns>
		/// The distance.
		/// </returns>
		public double DistanceTo(Point point)
		{
			double dx = point.x - this.x;
			double dy = point.y - this.y;
			return Math.Sqrt((dx * dx) + (dy * dy));
		}
		
		/// <summary>
		/// Gets the squared distance to the specified point.
		/// </summary>
		/// <param name="point">
		/// The point.
		/// </param>
		/// <returns>
		/// The squared distance.
		/// </returns>
		public double DistanceToSquared(Point point)
		{
			double dx = point.x - this.x;
			double dy = point.y - this.y;
			return (dx * dx) + (dy * dy);
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.x + " " + this.y;
		}
		
		/// <summary>
		/// Translates a <see cref="ScreenPoint"/> by a <see cref="Vector"/>.
		/// </summary>
		/// <param name="p1"> The point. </param>
		/// <param name="p2"> The vector. </param>
		/// <returns> The translated point. </returns>
		public static Point operator +(Point p1, Vector p2)
		{
			return new Point(p1.x + p2.x, p1.y + p2.y);
		}
		
		/// <summary>
		/// Subtracts a <see cref="ScreenPoint"/> from a <see cref="ScreenPoint"/>
		/// and returns the result as a <see cref="Vector"/>.
		/// </summary>
		/// <param name="p1"> The point on which to perform the subtraction. </param>
		/// <param name="p2"> The point to subtract from p1. </param>
		/// <returns> A <see cref="Vector"/> structure that represents the difference between p1 and p2. </returns>
		public static Vector operator -(Point p1, Point p2)
		{
			return new Vector(p1.x - p2.x, p1.y - p2.y);
		}
		
		/// <summary>
		/// Subtracts a <see cref="Vector"/> from a <see cref="ScreenPoint"/> 
		/// and returns the result as a <see cref="ScreenPoint"/>.
		/// </summary>
		/// <param name="point"> The point on which to perform the subtraction. </param>
		/// <param name="vector"> The vector to subtract from p1. </param>
		/// <returns> A <see cref="ScreenPoint"/> that represents point translated by the negative vector. </returns>
		public static Point operator -(Point point, Vector vector)
		{
			return new Point(point.x - vector.x, point.y - vector.y);
		}
	}
}

