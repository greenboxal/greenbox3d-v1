// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Graphics
{
    public class EffectPassCollection : IReadOnlyCollection<EffectPass>
    {
        #region Fields

        private readonly EffectPass[] _passes;

        #endregion

        #region Constructors and Destructors

        internal EffectPassCollection(EffectPass[] passes)
        {
            _passes = passes;
        }

        #endregion

        #region Public Properties

        public int Count { get { return _passes.Length; } }

        #endregion

        #region Public Methods and Operators

        public IEnumerator<EffectPass> GetEnumerator()
        {
            return ((IEnumerable<EffectPass>)_passes).GetEnumerator();
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}