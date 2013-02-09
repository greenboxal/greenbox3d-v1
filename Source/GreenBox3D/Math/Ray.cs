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
    [Serializable]
    public struct Ray : IEquatable<Ray>
    {
        #region Fields

        public Vector3 Direction;
        public Vector3 Position;

        #endregion

        #region Constructors and Destructors

        public Ray(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(Ray a, Ray b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Ray a, Ray b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            return (obj is Ray) ? Equals((Ray)obj) : false;
        }

        public bool Equals(Ray other)
        {
            return Position.Equals(other.Position) && Direction.Equals(other.Direction);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Direction.GetHashCode();
        }

        public float? Intersects(BoundingBox box)
        {
            //first test if start in box
            if (Position.X >= box.Min.X && Position.X <= box.Max.X && Position.Y >= box.Min.Y && Position.Y <= box.Max.Y && Position.Z >= box.Min.Z && Position.Z <= box.Max.Z)
                return 0.0f; // here we concidere cube is full and origine is in cube so intersect at origine

            //Second we check each face
            Vector3 maxT = new Vector3(-1.0f);
            //Vector3 minT = new Vector3(-1.0f);
            //calcul intersection with each faces
            if (Position.X < box.Min.X && Direction.X != 0.0f)
                maxT.X = (box.Min.X - Position.X) / Direction.X;
            else if (Position.X > box.Max.X && Direction.X != 0.0f)
                maxT.X = (box.Max.X - Position.X) / Direction.X;
            if (Position.Y < box.Min.Y && Direction.Y != 0.0f)
                maxT.Y = (box.Min.Y - Position.Y) / Direction.Y;
            else if (Position.Y > box.Max.Y && Direction.Y != 0.0f)
                maxT.Y = (box.Max.Y - Position.Y) / Direction.Y;
            if (Position.Z < box.Min.Z && Direction.Z != 0.0f)
                maxT.Z = (box.Min.Z - Position.Z) / Direction.Z;
            else if (Position.Z > box.Max.Z && Direction.Z != 0.0f)
                maxT.Z = (box.Max.Z - Position.Z) / Direction.Z;

            //get the maximum maxT
            if (maxT.X > maxT.Y && maxT.X > maxT.Z)
            {
                if (maxT.X < 0.0f)
                    return null; // ray go on opposite of face
                //coordonate of hit point of face of cube
                float coord = Position.Z + maxT.X * Direction.Z;
                // if hit point coord ( intersect face with ray) is out of other plane coord it miss 
                if (coord < box.Min.Z || coord > box.Max.Z)
                    return null;
                coord = Position.Y + maxT.X * Direction.Y;
                if (coord < box.Min.Y || coord > box.Max.Y)
                    return null;
                return maxT.X;
            }
            if (maxT.Y > maxT.X && maxT.Y > maxT.Z)
            {
                if (maxT.Y < 0.0f)
                    return null; // ray go on opposite of face
                //coordonate of hit point of face of cube
                float coord = Position.Z + maxT.Y * Direction.Z;
                // if hit point coord ( intersect face with ray) is out of other plane coord it miss 
                if (coord < box.Min.Z || coord > box.Max.Z)
                    return null;
                coord = Position.X + maxT.Y * Direction.X;
                if (coord < box.Min.X || coord > box.Max.X)
                    return null;
                return maxT.Y;
            }
            else //Z
            {
                if (maxT.Z < 0.0f)
                    return null; // ray go on opposite of face
                //coordonate of hit point of face of cube
                float coord = Position.X + maxT.Z * Direction.X;
                // if hit point coord ( intersect face with ray) is out of other plane coord it miss 
                if (coord < box.Min.X || coord > box.Max.X)
                    return null;
                coord = Position.Y + maxT.Z * Direction.Y;
                if (coord < box.Min.Y || coord > box.Max.Y)
                    return null;
                return maxT.Z;
            }
        }

        public void Intersects(ref BoundingBox box, out float? result)
        {
            result = Intersects(box);
        }

        public float? Intersects(BoundingFrustum frustum)
        {
            if (frustum == null)
                throw new ArgumentNullException("frustum");

            return frustum.Intersects(this);
        }

        public float? Intersects(BoundingSphere sphere)
        {
            float? result;
            Intersects(ref sphere, out result);
            return result;
        }

        public float? Intersects(Plane plane)
        {
            float? result;
            Intersects(ref plane, out result);
            return result;
        }

        public void Intersects(ref Plane plane, out float? result)
        {
            var den = Vector3.Dot(Direction, plane.Normal);
            if (System.Math.Abs(den) < 0.00001f)
            {
                result = null;
                return;
            }

            result = (-plane.D - Vector3.Dot(plane.Normal, Position)) / den;

            if (result < 0.0f)
            {
                if (result < -0.00001f)
                {
                    result = null;
                    return;
                }

                result = 0.0f;
            }
        }

        public void Intersects(ref BoundingSphere sphere, out float? result)
        {
            // Find the vector between where the ray starts the the sphere's centre
            Vector3 difference = sphere.Center - Position;

            float differenceLengthSquared = difference.LengthSquared();
            float sphereRadiusSquared = sphere.Radius * sphere.Radius;

            float distanceAlongRay;

            // If the distance between the ray start and the sphere's centre is less than
            // the radius of the sphere, it means we've intersected. N.B. checking the LengthSquared is faster.
            if (differenceLengthSquared < sphereRadiusSquared)
            {
                result = 0.0f;
                return;
            }

            Vector3.Dot(ref Direction, ref difference, out distanceAlongRay);
            // If the ray is pointing away from the sphere then we don't ever intersect
            if (distanceAlongRay < 0)
            {
                result = null;
                return;
            }

            // Next we kinda use Pythagoras to check if we are within the bounds of the sphere
            // if x = radius of sphere
            // if y = distance between ray position and sphere centre
            // if z = the distance we've travelled along the ray
            // if x^2 + z^2 - y^2 < 0, we do not intersect
            float dist = sphereRadiusSquared + distanceAlongRay * distanceAlongRay - differenceLengthSquared;

            result = (dist < 0) ? null : distanceAlongRay - (float?)System.Math.Sqrt(dist);
        }

        public override string ToString()
        {
            return string.Format("{{Position:{0} Direction:{1}}}", Position.ToString(), Direction.ToString());
        }

        #endregion
    }
}