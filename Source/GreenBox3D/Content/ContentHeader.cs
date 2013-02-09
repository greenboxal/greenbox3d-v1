using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Content
{
    public class ContentHeader
    {
        public string Magic { get; private set; }
        public Version Version { get; private set; }
        public Encoding Encoding { get; private set; }

        public ContentHeader(string magic, Version version, Encoding encoding = null)
        {
            Magic = magic;
            Version = version;
            Encoding = encoding ?? Encoding.UTF8;
        }
    }
}
