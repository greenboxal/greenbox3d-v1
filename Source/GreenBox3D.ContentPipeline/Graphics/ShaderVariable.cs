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

using GreenBox3D.Graphics.Shading;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class ShaderVariable
    {
        #region Constructors and Destructors

        public ShaderVariable(string type, string name, int size = 0)
        {
            Type = type;
            Name = name;
            Size = size;
        }

        #endregion

        #region Public Properties

        public string Name { get; private set; }
        public int Size { get; private set; }
        public string Type { get; private set; }

        #endregion
    }
}