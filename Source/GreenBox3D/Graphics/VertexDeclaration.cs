using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics.Shading;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics
{
    public class VertexDeclaration
    {
        private static VertexDeclaration _lastUsed;
        private static IntPtr _lastUsedPointer;
        private static Shader _lastUsedShader;

        public static void ResetLastUsedCache()
        {
            _lastUsed = null;
            _lastUsedPointer = IntPtr.Zero;
            _lastUsedShader = null;
        }

        private readonly VertexElement[] _vertexElements;

        public int VertexStride { get; private set; }

        public VertexDeclaration(int vertexStride, VertexElement[] elements)
        {
            _vertexElements = (VertexElement[])elements.Clone();
            VertexStride = vertexStride;
        }

        public VertexDeclaration(VertexElement[] elements)
            : this(CalculateStride(elements), elements)
        {
        }

        public static VertexDeclaration FromType(Type elementType)
        {
            IVertexType vertexType = Activator.CreateInstance(elementType) as IVertexType;

            if (vertexType == null)
                throw new ArgumentException("elementType must implement IVertexType", "elementType");

            return vertexType.VertexDeclaration;
        }

        public VertexElement[] GetVertexElements()
        {
            return _vertexElements;
        }

        private static int CalculateStride(IEnumerable<VertexElement> elements)
        {
            int stride = 0;

            foreach (VertexElement element in elements)
                stride = Math.Max(stride, element.Offset + element.SizeInBytes);

            return stride;
        }

        internal void Bind(GraphicsDevice graphicsDevice, IntPtr source)
        {
            if (_lastUsed == this && _lastUsedPointer == source)
                return;

            foreach (VertexElement element in _vertexElements)
            {
                int index = graphicsDevice.ActiveShader.GetInputIndex(element.VertexElementUsage, element.UsageIndex);

                if (index == -1)
                    continue;

                GL.EnableVertexAttribArray(index);
                GL.VertexAttribPointer(index, element.ElementCount, element.VertexAttribPointerType, !element.IsNormalized, VertexStride, source + element.Offset);
            }

            _lastUsed = this;
            _lastUsedPointer = source;
            _lastUsedShader = graphicsDevice.ActiveShader;
        }
    }
}
