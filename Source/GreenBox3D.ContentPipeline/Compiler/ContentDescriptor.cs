using System;
using System.Collections.Generic;
using System.Dynamic;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public class ContentDescriptor
    {
        public string Path { get; private set; }
        public string RelativePath { get; private set; }
        public dynamic Properties { get; private set; }

        public ContentDescriptor(string root, string relativePath)
        {
            RelativePath = relativePath;
            Path = System.IO.Path.Combine(root, relativePath);
            Properties = new OpenStruct();
        }
    }
}
