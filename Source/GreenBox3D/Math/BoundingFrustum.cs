// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Text;

namespace GreenBox3D.Math
{
    public class BoundingFrustum : IEquatable<BoundingFrustum>
    {
        #region Constants

        public const int CornerCount = 8;

        #endregion

        #region Fields

        private Plane _Bottom;
        private Vector3[] _Corners;
        private Plane _Far;
        private Plane _Left;
        private Matrix _Matrix;
        private Plane _Near;
        private Plane _Right;
        private Plane _Top;

        #endregion

        #region Constructors and Destructors

        public BoundingFrustum(Matrix value)
        {
            _Matrix = value;
            CreatePlanes();
            CreateCorners();
        }

        #endregion

        #region Public Properties

        public Plane Bottom { get { return _Bottom; } }
        public Plane Far { get { return _Far; } }
        public Plane Left { get { return _Left; } }

        public Matrix Matrix
        {
            get { return _Matrix; }
            set
            {
                _Matrix = value;
                CreatePlanes(); // FIXME: The odds are the planes will be used a lot more often than the matrix
                CreateCorners(); // is updated, so this should help performance. I hope ;)
            }
        }

        public Plane Near { get { return _Near; } }
        public Plane Right { get { return _Right; } }
        public Plane Top { get { return _Top; } }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(BoundingFrustum a, BoundingFrustum b)
        {
            if (Equals(a, null))
                return (Equals(b, null));

            if (Equals(b, null))
                return (Equals(a, null));

            return a._Matrix == (b._Matrix);
        }

        public static bool operator !=(BoundingFrustum a, BoundingFrustum b)
        {
            return !(a == b);
        }

        public ContainmentType Contains(BoundingBox box)
        {
            ContainmentType result;
            Contains(ref box, out result);
            return result;
        }

        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            var intersects = false;

            PlaneIntersectionType type;
            box.Intersects(ref _Near, out type);
            if (type == PlaneIntersectionType.Front)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (type == PlaneIntersectionType.Intersecting)
                intersects = true;

            box.Intersects(ref _Left, out type);
            if (type == PlaneIntersectionType.Front)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (type == PlaneIntersectionType.Intersecting)
                intersects = true;

            box.Intersects(ref _Right, out type);
            if (type == PlaneIntersectionType.Front)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (type == PlaneIntersectionType.Intersecting)
                intersects = true;

            box.Intersects(ref _Top, out type);
            if (type == PlaneIntersectionType.Front)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (type == PlaneIntersectionType.Intersecting)
                intersects = true;

            box.Intersects(ref _Bottom, out type);
            if (type == PlaneIntersectionType.Front)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (type == PlaneIntersectionType.Intersecting)
                intersects = true;

            box.Intersects(ref _Far, out type);
            if (type == PlaneIntersectionType.Front)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (type == PlaneIntersectionType.Intersecting)
                intersects = true;

            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        // TODO: Implement this
        public ContainmentType Contains(BoundingFrustum frustum)
        {
            if (this == frustum) // We check to see if the two frustums are equal
                return ContainmentType.Contains; // If they are, there's no need to go any further.

            throw new NotImplementedException();
        }

        public ContainmentType Contains(BoundingSphere sphere)
        {
            ContainmentType result;
            Contains(ref sphere, out result);
            return result;
        }

        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            float dist;
            result = ContainmentType.Contains;

            Vector3.Dot(ref _Bottom.Normal, ref sphere.Center, out dist);
            dist += _Bottom.D;
            if (dist > sphere.Radius)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (System.Math.Abs(dist) < sphere.Radius)
                result = ContainmentType.Intersects;

            Vector3.Dot(ref _Top.Normal, ref sphere.Center, out dist);
            dist += _Top.D;
            if (dist > sphere.Radius)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (System.Math.Abs(dist) < sphere.Radius)
                result = ContainmentType.Intersects;

            Vector3.Dot(ref _Near.Normal, ref sphere.Center, out dist);
            dist += _Near.D;
            if (dist > sphere.Radius)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (System.Math.Abs(dist) < sphere.Radius)
                result = ContainmentType.Intersects;

            Vector3.Dot(ref _Far.Normal, ref sphere.Center, out dist);
            dist += _Far.D;
            if (dist > sphere.Radius)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (System.Math.Abs(dist) < sphere.Radius)
                result = ContainmentType.Intersects;

            Vector3.Dot(ref _Left.Normal, ref sphere.Center, out dist);
            dist += _Left.D;
            if (dist > sphere.Radius)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (System.Math.Abs(dist) < sphere.Radius)
                result = ContainmentType.Intersects;

            Vector3.Dot(ref _Right.Normal, ref sphere.Center, out dist);
            dist += _Right.D;
            if (dist > sphere.Radius)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (System.Math.Abs(dist) < sphere.Radius)
                result = ContainmentType.Intersects;
        }

        public ContainmentType Contains(Vector3 point)
        {
            ContainmentType result;
            Contains(ref point, out result);
            return result;
        }

        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            float val;
            // If a point is on the POSITIVE side of the plane, then the point is not contained within the frustum

            // Check the top
            val = PlaneHelper.ClassifyPoint(ref point, ref _Top);
            if (val > 0)
            {
                result = ContainmentType.Disjoint;
                return;
            }

            // Check the bottom
            val = PlaneHelper.ClassifyPoint(ref point, ref _Bottom);
            if (val > 0)
            {
                result = ContainmentType.Disjoint;
                return;
            }

            // Check the left
            val = PlaneHelper.ClassifyPoint(ref point, ref _Left);
            if (val > 0)
            {
                result = ContainmentType.Disjoint;
                return;
            }

            // Check the right
            val = PlaneHelper.ClassifyPoint(ref point, ref _Right);
            if (val > 0)
            {
                result = ContainmentType.Disjoint;
                return;
            }

            // Check the near
            val = PlaneHelper.ClassifyPoint(ref point, ref _Near);
            if (val > 0)
            {
                result = ContainmentType.Disjoint;
                return;
            }

            // Check the far
            val = PlaneHelper.ClassifyPoint(ref point, ref _Far);
            if (val > 0)
            {
                result = ContainmentType.Disjoint;
                return;
            }

            // If we get here, it means that the point was on the correct side of each plane to be
            // contained. Therefore this point is contained
            result = ContainmentType.Contains;
        }

        public bool Equals(BoundingFrustum other)
        {
            return (this == other);
        }

        public override bool Equals(object obj)
        {
            BoundingFrustum f = obj as BoundingFrustum;
            return (Equals(f, null)) ? false : (this == f);
        }

        public Vector3[] GetCorners()
        {
            return (Vector3[])_Corners.Clone();
        }

        public void GetCorners(Vector3[] corners)
        {
            if (corners == null)
                throw new ArgumentNullException("corners");
            if (corners.Length < 8)
                throw new ArgumentOutOfRangeException("corners");

            _Corners.CopyTo(corners, 0);
        }

        public override int GetHashCode()
        {
            return _Matrix.GetHashCode();
        }

        public bool Intersects(BoundingBox box)
        {
            var result = false;
            Intersects(ref box, out result);
            return result;
        }

        public void Intersects(ref BoundingBox box, out bool result)
        {
            var containment = ContainmentType.Disjoint;
            Contains(ref box, out containment);
            result = containment != ContainmentType.Disjoint;
        }

        public bool Intersects(BoundingFrustum frustum)
        {
            throw new NotImplementedException();
        }

        public bool Intersects(BoundingSphere sphere)
        {
            throw new NotImplementedException();
        }

        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            throw new NotImplementedException();
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {
            throw new NotImplementedException();
        }

        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            throw new NotImplementedException();
        }

        public float? Intersects(Ray ray)
        {
            throw new NotImplementedException();
        }

        public void Intersects(ref Ray ray, out float? result)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(256);
            sb.Append("{Near:");
            sb.Append(_Near.ToString());
            sb.Append(" Far:");
            sb.Append(_Far.ToString());
            sb.Append(" Left:");
            sb.Append(_Left.ToString());
            sb.Append(" Right:");
            sb.Append(_Right.ToString());
            sb.Append(" Top:");
            sb.Append(_Top.ToString());
            sb.Append(" Bottom:");
            sb.Append(_Bottom.ToString());
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region Methods

        private static Vector3 IntersectionPoint(ref Plane a, ref Plane b, ref Plane c)
        {
            // Formula used
            //                d1 ( N2 * N3 ) + d2 ( N3 * N1 ) + d3 ( N1 * N2 )
            //P = 	-------------------------------------------------------------------------
            //                             N1 . ( N2 * N3 )
            //
            // Note: N refers to the normal, d refers to the displacement. '.' means dot product. '*' means cross product

            Vector3 v1, v2, v3;
            float f = -Vector3.Dot(a.Normal, Vector3.Cross(b.Normal, c.Normal));

            v1 = (a.D * (Vector3.Cross(b.Normal, c.Normal)));
            v2 = (b.D * (Vector3.Cross(c.Normal, a.Normal)));
            v3 = (c.D * (Vector3.Cross(a.Normal, b.Normal)));

            Vector3 vec = new Vector3(v1.X + v2.X + v3.X, v1.Y + v2.Y + v3.Y, v1.Z + v2.Z + v3.Z);
            return vec / f;
        }

        private void CreateCorners()
        {
            _Corners = new Vector3[8];
            _Corners[0] = IntersectionPoint(ref _Near, ref _Left, ref _Top);
            _Corners[1] = IntersectionPoint(ref _Near, ref _Right, ref _Top);
            _Corners[2] = IntersectionPoint(ref _Near, ref _Right, ref _Bottom);
            _Corners[3] = IntersectionPoint(ref _Near, ref _Left, ref _Bottom);
            _Corners[4] = IntersectionPoint(ref _Far, ref _Left, ref _Top);
            _Corners[5] = IntersectionPoint(ref _Far, ref _Right, ref _Top);
            _Corners[6] = IntersectionPoint(ref _Far, ref _Right, ref _Bottom);
            _Corners[7] = IntersectionPoint(ref _Far, ref _Left, ref _Bottom);
        }

        private void CreatePlanes()
        {
            // Pre-calculate the different planes needed
            _Left = new Plane(-_Matrix.M14 - _Matrix.M11, -_Matrix.M24 - _Matrix.M21, -_Matrix.M34 - _Matrix.M31, -_Matrix.M44 - _Matrix.M41);

            _Right = new Plane(_Matrix.M11 - _Matrix.M14, _Matrix.M21 - _Matrix.M24, _Matrix.M31 - _Matrix.M34, _Matrix.M41 - _Matrix.M44);

            _Top = new Plane(_Matrix.M12 - _Matrix.M14, _Matrix.M22 - _Matrix.M24, _Matrix.M32 - _Matrix.M34, _Matrix.M42 - _Matrix.M44);

            _Bottom = new Plane(-_Matrix.M14 - _Matrix.M12, -_Matrix.M24 - _Matrix.M22, -_Matrix.M34 - _Matrix.M32, -_Matrix.M44 - _Matrix.M42);

            _Near = new Plane(-_Matrix.M13, -_Matrix.M23, -_Matrix.M33, -_Matrix.M43);

            _Far = new Plane(_Matrix.M13 - _Matrix.M14, _Matrix.M23 - _Matrix.M24, _Matrix.M33 - _Matrix.M34, _Matrix.M43 - _Matrix.M44);

            NormalizePlane(ref _Left);
            NormalizePlane(ref _Right);
            NormalizePlane(ref _Top);
            NormalizePlane(ref _Bottom);
            NormalizePlane(ref _Near);
            NormalizePlane(ref _Far);
        }

        private void NormalizePlane(ref Plane p)
        {
            float factor = 1f / p.Normal.Length();
            p.Normal.X *= factor;
            p.Normal.Y *= factor;
            p.Normal.Z *= factor;
            p.D *= factor;
        }

        #endregion
    }
}