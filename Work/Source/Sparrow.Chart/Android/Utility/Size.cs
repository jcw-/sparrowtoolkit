
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
using System.Globalization;

namespace Sparrow.Chart
{
	/// <summary>
	/// Implements a structure that is used to describe the size of an object.
	/// </summary>
	public struct Size
	{
		/// <summary>
		/// Empty Size.
		/// </summary>
		public static Size Empty = new Size(0, 0);
		
		/// <summary>
		/// Initializes a new instance of the <see cref="OxySize"/> struct.
		/// </summary>
		/// <param name="width">
		/// The width.
		/// </param>
		/// <param name="height">
		/// The height.
		/// </param>
		public Size(double width, double height)
		: this()
		{
			this.Width = width;
			this.Height = height;
		}
		
		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		/// <value>
		/// The height.
		/// </value>
		public double Height { get; set; }
		
		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		/// <value>
		/// The width.
		/// </value>
		public double Width { get; set; }
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "({0}, {1})", this.Width, this.Height);
		}
	}
}

