﻿// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GreenBox3D.ContentPipeline.Writers
{
    public class ContentWriter : BinaryWriter
    {
        #region Constructors and Destructors

        internal ContentWriter(Stream stream, Encoding encoding)
            : base(stream, encoding, true)
        {
        }

        #endregion

        #region Public Methods and Operators

        public void Write(Vector2 value)
        {
            Write(value.X);
            Write(value.Y);
        }

        public void Write(Vector3 value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
        }

        public void Write(Vector4 value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
            Write(value.W);
        }

        public void Write(Color value)
        {
            Write(value.PackedValue);
        }

        public void Write(Quaternion value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
            Write(value.W);
        }

        public void Write(Matrix value)
        {
            Write(value.M11);
            Write(value.M12);
            Write(value.M13);
            Write(value.M14);
            Write(value.M21);
            Write(value.M22);
            Write(value.M23);
            Write(value.M24);
            Write(value.M31);
            Write(value.M32);
            Write(value.M33);
            Write(value.M34);
            Write(value.M41);
            Write(value.M42);
            Write(value.M43);
            Write(value.M44);
        }

        public void Write(Point value)
        {
            Write(value.X);
            Write(value.Y);
        }

        public void Write(Rectangle value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Width);
            Write(value.Height);
        }

        public void Write(BoundingBox value)
        {
            Write(value.Min);
            Write(value.Max);
        }

        public void Write(BoundingSphere value)
        {
            Write(value.Center);
            Write(value.Radius);
        }

        #endregion
    }
}