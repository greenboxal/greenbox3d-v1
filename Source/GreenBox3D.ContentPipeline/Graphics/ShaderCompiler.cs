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
        #region Static Fields

        private static readonly ScriptSource HelperSource;

        #endregion

        #region Fields

        private readonly dynamic _compilerEngine;

        #endregion

        #region Constructors and Destructors

        static ShaderCompiler()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GreenBox3D.ContentPipeline.Graphics.ShaderCompiler.rb");
            StreamReader reader = new StreamReader(stream);

            HelperSource = ScriptManager.Engine.CreateScriptSourceFromString(reader.ReadToEnd(), SourceCodeKind.File);

            reader.Close();
        }

        public ShaderCompiler()
        {
            ScriptScope scope = ScriptManager.CreateScope();
            HelperSource.Execute(scope);
            _compilerEngine = ScriptManager.Engine.Execute("ShaderLib::Compiler.new", scope);
        }

        #endregion

        #region Public Methods and Operators

        public ShaderContent Build()
        {
            ShaderContent content = new ShaderContent();
            dynamic units = _compilerEngine.units;

            foreach (dynamic unit in units)
            {
                foreach (dynamic shader in unit.shaders)
                    content.Add(new ShaderEntry(shader));
            }

            return content;
        }

        public void Compile(string filename)
        {
            Compile(File.ReadAllText(filename), filename);
        }

        public void Compile(string code, string filename)
        {
            _compilerEngine.compile(code, filename, 0);
        }

        #endregion
    }
}