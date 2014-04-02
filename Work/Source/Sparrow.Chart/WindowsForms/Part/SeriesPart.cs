using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public class SeriesPart
    {
        internal Graphics RootGraphics
        {
            get;
            set;
        }

        virtual public void DrawSeriesPart()
        {
           
        }
    }
}
