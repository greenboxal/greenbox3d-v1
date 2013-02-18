using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Graphics
{
    public class EffectPassCollection : IReadOnlyCollection<EffectPass>
    {
        private readonly EffectPass[] _passes;

        internal EffectPassCollection(EffectPass[] passes)
        {
            _passes = passes;
        }

        public int Count
        {
            get { return _passes.Length; }
        }

        public IEnumerator<EffectPass> GetEnumerator()
        {
            return ((IEnumerable<EffectPass>)_passes).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
