using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Graphics.Shading
{
    public class ShaderInput
    {
        public int Index { get; private set; }
        public VertexElementUsage Usage { get; private set; }
        public int UsageIndex { get; private set; }

        public ShaderInput(int index, VertexElementUsage usage, int usageIndex)
        {
            Index = index;
            Usage = usage;
            UsageIndex = usageIndex;
        }
    }
}
