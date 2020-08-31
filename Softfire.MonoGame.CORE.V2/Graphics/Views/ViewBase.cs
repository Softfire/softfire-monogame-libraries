using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.CORE.V2.Graphics.Views
{
    /// <summary>
    /// The base view class.
    /// </summary>
    public abstract class ViewBase
    {
        /// <summary>
        /// The <see cref="GraphicsDevice"/> in use.
        /// </summary>
        protected GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// The view's viewport.
        /// </summary>
        public abstract Viewport Viewport { get; }

        /// <summary>
        /// The view's world width.
        /// </summary>
        public abstract int WorldWidth { get; }

        /// <summary>
        /// The view's world height.
        /// </summary>
        public abstract int WorldHeight { get; }

        /// <summary>
        /// The view's width.
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// The view's height.
        /// </summary>
        public abstract int Height { get; }
        
        /// <summary>
        /// The view's bounds.
        /// </summary>
        public RectangleF Bounds => new RectangleF(Viewport.X, Viewport.Y, Width, Height);

        /// <summary>
        /// The view's origin.
        /// </summary>
        public Vector2 Origin => Bounds.Center;

        /// <summary>
        /// The base view class.
        /// </summary>
        /// <param name="graphicsDevice">The view's graphics device.</param>
        protected ViewBase(GraphicsDevice graphicsDevice) => GraphicsDevice = graphicsDevice;

        /// <summary>
        /// Transforms the point to a screen point.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <returns>Returns the transformed point.</returns>
        public Point PointToScreen(Point point) => PointToScreen(point.X, point.Y);

        /// <summary>
        /// Transforms the point to a screen point.
        /// </summary>
        /// <param name="x">The point's x coordinate to transform.</param>
        /// <param name="y">The point's y coordinate to transform.</param>
        /// <returns>Returns the transformed point.</returns>
        public Point PointToScreen(int x, int y) => Vector2.Transform(new Vector2(x, y), Matrix.Invert(GetScaleMatrix())).ToPoint();

        /// <summary>
        /// Gets the view's scale matrix.
        /// </summary>
        /// <returns>Returns a scale <see cref="Matrix"/>.</returns>
        public abstract Matrix GetScaleMatrix();
    }
}