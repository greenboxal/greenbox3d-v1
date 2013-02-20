// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics
{
    public class VertexBuffer : HardwareBuffer
    {
        #region Constructors and Destructors

        public VertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsage usage)
            : base(graphicsDevice, BufferTarget.ArrayBuffer, vertexDeclaration.VertexStride, vertexCount, usage)
        {
            VertexDeclaration = vertexDeclaration;
        }

        public VertexBuffer(GraphicsDevice graphicsDevice, Type elementType, int vertexCount, BufferUsage usage)
            : this(graphicsDevice, VertexDeclaration.FromType(elementType), vertexCount, usage)
        {
        }

        #endregion

        #region Public Properties

        public VertexDeclaration VertexDeclaration { get; private set; }

        #endregion
    }
}