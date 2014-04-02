using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Sparrow.Chart
{
    /// <summary>
    /// String To ChartPoint Converter
    /// </summary>
    public class StringToChartPointConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether the type converter can convert an object from the specified type to the type of this converter.
        /// </summary>
        /// <param name="context">An object that provides a format context.</param>
        /// <param name="sourceType">The type you want to convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context,Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// Converts from the specified value to the intended conversion type of the converter.
        /// </summary>
        /// <param name="context">An object that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The value to convert to the type of this converter.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context,System.Globalization.CultureInfo culture, object value)
        {
            List<string> result = ((string)value).Split(',').ToList();
            for (int j=0;j<result.Count;j++)
            {
                var point = result[j];
                if (string.IsNullOrEmpty(point))
                    result[j] = "0";
            }
            if (result.Count % 2 != 0)
                result.Add("0");
            PointsCollection collection = new PointsCollection();
            for (int i = 0; i < result.Count; i += 2)
            {
                collection.Add(new ChartPoint() { XValue = double.Parse(result[i].ToString(CultureInfo.InvariantCulture)), YValue = double.Parse(result[i + 1].ToString(CultureInfo.InvariantCulture)) });
            }
            return collection;
        }

        /// <summary>
        /// Returns whether the type converter can convert an object to the specified type.
        /// </summary>
        /// <param name="context">An object that provides a format context.</param>
        /// <param name="destinationType">The type you want to convert to.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context,Type destinationType)
        {
            return destinationType == typeof(string);
        }

        /// <summary>
        /// Converts the specified value object to the specified type.
        /// </summary>
        /// <param name="context">An object that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The object to convert.</param>
        /// <param name="destinationType">The type to convert the object to.</param>
        /// <returns>
        /// The converted object.
        /// </returns>
        public override object ConvertTo(ITypeDescriptorContext context,System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return null;
        }
    }
}
