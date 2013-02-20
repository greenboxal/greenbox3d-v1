using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics
{
    public class IndexBuffer : HardwareBuffer
    {
        internal DrawElementsType DrawElementsType;
        public IndexElementSize IndexElementSize { get; private set; }

        public IndexBuffer(GraphicsDevice graphicsDevice, IndexElementSize indexElementSize, int indexCount, BufferUsage usage)
            : base(graphicsDevice, BufferTarget.ElementArrayBuffer, GetElementSizeInBytes(indexElementSize), indexCount, usage)
        {
            IndexElementSize = indexElementSize;

            switch (indexElementSize)
            {
                case IndexElementSize.EightBits:
                    DrawElementsType = DrawElementsType.UnsignedByte;
                    break;
                case IndexElementSize.SixteenBits:
                    DrawElementsType = DrawElementsType.UnsignedShort;
                    break;
                case IndexElementSize.ThirtyTwoBits:
                    DrawElementsType = DrawElementsType.UnsignedInt;
                    break;
            }
        }

        public IndexBuffer(GraphicsDevice graphicsDevice, Type elementType, int indexCount, BufferUsage usage)
            : this(graphicsDevice, GetElementSizeFromType(elementType), indexCount, usage)
        {
        }

        private static IndexElementSize GetElementSizeFromType(Type type)
        {
            switch (Marshal.SizeOf(type))
            {
                case 1:
                    return IndexElementSize.EightBits;
                case 2:
                    return IndexElementSize.SixteenBits;
                case 3:
                    return IndexElementSize.ThirtyTwoBits;
                default:
                    throw new NotSupportedException();
            }
        }

        private static int GetElementSizeInBytes(IndexElementSize size)
        {
            switch (size)
            {
                case IndexElementSize.EightBits:
                    return 1;
                case IndexElementSize.SixteenBits:
                    return 2;
                case IndexElementSize.ThirtyTwoBits:
                    return 4;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
