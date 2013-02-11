using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Graphics;
using GreenBox3D.Graphics;

using Color = System.Drawing.Color;

namespace GreenBox3D.ContentPipeline.Processors
{
    [ContentProcessor(DisplayName = "Texture Processor")]
    public class SimpleTextureProcessor : ContentProcessor<Bitmap, Texture2DContent>
    {
        public override Texture2DContent Process(Bitmap input, BuildContext context)
        {
            Texture2DContent texture = new Texture2DContent();
            SurfaceFormat format = GetFormat(context.Descriptor["target_format"]);

            if (context.Descriptor["transparency_key"] != null)
            {
                dynamic rgb = context.Descriptor["transparency_key"];
                input.MakeTransparent(Color.FromArgb(rgb[0], rgb[1], rgb[2]));
            }

            texture.Mipmaps = CreateBitmapContent(format, input);

            if (context.Descriptor["create_mimaps"] == true)
                texture.GenerateMipmaps(true);

            return texture;
        }

        private BitmapContent CreateBitmapContent(SurfaceFormat format, Bitmap input)
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

            input.UnlockBits(data);

            return content;
        }

        public static SurfaceFormat GetFormat(dynamic o)
        {
            SurfaceFormat format;
            string str = o == null ? "color" : o.ToString();

            switch (str)
            {
                case "color":
                    format = SurfaceFormat.Color;
                    break;
                case "alpha":
                    format = SurfaceFormat.Color;
                    break;
                default:
                    format = SurfaceFormat.Color;
                    break;
            }

            return format;
        }
    }
}
