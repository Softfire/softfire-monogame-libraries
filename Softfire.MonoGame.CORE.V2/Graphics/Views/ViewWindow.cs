using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.CORE.V2.Graphics.Views
{
    /// <summary>
    /// An view for in-game windows.
    /// </summary>
    public class ViewWindow : ViewBase
    {
        /// <summary>
        /// The view's viewport.
        /// </summary>
        public override Viewport Viewport => new Viewport(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, Width, Height);

        /// <summary>
        /// The view's world width.
        /// </summary>
        public override int WorldWidth { get; }

        /// <summary>
        /// The view's world height.
        /// </summary>
        public override int WorldHeight { get; }

        /// <summary>
        /// The view's view width.
        /// </summary>
        public override int Width { get; }

        /// <summary>
        /// The view's view height.
        /// </summary>
        public override int Height { get; }

        /// <summary>
        /// An view for in-game windows.
        /// </summary>
        /// <param name="graphicsDevice">The current graphics device in use. Intaken as a <see cref="GraphicsDevice"/>.</param>
        /// <param name="viewWidth">the view's width. Intaken as an <see cref="int"/>.</param>
        /// <param name="viewHeight">the view's height. Intaken as an <see cref="int"/>.</param>
        /// <param name="worldWidth">the view's world width. Intaken as an <see cref="int"/>.</param>
        /// <param name="worldHeight">the view's world height. Intaken as an <see cref="int"/>.</param>
        public ViewWindow(GraphicsDevice graphicsDevice, int viewWidth, int viewHeight, int worldWidth, int worldHeight) : base(graphicsDevice)
        {
            Width = viewWidth;
            Height = viewHeight;
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
        }

        /// <summary>
        /// Returns the scaled matrix of the view.
        /// </summary>
        /// <returns>Returns the view's scaled matrix.</returns>
        public override Matrix GetScaleMatrix()
        {
            return Matrix.Identity;
        }
    }
}
