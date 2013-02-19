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
        public ShaderPassCollection Passes { get; private set; }

        public ShaderParameterCollection Parameters { get; private set; }
        public int ParametersSize { get; private set; }

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
                int textureCounter = 0;

                foreach (ShaderParameter parameter in Parameters)
                {
                    parameter.Offset = ParametersSize;
                    parameter.Slot = GL.GetUniformLocation(Passes[0].ProgramID, "u" + parameter.Name);
                    ParametersSize += parameter.ByteSize;

                    if (parameter.Class == EffectParameterClass.Sampler)
                    {
                        int count = parameter.Count == 0 ? 1 : parameter.Count;
                        parameter.TextureUnit = textureCounter;

                        if (textureCounter + count > GraphicsDevice.Textures.Count)
                        {
                            IsValid = false;
                            break;
                        }

                        textureCounter += count;
                    }
                }
            }

            Created = true;
        }
    }
}
