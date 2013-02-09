using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.ContentPipeline.Compiler;

namespace GreenBox3D.ContentPipeline
{
    public abstract class ContentProcessor<TInput, TOutput> : IContentProcessor
    {
        public abstract TOutput Process(TInput input, BuildContext context);

        object IContentProcessor.Process(object input, BuildContext context)
        {
            return Process((TInput)input, context);
        }
    }
}
