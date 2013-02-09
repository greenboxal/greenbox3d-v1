using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;

namespace GreenBox3D.ContentPipeline
{
    public abstract class ContentLoader<TInput, TOutput> : IContentLoader
    {
        public abstract TOutput Load(ContentManager manager, TInput input, BuildContext context);

        object IContentLoader.Load(ContentManager manager, object input, BuildContext context)
        {
            return Load(manager, (TInput)input, context);
        }
    }
}
