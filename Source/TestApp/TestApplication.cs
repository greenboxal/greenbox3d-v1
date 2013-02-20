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

        private Effect _effect;
        private IndexBuffer _indices;
        private VertexBuffer _vertices;
        protected override void LoadContent()
        {
            _effect = EffectManager.LoadEffect("Simple/Simple");
            
            int[] indices = new[] { 0, 1, 2 };
            VertexPositionNormalColor[] positions = new[]
            {
                new VertexPositionNormalColor(new Vector3(0.75f, 0.75f, 0.0f), new Vector3(), new Color(255, 0, 0)), 
                new VertexPositionNormalColor(new Vector3(0.75f, -0.75f, 0.0f), new Vector3(), new Color(0, 255, 0)),
                new VertexPositionNormalColor(new Vector3(-0.75f, -0.75f, 0.0f), new Vector3(), new Color(0, 0, 255)),
            };

            _indices = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.StaticDraw);
            _indices.SetData(indices);

            _vertices = new VertexBuffer(GraphicsDevice, typeof(VertexPositionNormalColor), positions.Length, BufferUsage.StaticDraw);
            _vertices.SetData(positions);
        }

        protected override void Render(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.Indices = _indices;
            GraphicsDevice.SetVertexBuffer(_vertices);

            _effect.Parameters["WorldViewProjection"].SetValue(Matrix.Identity);

            foreach (EffectPass pass in _effect.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, _indices.ElementCount, 0, 1);
            }
        }

        protected override void Update(GameTime gameTime)
        {
        }

        #endregion
    }
}