using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class ShaderSource
    {
        public string Source { get; private set; }
        public string Filename { get; private set; }
        public int IncludeLine { get; private set; }

        public ShaderSource(string source, string filename, int line)
        {
            Source = source;
            Filename = filename;
            IncludeLine = line;
        }
    }
}
