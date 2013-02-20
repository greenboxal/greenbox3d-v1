using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class ShaderEntryInput
    {
        public VertexElementUsage Usage { get; private set; }
        public int UsageIndex { get; private set; }
        public ShaderVariable Variable { get; private set; }

        public ShaderEntryInput(VertexElementUsage usage, int usageIndex, ShaderVariable variable)
        {
            Usage = usage;
            UsageIndex = usageIndex;
            Variable = variable;
        }
    }
}
