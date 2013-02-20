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
    public class Texture2DContent : TextureContent
    {
        #region Constructors and Destructors

        public Texture2DContent()
            : base(new MipmapChainCollection { new MipmapChain() })
        {
        }

        #endregion

        #region Public Properties

        public MipmapChain Mipmaps { get { return Faces[0]; } set { Faces[0] = value; } }

        #endregion
    }
}