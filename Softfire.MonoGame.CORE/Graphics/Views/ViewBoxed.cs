using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.CORE.Graphics.Views
{
    /// <summary>
    /// The available display boxing modes.
    /// </summary>
    public enum BoxingMode
    {
        /// <summary>
        /// Does not enforce either boxing mode.
        /// </summary>
        None,
        /// <summary>
        /// Displays horizontal bars at the top and bottom of the view.
        /// </summary>
        Letter,
        /// <summary>
        /// Displays vertical bars on the left and right sides of the view.
        /// </summary>
        Pillar
    }

    /// <summary>
    /// An view used for displaying different aspect ratios.
    /// </summary>
    public class ViewBoxed : ViewScaled
    {
        /// <summary>
        /// The client game window.
        /// </summary>
        protected readonly GameWindow Window;

        /// <summary>
        /// The horizontal aspect bar's height.
        /// </summary>
        public int HorizontalAspectBarHeight { get; }

        /// <summary>
        /// The vertical aspect bar's width.
        /// </summary>
        public int VerticalAspectBarWidth { get; }

        /// <summary>
        /// The current boxing mode for the view.
        /// </summary>
        public BoxingMode BoxingMode { get; private set; }

        /// <summary>
        /// An view for displaying content using an aspect ratio.
        /// </summary>
        /// <param name="window">The game client window. Intaken as a <see cref="GameWindow"/>.</param>
        /// <param name="graphicsDevice">The current graphics device in use. Intaken as a <see cref="GraphicsDevice"/>.</param>
        /// <param name="worldWidth">The world width of the view. Intaken as an <see cref="int"/>.</param>
        /// <param name="worldHeight">The world height of the view. Intaken as an <see cref="int"/>.</param>
        /// <param name="horizontalBarHeight">The horizontal aspect bar's height. Intaken as an <see cref="int"/>.</param>
        /// <param name="verticalBarWidth">The vertical aspect bar's width. Intaken as an <see cref="int"/>.</param>
        public ViewBoxed(GameWindow window, GraphicsDevice graphicsDevice, int worldWidth, int worldHeight,
                            int horizontalBarHeight = 0, int verticalBarWidth = 0) : base(graphicsDevice, worldWidth, worldHeight)
        {
            Window = window;
            Window.ClientSizeChanged += OnClientSizeChanged;
            HorizontalAspectBarHeight = horizontalBarHeight;
            VerticalAspectBarWidth = verticalBarWidth;
        }

        /// <summary>
        /// An event triggered when the <see cref="GameWindow"/> is resized.
        /// </summary>
        /// <param name="sender">The <see cref="ViewBoxed"/> object.</param>
        /// <param name="eventArgs">The passed event information.</param>
        private void OnClientSizeChanged(object sender, EventArgs eventArgs)
        {
            // Gets the current viewport.
            var viewport = GraphicsDevice.Viewport;

            // Calculates the scale.
            var worldScaleX = (float) viewport.Width / WorldWidth;
            var worldScaleY = (float) viewport.Height / WorldHeight;

            // Calculates the scaling limit.
            var safeScaleX = (float) viewport.Width / (WorldWidth - HorizontalAspectBarHeight);
            var safeScaleY = (float) viewport.Height / (WorldHeight - VerticalAspectBarWidth);

            // Find min/max values for the world scale to fit in the view.
            var worldScale = MathHelper.Max(worldScaleX, worldScaleY);
            var safeScale = MathHelper.Min(safeScaleX, safeScaleY);
            var scale = MathHelper.Min(worldScale, safeScale);

            // Calculates the view's width and height.
            var width = (int) (scale * WorldWidth + 0.5f);
            var height = (int) (scale * WorldHeight + 0.5f);

            // Finds the view's coordinates.
            var x = viewport.Width / 2 - width / 2;
            var y = viewport.Height / 2 - height / 2;

            // If the world height is greater than or equal to the viewport height and the world width is less than the viewport width then enable Pillar mode.
            // If the world width is greater than or equal to the viewport height and the world height is less than the viewport height then enable Letter mode.
            // Otherwise disable boxing mode.
            BoxingMode = (height >= viewport.Height) && (width < viewport.Width) ? BoxingMode.Pillar :
                         (width >= viewport.Height) && (height < viewport.Height) ? BoxingMode.Letter : BoxingMode.None;

            // Apply the view to the graphics device.
            GraphicsDevice.Viewport = new Viewport(x, y, width, height);
        }
    }
}