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
    public class Texture2D : Texture
    {
        #region Fields

        private bool _hasTexture;

        #endregion

        #region Constructors and Destructors

        public Texture2D(GraphicsDevice graphicsDevice, SurfaceFormat format, int width, int height)
            : base(graphicsDevice, format)
        {
            TextureTarget = TextureTarget.Texture2D;

            Width = width;
            Height = height;
        }

        public Texture2D(GraphicsDevice graphicsDevice, int width, int height)
            : this(graphicsDevice, SurfaceFormat.Color, width, height)
        {
        }

        #endregion

        #region Public Properties

        public int Height { get; private set; }
        public int Width { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void SetData<T>(int level, T[] data, int offset) where T : struct
        {
            Create();

            if (LevelCount != level)
                throw new ArgumentException("Level can be higher than the actual LevelCount", "level");

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                GL.TexImage2D(TextureTarget.Texture2D, level, InternalFormat, Width, Height, 0, PixelFormat, PixelType, Marshal.UnsafeAddrOfPinnedArrayElement(data, offset));
                _hasTexture = true;
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }

            if (GL.GetError() == ErrorCode.NoError)
                LevelCount++;
            else
                throw new OpenGLException();
        }

        public void SetData<T>(T[] data) where T : struct
        {
            SetData(0, data, 0);
        }

        #endregion

        #region Methods

        internal override void Create(bool texImage = false)
        {
            if (TextureID == -1)
            {
                TextureID = GL.GenTexture();

                if (TextureID == -1)
                    throw new OpenGLException();
            }

            if (texImage && !_hasTexture)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat, Width, Height, 0, PixelFormat, PixelType, IntPtr.Zero);
                _hasTexture = true;
            }
        }

        #endregion
    }
}