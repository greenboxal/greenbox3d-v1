using System;
using System.Collections.Generic;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public interface IPipelineProject
    {
        string ProjectBase { get; }

        void Consume(IPipelineProjectConsumer consumer);
    }
}
