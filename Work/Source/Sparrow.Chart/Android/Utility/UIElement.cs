
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
	public class UIElement : FrameWorkElement
	{
		/// <summary>
		/// Occurs when a mouse button is pressed down on the model.
		/// </summary>
		public event EventHandler<MouseEventArgs> MouseDown;
		
		/// <summary>
		/// Occurs when the mouse is moved on the plot element (only occurs after MouseDown).
		/// </summary>
		public event EventHandler<MouseEventArgs> MouseMove;
		
		/// <summary>
		/// Occurs when the mouse button is released on the plot element.
		/// </summary>
		public event EventHandler<MouseEventArgs> MouseUp;
		
		/// <summary>
		/// Raises the <see cref="MouseDown"/> event.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="e">
		/// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
		/// </param>
		protected internal virtual void OnMouseDown(object sender, MouseEventArgs e)
		{
			if (this.MouseDown != null)
			{
				this.MouseDown(sender, e);
			}
		}
		
		/// <summary>
		/// Raises the <see cref="MouseMove"/> event.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="e">
		/// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
		/// </param>
		protected internal virtual void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (this.MouseMove != null)
			{
				this.MouseMove(sender, e);
			}
		}
		
		/// <summary>
		/// Raises the <see cref="MouseUp"/> event.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="e">
		/// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
		/// </param>
		protected internal virtual void OnMouseUp(object sender, MouseEventArgs e)
		{
			if (this.MouseUp != null)
			{
				this.MouseUp(sender, e);
			}
		}
	}
}

