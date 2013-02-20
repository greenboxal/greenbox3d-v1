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
        #region Public Methods and Operators

        public override ShaderCollection Load(ContentManager manager, CompiledShaderContent input, BuildContext context)
        {
            ShaderCollection shaders = new ShaderCollection();

            foreach (CompiledShaderEntry entry in input)
            {
                Shader shader = new Shader(manager.GraphicsDevice, entry.Name);

                shader.Version = entry.Version;
                shader.Fallback = entry.Fallback;

                shader.Input = new ShaderInput[entry.Input.Count];
                for (int i = 0; i < shader.Input.Length; i++)
                    shader.Input[i] = new ShaderInput(i, entry.Input[i].Usage, entry.Input[i].UsageIndex);

                foreach (ShaderVariable var in entry.Parameters)
                    shader.Parameters.Add(new ShaderParameter(var.Name, var.Type, var.Size));

                foreach (CompiledPass pass in entry.Passes)
                    shader.Passes.Add(new ShaderPass(manager.GraphicsDevice, pass.VertexShader, pass.PixelShader));

                shaders.Add(shader);
            }

            return shaders;
        }

        #endregion
    }
}