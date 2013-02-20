// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Graphics;

namespace GreenBox3D.ContentPipeline.Importers
{
    [ContentImporter(".shader", DisplayName = "Shader Importer", DefaultProcessor = "ShaderProcessor")]
    public class ShaderImporter : ContentImporter<ShaderContent>
    {
        #region Public Methods and Operators

        public override ShaderContent Import(string filename, BuildContext context)
        {
            ShaderCompiler compiler = new ShaderCompiler();

            compiler.Compile(filename);

            return compiler.Build();
        }

        #endregion
    }
}