﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Input
{
    public static class Mouse
    {
        private static IMouse _mouse;

        [Browsable(false)]
        public static void SetMouseImplementation(IMouse mouse)
        {
            _mouse = mouse;
        }

        public static MouseState GetState()
        {
            if (_mouse != null)
                return _mouse.GetState();

            return new MouseState();
        }

        public static void SetPosition(int x, int y)
        {
            if (_mouse != null)
                _mouse.SetPosition(x, y);
        }
    }
}
