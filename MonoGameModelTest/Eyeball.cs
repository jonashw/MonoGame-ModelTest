using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameModelTest
{
    public class Eyeball
    {
        public Vector3 Position;
        private readonly Model _model;
        private readonly Tween _scaleTween;
        private float _scale = 1;
        private readonly EasyTimer _scaleTweenDelay;

        public Eyeball(Model model, Vector3 position, float scaleTweenDelayInSeconds)
        {
            _model = model;
            Position = position;
            _scaleTween = new Tween(Easing.CubicInOut, _scale, value => _scale = value, -0.25f, 3);
            _scaleTweenDelay = new EasyTimer(TimeSpan.FromSeconds(scaleTweenDelayInSeconds));
        }

        public void Update(GameTime gameTime)
        {
            if (!_scaleTweenDelay.IsFinished(gameTime))
            {
                return;
            }
            _scaleTween.Update(gameTime);
            if (_scaleTween.Finished)
            {
                _scaleTween.Reset(_scale);
                _scaleTween.ValueChange = -_scaleTween.ValueChange;
            }
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix, Vector3 lookAtPosition)
        {
            foreach (var mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World =
                        Matrix.CreateScale(_scale)
                        *Matrix.CreateWorld(Position, Position - lookAtPosition, Vector3.Up);
                        //Matrix.CreateTranslation(Position);
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.AmbientLightColor = Color.White.ToVector3();
                    effect.Alpha = 1;
                }
                mesh.Draw();
            }
        }
    }
}