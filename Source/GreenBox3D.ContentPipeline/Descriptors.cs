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
        public Type Type;
        public string DisplayName;
        public string[] Extensions;
        public ProcessorDescriptor DefaultProcessor;

        public ImporterDescriptor(Type type)
        {
            ContentImporterAttribute info = type.GetCustomAttribute<ContentImporterAttribute>();

            Type = type;
            DisplayName = info.DisplayName;
            Extensions = info.Extensions;
        }

        public IContentImporter Create()
        {
            return (IContentImporter)Activator.CreateInstance(Type);
        }
    }

    public class ProcessorDescriptor
    {
        public Type Type;
        public Type Input;
        public Type Output;
        public string DisplayName;
        public WriterDescriptor Writer;
        public LoaderDescriptor Loader;

        public ProcessorDescriptor(Type type)
        {
            ContentProcessorAttribute info = type.GetCustomAttribute<ContentProcessorAttribute>();

            Type = type;
            Input = type.BaseType.GenericTypeArguments[0];
            Output = type.BaseType.GenericTypeArguments[1];
            DisplayName = info.DisplayName;
        }

        public IContentProcessor Create()
        {
            return (IContentProcessor)Activator.CreateInstance(Type);
        }
    }

    public class LoaderDescriptor
    {
        public Type Type;
        public Type Loadee;

        public LoaderDescriptor(Type type)
        {
            Type = type;
            Loadee = type.BaseType.GenericTypeArguments[0];
        }

        public IContentLoader Create()
        {
            return (IContentLoader)Activator.CreateInstance(Type);
        }
    }

    public class WriterDescriptor
    {
        public Type Type;
        public string Extension;

        public WriterDescriptor(Type type)
        {
            ContentTypeWriterAttribute attribute = type.GetCustomAttribute<ContentTypeWriterAttribute>();

            Type = type;
            Extension = attribute.Extension;
        }

        public IContentTypeWriter Create()
        {
            return (IContentTypeWriter)Activator.CreateInstance(Type);
        }
    }
}
