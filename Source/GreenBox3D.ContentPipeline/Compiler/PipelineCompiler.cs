using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using GreenBox3D.ContentPipeline.Importers;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public class PipelineCompiler : IPipelineProjectConsumer
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(PipelineCompiler));

        public IPipelineProject Project { get; private set; }
        public string OutputPath { get; set; }

        public PipelineCompiler(IPipelineProject project)
        {
            Project = project;
            OutputPath = project.ProjectBase;
        }

        public void Compile()
        {
            Project.Consume(this);
        }

        void IPipelineProjectConsumer.AddReference(string name)
        {
            Assembly assembly = Assembly.Load(new AssemblyName(name));

            if (assembly == null)
                return;

            PipelineManager.ScanAssembly(assembly);
        }

        // TODO: Improve resolving algorithm 
        void IPipelineProjectConsumer.AddContent(ContentDescriptor content)
        {
            string extension = Path.GetExtension(content.Path);
            string filename = Path.GetFileName(content.Path);

            Logger.Message("Building {0}...", filename);

            BuildContext context = CreateBuildContext(content);
            ImporterDescriptor importer;
            ProcessorDescriptor processor = null;
            WriterDescriptor writer;
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
                        return;
                    }
                }
                else
                {
                    importer = PipelineManager.QueryImporterByExtension(extension);

                    if (importer == null)
                    {
                        Logger.Error("No valid importer was found for {1}", filename);
                        return;
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
                    return;
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
                        return;
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
                    return;
                }

                // Create Writer
                writer = processor.Writer;

                if (writer == null)
                {
                    Logger.Error("No valid writer was found for {0}", filename);
                    return;
                }

                // Write
                try
                {
                    string newExtension = writer.Extension;
                    string buildPath = Path.Combine(OutputPath, Path.GetDirectoryName(content.RelativePath), Path.GetFileNameWithoutExtension(content.RelativePath)) + newExtension;

                    Directory.CreateDirectory(Path.GetDirectoryName(buildPath));
                    Stream output = new FileStream(buildPath, FileMode.Create);

                    writer.Create().Write(output, temporary, context);
                    output.Close();
                }
                catch (Exception ex)
                {
                    Logger.Error("An exception ocurred writing {0}: {1}", filename, ex);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("An exception ocurred building {0}: {1}", filename, ex);
            }
        }

        private BuildContext CreateBuildContext(ContentDescriptor descriptor)
        {
            BuildContext context = new BuildContext();

            context.Descriptor = descriptor;
            context.Logger = Logger;

            return context;
        }
    }
}
