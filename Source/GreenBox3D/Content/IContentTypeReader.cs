using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Content
{
    public interface IContentTypeReader
    {
        object Load(ContentManager manager, Stream stream);
    }
}
