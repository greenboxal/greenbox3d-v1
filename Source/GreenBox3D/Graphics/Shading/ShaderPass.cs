using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics.Shading
{
    public class ShaderPass
    {
        public int ProgramID { get; set; }
        public int VertexShader { get; set; }
        public string VertexCode { get; private set; }
        public int PixelShader { get; set; }
        public string PixelCode { get; private set; }

        public bool Created { get; private set; }
        public bool IsValid { get; private set; }

        public ShaderPass(string vertex, string pixel)
        {
            ProgramID = -1;
            VertexShader = -1;
            VertexCode = vertex;
            PixelShader = -1;
            PixelCode = pixel;
        }

        // TODO: Error handling and debug
        public void Create()
        {
            int status;

            Created = true;

            if (string.IsNullOrEmpty(VertexCode) || string.IsNullOrEmpty(PixelCode))
                return;

            ProgramID = GL.CreateProgram();
            if (ProgramID == -1)
                throw new OpenGLException();

            VertexShader = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.VertexShader);
            if (VertexShader == -1)
                throw new OpenGLException();

            GL.ShaderSource(VertexShader, VertexCode);
            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, OpenTK.Graphics.OpenGL.ShaderParameter.CompileStatus, out status);
            if (status == 0)
                throw new Exception(GL.GetShaderInfoLog(VertexShader));

            PixelShader = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.FragmentShader);
            if (PixelShader == -1)
                throw new OpenGLException();

            GL.ShaderSource(PixelShader, PixelCode);
            GL.CompileShader(PixelShader);

            GL.GetShader(PixelShader, OpenTK.Graphics.OpenGL.ShaderParameter.CompileStatus, out status);
            if (status == 0)
                throw new Exception(GL.GetShaderInfoLog(PixelShader));

            GL.AttachShader(ProgramID, VertexShader);
            GL.AttachShader(ProgramID, PixelShader);
            GL.LinkProgram(ProgramID);

            GL.GetProgram(ProgramID, ProgramParameter.LinkStatus, out status);
            if (status == 0)
                throw new Exception(GL.GetProgramInfoLog(ProgramID));

            IsValid = true;
        }
    }
}
