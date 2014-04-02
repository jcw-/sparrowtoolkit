using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Sparrow.Resources
{
    public class SparrowButton : Button
    {
        static SparrowButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SparrowButton), new FrameworkPropertyMetadata(typeof(SparrowButton)));
        }
    }
}
