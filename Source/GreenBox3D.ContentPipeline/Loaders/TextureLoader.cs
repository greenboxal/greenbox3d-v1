using System;
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
            return new Texture2D(manager.GraphicsDevice, SurfaceFormat.Color, 128, 128);
        }
    }
}
