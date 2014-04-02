
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
	/// Provides information about the size of an image.
	/// </summary>
	public class ImageInfo
	{
		/// <summary>
		/// Gets or sets the width in pixels.
		/// </summary>
		/// <value>
		/// The width.
		/// </value>
		public uint Width { get; set; }
		
		/// <summary>
		/// Gets or sets the height in pixels.
		/// </summary>
		/// <value>
		/// The height.
		/// </value>
		public uint Height { get; set; }
		
		/// <summary>
		/// Gets or sets the horizontal resolution in dpi.
		/// </summary>
		/// <value>
		/// The dpi X.
		/// </value>
		public double DpiX { get; set; }
		
		/// <summary>
		/// Gets or sets the vertical resolution in dpi.
		/// </summary>
		/// <value>
		/// The dpi Y.
		/// </value>
		public double DpiY { get; set; }
	}
}

