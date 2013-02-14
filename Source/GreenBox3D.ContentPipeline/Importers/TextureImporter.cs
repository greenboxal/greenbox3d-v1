using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Graphics;
using GreenBox3D.Graphics;

namespace GreenBox3D.ContentPipeline.Importers
{
    [ContentImporter(".bmp", ".jpg", ".png", ".gif", DisplayName = "Texture Importer", DefaultProcessor = "TextureProcessor")]
    public class TextureImporter : ContentImporter<TextureContent>
    {
        public override TextureContent Import(Stream stream, BuildContext context)
        {
            Texture2DContent texture = new Texture2DContent();

            texture.Faces[0] = CreateBitmapContent(SurfaceFormat.Color, new Bitmap(stream));

            return texture;
        }

        private static BitmapContent CreateBitmapContent(SurfaceFormat format, Bitmap input)
        {
            BitmapContent content;
            int bpp = 0;

            if (input.PixelFormat != PixelFormat.Format32bppArgb)
            {
                Bitmap bmp = new Bitmap(input.Width, input.Height, PixelFormat.Format32bppArgb);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
                g.DrawImageUnscaled(input, 0, 0);
                g.Dispose();
                input = bmp;
            }

            switch (format)
            {
                case SurfaceFormat.Color:
                    content = new PixelBitmapContent<GreenBox3D.Math.Color>(input.Width, input.Height);
                    bpp = 4;
                    break;
                case SurfaceFormat.Alpha8:
                    content = new PixelBitmapContent<byte>(input.Width, input.Height);
                    bpp = 1;
                    break;
                default:
                    throw new NotSupportedException("Unsupported target pixel format.");
            }

            BitmapData data = input.LockBits(new Rectangle(0, 0, input.Width, input.Height), ImageLockMode.ReadOnly, input.PixelFormat);
            byte[] bytes = new byte[input.Width * input.Height * bpp];

            unsafe
            {
                switch (format)
                {
                    case SurfaceFormat.Color:
                        {
                            byte* ptr = (byte*)data.Scan0;

                            for (int i = 0; i < bytes.Length; i += 4)
                            {
                                bytes[i + 0] = ptr[i + 2];
                                bytes[i + 1] = ptr[i + 1];
                                bytes[i + 2] = ptr[i + 0];
                                bytes[i + 3] = ptr[i + 3];
                            }
                        }
                        break;
                    case SurfaceFormat.Alpha8:
                        {
                            byte* ptr = (byte*)data.Scan0;

                            for (int i = 0; i < bytes.Length; i += 4)
                                bytes[i / 4] = ptr[i + 3];
                        }
                        break;
                }
            }

            content.SetPixelData(bytes);
            input.UnlockBits(data);

            return content;
        }
    }
}
