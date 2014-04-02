using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Resources
{
    public class ClosingWindowEventHandlerArgs : EventArgs
    {
        public bool Cancelled { get; set; }
    }
}
