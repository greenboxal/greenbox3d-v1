﻿// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics;

namespace GreenBox3D.Content
{
    public class ContentManager : IDisposable
    {
        #region Fields

        private readonly Dictionary<string, object> _cache;
        private readonly ContentCachePolicy _cachePolicy;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Dictionary<string, WeakReference> _weakCache;

        #endregion

        #region Constructors and Destructors

        static ContentManager()
        {
            Loader = new DefaultRuntimeContentLoader();
            CheckContentChecksum = true;
        }

        public ContentManager(GraphicsDevice graphicsDevice, ContentCachePolicy cachePolicy = ContentCachePolicy.Cache)
        {
            _cachePolicy = cachePolicy;
            _graphicsDevice = graphicsDevice;

            switch (_cachePolicy)
            {
                case ContentCachePolicy.Cache:
                    _weakCache = new Dictionary<string, WeakReference>(StringComparer.InvariantCultureIgnoreCase);
                    break;
                case ContentCachePolicy.KeepAlive:
                    _cache = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
                    break;
            }
        }

        #endregion

        #region Public Properties

        public static bool CheckContentChecksum { get; set; }
        public GraphicsDevice GraphicsDevice { get { return _graphicsDevice; } }

        #endregion

        #region Properties

        internal static IRuntimeContentLoader Loader { get; set; }

        #endregion

        #region Public Methods and Operators

        public static string NormalizePath(string path)
        {
            return path.Replace('\\', '/');
        }

        public void CacheObject(string filename, object value)
        {
            switch (_cachePolicy)
            {
                case ContentCachePolicy.Cache:
                    _weakCache[filename] = new WeakReference(value);
                    break;
                case ContentCachePolicy.KeepAlive:
                    _cache[filename] = value;
                    break;
            }
        }

        public void Dispose()
        {
            InvalidateCache();
        }

        public void InvalidateCache()
        {
            switch (_cachePolicy)
            {
                case ContentCachePolicy.Cache:
                    _weakCache.Clear();
                    break;
                case ContentCachePolicy.KeepAlive:
                    _cache.Clear();
                    break;
            }

            GC.Collect();
        }

        public T LoadContent<T>(string filename) where T : class
        {
            Type type = typeof(T);

            filename = NormalizePath(filename);

            switch (_cachePolicy)
            {
                case ContentCachePolicy.Cache:
                {
                    WeakReference reference;

                    if (_weakCache.TryGetValue(filename, out reference))
                    {
                        if (reference.IsAlive && type.IsInstanceOfType(reference.Target))
                            return (T)reference.Target;
                        else
                            return null;
                    }

                    break;
                }
                case ContentCachePolicy.KeepAlive:
                {
                    object value;

                    if (_cache.TryGetValue(filename, out value))
                    {
                        if (type.IsInstanceOfType(value))
                            return (T)value;
                        else
                            return null;
                    }

                    break;
                }
            }

            return Loader.LoadContent<T>(this, filename);
        }

        #endregion
    }
}