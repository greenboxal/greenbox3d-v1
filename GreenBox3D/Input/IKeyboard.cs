﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Input
{
    [Browsable(false)]
    public interface IKeyboard
    {
        KeyboardState GetState();
    }
}
