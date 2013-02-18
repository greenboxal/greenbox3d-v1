using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics.Shading;
using GreenBox3D.Math;

namespace GreenBox3D.Graphics
{
    public class EffectParameter
    {
        public string Name { get; private set; }
        internal ShaderParameter Parameter { get; private set; }
        internal byte[] Data { get; private set; }
        internal GCHandle Handle { get; private set; }
        internal IntPtr Address { get; private set; }

        internal EffectParameter(ShaderParameter parameter)
        {
            Name = parameter.Name;
            Parameter = parameter;
            Data = new byte[Parameter.ByteSize];
            Handle = GCHandle.Alloc(Data, GCHandleType.Pinned);
            Address = Handle.AddrOfPinnedObject();
        }

        internal void Dispose()
        {
            Handle.Free();
            Data = null;
        }

        public unsafe void SetValue(float v)
        {
            *(float*)Address = v;
        }

        public unsafe void SetValue(double v)
        {
            *(double*)Address = v;
        }

        public unsafe void SetValue(Vector2 v)
        {
            *(Vector2*)Address = v;
        }

        public unsafe void SetValue(Vector3 v)
        {
            *(Vector3*)Address = v;
        }

        public unsafe void SetValue(Vector4 v)
        {
            *(Vector4*)Address = v;
        }

        public unsafe void SetValue(Matrix v)
        {
            *(Matrix*)Address = v;
        }

        public unsafe void SetValueTransposed(Matrix v)
        {
            *(Matrix*)Address = Matrix.Transpose(v);
        }
    }
}
