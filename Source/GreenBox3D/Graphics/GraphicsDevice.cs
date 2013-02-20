// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics.Shading;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics
{
    public class GraphicsDevice : IDisposable
    {
        #region Fields

        internal Shader ActiveShader;
        private IndexBuffer _indices;
        private bool _indicesDirty;
        private GraphicsDeviceManager _owner;
        private VertexBuffer _vertices;
        private bool _verticesDirty;
        private Viewport _viewport;

        #endregion

        #region Constructors and Destructors

        public GraphicsDevice(GraphicsDeviceManager owner)
        {
            _owner = owner;

            _viewport = new Viewport(0, 0, owner.PreferredBackBufferWidth, owner.PreferredBackBufferHeight);
            _viewport.MaxDepth = 1.0f;

            PresentationParameters = new PresentationParameters();
            PresentationParameters.DepthStencilFormat = DepthFormat.Depth24;

            Textures = new TextureCollection();
        }

        #endregion

        #region Public Properties

        public IndexBuffer Indices
        {
            get { return _indices; }
            set
            {
                if (_indices != value)
                {
                    _indicesDirty = true;
                    _indices = value;
                }
            }
        }

        public PresentationParameters PresentationParameters { get; private set; }
        public TextureCollection Textures { get; private set; }

        public Viewport Viewport
        {
            get { return _viewport; }
            set
            {
                _viewport = value;

                // TODO: After render target reimplementation
                //if (IsRenderTargetBound)
                //    GL.Viewport(value.X, value.Y, value.Width, value.Height);
                //else

                GL.Viewport(value.X, PresentationParameters.BackBufferHeight - value.Y - value.Height, value.Width, value.Height);
                // GL.DepthRange(value.MinDepth, value.MaxDepth);
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Clear(Color color)
        {
            Clear(ClearOptions.DepthBuffer | ClearOptions.Stencil | ClearOptions.Target, color);
        }

        public void Clear(ClearOptions options)
        {
            ClearBufferMask mask = 0;

            if ((options & ClearOptions.DepthBuffer) != 0)
                mask |= ClearBufferMask.DepthBufferBit;

            if ((options & ClearOptions.Stencil) != 0)
                mask |= ClearBufferMask.StencilBufferBit;

            if ((options & ClearOptions.Target) != 0)
                mask |= ClearBufferMask.ColorBufferBit;

            GL.Clear(mask);
        }

        public void Clear(ClearOptions options, Color color)
        {
            Clear(options);
            GL.ClearColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
        }

        public void Dispose()
        {
        }

        public void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int numVertices, int startIndex, int primitiveCount)
        {
            if (Indices == null)
                throw new InvalidOperationException("Vertices must be set before calling this method");

            if (ActiveShader == null)
                throw new InvalidOperationException("An Effect must be applied before calling this method");

            SetRenderingState();
            _vertices.VertexDeclaration.Bind(this, IntPtr.Zero);

            var indexOffsetInBytes = (IntPtr)(startIndex * _indices.ElementSize);
            var indexElementCount = GetElementCountArray(primitiveType, primitiveCount);
            var target = GetBeginMode(primitiveType);

            GL.DrawElementsBaseVertex(target, indexElementCount, Indices.DrawElementsType, indexOffsetInBytes, baseVertex);
        }

        public void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            if (_vertices == null)
                throw new InvalidOperationException("SetVertexBuffer must be called before calling this method");

            if (ActiveShader == null)
                throw new InvalidOperationException("An Effect must be applied before calling this method");

            SetRenderingState();
            _vertices.VertexDeclaration.Bind(this, IntPtr.Zero);

            GL.DrawArrays(GetBeginMode(primitiveType), startVertex, primitiveCount);
        }

        public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int vertexCount, int[] indexData, int indexOffset, int primitiveCount) where T : IVertexType
        {
            if (ActiveShader == null)
                throw new InvalidOperationException("An Effect must be applied before calling this method");

            VertexDeclaration vertexDeclaration = VertexDeclaration.FromType(typeof(T));
            SetRenderingState(false, false);

            GCHandle handle = GCHandle.Alloc(vertexData, GCHandleType.Pinned);
            IntPtr arrayStart = Marshal.UnsafeAddrOfPinnedArrayElement(vertexData, 0);

            if (vertexOffset > 0)
                arrayStart = new IntPtr(arrayStart.ToInt32() + (vertexOffset * vertexDeclaration.VertexStride));

            vertexDeclaration.Bind(this, arrayStart);

            unsafe
            {
                fixed (int* indicesPtr = &indexData[0])
                    GL.DrawElementsBaseVertex(GetBeginMode(primitiveType), primitiveCount, DrawElementsType.UnsignedInt, (IntPtr)(&indicesPtr[indexOffset]), vertexOffset);
            }

            handle.Free();
        }

        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount) where T : struct, IVertexType
        {
            if (ActiveShader == null)
                throw new InvalidOperationException("An Effect must be applied before calling this method");

            VertexDeclaration vertexDeclaration = VertexDeclaration.FromType(typeof(T));
            SetRenderingState(false, false);

            GCHandle handle = GCHandle.Alloc(vertexData, GCHandleType.Pinned);
            IntPtr arrayStart = Marshal.UnsafeAddrOfPinnedArrayElement(vertexData, 0);

            if (vertexOffset > 0)
                arrayStart = new IntPtr(arrayStart.ToInt32() + (vertexOffset * vertexDeclaration.VertexStride));

            vertexDeclaration.Bind(this, arrayStart);

            GL.DrawArrays(GetBeginMode(primitiveType), vertexOffset, primitiveCount);

            handle.Free();
        }

        public void Initialize()
        {
            _viewport = new Viewport(0, 0, PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight);
        }

        public void Present()
        {
            GL.Flush();
        }

        public void SetVertexBuffer(VertexBuffer vertexBuffer)
        {
            if (_vertices != vertexBuffer)
            {
                _vertices = vertexBuffer;
                _verticesDirty = true;
            }
        }

        #endregion

        #region Methods

        private static BeginMode GetBeginMode(PrimitiveType primitiveType)
        {
            switch (primitiveType)
            {
                case PrimitiveType.LineList:
                    return BeginMode.LineLoop;
                case PrimitiveType.LineStrip:
                    return BeginMode.LineStrip;
                case PrimitiveType.TriangleList:
                    return BeginMode.TriangleFan;
                case PrimitiveType.TriangleStrip:
                    return BeginMode.TriangleStrip;
                default:
                    throw new NotSupportedException();
            }
        }

        private static int GetElementCountArray(PrimitiveType primitiveType, int primitiveCount)
        {
            switch (primitiveType)
            {
                case PrimitiveType.LineList:
                    return primitiveCount * 2;
                case PrimitiveType.LineStrip:
                    return primitiveCount + 1;
                case PrimitiveType.TriangleList:
                    return primitiveCount * 3;
                case PrimitiveType.TriangleStrip:
                    return 3 + (primitiveCount - 1);
                default:
                    throw new NotSupportedException();
            }
        }

        private void SetRenderingState(bool setIndex = true, bool setVertex = true)
        {
            if (setIndex && _indicesDirty)
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indices.BufferID);
            else
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            if (setVertex && _verticesDirty)
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertices.BufferID);
            else
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        #endregion
    }
}