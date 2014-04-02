using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Sparrow.Chart
{
    public class TextStyle
    {
        private Font font;
        public Font Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
            }
        }

        private Brush foreground;
        public Brush Foreground
        {
            get
            {
                return foreground;
            }
            set
            {
                foreground = value;
            }
        }

        private HorizontalAlignment horizontalAlignment;
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return horizontalAlignment;
            }
            set { horizontalAlignment = value; }
        }

        private VerticalAlignment verticalAlignment;
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return verticalAlignment;
            }
            set { verticalAlignment = value; }
        }
    }
}
