
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Viewport = Microsoft.Xna.Framework.Graphics.Viewport;

namespace mike_and_conquer_monogame.main
{
    public class Camera2D
    {
        public float Zoom { get; set; }
        public Vector2 Location { get; set; }
        public float Rotation { get; set; }

        private Rectangle Bounds { get; set; }

        public Matrix TransformMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3((int)-Location.X, (int)-Location.Y, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Zoom, Zoom, 1.0f) *
                    // Added rounding to int in the second CreateTranslation to solve an issue with
                    // the first column of the texture getting doubled and the last column dropped, in cases
                    // when the width had a decimal component (e.g. 809.5 vs 809)
                    // Also decided to just go ahead and add same cast to (int) in the first CreateTranslation call too
                    Matrix.CreateTranslation(new Vector3((int)(Bounds.Width * 0.5f), (int)(Bounds.Height * 0.5f), 0));
            }
        }
        
        public Camera2D(Viewport viewport)
        {
            Bounds = viewport.Bounds;
        }
    }
}
