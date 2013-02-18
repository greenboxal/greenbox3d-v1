using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IronRuby.Runtime;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public class OpenStruct : DynamicObject
    {
        private readonly Dictionary<string, object> _values;

        public OpenStruct()
        {
            _values = new Dictionary<string, object>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            _values.TryGetValue(binder.Name, out result);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _values[RubyUtils.TryUnmangleName(binder.Name)] = value;
            return true;
        }
    }
}
