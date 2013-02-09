// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Input
{
    public static class Keyboard
    {
        #region Static Fields

        private static List<Keys> _Keys;

        #endregion

        #region Public Methods and Operators

        public static KeyboardState GetState()
        {
            return new KeyboardState(_Keys);
        }

        #endregion

        #region Methods

        internal static void SetKeys(List<Keys> keys)
        {
            _Keys = keys;
        }

        #endregion
    }
}