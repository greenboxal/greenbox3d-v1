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

namespace GreenBox3D.ContentPipeline.Processors
{
    [ContentProcessor(typeof(TextureTypeWriter), Loader = typeof(TextureLoader), DisplayName = "Texture Processor")]
    public class TextureProcessor : ContentProcessor<TextureContent, TextureContent>
    {
        public override TextureContent Process(TextureContent input, BuildContext context)
        {
            dynamic properties = context.Descriptor.Properties;

            if (properties.TransparencyKey != null)
            {
                TextureHelpers.MakeTransparent(input, new Color(properties.TransparencyKey[0], properties.TransparencyKey[1], properties.TransparencyKey[2]));
            }

            if (properties.CreateMipmaps == true)
                input.GenerateMipmaps(true);

            input.Validate();

            return input;
        }
    }
}
