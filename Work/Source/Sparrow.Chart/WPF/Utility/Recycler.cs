using System.Collections.Generic;
using System.Linq;
using System.Windows;
#if !WINRT
using System.Windows.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif


namespace Sparrow.Chart
{
    /// <summary>
    /// http://chriscavanagh.wordpress.com/2009/02/27/elementrecycler-for-silverlight-and-wpf/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ElementRecycler<T> where T : UIElement, new()
    {
        private Panel panel;
        private Stack<T> unused = new Stack<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementRecycler{T}"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
        public ElementRecycler(Panel panel)
        {
            this.panel = panel;
        }

        /// <summary>
        /// Recycles the children.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public IEnumerable<T> RecycleChildren(int count)
        {
            return RecycleChildren(panel, count, unused).ToArray();
        }

        /// <summary>
        /// Recycles the children.
        /// </summary>
        /// <param name="panel">The panel.</param>
        /// <param name="count">The count.</param>
        /// <param name="unused">The unused.</param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<T> RecycleChildren(Panel panel, int count, Stack<T> unused)
        {
            var elementEnum = panel.Children.OfType<T>().ToArray().AsEnumerable().GetEnumerator();

            while (count-- > 0)
            {
                if (elementEnum.MoveNext())
                {
                    yield return elementEnum.Current;
                }
                else if (unused.Count > 0)
                {
                    var recycled = unused.Pop();
                    panel.Children.Add(recycled);
                    yield return recycled;
                }
                else
                {
                    var element = new T();
                    panel.Children.Add(element);

                    yield return element;
                }
            }

            while (elementEnum.MoveNext())
            {
                panel.Children.Remove(elementEnum.Current);
                unused.Push(elementEnum.Current);
            }
        }
    }
}
