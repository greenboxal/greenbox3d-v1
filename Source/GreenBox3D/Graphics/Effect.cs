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
        private readonly Shader _shader;

        public EffectParameterCollection Parameters { get; private set; }
        public EffectPassCollection Passes { get; private set; }

        internal byte[] ParameterData { get; set; }

        internal Effect(Shader shader)
        {
            _shader = shader;
            ParameterData = new byte[_shader.ParametersSize];

            EffectParameter[] parameters = new EffectParameter[_shader.Parameters.Count];
            for (int i = 0; i < _shader.Parameters.Count; i++)
                parameters[i] = new EffectParameter(this, _shader.Parameters[i]);

            EffectPass[] passes = new EffectPass[_shader.Passes.Count];
            for (int i = 0; i < _shader.Passes.Count; i++)
                passes[i] = new EffectPass(this, _shader.Passes[i]);

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
    }
}
