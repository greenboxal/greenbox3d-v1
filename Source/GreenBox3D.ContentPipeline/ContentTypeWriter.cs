using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.ContentPipeline.Writers;
using GreenBox3D.Utilities;

namespace GreenBox3D.ContentPipeline
{
    public abstract class ContentTypeWriter<TInput> : IContentTypeWriter
    {
        public ContentHeader Header { get; private set; }
        public bool ShouldCompress { get; set; }

        protected ContentTypeWriter(ContentHeader header)
        {
            Header = header;
        }

        protected abstract void Write(ContentWriter stream, TInput input, BuildContext context);

        void IContentTypeWriter.Write(Stream stream, object input, BuildContext context)
        {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true);
            bool cenc = !Header.Encoding.Equals(Encoding.UTF8);
            byte flags = 0;

            if (cenc)
                flags |= 1;

            if (ShouldCompress)
                flags |= 2;

            writer.Write(flags);
            writer.Write((byte)Header.Magic.Length);
            writer.Write(Encoding.UTF8.GetBytes(Header.Magic));
            writer.Write(Header.Version.Major);
            writer.Write(Header.Version.Minor);
            writer.Write(Header.Version.Build);
            writer.Write(Header.Version.Revision);

            if (cenc)
                writer.Write(Header.Encoding.CodePage);

            Stream ms = new MemoryStream();

            if (ShouldCompress)
                ms = new DeflateStream(ms, CompressionMode.Compress);

            var cw = new ContentWriter(ms, Header.Encoding);
            Write(cw, (TInput)input, context);
            cw.Close();

            ContentCrc32 crc32 = new ContentCrc32();
            ms.Position = 0;
            crc32.ComputeHash(ms);
            writer.Write(crc32.CrcValue);
            writer.Close();

            ms.Position = 0;
            ms.CopyTo(stream);
            ms.Close();

            stream.Close();
        }
    }
}
