using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.IO.V2;

namespace Softfire.MonoGame.UI.V2.Items
{
    /// <summary>
    /// A text-box.
    /// </summary>
    public class UITextBox : UIBase
    {
        /// <summary>
        /// The window's camera.
        /// </summary>
        public IOCamera2D Camera { get; }

        /// <summary>
        /// The <see cref="Microsoft.Xna.Framework.Graphics.RasterizerState"/> for drawing contents.
        /// </summary>
        private RasterizerState RasterizerState { get; }

        /// <summary>
        /// The text-box's text.
        /// </summary>
        public UIText Text { get; private set; }

        /// <summary>
        /// A UI text-box for input.
        /// </summary>
        /// <param name="parent">The parent object. Intaken as a <see cref="UIBase"/>.</param>
        /// <param name="font">The text-box's font. Intaken as a <see cref="SpriteFont"/>.</param>
        /// <param name="id">The text-box's id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The text-box's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The text-box's position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="viewWidth">The text-box's view width. Intaken as a <see cref="float"/>.</param>
        /// <param name="viewHeight">The text-box's view height. Intaken as a <see cref="float"/>.</param>
        /// <param name="worldWidth">The text-box's world view width. Intaken as a <see cref="float"/>.</param>
        /// <param name="worldHeight">The text-box's world view height. Intaken as a <see cref="float"/>.</param>
        /// <param name="isVisible">The text-box's visibility. Intaken as a <see cref="bool"/>.</param>
        public UITextBox(UIBase parent, SpriteFont font, int id, string name, Vector2 position, int viewWidth, int viewHeight, int worldWidth, int worldHeight, bool isVisible = true) : base(parent, id, name, position, viewWidth, viewHeight, isVisible)
        {
            Font = font;
            Camera = new IOCamera2D(GraphicsDevice, viewWidth, viewHeight, worldWidth, worldHeight);

            RasterizerState = new RasterizerState
            {
                MultiSampleAntiAlias = false,
                ScissorTestEnable = true
            };
        }

        /// <summary>
        /// The text-box's content loader.
        /// </summary>
        public override void LoadContent(ContentManager content = null)
        {
            Text = new UIText(this, 0, "Text", Font, string.Empty, Vector2.Zero);
            Text.LoadContent();
            base.LoadContent();
        }

        /// <summary>
        /// The text-box's update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame <see cref="GameTime"/>.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Text?.Update(gameTime);
        }

        /// <summary>
        /// The text-box's contents draw method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform)
        {
            // Draw base.
            base.Draw(spriteBatch, transform);

            // Save the original view for restoration later.
            var originalScissor = spriteBatch.GraphicsDevice.ScissorRectangle;

            // Apply the window's view.
            spriteBatch.GraphicsDevice.ScissorRectangle = (Rectangle)Rectangle;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                              rasterizerState: RasterizerState, transformMatrix: Camera.GetViewMatrix());

            // Draw the window's elements.
            foreach (var component in Children)
            {
                component?.Draw(spriteBatch, transform);
            }

            spriteBatch.End();

            // Restore the original view to the graphics device.
            spriteBatch.GraphicsDevice.ScissorRectangle = originalScissor;
        }
    }
}