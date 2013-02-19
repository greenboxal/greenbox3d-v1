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

using GreenBox3D.Math;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics
{
    public class GraphicsDevice : IDisposable
    {
        #region Fields

        private GraphicsDeviceManager _owner;
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

        public PresentationParameters PresentationParameters { get; private set; }

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
                GL.DepthRange(value.MinDepth, value.MaxDepth);
            }
        }

        public TextureCollection Textures { get; private set; }

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

        public void Initialize()
        {
            _viewport = new Viewport(0, 0, PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight);
        }

        public void Present()
        {
            GL.Flush();
        }

        #endregion
    }
}