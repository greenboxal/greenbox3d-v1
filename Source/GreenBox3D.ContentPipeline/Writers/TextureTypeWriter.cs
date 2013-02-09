using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Graphics;

namespace GreenBox3D.ContentPipeline.Writers
{
    [ContentTypeWriter(Extension = ".tex")]
    public class TextureTypeWriter : ContentTypeWriter<TextureContent>
    {
        public TextureTypeWriter()
            : base(new ContentHeader("TEX", new Version(1, 0)))
        {
        }

        protected override void Write(ContentWriter stream, TextureContent input, BuildContext context)
        {
            stream.Write(0x135465);
            stream.Write(0x8fabd43);
            stream.Write(0xdeadb33f);
            stream.Write(0x135465);
        }
    }
}
