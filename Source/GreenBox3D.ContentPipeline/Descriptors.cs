// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline
{
    public class ImporterDescriptor
    {
        #region Fields

        public ProcessorDescriptor DefaultProcessor;
        public string DisplayName;
        public string[] Extensions;
        public Type Type;

        #endregion

        #region Constructors and Destructors

        public ImporterDescriptor(Type type)
        {
            ContentImporterAttribute info = type.GetCustomAttribute<ContentImporterAttribute>();

            Type = type;
            DisplayName = info.DisplayName;
            Extensions = info.Extensions;
        }

        #endregion

        #region Public Methods and Operators

        public IContentImporter Create()
        {
            return (IContentImporter)Activator.CreateInstance(Type);
        }

        #endregion
    }

    public class ProcessorDescriptor
    {
        #region Fields

        public string DisplayName;
        public Type Input;
        public LoaderDescriptor Loader;
        public Type Output;
        public Type Type;
        public WriterDescriptor Writer;

        #endregion

        #region Constructors and Destructors

        public ProcessorDescriptor(Type type)
        {
            ContentProcessorAttribute info = type.GetCustomAttribute<ContentProcessorAttribute>();

            Type = type;
            Input = type.BaseType.GenericTypeArguments[0];
            Output = type.BaseType.GenericTypeArguments[1];
            DisplayName = info.DisplayName;
        }

        #endregion

        #region Public Methods and Operators

        public IContentProcessor Create()
        {
            return (IContentProcessor)Activator.CreateInstance(Type);
        }

        #endregion
    }

    public class LoaderDescriptor
    {
        #region Fields

        public Type Loadee;
        public Type Type;

        #endregion

        #region Constructors and Destructors

        public LoaderDescriptor(Type type)
        {
            Type = type;
            Loadee = type.BaseType.GenericTypeArguments[0];
        }

        #endregion

        #region Public Methods and Operators

        public IContentLoader Create()
        {
            return (IContentLoader)Activator.CreateInstance(Type);
        }

        #endregion
    }

    public class WriterDescriptor
    {
        #region Fields

        public string Extension;
        public Type Type;

        #endregion

        #region Constructors and Destructors

        public WriterDescriptor(Type type)
        {
            ContentTypeWriterAttribute attribute = type.GetCustomAttribute<ContentTypeWriterAttribute>();

            Type = type;
            Extension = attribute.Extension;
        }

        #endregion

        #region Public Methods and Operators

        public IContentTypeWriter Create()
        {
            return (IContentTypeWriter)Activator.CreateInstance(Type);
        }

        #endregion
    }
}