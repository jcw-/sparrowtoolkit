using System;
using System.Reflection;

namespace Sparrow.Chart
{
    public static class DateTimeExtension
    {
        private static DateTime BaseDate = new DateTime(1899, 12, 30);
        /// <summary>
        /// Converts the value of this instance to the equivalent OLE Automation date.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>       
        public static double ToOADate(this DateTime dateTime)
        {
            return dateTime.Subtract(BaseDate).TotalDays;
        }
        /// <summary>
        /// Returns a DateTime equivalent to the specified OLE Automation Date.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>       
        public static DateTime FromOADate(this Double doubleDate)
        {
            return BaseDate.AddDays(doubleDate);
        }

        internal static MethodInfo GetSetMethod(this PropertyInfo propertyInfo)
        {
            return propertyInfo.SetMethod;
        }

        internal static MethodInfo GetGetMethod(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetMethod;
        }
        internal static bool IsValueType(this Type type)
        {
            return true;
        }
    }
}
