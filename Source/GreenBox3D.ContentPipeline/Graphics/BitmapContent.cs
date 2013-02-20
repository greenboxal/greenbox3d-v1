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

using GreenBox3D.Graphics;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public abstract class BitmapContent
    {
        #region Constructors and Destructors

        protected BitmapContent(int width, int height)
        {
            Width = width;
            Height = height;
        }

        #endregion

        #region Public Properties

        public int Height { get; private set; }
        public int Width { get; private set; }

        #endregion

        #region Public Methods and Operators

        public abstract byte[] GetPixelData();
        public abstract void SetPixelData(byte[] sourceData);
        public abstract void SetPixelData(IntPtr sourceData, int len);
        public abstract bool TryGetFormat(out SurfaceFormat format);

        #endregion
    }
}