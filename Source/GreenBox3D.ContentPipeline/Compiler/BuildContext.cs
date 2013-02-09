using System;
using System.Collections.Generic;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public class BuildContext
    {
        public ContentDescriptor Descriptor { get; internal set; }
        public ILogger Logger { get; internal set; }
    }
}
