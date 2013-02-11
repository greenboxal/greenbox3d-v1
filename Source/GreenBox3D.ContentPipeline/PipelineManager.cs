using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;

namespace GreenBox3D.ContentPipeline
{
    public static class PipelineManager
    {
        private static readonly List<ImporterDescriptor> Importers;
        private static readonly Dictionary<string, ImporterDescriptor> ImporterLookup;
        private static readonly Dictionary<string, ProcessorDescriptor> Processors;
        private static readonly Dictionary<Type, WriterDescriptor> Writers;
        private static readonly Dictionary<Type, LoaderDescriptor> Loaders;

        static PipelineManager()
        {
            Importers = new List<ImporterDescriptor>();
            Processors = new Dictionary<string, ProcessorDescriptor>();
            Writers = new Dictionary<Type, WriterDescriptor>();
            Loaders = new Dictionary<Type, LoaderDescriptor>();
            ImporterLookup = new Dictionary<string, ImporterDescriptor>();

            ScanAssembly(typeof(PipelineManager).Assembly);
        }

        public static void ScanAssembly(Assembly assembly)
        {
            List<ImporterDescriptor> importers = new List<ImporterDescriptor>();
            List<ProcessorDescriptor> processors = new List<ProcessorDescriptor>();
            List<LoaderDescriptor> loaders = new List<LoaderDescriptor>();

            foreach (Type type in assembly.GetExportedTypes())
            {
                try
                {
                    if (type.BaseType == null)
                        continue;

                    if (Attribute.IsDefined(type, typeof(ContentImporterAttribute)))
                    {
                        ImporterDescriptor descriptor = new ImporterDescriptor(type);

                        importers.Add(descriptor);
                        Importers.Add(descriptor);

                        foreach (string extension in descriptor.Extensions)
                            ImporterLookup[extension] = descriptor;
                    }
                    else if (Attribute.IsDefined(type, typeof(ContentProcessorAttribute)))
                    {
                        ProcessorDescriptor descriptor = new ProcessorDescriptor(type);

                        processors.Add(descriptor);
                        Processors.Add(type.Name, descriptor);
                    }
                    else if (Attribute.IsDefined(type, typeof(ContentTypeWriterAttribute)))
                    {
                        Writers.Add(type.BaseType.GenericTypeArguments[0], new WriterDescriptor(type));
                    }
                    else if (Attribute.IsDefined(type, typeof(ContentLoaderAttribute)))
                    {
                        LoaderDescriptor descriptor = new LoaderDescriptor(type);

                        loaders.Add(descriptor);
                        Loaders[descriptor.Loadee] = descriptor;
                    }
                }
                catch (Exception)
                {
                    // TODO: Do something men
                }
            }

            foreach (ImporterDescriptor importer in importers)
            {
                importer.DefaultProcessor = QueryProcessorByName(importer.Type.GetCustomAttribute<ContentImporterAttribute>().DefaultProcessor);
            }

            foreach (ProcessorDescriptor processor in processors)
            {
                processor.Writer = QueryWriterByType(processor.Output);
                processor.Loader = QueryLoaderByType(processor.Output);
            }
        }

        public static void RegisterJustInDesignExtensions(IPipelineProject project)
        {
            ContentManager.Loader = new PipelineRuntimeContentLoader(project);
        }

        #region Query

        public static ImporterDescriptor QueryImporterByName(string name)
        {
            return Importers.FirstOrDefault(importer => importer.Type.Name == name);
        }

        public static ImporterDescriptor QueryImporterByExtension(string extension)
        {
            ImporterDescriptor importer;
            ImporterLookup.TryGetValue(extension, out importer);
            return importer;
        }

        public static ProcessorDescriptor QueryProcessorByName(string name)
        {
            ProcessorDescriptor processor;
            Processors.TryGetValue(name, out processor);
            return processor;
        }

        public static WriterDescriptor QueryWriterByType(Type type)
        {
            WriterDescriptor writer;
            Writers.TryGetValue(type, out writer);
            return writer;
        }

        public static LoaderDescriptor QueryLoaderByType(Type type)
        {
            LoaderDescriptor loader;
            Loaders.TryGetValue(type, out loader);
            return loader;
        }

        #endregion
    }
}
