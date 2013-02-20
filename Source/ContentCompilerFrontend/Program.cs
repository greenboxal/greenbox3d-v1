// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;

namespace ContentCompilerFrontend
{
    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            PipelineProject project = new PipelineProject("ContentProject.rb");
            PipelineCompiler compiler = new PipelineCompiler(project);
            compiler.OutputPath = "Output";
            compiler.Compile();
        }

        #endregion
    }
}