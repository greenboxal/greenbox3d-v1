// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;

using GreenBox3D.Input;

using OpenTK.Input;

namespace GreenBox3D.Platform.Dekstop
{
    internal static class KeyboardUtil
    {
        // TODO: make a dictionary for fast mapping
        // ^ Is this necessary? .Net agressive optimization for switches is faster than dicts

        #region Public Methods and Operators

        public static Keys ConvertKey(Key key)
        {
            switch (key)
            {
                case Key.A:
                    return Keys.A;

                case Key.AltLeft:
                    return Keys.LeftAlt;

                case Key.AltRight:
                    return Keys.RightAlt;

                case Key.B:
                    return Keys.B;

                case Key.Back:
                    return Keys.Back;

                case Key.BackSlash:
                    // Issue 1012: XNA 4.0 returns this key as OemPipe, not OemBackslash,
                    // even though OemPipe is the shifted version of the key.
                    return Keys.OemPipe;

                case Key.BracketLeft:
                    return Keys.OemOpenBrackets;

                case Key.BracketRight:
                    return Keys.OemCloseBrackets;

                case Key.C:
                    return Keys.C;

                case Key.CapsLock:
                    return Keys.CapsLock;

                case Key.Clear:
                    return Keys.OemClear;

                case Key.Comma:
                    return Keys.OemComma;

                case Key.ControlLeft:
                    return Keys.LeftControl;

                case Key.ControlRight:
                    return Keys.RightControl;

                case Key.D:
                    return Keys.D;

                case Key.Delete:
                    return Keys.Delete;

                case Key.Down:
                    return Keys.Down;

                case Key.E:
                    return Keys.E;

                case Key.End:
                    return Keys.End;

                case Key.Enter:
                    return Keys.Enter;

                case Key.Escape:
                    return Keys.Escape;

                case Key.F:
                    return Keys.F;

                case Key.F1:
                    return Keys.F1;

                case Key.F10:
                    return Keys.F10;

                case Key.F11:
                    return Keys.F11;

                case Key.F12:
                    return Keys.F12;

                case Key.F13:
                    return Keys.F13;

                case Key.F14:
                    return Keys.F14;

                case Key.F15:
                    return Keys.F15;

                case Key.F16:
                    return Keys.F16;

                case Key.F17:
                    return Keys.F17;

                case Key.F18:
                    return Keys.F18;

                case Key.F19:
                    return Keys.F19;

                case Key.F2:
                    return Keys.F2;

                case Key.F20:
                    return Keys.F20;

                case Key.F21:
                    return Keys.F21;

                case Key.F22:
                    return Keys.F22;

                case Key.F23:
                    return Keys.F23;

                case Key.F24:
                    return Keys.F24;

                case Key.F25:
                    return Keys.None;

                case Key.F26:
                    return Keys.None;

                case Key.F27:
                    return Keys.None;

                case Key.F28:
                    return Keys.None;

                case Key.F29:
                    return Keys.None;

                case Key.F3:
                    return Keys.F3;

                case Key.F30:
                    return Keys.None;

                case Key.F31:
                    return Keys.None;

                case Key.F32:
                    return Keys.None;

                case Key.F33:
                    return Keys.None;

                case Key.F34:
                    return Keys.None;

                case Key.F35:
                    return Keys.None;

                case Key.F4:
                    return Keys.F4;

                case Key.F5:
                    return Keys.F5;

                case Key.F6:
                    return Keys.F6;

                case Key.F7:
                    return Keys.F7;

                case Key.F8:
                    return Keys.F8;

                case Key.F9:
                    return Keys.F9;

                case Key.G:
                    return Keys.G;

                case Key.H:
                    return Keys.H;

                case Key.Home:
                    return Keys.Home;

                case Key.I:
                    return Keys.I;

                case Key.Insert:
                    return Keys.Insert;

                case Key.J:
                    return Keys.J;

                case Key.K:
                    return Keys.K;

                case Key.Keypad0:
                    return Keys.NumPad0;

                case Key.Keypad1:
                    return Keys.NumPad1;

                case Key.Keypad2:
                    return Keys.NumPad2;

                case Key.Keypad3:
                    return Keys.NumPad3;

                case Key.Keypad4:
                    return Keys.NumPad4;

                case Key.Keypad5:
                    return Keys.NumPad5;

                case Key.Keypad6:
                    return Keys.NumPad6;

                case Key.Keypad7:
                    return Keys.NumPad7;

                case Key.Keypad8:
                    return Keys.NumPad8;

                case Key.Keypad9:
                    return Keys.NumPad9;

                case Key.KeypadAdd:
                    return Keys.Add;

                case Key.KeypadDecimal:
                    return Keys.Decimal;

                case Key.KeypadDivide:
                    return Keys.Divide;

                case Key.KeypadEnter:
                    return Keys.Enter;

                case Key.KeypadMinus:
                    return Keys.OemMinus;

                case Key.KeypadMultiply:
                    return Keys.Multiply;

                case Key.L:
                    return Keys.L;

                case Key.LShift:
                    return Keys.LeftShift;

                case Key.LWin:
                    return Keys.LeftWindows;

                case Key.Left:
                    return Keys.Left;

                case Key.M:
                    return Keys.M;

                case Key.Minus:
                    return Keys.OemMinus;

                case Key.N:
                    return Keys.N;

                case Key.NumLock:
                    return Keys.NumLock;

                case Key.Number0:
                    return Keys.D0;

                case Key.Number1:
                    return Keys.D1;

                case Key.Number2:
                    return Keys.D2;

                case Key.Number3:
                    return Keys.D3;

                case Key.Number4:
                    return Keys.D4;

                case Key.Number5:
                    return Keys.D5;

                case Key.Number6:
                    return Keys.D6;

                case Key.Number7:
                    return Keys.D7;

                case Key.Number8:
                    return Keys.D8;

                case Key.Number9:
                    return Keys.D9;

                case Key.O:
                    return Keys.O;

                case Key.P:
                    return Keys.P;

                case Key.PageDown:
                    return Keys.PageDown;

                case Key.PageUp:
                    return Keys.PageUp;

                case Key.Pause:
                    return Keys.Pause;

                case Key.Period:
                    return Keys.OemPeriod;

                case Key.Plus:
                    return Keys.OemPlus;

                case Key.PrintScreen:
                    return Keys.PrintScreen;

                case Key.Q:
                    return Keys.Q;

                case Key.Quote:
                    return Keys.OemQuotes;

                case Key.R:
                    return Keys.R;

                case Key.Right:
                    return Keys.Right;

                case Key.RShift:
                    return Keys.RightShift;

                case Key.RWin:
                    return Keys.RightWindows;

                case Key.S:
                    return Keys.S;

                case Key.ScrollLock:
                    return Keys.Scroll;

                case Key.Semicolon:
                    return Keys.OemSemicolon;

                case Key.Slash:
                    return Keys.OemQuestion;

                case Key.Sleep:
                    return Keys.Sleep;

                case Key.Space:
                    return Keys.Space;

                case Key.T:
                    return Keys.T;

                case Key.Tab:
                    return Keys.Tab;

                case Key.Tilde:
                    return Keys.OemTilde;

                case Key.U:
                    return Keys.U;

                case Key.Unknown:
                    return Keys.None;

                case Key.Up:
                    return Keys.Up;

                case Key.V:
                    return Keys.V;

                case Key.W:
                    return Keys.W;

                case Key.X:
                    return Keys.X;

                case Key.Y:
                    return Keys.Y;

                case Key.Z:
                    return Keys.Z;

                default:
                    return Keys.None;
            }
        }

        #endregion
    }
}