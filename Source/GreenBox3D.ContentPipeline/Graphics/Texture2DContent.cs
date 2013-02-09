using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class Texture2DContent : TextureContent
    {
        public MipmapChain Mipmaps { get { return Faces[0]; } set { Faces[0] = value; } }

        public Texture2DContent()
            : base(new MipmapChainCollection() { new MipmapChain() })
        {
        }
    }
}
