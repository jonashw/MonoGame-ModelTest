using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameModelTest
{
    public class Eyeball
    {
        public Vector3 Position;
        private readonly Model _model;

        public Eyeball(Model model, Vector3 position)
        {
            _model = model;
            Position = position;
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix, Vector3 lookAtPosition)
        {
            foreach (var mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateLookAt(Position, lookAtPosition, Vector3.Up);
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.PreferPerPixelLighting = true;
                    effect.AmbientLightColor = Color.White.ToVector3();
                    effect.Alpha = 1;
                }
                mesh.Draw();
            }
        }
    }
}