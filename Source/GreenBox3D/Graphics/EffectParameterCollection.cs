using System;
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
        private readonly OrderedDictionary _dictionary;

        internal EffectParameterCollection(EffectParameter[] parameters)
        {
            _dictionary = new OrderedDictionary(StringComparer.InvariantCultureIgnoreCase);

            foreach (EffectParameter parameter in parameters)
                _dictionary.Add(parameter.Name, parameter);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public IEnumerator<EffectParameter> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return (EffectParameter)_dictionary[i];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public EffectParameter this[int index]
        {
            get { return (EffectParameter)_dictionary[index]; }
        }

        public EffectParameter this[string index]
        {
            get { return (EffectParameter)_dictionary[index]; }
        }
    }
}
