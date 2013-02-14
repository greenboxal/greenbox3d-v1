﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Graphics;
using GreenBox3D.Graphics;

namespace GreenBox3D.ContentPipeline.Loaders
{
    [ContentLoader]
    public class TextureLoader : ContentLoader<TextureContent, Texture>
    {
        public override Texture Load(ContentManager manager, TextureContent input, BuildContext context)
        {
            if (!(input is Texture2DContent))
                return null;

            SurfaceFormat format;

            if (!input.Faces[0][0].TryGetFormat(out format))
                throw new NotSupportedException("Unsupported image format");

            Texture2D tex = new Texture2D(manager.GraphicsDevice, format, input.Faces[0][0].Width, input.Faces[0][0].Height);

            int level = 0;
            foreach (BitmapContent content in input.Faces[0])
                tex.SetData(level++, content.GetPixelData(), 0);

            return tex;
        }
    }
}