using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.UI.Items;

namespace Softfire.MonoGame.UI.Menu
{
    public class UIMenuColumn : UIBase
    {
        /// <summary>
        /// UI Menu Column Parent Menu.
        /// </summary>
        internal UIMenu ParentMenu { get; }

        /// <summary>
        /// UI Menu Column Buttons.
        /// </summary>
        private List<UIButton> Buttons { get; } = new List<UIButton>();

        /// <summary>
        /// UI Menu Column.
        /// </summary>
        /// <param name="parentMenu">The parent menu containing the column. Intaken as a UIMenu.</param>
        /// <param name="id">The column's id. Intaken as an int.</param>
        /// <param name="name">The column's name.</param>
        /// <param name="width">The column's width. Intaken as an int.</param>
        /// <param name="height">The column's height. Intaken as an int.</param>
        /// <param name="orderNumber">The column order number. Used in sorting. Intaken as an int.</param>
        public UIMenuColumn(UIMenu parentMenu, int id, string name, int width, int height, int orderNumber) : base(id, name, Vector2.Zero, width, height, orderNumber)
        {
            ParentMenu = parentMenu;
        }

        #region Buttons

        /// <summary>
        /// Add Button.
        /// Adds a new button on to the menu.
        /// </summary>
        /// <param name="name">The button's name. Intaken as a string.</param>
        /// <returns>Returns the button id of the newly added button as an int.</returns>
        public int AddButton(string name)
        {
            var nextButtonId = GetNextValidItemId(Buttons);

            var button = new UIButton(nextButtonId, name, new Vector2(ParentMenu.ParentGroup.ParentManager.GetViewportDimenions().Width / 2f, ParentMenu.ParentGroup.ParentManager.GetViewportDimenions().Height / 2f), Width, Height / 4, nextButtonId);
            button.LoadContent();

            Buttons.Add(button);

            return nextButtonId;
        }

        /// <summary>
        /// Get Button.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a UIButton with the requested id.</returns>
        public UIButton GetButton(int buttonId)
        {
            return GetItemById(Buttons, buttonId);
        }

        /// <summary>
        /// Get Button.
        /// </summary>
        /// <param name="buttonName">The name of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a UIButton with the requested name.</returns>
        public UIButton GetButton(string buttonName)
        {
            return GetItemByName(Buttons, buttonName);
        }

        /// <summary>
        /// Remove Button.
        /// </summary>
        /// <param name="buttonName">The id of the button to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button was removed.</returns>
        public bool RemoveButton(int buttonId)
        {
            return RemoveItemById(Buttons, buttonId);
        }

        /// <summary>
        /// Remove Button.
        /// </summary>
        /// <param name="buttonName">The name of the button to be removed. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the button was removed.</returns>
        public bool RemoveButton(string buttonName)
        {
            return RemoveItemByName(Buttons, buttonName);
        }

        #endregion

        /// <summary>
        /// UI Menu Column Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            ParentPosition = ParentMenu.ParentPosition + ParentMenu.Position;
            Transparencies["Background"] = ParentMenu.Transparencies["Background"];
            
            await base.Update(gameTime);
        }

        /// <summary>
        /// UIMenu Column Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                base.Draw(spriteBatch);

                foreach (var button in Buttons)
                {
                    button.Draw(spriteBatch);
                }
            }
        }
    }
}