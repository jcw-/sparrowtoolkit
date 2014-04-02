
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Sparrow.Chart
{
	public class SparrowChartView : View
	{
		private SparrowChart sparrowChart;
		
		public SparrowChart SparrowChart
		{
			get
			{
				return this.sparrowChart;
			}
			set
			{
				if (this.sparrowChart == value)
				{
					return;
				}
				
				this.sparrowChart = value;
				this.OnSparrowChartChanged();
			}
		}
		
		private void OnSparrowChartChanged()
		{
			this.InvalidateSparrowChart(true);
		}

		public void InvalidateSparrowChart(bool updateData)
		{
			lock (this.invalidateLock)
			{
				this.isSparrowChartInvalidated = true;
				this.updateDataFlag = this.updateDataFlag || updateData;
			}			
			this.Invalidate();
		}
		
		private readonly object invalidateLock = new object();
		private bool isSparrowChartInvalidated;
		private bool updateDataFlag = true;
		private CanvasRenderContext rootCanvas;

		public SparrowChartView (Context context) :
			base (context)
		{
			Initialize ();
		}

		public SparrowChartView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public SparrowChartView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			rootCanvas=new CanvasRenderContext();
		}

		protected override void OnDraw (Canvas canvas)
		{
			base.OnDraw(canvas);
			canvas.DrawColor(Color.White);
			this.rootCanvas.SetTarget(canvas);
			using (var bounds = new Rect())
			{
				canvas.GetClipBounds(bounds);
				rootCanvas.DrawRectangle(new RectF(bounds.Left, bounds.Top, bounds.Width(), bounds.Height()), Color.Red, Color.Transparent, 0);											
			}
		}
	}
}

