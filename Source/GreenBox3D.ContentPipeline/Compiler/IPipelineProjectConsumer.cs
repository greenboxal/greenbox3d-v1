using System;
using System.Collections.Generic;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public interface IPipelineProjectConsumer
    {
        void AddReference(string name);
        void AddContent(ContentDescriptor content);
    }
}
