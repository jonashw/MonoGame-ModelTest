using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameModelTest.Controls;

namespace MonoGameModelTest
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Eyeball[] _eyeballs;
        private Octohedron _octohedron;
        private SpriteFont _font;
        private Texture2D _pixelTexture;
        private Camera _camera;
        private readonly KeyboardEvents _keyboard;
        private readonly MouseEvents _mouse;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                PreferMultiSampling = true
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _keyboard = new KeyboardEvents();
            _mouse = new MouseEvents(Window);
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            var eyeballModel = Content.Load<Model>("eyeball");
            const int n = 17;
            const int f = 2;
            _eyeballs = Enumerable.Range(0, n).SelectMany(c =>
                Enumerable.Range(0,n).Select(r => 
                    new Eyeball(
                        eyeballModel,
                        new Vector3(-f*n/2f + f * c, -f*n/2f + f * r, -6),
                        (r*c)/20f))).ToArray();

            _pixelTexture = new Texture2D(GraphicsDevice,1,1);
            _pixelTexture.SetData(new []{Color.White });
            _font = Content.Load<SpriteFont>("Consolas");
            _octohedron = new Octohedron(Content.Load<Model>("octohedron"));
            _camera = new Camera(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)
            {
                Position = new Vector3(0, 0, 6)
            };
        }

        protected override void Initialize()
        {

            GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
            _keyboard.OnPress(Keys.Enter, () => _camera.ToggleProjectionType());
            _mouse.OnLeftClick((x,y) => _camera.ToggleProjectionType());
            _mouse.OnScroll(positive =>
            {
                _octohedron.TryChangeZOffset(positive);
            });
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _mouse.Update(gameTime);
            _keyboard.Update(Keyboard.GetState());
            foreach (var eb in _eyeballs)
            {
                eb.Update(gameTime);
            }

            _octohedron.RotationY += 0.01f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.Clear(Color.LightGreen);

            var viewMatrix = _camera.View;
            var projectionMatrix = _camera.Projection;

            var mouse = Mouse.GetState();
            var mouseWorldPosition = GraphicsDevice.Viewport.ScreenPositionToWorldSpace(
                mouse.Position,
                projectionMatrix,
                viewMatrix);

            _octohedron.TrackTo(mouseWorldPosition);
            _octohedron.Draw(viewMatrix, projectionMatrix);

            var eyeballLookAt = _octohedron.Position;

            foreach (var eb in _eyeballs)
            {
                eb.Draw(viewMatrix, projectionMatrix, eyeballLookAt);
            }

            _spriteBatch.Begin();
            _spriteBatch.FillRectangle(new Rectangle(0, GraphicsDevice.Viewport.Height - 50, GraphicsDevice.Viewport.Width, 50), Color.White * 0.75f );

            _spriteBatch.DrawString(_font, string.Format("Camera Projection: {0} (Click to change)", _camera.ProjectionType),
                new Vector2(10,GraphicsDevice.Viewport.Height-40), Color.Black);

            _spriteBatch.DrawString(_font, string.Format("Octohedron Z-offset: {0} (Scroll to change)", _octohedron.PositionZOffset),
                new Vector2(10,GraphicsDevice.Viewport.Height-20), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}