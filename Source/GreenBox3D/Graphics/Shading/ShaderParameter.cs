using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics.Shading
{
    internal class ShaderParameter
    {
        public unsafe delegate void ApplyDelegate(byte* ptr);

        public string Name { get; private set; }

        public EffectParameterType Type { get; private set; }
        public EffectParameterClass Class { get; private set; }

        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }

        public int Count { get; private set; }
        public int ByteSize { get; private set; }

        public int Offset { get; set; }
        public int Slot { get; set; }
        public int TextureUnit { get; set; }

        public ApplyDelegate Apply;

        public ShaderParameter(string name, string glslType, int size)
        {
            Name = name;
            Count = size;
            Slot = -1;

            if (glslType.StartsWith("mat") || glslType.Substring(1, 3) == "mat")
                Class = EffectParameterClass.Matrix;
            else if (glslType.StartsWith("vec") || glslType.Substring(1, 3) == "vec")
                Class = EffectParameterClass.Vector;
            else if (glslType.StartsWith("sampler"))
                Class = EffectParameterClass.Sampler;
            else
                Class = EffectParameterClass.Scalar;

            switch (Class)
            {
                case EffectParameterClass.Matrix:
                {
                    char m = glslType[glslType.Length - 1];
                    char n = m;

                    if (glslType[glslType.Length - 2] == 'x')
                        n = glslType[glslType.Length - 3];

                    switch (glslType[0])
                    {
                        case 'm':
                            Type = EffectParameterType.Single;
                            break;
                        case 'd':
                            Type = EffectParameterType.Double;
                            break;
                    }

                    RowCount = (n - '0');
                    ColumnCount = (m - '0');
                }
                    break;
                case EffectParameterClass.Vector:
                    switch (glslType[0])
                    {
                        case 'm':
                            Type = EffectParameterType.Single;
                            size = 4;
                            break;
                        case 'd':
                            Type = EffectParameterType.Double;
                            size = 4;
                            break;
                        case 'i':
                            Type = EffectParameterType.Int32;
                            break;
                        case 'b':
                            Type = EffectParameterType.Bool;
                            break;
                    }

                    RowCount = 1;
                    ColumnCount = glslType[glslType.Length - 1] - '0';
                    break;
                case EffectParameterClass.Sampler:
                    switch (glslType[glslType.Length - 2])
                    {
                        case '1':
                            Type = EffectParameterType.Texture1D;
                            break;
                        case '2':
                            Type = EffectParameterType.Texture2D;
                            break;
                        case '3':
                            Type = EffectParameterType.Texture3D;
                            break;
                        default:
                            if (glslType == "sampleCube")
                                Type = EffectParameterType.TextureCube;
                            else
                                throw new NotSupportedException();
                            break;
                    }
                    RowCount = 1;
                    ColumnCount = 1;
                    break;
                default:
                    switch (glslType)
                    {
                        case "int":
                        case "uint":
                            Type = EffectParameterType.Int32;
                            break;
                        case "bool":
                            Type = EffectParameterType.Bool;
                            break;
                        case "float":
                            Type = EffectParameterType.Single;
                            break;
                        case "double":
                            Type = EffectParameterType.Double;
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    RowCount = 1;
                    ColumnCount = 1;
                    break;
            }

            unsafe
            {
                if (Count == 0)
                {
                    if (RowCount > 1)
                    {
                        switch (Type)
                        {
                            case EffectParameterType.Single:
                                switch (RowCount)
                                {
                                    case 3:
                                        Apply = ApplyMatrix3F;
                                        break;
                                    case 4:
                                        Apply = ApplyMatrix4F;
                                        break;
                                }
                                break;
                            case EffectParameterType.Double:
                                switch (RowCount)
                                {
                                    case 3:
                                        Apply = ApplyMatrix3D;
                                        break;
                                    case 4:
                                        Apply = ApplyMatrix4D;
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (Type)
                        {
                            case EffectParameterType.Bool:
                                switch (ColumnCount)
                                {
                                    case 1:
                                        Apply = Apply1B;
                                        break;
                                    case 2:
                                        Apply = Apply2B;
                                        break;
                                    case 3:
                                        Apply = Apply3B;
                                        break;
                                    case 4:
                                        Apply = Apply4B;
                                        break;
                                }
                                break;
                            case EffectParameterType.Texture1D:
                            case EffectParameterType.Texture2D:
                            case EffectParameterType.Texture3D:
                            case EffectParameterType.TextureCube:
                            case EffectParameterType.Int32:
                                switch (ColumnCount)
                                {
                                    case 1:
                                        Apply = Apply1I;
                                        break;
                                    case 2:
                                        Apply = Apply2I;
                                        break;
                                    case 3:
                                        Apply = Apply3I;
                                        break;
                                    case 4:
                                        Apply = Apply4I;
                                        break;
                                }
                                break;
                            case EffectParameterType.Single:
                                switch (ColumnCount)
                                {
                                    case 1:
                                        Apply = Apply1F;
                                        break;
                                    case 2:
                                        Apply = Apply2F;
                                        break;
                                    case 3:
                                        Apply = Apply3F;
                                        break;
                                    case 4:
                                        Apply = Apply4F;
                                        break;
                                }
                                break;
                            case EffectParameterType.Double:
                                switch (ColumnCount)
                                {
                                    case 1:
                                        Apply = Apply1D;
                                        break;
                                    case 2:
                                        Apply = Apply2D;
                                        break;
                                    case 3:
                                        Apply = Apply3D;
                                        break;
                                    case 4:
                                        Apply = Apply4D;
                                        break;
                                }
                                break;
                        }
                    }
                }
            }

            ByteSize = RowCount * ColumnCount * (Type == EffectParameterType.Double ? 8 : 4);
        }

        private unsafe void Apply1B(byte* ptr)
        {
            GL.Uniform1(Slot, Count == 0 ? 1 : Count, (int*)ptr);
        }

        private unsafe void Apply2B(byte* ptr)
        {
            GL.Uniform2(Slot, Count == 0 ? 1 : Count, (int*)ptr);
        }

        private unsafe void Apply3B(byte* ptr)
        {
            GL.Uniform3(Slot, Count == 0 ? 1 : Count, (int*)ptr);
        }

        private unsafe void Apply4B(byte* ptr)
        {
            GL.Uniform4(Slot, Count == 0 ? 1 : Count, (int*)ptr);
        }

        private unsafe void Apply1F(byte* ptr)
        {
            GL.Uniform1(Slot, Count == 0 ? 1 : Count, (float*)ptr);
        }

        private unsafe void Apply2F(byte* ptr)
        {
            GL.Uniform2(Slot, Count == 0 ? 1 : Count, (float*)ptr);
        }

        private unsafe void Apply3F(byte* ptr)
        {
            GL.Uniform3(Slot, Count == 0 ? 1 : Count, (float*)ptr);
        }

        private unsafe void Apply4F(byte* ptr)
        {
            GL.Uniform4(Slot, Count == 0 ? 1 : Count, (float*)ptr);
        }

        private unsafe void Apply1I(byte* ptr)
        {
            GL.Uniform1(Slot, Count == 0 ? 1 : Count, (int*)ptr);
        }

        private unsafe void Apply2I(byte* ptr)
        {
            GL.Uniform2(Slot, Count == 0 ? 1 : Count, (int*)ptr);
        }

        private unsafe void Apply3I(byte* ptr)
        {
            GL.Uniform3(Slot, Count == 0 ? 1 : Count, (int*)ptr);
        }

        private unsafe void Apply4I(byte* ptr)
        {
            GL.Uniform4(Slot, Count == 0 ? 1 : Count, (int*)ptr);
        }

        private unsafe void Apply1D(byte* ptr)
        {
            GL.Uniform1(Slot, Count == 0 ? 1 : Count, (double*)ptr);
        }

        private unsafe void Apply2D(byte* ptr)
        {
            GL.Uniform2(Slot, Count == 0 ? 1 : Count, (double*)ptr);
        }

        private unsafe void Apply3D(byte* ptr)
        {
            GL.Uniform3(Slot, Count == 0 ? 1 : Count, (double*)ptr);
        }

        private unsafe void Apply4D(byte* ptr)
        {
            GL.Uniform4(Slot, Count == 0 ? 1 : Count, (double*)ptr);
        }

        private unsafe void ApplyMatrix3F(byte* ptr)
        {
            GL.UniformMatrix3(Slot, Count == 0 ? 1 : Count, false, (float*)ptr);
        }

        private unsafe void ApplyMatrix3D(byte* ptr)
        {
            GL.UniformMatrix3(Slot, Count == 0 ? 1 : Count, false, (double*)ptr);
        }

        private unsafe void ApplyMatrix4F(byte* ptr)
        {
            GL.UniformMatrix4(Slot, Count == 0 ? 1 : Count, false, (float*)ptr);
        }

        private unsafe void ApplyMatrix4D(byte* ptr)
        {
            GL.UniformMatrix4(Slot, Count == 0 ? 1 : Count, false, (double*)ptr);
        }
    }
}
