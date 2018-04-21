using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    public class UIBorder : UIBase
    {
        /// <summary>
        /// Border Text.
        /// </summary>
        public UIText Text { get; set; }

        /// <summary>
        /// UI Border Constructor.
        /// </summary>
        /// <param name="position">Intakes the UI's position as a Vector2.</param>
        /// <param name="width">Intakes the UI's width as an int.</param>
        /// <param name="height">Intakes the UI's height as an int.</param>
        /// <param name="orderNumber">Intakes an int that will be used to define the update/draw order. Update/Draw order is from lowest to highest.</param>
        /// <param name="color">Intakes the UI's color as Color.</param>
        /// <param name="textureFilePath">Intakes a texture's file path as a string.</param>
        /// <param name="isVisible">Indicates whether the UIBase is visible. Intaken as a bool.</param>
        public UIBorder(Vector2 position, int width, int height, int orderNumber,
                                                                 Color? color = null,
                                                                 string textureFilePath = null,
                                                                 bool isVisible = false) : base(position, width, height, orderNumber, color, textureFilePath, isVisible)
        {
            
        }

        /// <summary>
        /// UIBorder Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            if (Text != null)
            {
                Text.IsVisible = IsVisible;
                Text.Transparency = Transparency;
                Text.ParentPosition = ParentPosition + Position;

                await Text.Update(gameTime);
            }
            
            await base.Update(gameTime);
        }

        /// <summary>
        /// UIBorder Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsVisible)
            {
                Text?.Draw(spriteBatch);
            }
        }
    }
}