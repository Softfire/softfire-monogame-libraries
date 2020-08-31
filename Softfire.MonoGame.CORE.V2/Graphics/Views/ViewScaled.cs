using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.CORE.V2.Graphics.Views
{
    /// <summary>
    /// A scaled view.
    /// </summary>
    public class ViewScaled : ViewBase
    {
        /// <summary>
        /// The view's viewport.
        /// </summary>
        public override Viewport Viewport => GraphicsDevice.Viewport;

        /// <summary>
        /// The view's world width.
        /// </summary>
        public override int WorldWidth { get; }

        /// <summary>
        /// The view's world height.
        /// </summary>
        public override int WorldHeight { get; }

        /// <summary>
        /// The view's width.
        /// </summary>
        public override int Width => GraphicsDevice.Viewport.Width;

        /// <summary>
        /// The view's height.
        /// </summary>
        public override int Height => GraphicsDevice.Viewport.Height;

        /// <summary>
        /// A scaled view.
        /// </summary>
        /// <param name="graphicsDevice">The view's graphics device.</param>
        /// <param name="worldWidth">The view's world width.</param>
        /// <param name="worldHeight">The view's world height.</param>
        public ViewScaled(GraphicsDevice graphicsDevice, int worldWidth, int worldHeight) : base(graphicsDevice)
        {
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
        }

        /// <summary>
        /// Returns the scaled matrix of the view.
        /// </summary>
        /// <returns>Returns the view's scaled matrix.</returns>
        public override Matrix GetScaleMatrix()
        {
            var scaleX = (float)Width / WorldWidth;
            var scaleY = (float)Height / WorldHeight;
            return Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }
    }
}