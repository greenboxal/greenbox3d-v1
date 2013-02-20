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
using GreenBox3D.ContentPipeline.Graphics;
using GreenBox3D.ContentPipeline.Loaders;
using GreenBox3D.ContentPipeline.Writers;
using GreenBox3D.Graphics;
using GreenBox3D.Graphics.Shading;

using ShaderPass = GreenBox3D.ContentPipeline.Graphics.ShaderPass;

namespace GreenBox3D.ContentPipeline.Processors
{
    [ContentProcessor(typeof(ShaderTypeWriter), Loader = typeof(ShaderLoader), DisplayName = "Shader Processor")]
    public class ShaderProcessor : ContentProcessor<ShaderContent, CompiledShaderContent>
    {
        #region Public Methods and Operators

        public static CompiledPass CompilePass(CompiledShaderEntry entry, ShaderPass pass)
        {
            CompiledPass cp = new CompiledPass();

            cp.VertexShader = BuildPassStub(entry, pass, pass.Vertex, ShaderType.Vertex);
            cp.PixelShader = BuildPassStub(entry, pass, pass.Pixel, ShaderType.Fragment);

            return cp;
        }

        public override CompiledShaderContent Process(ShaderContent input, BuildContext context)
        {
            CompiledShaderContent content = new CompiledShaderContent();

            foreach (ShaderEntry entry in input)
            {
                CompiledShaderEntry compiled = new CompiledShaderEntry(entry.Name);

                compiled.Version = entry.Version;

                foreach (ShaderEntryInput var in entry.Input)
                    compiled.Input.Add(new ShaderEntryInput(var.Usage, var.UsageIndex, Captalize(var.Variable)));

                foreach (ShaderVariable var in entry.Parameters)
                    compiled.Parameters.Add(Captalize(var));

                foreach (ShaderVariable var in entry.Globals)
                    compiled.Globals.Add(Captalize(var));

                foreach (ShaderPass pass in entry.Passes)
                    compiled.Passes.Add(CompilePass(compiled, pass));

                compiled.Fallback = entry.Fallback;
                content.Add(compiled);
            }

            return content;
        }

        #endregion

        #region Methods

        private static string BuildPassStub(CompiledShaderEntry entry, ShaderPass pass, ShaderSource source, ShaderType type)
        {
            StringBuilder c = new StringBuilder();
            string attribute;
            string varying;

            if (entry.Version >= 130)
            {
                attribute = "in";
                varying = type == ShaderType.Vertex ? "out" : "in";
            }
            else
            {
                attribute = "attribute";
                varying = "varying";
            }

            c.AppendFormat("#version {0}\n", entry.Version);

            foreach (ShaderSource header in pass.Headers)
                c.AppendLine(header.Source);

            if (type == ShaderType.Vertex)
            {
                for (int i = 0; i < entry.Input.Count; i++)
                    c.AppendFormat("layout(location = {0}) {1} {2};\n", i, attribute, BuildVariable(entry.Input[i].Variable, "i"));
            }

            foreach (ShaderVariable parameter in entry.Parameters)
                c.AppendFormat("uniform {0};\n", BuildVariable(parameter, "u"));

            foreach (ShaderVariable global in entry.Globals)
                c.AppendFormat("{0} {1};\n", varying, BuildVariable(global, "g"));

            foreach (ShaderSource include in pass.Include)
                c.AppendLine(include.Source);

            c.AppendLine(source.Source);

            return c.ToString();
        }

        private static string BuildVariable(ShaderVariable var, string prefix)
        {
            string code = var.Type + " " + prefix + var.Name;

            if (var.Size > 0)
                code += "[" + var.Size + "]";

            return code;
        }

        private static ShaderVariable Captalize(ShaderVariable s)
        {
            return new ShaderVariable(s.Type, Captalize(s.Name), s.Size);
        }

        private static string Captalize(string s)
        {
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        #endregion
    }
}