using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sparrow.Chart.Demos
{
    public class SampleToContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is SampleModel)
            {
                if (!(value as SampleModel).IsIntialized)
                    (value as SampleModel).View = (UserControl)Activator.CreateInstance(Type.GetType((value as SampleModel).ViewClass));
                return (value as SampleModel).View;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
