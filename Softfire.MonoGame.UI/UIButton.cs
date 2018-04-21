using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.IO;

namespace Softfire.MonoGame.UI
{
    public class UIButton : UIBase
    {
        /// <summary>
        /// Is Button Clickable?
        /// </summary>
        public bool IsClickable { get; set; }

        /// <summary>
        /// UIButton Hover Window.
        /// </summary>
        public UIWindow HoverWindow { get; set; }

        /// <summary>
        /// UIButton Hover Delay.
        /// Default delay is 1.0 seconds.
        /// </summary>
        public double HoverDelay { get; set; } = 1.0;

        /// <summary>
        /// Is Hovered?
        /// </summary>
        private bool IsHovered { get; set; }

        /// <summary>
        /// UIButton Text.
        /// </summary>
        public UIText Text { get; set; }

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
        /// <param name="action"></param>
        public void AssignAction(Action action)
        {
            AssignedAction = action;
        }

        /// <summary>
        /// UI Button.
        /// </summary>
        /// <param name="position">Intakes the Button's position as a Vector2.</param>
        /// <param name="width">Intakes the Button's width as a float.</param>
        /// <param name="height">Intakes the Button's height as a float.</param>
        /// <param name="orderNumber">Intakes an int that will be used to define the update/draw order. Update/Draw order is from lowest to highest.</param>
        /// <param name="color">Intakes the Button's color as Color.</param>
        /// <param name="borderThickness">Intakes the Button's border thickness as an int.</param>
        /// <param name="borderColor">Intakes the Button's border Color as a Color.</param>
        /// <param name="textureFilePath">Intakes a texture's file path as a string.</param>
        /// <param name="isVisible">Indicates whether the UIBase is visible. Intaken as a bool.</param>
        public UIButton(Vector2 position, int width, int height, int orderNumber, Color color, int borderThickness, Color borderColor,
                                                                                                                    string textureFilePath = null,
                                                                                                                    bool isVisible = false) : base(position, width, height, orderNumber, color, textureFilePath, isVisible)
        {
            HoverWindow = new UIWindow(new Vector2(), width, height, orderNumber, color, borderThickness: borderThickness, borderColor: borderColor);

            UpdateChildren(position, width, height);
        }

        /// <summary>
        /// Update UIButton Child Objects.
        /// </summary>
        /// <param name="parentPosition">Intakes the Parent's Position as a Vector2.</param>
        /// <param name="parentWidth">Intakes the Parent's Width as an int.</param>
        /// <param name="parentHeight">Intakes the Parent's Height as an int.</param>
        private void UpdateChildren(Vector2 parentPosition, int parentWidth, int parentHeight)
        {
            HoverWindow.Position = new Vector2(parentPosition.X, parentPosition.Y);
            HoverWindow.Width = parentWidth;
            HoverWindow.Height = parentHeight;
        }

        /// <summary>
        /// UIButton Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            UpdateChildren(Position, Width, Height);

            IsHovered = IOMouse.Hover(Rectangle, HoverDelay);

            if (IsClickable)
            {
                if (IOMouse.LeftClickDownInside(Rectangle))
                {
                    AssignedAction();
                }
            }

            await base.Update(gameTime);
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

                if (IsHovered)
                {
                    HoverWindow.Draw(spriteBatch);
                }
            }
        }
    }
}