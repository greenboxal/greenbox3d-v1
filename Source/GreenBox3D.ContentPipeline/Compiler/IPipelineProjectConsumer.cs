// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public interface IPipelineProjectConsumer
    {
        #region Public Methods and Operators

        void AddContent(ContentDescriptor content);
        void AddReference(string name);

        #endregion
    }
}