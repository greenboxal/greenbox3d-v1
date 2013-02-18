using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics.Shading
{
    internal class Shader : GraphicsResource
    {
        public string Name { get; private set; }
        public int Version { get; set; }
        public string Fallback { get; set; }
        public ShaderParameterCollection Parameters { get; private set; }
        public ShaderPassCollection Passes { get; private set; }

        public bool Created { get; private set; }
        public bool IsValid { get; private set; }

        public Shader(GraphicsDevice graphicsDevice, string name)
            : base(graphicsDevice)
        {
            Name = name;
            Version = 110;
            Fallback = null;
            Parameters = new ShaderParameterCollection();
            Passes = new ShaderPassCollection();
            Created = false;
            IsValid = false;
        }

        public void Create()
        {
            IsValid = true;

            foreach (ShaderPass pass in Passes)
            {
                pass.Create();
                IsValid &= pass.IsValid;
            }

            if (IsValid)
            {
                foreach (ShaderParameter parameter in Parameters)
                    parameter.Slot = GL.GetUniformLocation(Passes[0].ProgramID, "u" + parameter.Name);
            }

            Created = true;
        }
    }
}
