using System;
using System.Collections.Generic;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Data;
using Line = System.Windows.Shapes.Line;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
#endif


namespace Sparrow.Chart
{

    /// <summary>
    /// Axis Cross Line Container
    /// </summary>
    public class AxisCrossLinesContainer : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisCrossLinesContainer"/> class.
        /// </summary>
        public AxisCrossLinesContainer()
        {
            this.SizeChanged += AxisCrossLinesContainer_SizeChanged;
        }

        /// <summary>
        /// Handles the SizeChanged event of the AxisCrossLinesContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        void AxisCrossLinesContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           Refresh();
        }
               

        List<Line> _xLines;
        List<Line> _yLines;
        List<Line> _xMinorLines;
        List<Line> _yMinorLines;
        bool _isInitialized;

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            if (!_isInitialized)
                Initialize();
            else
                Update();

        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        private void Update()
        {
            double xAxisWidthStep = (int)this.ActualWidth / this.XAxis.MIntervalCount;
            double xAxisWidthPosition = xAxisWidthStep;
            int xminorCount = 0;
            int yminorCount = 0;
            if ((this.XAxis.MLabels.Count) == _xLines.Count)
            {
                for (int i = 0; i < this.XAxis.MLabels.Count; i++)
                {
                    Line line = _xLines[i];
                    line.X1 = xAxisWidthPosition;
                    line.X2 = xAxisWidthPosition;                   
                    line.Y2 = (int)this.ActualHeight;
                    if (i != (this.XAxis.MLabels.Count - 1))
                    {
                        for (int a = 0; a < this.XAxis.MinorTicksCount; a++)
                        {
                            Line minorline = _xMinorLines[xminorCount];                          
                            minorline.X1 = this.XAxis.DataToPoint((this.XAxis.MStartValue + (this.XAxis.MInterval * i)) + ((this.XAxis.MInterval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X2 = this.XAxis.DataToPoint((this.XAxis.MStartValue + (this.XAxis.MInterval * i)) + ((this.XAxis.MInterval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y1 = 0;
                            minorline.Y2 = this.ActualHeight;
                            xminorCount++;
                        }
                    }
                    xAxisWidthPosition += xAxisWidthStep;
                }
            }
            else
            {
                if ((this.XAxis.MLabels.Count) > _xLines.Count)
                {
                    int offset = (this.XAxis.MLabels.Count) - _xLines.Count;
                    for (int j = 0; j < offset; j++)
                    {
                        Line line = new Line {X1 = 0, X2 = 0, Y2 = this.ActualHeight};
                        Binding styleBinding = new Binding
                            {
                                Path = new PropertyPath("CrossLineStyle"),
                                Source = this.XAxis
                            };
                        Binding showCrossLines = new Binding
                            {
                                Path = new PropertyPath("ShowCrossLines"),
                                Source = this.XAxis,
                                Converter = new BooleanToVisibilityConverter()
                            };
                        line.SetBinding(Line.VisibilityProperty, showCrossLines);
                        line.SetBinding(Line.StyleProperty, styleBinding);
                        for (int a = 0; a < this.XAxis.MinorTicksCount; a++)
                        {
                            Line minorline = new Line();
                            Binding minorstyleBinding = new Binding
                                {
                                    Path = new PropertyPath("MinorCrossLineStyle"),
                                    Source = this.XAxis
                                };
                            minorline.SetBinding(Line.StyleProperty, minorstyleBinding);
                            Binding minorshowCrossLines = new Binding
                                {
                                    Path = new PropertyPath("ShowMinorCrossLines"),
                                    Source = this.XAxis
                                };
                            minorshowCrossLines.Converter = new BooleanToVisibilityConverter();
                            minorline.SetBinding(Line.VisibilityProperty, minorshowCrossLines);
                            minorline.X1 = this.XAxis.DataToPoint((this.XAxis.MStartValue + (this.XAxis.MInterval * j)) + ((this.XAxis.MInterval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X2 = this.XAxis.DataToPoint((this.XAxis.MStartValue + (this.XAxis.MInterval * j)) + ((this.XAxis.MInterval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y1 = 0;
                            minorline.Y2 = this.ActualHeight;
                            this.Children.Add(minorline);
                            _xMinorLines.Add(minorline);
                        }
                        _xLines.Add(line);
                        this.Children.Add(line);
                    }
                    
                }
                else
                {
                    int offset = _xLines.Count - (this.XAxis.MLabels.Count);
                    for (int j = 0; j < offset; j++)
                    {
                        for (int a = 0; a < this.XAxis.MinorTicksCount; a++)
                        {
                            this.Children.Remove(_xMinorLines[_xMinorLines.Count - 1]);
                            _xMinorLines.RemoveAt(_xMinorLines.Count - 1);
                        }
                        this.Children.Remove(_xLines[_xLines.Count - 1]);
                        _xLines.RemoveAt(_xLines.Count - 1);
                    }
                }
                for (int i = 0; i < this.XAxis.MLabels.Count; i++)
                {
                    Line line = _xLines[i];
                    line.X1 = xAxisWidthPosition;
                    line.X2 = xAxisWidthPosition;
                    line.Y2 = this.ActualHeight;
                    if (i != (this.XAxis.MLabels.Count - 1))
                    {
                        for (int a = 0; a < this.XAxis.MinorTicksCount; a++)
                        {
                            Line minorline = _xMinorLines[xminorCount];
                            minorline.X1 = this.XAxis.DataToPoint((this.XAxis.MStartValue + (this.XAxis.MInterval * i)) + ((this.XAxis.MInterval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X2 = this.XAxis.DataToPoint((this.XAxis.MStartValue + (this.XAxis.MInterval * i)) + ((this.XAxis.MInterval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y1 = 0;
                            minorline.Y2 = this.ActualHeight;
                            xminorCount++;
                        }
                    }
                    xAxisWidthPosition += xAxisWidthStep;
                }  
            }
            double yAxisHeightStep = (int)this.ActualHeight / this.YAxis.MIntervalCount;
            double yAxisHeightPosition = yAxisHeightStep;
            if (YAxis.MLabels.Count == _yLines.Count)
            {
                for (int i = 0; i < YAxis.MLabels.Count ; i++)
                {
                    Line line = _yLines[i];
                    line.X2 = this.ActualWidth;
                    line.Y1 = yAxisHeightPosition;
                    line.Y2 = yAxisHeightPosition;
                    if (i != (this.YAxis.MLabels.Count - 1))
                    {
                        for (int a = 0; a < this.YAxis.MinorTicksCount; a++)
                        {
                            Line minorline = _yMinorLines[yminorCount];
                            minorline.Y1 = this.YAxis.DataToPoint((this.YAxis.MStartValue + (this.YAxis.MInterval * i)) + ((this.YAxis.MInterval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y2 = this.YAxis.DataToPoint((this.YAxis.MStartValue + (this.YAxis.MInterval * i)) + ((this.YAxis.MInterval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X1 = 0;
                            minorline.X2 = this.ActualWidth;
                            yminorCount++;
                        }
                    }
                    yAxisHeightPosition += yAxisHeightStep;
                }
            }
            else
            {
                if ((this.YAxis.MLabels.Count) > _yLines.Count)
                {
                    int offset = (this.YAxis.MLabels.Count) - _yLines.Count;
                    for (int j = 0; j < offset; j++)
                    {
                        Line line = new Line();
                        line.X1 = 0;
                        line.X2 = this.ActualWidth;
                        line.Y1 = yAxisHeightPosition;
                        line.Y2 = yAxisHeightPosition;
                        Binding showCrossLines = new Binding
                            {
                                Path = new PropertyPath("ShowCrossLines"),
                                Source = this.YAxis,
                                Converter = new BooleanToVisibilityConverter()
                            };
                        line.SetBinding(Line.VisibilityProperty, showCrossLines);
                        Binding styleBinding = new Binding
                            {
                                Path = new PropertyPath("CrossLineStyle"),
                                Source = this.YAxis
                            };
                        line.SetBinding(Line.StyleProperty, styleBinding);
                        for (int a = 0; a < this.YAxis.MinorTicksCount; a++)
                        {
                            Line minorline = new Line();
                            Binding minorstyleBinding = new Binding
                                {
                                    Path = new PropertyPath("MinorCrossLineStyle"),
                                    Source = this.YAxis
                                };
                            minorline.SetBinding(Line.StyleProperty, minorstyleBinding);
                            Binding minorshowCrossLines = new Binding
                                {
                                    Path = new PropertyPath("ShowMinorCrossLines"),
                                    Source = this.YAxis
                                };
                            minorshowCrossLines.Converter = new BooleanToVisibilityConverter();
                            minorline.SetBinding(Line.VisibilityProperty, minorshowCrossLines);
                            minorline.Y1 = this.YAxis.DataToPoint((this.YAxis.MStartValue + (this.YAxis.MInterval * j)) + ((this.YAxis.MInterval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y2 = this.YAxis.DataToPoint((this.YAxis.MStartValue + (this.YAxis.MInterval * j)) + ((this.YAxis.MInterval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X1 = 0;
                            minorline.X2 = this.ActualWidth;
                            this.Children.Add(minorline);
                            _yMinorLines.Add(minorline);
                        }
                        this.Children.Add(line);
                        this._yLines.Add(line);
                    }
                }
                else
                {
                    int offset = _yLines.Count - (this.YAxis.MLabels.Count);
                    for (int j = 0; j < offset; j++)
                    {
                        for (int a = 0; a < this.YAxis.MinorTicksCount; a++)
                        {
                            this.Children.Remove(_yMinorLines[_yMinorLines.Count - 1]);
                            _yMinorLines.RemoveAt(_yMinorLines.Count - 1);
                        }
                        this.Children.Remove(_yLines[_yLines.Count - 1]);
                        _yLines.RemoveAt(_yLines.Count - 1);
                    }
                }
                for (int i = 0; i < YAxis.MLabels.Count-1; i++)
                {
                    Line line = _yLines[i];
                    line.X2 = this.ActualWidth;
                    line.Y1 = yAxisHeightPosition;
                    line.Y2 = yAxisHeightPosition;
                    if (i != (this.YAxis.MLabels.Count - 1))
                    {
                        for (int a = 0; a < this.YAxis.MinorTicksCount; a++)
                        {
                            Line minorline = _yMinorLines[yminorCount];
                            minorline.Y1 = this.YAxis.DataToPoint((this.YAxis.MStartValue + (this.YAxis.MInterval * i)) + ((this.YAxis.MInterval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y2 = this.YAxis.DataToPoint((this.YAxis.MStartValue + (this.YAxis.MInterval * i)) + ((this.YAxis.MInterval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X1 = 0;
                            minorline.X2 = this.ActualWidth;
                            yminorCount++;
                        }
                    }
                    yAxisHeightPosition += yAxisHeightStep;
                }
            }
        }
        private void Initialize()
        {
            this.Children.Clear();
            if (this.ActualHeight > 0 && this.ActualWidth > 0)
            {
                double xAxisWidthStep = this.ActualWidth / this.XAxis.MIntervalCount;
                double xAxisWidthPosition = 0;
                _xLines = new List<Line>();
                _yLines = new List<Line>();
                _xMinorLines = new List<Line>();
                _yMinorLines = new List<Line>();
                int k = 0;
                for (int i = 0; i < this.XAxis.MLabels.Count; i++)
                {
                    Line line = new Line();
                    line.X1 = this.XAxis.DataToPoint(this.XAxis.MStartValue + (this.XAxis.MInterval * k));
                    line.X2 = this.XAxis.DataToPoint(this.XAxis.MStartValue + (this.XAxis.MInterval * k));
                    line.Y1 = 0;
                    line.Y2 = this.ActualHeight;
                    Binding styleBinding = new Binding {Path = new PropertyPath("CrossLineStyle"), Source = this.XAxis};
                    line.SetBinding(Line.StyleProperty, styleBinding);
                    Binding showCrossLines = new Binding
                        {
                            Path = new PropertyPath("ShowCrossLines"),
                            Source = this.XAxis
                        };
                    showCrossLines.Converter = new BooleanToVisibilityConverter();
                    line.SetBinding(Line.VisibilityProperty, showCrossLines);
                    if (i != (this.XAxis.MLabels.Count - 1))
                    {
                        for (int a = 0; a < this.XAxis.MinorTicksCount; a++)
                        {
                            Line minorline = new Line();
                            Binding minorstyleBinding = new Binding
                                {
                                    Path = new PropertyPath("MinorCrossLineStyle"),
                                    Source = this.XAxis
                                };
                            minorline.SetBinding(Line.StyleProperty, minorstyleBinding);
                            Binding minorshowCrossLines = new Binding
                                {
                                    Path = new PropertyPath("ShowMinorCrossLines"),
                                    Source = this.XAxis
                                };
                            minorshowCrossLines.Converter = new BooleanToVisibilityConverter();
                            minorline.SetBinding(Line.VisibilityProperty, minorshowCrossLines);
                            minorline.X1 = this.XAxis.DataToPoint((this.XAxis.MStartValue + (this.XAxis.MInterval * k)) + ((this.XAxis.MInterval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X2 = this.XAxis.DataToPoint((this.XAxis.MStartValue + (this.XAxis.MInterval * k)) + ((this.XAxis.MInterval / (this.XAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y1 = 0;
                            minorline.Y2 = this.ActualHeight;
                            this.Children.Add(minorline);
                            _xMinorLines.Add(minorline);
                        }
                    }
                    _xLines.Add(line);
                    this.Children.Add(line);
                    xAxisWidthPosition += xAxisWidthStep;
                    k++;
                }
                double yAxisHeightStep = this.ActualHeight / this.YAxis.MIntervalCount;
                double yAxisHeightPosition = 0;
                int j = 0;
                for (int i = 0; i < YAxis.MLabels.Count; i++)
                {
                    Line line = new Line();
                    line.X1 = 0;
                    line.X2 = this.ActualWidth;
                    line.Y1 = this.YAxis.DataToPoint(this.YAxis.MStartValue + (this.YAxis.MInterval * j));
                    line.Y2 = this.YAxis.DataToPoint(this.YAxis.MStartValue + (this.YAxis.MInterval * j));
                    Binding showCrossLines = new Binding
                        {
                            Path = new PropertyPath("ShowCrossLines"),
                            Source = this.YAxis
                        };
                    showCrossLines.Converter = new BooleanToVisibilityConverter();
                    line.SetBinding(Line.VisibilityProperty, showCrossLines);
                    Binding styleBinding = new Binding {Path = new PropertyPath("CrossLineStyle"), Source = this.YAxis};
                    line.SetBinding(Line.StyleProperty, styleBinding);
                    if (i != (this.YAxis.MLabels.Count - 1))
                    {
                        for (int a = 0; a < this.YAxis.MinorTicksCount; a++)
                        {
                            Line minorline = new Line();
                            Binding minorstyleBinding = new Binding
                                {
                                    Path = new PropertyPath("MinorCrossLineStyle"),
                                    Source = this.YAxis
                                };
                            minorline.SetBinding(Line.StyleProperty, minorstyleBinding);
                            Binding minorshowCrossLines = new Binding
                                {
                                    Path = new PropertyPath("ShowMinorCrossLines"),
                                    Source = this.YAxis
                                };
                            minorshowCrossLines.Converter = new BooleanToVisibilityConverter();
                            minorline.SetBinding(Line.VisibilityProperty, minorshowCrossLines);
                            minorline.Y1 = this.YAxis.DataToPoint((this.YAxis.MStartValue + (this.YAxis.MInterval * j)) + ((this.YAxis.MInterval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.Y2 = this.YAxis.DataToPoint((this.YAxis.MStartValue + (this.YAxis.MInterval * j)) + ((this.YAxis.MInterval / (this.YAxis.MinorTicksCount + 1)) * (a + 1)));
                            minorline.X1 = 0;
                            minorline.X2 = this.ActualWidth;
                            this.Children.Add(minorline);
                            _yMinorLines.Add(minorline);
                        }
                    }
                    this.Children.Add(line);
                    this._yLines.Add(line);
                    yAxisHeightPosition += yAxisHeightStep;
                    j++;
                }
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Gets or sets the X axis.
        /// </summary>
        /// <value>
        /// The X axis.
        /// </value>
        public XAxis XAxis
        {
            get { return (XAxis)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        /// <summary>
        /// The X axis property
        /// </summary>
        public static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register("XAxis", typeof(XAxis), typeof(AxisCrossLinesContainer), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the Y axis.
        /// </summary>
        /// <value>
        /// The Y axis.
        /// </value>
        public YAxis YAxis
        {
            get { return (YAxis)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        /// <summary>
        /// The Y axis property
        /// </summary>
        public static readonly DependencyProperty YAxisProperty =
            DependencyProperty.Register("YAxis", typeof(YAxis), typeof(AxisCrossLinesContainer), new PropertyMetadata(null));

        /// <summary>
        /// Provides the behavior for the Arrange pass of Silverlight layout. Classes can override this method to define their own Arrange pass behavior.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
        /// <returns>
        /// The actual size that is used after the element is arranged in layout.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(new Point(0, 0), finalSize));
            }
           
            return finalSize;
        }
        /// <summary>
        /// Provides the behavior for the Measure pass of Silverlight layout. Classes can override this method to define their own Measure pass behavior.
        /// </summary>
        /// <param name="availableSize">The available size that this object can give to child objects. Infinity (<see cref="F:System.Double.PositiveInfinity" />) can be specified as a value to indicate that the object will size to whatever content is available.</param>
        /// <returns>
        /// The size that this object determines it needs during layout, based on its calculations of the allocated sizes for child objects; or based on other considerations, such as a fixed container size.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = new Size(0, 0);
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
                desiredSize.Width += child.DesiredSize.Width;
                desiredSize.Height += child.DesiredSize.Height;
            }
            if (Double.IsInfinity(availableSize.Height))
                availableSize.Height = desiredSize.Height;
            if (Double.IsInfinity(availableSize.Width))
                availableSize.Width = desiredSize.Width;
            return availableSize;
        }
    }
}
