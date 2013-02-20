// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

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
        #region Public Methods and Operators

        public abstract TOutput Process(TInput input, BuildContext context);

        #endregion

        #region Explicit Interface Methods

        object IContentProcessor.Process(object input, BuildContext context)
        {
            return Process((TInput)input, context);
        }

        #endregion
    }
}