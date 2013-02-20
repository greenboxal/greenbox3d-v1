// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace GreenBox3D.Graphics
{
    public class TextureCollection : IReadOnlyCollection<Texture>
    {
        #region Fields

        private readonly BitArray _array;
        private readonly Texture[] _textures;

        #endregion

        #region Constructors and Destructors

        internal TextureCollection()
        {
            int size = 0;
            GL.GetInteger(GetPName.MaxTextureUnits, out size);

            _array = new BitArray(size, false);
            _textures = new Texture[size];
        }

        #endregion

        #region Public Properties

        public int Count { get { return _textures.Length; } }

        #endregion

        #region Public Indexers

        public Texture this[int index]
        {
            get
            {
                lock (_textures)
                {
                    return _textures[index];
                }
            }
            set
            {
                if (index >= _textures.Length)
                    throw new IndexOutOfRangeException();

                lock (_textures)
                {
                    if (_textures[index] == value)
                        return;

                    _textures[index] = value;
                    _array[index] = true;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerator<Texture> GetEnumerator()
        {
            return ((IEnumerable<Texture>)_textures).GetEnumerator();
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Methods

        internal void Apply()
        {
            for (int i = 0; i < _textures.Length; i++)
            {
                if (!_array[i])
                    continue;

                Texture tex = _textures[i];

                tex.Create(true);
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                GL.BindTexture(_textures[i].TextureTarget, _textures[i].TextureID);

                _array[i] = false;
            }
        }

        #endregion
    }
}