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

using GreenBox3D.Graphics.Shading;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class CompiledShaderEntry
    {
        #region Constructors and Destructors

        public CompiledShaderEntry(string name)
        {
            Name = name;
            Input = new ShaderEntryInputCollection();
            Parameters = new ShaderVariableCollection();
            Globals = new ShaderVariableCollection();
            Passes = new CompiledPassCollection();
            Fallback = null;
        }

        #endregion

        #region Public Properties

        public string Fallback { get; set; }
        public ShaderVariableCollection Globals { get; private set; }
        public ShaderEntryInputCollection Input { get; private set; }
        public string Name { get; private set; }
        public ShaderVariableCollection Parameters { get; private set; }
        public CompiledPassCollection Passes { get; private set; }
        public int Version { get; set; }

        #endregion
    }
}