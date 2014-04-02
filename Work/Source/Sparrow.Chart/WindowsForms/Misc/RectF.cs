using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    public struct RectF
    {
        internal float x;
        internal float y;
        internal float width;
        internal float height;

        private static readonly RectF empty = RectF.CreateEmptyRectF();

        private static RectF CreateEmptyRectF()
        {
            return new RectF()
            {
                x = float.PositiveInfinity,
                y = float.PositiveInfinity,
                width = float.NegativeInfinity,
                height = float.NegativeInfinity
            };
        }

        public bool IntersectsWith(RectF rect)
        {
            if (this.IsEmpty || rect.IsEmpty || (rect.Left > this.Right || rect.Right < this.Left) || rect.Top > this.Bottom)
                return false;
            else
                return rect.Bottom >= this.Top;
        }

        public bool Contains(RectF rect)
        {
            if (this.IsEmpty || rect.IsEmpty || (this.x > rect.x || this.y > rect.y) || this.x + this.width < rect.x + rect.width)
                return false;
            else
                return this.y + this.height >= rect.y + rect.height;
        }

        public bool Contains(PointF point)
        {
            return this.Contains(point.X, point.Y);
        }

        public bool Contains(float x, float y)
        {
            if (this.IsEmpty)
                return false;
            else
                return this.ContainsInternal(x, y);
        }

        private bool ContainsInternal(float x, float y)
        {
            if (x >= this.x && x - this.width <= this.x && y >= this.y)
                return y - this.height <= this.y;
            else
                return false;
        }

        public RectF(PointF location, SizeF size)
        {
            if (size.IsEmpty)
            {
                this = RectF.empty;
            }
            else
            {
                this.x = location.X;
                this.y = location.Y;
                this.width = size.Width;
                this.height = size.Height;
            }
        }

        public RectF(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public RectF(PointF point1, PointF point2)
        {
            this.x = (float) Math.Min(point1.X, point2.X);
            this.y = (float) Math.Min(point1.Y, point2.Y);
            this.width = (float) Math.Max(Math.Max(point1.X, point2.X) - this.x, 0.0);
            this.height = (float) Math.Max(Math.Max(point1.Y, point2.Y) - this.y, 0.0);
        }

        public bool IsEmpty
        {
            get
            {
                return this.width < 0.0;
            }
        }
        public PointF Location
        {
            get
            {
                return new PointF(this.x, this.y);
            }
            set
            {
                if (this.IsEmpty)
                    throw new InvalidOperationException("Cannot set for empty rect");
                this.x = value.X;
                this.y = value.Y;
            }
        }

        public SizeF Size
        {
            get
            {
                if (this.IsEmpty)
                    return SizeF.Empty;
                else
                    return new SizeF(this.width, this.height);
            }
            set
            {
                if (value.IsEmpty)
                {
                    this = RectF.empty;
                }
                else
                {
                    if (this.IsEmpty)
                        throw new InvalidOperationException("Cannot set for empty rect");
                    this.width = value.Width;
                    this.height = value.Height;
                }
            }
        }

        public float X
        {            
            get
            {
                return this.x;
            }
            set
            {                
                this.x = value;
            }
        }

        public float Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        public float Width
        {           
            get
            {
                return this.width;
            }
            set
            {               
                this.width = value;
            }
        }

        public float Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        public float Left
        {            
            get
            {
                return this.x;
            }
        }
        public float Top
        {           
            get
            {
                return this.y;
            }
        }

        public float Right
        {
            get
            {
                if (this.IsEmpty)
                    return float.NegativeInfinity;
                else
                    return this.x + this.width;
            }
        }

        public float Bottom
        {
            get
            {
                if (this.IsEmpty)
                    return float.NegativeInfinity;
                else
                    return this.y + this.height;
            }
        }

        public PointF TopRight
        {
            get
            {
                return new PointF(this.Right, this.Top);
            }
        }

        public PointF BottomLeft
        {
            get
            {
                return new PointF(this.Left, this.Bottom);
            }
        }

        public PointF BottomRight
        {
            get
            {
                return new PointF(this.Right, this.Bottom);
            }
        }
    }
}
