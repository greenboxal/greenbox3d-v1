using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics
{
    public class VertexElement
    {
        internal VertexAttribPointerType VertexAttribPointerType;
        internal bool IsNormalized;
        internal int ElementCount;

        public int Offset { get; private set; }
        public VertexElementFormat VertexElementFormat { get; private set; }
        public VertexElementUsage VertexElementUsage { get; private set; }
        public int UsageIndex { get; private set; }
        public int SizeInBytes { get; private set; }

        public VertexElement(int offset, VertexElementFormat elementFormat, VertexElementUsage elementUsage, int usageIndex = 0)
        {
            Offset = offset;
            VertexElementFormat = elementFormat;
            VertexElementUsage = elementUsage;
            UsageIndex = usageIndex;
            IsNormalized = true;

            switch (elementFormat)
            {
                case VertexElementFormat.Single:
                    ElementCount = 1;
                    SizeInBytes = 4;
                    VertexAttribPointerType = VertexAttribPointerType.Float;
                    break;
                case VertexElementFormat.Vector2:
                    ElementCount = 2;
                    SizeInBytes = 8;
                    VertexAttribPointerType = VertexAttribPointerType.Float;
                    break;
                case VertexElementFormat.Vector3:
                    ElementCount = 3;
                    SizeInBytes = 12;
                    VertexAttribPointerType = VertexAttribPointerType.Float;
                    break;
                case VertexElementFormat.Vector4:
                    ElementCount = 4;
                    SizeInBytes = 16;
                    VertexAttribPointerType = VertexAttribPointerType.Float;
                    break;
                case VertexElementFormat.Color:
                    ElementCount = 4;
                    SizeInBytes = 4;
                    VertexAttribPointerType = VertexAttribPointerType.UnsignedByte;
                    IsNormalized = false;
                    break;
                case VertexElementFormat.Byte4:
                    ElementCount = 4;
                    SizeInBytes = 4;
                    VertexAttribPointerType = VertexAttribPointerType.Byte;
                    IsNormalized = false;
                    break;
                case VertexElementFormat.Short2:
                    ElementCount = 2;
                    SizeInBytes = 4;
                    VertexAttribPointerType = VertexAttribPointerType.Short;
                    IsNormalized = false;
                    break;
                case VertexElementFormat.Short4:
                    ElementCount = 4;
                    SizeInBytes = 8;
                    VertexAttribPointerType = VertexAttribPointerType.Short;
                    IsNormalized = false;
                    break;
                case VertexElementFormat.NormalizedShort2:
                    ElementCount = 2;
                    SizeInBytes = 4;
                    VertexAttribPointerType = VertexAttribPointerType.Short;
                    break;
                case VertexElementFormat.NormalizedShort4:
                    ElementCount = 4;
                    SizeInBytes = 8;
                    VertexAttribPointerType = VertexAttribPointerType.Short;
                    break;
                case VertexElementFormat.HalfVector2:
                    ElementCount = 2;
                    SizeInBytes = 4;
                    VertexAttribPointerType = VertexAttribPointerType.HalfFloat;
                    break;
                case VertexElementFormat.HalfVector4:
                    ElementCount = 4;
                    VertexAttribPointerType = VertexAttribPointerType.HalfFloat;
                    SizeInBytes = 8;
                    break;
            }
        }
    }
}
