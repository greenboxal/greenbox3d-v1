using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics
{
    public class OpenGLException : Exception
    {
        public ErrorCode Error { get; private set; }

        public OpenGLException()
        {
            Error = GL.GetError();
        }

        public OpenGLException(ErrorCode error)
        {
            Error = error;
        }

        public override string ToString()
        {
            // TODO: Better information?
            return Error.ToString();
        }
    }
}
