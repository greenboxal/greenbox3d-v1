using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class MipmapChain : Collection<BitmapContent>
    {
        public static implicit operator MipmapChain(BitmapContent bitmap)
        {
            MipmapChain chain = new MipmapChain();
            chain.Add(bitmap);
            return chain;
        }
    }
}
