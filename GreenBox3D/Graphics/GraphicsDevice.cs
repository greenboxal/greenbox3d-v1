using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Graphics
{
    public abstract class GraphicsDevice : IDisposable
    {
        [ThreadStatic]
        private static GraphicsDevice _currentDevice;
        public static GraphicsDevice ActiveDevice
        {
            get { return _currentDevice; }
        }

        public abstract PresentationParameters PresentationParameters { get; }
        public abstract Viewport Viewport { get; set; }

        public void MakeCurrent()
        {
            if (MakeCurrentInternal())
            {
                _currentDevice = this;
            }
        }

        public virtual void Clear(Color color)
        {
            Clear(ClearOptions.DepthBuffer | ClearOptions.Stencil | ClearOptions.Target, color);
        }

        public virtual void Clear(ClearOptions options)
        {
            Clear(options, Color.Black);
        }

        public abstract void Clear(ClearOptions options, Color color);
        public abstract void Present();
        public abstract void Dispose();

        protected abstract bool MakeCurrentInternal();
    }
}
