using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class ShaderVariable
    {
        public string Type { get; private set; }
        public string Name { get; private set; }
        public int Size { get; private set; }

        public ShaderVariable(string type, string name, int size = 0)
        {
            Type = type;
            Name = name;
            Size = size;
        }
    }
}
