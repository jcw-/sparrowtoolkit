
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
	public interface IRenderContext
	{
		/// <summary>
		/// Gets a value indicating whether the context renders to screen.
		/// </summary>
		/// <value>
		///   <c>true</c> if the context renders to screen; otherwise, <c>false</c>.
		/// </value>
		bool RendersToScreen { get; }
		
		/// <summary>
		/// Draws an ellipse.
		/// </summary>
		/// <param name="rect">
		/// The rectangle.
		/// </param>
		/// <param name="fill">
		/// The fill color.
		/// </param>
		/// <param name="stroke">
		/// The stroke color.
		/// </param>
		/// <param name="thickness">
		/// The thickness.
		/// </param>
		void DrawEllipse(RectF rect, Color fill, Color stroke, double thickness = 1.0);
		
		/// <summary>
		/// Draws the collection of ellipses, where all have the same stroke and fill.
		/// This performs better than calling DrawEllipse multiple times.
		/// </summary>
		/// <param name="rectangles">
		/// The rectangles.
		/// </param>
		/// <param name="fill">
		/// The fill color.
		/// </param>
		/// <param name="stroke">
		/// The stroke color.
		/// </param>
		/// <param name="thickness">
		/// The stroke thickness.
		/// </param>
		void DrawEllipses(IList<RectF> rectangles, Color fill, Color stroke, double thickness = 1.0);
		
		/// <summary>
		/// Draws the polyline from the specified points.
		/// </summary>
		/// <param name="points">
		/// The points.
		/// </param>
		/// <param name="stroke">
		/// The stroke color.
		/// </param>
		/// <param name="thickness">
		/// The stroke thickness.
		/// </param>
		/// <param name="dashArray">
		/// The dash array.
		/// </param>
		/// <param name="lineJoin">
		/// The line join type.
		/// </param>
		/// <param name="aliased">
		/// if set to <c>true</c> the shape will be aliased.
		/// </param>
		void DrawLine(
			IList<Point> points,
			Color stroke,
			double thickness = 1.0,
			double[] dashArray = null,
			PenLineJoin lineJoin = PenLineJoin.Miter,
			bool aliased = false);
		
		/// <summary>
		/// Draws the multiple line segments defined by points (0,1) (2,3) (4,5) etc.
		/// This should have better performance than calling DrawLine for each segment.
		/// </summary>
		/// <param name="points">
		/// The points.
		/// </param>
		/// <param name="stroke">
		/// The stroke color.
		/// </param>
		/// <param name="thickness">
		/// The stroke thickness.
		/// </param>
		/// <param name="dashArray">
		/// The dash array.
		/// </param>
		/// <param name="lineJoin">
		/// The line join type.
		/// </param>
		/// <param name="aliased">
		/// if set to <c>true</c> the shape will be aliased.
		/// </param>
		void DrawLineSegments(
			IList<Point> points,
			Color stroke,
			double thickness = 1.0,
			double[] dashArray = null,
			PenLineJoin lineJoin = PenLineJoin.Miter,
			bool aliased = false);
		
		/// <summary>
		/// Draws the polygon from the specified points. The polygon can have stroke and/or fill.
		/// </summary>
		/// <param name="points">
		/// The points.
		/// </param>
		/// <param name="fill">
		/// The fill color.
		/// </param>
		/// <param name="stroke">
		/// The stroke color.
		/// </param>
		/// <param name="thickness">
		/// The stroke thickness.
		/// </param>
		/// <param name="dashArray">
		/// The dash array.
		/// </param>
		/// <param name="lineJoin">
		/// The line join type.
		/// </param>
		/// <param name="aliased">
		/// if set to <c>true</c> the shape will be aliased.
		/// </param>
		void DrawPolygon(
			IList<Point> points,
			Color fill,
			Color stroke,
			double thickness = 1.0,
			double[] dashArray = null,
			PenLineJoin lineJoin = PenLineJoin.Miter,
			bool aliased = false);
		
		/// <summary>
		/// Draws a collection of polygons, where all polygons have the same stroke and fill.
		/// This performs better than calling DrawPolygon multiple times.
		/// </summary>
		/// <param name="polygons">
		/// The polygons.
		/// </param>
		/// <param name="fill">
		/// The fill color.
		/// </param>
		/// <param name="stroke">
		/// The stroke color.
		/// </param>
		/// <param name="thickness">
		/// The stroke thickness.
		/// </param>
		/// <param name="dashArray">
		/// The dash array.
		/// </param>
		/// <param name="lineJoin">
		/// The line join type.
		/// </param>
		/// <param name="aliased">
		/// if set to <c>true</c> the shape will be aliased.
		/// </param>
		void DrawPolygons(
			IList<IList<Point>> polygons,
			Color fill,
			Color stroke,
			double thickness = 1.0,
			double[] dashArray = null,
			PenLineJoin lineJoin = PenLineJoin.Miter,
			bool aliased = false);
		
		/// <summary>
		/// Draws the rectangle.
		/// </summary>
		/// <param name="rect">
		/// The rectangle.
		/// </param>
		/// <param name="fill">
		/// The fill color.
		/// </param>
		/// <param name="stroke">
		/// The stroke color.
		/// </param>
		/// <param name="thickness">
		/// The stroke thickness.
		/// </param>
		void DrawRectangle(RectF rect, Color fill, Color stroke, double thickness = 1.0);
		
		/// <summary>
		/// Draws a collection of rectangles, where all have the same stroke and fill.
		/// This performs better than calling DrawRectFangle multiple times.
		/// </summary>
		/// <param name="rectangles">
		/// The rectangles.
		/// </param>
		/// <param name="fill">
		/// The fill color.
		/// </param>
		/// <param name="stroke">
		/// The stroke color.
		/// </param>
		/// <param name="thickness">
		/// The stroke thickness.
		/// </param>
		void DrawRectangles(IList<RectF> rectangles, Color fill, Color stroke, double thickness = 1.0);
		
		/// <summary>
		/// Draws the text.
		/// </summary>
		/// <param name="p">
		/// The position.
		/// </param>
		/// <param name="text">
		/// The text.
		/// </param>
		/// <param name="fill">
		/// The fill color.
		/// </param>
		/// <param name="fontFamily">
		/// The font family.
		/// </param>
		/// <param name="fontSize">
		/// Size of the font.
		/// </param>
		/// <param name="fontWeight">
		/// The font weight.
		/// </param>
		/// <param name="rotate">
		/// The rotation angle.
		/// </param>
		/// <param name="halign">
		/// The horizontal alignment.
		/// </param>
		/// <param name="valign">
		/// The vertical alignment.
		/// </param>
		/// <param name="maxSize">
		/// The maximum size of the text.
		/// </param>
		void DrawText(
			Point p,
			string text,
			Color fill,
			string fontFamily = null,
			double fontSize = 10,
			double fontWeight = 500,
			double rotate = 0,
			HorizontalAlignment halign = HorizontalAlignment.Left,
			VerticalAlignment valign = VerticalAlignment.Top,
			Size? maxSize = null);
		
		/// <summary>
		/// Measures the text.
		/// </summary>
		/// <param name="text">
		/// The text.
		/// </param>
		/// <param name="fontFamily">
		/// The font family.
		/// </param>
		/// <param name="fontSize">
		/// Size of the font.
		/// </param>
		/// <param name="fontWeight">
		/// The font weight.
		/// </param>
		/// <returns>
		/// The text size.
		/// </returns>
		Size MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500);
		
		/// <summary>
		/// Sets the tool tip for the following items.
		/// </summary>
		/// <params>
		/// This is only used in the plot controls.
		/// </params>
		/// <param name="text">
		/// The text in the tooltip.
		/// </param>
		void SetToolTip(string text);
		
		/// <summary>
		/// Cleans up resources not in use.
		/// </summary>
		/// <remarks>
		/// This method is called at the end of each rendering.
		/// </remarks>
		void CleanUp();
		
		/// <summary>
		/// Gets the size of the specified image.
		/// </summary>
		/// <param name="source">The image source.</param>
		/// <returns>The image info.</returns>
		ImageInfo GetImageInfo(Image source);
		
		/// <summary>
		/// Draws the specified portion of the specified <see cref="OxyImage"/> at the specified location and with the specified size.
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
		void DrawImage(Image source, uint srcX, uint srcY, uint srcWidth, uint srcHeight, double destX, double destY, double destWidth, double destHeight, double opacity, bool interpolate);
		
		/// <summary>
		/// Sets the clip rectangle.
		/// </summary>
		/// <param name="rect">The clip rectangle.</param>
		/// <returns>True if the clip rectangle was set.</returns>
		bool SetClip(RectF rect);
		
		/// <summary>
		/// Resets the clip rectangle.
		/// </summary>
		void ResetClip();
	}
}

