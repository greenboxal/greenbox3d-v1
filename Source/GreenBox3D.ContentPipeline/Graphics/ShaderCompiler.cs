using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;

using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class ShaderCompiler
    {
        private static readonly ScriptSource HelperSource;

        static ShaderCompiler()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GreenBox3D.ContentPipeline.Graphics.ShaderCompiler.rb");
            StreamReader reader = new StreamReader(stream);
            
            HelperSource = ScriptManager.Engine.CreateScriptSourceFromString(reader.ReadToEnd(), SourceCodeKind.File);

            reader.Close();
        }

        private readonly dynamic _compilerEngine;

        public ShaderCompiler()
        {
            ScriptScope scope = ScriptManager.CreateScope();
            HelperSource.Execute(scope);
            _compilerEngine = ScriptManager.Engine.Execute("ShaderLib::Compiler.new", scope);
        }

        public void Compile(string filename)
        {
            Compile(File.ReadAllText(filename), filename);
        }

        public void Compile(string code, string filename)
        {
            _compilerEngine.compile(code, filename, 0);
        }

        public ShaderContent Build()
        {
            ShaderContent content = new ShaderContent();
            dynamic units = _compilerEngine.units;

            foreach (dynamic unit in units)
            {
                foreach (dynamic shader in unit.shaders)
                {
                    content.Add(new ShaderEntry(shader));
                }
            }

            return content;
        }
    }
}
