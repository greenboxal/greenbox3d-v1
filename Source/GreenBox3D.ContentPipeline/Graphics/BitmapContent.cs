using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics;
using GreenBox3D.Math;

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
        public abstract byte[] GetPixelData();
        public abstract void MakeTransparent(Color color);
    }
}
