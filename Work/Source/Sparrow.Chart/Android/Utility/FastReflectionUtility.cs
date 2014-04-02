using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Sparrow.Chart
{    
    /// <summary>
    /// http://geekswithblogs.net/Madman/archive/2008/06/27/faster-reflection-using-expression-trees.aspx
    /// </summary>
    public class FastProperty
    {
        /// <summary>
        /// 
        /// </summary>
        public PropertyInfo Property { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Func<object, object> GetDelegate;
        /// <summary>
        /// 
        /// </summary>
        public Action<object, object> SetDelegate;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        public FastProperty(PropertyInfo property)
        {
            this.Property = property;
            InitializeGet();
            InitializeSet();
        }

        private void InitializeSet()
        {
            var instance = System.Linq.Expressions.Expression.Parameter(typeof(object), "instance");
            var value = System.Linq.Expressions.Expression.Parameter(typeof(object), "value");

            // value as T is slightly faster than (T)value, so if it's not a value type, use that

            UnaryExpression instanceCast = (!true) ? System.Linq.Expressions.Expression.TypeAs(instance, this.Property.DeclaringType) : System.Linq.Expressions.Expression.Convert(instance, this.Property.DeclaringType);
            UnaryExpression valueCast = (!true) ? System.Linq.Expressions.Expression.TypeAs(value, this.Property.PropertyType) : System.Linq.Expressions.Expression.Convert(value, this.Property.PropertyType);
            this.SetDelegate = System.Linq.Expressions.Expression.Lambda<Action<object, object>>(System.Linq.Expressions.Expression.Call(instanceCast, this.Property.GetSetMethod(), valueCast), new ParameterExpression[] { instance, value }).Compile();
        }

        private void InitializeGet()
        {
            var instance = System.Linq.Expressions.Expression.Parameter(typeof(object), "instance");
			UnaryExpression instanceCast = (!true) ? System.Linq.Expressions.Expression.TypeAs(instance, this.Property.DeclaringType) : System.Linq.Expressions.Expression.Convert(instance, this.Property.DeclaringType);
            this.GetDelegate = System.Linq.Expressions.Expression.Lambda<Func<object, object>>(System.Linq.Expressions.Expression.TypeAs(System.Linq.Expressions.Expression.Call(instanceCast, this.Property.GetGetMethod()), typeof(object)), instance).Compile();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public object Get(object instance)
        {
            return this.GetDelegate(instance);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void Set(object instance, object value)
        {
            this.SetDelegate(instance, value);
        }
    }
}
