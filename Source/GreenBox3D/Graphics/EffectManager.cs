// GreenBox3D
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

using GreenBox3D.Content;
using GreenBox3D.Graphics.Shading;

namespace GreenBox3D.Graphics
{
    public static class EffectManager
    {
        #region Static Fields

        private static ContentManager _contentManager;
        private static Dictionary<string, Dictionary<string, Shader>> _shaders;

        #endregion

        #region Public Methods and Operators

        public static Effect LoadEffect(string name)
        {
            return new Effect(LoadShader(name));
        }

        #endregion

        #region Methods

        internal static void Intiailize(GraphicsDevice device)
        {
            _contentManager = new ContentManager(device);
            _shaders = new Dictionary<string, Dictionary<string, Shader>>(StringComparer.InvariantCultureIgnoreCase);
        }

        internal static Shader LoadShader(string name)
        {
            string contentName = Path.GetDirectoryName(name);
            string shaderName = Path.GetFileName(name);
            Dictionary<string, Shader> cache;
            Shader s;

            lock (_shaders)
            {
                if (!_shaders.TryGetValue(contentName, out cache))
                {
                    ShaderCollection shaders = _contentManager.LoadContent<ShaderCollection>("Data/Shaders/" + contentName);

                    if (shaders == null)
                        return null;

                    cache = new Dictionary<string, Shader>(StringComparer.InvariantCultureIgnoreCase);
                    foreach (Shader shader in shaders)
                        cache.Add(Path.GetFileName(shader.Name), shader);

                    _shaders[contentName] = cache;
                }
            }

            if (!cache.TryGetValue(shaderName, out s))
                return null;

            if (!s.Created)
                s.Create();

            if (!s.IsValid)
                return !string.IsNullOrEmpty(s.Fallback) ? LoadShader(s.Fallback) : null;

            return s;
        }

        #endregion
    }
}