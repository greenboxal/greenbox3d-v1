using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class ShaderPass
    {
        public ShaderSourceCollection Headers { get; private set; }
        public ShaderSourceCollection Include { get; private set; }
        public ShaderSource Vertex { get; set; }
        public ShaderSource Pixel { get; set; }

        public ShaderPass()
        {
            Headers = new ShaderSourceCollection();
            Include = new ShaderSourceCollection();
        }

        public ShaderPass(dynamic pass)
            : this()
        {
            foreach (dynamic header in pass.headers)
                Headers.Add(new ShaderSource((string)header[0], (string)header[1], header[2]));

            foreach (dynamic include in pass.includes)
                Include.Add(new ShaderSource((string)include[0], (string)include[1], include[2]));

            Vertex = new ShaderSource((string)pass.vertex_code, (string)pass.vertex_fname, pass.vertex_line);
            Pixel = new ShaderSource((string)pass.pixel_code, (string)pass.pixel_fname, pass.pixel_line);
        }
    }
}
