// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;

namespace GreenBox3D.ContentPipeline
{
    public interface IContentLoader
    {
        #region Public Methods and Operators

        object Load(ContentManager manager, object input, BuildContext context);

        #endregion
    }
}