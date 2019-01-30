using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.CORE.Graphics.Views
{
    /// <summary>
    /// The default view class.
    /// </summary>
    public class ViewDefault : ViewBase
    {
        /// <summary>
        /// The view's viewport.
        /// </summary>
        public override Viewport Viewport => GraphicsDevice.Viewport;

        /// <summary>
        /// The view's world width.
        /// </summary>
        public override int WorldWidth => GraphicsDevice.Viewport.Width;

        /// <summary>
        /// The view's world height.
        /// </summary>
        public override int WorldHeight => GraphicsDevice.Viewport.Height;

        /// <summary>
        /// The view's view width.
        /// </summary>
        public override int Width => GraphicsDevice.Viewport.Width;

        /// <summary>
        /// The view's view height.
        /// </summary>
        public override int Height => GraphicsDevice.Viewport.Height;

        /// <summary>
        /// A default view.
        /// </summary>
        /// <param name="graphicsDevice">The view's graphics device.</param>
        public ViewDefault(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
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