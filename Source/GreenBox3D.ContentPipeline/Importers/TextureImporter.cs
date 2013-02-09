using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Graphics;

namespace GreenBox3D.ContentPipeline.Importers
{
    [ContentImporter(".bmp", ".jpg", ".png", ".gif", ".tga", DisplayName = "Texture Importer", DefaultProcessor = "TextureProcessor")]
    public class TextureImporter : ContentImporter<TextureContent>
    {
        public override TextureContent Import(Stream stream, BuildContext context)
        {
            return null;
        }
    }
}
