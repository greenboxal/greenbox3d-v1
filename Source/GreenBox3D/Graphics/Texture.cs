﻿// GreenBox3D
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

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics
{
    public abstract class Texture : GraphicsResource
    {
        #region Fields

        internal PixelInternalFormat InternalFormat;
        internal PixelFormat PixelFormat;
        internal PixelType PixelType;
        internal int TextureID;
        internal TextureTarget TextureTarget;

        #endregion

        #region Constructors and Destructors

        protected Texture(GraphicsDevice graphicsDevice, SurfaceFormat format)
            : base(graphicsDevice)
        {
            TextureID = -1;

            Format = format;
            GetOpenGLTextureFormat(format, out InternalFormat, out PixelFormat, out PixelType);
        }

        #endregion

        #region Public Properties

        public SurfaceFormat Format { get; private set; }
        public int LevelCount { get; protected set; }

        #endregion

        #region Methods

        internal static void GetOpenGLTextureFormat(SurfaceFormat format, out PixelInternalFormat glInternalFormat, out PixelFormat glFormat, out PixelType glType)
        {
            glInternalFormat = PixelInternalFormat.Rgba;
            glFormat = PixelFormat.Rgba;
            glType = PixelType.UnsignedByte;

            switch (format)
            {
                case SurfaceFormat.Color:
                    glInternalFormat = PixelInternalFormat.Rgba;
                    glFormat = PixelFormat.Rgba;
                    glType = PixelType.UnsignedByte;
                    break;
                case SurfaceFormat.Bgr565:
                    glInternalFormat = PixelInternalFormat.Rgb;
                    glFormat = PixelFormat.Rgb;
                    glType = PixelType.UnsignedShort565;
                    break;
                case SurfaceFormat.Bgra4444:
                    glInternalFormat = PixelInternalFormat.Rgba4;
                    glFormat = PixelFormat.Rgba;
                    glType = PixelType.UnsignedShort4444;
                    break;
                case SurfaceFormat.Bgra5551:
                    glInternalFormat = PixelInternalFormat.Rgba;
                    glFormat = PixelFormat.Rgba;
                    glType = PixelType.UnsignedShort5551;
                    break;
                case SurfaceFormat.Alpha8:
                    glInternalFormat = PixelInternalFormat.Luminance;
                    glFormat = PixelFormat.Luminance;
                    glType = PixelType.UnsignedByte;
                    break;
                case SurfaceFormat.Dxt1:
                    glInternalFormat = PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;
                    glFormat = (PixelFormat)All.CompressedTextureFormats;
                    break;
                case SurfaceFormat.Dxt3:
                    glInternalFormat = PixelInternalFormat.CompressedRgbaS3tcDxt3Ext;
                    glFormat = (PixelFormat)All.CompressedTextureFormats;
                    break;
                case SurfaceFormat.Dxt5:
                    glInternalFormat = PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;
                    glFormat = (PixelFormat)All.CompressedTextureFormats;
                    break;

                case SurfaceFormat.Single:
                    glInternalFormat = PixelInternalFormat.R32f;
                    glFormat = PixelFormat.Red;
                    glType = PixelType.Float;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        internal abstract void Create(bool texImage = false);

        #endregion
    }
}