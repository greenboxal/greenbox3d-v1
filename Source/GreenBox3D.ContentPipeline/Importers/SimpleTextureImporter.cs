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
    [ContentImporter(".bmp", ".jpg", ".png", ".gif", DisplayName = "Simple Texture2D Importer", DefaultProcessor = "SimpleTextureProcessor")]
    public class SimpleTextureImporter : ContentImporter<Bitmap>
    {
        public override Bitmap Import(Stream stream, BuildContext context)
        {
            return new Bitmap(stream);
        }
    }
}
