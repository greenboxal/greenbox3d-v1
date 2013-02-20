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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Graphics
{
    public class EffectParameterCollection : IReadOnlyCollection<EffectParameter>
    {
        #region Fields

        private readonly OrderedDictionary _dictionary;

        #endregion

        #region Constructors and Destructors

        internal EffectParameterCollection(EffectParameter[] parameters)
        {
            _dictionary = new OrderedDictionary(StringComparer.InvariantCultureIgnoreCase);

            foreach (EffectParameter parameter in parameters)
                _dictionary.Add(parameter.Name, parameter);
        }

        #endregion

        #region Public Properties

        public int Count { get { return _dictionary.Count; } }

        #endregion

        #region Public Indexers

        public EffectParameter this[int index] { get { return (EffectParameter)_dictionary[index]; } }
        public EffectParameter this[string index] { get { return (EffectParameter)_dictionary[index]; } }

        #endregion

        #region Public Methods and Operators

        public IEnumerator<EffectParameter> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return (EffectParameter)_dictionary[i];
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