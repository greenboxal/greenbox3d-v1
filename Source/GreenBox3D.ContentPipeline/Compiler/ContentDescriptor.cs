// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Dynamic;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public class ContentDescriptor
    {
        #region Constructors and Destructors

        public ContentDescriptor(string root, string relativePath)
        {
            RelativePath = relativePath;
            Path = System.IO.Path.Combine(root, relativePath);
            Properties = new OpenStruct();
        }

        #endregion

        #region Public Properties

        public string Path { get; private set; }
        public dynamic Properties { get; private set; }
        public string RelativePath { get; private set; }

        #endregion
    }
}