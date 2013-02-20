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

namespace GreenBox3D.ContentPipeline
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ContentProcessorAttribute : Attribute
    {
        #region Constructors and Destructors

        public ContentProcessorAttribute(Type writer)
        {
            Writer = writer;
        }

        #endregion

        #region Public Properties

        public string DisplayName { get; set; }
        public Type Loader { get; set; }
        public Type Writer { get; set; }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ContentImporterAttribute : Attribute
    {
        #region Constructors and Destructors

        public ContentImporterAttribute(params string[] extensions)
        {
            Extensions = extensions;
        }

        #endregion

        #region Public Properties

        public string DefaultProcessor { get; set; }
        public string DisplayName { get; set; }
        public string[] Extensions { get; set; }

        #endregion
    }

    public sealed class ContentTypeWriterAttribute : Attribute
    {
        #region Constructors and Destructors

        public ContentTypeWriterAttribute()
        {
            Extension = ".bin";
        }

        #endregion

        #region Public Properties

        public string Extension { get; set; }

        #endregion
    }

    public sealed class ContentLoaderAttribute : Attribute
    {
    }
}