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
    public struct Plane : IEquatable<Plane>
    {
        #region Fields

        public float D;
        public Vector3 Normal;

        #endregion

        #region Constructors and Destructors

        public Plane(Vector4 value)
            : this(new Vector3(value.X, value.Y, value.Z), value.W)
        {
        }

        public Plane(Vector3 normal, float d)
        {
            Normal = normal;
            D = d;
        }

        public Plane(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 ab = b - a;
            Vector3 ac = c - a;

            Vector3 cross = Vector3.Cross(ab, ac);
            Normal = Vector3.Normalize(cross);
            D = -(Vector3.Dot(Normal, a));
        }

        public Plane(float a, float b, float c, float d)
            : this(new Vector3(a, b, c), d)
        {
        }

        #endregion

        #region Public Methods and Operators

        public static Plane Normalize(Plane value)
        {
            Plane ret;
            Normalize(ref value, out ret);
            return ret;
        }

        public static void Normalize(ref Plane value, out Plane result)
        {
            float factor;
            result.Normal = Vector3.Normalize(value.Normal);
            factor = (float)System.Math.Sqrt(result.Normal.X * result.Normal.X + result.Normal.Y * result.Normal.Y + result.Normal.Z * result.Normal.Z) / (float)System.Math.Sqrt(value.Normal.X * value.Normal.X + value.Normal.Y * value.Normal.Y + value.Normal.Z * value.Normal.Z);
            result.D = value.D * factor;
        }

        public static void Transform(ref Plane plane, ref Quaternion rotation, out Plane result)
        {
            throw new NotImplementedException();
        }

        public static void Transform(ref Plane plane, ref Matrix matrix, out Plane result)
        {
            throw new NotImplementedException();
        }

        public static Plane Transform(Plane plane, Quaternion rotation)
        {
            throw new NotImplementedException();
        }

        public static Plane Transform(Plane plane, Matrix matrix)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(Plane plane1, Plane plane2)
        {
            return plane1.Equals(plane2);
        }

        public static bool operator !=(Plane plane1, Plane plane2)
        {
            return !plane1.Equals(plane2);
        }

        public float Dot(Vector4 value)
        {
            return ((((Normal.X * value.X) + (Normal.Y * value.Y)) + (Normal.Z * value.Z)) + (D * value.W));
        }

        public void Dot(ref Vector4 value, out float result)
        {
            result = (((Normal.X * value.X) + (Normal.Y * value.Y)) + (Normal.Z * value.Z)) + (D * value.W);
        }

        public float DotCoordinate(Vector3 value)
        {
            return ((((Normal.X * value.X) + (Normal.Y * value.Y)) + (Normal.Z * value.Z)) + D);
        }

        public void DotCoordinate(ref Vector3 value, out float result)
        {
            result = (((Normal.X * value.X) + (Normal.Y * value.Y)) + (Normal.Z * value.Z)) + D;
        }

        public float DotNormal(Vector3 value)
        {
            return (((Normal.X * value.X) + (Normal.Y * value.Y)) + (Normal.Z * value.Z));
        }

        public void DotNormal(ref Vector3 value, out float result)
        {
            result = ((Normal.X * value.X) + (Normal.Y * value.Y)) + (Normal.Z * value.Z);
        }

        public override bool Equals(object other)
        {
            return (other is Plane) ? Equals((Plane)other) : false;
        }

        public bool Equals(Plane other)
        {
            return ((Normal == other.Normal) && (D == other.D));
        }

        public override int GetHashCode()
        {
            return Normal.GetHashCode() ^ D.GetHashCode();
        }

        public PlaneIntersectionType Intersects(BoundingBox box)
        {
            return box.Intersects(this);
        }

        public void Intersects(ref BoundingBox box, out PlaneIntersectionType result)
        {
            result = Intersects(box);
        }

        public PlaneIntersectionType Intersects(BoundingFrustum frustum)
        {
            return frustum.Intersects(this);
        }

        public PlaneIntersectionType Intersects(BoundingSphere sphere)
        {
            return sphere.Intersects(this);
        }

        public void Intersects(ref BoundingSphere sphere, out PlaneIntersectionType result)
        {
            result = Intersects(sphere);
        }

        public void Normalize()
        {
            float factor;
            Vector3 normal = Normal;
            Normal = Vector3.Normalize(Normal);
            factor = (float)System.Math.Sqrt(Normal.X * Normal.X + Normal.Y * Normal.Y + Normal.Z * Normal.Z) / (float)System.Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);
            D = D * factor;
        }

        public override string ToString()
        {
            return string.Format("{{Normal:{0} D:{1}}}", Normal, D);
        }

        #endregion
    }
}