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

namespace GreenBox3D
{
    public class GraphicsDeviceManager : IDisposable
    {
        private readonly Game _game;
        private GraphicsDevice _graphicsDevice;
        private int _preferredBackBufferHeight;
        private int _preferredBackBufferWidth;
        private SurfaceFormat _preferredBackBufferFormat;
        private DepthFormat _preferredDepthStencilFormat;
        private bool _synchronizedWithVerticalRetrace = true;
        private bool _disposed;
#if !(WINDOWS || LINUX)
        private bool _wantFullScreen = false;
#endif
        public static readonly int DefaultBackBufferHeight = 480;
        public static readonly int DefaultBackBufferWidth = 800;

        public GraphicsDeviceManager(Game game)
        {
            if (game == null)
                throw new ArgumentNullException("game", "The game cannot be null!");

            _game = game;

#if WINDOWS || MONOMAC || LINUX
            _preferredBackBufferHeight = DefaultBackBufferHeight;
            _preferredBackBufferWidth = DefaultBackBufferWidth;
#endif

            _preferredBackBufferFormat = SurfaceFormat.Color;
            _preferredDepthStencilFormat = DepthFormat.Depth24;
            _synchronizedWithVerticalRetrace = true;

#if WINDOWS || LINUX
            // TODO: This should not occur here... it occurs during Game.Initialize().
            CreateDevice();
#endif
        }

        ~GraphicsDeviceManager()
        {
            Dispose(false);
        }

        public void CreateDevice()
        {
            _graphicsDevice = new GraphicsDevice(this);

            Initialize();
            OnDeviceCreated(EventArgs.Empty);
        }

        public bool BeginDraw()
        {
            return true;
        }

        public void EndDraw()
        {
        }

        #region IGraphicsDeviceService Members

        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;
        // FIXME: Why does the GraphicsDeviceManager not know enough about the
        //        GraphicsDevice to raise these events without help?
        internal void OnDeviceDisposing(EventArgs e)
        {
            Raise(DeviceDisposing, e);
        }

        // FIXME: Why does the GraphicsDeviceManager not know enough about the
        //        GraphicsDevice to raise these events without help?
        internal void OnDeviceResetting(EventArgs e)
        {
            Raise(DeviceResetting, e);
        }

        // FIXME: Why does the GraphicsDeviceManager not know enough about the
        //        GraphicsDevice to raise these events without help?
        internal void OnDeviceReset(EventArgs e)
        {
            Raise(DeviceReset, e);
        }

        // FIXME: Why does the GraphicsDeviceManager not know enough about the
        //        GraphicsDevice to raise these events without help?
        internal void OnDeviceCreated(EventArgs e)
        {
            Raise(DeviceCreated, e);
        }

        private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e) where TEventArgs : EventArgs
        {
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_graphicsDevice != null)
                    {
                        _graphicsDevice.Dispose();
                        _graphicsDevice = null;
                    }
                }
                _disposed = true;
            }
        }

        #endregion

        public void ApplyChanges()
        {
            // Calling ApplyChanges() before CreateDevice() should have no effect
            if (_graphicsDevice == null)
                return;

#if WINDOWS || LINUX
            _game.ResizeWindow(false);
#elif MONOMAC
            _graphicsDevice.PresentationParameters.IsFullScreen = _wantFullScreen;

            // TODO: Implement multisampling (aka anti-alising) for all platforms!
			_game.ApplyChanges(this);
#endif
        }

        private void Initialize()
        {
#if WINDOWS
            _graphicsDevice.PresentationParameters.BackBufferFormat = _preferredBackBufferFormat;
            _graphicsDevice.PresentationParameters.BackBufferWidth = _preferredBackBufferWidth;
            _graphicsDevice.PresentationParameters.BackBufferHeight = _preferredBackBufferHeight;
            _graphicsDevice.PresentationParameters.DepthStencilFormat = _preferredDepthStencilFormat;
            _graphicsDevice.PresentationParameters.IsFullScreen = false;
            _graphicsDevice.Initialize();
#else
#if MONOMAC
            _graphicsDevice.PresentationParameters.IsFullScreen = _wantFullScreen;
#elif LINUX
            _graphicsDevice.PresentationParameters.IsFullScreen = false;
#endif

            // TODO: Implement multisampling (aka anti-alising) for all platforms!
            _graphicsDevice.Initialize();

#if !MONOMAC
            ApplyChanges();
#endif
#endif
        }

        public void ToggleFullScreen()
        {
            IsFullScreen = !IsFullScreen;
        }

        public bool IsFullScreen
        {
            get
            {
#if WINDOWS || LINUX
                return _graphicsDevice.PresentationParameters.IsFullScreen;
#else
                if (_graphicsDevice != null)
                    return _graphicsDevice.PresentationParameters.IsFullScreen;
                else
                    return _wantFullScreen;
#endif
            }
            set
            {
#if WINDOWS || LINUX
                _graphicsDevice.PresentationParameters.IsFullScreen = value;
#else
                _wantFullScreen = value;
                if (_graphicsDevice != null)
                {
                    _graphicsDevice.PresentationParameters.IsFullScreen = value;
                }
#endif
            }
        }

        public bool PreferMultiSampling { get; set; }
        public SurfaceFormat PreferredBackBufferFormat { get { return _preferredBackBufferFormat; } set { _preferredBackBufferFormat = value; } }
        public int PreferredBackBufferHeight { get { return _preferredBackBufferHeight; } set { _preferredBackBufferHeight = value; } }
        public int PreferredBackBufferWidth { get { return _preferredBackBufferWidth; } set { _preferredBackBufferWidth = value; } }
        public DepthFormat PreferredDepthStencilFormat { get { return _preferredDepthStencilFormat; } set { _preferredDepthStencilFormat = value; } }

        public bool SynchronizeWithVerticalRetrace
        {
            get
            {
#if LINUX
                return _game.Platform.VSyncEnabled;
#else
                return _synchronizedWithVerticalRetrace;
#endif
            }
            set
            {
#if LINUX
    // TODO: I'm pretty sure this shouldn't occur until ApplyChanges().
                _game.Platform.VSyncEnabled = value;
#else
                _synchronizedWithVerticalRetrace = value;
#endif
            }
        }

        public GraphicsDevice GraphicsDevice { get { return _graphicsDevice; } }
    }
}