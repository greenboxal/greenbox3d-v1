// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

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
        #region Public Properties

        public bool? ShouldCompress { get; set; }

        #endregion

        #region Explicit Interface Methods

        void IContentTypeWriter.Write(Stream stream, object input, BuildContext context)
        {
            ContentHeader header = GetHeader((TInput)input, context);
            BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true);
            bool cenc = !header.Encoding.Equals(Encoding.UTF8);
            byte flags = 0;

            if (!ShouldCompress.HasValue)
                ShouldCompress = (bool)(context.Descriptor.Properties.CompressOutput ?? false);

            if (cenc)
                flags |= 1;

            if (ShouldCompress.GetValueOrDefault(false))
                flags |= 2;

            writer.Write(flags);
            writer.Write((byte)header.Magic.Length);
            writer.Write(Encoding.UTF8.GetBytes(header.Magic));
            writer.Write(header.Version.Major);
            writer.Write(header.Version.Minor);

            if (cenc)
                writer.Write(header.Encoding.CodePage);

            MemoryStream ms = new MemoryStream();
            Stream s = ms;

            if (ShouldCompress.GetValueOrDefault(false))
                s = new DeflateStream(ms, CompressionMode.Compress, true);

            var cw = new ContentWriter(s, header.Encoding);
            Write(cw, (TInput)input, context);
            cw.Close();

            if (s != ms)
                s.Close();

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

        #endregion

        #region Methods

        protected abstract ContentHeader GetHeader(TInput input, BuildContext context);
        protected abstract void Write(ContentWriter stream, TInput input, BuildContext context);

        #endregion
    }
}