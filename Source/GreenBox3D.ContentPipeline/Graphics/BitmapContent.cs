using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public abstract class BitmapContent
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        protected BitmapContent(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public abstract bool TryGetFormat(out SurfaceFormat format);
        public abstract void SetPixelData(byte[] sourceData);
        public abstract void SetPixelData(IntPtr sourceData, int len);
        public abstract byte[] GetPixelData();
    }
}
