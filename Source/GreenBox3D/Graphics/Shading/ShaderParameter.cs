using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Graphics.Shading
{
    internal class ShaderParameter
    {
        public string Name { get; private set; }
        public EffectParameterType Type { get; private set; }
        public EffectParameterClass Class { get; private set; }
        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }
        public int Count { get; private set; }
        public int ByteSize { get; private set; }
        public int Slot { get; set; }

        public ShaderParameter(string name, string glslType, int size)
        {
            Name = name;
            Count = size;
            Slot = -1;

            if (Count == 0)
                Count = 1;

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

            ByteSize = RowCount * ColumnCount * (Type == EffectParameterType.Double ? 8 : 4);
        }
    }
}
