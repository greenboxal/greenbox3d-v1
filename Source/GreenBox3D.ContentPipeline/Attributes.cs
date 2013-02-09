using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.ContentPipeline
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ContentProcessorAttribute : Attribute
    {
        public string DisplayName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ContentImporterAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string[] Extensions { get; set; }
        public string DefaultProcessor { get; set; }

        public ContentImporterAttribute(params string[] extensions)
        {
            Extensions = extensions;
        }
    }

    public sealed class ContentTypeWriterAttribute : Attribute
    {
        public string Extension { get; set; }

        public ContentTypeWriterAttribute()
        {
            Extension = ".bin";
        }
    }

    public sealed class ContentLoaderAttribute : Attribute
    {
    }
}
