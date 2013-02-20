// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

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
        #region Static Fields

        private static VertexDeclaration _lastUsed;
        private static IntPtr _lastUsedPointer;
        private static Shader _lastUsedShader;

        #endregion

        #region Fields

        private readonly VertexElement[] _vertexElements;

        #endregion

        #region Constructors and Destructors

        public VertexDeclaration(int vertexStride, VertexElement[] elements)
        {
            _vertexElements = (VertexElement[])elements.Clone();
            VertexStride = vertexStride;
        }

        public VertexDeclaration(VertexElement[] elements)
            : this(CalculateStride(elements), elements)
        {
        }

        #endregion

        #region Public Properties

        public int VertexStride { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static VertexDeclaration FromType(Type elementType)
        {
            IVertexType vertexType = Activator.CreateInstance(elementType) as IVertexType;

            if (vertexType == null)
                throw new ArgumentException("elementType must implement IVertexType", "elementType");

            return vertexType.VertexDeclaration;
        }

        public static void ResetLastUsedCache()
        {
            _lastUsed = null;
            _lastUsedPointer = IntPtr.Zero;
            _lastUsedShader = null;
        }

        public VertexElement[] GetVertexElements()
        {
            return _vertexElements;
        }

        #endregion

        #region Methods

        internal void Bind(GraphicsDevice graphicsDevice, IntPtr source)
        {
            if (_lastUsed == this && _lastUsedPointer == source && _lastUsedShader == graphicsDevice.ActiveShader)
                return;

            for (int i = 0; i < graphicsDevice.ActiveShader.Input.Length; i++)
            {
                VertexElement element = null;
                ShaderInput input = graphicsDevice.ActiveShader.Input[i];

                for (int j = 0; j < _vertexElements.Length; j++)
                {
                    if (_vertexElements[j].VertexElementUsage == input.Usage && _vertexElements[j].UsageIndex == input.UsageIndex)
                    {
                        element = _vertexElements[j];
                        break;
                    }
                }

                if (element != null)
                {
                    GL.EnableVertexAttribArray(input.Index);
                    GL.VertexAttribPointer(input.Index, element.ElementCount, element.VertexAttribPointerType, !element.IsNormalized, VertexStride, source + element.Offset);
                }
                else
                {
                    GL.DisableVertexAttribArray(input.Index);
                }
            }

            foreach (VertexElement element in _vertexElements)
            {
                int index = graphicsDevice.ActiveShader.GetInputIndex(element.VertexElementUsage, element.UsageIndex);

                if (index == -1)
                {
                }
                else
                {
                }
            }

            _lastUsed = this;
            _lastUsedPointer = source;
            _lastUsedShader = graphicsDevice.ActiveShader;
        }

        private static int CalculateStride(IEnumerable<VertexElement> elements)
        {
            int stride = 0;

            foreach (VertexElement element in elements)
                stride = Math.Max(stride, element.Offset + element.SizeInBytes);

            return stride;
        }

        #endregion
    }
}