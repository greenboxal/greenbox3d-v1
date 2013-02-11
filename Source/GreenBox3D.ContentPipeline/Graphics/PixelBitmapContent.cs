using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics;
using GreenBox3D.Math;

using IronRuby.Builtins;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class PixelBitmapContent<T> : BitmapContent where T : struct
    {
        private readonly T[] _data;

        public PixelBitmapContent(int width, int height)
            : base(width, height)
        {
            _data = new T[width * height];
        }
        
        public override bool TryGetFormat(out SurfaceFormat format)
        {
            format = SurfaceFormat.Color;

            if (typeof(T) == typeof(byte))
                format = SurfaceFormat.Alpha8;
            else if (typeof(T) == typeof(Color) || typeof(T) == typeof(Vector4))
                format = SurfaceFormat.Color;
            else
                return false;

            return true;
        }

        public void SetPixel(int x, int y, T color)
        {
            _data[y * Width + x] = color;
        }

        public override void SetPixelData(byte[] sourceData)
        {
            if (Marshal.SizeOf(typeof(T)) * _data.Length != sourceData.Length)
                throw new ArgumentException("Data don't fit expected size", "sourceData");

            GCHandle handle = GCHandle.Alloc(_data, GCHandleType.Pinned);
            Marshal.Copy(sourceData, 0, handle.AddrOfPinnedObject(), sourceData.Length);
            handle.Free();
        }

        public T GetPixel(int x, int y)
        {
            return _data[y * Width + x];
        }

        public T[] GetRow(int y)
        {
            T[] row = new T[Width];
            Buffer.BlockCopy(_data, y * Width, row, 0, Width);
            return row;
        }

        public override byte[] GetPixelData()
        {
            byte[] data = new byte[Marshal.SizeOf(typeof(T)) * _data.Length];

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            Marshal.Copy(handle.AddrOfPinnedObject(), data, 0, data.Length);
            handle.Free();

            return data;
        }

        public override void MakeTransparent(Color color)
        {
            throw new NotImplementedException();
        }

        public void ReplaceColor(T originalColor, T newColor)
        {
            for (int i = 0; i < _data.Length; i++)
                if (_data[i].Equals(originalColor))
                    _data[i] = newColor;
        }
    }
}
