﻿// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;

namespace GreenBox3D
{
    [Serializable]
    public struct BoundingBox : IEquatable<BoundingBox>
    {
        #region Public Fields

#if WINRT
        [DataMember]
#endif
        public Vector3 Min;
#if WINRT
        [DataMember]
#endif
        public Vector3 Max;
        public const int CornerCount = 8;

        #endregion Public Fields

        #region Public Constructors

        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        #endregion Public Constructors

        #region Public Methods

        public ContainmentType Contains(BoundingBox box)
        {
            //test if all corner is in the same side of a face by just checking min and max
            if (box.Max.X < Min.X || box.Min.X > Max.X || box.Max.Y < Min.Y || box.Min.Y > Max.Y || box.Max.Z < Min.Z || box.Min.Z > Max.Z)
                return ContainmentType.Disjoint;

            if (box.Min.X >= Min.X && box.Max.X <= Max.X && box.Min.Y >= Min.Y && box.Max.Y <= Max.Y && box.Min.Z >= Min.Z && box.Max.Z <= Max.Z)
                return ContainmentType.Contains;

            return ContainmentType.Intersects;
        }

        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            result = Contains(box);
        }

        public ContainmentType Contains(BoundingFrustum frustum)
        {
            //TODO: bad done here need a fix. 
            //Because question is not frustum contain box but reverse and this is not the same
            int i;
            ContainmentType contained;
            Vector3[] corners = frustum.GetCorners();

            // First we check if frustum is in box
            for (i = 0; i < corners.Length; i++)
            {
                Contains(ref corners[i], out contained);
                if (contained == ContainmentType.Disjoint)
                    break;
            }

            if (i == corners.Length) // This means we checked all the corners and they were all contain or instersect
                return ContainmentType.Contains;

            if (i != 0) // if i is not equal to zero, we can fastpath and say that this box intersects
                return ContainmentType.Intersects;

            // If we get here, it means the first (and only) point we checked was actually contained in the frustum.
            // So we assume that all other points will also be contained. If one of the points is disjoint, we can
            // exit immediately saying that the result is Intersects
            i++;
            for (; i < corners.Length; i++)
            {
                Contains(ref corners[i], out contained);
                if (contained != ContainmentType.Contains)
                    return ContainmentType.Intersects;
            }

            // If we get here, then we know all the points were actually contained, therefore result is Contains
            return ContainmentType.Contains;
        }

        public ContainmentType Contains(BoundingSphere sphere)
        {
            if (sphere.Center.X - Min.X > sphere.Radius && sphere.Center.Y - Min.Y > sphere.Radius && sphere.Center.Z - Min.Z > sphere.Radius && Max.X - sphere.Center.X > sphere.Radius && Max.Y - sphere.Center.Y > sphere.Radius && Max.Z - sphere.Center.Z > sphere.Radius)
                return ContainmentType.Contains;

            double dmin = 0;

            if (sphere.Center.X - Min.X <= sphere.Radius)
                dmin += (sphere.Center.X - Min.X) * (sphere.Center.X - Min.X);
            else if (Max.X - sphere.Center.X <= sphere.Radius)
                dmin += (sphere.Center.X - Max.X) * (sphere.Center.X - Max.X);
            if (sphere.Center.Y - Min.Y <= sphere.Radius)
                dmin += (sphere.Center.Y - Min.Y) * (sphere.Center.Y - Min.Y);
            else if (Max.Y - sphere.Center.Y <= sphere.Radius)
                dmin += (sphere.Center.Y - Max.Y) * (sphere.Center.Y - Max.Y);
            if (sphere.Center.Z - Min.Z <= sphere.Radius)
                dmin += (sphere.Center.Z - Min.Z) * (sphere.Center.Z - Min.Z);
            else if (Max.Z - sphere.Center.Z <= sphere.Radius)
                dmin += (sphere.Center.Z - Max.Z) * (sphere.Center.Z - Max.Z);

            if (dmin <= sphere.Radius * sphere.Radius)
                return ContainmentType.Intersects;

            return ContainmentType.Disjoint;
        }

        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            result = Contains(sphere);
        }

        public ContainmentType Contains(Vector3 point)
        {
            ContainmentType result;
            Contains(ref point, out result);
            return result;
        }

        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            //first we get if point is out of box
            if (point.X < Min.X || point.X > Max.X || point.Y < Min.Y || point.Y > Max.Y || point.Z < Min.Z || point.Z > Max.Z)
                result = ContainmentType.Disjoint;
            else if (point.X == Min.X || point.X == Max.X || point.Y == Min.Y || point.Y == Max.Y || point.Z == Min.Z || point.Z == Max.Z)
                result = ContainmentType.Intersects;
            else
                result = ContainmentType.Contains;
        }

        public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
        {
            if (points == null)
                throw new ArgumentNullException();

            // TODO: Just check that Count > 0
            bool empty = true;
            Vector3 vector2 = new Vector3(float.MaxValue);
            Vector3 vector1 = new Vector3(float.MinValue);
            foreach (Vector3 vector3 in points)
            {
                vector2 = Vector3.Min(vector2, vector3);
                vector1 = Vector3.Max(vector1, vector3);
                empty = false;
            }
            if (empty)
                throw new ArgumentException();

            return new BoundingBox(vector2, vector1);
        }

        public static BoundingBox CreateFromSphere(BoundingSphere sphere)
        {
            Vector3 vector1 = new Vector3(sphere.Radius);
            return new BoundingBox(sphere.Center - vector1, sphere.Center + vector1);
        }

        public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
        {
            result = CreateFromSphere(sphere);
        }

        public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
        {
            return new BoundingBox(Vector3.Min(original.Min, additional.Min), Vector3.Max(original.Max, additional.Max));
        }

        public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
        {
            result = CreateMerged(original, additional);
        }

        public bool Equals(BoundingBox other)
        {
            return (Min == other.Min) && (Max == other.Max);
        }

        public override bool Equals(object obj)
        {
            return (obj is BoundingBox) ? Equals((BoundingBox)obj) : false;
        }

        public Vector3[] GetCorners()
        {
            return new[] { new Vector3(Min.X, Max.Y, Max.Z), new Vector3(Max.X, Max.Y, Max.Z), new Vector3(Max.X, Min.Y, Max.Z), new Vector3(Min.X, Min.Y, Max.Z), new Vector3(Min.X, Max.Y, Min.Z), new Vector3(Max.X, Max.Y, Min.Z), new Vector3(Max.X, Min.Y, Min.Z), new Vector3(Min.X, Min.Y, Min.Z) };
        }

        public void GetCorners(Vector3[] corners)
        {
            if (corners == null)
                throw new ArgumentNullException("corners");
            if (corners.Length < 8)
                throw new ArgumentOutOfRangeException("corners", "Not Enought Corners");
            corners[0].X = Min.X;
            corners[0].Y = Max.Y;
            corners[0].Z = Max.Z;
            corners[1].X = Max.X;
            corners[1].Y = Max.Y;
            corners[1].Z = Max.Z;
            corners[2].X = Max.X;
            corners[2].Y = Min.Y;
            corners[2].Z = Max.Z;
            corners[3].X = Min.X;
            corners[3].Y = Min.Y;
            corners[3].Z = Max.Z;
            corners[4].X = Min.X;
            corners[4].Y = Max.Y;
            corners[4].Z = Min.Z;
            corners[5].X = Max.X;
            corners[5].Y = Max.Y;
            corners[5].Z = Min.Z;
            corners[6].X = Max.X;
            corners[6].Y = Min.Y;
            corners[6].Z = Min.Z;
            corners[7].X = Min.X;
            corners[7].Y = Min.Y;
            corners[7].Z = Min.Z;
        }

        public override int GetHashCode()
        {
            return Min.GetHashCode() + Max.GetHashCode();
        }

        public bool Intersects(BoundingBox box)
        {
            bool result;
            Intersects(ref box, out result);
            return result;
        }

        public void Intersects(ref BoundingBox box, out bool result)
        {
            if ((Max.X >= box.Min.X) && (Min.X <= box.Max.X))
            {
                if ((Max.Y < box.Min.Y) || (Min.Y > box.Max.Y))
                {
                    result = false;
                    return;
                }

                result = (Max.Z >= box.Min.Z) && (Min.Z <= box.Max.Z);
                return;
            }

            result = false;
            return;
        }

        public bool Intersects(BoundingFrustum frustum)
        {
            return frustum.Intersects(this);
        }

        public bool Intersects(BoundingSphere sphere)
        {
            if (sphere.Center.X - Min.X > sphere.Radius && sphere.Center.Y - Min.Y > sphere.Radius && sphere.Center.Z - Min.Z > sphere.Radius && Max.X - sphere.Center.X > sphere.Radius && Max.Y - sphere.Center.Y > sphere.Radius && Max.Z - sphere.Center.Z > sphere.Radius)
                return true;

            double dmin = 0;

            if (sphere.Center.X - Min.X <= sphere.Radius)
                dmin += (sphere.Center.X - Min.X) * (sphere.Center.X - Min.X);
            else if (Max.X - sphere.Center.X <= sphere.Radius)
                dmin += (sphere.Center.X - Max.X) * (sphere.Center.X - Max.X);

            if (sphere.Center.Y - Min.Y <= sphere.Radius)
                dmin += (sphere.Center.Y - Min.Y) * (sphere.Center.Y - Min.Y);
            else if (Max.Y - sphere.Center.Y <= sphere.Radius)
                dmin += (sphere.Center.Y - Max.Y) * (sphere.Center.Y - Max.Y);

            if (sphere.Center.Z - Min.Z <= sphere.Radius)
                dmin += (sphere.Center.Z - Min.Z) * (sphere.Center.Z - Min.Z);
            else if (Max.Z - sphere.Center.Z <= sphere.Radius)
                dmin += (sphere.Center.Z - Max.Z) * (sphere.Center.Z - Max.Z);

            if (dmin <= sphere.Radius * sphere.Radius)
                return true;

            return false;
        }

        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            result = Intersects(sphere);
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {
            PlaneIntersectionType result;
            Intersects(ref plane, out result);
            return result;
        }

        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            // See http://zach.in.tu-clausthal.de/teaching/cg_literatur/lighthouse3d_view_frustum_culling/index.html

            Vector3 positiveVertex;
            Vector3 negativeVertex;

            if (plane.Normal.X >= 0)
            {
                positiveVertex.X = Max.X;
                negativeVertex.X = Min.X;
            }
            else
            {
                positiveVertex.X = Min.X;
                negativeVertex.X = Max.X;
            }

            if (plane.Normal.Y >= 0)
            {
                positiveVertex.Y = Max.Y;
                negativeVertex.Y = Min.Y;
            }
            else
            {
                positiveVertex.Y = Min.Y;
                negativeVertex.Y = Max.Y;
            }

            if (plane.Normal.Z >= 0)
            {
                positiveVertex.Z = Max.Z;
                negativeVertex.Z = Min.Z;
            }
            else
            {
                positiveVertex.Z = Min.Z;
                negativeVertex.Z = Max.Z;
            }

            var distance = Vector3.Dot(plane.Normal, negativeVertex) + plane.D;
            if (distance > 0)
            {
                result = PlaneIntersectionType.Front;
                return;
            }

            distance = Vector3.Dot(plane.Normal, positiveVertex) + plane.D;
            if (distance < 0)
            {
                result = PlaneIntersectionType.Back;
                return;
            }

            result = PlaneIntersectionType.Intersecting;
        }

        public float? Intersects(Ray ray)
        {
            return ray.Intersects(this);
        }

        public void Intersects(ref Ray ray, out float? result)
        {
            result = Intersects(ray);
        }

        public static bool operator ==(BoundingBox a, BoundingBox b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(BoundingBox a, BoundingBox b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            return string.Format("{{Min:{0} Max:{1}}}", Min.ToString(), Max.ToString());
        }

        #endregion Public Methods
    }
}