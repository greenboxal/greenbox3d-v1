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

using GreenBox3D.Graphics;

using IronRuby.Builtins;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class ShaderEntry
    {
        #region Constructors and Destructors

        public ShaderEntry(string name)
        {
            Name = name;
            Input = new ShaderEntryInputCollection();
            Parameters = new ShaderVariableCollection();
            Globals = new ShaderVariableCollection();
            Passes = new ShaderPassCollection();
            Fallback = null;
        }

        internal ShaderEntry(dynamic shader)
            : this((string)shader.name)
        {
            Version = shader.glsl_version;

            foreach (dynamic input in shader.inputs)
            {
                ShaderVariable variable;
                VertexElementUsage elementUsage;

                dynamic type = input.Value[0];
                string usage = (string)input.Value[1];
                int index = 0;

                if (input.Value.Count >= 3)
                    index = input.Value[2];

                if (type.GetType() == typeof(RubyArray))
                    variable = new ShaderVariable((string)type[0], (string)input.Key, type[1]);
                else
                    variable = new ShaderVariable((string)type, (string)input.Key);

                if (!Enum.TryParse(usage, true, out elementUsage))
                    throw new InvalidDataException("Invalid shader input usage for '" + variable.Name + "'");

                Input.Add(new ShaderEntryInput(elementUsage, index, variable));
            }

            foreach (dynamic parameter in shader.pars)
            {
                ShaderVariable variable;

                if (parameter.Value.GetType() == typeof(RubyArray))
                    variable = new ShaderVariable((string)parameter.Value[0], (string)parameter.Key, parameter.Value[1]);
                else
                    variable = new ShaderVariable((string)parameter.Value, (string)parameter.Key);

                Parameters.Add(variable);
            }

            foreach (dynamic global in shader.global_list)
            {
                ShaderVariable variable;

                if (global.Value.GetType() == typeof(RubyArray))
                    variable = new ShaderVariable((string)global.Value[0], (string)global.Key, global.Value[1]);
                else
                    variable = new ShaderVariable((string)global.Value, (string)global.Key);

                Globals.Add(variable);
            }

            foreach (dynamic pass in shader.passes)
                Passes.Add(new ShaderPass(pass));

            Fallback = (string)shader.fallback_shader;
        }

        #endregion

        #region Public Properties

        public string Fallback { get; set; }
        public ShaderVariableCollection Globals { get; private set; }
        public ShaderEntryInputCollection Input { get; private set; }
        public string Name { get; private set; }
        public ShaderVariableCollection Parameters { get; private set; }
        public ShaderPassCollection Passes { get; private set; }
        public int Version { get; set; }

        #endregion
    }
}