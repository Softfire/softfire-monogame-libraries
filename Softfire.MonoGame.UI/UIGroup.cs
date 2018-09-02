using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Softfire.MonoGame.IO;
using Softfire.MonoGame.UI.Items;
using Softfire.MonoGame.UI.Menu;

namespace Softfire.MonoGame.UI
{
    public class UIGroup : IUIIdentifier
    {
        /// <summary>
        /// Parent UI Manager.
        /// </summary>
        internal UIManager ParentManager { get; }

        /// <summary>
        /// UI Group Id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// UI Group Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Is the group active?
        /// </summary>
        public bool IsActive { get; private set; } = true;

        /// <summary>
        /// UI Group Buttons.
        /// </summary>
        internal List<UIButton> Buttons { get; } = new List<UIButton>();

        /// <summary>
        /// UI Group Texts.
        /// </summary>
        internal List<UIText> Texts { get; } = new List<UIText>();

        /// <summary>
        /// UI Group Menus.
        /// </summary>
        internal List<UIMenu> Menus { get; } = new List<UIMenu>();

        /// <summary>
        /// UI Group Windows.
        /// </summary>
        internal List<UIWindow> Windows { get; } = new List<UIWindow>();

        /// <summary>
        /// Internal Order Number.
        /// </summary>
        private int _orderNumber;

        /// <summary>
        /// Order Number.
        /// Group will be updated in order from smallest to largest.
        /// </summary>
        public int OrderNumber
        {
            get => _orderNumber;
            set => _orderNumber = value >= 1 ? value : 0;
        }

        /// <summary>
        /// Current Internal Button Identifier.
        /// </summary>
        private string CurrentButtonIdentifier { get; set; }

        /// <summary>
        /// Current Internal Text Identifier.
        /// </summary>
        private string CurrentTextIdentifier { get; set; }

        /// <summary>
        /// Current Internal Menu Identifier.
        /// </summary>
        private string CurrentMenuIdentifier { get; set; }

        /// <summary>
        /// Current Internal Window Identifier.
        /// </summary>
        private string CurrentWindowIdentifier { get; set; }

        /// <summary>
        /// UI Group Constructor.
        /// </summary>
        /// <param name="parentManager">The parent UI manager. Intaken as a UIManager.</param>
        /// <param name="id">The group's id. Intaken as an int.</param>
        /// <param name="name">The group's name. Intaken as a string.</param>
        /// <param name="orderNumber">Intakes the group's Update/Draw Order Number as an int.</param>
        public UIGroup(UIManager parentManager, int id, string name, int orderNumber)
        {
            ParentManager = parentManager;
            Id = id;
            Name = name;
            OrderNumber = orderNumber;
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
                nextButtonId = UIBase.GetNextValidItemId(Buttons);

                if (CheckForButton(nextButtonId) == false)
                {
                    var button = new UIButton(nextButtonId, buttonName, new Vector2(ParentManager.GetViewportDimenions().Width / 2f,
                                                                                    ParentManager.GetViewportDimenions().Height / 2f), 100, 50, nextButtonId);
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
            return UIBase.CheckItemById(Buttons, buttonId);
        }

        /// <summary>
        /// Checks for a button by name.
        /// </summary>
        /// <param name="buttonName">The name of the button to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the button is present.</returns>
        public bool CheckForButton(string buttonName)
        {
            return UIBase.CheckItemByName(Buttons, buttonName);
        }

        /// <summary>
        /// Gets a button by id.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns the button with the specified id, if present, otherwise null.</returns>
        public UIButton GetButton(int buttonId)
        {
            return CheckForButton(buttonId) ? UIBase.GetItemById(Buttons, buttonId) : default(UIButton);
        }

        /// <summary>
        /// Gets a button by name.
        /// </summary>
        /// <param name="buttonName">The name of the button to retrieve. Intaken as a string.</param>
        /// <returns>Returns the button with the specified name, if present, otherwise null.</returns>
        public UIButton GetButton(string buttonName)
        {
            return CheckForButton(buttonName) ? UIBase.GetItemByName(Buttons, buttonName) : default(UIButton);
        }

        /// <summary>
        /// Removes a button by id.
        /// </summary>
        /// <param name="buttonId">The id of the button to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button was removed.</returns>
        public bool RemoveButton(int buttonId)
        {
            return UIBase.RemoveItemById(Buttons, buttonId);
        }

        /// <summary>
        /// Removes a button by name.
        /// </summary>
        /// <param name="buttonName">The name of the button to be removed. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the button was removed.</returns>
        public bool RemoveButton(string buttonName)
        {
            return UIBase.RemoveItemByName(Buttons, buttonName);
        }

        /// <summary>
        /// Increases a button's order number by id.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was increased.</returns>
        public bool IncreaseButtonOrderNumber(int buttonId)
        {
            return UIBase.IncreaseItemOrderNumber(Buttons, buttonId);
        }

        /// <summary>
        /// Increases a button's order number bu name.
        /// </summary>
        /// <param name="buttonName">The name of the button to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was increased.</returns>
        public bool IncreaseButtonOrderNumber(string buttonName)
        {
            return UIBase.IncreaseItemOrderNumber(Buttons, buttonName);
        }

        /// <summary>
        /// Decreases a button's order number by id.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was decreased.</returns>
        public bool DecreaseButtonOrderNumber(int buttonId)
        {
            return UIBase.DecreaseItemOrderNumber(Buttons, buttonId);
        }

        /// <summary>
        /// Decreases a button's order number by name.
        /// </summary>
        /// <param name="buttonName">The name of the button to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was decreased.</returns>
        public bool DecreaseButtonOrderNumber(string buttonName)
        {
            return UIBase.DecreaseItemOrderNumber(Buttons, buttonName);
        }

        #endregion

        #region Texts

        /// <summary>
        /// Add Text.
        /// Adds a new text on to the group.
        /// </summary>
        /// <param name="textName">The text's name. Intaken as a string.</param>
        /// <param name="font">The text's font. Intaken as a SpriteFont.</param>
        /// <param name="text">The text's text. Intaken as a string.</param>
        /// <returns>Returns the text id, if added, otherwise zero.</returns>
        /// <remarks>If text already exists with the provided name then a zero is returned indicating failure to add the text.</remarks>
        public int AddText(string textName, SpriteFont font, string text)
        {
            var nextTextId = 0;

            if (CheckForText(textName) == false)
            {
                nextTextId = UIBase.GetNextValidItemId(Texts);

                if (CheckForText(nextTextId) == false)
                {
                    var newText = new UIText(nextTextId, textName, font, text, nextTextId, new Vector2(ParentManager.GetViewportDimenions().Width / 2f,
                                                                                                       ParentManager.GetViewportDimenions().Height / 2f));
                    newText.LoadContent();

                    Texts.Add(newText);
                }
            }

            return nextTextId;
        }

        /// <summary>
        /// Checks for text by id.
        /// </summary>
        /// <param name="textId">The id of the text to search. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the text is present.</returns>
        public bool CheckForText(int textId)
        {
            return UIBase.CheckItemById(Texts, textId);
        }

        /// <summary>
        /// Checks for text by name.
        /// </summary>
        /// <param name="textName">The name of the text to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the text is present.</returns>
        public bool CheckForText(string textName)
        {
            return UIBase.CheckItemByName(Texts, textName);
        }

        /// <summary>
        /// Gets text by id.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve.</param>
        /// <returns>Returns a text with the requested id, if present, otherwise null.</returns>
        public UIText GetText(int textId)
        {
            return CheckForText(textId) ? UIBase.GetItemById(Texts, textId) : default(UIText);
        }

        /// <summary>
        /// Gets text by name.
        /// </summary>
        /// <param name="textName">The name of the text to retrieve.</param>
        /// <returns>Returns a text with the requested name, if present, otherwise null.</returns>
        public UIText GetText(string textName)
        {
            return CheckForText(textName) ? UIBase.GetItemByName(Texts, textName) : default(UIText);
        }

        /// <summary>
        /// Removes text by id.
        /// </summary>
        /// <param name="textId">The id of the text to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text was removed.</returns>
        public bool RemoveText(int textId)
        {
            return UIBase.RemoveItemById(Texts, textId);
        }

        /// <summary>
        /// Removes text by name.
        /// </summary>
        /// <param name="textName">The name of the text to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text was removed.</returns>
        public bool RemoveText(string textName)
        {
            return UIBase.RemoveItemByName(Texts, textName);
        }

        /// <summary>
        /// Increases text order number by id.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was increased.</returns>
        public bool IncreaseTextOrderNumber(int textId)
        {
            return UIBase.IncreaseItemOrderNumber(Texts, textId);
        }

        /// <summary>
        /// Increases text order number by name.
        /// </summary>
        /// <param name="textName">The name of the text to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was increased.</returns>
        public bool IncreaseTextOrderNumber(string textName)
        {
            return UIBase.IncreaseItemOrderNumber(Texts, textName);
        }

        /// <summary>
        /// Decreases text order number by id.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was decreased.</returns>
        public bool DecreaseTextOrderNumber(int textId)
        {
            return UIBase.DecreaseItemOrderNumber(Texts, textId);
        }

        /// <summary>
        /// Decreases text order number by name.
        /// </summary>
        /// <param name="textName">The name of the text to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was decreased.</returns>
        public bool DecreaseTextOrderNumber(string textName)
        {
            return UIBase.DecreaseItemOrderNumber(Texts, textName);
        }

        #endregion

        #region Menus

        /// <summary>
        /// Add Menu.
        /// </summary>
        /// <param name="menuName">The menu's name. Intaken as a string.</param>
        /// <param name="width">The menu's width. Intaken as an int. Default is 100.</param>
        /// <param name="height">The menu's height. Intaken as an int. Default is 400.</param>
        /// <returns>Returns the menu id of the newly added menu as an int.</returns>
        public int AddMenu(string menuName, int width = 100, int height = 400)
        {
            var nextMenuId = 0;

            if (CheckForMenu(menuName) == false)
            {
                nextMenuId = UIBase.GetNextValidItemId(Menus);

                if (CheckForMenu(nextMenuId) == false)
                {
                    var newMenu = new UIMenu(this, nextMenuId, menuName, new Vector2(ParentManager.GetViewportDimenions().Width / 2f, ParentManager.GetViewportDimenions().Height / 2f), width, height, nextMenuId);
                    newMenu.LoadContent();

                    Menus.Add(newMenu);
                }
            }

            return nextMenuId;
        }

        /// <summary>
        /// Checks for menu by id.
        /// </summary>
        /// <param name="menuId">The id of the menu to search. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the menu is present.</returns>
        public bool CheckForMenu(int menuId)
        {
            return UIBase.CheckItemById(Menus, menuId);
        }

        /// <summary>
        /// Checks for menu by name.
        /// </summary>
        /// <param name="menuName">The name of the menu to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the menu is present.</returns>
        public bool CheckForMenu(string menuName)
        {
            return UIBase.CheckItemByName(Menus, menuName);
        }

        /// <summary>
        /// Gets a menu ny id.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a menu with the requested id, if present, otherwise null.</returns>
        public UIMenu GetMenu(int menuId)
        {
            return CheckForMenu(menuId) ? UIBase.GetItemById(Menus, menuId) : default(UIMenu);
        }

        /// <summary>
        /// Gets a menu by name.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a menu with the requested name, if present, otherwise null.</returns>
        public UIMenu GetMenu(string menuName)
        {
            return CheckForMenu(menuName) ? UIBase.GetItemByName(Menus, menuName) : default(UIMenu);
        }

        /// <summary>
        /// Removes a menu by id.
        /// </summary>
        /// <param name="menuId">The id of the menu to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu was removed.</returns>
        public bool RemoveMenu(int menuId)
        {
            return UIBase.RemoveItemById(Menus, menuId);
        }

        /// <summary>
        /// Removes a menu by name.
        /// </summary>
        /// <param name="menuName">The name of the menu to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu was removed.</returns>
        public bool RemoveMenu(string menuName)
        {
            return UIBase.RemoveItemByName(Menus, menuName);
        }

        /// <summary>
        /// Increases a menu's order number by id.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was increased.</returns>
        public bool IncreaseMenuOrderNumber(int menuId)
        {
            return UIBase.IncreaseItemOrderNumber(Menus, menuId);
        }

        /// <summary>
        /// Increases a menu's order number by name.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was increased.</returns>
        public bool IncreaseMenuOrderNumber(string menuName)
        {
            return UIBase.IncreaseItemOrderNumber(Menus, menuName);
        }

        /// <summary>
        /// Decreases a menu's order number by id.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was decreased.</returns>
        public bool DecreaseMenuOrderNumber(int menuId)
        {
            return UIBase.DecreaseItemOrderNumber(Menus, menuId);
        }

        /// <summary>
        /// Decreases a menu's order number by name.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was decreased.</returns>
        public bool DecreaseMenuOrderNumber(string menuName)
        {
            return UIBase.DecreaseItemOrderNumber(Menus, menuName);
        }

        #endregion

        #region Windows

        /// <summary>
        /// Add Windows.
        /// </summary>
        /// <param name="name">The window's name. Intaken as a string.</param>
        /// <param name="width">The window's width. Intaken as an int. Default is 400.</param>
        /// <param name="height">The window's height. Intaken as an int. Default is 500.</param>
        /// <returns>Returns the window id of the newly added window as an int.</returns>
        public int AddWindow(string name, int width = 200, int height = 200)
        {
            var nextWindowId = 0;

            if (CheckForWindow(nextWindowId) == false)
            {
                nextWindowId = UIBase.GetNextValidItemId(Windows);

                if (CheckForWindow(nextWindowId) == false)
                {
                    var newWindow = new UIWindow(this, nextWindowId, name, new Vector2(ParentManager.GetViewportDimenions().Width / 2f, ParentManager.GetViewportDimenions().Height / 2f), width, height, nextWindowId);
                    newWindow.LoadContent();

                    Windows.Add(newWindow);
                }
            }

            return nextWindowId;
        }

        /// <summary>
        /// Checks for window by id.
        /// </summary>
        /// <param name="windowId">The id of the window to search. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the window is present.</returns>
        public bool CheckForWindow(int windowId)
        {
            return UIBase.CheckItemById(Windows, windowId);
        }

        /// <summary>
        /// Checks for window by name.
        /// </summary>
        /// <param name="windowName">The name of the window to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the window is present.</returns>
        public bool CheckForWindow(string windowName)
        {
            return UIBase.CheckItemByName(Windows, windowName);
        }

        /// <summary>
        /// Get window by id.
        /// </summary>
        /// <param name="windowId">The id of the window to retrieve. Intaken as an int.</param>
        /// <returns>Returns a UIWindow with the requested id, if present, otherwise null.</returns>
        public UIWindow GetWindow(int windowId)
        {
            return CheckForWindow(windowId) ? UIBase.GetItemById(Windows, windowId) : default(UIWindow);
        }

        /// <summary>
        /// Get window by name.
        /// </summary>
        /// <param name="windowName">The name of the window to retrieve. Intaken as a string.</param>
        /// <returns>Returns a UIWindow with the requested name, if present, otherwise null.</returns>
        public UIWindow GetWindow(string windowName)
        {
            return CheckForWindow(windowName) ? UIBase.GetItemByName(Windows, windowName) : default(UIWindow);
        }

        /// <summary>
        /// Removes a window by id.
        /// </summary>
        /// <param name="windowId">The id of the window to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the window was removed.</returns>
        public bool RemoveWindow(int windowId)
        {
            return UIBase.RemoveItemById(Windows, windowId);
        }

        /// <summary>
        /// Removes a window by name.
        /// </summary>
        /// <param name="windowName">The name of the window to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the window was removed.</returns>
        public bool RemoveWindow(string windowName)
        {
            return UIBase.RemoveItemByName(Windows, windowName);
        }

        /// <summary>
        /// Increases window's order number by id.
        /// </summary>
        /// <param name="windowId">The id of the window to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the window's order number was increased.</returns>
        public bool IncreaseWindowOrderNumber(int windowId)
        {
            return UIBase.IncreaseItemOrderNumber(Windows, windowId);
        }

        /// <summary>
        /// Increases window's order number by name.
        /// </summary>
        /// <param name="windowName">The name of the window to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the window's order number was increased.</returns>
        public bool IncreaseWindowOrderNumber(string windowName)
        {
            return UIBase.IncreaseItemOrderNumber(Windows, windowName);
        }

        /// <summary>
        /// Decreases window's order number by id.
        /// </summary>
        /// <param name="windowId">The id of the window to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the window's order number was decreased.</returns>
        public bool DecreaseWindowOrderNumber(int windowId)
        {
            return UIBase.DecreaseItemOrderNumber(Windows, windowId);
        }

        /// <summary>
        /// Decreases window' order number by name.
        /// </summary>
        /// <param name="windowName">The name of the window to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the window's order number was decreased.</returns>
        public bool DecreaseWindowOrderNumber(string windowName)
        {
            return UIBase.DecreaseItemOrderNumber(Windows, windowName);
        }

        /// <summary>
        /// Checks to see if any Windows are in motion.
        /// </summary>
        /// <returns>Returns a boolean indicating if Windows are currently in motion.</returns>
        private bool AreWindowsMoving()
        {
            return Windows.Count(win => win.IsMoving) > 0;
        }

        #endregion

        /// <summary>
        /// Update Method.
        /// Updates all windows and their contents.
        /// Updates are in Ascending Order by OrderNumber.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public async Task Update(GameTime gameTime)
        {
            #region Buttons

            // Update Order is Ascending.
            // From lowest to highest.
            foreach (var button in Buttons.OrderBy(button => button.OrderNumber))
            {
                await button.Update(gameTime);
            }

            // Get Identifier from Text that has Focus.
            // Will check in order from lowest to highest.
            foreach (var button in Texts.OrderBy(button => button.OrderNumber))
            {
                if (button.IsInFocus)
                {
                    CurrentButtonIdentifier = button.Name;
                }
            }

            #endregion

            #region Texts

            // Update Order is Ascending.
            // From lowest to highest.
            foreach (var text in Texts.OrderBy(text => text.OrderNumber))
            {
                await text.Update(gameTime);
            }

            // Get Identifier from Text that has Focus.
            // Will check in order from lowest to highest.
            foreach (var text in Texts.OrderBy(text => text.OrderNumber))
            {
                if (text.IsInFocus)
                {
                    CurrentTextIdentifier = text.Name;
                }
            }

            #endregion

            #region Menus

            // Update Order is Ascending.
            // From lowest to highest.
            foreach (var menu in Menus.OrderBy(menu => menu.OrderNumber))
            {
                await menu.Update(gameTime);
            }

            // Get Identifier from Menu that has Focus.
            // Will check in order from lowest to highest.
            foreach (var menu in Menus.OrderBy(menu => menu.OrderNumber))
            {
                if (menu.IsInFocus)
                {
                    CurrentMenuIdentifier = menu.Name;
                }
            }

            #endregion

            #region Windows

            // Update Order is Ascending.
            // From lowest to highest.
            foreach (var window in Windows.OrderBy(win => win.OrderNumber))
            {
                await window.Update(gameTime);
            }

            // Get Identifier from Window that has Focus.
            // Will check in order from lowest to highest.
            foreach (var window in Windows.OrderBy(win => win.OrderNumber))
            {
                if (window.IsInFocus)
                {
                    CurrentWindowIdentifier = window.Name;
                }
            }

            #endregion
        }

        /// <summary>
        /// Draw Method.
        /// Draws all windows and their contents.
        /// Draws are done is Ascending Order by OrderNumber.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                #region Buttons

                // Draw Order is Ascending.
                // Drawing from lowest to highest.
                foreach (var button in Buttons.OrderBy(button => button.OrderNumber))
                {
                    // Draw Text UIBase.
                    button.Draw(spriteBatch);
                }

                #endregion

                #region Texts

                // Draw Order is Ascending.
                // Drawing from lowest to highest.
                foreach (var text in Texts.OrderBy(text => text.OrderNumber))
                {
                    // Draw Text UIBase.
                    text.Draw(spriteBatch);
                }

                #endregion

                #region Menus

                // Draw Order is Ascending.
                // Drawing from lowest to highest.
                foreach (var menu in Menus.OrderBy(menu => menu.OrderNumber))
                {
                    // Draw Menu UIBase.
                    menu.Draw(spriteBatch);
                }

                #endregion

                #region Windows

                // Draw Order is Ascending.
                // Drawing from lowest to highest.
                foreach (var window in Windows.OrderBy(win => win.OrderNumber))
                {
                    // Draw Window UIBase.
                    window.Draw(spriteBatch);
                }

                #endregion
            }
        }
    }
}