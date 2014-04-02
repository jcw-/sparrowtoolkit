
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
	public class SparrowChart : UIElement
	{
		private static List<string> actualCategoryValues;
		internal static List<string> ActualCategoryValues
		{
			get { return actualCategoryValues; }
			set { actualCategoryValues = value; }
		}

	}
}

