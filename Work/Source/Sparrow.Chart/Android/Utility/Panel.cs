
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
	/// Panel for arrange the child elements
	/// </summary>
	public abstract class Panel : UIElement
	{
		private UIElementCollecion children;
		public UIElementCollecion Children
		{
			get
			{
				return children;
			}
			set
			{
				if(children.Count>0)

			}
		}
	}
}

