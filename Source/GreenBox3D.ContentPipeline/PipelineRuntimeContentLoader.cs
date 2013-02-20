// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        #region Static Fields

        private static readonly ILogger Logger = LogManager.GetLogger(typeof(PipelineRuntimeContentLoader));

        #endregion

        #region Fields

        private readonly Dictionary<string, ContentDescriptor> _fileMap;
        private readonly Dictionary<string, Tuple<LoaderDescriptor, object, BuildContext>> _processorCache;
        private readonly IPipelineProject _project;

        #endregion

        #region Constructors and Destructors

        public PipelineRuntimeContentLoader(IPipelineProject project)
        {
            _fileMap = new Dictionary<string, ContentDescriptor>(StringComparer.InvariantCultureIgnoreCase);
            _processorCache = new Dictionary<string, Tuple<LoaderDescriptor, object, BuildContext>>(StringComparer.InvariantCultureIgnoreCase);
            _project = project;
            _project.Consume(this);
        }

        #endregion

        #region Public Methods and Operators

        public T LoadContent<T>(ContentManager manager, string filename) where T : class
        {
            ContentDescriptor descriptor;
            Tuple<LoaderDescriptor, object, BuildContext> cache;

            if (_processorCache.TryGetValue(filename, out cache))
                return (T)cache.Item1.Create().Load(manager, cache.Item2, cache.Item3);

            if (!_fileMap.TryGetValue(filename, out descriptor))
                return null;

            object result = Compile(manager, filename, descriptor);

            if (result != null)
                manager.CacheObject(filename, result);

            return (T)result;
        }

        #endregion

        #region Explicit Interface Methods

        void IPipelineProjectConsumer.AddContent(ContentDescriptor content)
        {
            _fileMap[ContentManager.NormalizePath(Path.Combine(Path.GetDirectoryName(content.RelativePath), Path.GetFileNameWithoutExtension(content.RelativePath)))] = content;
        }

        void IPipelineProjectConsumer.AddReference(string name)
        {
            PipelineManager.ScanAssembly(Assembly.Load(new AssemblyName(name)));
        }

        #endregion

        // TODO: Improve resolving algorithm 

        #region Methods

        private object Compile(ContentManager manager, string normalized, ContentDescriptor content)
        {
            string extension = Path.GetExtension(content.Path);
            string filename = Path.GetFileName(content.Path);

            BuildContext context = CreateBuildContext(content);
            ImporterDescriptor importer;
            ProcessorDescriptor processor = null;
            LoaderDescriptor loader;
            object temporary;
            var props = context.Descriptor.Properties;

            try
            {
                // Create Importer
                if (props.Importer != null)
                {
                    importer = PipelineManager.QueryImporterByName(props.Importer);

                    if (importer == null)
                    {
                        Logger.Error("Invalid importer '{0}' for {1}", props.Importer, filename);
                        return null;
                    }
                }
                else
                {
                    importer = PipelineManager.QueryImporterByExtension(extension);

                    if (importer == null)
                    {
                        Logger.Error("No valid importer was found for {1}", filename);
                        return null;
                    }
                }

                // Import
                try
                {
                    temporary = importer.Create().Import(content.Path, context);
                }
                catch (Exception ex)
                {
                    Logger.Error("An exception ocurred importing {0}: {1}", filename, ex);
                    return null;
                }

                // Create Processor
                if (props.Processor != null)
                    processor = PipelineManager.QueryProcessorByName(props.Processor);

                if (processor == null)
                {
                    processor = importer.DefaultProcessor;

                    if (processor == null)
                    {
                        Logger.Error("No valid processor was found for {1}", filename);
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
                    Logger.Error("An exception ocurred processing {0}: {1}", filename, ex);
                    return null;
                }

                // Loader
                loader = processor.Loader;
                temporary = loader.Create().Load(manager, temporary, context);

                if (temporary != null)
                {
                    _processorCache[normalized] = new Tuple<LoaderDescriptor, object, BuildContext>(loader, temporary, context);
                    return temporary;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("An exception ocurred building {0}: {1}", filename, ex);
                return null;
            }

            return null;
        }

        private BuildContext CreateBuildContext(ContentDescriptor content)
        {
            BuildContext context = new BuildContext();

            context.Descriptor = content;
            context.Logger = Logger;

            return context;
        }

        #endregion
    }
}