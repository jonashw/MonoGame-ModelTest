using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameModelTest
{
    public static class ViewportExtensions
    {
        /*  This code was adapted from 
         *  http://www.gamedev.net/topic/503488-xna-making-the-mouse-x-and-y-coordinate-into-world-space-coordinates/ 
         */
        public static Vector3 ScreenPositionToWorldSpace(this Viewport viewport, Point point, Matrix projectionMatrix, Matrix viewMatrix)
        {
            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 10 is as far away as possible
            var nearSource = new Vector3(point.X, point.Y, 1f);
            var farSource = new Vector3(point.X, point.Y, 0f);

            // find the two screen space positions in world space
            var nearPoint = viewport.Unproject(nearSource, projectionMatrix, viewMatrix, Matrix.Identity);
            var farPoint = viewport.Unproject(farSource, projectionMatrix, viewMatrix, Matrix.Identity);

            // compute normalized direction vector from nearPoint to farPoint
            var direction = farPoint - nearPoint;
            direction.Normalize();

            // create a ray using nearPoint as the source
            var r = new Ray(nearPoint, direction);

            // calculate the ray-plane intersection point
            var n = new Vector3(0f, 0f, 1f);
            var p = new Plane(n, 0f);

            // calculate distance of intersection point from r.origin
            var denominator = Vector3.Dot(p.Normal, r.Direction);
            var numerator = Vector3.Dot(p.Normal, r.Position) + p.D;
            var t = -(numerator / denominator);

            // calculate the picked position on the y = 0 plane
            return nearPoint + direction * t;
        }
    }
}