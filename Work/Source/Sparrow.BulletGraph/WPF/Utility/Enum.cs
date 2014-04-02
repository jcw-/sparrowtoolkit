using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.BulletGraph
{
    public enum ScalePosition
    {
        Default,
        Cross,
        Opposed
    }

    public enum LabelPosition
    {
        Default,
        Cross,
        Opposed
    }

#if !WPF
    // Summary:
    //     Specifies values that control the behavior of a control positioned inside
    //     another control.
    public enum Dock
    {
        // Summary:
        //     Specifies that the control should be positioned on the left of the control.
        /// <summary>
        /// The left
        /// </summary>
        Left = 0,
        //
        // Summary:
        //     Specifies that the control should be positioned on top of the control.
        /// <summary>
        /// The top
        /// </summary>
        Top = 1,
        //
        // Summary:
        //     Specifies that the control should be positioned on the right of the control.
        /// <summary>
        /// The right
        /// </summary>
        Right = 2,
        //
        // Summary:
        //     Specifies that the control should be positioned at the bottom of the control.
        /// <summary>
        /// The bottom
        /// </summary>
        Bottom = 3,
    }
#endif

}
