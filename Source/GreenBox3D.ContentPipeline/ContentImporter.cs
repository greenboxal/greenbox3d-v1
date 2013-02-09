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
        public abstract TOutput Import(Stream stream, BuildContext context);

        object IContentImporter.Import(Stream stream, BuildContext context)
        {
            return Import(stream, context);
        }
    }
}
