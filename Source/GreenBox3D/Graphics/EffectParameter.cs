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

        public EffectParameterType Type { get { return Parameter.Type; } }
        public EffectParameterClass Class { get { return Parameter.Class; } }

        public int RowCount { get { return Parameter.RowCount; } }
        public int ColumnCount { get { return Parameter.ColumnCount; } }

        private readonly Effect _owner;

        internal bool Dirty;
        internal ShaderParameter Parameter;
        internal Texture[] Textures;

        internal EffectParameter(Effect owner, ShaderParameter parameter)
        {
            _owner = owner;

            Name = parameter.Name;
            Parameter = parameter;

            Dirty = true;
        }

        internal unsafe void Apply(byte* ptr)
        {
            Parameter.Apply(&ptr[Parameter.Offset]);
            Dirty = false;
        }

        public void SetValue(Texture value)
        {
            if (Class != EffectParameterClass.Sampler)
                throw new InvalidOperationException();

            Textures = new[] { value };
        }

        public unsafe void SetValue(bool value)
        {
            // Bool has 4 bytes on the GPU
            if (sizeof(int) > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                *(bool*)ptr = value;

            Dirty = true;
        }

        public unsafe void SetValue(float value)
        {
            if (sizeof(float) > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                *(float*)ptr = value;

            Dirty = true;
        }

        public unsafe void SetValue(int value)
        {
            if (sizeof(int) > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                *(int*)ptr = value;

            Dirty = true;
        }

        public unsafe void SetValue(double value)
        {
            if (sizeof(double) > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                *(double*)ptr = value;

            Dirty = true;
        }

        public unsafe void SetValue(Vector2 value)
        {
            if (sizeof(Vector2) > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                *(Vector2*)ptr = value;

            Dirty = true;
        }

        public unsafe void SetValue(Vector3 value)
        {
            if (sizeof(Vector3) > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                *(Vector3*)ptr = value;

            Dirty = true;
        }

        public unsafe void SetValue(Vector4 value)
        {
            if (sizeof(Vector4) > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                *(Vector4*)ptr = value;

            Dirty = true;
        }

        public unsafe void SetValue(Matrix value)
        {
            if (sizeof(Matrix) > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                *(Matrix*)ptr = value;

            Dirty = true;
        }

        public unsafe void SetValueTranspose(Matrix value)
        {
            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                *(Matrix*)ptr = Matrix.Transpose(value);

            Dirty = true;
        }

        public void SetValue(Texture[] value)
        {
            if (Class != EffectParameterClass.Sampler)
                throw new InvalidOperationException();

            Textures = (Texture2D[])value.Clone();
        }

        public unsafe void SetValue(bool[] value)
        {
            // Bool has 4 bytes on the GPU
            if (sizeof(int) * value.Length > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                for (int i = 0; i < value.Length; i++)
                    *(bool*)&ptr[i * sizeof(bool)] = value[i];

            Dirty = true;
        }

        public unsafe void SetValue(int[] value)
        {
            if (sizeof(int) * value.Length > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                for (int i = 0; i < value.Length; i++)
                    *(int*)&ptr[i * sizeof(int)] = value[i];

            Dirty = true;
        }

        public unsafe void SetValue(float[] value)
        {
            if (sizeof(float) * value.Length > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                for (int i = 0; i < value.Length; i++)
                    *(float*)&ptr[i * sizeof(float)] = value[i];

            Dirty = true;
        }

        public unsafe void SetValue(double[] value)
        {
            if (sizeof(double) * value.Length > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                for (int i = 0; i < value.Length; i++)
                    *(double*)&ptr[i * sizeof(double)] = value[i];

            Dirty = true;
        }

        public unsafe void SetValue(Vector2[] value)
        {
            if (sizeof(Vector2) * value.Length > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                for (int i = 0; i < value.Length; i++)
                    *(Vector2*)&ptr[i * sizeof(Vector2)] = value[i];

            Dirty = true;
        }

        public unsafe void SetValue(Vector3[] value)
        {
            if (sizeof(Vector3) * value.Length > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                for (int i = 0; i < value.Length; i++)
                    *(Vector3*)&ptr[i * sizeof(Vector3)] = value[i];

            Dirty = true;
        }

        public unsafe void SetValue(Vector4[] value)
        {
            if (sizeof(Vector4) * value.Length > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                for (int i = 0; i < value.Length; i++)
                    *(Vector4*)&ptr[i * sizeof(Vector4)] = value[i];

            Dirty = true;
        }

        public unsafe void SetValue(Matrix[] value)
        {
            if (sizeof(Matrix) * value.Length > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                for (int i = 0; i < value.Length; i++)
                    *(Matrix*)&ptr[i * sizeof(Matrix)] = value[i];

            Dirty = true;
        }

        public unsafe void SetValueTranspose(Matrix[] value)
        {
            if (sizeof(Matrix) * value.Length > Parameter.ByteSize)
                throw new InvalidOperationException();

            fixed (byte* ptr = &_owner.ParameterData[Parameter.Offset])
                for (int i = 0; i < value.Length; i++)
                    *(Matrix*)&ptr[i * sizeof(Matrix)] = Matrix.Transpose(value[i]);

            Dirty = true;
        }
    }
}
