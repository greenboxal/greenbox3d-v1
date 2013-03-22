using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreenBox3D;
using GreenBox3D.Graphics;
using GreenBox3D.Input;

namespace TestApp
{
    public class TestGame : Game, IMouseFilter, IKeyboardFilter
    {
        private static readonly ILogger Log = LogManager.GetLogger(typeof(TestGame));

        private GraphicsDevice _graphicsDevice;
        private IInputManager _inputManager;

        public TestGame()
        {
        }

        protected override void Initialize()
        {
            PresentationParameters presentationParameters = new PresentationParameters();

            presentationParameters.BackBufferWidth = 1280;
            presentationParameters.BackBufferHeight = 720;

            Platform.InitializeGraphics(presentationParameters);
            Platform.InitializeInput();

            _graphicsDevice = GetService<IGraphicsDeviceManager>().GraphicsDevice;
            _inputManager = GetService<IInputManager>();
            _inputManager.RegisterMouseFilter(this);
            _inputManager.RegisterKeyboardFilter(this);
        }

        int lastX, lastY;
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            bool show = false;
            
            if (mouse.X != lastX)
            {
                lastX = mouse.X;
                show = true;
            }

            if (mouse.Y != lastY)
            {
                lastY = mouse.Y;
                show = true;
            }

            if (show)
                Log.Message("Raw Input: {0} {1}", lastX, lastY);
        }

        protected override void Render(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);
        }

        protected override void OnResize()
        {
            
        }

        public bool OnMouseDown(MouseButton button, int x, int y)
        {
            Log.Message("OnMouseDown: {0} {1} {2}", button, x, y);
            return true;
        }

        public bool OnMouseUp(MouseButton button, int x, int y)
        {
            Log.Message("OnMouseUp: {0} {1} {2}", button, x, y);
            return true;
        }

        public bool OnMouseMove(int x, int y)
        {
            Log.Message("OnMouseMove: {0} {1}", x, y);
            return true;
        }

        public bool OnKeyDown(Keys key, KeyModifiers mod)
        {
            Log.Message("OnKeyDown: {0} {1}", key, mod);
            return true;
        }

        public bool OnKeyUp(Keys key, KeyModifiers mod)
        {
            Log.Message("OnKeyUp: {0} {1}", key, mod);
            return true;
        }

        public bool OnKeyChar(char unicode)
        {
            Log.Message("OnKeyChar: {0}", unicode);
            return true;
        }

        public bool OnMouseWheel(int delta, int x, int y)
        {
            Log.Message("OnMouseWheel: {0} {1} {2}", delta, x, y);
            return true;
        }
    }
}
