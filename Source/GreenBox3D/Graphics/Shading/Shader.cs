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

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics.Shading
{
    public class Shader : GraphicsResource
    {
        #region Constructors and Destructors

        public Shader(GraphicsDevice graphicsDevice, string name)
            : base(graphicsDevice)
        {
            Name = name;
            Version = 110;
            Fallback = null;
            Passes = new ShaderPassCollection();
            Parameters = new ShaderParameterCollection();
            Created = false;
            IsValid = false;
        }

        #endregion

        #region Public Properties

        public bool Created { get; private set; }
        public string Fallback { get; set; }
        public ShaderInput[] Input { get; set; }
        public bool IsValid { get; private set; }
        public string Name { get; private set; }
        public ShaderParameterCollection Parameters { get; private set; }
        public int ParametersSize { get; private set; }
        public ShaderPassCollection Passes { get; private set; }
        public int Version { get; set; }

        #endregion

        #region Public Methods and Operators

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

        public int GetInputIndex(VertexElementUsage usage, int usageIndex)
        {
            foreach (ShaderInput input in Input)
            {
                if (input.Usage == usage && input.UsageIndex == usageIndex)
                    return input.Index;
            }

            return -1;
        }

        #endregion
    }
}