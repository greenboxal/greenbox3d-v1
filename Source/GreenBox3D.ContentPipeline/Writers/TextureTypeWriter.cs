using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Graphics;
using GreenBox3D.Graphics;

namespace GreenBox3D.ContentPipeline.Writers
{
    [ContentTypeWriter(Extension = ".tex")]
    public class TextureTypeWriter : ContentTypeWriter<TextureContent>
    {
        protected override void Write(ContentWriter stream, TextureContent input, BuildContext context)
        {
            Type type = input.GetType();
            BitmapContent first = input.Faces[0][0];
            SurfaceFormat format;

            if (!first.TryGetFormat(out format))
                throw new NotSupportedException("Invalid surface format");

            if (type == typeof(Texture2DContent))
            {
                stream.Write((int)format);
                stream.Write(first.Width);
                stream.Write(first.Height);
            }
            else
            {
                throw new NotSupportedException("Invalid TextureContent type");
            }

            foreach (MipmapChain chain in input.Faces)
            {
                stream.Write(chain.Count);

                foreach (BitmapContent content in chain)
                {
                    byte[] data = content.GetPixelData();
                    stream.Write(data.Length);
                    stream.Write(data);
                }
            }
        }

        protected override ContentHeader GetHeader(TextureContent input, BuildContext context)
        {
            Type type = input.GetType();

            if (type == typeof(Texture2DContent))
                return new ContentHeader("TEX2", new Version(1, 0));
            
            throw new NotSupportedException("Invalid TextureContent type");
        }
    }
}
