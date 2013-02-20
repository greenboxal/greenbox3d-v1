// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;

namespace GreenBox3D
{
    public struct Rectangle : IEquatable<Rectangle>
    {
        #region Static Fields

        private static readonly Rectangle emptyRectangle = new Rectangle();

        #endregion

        #region Fields

        public int Height;
        public int Width;
        public int X;
        public int Y;

        #endregion

        #region Constructors and Destructors

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        #endregion

        #region Public Properties

        public static Rectangle Empty { get { return emptyRectangle; } }
        public int Bottom { get { return (Y + Height); } }

        public Point Center
        {
            get
            {
                // This is incorrect
                //return new Point( (this.X + this.Width) / 2,(this.Y + this.Height) / 2 );
                // What we want is the Center of the rectangle from the X and Y Origins
                return new Point(X + (Width / 2), Y + (Height / 2));
            }
        }

        public bool IsEmpty { get { return ((((Width == 0) && (Height == 0)) && (X == 0)) && (Y == 0)); } }
        public int Left { get { return X; } }

        public Point Location
        {
            get { return new Point(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public int Right { get { return (X + Width); } }
        public int Top { get { return Y; } }

        #endregion

        #region Public Methods and Operators

        public static Rectangle Intersect(Rectangle value1, Rectangle value2)
        {
            Rectangle rectangle;
            Intersect(ref value1, ref value2, out rectangle);
            return rectangle;
        }

        public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
        {
            if (value1.Intersects(value2))
            {
                int right_side = System.Math.Min(value1.X + value1.Width, value2.X + value2.Width);
                int left_side = System.Math.Max(value1.X, value2.X);
                int top_side = System.Math.Max(value1.Y, value2.Y);
                int bottom_side = System.Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
                result = new Rectangle(left_side, top_side, right_side - left_side, bottom_side - top_side);
            }
            else
                result = new Rectangle(0, 0, 0, 0);
        }

        public static Rectangle Union(Rectangle value1, Rectangle value2)
        {
            int x = System.Math.Min(value1.X, value2.X);
            int y = System.Math.Min(value1.Y, value2.Y);
            return new Rectangle(x, y, System.Math.Max(value1.Right, value2.Right) - x, System.Math.Max(value1.Bottom, value2.Bottom) - y);
        }

        public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
        {
            result.X = System.Math.Min(value1.X, value2.X);
            result.Y = System.Math.Min(value1.Y, value2.Y);
            result.Width = System.Math.Max(value1.Right, value2.Right) - result.X;
            result.Height = System.Math.Max(value1.Bottom, value2.Bottom) - result.Y;
        }

        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return ((a.X == b.X) && (a.Y == b.Y) && (a.Width == b.Width) && (a.Height == b.Height));
        }

        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !(a == b);
        }

        public bool Contains(int x, int y)
        {
            return ((((X <= x) && (x < (X + Width))) && (Y <= y)) && (y < (Y + Height)));
        }

        public bool Contains(Point value)
        {
            return ((((X <= value.X) && (value.X < (X + Width))) && (Y <= value.Y)) && (value.Y < (Y + Height)));
        }

        public bool Contains(Rectangle value)
        {
            return ((((X <= value.X) && ((value.X + value.Width) <= (X + Width))) && (Y <= value.Y)) && ((value.Y + value.Height) <= (Y + Height)));
        }

        public bool Equals(Rectangle other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return (obj is Rectangle) ? this == ((Rectangle)obj) : false;
        }

        public override int GetHashCode()
        {
            return (X ^ Y ^ Width ^ Height);
        }

        public void Inflate(int horizontalValue, int verticalValue)
        {
            X -= horizontalValue;
            Y -= verticalValue;
            Width += horizontalValue * 2;
            Height += verticalValue * 2;
        }

        public bool Intersects(Rectangle value)
        {
            return value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;
        }

        public void Intersects(ref Rectangle value, out bool result)
        {
            result = value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;
        }

        public void Offset(Point offset)
        {
            X += offset.X;
            Y += offset.Y;
        }

        public void Offset(int offsetX, int offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1} Width:{2} Height:{3}}}", X, Y, Width, Height);
        }

        #endregion
    }
}