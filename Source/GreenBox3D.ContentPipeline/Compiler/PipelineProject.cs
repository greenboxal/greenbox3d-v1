// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

using Microsoft.Scripting.Hosting;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public class PipelineProject : IPipelineProject
    {
        #region Fields

        private readonly string _projectBase;
        private readonly string _projectPath;
        // Loading helpers
        private readonly ScriptScope _scope;
        // Consumer logic
        private IPipelineProjectConsumer _consumer;
        private string _currentBase;
        private string _currentRelative;

        #endregion

        #region Constructors and Destructors

        public PipelineProject(string filename)
        {
            _scope = ScriptManager.CreateScope();
            _projectPath = Path.GetFullPath(filename);
            _projectBase = Path.GetDirectoryName(_projectPath);
            _currentRelative = "";

            SetupDSL();
        }

        #endregion

        #region Public Properties

        public string ProjectBase { get { return _projectBase; } }

        #endregion

        #region Public Methods and Operators

        public void Consume(IPipelineProjectConsumer consumer)
        {
            _consumer = consumer;
            LoadProjectInternal("", _projectPath);
        }

        #endregion

        #region Methods

        private void DSLContent(string path, dynamic handler)
        {
            ContentDescriptor descriptor = new ContentDescriptor(_projectBase, Path.Combine(_currentRelative, path));

            if (handler != null)
                handler.call(descriptor.Properties);

            _consumer.AddContent(descriptor);
        }

        private void DSLImport(string path)
        {
            LoadProjectInternal(path, Path.Combine(_currentBase, path, "ContentProject.rb"));
        }

        private void DSLReference(string path)
        {
            _consumer.AddReference(path);
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

        private void SetupDSL()
        {
            _scope.SetMethod("import", new Action<string>(DSLImport));
            _scope.SetMethod("reference", new Action<string>(DSLReference));
            _scope.SetMethodWithBlock("content", new Action<string, dynamic>(DSLContent));
        }

        #endregion
    }
}