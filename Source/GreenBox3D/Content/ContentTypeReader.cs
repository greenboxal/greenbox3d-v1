﻿// GreenBox3D
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

using GreenBox3D.Utilities;

namespace GreenBox3D.Content
{
    public abstract class ContentTypeReader<T> : IContentTypeReader
    {
        #region Public Properties

        public string Magic { get; protected set; }
        public Version Version { get; protected set; }

        #endregion

        #region Explicit Interface Methods

        object IContentTypeReader.Load(ContentManager manager, Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true);

            byte flags = reader.ReadByte();
            byte msize = reader.ReadByte();
            byte[] mbytes = reader.ReadBytes(msize);
            string magic = Encoding.UTF8.GetString(mbytes);
            Version version = new Version(reader.ReadInt32(), reader.ReadInt32());
            Encoding encoding = Encoding.UTF8;

            if ((flags & 1) == 1)
                encoding = Encoding.GetEncoding(reader.ReadInt32());

            uint hash = reader.ReadUInt32();
            reader.Close();

            if (!CheckHeader(new ContentHeader(magic, version, encoding)))
                return null;

            byte[] data = new byte[stream.Length - stream.Position];
            stream.Read(data, 0, data.Length);
            stream.Close();

            using (Stream ms = new MemoryStream(data))
            {
                Stream cr = ms;

                if (ContentManager.CheckContentChecksum)
                {
                    ContentCrc32 crc32 = new ContentCrc32();

                    crc32.ComputeHash(ms);
                    ms.Position = 0;

                    // TODO: crAsH?
                    if (crc32.CrcValue != hash)
                        return null;
                }

                if ((flags & 2) == 2)
                    cr = new DeflateStream(ms, CompressionMode.Decompress);

                return Load(manager, new ContentReader(cr, encoding));
            }
        }

        #endregion

        #region Methods

        protected virtual bool CheckHeader(ContentHeader header)
        {
            return header.Magic == Magic && header.Version <= Version;
        }

        protected abstract T Load(ContentManager manager, ContentReader reader);

        #endregion
    }
}