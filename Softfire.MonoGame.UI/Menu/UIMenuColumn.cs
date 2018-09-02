using System.Collections.Generic;
using System.Linq;
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
        internal List<UIButton> Buttons { get; } = new List<UIButton>();

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
        /// Adds a button.
        /// </summary>
        /// <param name="buttonName">The button's name. Intaken as a string.</param>
        /// <returns>Returns the button id, if added, otherwise zero.</returns>
        /// <remarks>If a button already exists with the provided name then a zero is returned indicating failure to add the button.</remarks>
        public int AddButton(string buttonName)
        {
            var nextButtonId = 0;

            if (CheckForButton(buttonName) == false)
            {
                nextButtonId = GetNextValidItemId(Buttons);

                if (CheckForButton(nextButtonId) == false)
                {
                    var button = new UIButton(nextButtonId, buttonName, Position, Width, Height / 4, nextButtonId);
                    button.LoadContent();

                    Buttons.Add(button);
                }
            }

            return nextButtonId;
        }

        /// <summary>
        /// Checks for a button by id.
        /// </summary>
        /// <param name="buttonId">The id of the button to search. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the button is present.</returns>
        public bool CheckForButton(int buttonId)
        {
            return CheckItemById(Buttons, buttonId);
        }

        /// <summary>
        /// Checks for a button by name.
        /// </summary>
        /// <param name="buttonName">The name of the button to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the button is present.</returns>
        public bool CheckForButton(string buttonName)
        {
            return CheckItemByName(Buttons, buttonName);
        }

        /// <summary>
        /// Gets a button by id.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns the button with the specified id, if present, otherwise null.</returns>
        public UIButton GetButton(int buttonId)
        {
            return CheckForButton(buttonId) ? GetItemById(Buttons, buttonId) : default(UIButton);
        }

        /// <summary>
        /// Gets a button by name.
        /// </summary>
        /// <param name="buttonName">The name of the button to retrieve. Intaken as a string.</param>
        /// <returns>Returns the button with the specified name, if present, otherwise null.</returns>
        public UIButton GetButton(string buttonName)
        {
            return CheckForButton(buttonName) ? GetItemByName(Buttons, buttonName) : default(UIButton);
        }

        /// <summary>
        /// Removes a button by id.
        /// </summary>
        /// <param name="buttonId">The id of the button to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button was removed.</returns>
        public bool RemoveButton(int buttonId)
        {
            return RemoveItemById(Buttons, buttonId);
        }

        /// <summary>
        /// Removes a button by name.
        /// </summary>
        /// <param name="buttonName">The name of the button to be removed. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the button was removed.</returns>
        public bool RemoveButton(string buttonName)
        {
            return RemoveItemByName(Buttons, buttonName);
        }

        /// <summary>
        /// Increases a button's order number by id.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was increased.</returns>
        public bool IncreaseButtonOrderNumber(int buttonId)
        {
            return IncreaseItemOrderNumber(Buttons, buttonId);
        }

        /// <summary>
        /// Increases a button's order number bu name.
        /// </summary>
        /// <param name="buttonName">The name of the button to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was increased.</returns>
        public bool IncreaseButtonOrderNumber(string buttonName)
        {
            return IncreaseItemOrderNumber(Buttons, buttonName);
        }

        /// <summary>
        /// Decreases a button's order number by id.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was decreased.</returns>
        public bool DecreaseButtonOrderNumber(int buttonId)
        {
            return DecreaseItemOrderNumber(Buttons, buttonId);
        }

        /// <summary>
        /// Decreases a button's order number by name.
        /// </summary>
        /// <param name="buttonName">The name of the button to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was decreased.</returns>
        public bool DecreaseButtonOrderNumber(string buttonName)
        {
            return DecreaseItemOrderNumber(Buttons, buttonName);
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

            foreach (var button in Buttons.OrderBy(button => button.OrderNumber))
            {
                button.ParentPosition = ParentPosition + Position;
                await button.Update(gameTime);
            }
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

                foreach (var button in Buttons.OrderBy(button => button.OrderNumber))
                {
                    button.Draw(spriteBatch);
                }
            }
        }
    }
}