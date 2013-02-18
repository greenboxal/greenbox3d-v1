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
        private static EffectPass _lastPass;

        private readonly Effect _owner;
        private readonly ShaderPass _pass;

        internal EffectPass(Effect owner, ShaderPass pass)
        {
            _owner = owner;
            _pass = pass;
        }

        public void Apply()
        {
            if (_lastPass == this)
                return;

            GL.UseProgram(_pass.ProgramID);
            foreach (EffectParameter parameter in _owner.Parameters)
            {
                if (parameter.Parameter.Slot == -1)
                    continue;

                unsafe
                {
                    if (parameter.Parameter.Type == EffectParameterType.Double)
                    {
                        GL.Uniform1(parameter.Parameter.Slot, parameter.Parameter.RowCount * parameter.Parameter.ColumnCount, (double*)parameter.Handle.AddrOfPinnedObject());
                    }
                    else
                    {
                        GL.Uniform1(parameter.Parameter.Slot, parameter.Parameter.RowCount * parameter.Parameter.ColumnCount, (int*)parameter.Handle.AddrOfPinnedObject());
                    }
                }
            }

            _lastPass = this;
        }
    }
}
