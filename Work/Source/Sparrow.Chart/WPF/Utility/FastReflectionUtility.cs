using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sparrow.Chart
{
    #region FastProperty
    /// <summary>
    /// http://geekswithblogs.net/Madman/archive/2008/06/27/faster-reflection-using-expression-trees.aspx
    /// </summary>
    public class FastProperty
    {

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// The get delegate
        /// </summary>
        public Func<object, object> GetDelegate;

        /// <summary>
        /// The set delegate
        /// </summary>
        public Action<object, object> SetDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="FastProperty"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        public FastProperty(PropertyInfo property)
        {
            this.Property = property;
            InitializeGet();
            InitializeSet();
        }

        /// <summary>
        /// Initializes the set.
        /// </summary>
        private void InitializeSet()
        {
            var instance = System.Linq.Expressions.Expression.Parameter(typeof(object), "instance");
            var value = System.Linq.Expressions.Expression.Parameter(typeof(object), "value");

            // value as T is slightly faster than (T)value, so if it's not a value type, use that
#if !WINRT
            UnaryExpression instanceCast = (!this.Property.DeclaringType.IsValueType) ? System.Linq.Expressions.Expression.TypeAs(instance, this.Property.DeclaringType) : System.Linq.Expressions.Expression.Convert(instance, this.Property.DeclaringType);
            UnaryExpression valueCast = (!this.Property.PropertyType.IsValueType) ? System.Linq.Expressions.Expression.TypeAs(value, this.Property.PropertyType) : System.Linq.Expressions.Expression.Convert(value, this.Property.PropertyType);
#else
            UnaryExpression instanceCast = (!this.Property.DeclaringType.IsValueType()) ? System.Linq.Expressions.Expression.TypeAs(instance, this.Property.DeclaringType) : System.Linq.Expressions.Expression.Convert(instance, this.Property.DeclaringType);
            UnaryExpression valueCast = (!this.Property.PropertyType.IsValueType()) ? System.Linq.Expressions.Expression.TypeAs(value, this.Property.PropertyType) : System.Linq.Expressions.Expression.Convert(value, this.Property.PropertyType);
#endif
            this.SetDelegate = System.Linq.Expressions.Expression.Lambda<Action<object, object>>(System.Linq.Expressions.Expression.Call(instanceCast, this.Property.GetSetMethod(), valueCast), new ParameterExpression[] { instance, value }).Compile();
        }

        /// <summary>
        /// Initializes the get.
        /// </summary>
        private void InitializeGet()
        {
            var instance = System.Linq.Expressions.Expression.Parameter(typeof(object), "instance");
#if !WINRT
            UnaryExpression instanceCast = (!this.Property.DeclaringType.IsValueType) ? System.Linq.Expressions.Expression.TypeAs(instance, this.Property.DeclaringType) : System.Linq.Expressions.Expression.Convert(instance, this.Property.DeclaringType);
#else
            UnaryExpression instanceCast = (!this.Property.DeclaringType.IsValueType()) ? System.Linq.Expressions.Expression.TypeAs(instance, this.Property.DeclaringType) : System.Linq.Expressions.Expression.Convert(instance, this.Property.DeclaringType);
#endif
            this.GetDelegate = System.Linq.Expressions.Expression.Lambda<Func<object, object>>(System.Linq.Expressions.Expression.TypeAs(System.Linq.Expressions.Expression.Call(instanceCast, this.Property.GetGetMethod()), typeof(object)), instance).Compile();
        }

        /// <summary>
        /// Gets the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public object Get(object instance)
        {
            return this.GetDelegate(instance);
        }

        /// <summary>
        /// Sets the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        public void Set(object instance, object value)
        {
            this.SetDelegate(instance, value);
        }
    }
    #endregion
}
