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
    public static class TextureHelpers
    {
        #region Public Methods and Operators

        public static bool IsCompleteFormat(Type type)
        {
            return type == typeof(PixelBitmapContent<Color>) || type == typeof(PixelBitmapContent<Vector4>);
        }

        public static bool IsCompleteFormat(SurfaceFormat format)
        {
            return format == SurfaceFormat.Color || format == SurfaceFormat.Vector4;
        }

        public static bool IsCompletePixelType(Type type)
        {
            return type == typeof(Vector4) || type == typeof(Color);
        }

        public static void MakeTransparent(TextureContent texture, Color color)
        {
            foreach (BitmapContent bitmap in texture.Faces.SelectMany(chain => chain))
            {
                if (bitmap is PixelBitmapContent<Vector4>)
                    ((PixelBitmapContent<Vector4>)bitmap).ReplaceColor(color.ToVector4(), Vector4.Zero);
                else if (bitmap is PixelBitmapContent<Color>)
                    ((PixelBitmapContent<Color>)bitmap).ReplaceColor(color, Color.Transparent);
            }
        }

        #endregion
    }
}