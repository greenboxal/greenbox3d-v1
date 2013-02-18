using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics.Shading;

namespace GreenBox3D.Graphics
{
    public class Effect : GraphicsResource
    {
        private readonly Shader _shader;

        public EffectParameterCollection Parameters { get; private set; }
        public EffectPassCollection Passes { get; private set; }

        internal Effect(GraphicsDevice device, Shader shader)
            : base(device)
        {
            _shader = shader;

            EffectParameter[] parameters = new EffectParameter[_shader.Parameters.Count];
            for (int i = 0; i < _shader.Parameters.Count; i++)
                parameters[i] = new EffectParameter(_shader.Parameters[i]);

            EffectPass[] passes = new EffectPass[_shader.Passes.Count];
            for (int i = 0; i < _shader.Passes.Count; i++)
                passes[i] = new EffectPass(this, _shader.Passes[i]);

            Parameters = new EffectParameterCollection(parameters);
            Passes = new EffectPassCollection(passes);
        }

        protected override void Dispose(bool disposing)
        {
            foreach (EffectParameter parameter in Parameters)
                parameter.Dispose();

            base.Dispose(disposing);
        }
    }
}
