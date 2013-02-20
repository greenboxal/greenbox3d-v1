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

namespace GreenBox3D.ContentPipeline.Writers
{
    [ContentTypeWriter(Extension = ".fx")]
    public class ShaderTypeWriter : ContentTypeWriter<CompiledShaderContent>
    {
        #region Methods

        protected override ContentHeader GetHeader(CompiledShaderContent input, BuildContext context)
        {
            ShouldCompress = true;
            return new ContentHeader("FX", new Version(1, 0), Encoding.UTF8);
        }

        protected override void Write(ContentWriter stream, CompiledShaderContent input, BuildContext context)
        {
            stream.Write(input.Count);

            foreach (CompiledShaderEntry shader in input)
            {
                stream.Write(shader.Name);
                stream.Write(shader.Version);
                stream.Write(shader.Fallback ?? "");
                stream.Write(shader.Input.Count);
                stream.Write(shader.Parameters.Count);
                stream.Write(shader.Globals.Count);
                stream.Write(shader.Passes.Count);

                foreach (ShaderEntryInput i in shader.Input)
                {
                    stream.Write(i.Variable.Type);
                    stream.Write(i.Variable.Name);
                    stream.Write(i.Variable.Size);
                    stream.Write((int)i.Usage);
                    stream.Write(i.UsageIndex);
                }

                WriteVariables(stream, shader.Parameters);
                WriteVariables(stream, shader.Globals);

                foreach (CompiledPass pass in shader.Passes)
                {
                    stream.Write(pass.VertexShader);
                    stream.Write(pass.PixelShader);
                }
            }
        }

        private void WriteVariables(ContentWriter stream, IEnumerable<ShaderVariable> variables)
        {
            foreach (ShaderVariable variable in variables)
            {
                stream.Write(variable.Type);
                stream.Write(variable.Name);
                stream.Write(variable.Size);
            }
        }

        #endregion
    }
}