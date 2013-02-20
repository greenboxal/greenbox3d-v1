// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D.Graphics;
using GreenBox3D.Input;
using GreenBox3D.Platform.Dekstop;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

using Keyboard = GreenBox3D.Input.Keyboard;
using Mouse = GreenBox3D.Input.Mouse;
using Rectangle = GreenBox3D.Rectangle;

namespace GreenBox3D.Platform.Desktop
{
    internal class DesktopWindow : GameWindow
    {
        #region Fields

        private bool _allowUserResizing;
        private Rectangle _clientBounds;
        private bool _disposed;
        private List<Keys> _keys;
        private bool _updateClientBounds;
        private OpenTK.GameWindow _window;
        private WindowState _windowState;

        #endregion

        #region Constructors and Destructors

        public DesktopWindow()
        {
            Initialize();
        }

        ~DesktopWindow()
        {
            Dispose(false);
        }

        #endregion

        #region Public Properties

        public override bool AllowUserResizing
        {
            get { return _allowUserResizing; }
            set
            {
                _allowUserResizing = value;
                _window.WindowBorder = _allowUserResizing ? WindowBorder.Resizable : WindowBorder.Fixed;
            }
        }

        public override Rectangle ClientBounds { get { return _clientBounds; } }

        #endregion

        #region Properties

        internal Game Game { get; set; }
        internal OpenTK.GameWindow Window { get { return _window; } }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        internal void ChangeClientBounds(Rectangle clientBounds)
        {
            if (_updateClientBounds)
                return;

            _updateClientBounds = true;
            _clientBounds = clientBounds;
        }

        internal void Run(double updateRate)
        {
            _window.Run(updateRate);
        }

        internal void ToggleFullScreen()
        {
            _windowState = _windowState == WindowState.Fullscreen ? WindowState.Normal : WindowState.Fullscreen;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Dispose/release managed objects
                _window.Dispose();
            }

            _disposed = true;
        }

        protected override void SetTitle(string title)
        {
            _window.Title = title;
        }

        private void HandleInput()
        {
            Keyboard.SetKeys(_keys);
        }

        private void Initialize()
        {
            GraphicsContext.ShareContexts = true;

            _window = new OpenTK.GameWindow();
            _window.RenderFrame += OnRenderFrame;
            _window.UpdateFrame += OnUpdateFrame;
            _window.Closing += OpenTkGameWindowClosing;
            _window.Resize += OnResize;
            _window.Keyboard.KeyDown += KeyboardKeyDown;
            _window.Keyboard.KeyUp += KeyboardKeyUp;

            // Set the window icon.
            _window.Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);

            _updateClientBounds = false;
            _clientBounds = new Rectangle(_window.ClientRectangle.X, _window.ClientRectangle.Y, _window.ClientRectangle.Width, _window.ClientRectangle.Height);
            _windowState = _window.WindowState;

            _keys = new List<Keys>();

#if WINDOWS || LINUX
            Mouse.SetWindow(_window);
#else
            Mouse.UpdateMouseInfo(window.Mouse);
#endif

            // Default no resizing
            AllowUserResizing = false;
        }

        private void KeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            Keys key = KeyboardUtil.ConvertKey(e.Key);
            if (!_keys.Contains(key))
                _keys.Add(key);
        }

        private void KeyboardKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            Keys key = KeyboardUtil.ConvertKey(e.Key);
            if (_keys.Contains(key))
                _keys.Remove(key);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            if (!GraphicsContext.CurrentContext.IsCurrent)
                _window.MakeCurrent();

            UpdateWindowState();
        }

        private void OnResize(object sender, EventArgs e)
        {
            var winWidth = _window.ClientRectangle.Width;
            var winHeight = _window.ClientRectangle.Height;
            var winRect = new Rectangle(0, 0, winWidth, winHeight);

            // If window size is zero, leave bounds unchanged
            // OpenTK appears to set the window client size to 1x1 when minimizing
            if (winWidth <= 1 || winHeight <= 1)
                return;

            // If we've already got a pending change, do nothing
            if (_updateClientBounds)
                return;

            // FIXME: This should be really set?
            Game.GraphicsDevice.Viewport = new Viewport(0, 0, winWidth, winHeight);

            ChangeClientBounds(winRect);
            OnClientSizeChanged();
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            UpdateWindowState();

            if (Game != null)
            {
                HandleInput();
                Game.Tick();
            }
        }

        private void OpenTkGameWindowClosing(object sender, CancelEventArgs e)
        {
            Game.Exit();
        }

        private void UpdateWindowState()
        {
            // we should wait until window's not fullscreen to resize
            if (_updateClientBounds)
            {
                _window.ClientRectangle = new System.Drawing.Rectangle(_clientBounds.X, _clientBounds.Y, _clientBounds.Width, _clientBounds.Height);
                _updateClientBounds = false;

                // if the window-state is set from the outside (maximized button pressed) we have to update it here.
                // if it was set from the inside (.IsFullScreen changed), we have to change the window.
                // this code might not cover all corner cases
                // window was maximized
                if ((_windowState == WindowState.Normal && _window.WindowState == WindowState.Maximized) || (_windowState == WindowState.Maximized && _window.WindowState == WindowState.Normal))
                    _windowState = _window.WindowState; // maximize->normal and normal->maximize are usually set from the outside
                else
                    _window.WindowState = _windowState; // usually fullscreen-stuff is set from the code

                // fixes issue on linux (and windows?) that AllowUserResizing is not set any more when exiting fullscreen mode
                WindowBorder desired = AllowUserResizing ? WindowBorder.Resizable : WindowBorder.Fixed;
                if (desired != _window.WindowBorder && _window.WindowState != WindowState.Fullscreen)
                    _window.WindowBorder = desired;
            }
        }

        #endregion
    }
}