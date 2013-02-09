using System;
using System.Collections.Generic;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public class ContentDescriptor
    {
        private readonly Dictionary<string, dynamic> _attributes;

        public string Path { get; private set; }
        public string RelativePath { get; private set; }

        public ContentDescriptor(string root, string relativePath)
        {
            _attributes = new Dictionary<string, dynamic>(StringComparer.InvariantCultureIgnoreCase);
            RelativePath = relativePath;
            Path = System.IO.Path.Combine(root, relativePath);
        }

        public dynamic this[dynamic index]
        {
            get
            {
                dynamic ret;
                return _attributes.TryGetValue(index.ToString(), out ret) ? ret : null;
            }
            set { _attributes[index.ToString()] = value; }
        }
    }
}
