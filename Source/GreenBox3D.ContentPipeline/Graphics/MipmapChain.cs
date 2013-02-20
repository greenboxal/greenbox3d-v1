// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public class MipmapChain : Collection<BitmapContent>
    {
        #region Public Methods and Operators

        public static implicit operator MipmapChain(BitmapContent bitmap)
        {
            MipmapChain chain = new MipmapChain();
            chain.Add(bitmap);
            return chain;
        }

        #endregion
    }
}