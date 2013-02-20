// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

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
        #region Fields

        private readonly Dictionary<string, object> _values;

        #endregion

        #region Constructors and Destructors

        public OpenStruct()
        {
            _values = new Dictionary<string, object>();
        }

        #endregion

        #region Public Methods and Operators

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

        #endregion
    }
}