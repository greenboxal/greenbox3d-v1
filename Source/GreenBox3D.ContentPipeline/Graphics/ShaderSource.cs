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

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class ShaderSource
    {
        #region Constructors and Destructors

        public ShaderSource(string source, string filename, int line)
        {
            Source = source;
            Filename = filename;
            IncludeLine = line;
        }

        #endregion

        #region Public Properties

        public string Filename { get; private set; }
        public int IncludeLine { get; private set; }
        public string Source { get; private set; }

        #endregion
    }
}