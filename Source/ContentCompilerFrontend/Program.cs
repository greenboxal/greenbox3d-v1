using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;

namespace ContentCompilerFrontend
{
    class Program
    {
        static void Main(string[] args)
        {
            PipelineProject project = new PipelineProject("ContentProject.rb");
            PipelineCompiler compiler = new PipelineCompiler(project);
            compiler.OutputPath = "Output";
            compiler.Compile();
        }
    }
}
