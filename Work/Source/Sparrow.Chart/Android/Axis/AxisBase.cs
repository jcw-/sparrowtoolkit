
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
	/// AxisBase
	/// </summary>
	public class AxisBase : Canvas, IAxis
	{

		internal virtual void InvalidateVisuals()
		{
		}
		
		protected Line axisLine;
		protected List<ContentControl> labels;
		protected List<Line> majorTickLines;
		protected List<Line> minorTickLines;
		protected ContentControl header;
		protected bool isInitialized;
		private bool isIntervalCountZero;
		
		private ActualType actualType;
		internal ActualType ActualType 
		{
			get { return actualType; }
			set { actualType = value; }
		}
		
		public AxisBase()
		{
			this.m_Labels = new List<string>();
			this.m_labelValues = new List<double>();
			this.ActualType = ActualType.Double;
			GetStyles();
			this.StringFormat=string.Empty;
		}
		
		protected virtual void GetStyles()
		{					
			this.AxisLineStyle = LineStyle.Solid;
			this.MajorLineStyle = LineStyle.Solid;
			this.MinorLineStyle = LineStyle.Solid;
			//this.LabelTemplate = (DataTemplate)styles["axisLabelTemplate"];
			this.CrossLineStyle = LineStyle.Solid;
			this.MinorCrossLineStyle = LineStyle.Solid;
			this.ZoomOffset=0d;
			this.ZoomCoefficient=1d;
		}
		
		private double zoomOffset;
		public double ZoomOffset
		{
			get { return zoomOffset; }
			set 
			{
				zoomOffset= value;
			}
		}

		private double zoomCoefficient;
		public double ZoomCoefficient
		{
			get { return zoomCoefficient; }
			set
			{ 
				if(zoomCoefficient!=value)
				{				
					zoomCoefficient= value; 
					ZoomPropertyChanged();
				}
			}
		}

		internal void ZoomPropertyChanged()
		{           
			Refresh();
		}


		private object minValue;
		/// <summary>
		/// Axis Minimum Value
		/// </summary>
		public object MinValue
		{
			get { return minValue; }
			set 
			{ 
				if(minValue !=value)
				{
					minValue= value;
					MinValueChnaged();
				}
			}
		}		

		internal void MinValueChnaged()
		{
			this.m_MinValue =Double.Parse(this.MinValue.ToString());
			this.m_startValue = Double.Parse(this.MinValue.ToString()); 
			if (this.Chart != null && this.Chart.containers != null)
				Chart.containers.Refresh();
			RefreshSeries();
		}

		internal void MaxValueChnaged()
		{
			this.m_MaxValue = Double.Parse(this.MaxValue.ToString());
			if (this.Chart != null && this.Chart.containers != null)
				Chart.containers.Refresh();
			RefreshSeries();
		}

		internal void IntervalChanged()
		{
			switch (ActualType)
			{                 
			case ActualType.Double:
			case ActualType.Category:                    
				if (!this.Interval.ToString().Contains(":"))
					this.m_Interval = Double.Parse(this.Interval.ToString());
				else
					this.m_Interval = (DateTime.Now + (TimeSpan)TimeSpan.Parse(Interval.ToString())).ToOADate() - DateTime.Now.ToOADate();
				break;
			case ActualType.DateTime:
				this.m_Interval = (DateTime.Now + (TimeSpan)TimeSpan.Parse(Interval.ToString())).ToOADate() - DateTime.Now.ToOADate();
				break;
			default:
				break;
			}
			
			if (this.Chart != null && this.Chart.containers != null)
			{
				Chart.containers.Refresh();
				Chart.containers.axisLinesconatiner.Refresh();
			}
			RefreshSeries();
		}


		private object maxValue;
		/// <summary>
		/// Maximum Value for Axis
		/// </summary>
		public object MaxValue
		{
			get { return maxValue; }
			set
			{
				if(maxValue != value)
				{
					maxValue= value;
					MaxValueChnaged();
				}
			}
		}


		private object interval;
		/// <summary>
		/// Interval for Axis
		/// </summary>
		public object Interval
		{
			get { return interval; }
			set
			{ 
				if(interval!=value)
				{
					interval= value;
					IntervalChanged();
				}
			}
		}
			
		
		/// <summary>
		/// Add Minimum Value to Axis
		/// </summary>
		/// <param name="min">Minimum Value To be added to Axis</param>
		/// <param name="max">maximum Value To be added to Axis</param>
		public void AddMinMax(double min, double max)
		{
			if (this.MinValue == null)
			{
				m_MinValue = min;
				
				if (!isStartSet)
				{
					m_startValue = min;
					isStartSet = true;
				}
				actualMinvalue = min;
			}
			else
			{
				actualMinvalue = Double.Parse(this.MinValue.ToString());
			}
			
			if (this.MaxValue == null)
			{
				m_MaxValue = max;
				actualMaxvalue = max;
			}
			else
			{
				actualMaxvalue = Double.Parse(this.MaxValue.ToString());
			}
			
		}   
		
		/// <summary>
		/// Add Interval to Axis
		/// </summary>
		/// <param name="interval">Interval to be added for Axis</param>
		public void AddInterval(double interval)
		{
			if (this.Interval == null)
				m_Interval = interval;            
		}
		
		internal void GenerateLabels()
		{           
			m_Labels.Clear();
			m_labelValues.Clear();
			double value = m_MinValue;
			for (int i = 0; i <= m_IntervalCount; i++)
			{
				//if (value >= m_MinValue && value <= m_MaxValue)
				//{
				m_Labels.Add(GetOriginalLabel(value));
				m_labelValues.Add(value);                    
				//}                
				value += m_Interval;
				if (isIntervalCountZero)
					break;
			}           
		}
		
		protected bool IsRectIntersect(Rect source, Rect relativeTo)
		{
			return !(relativeTo.Left > source.Right || relativeTo.Right < source.Left || relativeTo.Top > source.Bottom || relativeTo.Bottom < source.Top);
		}
		
		
		public virtual double DataToPoint(double value)
		{
			return 0;            
		}
		
		public virtual void CalculateIntervalFromSeriesPoints()
		{
			
		}
		public virtual string GetOriginalLabel(double value)
		{
			return string.Empty;
		}
		
		protected void CalculateAutoInterval()
		{
			isIntervalCountZero = false;
			if (CheckType())
			{
				
				if (ZoomCoefficient < 1 || (ZoomOffset > 0 && ZoomCoefficient < 1))
				{
					m_MinValue = actualMinvalue + ZoomOffset * (actualMaxvalue - actualMinvalue); ;
					m_MaxValue = m_MinValue + ZoomCoefficient * (actualMaxvalue - actualMinvalue);
					if (m_MinValue < actualMinvalue)
					{
						m_MaxValue = m_MaxValue + (actualMinvalue - m_MinValue);
						m_MinValue = actualMinvalue;
					}
					
					if (m_MaxValue > actualMaxvalue)
					{
						m_MinValue = m_MinValue - (m_MaxValue - actualMaxvalue);
						m_MaxValue = actualMaxvalue;
					}
					m_startValue = m_MinValue;
				}
				else
				{
					m_MaxValue = actualMaxvalue;
					m_MinValue = actualMinvalue;
					m_startValue = actualMinvalue;
				}
				if (this.Interval == null)
				{
					switch (ActualType)
					{
					case ActualType.Double:
						//m_MinValue = Math.Floor(m_MinValue);                           
						//this.m_Interval = AxisUtil.CalculateInetrval(m_MinValue, m_MaxValue, m_IntervalCount);
						//m_MaxValue = m_MinValue + (m_IntervalCount * this.m_Interval);
						break;
					case ActualType.DateTime:
						this.m_Interval = (m_MaxValue - m_MinValue) / m_IntervalCount;
						break;
					default:
						break;
					}
					
					this.m_Interval = (m_MaxValue - m_MinValue) / m_IntervalCount;
				}
				else
				{
					switch (ActualType)
					{
					case ActualType.Double:
						this.m_IntervalCount = (int)Math.Abs((m_MaxValue - m_MinValue) / m_Interval);
						double temp_max = (this.m_IntervalCount * this.m_Interval) + m_MinValue;
						if (temp_max >= this.m_MaxValue)
							this.m_MaxValue = temp_max;
						if(temp_max<this.m_MaxValue)
						{
							this.m_MaxValue = temp_max + this.m_Interval;
							m_IntervalCount++;
						}
						
						break;
					case ActualType.DateTime:
						this.m_IntervalCount = (int)Math.Abs((m_MaxValue - m_MinValue) / m_Interval);
						break;
					default:
						break;
					}
					if (m_IntervalCount <= 1)
						isIntervalCountZero = true;
					m_IntervalCount = (m_IntervalCount > 0) ? m_IntervalCount : 1;
					
				}
				
				//if ((m_MinValue >= m_startValue + m_Interval))
				//    m_startValue = m_MinValue + (m_MinValue % m_Interval);
				
				
			}
			else
			{
				if (this.Interval == null)
				{
					this.m_Interval = 1;
					m_startValue = m_MinValue;
				}
				else
				{
					m_IntervalCount = (int)(((int)m_MaxValue - (int)m_MinValue) / (int)m_Interval);
					double temp_max = (this.m_IntervalCount * this.m_Interval) + m_MinValue;
					if (temp_max >= this.m_MaxValue)
						this.m_MaxValue = temp_max;
					else
						if (temp_max < this.m_MaxValue)
					{
						this.m_MaxValue = temp_max + this.m_Interval;
						m_IntervalCount++;
					}                    
					if ((m_MinValue >= m_startValue + m_Interval))
						m_startValue = m_startValue + m_Interval;
				}
				this.m_IntervalCount = (int)Math.Abs(((int)m_MaxValue - (int)m_MinValue) / (int)m_Interval);
				m_IntervalCount = (m_IntervalCount > 0) ? m_IntervalCount : 1;
				
			}
			
			RefreshSeries();
			if (this.Chart != null && this.Chart.containers != null && this.Chart.containers.axisLinesconatiner != null)
				this.Chart.containers.axisLinesconatiner.Refresh();
		}
		
		protected virtual bool CheckType()
		{
			return true;
		}
		
		public virtual void Refresh()
		{
			if (this.Chart != null)
				InvalidateVisuals();
		}
		
		internal void RefreshSeries()
		{
			if (Series != null)
				foreach (SeriesBase series in this.Series)
			{
				series.RefreshWithoutAxis(this);
			}
		}
		internal double m_Interval;
		internal double m_MaxValue = 1;
		internal double m_MinValue = 0;
		internal double actualMaxvalue = 1;
		internal double actualMinvalue = 0;
		internal double m_IntervalCount = 5;
		internal List<string> m_Labels;
		internal List<double> m_labelValues;
		internal double m_offset = 0;
		internal double m_startValue = 0;
		bool isStartSet;
		
		/// <summary>
		/// Axis Line Style
		/// </summary>
		public LineStyle AxisLineStyle
		{
			get;
			set;
		}

		private string stringFormat;
		/// <summary>
		/// String Format for AxisLabel
		/// </summary>
		public string StringFormat
		{
			get { return stringFormat; }
			set { stringFormat= value; }
		}
		
		/// <summary>
		/// Minor Line Size
		/// </summary>
		public double MinorLineSize
		{
			get { return (double)GetValue(MinorLineSizeProperty); }
			set { SetValue(MinorLineSizeProperty, value); }
		}
		
		public static readonly DependencyProperty MinorLineSizeProperty =
			DependencyProperty.Register("MinorLineSize", typeof(double), typeof(AxisBase), new PropertyMetadata(6d));
		
		
		/// <summary>
		/// Minor Line Style
		/// </summary>
		public Style MinorLineStyle
		{
			get { return (Style)GetValue(MinorLineStyleProperty); }
			set { SetValue(MinorLineStyleProperty, value); }
		}
		
		public static readonly DependencyProperty MinorLineStyleProperty =
			DependencyProperty.Register("MinorLineStyle", typeof(Style), typeof(AxisBase), new PropertyMetadata(null));
		
		
		/// <summary>
		/// Major Line Style
		/// </summary>
		public Style MajorLineStyle
		{
			get { return (Style)GetValue(MajorLineStyleProperty); }
			set { SetValue(MajorLineStyleProperty, value); }
		}
		
		public static readonly DependencyProperty MajorLineStyleProperty =
			DependencyProperty.Register("MajorLineStyle", typeof(Style), typeof(AxisBase), new PropertyMetadata(null));
		
		
		/// <summary>
		/// Major Line Size
		/// </summary>
		public double MajorLineSize
		{
			get { return (double)GetValue(MajorLineSizeProperty); }
			set { SetValue(MajorLineSizeProperty, value); }
		}
		
		public static readonly DependencyProperty MajorLineSizeProperty =
			DependencyProperty.Register("MajorLineSize", typeof(double), typeof(AxisBase), new PropertyMetadata(10d));
		
		private bool showMajorTicks;
		/// <summary>
		/// Major Tick Line Visible only if ShowMajorTicks is set to True
		/// </summary>
		public bool ShowMajorTicks
		{
			get { return showMajorTicks; }
			set { showMajorTicks= value; }
		}

		
		/// <summary>
		/// 
		/// </summary>
		public int MinorTicksCount
		{
			get { return (int)GetValue(MinorTicksCountProperty); }
			set { SetValue(MinorTicksCountProperty, value);}
		}
		
		public static readonly DependencyProperty MinorTicksCountProperty =
			DependencyProperty.Register("MinorTicksCount", typeof(int), typeof(AxisBase), new PropertyMetadata(0));
		
		
		
		public object Header
		{
			get { return (object)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}
		
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(object), typeof(AxisBase), new PropertyMetadata(null));
		
		
		
		public DataTemplate HeaderTemplate
		{
			get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
			set { SetValue(HeaderTemplateProperty, value); }
		}
		
		public static readonly DependencyProperty HeaderTemplateProperty =
			DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(AxisBase), new PropertyMetadata(null));
		
		
		
		public SparrowChart Chart
		{
			get { return (SparrowChart)GetValue(ChartProperty); }
			set { SetValue(ChartProperty, value); }
		}
		
		public static readonly DependencyProperty ChartProperty =
			DependencyProperty.Register("Chart", typeof(SparrowChart), typeof(AxisBase), new PropertyMetadata(null));
		
		
		
		public DataTemplate LabelTemplate
		{
			get { return (DataTemplate)GetValue(LabelTemplateProperty); }
			set { SetValue(LabelTemplateProperty, value); }
		}
		
		public static readonly DependencyProperty LabelTemplateProperty =
			DependencyProperty.Register("LabelTemplate", typeof(DataTemplate), typeof(AxisBase), new PropertyMetadata(null));
		
		
		
		public bool ShowCrossLines
		{
			get { return (bool)GetValue(ShowCrossLinesProperty); }
			set { SetValue(ShowCrossLinesProperty, value); }
		}
		
		public static readonly DependencyProperty ShowCrossLinesProperty =
			DependencyProperty.Register("ShowCrossLines", typeof(bool), typeof(AxisBase), new PropertyMetadata(true));
		
		
		
		public bool ShowMinorCrossLines
		{
			get { return (bool)GetValue(ShowMinorCrossLinesProperty); }
			set { SetValue(ShowMinorCrossLinesProperty, value); }
		}
		
		public static readonly DependencyProperty ShowMinorCrossLinesProperty =
			DependencyProperty.Register("ShowMinorCrossLines", typeof(bool), typeof(AxisBase), new PropertyMetadata(true));
		
		
		
		
		public Style MinorCrossLineStyle
		{
			get { return (Style)GetValue(MinorCrossLineStyleProperty); }
			set { SetValue(MinorCrossLineStyleProperty, value); }
		}
		
		public static readonly DependencyProperty MinorCrossLineStyleProperty =
			DependencyProperty.Register("MinorCrossLineStyle", typeof(Style), typeof(AxisBase), new PropertyMetadata(null));
		
		
		
		public Style CrossLineStyle
		{
			get { return (Style)GetValue(CrossLineStyleProperty); }
			set { SetValue(CrossLineStyleProperty, value); }
		}
		
		public static readonly DependencyProperty CrossLineStyleProperty =
			DependencyProperty.Register("CrossLineStyle", typeof(Style), typeof(AxisBase), new PropertyMetadata(null));
		
		
		
		public SeriesCollection Series
		{
			get { return (SeriesCollection)GetValue(SeriesProperty); }
			set { SetValue(SeriesProperty, value); }
		}
		
		public static readonly DependencyProperty SeriesProperty =
			DependencyProperty.Register("Series", typeof(SeriesCollection), typeof(AxisBase), new PropertyMetadata(null));
	}
}

