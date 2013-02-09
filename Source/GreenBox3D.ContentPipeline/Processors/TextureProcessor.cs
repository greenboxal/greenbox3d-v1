using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Graphics;

namespace GreenBox3D.ContentPipeline.Processors
{
    [ContentProcessor(DisplayName = "Texture Processor")]
    public class TextureProcessor : ContentProcessor<TextureContent, TextureContent>
    {
        public override TextureContent Process(TextureContent input, BuildContext context)
        {
            return new Texture2DContent();
        }
    }
}
