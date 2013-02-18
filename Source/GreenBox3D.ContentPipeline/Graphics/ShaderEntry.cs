using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class ShaderEntry
    {
        public string Name { get; private set; }
        public int Version { get; set; }
        public ShaderVariableCollection Input { get; private set; }
        public ShaderVariableCollection Parameters { get; private set; }
        public ShaderVariableCollection Globals { get; private set; }
        public ShaderPassCollection Passes { get; private set; }
        public string Fallback { get; set; }

        public ShaderEntry(string name)
        {
            Name = name;
            Input = new ShaderVariableCollection();
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

                if (input.Value.GetType() == typeof(IronRuby.Builtins.RubyArray))
                    variable = new ShaderVariable((string)input.Value[0], (string)input.Key, input.Value[1]);
                else
                    variable = new ShaderVariable((string)input.Value, (string)input.Key);

                Input.Add(variable);
            }

            foreach (dynamic parameter in shader.pars)
            {
                ShaderVariable variable;

                if (parameter.Value.GetType() == typeof(IronRuby.Builtins.RubyArray))
                    variable = new ShaderVariable((string)parameter.Value[0], (string)parameter.Key, parameter.Value[1]);
                else
                    variable = new ShaderVariable((string)parameter.Value, (string)parameter.Key);

                Parameters.Add(variable);
            }

            foreach (dynamic global in shader.global_list)
            {
                ShaderVariable variable;
            
                if (global.Value.GetType() == typeof(IronRuby.Builtins.RubyArray))
                    variable = new ShaderVariable((string)global.Value[0], (string)global.Key, global.Value[1]);
                else
                    variable = new ShaderVariable((string)global.Value, (string)global.Key);

                Globals.Add(variable);
            }

            foreach (dynamic pass in shader.passes)
                Passes.Add(new ShaderPass(pass));

            Fallback = (string)shader.fallback_shader;
        }
    }
}
