using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;

namespace GreenBox3D.ContentPipeline
{
    public abstract class ContentImporter<TOutput> : IContentImporter
    {
        public abstract TOutput Import(string filename, BuildContext context);

        object IContentImporter.Import(string filename, BuildContext context)
        {
            return Import(filename, context);
        }
    }
}
