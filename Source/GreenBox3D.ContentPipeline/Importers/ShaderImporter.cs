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
        public override ShaderContent Import(string filename, BuildContext context)
        {
            ShaderCompiler compiler = new ShaderCompiler();

            compiler.Compile(filename);

            return compiler.Build();
        }
    }
}
