using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public abstract class TextureContent
    {
        public MipmapChainCollection Faces { get; private set; }

        protected TextureContent(MipmapChainCollection faces)
        {
            Faces = faces;
        }

        public virtual void GenerateMipmaps(bool overwrite)
        {
            
        }
    }
}
