
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
	/// Provides an abstract base class for elements contained in a <see cref="FrameWorkElement"/>.
	/// </summary>
	public abstract class FrameWorkElement
	{

		private IRenderContext rootContext;
		public IRenderContext RootContext {
			get{return rootContext;}
			set{rootContext=value;}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FrameWorkElement"/> class.
		/// </summary>
		protected FrameWorkElement()
		{
			this.FontFamily = null;
			this.FontSize = double.NaN;
			this.FontWeight = FontWeights.Normal;
			this.Bakground=Color.White;
			this.Foreground=Color.Black;
			this.HorizontalAlignment=HorizontalAlignment.Center;
			this.VerticalAlignment=VerticalAlignment.Center;
			this.Height=0;
			this.Width=0;
			this.Margin=new Thickness(0,0,0,0);
		}
		
		/// <summary>
		/// Gets or sets the fontfamily.
		/// </summary>
		/// <value> The fontfamily for the element. </value>
		/// <remarks>
		/// If the value is null, DefaultFont will be used.
		/// </remarks>
		public string FontFamily { get; set; }
		
		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value> The size of the font. </value>
		/// <remarks>
		/// If the value is NaN, DefaultFontSize will be used.
		/// </remarks>
		public double FontSize { get; set; }
		
		/// <summary>
		/// Gets or sets the font weight.
		/// </summary>
		/// <value> The font weight. </value>
		public double FontWeight { get; set; }
		

		/// <summary>
		/// Gets or sets an arbitrary object value that can be used to store custom information about this plot element.
		/// </summary>
		/// <value> The intended value. This property has no default value. </value>
		/// <remarks>
		/// This property is analogous to Tag properties in other Microsoft programming models. Tag is intended to provide a pre-existing property location where you can store some basic custom information about any PlotElement without requiring you to subclass an element.
		/// </remarks>
		public object Tag { get; set; }
		
		/// <summary>
		/// Gets or sets the forground of the text.
		/// </summary>
		/// <value> The foreground of the text. </value>
		/// <remarks>
		/// If the value is null, the default Foreground will be used.
		/// </remarks>
		public Color Foreground { get; set; }


		public Color Bakground {
			get;
			set;
		}


		public HorizontalAlignment HorizontalAlignment {
			get;
			set;
		}

		public VerticalAlignment VerticalAlignment {
			get;
			set;
		}

		/// <summary>
		/// Gets the actual font.
		/// </summary>
		protected internal string ActualFont
		{
			get
			{
				return this.FontFamily ?? "Segoe UI";
			}
		}
		
		/// <summary>
		/// Gets the actual size of the font.
		/// </summary>
		/// <value> The actual size of the font. </value>
		protected internal double ActualFontSize
		{
			get
			{
				return !double.IsNaN(this.FontSize) ? this.FontSize : 12;
			}
		}
		
		/// <summary>
		/// Gets the actual font weight.
		/// </summary>
		protected internal double ActualFontWeight
		{
			get
			{
				return this.FontWeight;
			}
		}
		
		/// <summary>
		/// Gets the actual color of the text.
		/// </summary>
		/// <value> The actual color of the text. </value>
		protected internal Color ActualForeground
		{
			get
			{
				return this.Foreground;
			}
		}

		/// <summary>
		/// Gets or sets the name of the FrameworkElement.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Margin around the element.
		/// </summary>
		/// <value> The Margin. </value>
		public Thickness Margin { get; set; }
		
		/// <summary>
		/// Gets the total width of the element (in device units).
		/// </summary>
		public double Width { get; private set; }
		
		/// <summary>
		/// Gets the total height of the element (in device units).
		/// </summary>
		public double Height { get; private set; }


		public virtual void Measure(Size avilableSize);

		public virtual void Arrange(Size finalSize);

		public virtual void OnRender(IRenderContext root);
	}
}

