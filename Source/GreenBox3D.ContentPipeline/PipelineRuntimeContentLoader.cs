using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;

namespace GreenBox3D.ContentPipeline
{
    internal class PipelineRuntimeContentLoader : IRuntimeContentLoader, IPipelineProjectConsumer
    {
        private readonly IPipelineProject _project;
        private readonly Dictionary<string, ContentDescriptor> _fileMap;
        private readonly Dictionary<string, Tuple<LoaderDescriptor, object, BuildContext>> _processorCache;

        private readonly ILogger _logger;

        public PipelineRuntimeContentLoader(IPipelineProject project)
        {
            _fileMap = new Dictionary<string, ContentDescriptor>(StringComparer.InvariantCultureIgnoreCase);
            _processorCache = new Dictionary<string, Tuple<LoaderDescriptor, object, BuildContext>>(StringComparer.InvariantCultureIgnoreCase);
            _logger = new ConsoleLogger();
            _project = project;
            _project.Consume(this);
        }

        public T LoadContent<T>(ContentManager manager, string filename) where T : class
        {
            ContentDescriptor descriptor;
            Tuple<LoaderDescriptor, object, BuildContext> cache;

            if (_processorCache.TryGetValue(filename, out cache))
                return (T)cache.Item1.Create().Load(manager, cache.Item2, cache.Item3);

            if (!_fileMap.TryGetValue(filename, out descriptor))
                return null;

            object result = Compile(manager, filename, descriptor);;

            if (result != null)
                manager.CacheObject(filename, result);

            return (T)result;
        }

        // TODO: Improve resolving algorithm 
        private object Compile(ContentManager manager, string normalized, ContentDescriptor content)
        {
            string extension = Path.GetExtension(content.Path);
            string filename = Path.GetFileName(content.Path);

            BuildContext context = CreateBuildContext(content);
            ImporterDescriptor importer;
            ProcessorDescriptor processor = null;
            LoaderDescriptor loader;
            object temporary;
            Stream stream;

            try
            {
                // Open File
                stream = new FileStream(content.Path, FileMode.Open);

                // Create Importer
                if (context.Descriptor["importer"] != null)
                {
                    importer = PipelineManager.QueryImporterByName(context.Descriptor["importer"].ToString());

                    if (importer == null)
                    {
                        _logger.LogError("Invalid importer '{0}' for {1}", context.Descriptor["importer"], filename);
                        return null;
                    }
                }
                else
                {
                    importer = PipelineManager.QueryImporterByExtension(extension);

                    if (importer == null)
                    {
                        _logger.LogError("No valid importer was found for {1}", filename);
                        return null;
                    }
                }

                // Import
                try
                {
                    temporary = importer.Create().Import(stream, context);
                    stream.Close();
                }
                catch (Exception ex)
                {
                    _logger.LogError("An exception ocurred importing {0}: {1}", filename, ex);
                    return null;
                }

                // Create Processor
                if (context.Descriptor["processor"] != null)
                    processor = PipelineManager.QueryProcessorByName(context.Descriptor["processor"].ToString());

                if (processor == null)
                {
                    processor = importer.DefaultProcessor;

                    if (processor == null)
                    {
                        _logger.LogError("No valid processor was found for {1}", filename);
                        return null;
                    }
                }

                // Process
                try
                {
                    temporary = processor.Create().Process(temporary, context);
                }
                catch (Exception ex)
                {
                    _logger.LogError("An exception ocurred processing {0}: {1}", filename, ex);
                    return null;
                }

                // Loader
                loader = processor.Loader;
                temporary = loader.Create().Load(manager, temporary, context);

                if (temporary != null)
                {
                    _processorCache.Add(normalized, new Tuple<LoaderDescriptor, object, BuildContext>(loader, temporary, context));
                    return temporary;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception ocurred building {0}: {1}", filename, ex);
                return null;
            }

            return null;
        }

        private BuildContext CreateBuildContext(ContentDescriptor content)
        {
            BuildContext context = new BuildContext();

            context.Descriptor = content;
            context.Logger = _logger;

            return context;
        }

        void IPipelineProjectConsumer.AddReference(string name)
        {
            PipelineManager.ScanAssembly(Assembly.Load(new AssemblyName(name)));
        }

        void IPipelineProjectConsumer.AddContent(ContentDescriptor content)
        {
            _fileMap[ContentManager.NormalizePath(Path.Combine(Path.GetDirectoryName(content.RelativePath), Path.GetFileNameWithoutExtension(content.RelativePath)))] = content;
        }
    }
}
