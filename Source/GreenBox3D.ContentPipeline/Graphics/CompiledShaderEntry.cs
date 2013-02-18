using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class CompiledShaderEntry
    {
        public string Name { get; private set; }
        public int Version { get; set; }
        public ShaderVariableCollection Input { get; private set; }
        public ShaderVariableCollection Parameters { get; private set; }
        public ShaderVariableCollection Globals { get; private set; }
        public CompiledPassCollection Passes { get; private set; }
        public string Fallback { get; set; }

        public CompiledShaderEntry(string name)
        {
            Name = name;
            Input = new ShaderVariableCollection();
            Parameters = new ShaderVariableCollection();
            Globals = new ShaderVariableCollection();
            Passes = new CompiledPassCollection();
            Fallback = null;
        }
    }
}
