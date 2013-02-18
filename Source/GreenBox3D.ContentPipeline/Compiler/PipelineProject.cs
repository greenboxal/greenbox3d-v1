using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

using Microsoft.Scripting.Hosting;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public class PipelineProject : IPipelineProject
    {
        private readonly string _projectPath;
        private readonly string _projectBase;

        // Loading helpers
        private string _currentBase;
        private string _currentRelative;
        private readonly ScriptScope _scope;

        // Consumer logic
        private IPipelineProjectConsumer _consumer;

        public string ProjectBase { get { return _projectBase; } }

        public PipelineProject(string filename)
        {
            _scope = ScriptManager.CreateScope();
            _projectPath = Path.GetFullPath(filename);
            _projectBase = Path.GetDirectoryName(_projectPath);
            _currentRelative = "";

            SetupDSL();
        }

        public void Consume(IPipelineProjectConsumer consumer)
        {
            _consumer = consumer;
            LoadProjectInternal("", _projectPath);
        }

        private void SetupDSL()
        {
            _scope.SetMethod("import", new Action<string>(DSLImport));
            _scope.SetMethod("reference", new Action<string>(DSLReference));
            _scope.SetMethodWithBlock("content", new Action<string, dynamic>(DSLContent));
        }

        private void LoadProjectInternal(string rel, string filename)
        {
            string oldBase = _currentBase;
            string oldRelative = _currentRelative;

            filename = Path.GetFullPath(filename);
            _currentBase = Path.GetDirectoryName(filename);
            _currentRelative = Path.Combine(_currentRelative, rel);

            ScriptManager.Engine.CreateScriptSourceFromFile(filename).Execute(_scope);

            _currentRelative = oldRelative;
            _currentBase = oldBase;
        }

        private void DSLImport(string path)
        {
            LoadProjectInternal(path, Path.Combine(_currentBase, path, "ContentProject.rb"));
        }

        private void DSLReference(string path)
        {
            _consumer.AddReference(path);
        }

        private void DSLContent(string path, dynamic handler)
        {
            ContentDescriptor descriptor = new ContentDescriptor(_projectBase, Path.Combine(_currentRelative, path));

            if (handler != null)
                handler.call(descriptor.Properties);

            _consumer.AddContent(descriptor);
        }
    }
}
