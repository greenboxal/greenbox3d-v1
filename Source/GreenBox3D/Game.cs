// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GreenBox3D.Content;
using GreenBox3D.Graphics;
using GreenBox3D.Platform;
#if WINDOWS || LINUX
using GreenBox3D.Platform.Desktop;

#else
#error Unsupported platform.
#endif

namespace GreenBox3D
{
    public class Game : IDisposable
    {
        #region Constants

        private const float DefaultTargetFramesPerSecond = 60.0f;

        #endregion

        #region Static Fields

        private static Game _instance;

        #endregion

        #region Fields

        internal GamePlatform Platform;
        private readonly GameTime _gameTime = new GameTime();
        private readonly Stopwatch _gameTimer = Stopwatch.StartNew();
        private readonly TimeSpan _maxElapsedTime = TimeSpan.FromMilliseconds(500);
        private TimeSpan _accumulatedElapsedTime;
        private GraphicsDeviceManager _graphicsDeviceManager;
        private TimeSpan _inactiveSleepTime = TimeSpan.FromSeconds(1);
        private bool _initialized;
        private bool _isDisposed;
        private bool _isFixedTimeStep = true;
        private bool _suppressDraw;
        private TimeSpan _targetElapsedTime = TimeSpan.FromTicks(10000000 / (long)DefaultTargetFramesPerSecond);

        #endregion

        #region Constructors and Destructors

        public Game()
        {
            _instance = this;
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            Content = new ContentManager(_graphicsDeviceManager.GraphicsDevice);

            Platform = GamePlatform.Create(this);
            Platform.Activated += OnActivated;
            Platform.Deactivated += OnDeactivated;

            // Set the window title.
            // TODO: Get the title from the WindowsPhoneManifest.xml for WP7 projects.
            string windowTitle = string.Empty;
            var assembly = Assembly.GetEntryAssembly();

            //Use the Title attribute of the Assembly if possible.
            var assemblyTitleAtt = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)));
            if (assemblyTitleAtt != null)
                windowTitle = assemblyTitleAtt.Title;

            // Otherwise, fallback to the Name of the assembly.
            if (string.IsNullOrEmpty(windowTitle))
                windowTitle = assembly.GetName().Name;

            Window.Title = windowTitle;
        }

        ~Game()
        {
            Dispose(false);
        }

        #endregion

        #region Public Events

        public event EventHandler<EventArgs> Activated;
        public event EventHandler<EventArgs> Deactivated;
        public event EventHandler<EventArgs> Disposed;
        public event EventHandler<EventArgs> Exiting;

        #endregion

        #region Public Properties

        public ContentManager Content { get; set; }

        public GraphicsDevice GraphicsDevice
        {
            get
            {
                if (_graphicsDeviceManager == null)
                    throw new InvalidOperationException("No GraphicsDevice Service");

                return _graphicsDeviceManager.GraphicsDevice;
            }
        }

        public GraphicsDeviceManager GraphicsDeviceManager { get { return _graphicsDeviceManager; } }
        public TimeSpan InactiveSleepTime { get { return _inactiveSleepTime; } set { _inactiveSleepTime = value; } }
        public bool IsActive { get { return Platform.IsActive; } }
        public bool IsFixedTimeStep { get { return _isFixedTimeStep; } set { _isFixedTimeStep = value; } }
        public bool IsMouseVisible { get { return Platform.IsMouseVisible; } set { Platform.IsMouseVisible = value; } }

        public TimeSpan TargetElapsedTime
        {
            get { return _targetElapsedTime; }
            set
            {
                // Give GamePlatform implementations an opportunity to override
                // the new value.
                value = Platform.TargetElapsedTimeChanging(value);

                if (value <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("value", "value must be positive and non-zero.");

                if (value == _targetElapsedTime)
                    return;

                _targetElapsedTime = value;
                Platform.TargetElapsedTimeChanged();
            }
        }

        public GameWindow Window { get { return Platform.Window; } }

        #endregion

        #region Properties

        internal static Game Instance { get { return _instance; } }
        // FIXME: Internal members should be eliminated.
        // Currently Game.Initialized is used by the Mac game window class to
        // determine whether to raise DeviceResetting and DeviceReset on
        // GraphicsDeviceManager.
        internal bool Initialized { get { return _initialized; } }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Raise(Disposed, EventArgs.Empty);
        }

        public void Exit()
        {
            Platform.Exit();
        }

        public void ResetElapsedTime()
        {
            Platform.ResetElapsedTime();
            _gameTimer.Reset();
            _gameTimer.Start();
            _accumulatedElapsedTime = TimeSpan.Zero;
            _gameTime.ElapsedTime = TimeSpan.Zero;
        }

        public void Run()
        {
            AssertNotDisposed();

            if (!Platform.BeforeRun())
                return;

            if (!_initialized)
            {
                DoInitialize();
                _initialized = true;
            }

            BeginRun();
            Platform.RunLoop();
            EndRun();
            DoExiting();
        }

        public void RunOneFrame()
        {
            AssertNotDisposed();

            if (!Platform.BeforeRun())
                return;

            if (!_initialized)
            {
                DoInitialize();
                _initialized = true;
            }

            BeginRun();
            Tick();
            EndRun();
        }

        public void SuppressDraw()
        {
            _suppressDraw = true;
        }

        #endregion

        #region Methods

        internal void ApplyChanges(GraphicsDeviceManager manager)
        {
            if (GraphicsDevice.PresentationParameters.IsFullScreen)
                Platform.EnterFullScreen();
            else
                Platform.ExitFullScreen();

            var viewport = new Viewport(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

            GraphicsDevice.Viewport = viewport;
        }

        internal void DoExiting()
        {
            OnExiting(this, EventArgs.Empty);
            UnloadContent();
        }

        internal void DoInitialize()
        {
            AssertNotDisposed();
            Platform.BeforeInitialize();
            Initialize();
        }

        internal void DoRender(GameTime gameTime)
        {
            AssertNotDisposed();

            if (Platform.BeforeRender(gameTime) && BeginRender())
            {
                Render(gameTime);
                EndRender();
            }
        }

        internal void DoUpdate(GameTime gameTime)
        {
            AssertNotDisposed();
            if (Platform.BeforeUpdate(gameTime))
                Update(gameTime);
        }

        internal void ResizeWindow(bool changed)
        {
            ((DesktopPlatform)Platform).ResetWindowBounds(changed);
        }

        internal void Tick()
        {
            // NOTE: This code is very sensitive and can break very badly
            // with even what looks like a safe change.  Be sure to test 
            // any change fully in both the fixed and variable timestep 
            // modes across multiple devices and platforms.

            RetryTick:

            // Advance the accumulated elapsed time.
            _accumulatedElapsedTime += _gameTimer.Elapsed;
            _gameTimer.Reset();
            _gameTimer.Start();

            // If we're in the fixed timestep mode and not enough time has elapsed
            // to perform an update we sleep off the the remaining time to save battery
            // life and/or release CPU time to other threads and processes.
            if (IsFixedTimeStep && _accumulatedElapsedTime < TargetElapsedTime)
            {
                var sleepTime = (int)(TargetElapsedTime - _accumulatedElapsedTime).TotalMilliseconds;

                // NOTE: While sleep can be inaccurate in general it is 
                // accurate enough for frame limiting purposes if some
                // fluctuation is an acceptable result.
#if WINRT
                Task.Delay(sleepTime).Wait();
#else
                Thread.Sleep(sleepTime);
#endif
                goto RetryTick;
            }

            // Do not allow any update to take longer than our maximum.
            if (_accumulatedElapsedTime > _maxElapsedTime)
                _accumulatedElapsedTime = _maxElapsedTime;

            // http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.gametime.isrunningslowly.aspx
            // Calculate IsRunningSlowly for the fixed time step, but only when the accumulated time
            // exceeds the target time.

            if (IsFixedTimeStep)
            {
                _gameTime.ElapsedTime = TargetElapsedTime;
                var stepCount = 0;

                _gameTime.IsRunningSlowly = (_accumulatedElapsedTime > TargetElapsedTime);

                // Perform as many full fixed length time steps as we can.
                while (_accumulatedElapsedTime >= TargetElapsedTime)
                {
                    _gameTime.TotalTime += TargetElapsedTime;
                    _accumulatedElapsedTime -= TargetElapsedTime;
                    ++stepCount;

                    DoUpdate(_gameTime);
                }

                // Draw needs to know the total elapsed time
                // that occured for the fixed length updates.
                _gameTime.ElapsedTime = TimeSpan.FromTicks(TargetElapsedTime.Ticks * stepCount);
            }
            else
            {
                // Perform a single variable length update.
                _gameTime.ElapsedTime = _accumulatedElapsedTime;
                _gameTime.TotalTime += _accumulatedElapsedTime;
                _accumulatedElapsedTime = TimeSpan.Zero;

                DoUpdate(_gameTime);
            }

            // Draw unless the update suppressed it.
            if (_suppressDraw)
                _suppressDraw = false;
            else
                DoRender(_gameTime);
        }

        protected virtual bool BeginRender()
        {
            return true;
        }

        protected virtual void BeginRun()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                if (Content != null)
                    Content.Dispose();

                if (_graphicsDeviceManager != null)
                {
                    _graphicsDeviceManager.Dispose();
                    _graphicsDeviceManager = null;
                }

                Platform.Dispose();
            }

            _isDisposed = true;
        }

        protected virtual void EndRender()
        {
            Platform.Present();
        }

        protected virtual void EndRun()
        {
        }

        protected virtual void Initialize()
        {
            // TODO: We shouldn't need to do this here.
            ApplyChanges(_graphicsDeviceManager);

            // FIXME: If this test fails, is LoadContent ever called?  This
            //        seems like a condition that warrants an exception more
            //        than a silent failure.
            if (_graphicsDeviceManager != null && _graphicsDeviceManager.GraphicsDevice != null)
                LoadContent();
        }

        protected virtual void LoadContent()
        {
        }

        protected virtual void OnActivated(object sender, EventArgs args)
        {
            AssertNotDisposed();
            Raise(Activated, args);
        }

        protected virtual void OnDeactivated(object sender, EventArgs args)
        {
            AssertNotDisposed();
            Raise(Deactivated, args);
        }

        protected virtual void OnExiting(object sender, EventArgs args)
        {
            Raise(Exiting, args);
        }

        protected virtual void Render(GameTime gameTime)
        {
        }

        protected virtual void UnloadContent()
        {
        }

        protected virtual void Update(GameTime gameTime)
        {
        }

        [DebuggerNonUserCode]
        private void AssertNotDisposed()
        {
            if (!_isDisposed)
                return;

            string name = GetType().Name;
            throw new ObjectDisposedException(name, string.Format("The {0} object was used after being Disposed.", name));
        }

        // FIXME: We should work toward eliminating internal methods.  They
        //        break entirely the possibility that additional platforms could
        //        be added by third parties without changing MonoGame itself.
        private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e) where TEventArgs : EventArgs
        {
            if (handler != null)
                handler(this, e);
        }

        #endregion
    }
}