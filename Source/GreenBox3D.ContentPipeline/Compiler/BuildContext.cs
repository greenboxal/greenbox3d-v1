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
    public class BuildContext
    {
        #region Public Properties

        public ContentDescriptor Descriptor { get; internal set; }
        public ILogger Logger { get; internal set; }

        #endregion
    }
}