using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.CORE.Graphics.Views
{
    /// <summary>
    /// A game window view.
    /// </summary>
    public class ViewGameClient : ViewBase
    {
        /// <summary>
        /// The view's viewport.
        /// </summary>
        public override Viewport Viewport => GraphicsDevice.Viewport;

        /// <summary>
        /// The client game window.
        /// </summary>
        protected readonly GameWindow Window;

        /// <summary>
        /// The view's world width.
        /// </summary>
        public override int WorldWidth => Window.ClientBounds.Width;

        /// <summary>
        /// The view's world height.
        /// </summary>
        public override int WorldHeight => Window.ClientBounds.Height;

        /// <summary>
        /// The view's width.
        /// </summary>
        public override int Width => Window.ClientBounds.Width;

        /// <summary>
        /// The view's height.
        /// </summary>
        public override int Height => Window.ClientBounds.Height;

        /// <summary>
        /// An view for the game client window.
        /// </summary>
        /// <param name="window">The game client window. Intaken as a <see cref="GameWindow"/>.</param>
        /// <param name="graphicsDevice">The current graphics device in use. Intaken as a <see cref="GraphicsDevice"/>.</param>
        public ViewGameClient(GameWindow window, GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            Window = window;
            window.ClientSizeChanged += OnClientSizeChanged;
        }

        /// <summary>
        /// Returns the scaled matrix of the view.
        /// </summary>
        /// <returns>Returns the view's scaled matrix.</returns>
        public override Matrix GetScaleMatrix()
        {
            return Matrix.Identity;
        }

        /// <summary>
        /// On a client window update the graphics device viewport is updated.
        /// </summary>
        private void OnClientSizeChanged(object sender, EventArgs eventArgs)
        {
            var x = Window.ClientBounds.Width;
            var y = Window.ClientBounds.Height;

            GraphicsDevice.Viewport = new Viewport(0, 0, x, y);
        }
    }
}