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

using GreenBox3D.Platform.Desktop;

namespace GreenBox3D.Platform
{
    internal abstract class GamePlatform : IDisposable
    {
        #region Fields

        protected TimeSpan InactiveSleepTime = TimeSpan.FromMilliseconds(20.0);
        protected bool NeedsToResetElapsedTime = false;
        private bool _disposed;
        private bool _isActive;
        private bool _isMouseVisible;

        #endregion

        #region Constructors and Destructors

        protected GamePlatform(Game game)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            Game = game;
        }

        ~GamePlatform()
        {
            Dispose(false);
        }

        #endregion

        #region Public Events

        public event EventHandler<EventArgs> Activated;
        public event EventHandler<EventArgs> Deactivated;

        #endregion

        #region Public Properties

        public Game Game { get; private set; }

        public bool IsActive
        {
            get { return _isActive; }
            internal set
            {
                _isActive = value;
                Raise(_isActive ? Activated : Deactivated, EventArgs.Empty);
            }
        }

        public bool IsMouseVisible
        {
            get { return _isMouseVisible; }
            set
            {
                _isMouseVisible = value;
                OnIsMouseVisibleChanged();
            }
        }

        public abstract bool VSyncEnabled { get; set; }
        public GameWindow Window { get; protected set; }

        #endregion

        #region Properties

        protected bool IsDisposed { get { return _disposed; } }

        #endregion

        #region Public Methods and Operators

        public static GamePlatform Create(Game game)
        {
            return new DesktopPlatform(game);
        }

        /// <summary>
        ///     Gives derived classes an opportunity to do work before any
        ///     components are initialized.  Note that the base implementation sets
        ///     IsActive to true, so derived classes should either call the base
        ///     implementation or set IsActive to true by their own means.
        /// </summary>
        public virtual void BeforeInitialize()
        {
            IsActive = true;

            if (Game.GraphicsDevice == null)
                Game.GraphicsDeviceManager.CreateDevice();
        }

        /// <summary>
        ///     Gives derived classes an opportunity to do work just before Draw
        ///     is called for all IDrawable components.  Returning false from this
        ///     method will result in this round of Draw calls being skipped.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public abstract bool BeforeRender(GameTime gameTime);

        /// <summary>
        ///     Gives derived classes an opportunity to do work just before the
        ///     run loop is begun.  Implementations may also return false to prevent
        ///     the run loop from starting.
        /// </summary>
        /// <returns></returns>
        public virtual bool BeforeRun()
        {
            return true;
        }

        /// <summary>
        ///     Gives derived classes an opportunity to do work just before Update
        ///     is called for all IUpdatable components.  Returning false from this
        ///     method will result in this round of Update calls being skipped.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public abstract bool BeforeUpdate(GameTime gameTime);

        /// <summary>
        ///     Performs application-defined tasks associated with freeing,
        ///     releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     When implemented in a derived class, causes the game to enter
        ///     full-screen mode.
        /// </summary>
        public abstract void EnterFullScreen();

        /// <summary>
        ///     When implemented in a derived, ends the active run loop.
        /// </summary>
        public abstract void Exit();

        /// <summary>
        ///     When implemented in a derived class, causes the game to exit
        ///     full-screen mode.
        /// </summary>
        public abstract void ExitFullScreen();

        public virtual void Present()
        {
        }

        /// <summary>
        ///     MSDN: Use this method if your game is recovering from a slow-running state, and ElapsedGameTime is too large to be useful.
        ///     Frame timing is generally handled by the Game class, but some platforms still handle it elsewhere. Once all platforms
        ///     rely on the Game class's functionality, this method and any overrides should be removed.
        /// </summary>
        public virtual void ResetElapsedTime()
        {
        }

        /// <summary>
        ///     When implemented in a derived, starts the run loop and blocks
        ///     until it has ended.
        /// </summary>
        public abstract void RunLoop();

        /// <summary>
        ///     Gives derived classes an opportunity to take action after
        ///     Game.TargetElapsedTime has been set.
        /// </summary>
        public virtual void TargetElapsedTimeChanged()
        {
        }

        /// <summary>
        ///     Gives derived classes an opportunity to modify
        ///     Game.TargetElapsedTime before it is set.
        /// </summary>
        /// <param name="value">The proposed new value of TargetElapsedTime.</param>
        /// <returns>The new value of TargetElapsedTime that will be set.</returns>
        public virtual TimeSpan TargetElapsedTimeChanging(TimeSpan value)
        {
            return value;
        }

        #endregion

        #region Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                _disposed = true;
        }

        protected virtual void OnIsMouseVisibleChanged()
        {
        }

        private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e) where TEventArgs : EventArgs
        {
            if (handler != null)
                handler(this, e);
        }

        #endregion
    }
}