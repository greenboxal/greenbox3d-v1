using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Graphics;
using GreenBox3D.ContentPipeline.Loaders;
using GreenBox3D.ContentPipeline.Writers;
using GreenBox3D.Math;

namespace GreenBox3D.ContentPipeline.Processors
{
    [ContentProcessor(typeof(TextureTypeWriter), Loader = typeof(TextureLoader), DisplayName = "Texture Processor")]
    public class TextureProcessor : ContentProcessor<TextureContent, TextureContent>
    {
        public override TextureContent Process(TextureContent input, BuildContext context)
        {
            if (context.Descriptor["transparency_key"] != null)
            {
                dynamic tkey = context.Descriptor["transparency_key"];
                TextureHelpers.MakeTransparent(input, new Color(tkey[0], tkey[1], tkey[2]));
            }

            if (context.Descriptor["create_mimaps"] == true)
                input.GenerateMipmaps(true);

            input.Validate();

            return input;
        }
    }
}
