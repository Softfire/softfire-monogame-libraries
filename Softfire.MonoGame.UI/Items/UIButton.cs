using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.IO;

namespace Softfire.MonoGame.UI.Items
{
    public class UIButton : UIBase
    {
        /// <summary>
        /// Is Clickable?
        /// Enable/Disable the activation on the AssignedAction. Defualt is true.
        /// </summary>
        public bool IsClickable { get; set; } = true;
        
        /// <summary>
        /// UIButton Hover Delay.
        /// Default delay is 1.0 seconds.
        /// </summary>
        public double HoverDelay { get; set; } = 1.0D;

        /// <summary>
        /// On Hover.
        /// </summary>
        public bool OnHover { get; set; }
        
        /// <summary>
        /// UIButton Text.
        /// </summary>
        private UIText Text { get; set; }

        /// <summary>
        /// Activate.
        /// Activates assigned action.
        /// </summary>
        public bool Activate { get; set; }

        /// <summary>
        /// Assigned Action.
        /// Sets an action to be performed upon a left click of the mouse.
        /// </summary>
        private Action AssignedAction { get; set; }

        /// <summary>
        /// Assign Action.
        /// Assigns an action to the UIButton.
        /// Use "() => Method()" to assign an action.
        /// </summary>
        /// <param name="action">The method to process. OnPress and OnRelease control execution.</param>
        public void AssignAction(Action action)
        {
            AssignedAction = action;
        }

        /// <summary>
        /// UI Button.
        /// </summary>
        /// <param name="id">The button's id. Intaken as an int.</param>
        /// <param name="name">The button's name. Intken as a string.</param>
        /// <param name="position">Intakes the Button's position as a Vector2.</param>
        /// <param name="width">Intakes the Button's width as a float.</param>
        /// <param name="height">Intakes the Button's height as a float.</param>
        /// <param name="orderNumber">Intakes an int that will be used to define the update/draw order. Update/Draw order is from lowest to highest.</param>
        public UIButton(int id, string name, Vector2 position, int width, int height, int orderNumber) : base(id, name, position, width, height, orderNumber)
        {

        }

        /// <summary>
        /// Add Text.
        /// Adds text to the button.
        /// </summary>
        /// <param name="font">The font to use for the button's text. Intaken as a SpriteFont.</param>
        /// <param name="text">The button's text. Intaken as a string.</param>
        public void AddText(SpriteFont font, string text)
        {
            Text = new UIText(0, "Text", font, text, 1);
            Text.LoadContent();
        }

        /// <summary>
        /// Get Text.
        /// Retrieves the button's text.
        /// </summary>
        /// <returns></returns>
        public UIText GetText()
        {
            UIText result = null;

            if (Text != null)
            {
                result = Text;
            }

            return result;
        }

        /// <summary>
        /// UIButton Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            await base.Update(gameTime);

            if (IsClickable)
            {
                if (Activate)
                {
                    AssignedAction();
                }
            }

            if (Text != null)
            {
                Text.ParentPosition = ParentPosition + Position;

                switch (Text.VerticalAlignment)
                {
                    case UIText.VerticalAlignments.Upper:
                        Text.Position = new Vector2(0, -Height / 2f);
                        break;
                    case UIText.VerticalAlignments.Center:
                        Text.Position = Vector2.Zero;
                        break;
                    case UIText.VerticalAlignments.Lower:
                        Text.Position = new Vector2(0, Height / 2f);
                        break;
                }

                switch (Text.HorizontalAlignment)
                {
                    case UIText.HorizontalAlignments.Left:
                        Text.Position = new Vector2(-Width / 2f, 0);
                        break;
                    case UIText.HorizontalAlignments.Center:
                        Text.Position = Vector2.Zero;
                        break;
                    case UIText.HorizontalAlignments.Right:
                        Text.Position = new Vector2(Width / 2f, 0);
                        break;
                }

                await Text.Update(gameTime);
            }
        }


        /// <summary>
        /// UIButton Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                base.Draw(spriteBatch);

                Text?.Draw(spriteBatch);
            }
        }
    }
}