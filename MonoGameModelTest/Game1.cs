using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameModelTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Eyeball[] _eyeballs;
        private Model _octohedron;
        private SpriteFont _font;
        private Texture2D _pixelTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            var eyeballModel = Content.Load<Model>("eyeball");
            const int n = 12;
            const int f = 2;
            _eyeballs = Enumerable.Range(0, n).SelectMany(c =>
                Enumerable.Range(0,n).Select(r => 
                    new Eyeball(eyeballModel, new Vector3(-f*n/2f + f * c, -f*n/2f + f * r, 0)))).ToArray();

            _pixelTexture = new Texture2D(GraphicsDevice,1,1);
            _pixelTexture.SetData(new []{Color.White });
            _font = Content.Load<SpriteFont>("Consolas");
            _octohedron = Content.Load<Model>("octohedron");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            base.Update(gameTime);
        }

        private Vector3 mouseInObjectSpace(MouseState mouse, Matrix projectionMatrix, Matrix viewMatrix)
        {
            var nearScreenPoint = new Vector3(mouse.X, mouse.Y, 0);
            var farScreenPoint = new Vector3(mouse.X, mouse.Y, 1);
            var nearWorldPoint = GraphicsDevice.Viewport.Unproject(nearScreenPoint, projectionMatrix, viewMatrix, Matrix.Identity);
            var farWorldPoint = GraphicsDevice.Viewport.Unproject(farScreenPoint, projectionMatrix, viewMatrix, Matrix.Identity);
            return nearWorldPoint * new Vector3(100,100,0) + new Vector3(0,0,-10);
            var direction = farWorldPoint - nearWorldPoint;
            var zFactor = -nearWorldPoint.Y/direction.Y;
            return nearWorldPoint + direction*zFactor;
        }

        private Vector3 _eyeballPosition = new Vector3();
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);
            var mouse = Mouse.GetState();

            var viewMatrix = Matrix.CreateLookAt(new Vector3(0, 00, 6), Vector3.Zero, Vector3.Up);
            var projectionMatrix = new
            {
                perspective = Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45.0f),
                    1.6f, 0.1f, 10000.0f),
                orthographic = Matrix.CreateOrthographic(
                    GraphicsDevice.Viewport.Width,
                    GraphicsDevice.Viewport.Height,
                    0, 1000)
            };

            var mouseObjPosition = mouseInObjectSpace(Mouse.GetState(), projectionMatrix.perspective, viewMatrix);

            foreach (var eb in _eyeballs)
            {
                eb.Draw(viewMatrix, projectionMatrix.perspective, mouseObjPosition);
            }


            /*
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, string.Format("Mouse Position (on screen) = {{X={0}, Y={1}}}", mouse.X, mouse.Y),
                new Vector2(10,10), Color.Black);
            _spriteBatch.DrawString(_font, string.Format("Mouse Position (in object space)= {{X={0}, Y={1}, Z={2}}}", mouseObj.X, mouseObj.Y, mouseObj.Z),
                new Vector2(10,30), Color.Black);
            _spriteBatch.End();
            */

            base.Draw(gameTime);
        }
    }
}