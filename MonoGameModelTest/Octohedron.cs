using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameModelTest
{
    public class Octohedron
    {
        private readonly Model _model;

        public Octohedron(Model model)
        {
            _model = model;
        }

        public float RotationY;

        public void TryChangeZOffset(bool positive)
        {
            _positionZOffset = MathHelper.Clamp(
                positive ? (_positionZOffset + 1) : (_positionZOffset - 1),
                -4f, 4f);
        }

        public float PositionZOffset
        {
            get { return _positionZOffset; }
        }

        private float _positionZOffset;
        private Vector3 _basePosition;
        public Vector3 Position
        {
            get { return _basePosition + new Vector3(0, 0, _positionZOffset); }
        }
        public void TrackTo(Vector3 mouseWorldPosition)
        {
            _basePosition = mouseWorldPosition;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (var mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World =
                        Matrix.CreateRotationY(RotationY)
                        *Matrix.CreateTranslation(Position);
                    effect.View = view;
                    effect.Projection = projection;
                    effect.PreferPerPixelLighting = true;
                    effect.AmbientLightColor = Color.White.ToVector3();
                    effect.Alpha = 1;
                }
                mesh.Draw();
            }
        }
    }
}