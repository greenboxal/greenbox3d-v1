﻿// GreenBox3D
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

namespace GreenBox3D.Graphics
{
    public class Effect
    {
        #region Fields

        internal readonly Shader Shader;

        #endregion

        #region Constructors and Destructors

        internal Effect(Shader shader)
        {
            Shader = shader;
            ParameterData = new byte[Shader.ParametersSize];

            EffectParameter[] parameters = new EffectParameter[Shader.Parameters.Count];
            for (int i = 0; i < Shader.Parameters.Count; i++)
                parameters[i] = new EffectParameter(this, Shader.Parameters[i]);

            EffectPass[] passes = new EffectPass[Shader.Passes.Count];
            for (int i = 0; i < Shader.Passes.Count; i++)
                passes[i] = new EffectPass(this, Shader.Passes[i]);

            Parameters = new EffectParameterCollection(parameters);
            Passes = new EffectPassCollection(passes);

            foreach (EffectParameter parameter in Parameters)
            {
                if (parameter.Class == EffectParameterClass.Sampler)
                {
                    int count = parameter.Parameter.Count == 0 ? 1 : parameter.Parameter.Count;
                    int[] units = new int[count];

                    for (int i = 0; i < count; i++)
                        units[i] = parameter.Parameter.TextureUnit + i;

                    parameter.SetValue(count);
                }
            }
        }

        #endregion

        #region Public Properties

        public EffectParameterCollection Parameters { get; private set; }
        public EffectPassCollection Passes { get; private set; }

        #endregion

        #region Properties

        internal byte[] ParameterData { get; set; }

        #endregion
    }
}