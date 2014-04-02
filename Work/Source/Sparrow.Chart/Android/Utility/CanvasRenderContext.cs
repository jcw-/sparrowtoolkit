
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
using Android.Graphics;

namespace Sparrow.Chart
{
	/// <summary>
	/// Provides a render context for Android.Graphics.Canvas.
	/// </summary>
	public class CanvasRenderContext : IRenderContext
	{
		private Canvas canvas;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="CanvasRenderContext"/> class.
		/// </summary>
		public CanvasRenderContext()
		{
		}
		
		/// <summary>
		/// Sets the target.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		public void SetTarget(Canvas canvas)
		{
			this.canvas = canvas;
		}
		
		/// <summary>
		/// Gets a value indicating whether the context renders to screen.
		/// </summary>
		/// <value>
		/// <c>true</c> if the context renders to screen; otherwise, <c>false</c>.
		/// </value>
		public bool RendersToScreen
		{
			get
			{
				return true;
			}
		}
		
		public void DrawEllipse(RectF rect, Color fill, Color stroke, double thickness = 1)
		{
			using (var paint = new Paint())
			{
				paint.AntiAlias = true;
				paint.StrokeWidth = (float)thickness;
				if (fill != null)
				{
					paint.SetStyle(Paint.Style.Fill);
					paint.Color = stroke;
					canvas.DrawOval(rect, paint);
				}
				if (stroke != null)
				{
					paint.SetStyle(Paint.Style.Stroke);
					paint.Color = stroke;
					canvas.DrawOval(rect, paint);
				}
			}
		}
		
		public void DrawEllipses(IList<RectF> rectangles, Color fill, Color stroke, double thickness = 1)
		{
			using (var paint = new Paint())
			{
				paint.AntiAlias = true;
				paint.StrokeWidth = (float)thickness;
				foreach (var rect in rectangles)
				{
					if (fill != null)
					{
						paint.SetStyle(Paint.Style.Fill);
						paint.Color = fill;
						canvas.DrawOval(rect, paint);
					}
					
					if (stroke != null)
					{
						paint.SetStyle(Paint.Style.Stroke);
						paint.Color = stroke;
						canvas.DrawOval(rect, paint);
					}
				}
				
			}
		}
		
		public void DrawLine(IList<Point> points, Color stroke, double thickness = 1, double[] dashArray = null, PenLineJoin lineJoin = PenLineJoin.Miter, bool aliased = false)
		{
			using (var paint = new Paint())
			{
				paint.StrokeWidth = (float)thickness;
				paint.Color = stroke;
				paint.AntiAlias = !aliased;
				var pts = new float[(points.Count - 1) * 4];
				int j = 0;
				for (int i = 0; i + 1 < points.Count; i++)
				{
					pts[j++] = (float)points[i].X;
					pts[j++] = (float)points[i].Y;
					pts[j++] = (float)points[i + 1].X;
					pts[j++] = (float)points[i + 1].Y;
				}
				
				canvas.DrawLines(pts, paint);
			}
		}
		
		public void DrawLineSegments(IList<Point> points, Color stroke, double thickness = 1, double[] dashArray = null, PenLineJoin lineJoin = PenLineJoin.Miter, bool aliased = false)
		{
			using (var paint = new Paint())
			{
				paint.StrokeWidth = (float)thickness;
				paint.Color = stroke;
				paint.AntiAlias = !aliased;
				var pts = new float[points.Count * 2];
				int i = 0;
				foreach (var p in points)
				{
					pts[i++] = (float)p.X;
					pts[i++] = (float)p.Y;
				}
				
				canvas.DrawLines(pts, paint);
			}
		}
		
		public void DrawPolygon(IList<Point> points, Color fill, Color stroke, double thickness = 1, double[] dashArray = null, PenLineJoin lineJoin = PenLineJoin.Miter, bool aliased = false)
		{
			using (var paint = new Paint())
			{
				paint.AntiAlias = !aliased;
				paint.StrokeWidth = (float)thickness;
				using (var path = new Path())
				{
					path.MoveTo((float)points[0].X, (float)points[0].Y);
					for (int i = 1; i <= points.Count; i++)
					{
						path.LineTo((float)points[i % points.Count].X, (float)points[i % points.Count].Y);
					}
					
					if (fill != null)
					{
						paint.SetStyle(Paint.Style.Fill);
						paint.Color = fill;
						canvas.DrawPath(path, paint);
					}
					
					if (stroke != null)
					{
						paint.SetStyle(Paint.Style.Stroke);
						paint.Color = stroke;
						canvas.DrawPath(path, paint);
					}
				}
			}
		}
		
		public void DrawPolygons(IList<IList<Point>> polygons, Color fill, Color stroke, double thickness = 1, double[] dashArray = null, PenLineJoin lineJoin = PenLineJoin.Miter, bool aliased = false)
		{
			foreach (var p in polygons)
				this.DrawPolygon(p, fill, stroke, thickness, dashArray, lineJoin, aliased);
		}
		
		public void DrawRectangle(RectF rect, Color fill, Color stroke, double thickness = 1)
		{
			using (var paint = new Paint())
			{
				paint.AntiAlias = false;
				paint.StrokeWidth = (float)thickness;
				if (fill != null)
				{
					paint.SetStyle(Paint.Style.Fill);
					paint.Color = fill;
					canvas.DrawRect((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom, paint);
				}
				if (stroke != null)
				{
					paint.SetStyle(Paint.Style.Stroke);
					paint.Color = stroke;
					canvas.DrawRect((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom, paint);
				}
			}
		}
		
		public void DrawRectangles(IList<RectF> rectangles, Color fill, Color stroke, double thickness = 1)
		{
			foreach (var r in rectangles)
			{
				this.DrawRectangle(r, fill, stroke, thickness);
			}
		}
		
		public void DrawText(Point p, string text, Color fill, string fontFamily = null, double fontSize = 10, double fontWeight = 500, double rotate = 0, HorizontalAlignment halign = HorizontalAlignment.Left, VerticalAlignment valign = VerticalAlignment.Top, Size? maxSize = new Size?())
		{
			using (var paint = new Paint())
			{
				paint.AntiAlias = true;
				paint.TextSize = (float)fontSize;
				paint.Color = fill;
				var bounds = new Rect();
				paint.GetTextBounds(text, 0, text.Length, bounds);
				
				float dx = 0;
				if (halign == HorizontalAlignment.Center)
				{
					dx = -bounds.Width() / 2;
				}
				
				if (halign == HorizontalAlignment.Right)
				{
					dx = -bounds.Width();
				}
				
				float dy = 0;
				if (valign == VerticalAlignment.Center)
				{
					dy = +bounds.Height() / 2;
				}
				
				if (valign == VerticalAlignment.Top)
				{
					dy = bounds.Height();
				}
				
				canvas.Save();
				canvas.Translate(dx, dy);
				canvas.Rotate((float)rotate);
				canvas.Translate((float)p.X, (float)p.Y);
				canvas.DrawText(text, 0, 0, paint);
				canvas.Restore();
			}
		}
		
		public Size MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			
			using (var paint = new Paint())
			{
				paint.AntiAlias = true;
				paint.TextSize = (float)fontSize;
				var bounds = new Rect();
				paint.GetTextBounds(text, 0, text.Length, bounds);
				// var width = paint.MeasureText(text);
				return new Size(bounds.Width(), bounds.Height());
			}
		}
		
		/// <summary>
		/// Sets the tool tip for the following items.
		/// </summary>
		/// <param name="text">The text in the tooltip.</param>
		/// <params>
		/// This is only used in the plot controls.
		/// </params>
		public void SetToolTip(string text)
		{
			// TODO
		}
		
		/// <summary>
		/// Cleans up resources not in use.
		/// </summary>
		/// <remarks>
		/// This method is called at the end of each rendering.
		/// </remarks>
		public void CleanUp()
		{
			// TODO
		}
		
		/// <summary>
		/// Gets the size of the specified image.
		/// </summary>
		/// <param name="source">The image source.</param>
		/// <returns>
		/// The image info.
		/// </returns>
		public ImageInfo GetImageInfo(Image source)
		{
			// TODO
			return null;
		}
		
		/// <summary>
		/// Draws the specified portion of the specified <see cref="Image" /> at the specified location and with the specified size.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw.</param>
		/// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw.</param>
		/// <param name="srcWidth">Width of the portion of the source image to draw.</param>
		/// <param name="srcHeight">Height of the portion of the source image to draw.</param>
		/// <param name="destX">The x-coordinate of the upper-left corner of drawn image.</param>
		/// <param name="destY">The y-coordinate of the upper-left corner of drawn image.</param>
		/// <param name="destWidth">The width of the drawn image.</param>
		/// <param name="destHeight">The height of the drawn image.</param>
		/// <param name="opacity">The opacity.</param>
		/// <param name="interpolate">interpolate if set to <c>true</c>.</param>
		public void DrawImage(
			Image source,
			uint srcX,
			uint srcY,
			uint srcWidth,
			uint srcHeight,
			double destX,
			double destY,
			double destWidth,
			double destHeight,
			double opacity,
			bool interpolate)
		{
			// TODO
		}
		
		/// <summary>
		/// Sets the clip rectangle.
		/// </summary>
		/// <param name="rect">The clip rectangle.</param>
		/// <returns>
		/// True if the clip rectangle was set.
		/// </returns>
		public bool SetClip(RectF rect)
		{
			// TODO
			return false;
		}
		
		/// <summary>
		/// Resets the clip rectangle.
		/// </summary>
		public void ResetClip()
		{
			// TODO
		}
	}
}

