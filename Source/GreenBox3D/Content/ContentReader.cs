﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Math;

namespace GreenBox3D.Content
{
    public class ContentReader : BinaryReader
    {
        internal ContentReader(Stream input, Encoding encoding)
            : base(input, encoding, true)
        {
        }

        public Vector2 ReadVector2()
        {
            return new Vector2(ReadSingle(), ReadSingle());
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Vector4 ReadVector4()
        {
            return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Color ReadColor()
        {
            return new Color(ReadUInt32());
        }

        public Point ReadPoint()
        {
            return new Point(ReadInt32(), ReadInt32());
        }

        public Rectangle ReadRectangle()
        {
            return new Rectangle(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());
        }

        public Matrix ReadMatrix()
        {
            return new Matrix(
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(),
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(),
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(),
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public BoundingBox ReadBoundingBox()
        {
            return new BoundingBox(ReadVector3(), ReadVector3());
        }

        public BoundingSphere ReadBoundingSphere()
        {
            return new BoundingSphere(ReadVector3(), ReadSingle());
        }
    }
}