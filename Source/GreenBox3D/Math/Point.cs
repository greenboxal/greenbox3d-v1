// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;

namespace GreenBox3D.Math
{
    public struct Point : IEquatable<Point>
    {
        #region Static Fields

        private static readonly Point zeroPoint = new Point();

        #endregion

        #region Fields

        public int X;
        public int Y;

        #endregion

        #region Constructors and Destructors

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Public Properties

        public static Point Zero { get { return zeroPoint; } }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b)
        {
            return !a.Equals(b);
        }

        public bool Equals(Point other)
        {
            return ((X == other.X) && (Y == other.Y));
        }

        public override bool Equals(object obj)
        {
            return (obj is Point) && Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1}}}", X, Y);
        }

        #endregion
    }
}