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

using GreenBox3D.Graphics;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class ShaderEntryInput
    {
        #region Constructors and Destructors

        public ShaderEntryInput(VertexElementUsage usage, int usageIndex, ShaderVariable variable)
        {
            Usage = usage;
            UsageIndex = usageIndex;
            Variable = variable;
        }

        #endregion

        #region Public Properties

        public VertexElementUsage Usage { get; private set; }
        public int UsageIndex { get; private set; }
        public ShaderVariable Variable { get; private set; }

        #endregion
    }
}