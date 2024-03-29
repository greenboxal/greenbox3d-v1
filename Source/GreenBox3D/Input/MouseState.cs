﻿// GreenBox3D
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
    public struct MouseState
    {
        #region Fields

        private ButtonState _leftButton;
        private ButtonState _middleButton;
        private ButtonState _rightButton;
        private int _scrollWheelValue;
        private int _x, _y;

        #endregion

        #region Constructors and Destructors

        public MouseState(int x, int y, int scrollWheel, ButtonState leftButton, ButtonState middleButton, ButtonState rightButton)
        {
            _x = x;
            _y = y;
            _scrollWheelValue = scrollWheel;
            _leftButton = leftButton;
            _middleButton = middleButton;
            _rightButton = rightButton;
        }

        #endregion

        #region Public Properties

        public ButtonState LeftButton { get { return _leftButton; } internal set { _leftButton = value; } }
        public ButtonState MiddleButton { get { return _middleButton; } internal set { _middleButton = value; } }
        public ButtonState RightButton { get { return _rightButton; } internal set { _rightButton = value; } }
        public int ScrollWheelValue { get { return _scrollWheelValue; } internal set { _scrollWheelValue = value; } }
        public int X { get { return _x; } internal set { _x = value; } }
        public ButtonState XButton1 { get { return ButtonState.Released; } }
        public ButtonState XButton2 { get { return ButtonState.Released; } }
        public int Y { get { return _y; } internal set { _y = value; } }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(MouseState left, MouseState right)
        {
            return left._x == right._x && left._y == right._y && left._leftButton == right._leftButton && left._middleButton == right._middleButton && left._rightButton == right._rightButton && left._scrollWheelValue == right._scrollWheelValue;
        }

        public static bool operator !=(MouseState left, MouseState right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is MouseState)
                return this == (MouseState)obj;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}