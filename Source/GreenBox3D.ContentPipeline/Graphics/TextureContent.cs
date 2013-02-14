﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics;

namespace GreenBox3D.ContentPipeline.Graphics
{
    public abstract class TextureContent
    {
        public MipmapChainCollection Faces { get; private set; }

        protected TextureContent(MipmapChainCollection faces)
        {
            Faces = faces;
        }

        public virtual void GenerateMipmaps(bool overwrite)
        {
            
        }

        public virtual void Validate()
        {
            if (Faces == null || Faces.Count == 0)
                throw new InvalidDataException("Invalid face data");

            SurfaceFormat format = SurfaceFormat.Color;
            for (int i = 0; i < Faces.Count; i++)
            {
                if (Faces[i].Count == 0)
                    throw new InvalidDataException("Face " + i + " doesn't have any BitmapContent");

                if (i == 0)
                    if (!Faces[i][0].TryGetFormat(out format))
                        throw new Exception("Face " + i + " has an invalid image format");

                foreach (BitmapContent content in Faces[i])
                {
                    SurfaceFormat format2;

                    if (!content.TryGetFormat(out format2))
                        throw new Exception("Face " + i + " has an invalid image format");

                    if (format2 != format)
                        throw new Exception("Face " + i + " doesn't have a consistent mipmap format");
                }
            }
        }
    }
}
