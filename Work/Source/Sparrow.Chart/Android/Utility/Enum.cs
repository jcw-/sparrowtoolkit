
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
	/// Specifies how to join line segments.
	/// </summary>
	public enum PenLineJoin
	{
		/// <summary>
		/// Line joins use regular angular vertices.
		/// </summary>
		Miter,
		
		/// <summary>
		/// Line joins use rounded vertices.
		/// </summary>
		Round,
		
		/// <summary>
		/// Line joins use beveled vertices.
		/// </summary>
		Bevel
	}

	/// <summary>
	/// Specifies the horizontal alignment.
	/// </summary>
	public enum HorizontalAlignment
	{
		/// <summary>
		/// Aligned to the left.
		/// </summary>
		Left = -1,
		
		/// <summary>
		/// Aligned in the center.
		/// </summary>
		Center = 0,
		
		/// <summary>
		/// Aligned to the right.
		/// </summary>
		Right = 1
	}

	/// <summary>
	/// Specifies the vertical alignment.
	/// </summary>
	public enum VerticalAlignment
	{
		/// <summary>
		/// Aligned at the top.
		/// </summary>
		Top = -1,
		
		/// <summary>
		/// Aligned in the middle.
		/// </summary>
		Center = 0,
		
		/// <summary>
		/// Aligned at the bottom.
		/// </summary>
		Bottom = 1
	}

	/// <summary>
	/// Provides a set of static predefined font weight values.
	/// </summary>
	public static class FontWeights
	{
		/// <summary>
		/// Specifies a bold font weight.
		/// </summary>
		public const double Bold = 700;
		
		/// <summary>
		/// Specifies a normal font weight.
		/// </summary>
		public const double Normal = 400;
		
	}

	/// <summary>
	/// Set Theme for SparrowChart
	/// </summary>
	public enum Theme
	{
		Arctic,
		Autmn,
		Cold,
		Flower,
		Forest,
		Grayscale,
		Ground,
		Lialac,
		Natural,
		Pastel,
		Rainbow,
		Spring,
		Summer,
		Warm,
		Metro,                
		Custom
	}
	
	/// <summary>
	/// Set ValueType of XAxis
	/// </summary>
	public enum XType
	{
		Double, // For use Double type Axis
		DateTime, // For use DateTime type Axis
		Category // For use Category type axis
		
	}
	
	internal enum ActualType
	{
		Double, // For use Double type Axis
		DateTime, // For use DateTime type Axis
		Category // For use Category type axis
	}
	
	/// <summary>
	/// Set ValueType of YAxis
	/// </summary>
	public enum YType
	{
		Double, // For use Double type Axis
		DateTime, // For use DateTime type Axis
	}

	// Summary:
	//     Specifies values that control the behavior of a control positioned inside
	//     another control.
	public enum Dock
	{
		// Summary:
		//     Specifies that the control should be positioned on the left of the control.
		Left = 0,
		//
		// Summary:
		//     Specifies that the control should be positioned on top of the control.
		Top = 1,
		//
		// Summary:
		//     Specifies that the control should be positioned on the right of the control.
		Right = 2,
		//
		// Summary:
		//     Specifies that the control should be positioned at the bottom of the control.
		Bottom = 3,
	}

	/// <summary>
	/// Specifies the style of a line.
	/// </summary>
	public enum LineStyle
	{
		/// <summary>
		/// The solid line style.
		/// </summary>
		Solid,
		
		/// <summary>
		/// The dash line style.
		/// </summary>
		Dash,
		
		/// <summary>
		/// The dot line style.
		/// </summary>
		Dot,
		
		/// <summary>
		/// The dash dot line style.
		/// </summary>
		DashDot,
		
		/// <summary>
		/// The dash dash dot line style.
		/// </summary>
		DashDashDot,
		
		/// <summary>
		/// The dash dot dot line style.
		/// </summary>
		DashDotDot,
		
		/// <summary>
		/// The dash dash dot dot line style.
		/// </summary>
		DashDashDotDot,
		
		/// <summary>
		/// The long dash line style.
		/// </summary>
		LongDash,
		
		/// <summary>
		/// The long dash dot line style.
		/// </summary>
		LongDashDot,
		
		/// <summary>
		/// The long dash dot dot line style.
		/// </summary>
		LongDashDotDot,
		
		/// <summary>
		/// The hidden line style.
		/// </summary>
		None,
		
		/// <summary>
		/// The undefined line style.
		/// </summary>
		Undefined
	}
}

