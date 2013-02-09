using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Content
{
    public sealed class ContentTypeReaderAttribute : Attribute
    {
        public string Extension { get; set; }

        public ContentTypeReaderAttribute()
        {
            Extension = ".bin";
        }
    }
}
