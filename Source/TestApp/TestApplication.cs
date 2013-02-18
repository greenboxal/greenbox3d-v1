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
using System.Text;
using System.Threading.Tasks;

using GreenBox3D;
using GreenBox3D.Content;
using GreenBox3D.ContentPipeline.Compiler;
using GreenBox3D.Graphics;
using GreenBox3D.Input;
using GreenBox3D.Math;

namespace TestApp
{
    public class TestApplication : Game
    {
        #region Constructors and Destructors

        public TestApplication()
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
            GraphicsDeviceManager.PreferredBackBufferHeight = 720;

            FileManager.RegisterLoader(new FolderFileLoader("./Output/"));

#if DEBUG
            GreenBox3D.ContentPipeline.PipelineManager.RegisterJustInDesignExtensions(new PipelineProject("ContentProject.rb"));
#endif
        }

        #endregion

        #region Methods

        protected override void Initialize()
        {
            base.Initialize();
        }

        private Effect effect;
        protected override void LoadContent()
        {
            effect = EffectManager.LoadEffect("Simple/Simple");
        }

        protected override void Render(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        protected override void Update(GameTime gameTime)
        {
        }

        #endregion
    }
}