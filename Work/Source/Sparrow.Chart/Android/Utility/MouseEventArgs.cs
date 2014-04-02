
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
	/// Provides data for the mouse events.
	/// </summary>
	public class MouseEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the mouse button that has changed.
		/// </summary>
		//public MouseButton ChangedButton { get; set; }
		
		/// <summary>
		/// Gets or sets the click count.
		/// </summary>
		/// <value> The click count. </value>
		public int ClickCount { get; set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether Handled.
		/// </summary>
		public bool Handled { get; set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether the alt key was pressed when the event was raised.
		/// </summary>
		public bool IsAltDown { get; set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether the control key was pressed when the event was raised.
		/// </summary>
		public bool IsControlDown { get; set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether the shift key was pressed when the event was raised.
		/// </summary>
		public bool IsShiftDown { get; set; }
	
		
		/// <summary>
		/// Gets or sets the control.
		/// </summary>
		/// <value> The control. </value>
		//public Control Control { get; set; }
		
		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Point Position { get; set; }
		
	}
}

