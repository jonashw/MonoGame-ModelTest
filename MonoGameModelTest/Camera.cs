using System.IO;
using Microsoft.Xna.Framework;

namespace MonoGameModelTest
{
    public enum ProjectionType
    {
        Perspective, Orthographic
    }

    public class Camera
    {
        private readonly int _viewportWidth;
        private readonly int _viewportHeight;
        public Camera(int viewportWidth, int viewportHeight)
        {
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
            ProjectionType = ProjectionType.Orthographic;
        }

        public Vector3 Position;
        
        public ProjectionType ProjectionType;

        public void ToggleProjectionType()
        {
            switch (ProjectionType)
            {
                case ProjectionType.Perspective:
                    ProjectionType = ProjectionType.Orthographic;
                    return;
                case ProjectionType.Orthographic:
                    ProjectionType = ProjectionType.Perspective;
                    return;
                default:
                    throw new InvalidDataException("Unexpected ProjectionType: " + ProjectionType);
            }
        }
        public Matrix View
        {
            get { return Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up); }
        }

        public Matrix Projection
        {
            get
            {
                switch (ProjectionType)
                {
                    case ProjectionType.Perspective:
                        return Matrix.CreatePerspectiveFieldOfView(
                            MathHelper.ToRadians(45.0f),
                            1.6f, 0.1f, 10000.0f);
                    case ProjectionType.Orthographic:
                        return Matrix.CreateOrthographic(
                            _viewportWidth/40f,
                            _viewportHeight/40f,
                            -1000, 10000);
                    default:
                        throw new InvalidDataException("Unexpected ProjectionType: " + ProjectionType);
                }
            }
        }
    }
}