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

using GreenBox3D.Math;

namespace GreenBox3D
{
    public abstract class GameWindow
    {
        #region Fields

        private string _title;

        #endregion

        #region Public Events

        public event EventHandler<EventArgs> ClientSizeChanged;

        #endregion

        #region Public Properties

        public abstract bool AllowUserResizing { get; set; }
        public abstract Rectangle ClientBounds { get; }

        public string Title
        {
            get { return _title; }
            set
            {
                SetTitle(value);
                _title = value;
            }
        }

        #endregion

        #region Methods

        protected void OnActivated()
        {
        }

        protected void OnClientSizeChanged()
        {
            if (ClientSizeChanged != null)
                ClientSizeChanged(this, EventArgs.Empty);
        }

        protected void OnDeactivated()
        {
        }

        protected void OnPaint()
        {
        }

        protected abstract void SetTitle(string title);

        #endregion
    }
}