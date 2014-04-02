
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
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Collections.Specialized;
using Android.Graphics;

namespace Sparrow.Chart
{
	/// <summary>
	/// Base for All Series 
	/// </summary>
	public abstract class SeriesBase : UIElement
	{
		protected double xMin { get; set; }
		protected double xMax { get; set; }
		protected double yMin { get; set; }
		protected double yMax { get; set; }
		double xDifference;
		double yDifference;
		double xAbs;
		double yAbs;
		internal List<double> xValues;
		internal List<double> yValues;
		internal SeriesContainer seriesContainer;
		internal bool isFill;
		protected bool isRefreshed;
		internal bool isPointsGenerated;
		
		public virtual void GenerateDatas()
		{
		}
		
		
		public SeriesBase()
		{
			this.Parts = new PartsCollection();
			this.Points = new PointsCollection();
		}
		
		internal void CheckMinMaxandInterval(Double value, AxisBase axis)
		{
			axis.m_MinValue = Math.Min(axis.m_MinValue, value);
			axis.m_MaxValue = Math.Max(axis.m_MaxValue, value);
		}
		
		public virtual void CalculateMinAndMax()
		{
			if (this.XAxis != null)
			{
				xMin = this.XAxis.m_MinValue;
				xMax = this.XAxis.m_MaxValue;
			}
			if (this.YAxis != null)
			{
				yMin = this.YAxis.m_MinValue;
				yMax = this.YAxis.m_MaxValue;
			}
		}
		
		public Point NormalizePoint(Point pt)
		{
			Point result = new Point();

			result.X = ((pt.X - xMin) * (seriesContainer.collection.ActualWidth / (xMax - xMin)));
			result.Y = (seriesContainer.collection.ActualHeight - ((pt.Y - yMin) * seriesContainer.collection.ActualHeight) / (yMax - yMin));
			return result;
		}
		
		protected bool CheckValue(double value)
		{
			bool isOk = true;
			if (double.IsNaN(value) || double.IsInfinity(value) || double.IsNegativeInfinity(value) || double.IsPositiveInfinity(value) || value == double.MaxValue || value == double.MinValue)
				isOk = false;
			return isOk;
		}
		
		public virtual void Refresh()
		{
			if (this.XAxis != null)
			{
				this.XAxis.m_MinValue = 0;
				this.XAxis.m_MaxValue = 1;
				this.XAxis.CalculateIntervalFromSeriesPoints();
				this.XAxis.Refresh();
			}
			if (this.YAxis != null)
			{
				this.YAxis.m_MinValue = 0;
				this.YAxis.m_MaxValue = 1;
				this.YAxis.CalculateIntervalFromSeriesPoints();
				this.YAxis.Refresh();
			}
		}
		
		internal virtual SeriesContainer CreateContainer()
		{
			return null;
		}
		
		public void RefreshWithoutAxis(AxisBase axis)
		{           
			if (IsRefresh)
			{
				this.GenerateDatas();
			}
		}
		
		protected PointsCollection GetPointsFromValues(List<double> xValues,List<double> yValues)
		{
			PointsCollection tempPoints = new PointsCollection();
			for (int i = 0; (i < xValues.Count && i < yValues.Count); i++)
			{
				ChartPoint point = new ChartPoint() { XValue = xValues[i], YValue = yValues[i] };
				tempPoints.Add(point);
			}
			return tempPoints;
		}
		
		protected void IntializePoints()
		{
			xDifference = xMax - xMin;
			yDifference = yMax - yMin;
			xAbs = Math.Abs(xDifference / Width);
			yAbs = Math.Abs(yDifference / Height);
		}
		
		protected bool CheckValuePoint(ChartPoint oldPoint,ChartPoint point)
		{
			//point.YValue <= yMax && point.YValue >= yMin && point.XValue <= xMax && point.XValue >= xMin && 
			if ((Math.Abs(oldPoint.XValue - point.XValue) >= xAbs || Math.Abs(oldPoint.YValue - point.YValue) >= yAbs))
				return true;
			else
				return false;
		}
		
		public double GetReflectionValueFromItem(string path, Object item)
		{
			PropertyInfo propertInfo = item.GetType().GetProperty(path);
			FastProperty fastPropertInfo = new FastProperty(propertInfo);
			
			if (propertInfo != null)
			{
				object value = fastPropertInfo.Get(item);
				if (value is Double || value is int)
				{
					return Double.Parse(value.ToString());
				}
				else if (value is DateTime)
				{
					return ((DateTime)value).ToOADate();
				}
				else if (value is String)
				{
					if (!SparrowChart.ActualCategoryValues.Contains(value.ToString()))
						SparrowChart.ActualCategoryValues.Add(value.ToString());
					return SparrowChart.ActualCategoryValues.IndexOf(value.ToString());
				}
				else
					throw new NotSupportedException(String.Format("The {0} type is Not Supported by Chart", path));
			}
			return 0d;
		}
		
		public double GetReflectionValue(string path, IEnumerable source, int position)
		{
			if (!String.IsNullOrEmpty(path))
			{
				IEnumerator enumerator = source.GetEnumerator();
				double index = 0;
				for (int i = 0; i < position - 1; i++)
				{
					enumerator.MoveNext();
				}
				
				if (enumerator.MoveNext())
				{
					PropertyInfo propertInfo = enumerator.Current.GetType().GetProperty(path);
					FastProperty fastPropertInfo = new FastProperty(propertInfo);
					if (propertInfo != null)
					{
						object value = fastPropertInfo.Get(enumerator.Current);
						if (value is Double || value is int)
						{
							return Double.Parse(value.ToString());
						}
						else if (value is DateTime)
						{
							return ((DateTime)value).ToOADate();
						}
						else if (value is String)
						{
							if (!SparrowChart.ActualCategoryValues.Contains(value.ToString()))
								SparrowChart.ActualCategoryValues.Add(value.ToString());
							return SparrowChart.ActualCategoryValues.IndexOf(value.ToString());
						}
						else
							throw new NotSupportedException(String.Format("The {0} type is Not Supported by Chart", path));
						
					}
				}
			}
			return 0d;
		}
		
		public List<double> GetReflectionValues(string path,IEnumerable source, List<double> oldValues,bool isUpdate)
		{
			List<double> values;
			if(isUpdate)
				values=oldValues;
			else
				values = new List<double>();
			bool notifyCollectionChanged=false;
			if (!string.IsNullOrEmpty(path))
			{
				IEnumerator enumerator = source.GetEnumerator();
				double index = 0d;
				
				if (enumerator.MoveNext())
				{                    
					if (enumerator.Current is INotifyPropertyChanged)
						notifyCollectionChanged = true;
					PropertyInfo xPropertInfo = enumerator.Current.GetType().GetProperty(path);

					FastProperty fastPropertInfo = new FastProperty(xPropertInfo);
					do
					{                        
						if (xPropertInfo != null)
						{
							object value = fastPropertInfo.Get(enumerator.Current);
							if (value is Double || value is int)
							{
								values.Add(Double.Parse(value.ToString()));
							}
							else if (value is DateTime)
							{
								values.Add(((DateTime)value).ToOADate());
							}
							else if (value is String)
							{
								if (!SparrowChart.ActualCategoryValues.Contains(value.ToString()))
									SparrowChart.ActualCategoryValues.Add(value.ToString());
								values.Add(SparrowChart.ActualCategoryValues.IndexOf(value.ToString()));
							}
							else
								throw new NotSupportedException(String.Format("The {0} type is Not Supported by Chart", path));
							index++;
						}
					} while (enumerator.MoveNext());
					if (notifyCollectionChanged)
					{
						enumerator.Reset();
						while (enumerator.MoveNext())
						{
							(enumerator.Current as INotifyPropertyChanged).PropertyChanged -= Collection_PropertyChanged;
							(enumerator.Current as INotifyPropertyChanged).PropertyChanged += Collection_PropertyChanged;
						}
						
					}
				}
				
			}
			return values;
		}
		
		virtual protected void Collection_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Refresh();
		}

		private XAxis xAxis;
		public XAxis XAxis
		{
			get { return xAxis; }
			set { xAxis=value; }
		}

		private bool isRefresh;
		public bool IsRefresh
		{
			get { return isRefresh; }
			set { isRefresh= value; }
		}

		private YAxis yAxis;
		public YAxis YAxis
		{
			get { return yAxis; }
			set { yAxis= value; }
		}

		private PartsCollection parts;
		public PartsCollection Parts
		{
			get { return parts; }
			set { parts = value; }
		}

		private bool useSinglePart;
		public bool UseSinglePart
		{
			get { return useSinglePart; }
			set { useSinglePart= value; }
		}	
		
		
		private SparrowChart chart;
		public SparrowChart Chart
		{
			get { return chart; }
			set { chart= value; }
		}
		
		
		private Color stroke;
		public Color Stroke
		{
			get { return stroke; }
			set { stroke = value; }
		}

		private PointsCollection points;
		public PointsCollection Points
		{
			get { return points; }
			set {
				if (points != null)
				{
					points.CollectionChanged -= Points_CollectionChanged;
				}
				points = value;
				if (points != null)
				{
					points.CollectionChanged += Points_CollectionChanged;
				}
				isPointsGenerated = false;
				
				if (this.IsRefresh)
					Refresh();  

			}
		}

		void Points_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			isPointsGenerated = false;
			if (this.IsRefresh)
				Refresh();
		}

		private string xPath;
		public string XPath
		{
			get { return xPath; }
			set { xPath= value; }
		}
		
		
		private object label;
		public object Label
		{
			get { return label; }
			set { label= value; }
		}

		private float strokeThickness;
		public float StrokeThickness
		{
			get { return strokeThickness; }
			set { strokeThickness= value; }
		}
				
		protected virtual void SetBindingForStrokeandStrokeThickness(SeriesPartBase part)
		{
			part.Stroke=this.Stroke;
			part.StrokeThickness=this.StrokeThickness;
		}
		
		private int index;
		internal int Index
		{
			get { return index; }
			set { index = value; }
		}
	}   
}

