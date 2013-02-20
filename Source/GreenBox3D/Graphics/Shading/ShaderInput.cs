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

namespace GreenBox3D.Graphics.Shading
{
    public class ShaderInput
    {
        #region Constructors and Destructors

        public ShaderInput(int index, VertexElementUsage usage, int usageIndex)
        {
            Index = index;
            Usage = usage;
            UsageIndex = usageIndex;
        }

        #endregion

        #region Public Properties

        public int Index { get; private set; }
        public VertexElementUsage Usage { get; private set; }
        public int UsageIndex { get; private set; }

        #endregion
    }
}