using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Graphics;
using GreenBox3D.Graphics.Shading;

using ShaderPass = GreenBox3D.Graphics.Shading.ShaderPass;

namespace GreenBox3D.ContentPipeline.Loaders
{
    [ContentLoader]
    internal class ShaderLoader : ContentLoader<CompiledShaderContent, ShaderCollection>
    {
        public override ShaderCollection Load(ContentManager manager, CompiledShaderContent input, BuildContext context)
        {
            ShaderCollection shaders = new ShaderCollection();

            foreach (CompiledShaderEntry entry in input)
            {
                Shader shader = new Shader(manager.GraphicsDevice, entry.Name);

                shader.Version = entry.Version;
                shader.Fallback = entry.Fallback;
                
                foreach (ShaderVariable var in entry.Parameters)
                    shader.Parameters.Add(new ShaderParameter(var.Name, var.Type, var.Size));

                foreach (CompiledPass pass in entry.Passes)
                    shader.Passes.Add(new ShaderPass(manager.GraphicsDevice, pass.VertexShader, pass.PixelShader));

                shaders.Add(shader);
            }

            return shaders;
        }
    }
}
