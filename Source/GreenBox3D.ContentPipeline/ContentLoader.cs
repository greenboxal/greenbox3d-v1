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

using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;

namespace GreenBox3D.ContentPipeline
{
    public abstract class ContentLoader<TInput, TOutput> : IContentLoader
    {
        #region Public Methods and Operators

        public abstract TOutput Load(ContentManager manager, TInput input, BuildContext context);

        #endregion

        #region Explicit Interface Methods

        object IContentLoader.Load(ContentManager manager, object input, BuildContext context)
        {
            return Load(manager, (TInput)input, context);
        }

        #endregion
    }
}