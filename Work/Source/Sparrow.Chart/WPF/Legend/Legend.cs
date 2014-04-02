using System;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
using System.Windows.Media;

#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#endif

namespace Sparrow.Chart
{

    /// <summary>
    /// Chart Legend
    /// </summary>
    public class Legend : ItemsControl
    {        
        ResourceDictionary styles;

        /// <summary>
        /// Initializes a new instance of the <see cref="Legend"/> class.
        /// </summary>
        public Legend()
        {
            this.DefaultStyleKey = typeof(Legend);
            styles = new ResourceDictionary()
            {
#if X86
                Source = new Uri(@"/Sparrow.Chart.DirectX2D_x86;component/Themes/Styles.xaml", UriKind.Relative)
#elif X64
                Source = new Uri(@"/Sparrow.Chart.DirectX2D_x64;component/Themes/Styles.xaml", UriKind.Relative)
#elif WPF
#if NET45
                Source = new Uri(@"/Sparrow.Chart.Wpf.45;component/Themes/Styles.xaml", UriKind.Relative)
#elif NET40
                Source = new Uri(@"/Sparrow.Chart.Wpf.40;component/Themes/Styles.xaml", UriKind.Relative)
#else
                Source = new Uri(@"/Sparrow.Chart.Wpf.35;component/Themes/Styles.xaml", UriKind.Relative)
#endif
#elif SILVERLIGHT
#if SL5
                Source = new Uri(@"/Sparrow.Chart.Silverlight.50;component/Themes/Styles.xaml", UriKind.Relative)
#else
                Source = new Uri(@"/Sparrow.Chart.Silverlight.40;component/Themes/Styles.xaml", UriKind.Relative)
#endif
#elif WINRT
                Source = new Uri(@"ms-appx:///Sparrow.Chart.WinRT.45/Themes/Styles.xaml")
#elif WP7
#if NET45
                Source = new Uri(@"/Sparrow.Chart.WP7.45;component/Themes/Styles.xaml", UriKind.Relative)
#else
                Source = new Uri(@"/Sparrow.Chart.WP7.40;component/Themes/Styles.xaml", UriKind.Relative)
#endif
#elif WP8
                Source = new Uri(@"/Sparrow.Chart.WP8.45;component/Themes/Styles.xaml", UriKind.Relative)
#endif
            };
            this.HeaderTemplate = (DataTemplate)styles["legendTitleTemplate"];
            this.ItemTemplate = (DataTemplate)styles["defaultItemTemplate"];
#if WINRT || WP7 || WP8 || SILVERLIGHT4
            Loaded += Legend_Loaded;
#endif
        }

        /// <summary>
        /// Gets or sets the chart.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public SparrowChart Chart
        {
            get { return (SparrowChart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        /// <summary>
        /// The chart property
        /// </summary>
        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(SparrowChart), typeof(Legend), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the dock.
        /// </summary>
        /// <value>
        /// The dock.
        /// </value>
        public Dock Dock
        {
            get { return (Dock)GetValue(DockProperty); }
            set { SetValue(DockProperty, value); }
        }



        /// <summary>
        /// Gets or sets the legend position.
        /// </summary>
        /// <value>
        /// The legend position.
        /// </value>
        public LegendPosition LegendPosition
        {
            get { return (LegendPosition)GetValue(LegendPositionProperty); }
            set { SetValue(LegendPositionProperty, value); }
        }

        /// <summary>
        /// The legend position property
        /// </summary>
        public static readonly DependencyProperty LegendPositionProperty =
            DependencyProperty.Register("LegendPosition", typeof(LegendPosition), typeof(Legend), new PropertyMetadata(LegendPosition.Outside));



#if WPF || SILVERLIGHT && !SILVERLIGHT4
        /// <summary>
        /// The dock property
        /// </summary>
        public static readonly DependencyProperty DockProperty =
            DependencyProperty.Register("Dock", typeof(Dock), typeof(Legend), new PropertyMetadata(Dock.Top));
#endif

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// The header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(Legend), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the header template.
        /// </summary>
        /// <value>
        /// The header template.
        /// </value>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// The header template property
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Legend), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>
        /// The corner radius.
        /// </value>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// The corner radius property
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Legend), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// Gets or sets a value indicating whether [show icon].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show icon]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        /// <summary>
        /// The show icon property
        /// </summary>
        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(Legend), new PropertyMetadata(true));

#if WINRT || WP7 || WP8 || SILVERLIGHT4
        void Legend_Loaded(object sender, RoutedEventArgs e)
        {
            DockChanged();
        }     

        public static readonly DependencyProperty DockProperty =
            DependencyProperty.Register("Dock", typeof(Dock), typeof(Legend), new PropertyMetadata(Dock.Top,OnDockChanged));

        private static void OnDockChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as Legend).DockChanged();
        }

        internal void DockChanged()
        {           
            ItemsPresenter itemsPresenter = GetVisualChild<ItemsPresenter>(this);
            if (itemsPresenter != null)
            {
                if (VisualTreeHelper.GetChildrenCount(itemsPresenter) > 0)
                {
#if WINRT
                    StackPanel itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 1) as StackPanel;
#else
                    StackPanel itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as StackPanel;
#endif                   
                    if (itemsPanel != null)
                    {
                        switch (Dock)
                        {
                            case Dock.Top:
                            case Dock.Bottom:
                                itemsPanel.Orientation = Orientation.Horizontal;
                                break;
                            case Dock.Left:
                            case Dock.Right:
                                itemsPanel.Orientation = Orientation.Vertical;
                                break;
                            default:
                                break;
                        }                       
                    }
                }
            }
        }        
        /// <summary>
        /// http://svgvijay.blogspot.in/2013/01/how-to-get-datagrid-cell-in-wpf.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        private static T GetVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                DependencyObject v = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
#endif

    }
}
