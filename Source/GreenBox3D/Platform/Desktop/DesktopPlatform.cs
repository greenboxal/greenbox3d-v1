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

using GreenBox3D.Graphics;
using GreenBox3D.Input;
using GreenBox3D.Math;

using OpenTK;

namespace GreenBox3D.Platform.Desktop
{
    internal class DesktopPlatform : GamePlatform
    {
        // Stored the current screen state, so we can check if it has changed.

        #region Fields

        private bool _isCurrentlyFullScreen;
        private DesktopWindow _view;

        #endregion

        #region Constructors and Destructors

        public DesktopPlatform(Game game)
            : base(game)
        {
            _view = new DesktopWindow();
            _view.Game = game;

            Window = _view;
            IsMouseVisible = true;
        }

        #endregion

        #region Public Properties

        public override bool VSyncEnabled { get { return _view.Window.VSync == VSyncMode.On ? true : false; } set { _view.Window.VSync = value ? VSyncMode.On : VSyncMode.Off; } }

        #endregion

        #region Public Methods and Operators

        public override bool BeforeRender(GameTime gameTime)
        {
            return true;
        }

        public override bool BeforeUpdate(GameTime gameTime)
        {
            IsActive = _view.Window.Focused;
            return true;
        }

        public override void EnterFullScreen()
        {
            ResetWindowBounds(false);
        }

        public override void Exit()
        {
            if (!_view.Window.IsExiting)
                _view.Window.Exit();

            DisplayDevice.Default.RestoreResolution();
        }

        public override void ExitFullScreen()
        {
            ResetWindowBounds(false);
        }

        public override void Present()
        {
            base.Present();

            var device = Game.GraphicsDevice;
            if (device != null)
                device.Present();

            if (_view != null)
                _view.Window.SwapBuffers();
        }

        public override void RunLoop()
        {
            ResetWindowBounds(false);
            _view.Window.Run(0);
        }

        #endregion

        #region Methods

        internal void ResetWindowBounds(bool toggleFullScreen)
        {
            Rectangle bounds = Window.ClientBounds;
            var graphicsDeviceManager = Game.GraphicsDeviceManager;

            if (graphicsDeviceManager.IsFullScreen)
            {
                bounds = new Rectangle(0, 0, graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight);

                if (DisplayDevice.Default.Width != graphicsDeviceManager.PreferredBackBufferWidth || DisplayDevice.Default.Height != graphicsDeviceManager.PreferredBackBufferHeight)
                    DisplayDevice.Default.ChangeResolution(graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight, DisplayDevice.Default.BitsPerPixel, DisplayDevice.Default.RefreshRate);
            }
            else
            {
                // Switch back to the normal screen resolution
                DisplayDevice.Default.RestoreResolution();

                // Now update the bounds 
                bounds.Width = graphicsDeviceManager.PreferredBackBufferWidth;
                bounds.Height = graphicsDeviceManager.PreferredBackBufferHeight;
            }

            // Now we set our Presentation Parameters
            var device = graphicsDeviceManager.GraphicsDevice;

            // FIXME: Eliminate the need for null checks by only calling
            //        ResetWindowBounds after the device is ready.  Or,
            //        possibly break this method into smaller methods.
            if (device != null)
            {
                PresentationParameters parms = device.PresentationParameters;
                parms.BackBufferHeight = bounds.Height;
                parms.BackBufferWidth = bounds.Width;
            }

            if (graphicsDeviceManager.IsFullScreen != _isCurrentlyFullScreen)
                _view.ToggleFullScreen();

            // we only change window bounds if we are not fullscreen
            // or if fullscreen mode was just entered
            if (!graphicsDeviceManager.IsFullScreen || (graphicsDeviceManager.IsFullScreen != _isCurrentlyFullScreen))
                _view.ChangeClientBounds(bounds);

            // store the current fullscreen state
            _isCurrentlyFullScreen = graphicsDeviceManager.IsFullScreen;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (_view != null)
                {
                    _view.Dispose();
                    _view = null;
                }
            }

            base.Dispose(disposing);
        }

        protected override void OnIsMouseVisibleChanged()
        {
            MouseState oldState = Mouse.GetState();
            _view.Window.CursorVisible = IsMouseVisible;

            // IsMouseVisible changes the location of the cursor on Linux (and Windows?) and we have to manually set it back to the correct position
            System.Drawing.Point mousePos = _view.Window.PointToScreen(new System.Drawing.Point(oldState.X, oldState.Y));
            OpenTK.Input.Mouse.SetPosition(mousePos.X, mousePos.Y);
        }

        #endregion
    }
}