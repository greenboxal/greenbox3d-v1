using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics.Shading;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics
{
    public class EffectPass
    {
        private static Effect _lastEffect;
        private static ShaderPass _lastEffectPass;

        public static void ResetActiveProgramCache()
        {
            _lastEffect = null;
            _lastEffectPass = null;
        }

        private readonly Effect _owner;
        private readonly ShaderPass _pass;

        internal EffectPass(Effect owner, ShaderPass pass)
        {
            _owner = owner;
            _pass = pass;
        }

        public void Apply()
        {
            bool applyTextures = false;

            if (_lastEffectPass != _pass)
            {
                GL.UseProgram(_pass.ProgramID);
                _lastEffectPass = _pass;
            }

            unsafe
            {
                fixed (byte* ptr = &_owner.ParameterData[0])
                {
                    foreach (EffectParameter parameter in _owner.Parameters)
                    {
                        if (parameter.Parameter.Slot == -1)
                            continue;

                        if (_lastEffect != _owner || parameter.Dirty)
                            parameter.Apply(ptr);

                        if (parameter.Class == EffectParameterClass.Sampler && parameter.Textures != null)
                        {
                            for (int i = 0; i < parameter.Textures.Length; i++)
                            {
                                _pass.GraphicsDevice.Textures[parameter.Parameter.TextureUnit + i] = parameter.Textures[i];
                                applyTextures = true;
                            }
                        }
                    }
                }
            }

            if (applyTextures)
                _pass.GraphicsDevice.Textures.Apply();

            _pass.GraphicsDevice.ActiveShader = _owner.Shader;
            _lastEffect = _owner;
        }
    }
}
