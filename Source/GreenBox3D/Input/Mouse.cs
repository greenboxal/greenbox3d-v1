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
#if MONOMAC || WINDOWS
using System.Runtime.InteropServices;
using System.Drawing;
#endif
#if WINDOWS || LINUX
using OpenTK;
using OpenTK.Input;

using MouseInfo = OpenTK.Input.Mouse;

#elif MONOMAC
using MonoMac.Foundation;
using MonoMac.AppKit;
#endif

namespace GreenBox3D.Input
{
    using Point = System.Drawing.Point;

    public static class Mouse
    {
        internal static MouseState State;
#if WINDOWS || LINUX
        private static MouseDevice _mouse;
#endif
#if WINDOWS
        private static OpenTK.GameWindow _window;

        internal static void SetWindow(OpenTK.GameWindow window)
        {
            _window = window;
            _mouse = window.Mouse;
        }
#elif LINUX         
        
        static OpenTK.GameWindow Window;

        internal static void setWindows(OpenTK.GameWindow window)
		{
            Window = window;
            
			_mouse = window.Mouse;
			_mouse.Move += HandleWindowMouseMove;
		}

        internal static void HandleWindowMouseMove (object sender, OpenTK.Input.MouseMoveEventArgs e)
		{          
			UpdateStatePosition(e.X, e.Y);
		}

#elif MONOMAC
        internal static GameWindow Window;
        internal static float ScrollWheelValue;
#endif

        #region Public interface

        public static MouseState GetState()
        {
#if MONOMAC
    //We need to maintain precision...
            State.ScrollWheelValue = (int)ScrollWheelValue;
#elif WINDOWS || LINUX

            // Maybe someone is tring to get mouse before initialize
            if (_mouse == null)
                return State;

#if WINDOWS
            POINT p;
            GetCursorPos(out p);
            var pc = _window.PointToClient(p.ToPoint());
            State.X = pc.X;
            State.Y = pc.Y;
#endif

            State.LeftButton = _mouse[MouseButton.Left] ? ButtonState.Pressed : ButtonState.Released;
            State.RightButton = _mouse[MouseButton.Right] ? ButtonState.Pressed : ButtonState.Released;
            State.MiddleButton = _mouse[MouseButton.Middle] ? ButtonState.Pressed : ButtonState.Released;

            // WheelPrecise is divided by 120 (WHEEL_DELTA) in OpenTK (WinGLNative.cs)
            // We need to counteract it to get the same value XNA provides
            State.ScrollWheelValue = (int)(_mouse.WheelPrecise * 120);
#endif

            return State;
        }

        public static void SetPosition(int x, int y)
        {
            UpdateStatePosition(x, y);

#if WINDOWS || LINUX
            // Correcting the coordinate system
            // Only way to set the mouse position !!!
            Point pt = _window.PointToScreen(new Point(x, y));
#endif

#if WINDOWS
            SetCursorPos(pt.X, pt.Y);
#elif LINUX
            OpenTK.Input.Mouse.SetPosition(pt.X, pt.Y);
#elif MONOMAC
            var mousePt = NSEvent.CurrentMouseLocation;
            NSScreen currentScreen = null;
            foreach (var screen in NSScreen.Screens) {
                if (screen.Frame.Contains(mousePt)) {
                    currentScreen = screen;
                    break;
                }
            }
            
            var point = new PointF(x, Window.ClientBounds.Height - y);
            var windowPt = Window.ConvertPointToView(point, null);
            var screenPt = Window.Window.ConvertBaseToScreen(windowPt);
            var flippedPt = new PointF(screenPt.X, currentScreen.Frame.Size.Height - screenPt.Y);
            flippedPt.Y += currentScreen.Frame.Location.Y;
            
            
            CGSetLocalEventsSuppressionInterval(0.0);
            CGWarpMouseCursorPosition(flippedPt);
            CGSetLocalEventsSuppressionInterval(0.25);
#endif
        }

        #endregion // Public interface

        private static void UpdateStatePosition(int x, int y)
        {
            State.X = x;
            State.Y = y;
        }

#if WINDOWS
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int X, int Y);

        /// <summary>
        ///     Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public Point ToPoint()
            {
                return new Point(X, Y);
            }
        }

        /// <summary>
        ///     Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
#elif MONOMAC
        [DllImport (MonoMac.Constants.CoreGraphicsLibrary)]
        extern static void CGWarpMouseCursorPosition(PointF newCursorPosition);
        
        [DllImport (MonoMac.Constants.CoreGraphicsLibrary)]
        extern static void CGSetLocalEventsSuppressionInterval(double seconds);
#endif
    }
}