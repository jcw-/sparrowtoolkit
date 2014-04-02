
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
	/// Represents a vector defined in the screen coordinate system.
	/// </summary>
	public struct Vector
	{
		/// <summary>
		/// The x-coordinate.
		/// </summary>
		internal double x;
		
		/// <summary>
		/// The y-coordinate.
		/// </summary>
		internal double y;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ScreenVector"/> structure.
		/// </summary>
		/// <param name="x">
		/// The x-coordinate.
		/// </param>
		/// <param name="y">
		/// The y-coordinate.
		/// </param>
		public Vector(double x, double y)
		{
			this.x = x;
			this.y = y;
		}
		
		/// <summary>
		/// Gets the length.
		/// </summary>
		public double Length
		{
			get
			{
				return Math.Sqrt((this.x * this.x) + (this.y * this.y));
			}
		}
		
		/// <summary>
		/// Gets the length squared.
		/// </summary>
		public double LengthSquared
		{
			get
			{
				return (this.x * this.x) + (this.y * this.y);
			}
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
		/// Normalizes this vector.
		/// </summary>
		public void Normalize()
		{
			double l = Math.Sqrt((this.x * this.x) + (this.y * this.y));
			if (l > 0)
			{
				this.x /= l;
				this.y /= l;
			}
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
		/// Implements the operator *.
		/// </summary>
		/// <param name="v"> The vector. </param>
		/// <param name="d"> The multiplication factor. </param>
		/// <returns> The result of the operator. </returns>
		public static Vector operator *(Vector v, double d)
		{
			return new Vector(v.x * d, v.y * d);
		}
	}
}

