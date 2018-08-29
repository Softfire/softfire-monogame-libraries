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
        private List<UIButton> Buttons { get; } = new List<UIButton>();

        /// <summary>
        /// UI Group Texts.
        /// </summary>
        private List<UIText> Texts { get; } = new List<UIText>();

        /// <summary>
        /// UI Group Menus.
        /// </summary>
        private List<UIMenu> Menus { get; } = new List<UIMenu>();

        /// <summary>
        /// UI Group Windows.
        /// </summary>
        private List<UIWindow> Windows { get; } = new List<UIWindow>();

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
        /// Add Button.
        /// </summary>
        /// <param name="name">The button's name. Intaken as a string.</param>
        /// <returns>Returns the button id of the newly added button as an int.</returns>
        public int AddButton(string name)
        {
            var nextButtonId = UIBase.GetNextValidItemId(Buttons);

            var button = new UIButton(nextButtonId, name, new Vector2(ParentManager.GetViewportDimenions().Width / 2f, ParentManager.GetViewportDimenions().Height / 2f), 100, 50, nextButtonId);
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
            return UIBase.GetItemById(Buttons, buttonId);
        }

        /// <summary>
        /// Get Button.
        /// </summary>
        /// <param name="buttonName">The name of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a UIButton with the requested name.</returns>
        public UIButton GetButton(string buttonName)
        {
            return UIBase.GetItemByName(Buttons, buttonName);
        }

        /// <summary>
        /// Remove Button.
        /// </summary>
        /// <param name="buttonName">The id of the button to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button was removed.</returns>
        public bool RemoveButton(int buttonId)
        {
            return UIBase.RemoveItemById(Buttons, buttonId);
        }

        /// <summary>
        /// Remove Button.
        /// </summary>
        /// <param name="buttonName">The name of the button to be removed. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the button was removed.</returns>
        public bool RemoveButton(string buttonName)
        {
            return UIBase.RemoveItemByName(Buttons, buttonName);
        }

        /// <summary>
        /// Increase Button Order Number.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was increased.</returns>
        public bool IncreaseButtonOrderNumber(int buttonId)
        {
            return UIBase.IncreaseItemOrderNumber(Buttons, buttonId);
        }

        /// <summary>
        /// Increase Button Order Number.
        /// </summary>
        /// <param name="buttonName">The name of the button to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was increased.</returns>
        public bool IncreaseButtonOrderNumber(string buttonName)
        {
            return UIBase.IncreaseItemOrderNumber(Buttons, buttonName);
        }

        /// <summary>
        /// Decrease Button Order Number.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was decreased.</returns>
        public bool DecreaseButtonOrderNumber(int buttonId)
        {
            return UIBase.DecreaseItemOrderNumber(Buttons, buttonId);
        }

        /// <summary>
        /// Decrease Button Order Number.
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
        /// <param name="name">The text's name. Intaken as a string.</param>
        /// <param name="font">The text's font. Intaken as a SpriteFont.</param>
        /// <param name="text">The text's text. Intaken as a string.</param>
        /// <returns>Returns the text id of the newly added text as an int.</returns>
        public int AddText(string name, SpriteFont font, string text)
        {
            var nextTextId = UIBase.GetNextValidItemId(Texts);

            var newText = new UIText(nextTextId, name, font, text, nextTextId, new Vector2(ParentManager.GetViewportDimenions().Width / 2f, ParentManager.GetViewportDimenions().Height / 2f));
            newText.LoadContent();

            Texts.Add(newText);

            return nextTextId;
        }

        /// <summary>
        /// Get Text.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve.</param>
        /// <returns>Returns a UIText with the requested id, if present, otherwise null.</returns>
        public UIText GetText(int textId)
        {
            return UIBase.GetItemById(Texts, textId);
        }

        /// <summary>
        /// Get Text.
        /// </summary>
        /// <param name="textName">The name of the text to retrieve.</param>
        /// <returns>Returns a UIText with the requested name, if present, otherwise null.</returns>
        public UIText GetText(string textName)
        {
            return UIBase.GetItemByName(Texts, textName);
        }

        /// <summary>
        /// Remove Text.
        /// </summary>
        /// <param name="textId">The id of the text to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text was removed.</returns>
        public bool RemoveText(int textId)
        {
            return UIBase.RemoveItemById(Texts, textId);
        }

        /// <summary>
        /// Remove Text.
        /// </summary>
        /// <param name="textName">The name of the text to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text was removed.</returns>
        public bool RemoveText(string textName)
        {
            return UIBase.RemoveItemByName(Texts, textName);
        }

        /// <summary>
        /// Increase Text Order Number.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was increased.</returns>
        public bool IncreaseTextOrderNumber(int textId)
        {
            return UIBase.IncreaseItemOrderNumber(Texts, textId);
        }

        /// <summary>
        /// Increase Text Order Number.
        /// </summary>
        /// <param name="textName">The name of the text to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was increased.</returns>
        public bool IncreaseTextOrderNumber(string textName)
        {
            return UIBase.IncreaseItemOrderNumber(Texts, textName);
        }

        /// <summary>
        /// Decrease Text Order Number.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was decreased.</returns>
        public bool DecreaseTextOrderNumber(int textId)
        {
            return UIBase.DecreaseItemOrderNumber(Texts, textId);
        }

        /// <summary>
        /// Decrease Text Order Number.
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
        /// <param name="name">The menu's name. Intaken as a string.</param>
        /// <param name="width">The menu's width. Intaken as an int. Default is 100.</param>
        /// <param name="height">The menu's height. Intaken as an int. Default is 400.</param>
        /// <returns>Returns the menu id of the newly added menu as an int.</returns>
        public int AddMenu(string name, int width = 100, int height = 400)
        {
            var nextMenuId = UIBase.GetNextValidItemId(Menus);

            var newMenu = new UIMenu(this, nextMenuId, name, new Vector2(ParentManager.GetViewportDimenions().Width / 2f, ParentManager.GetViewportDimenions().Height / 2f), width, height, nextMenuId);
            newMenu.LoadContent();

            Menus.Add(newMenu);

            return nextMenuId;
        }

        /// <summary>
        /// Get Menu.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a UIMenu with the requested id, if present, otherwise null.</returns>
        public UIMenu GetMenu(int menuId)
        {
            return UIBase.GetItemById(Menus, menuId);
        }

        /// <summary>
        /// Get Menu.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a UIMenu with the requested name, if present, otherwise null.</returns>
        public UIMenu GetMenu(string menuName)
        {
            return UIBase.GetItemByName(Menus, menuName);
        }

        /// <summary>
        /// Remove Menu.
        /// </summary>
        /// <param name="menuId">The id of the menu to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu was removed.</returns>
        public bool RemoveMenu(int menuId)
        {
            return UIBase.RemoveItemById(Menus, menuId);
        }

        /// <summary>
        /// Remove Menu.
        /// </summary>
        /// <param name="menuName">The name of the menu to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu was removed.</returns>
        public bool RemoveMenu(string menuName)
        {
            return UIBase.RemoveItemByName(Menus, menuName);
        }

        /// <summary>
        /// Increase Menu Order Number.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was increased.</returns>
        public bool IncreaseMenuOrderNumber(int menuId)
        {
            return UIBase.IncreaseItemOrderNumber(Menus, menuId);
        }

        /// <summary>
        /// Increase Menu Order Number.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was increased.</returns>
        public bool IncreaseMenuOrderNumber(string menuName)
        {
            return UIBase.IncreaseItemOrderNumber(Menus, menuName);
        }

        /// <summary>
        /// Decrease Menu Order Number.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was decreased.</returns>
        public bool DecreaseMenuOrderNumber(int menuId)
        {
            return UIBase.DecreaseItemOrderNumber(Menus, menuId);
        }

        /// <summary>
        /// Decrease Menu Order Number.
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
        public int AddWindow(string name, int width = 400, int height = 400)
        {
            var nextWindowId = UIBase.GetNextValidItemId(Windows);

            var newWindow = new UIWindow(this, nextWindowId, name, new Vector2(ParentManager.GetViewportDimenions().Width / 2f, ParentManager.GetViewportDimenions().Height / 2f), width, height, nextWindowId);
            newWindow.LoadContent();

            Windows.Add(newWindow);

            return nextWindowId;
        }

        /// <summary>
        /// Get Window.
        /// </summary>
        /// <param name="windowId">The id of the window to retrieve. Intaken as an int.</param>
        /// <returns>Returns a UIWindow with the requested id, if present, otherwise null.</returns>
        public UIWindow GetWindow(int windowId)
        {
            return UIBase.GetItemById(Windows, windowId);
        }

        /// <summary>
        /// Get Window.
        /// </summary>
        /// <param name="windowName">The name of the window to retrieve. Intaken as a string.</param>
        /// <returns>Returns a UIWindow with the requested name, if present, otherwise null.</returns>
        public UIWindow GetWindow(string windowName)
        {
            return UIBase.GetItemByName(Windows, windowName);
        }

        /// <summary>
        /// Remove Window.
        /// </summary>
        /// <param name="windowId">The id of the window to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the window was removed.</returns>
        public bool RemoveWindow(int windowId)
        {
            return UIBase.RemoveItemById(Windows, windowId);
        }

        /// <summary>
        /// Remove Window.
        /// </summary>
        /// <param name="windowName">The name of the window to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the window was removed.</returns>
        public bool RemoveWindow(string windowName)
        {
            return UIBase.RemoveItemByName(Windows, windowName);
        }

        /// <summary>
        /// Increase Windows Order Number.
        /// </summary>
        /// <param name="windowId">The id of the window to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the window's order number was increased.</returns>
        public bool IncreaseWindowOrderNumber(int windowId)
        {
            return UIBase.IncreaseItemOrderNumber(Windows, windowId);
        }

        /// <summary>
        /// Increase Windows Order Number.
        /// </summary>
        /// <param name="windowName">The name of the window to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the window's order number was increased.</returns>
        public bool IncreaseWindowOrderNumber(string windowName)
        {
            return UIBase.IncreaseItemOrderNumber(Windows, windowName);
        }

        /// <summary>
        /// Decrease Windows Order Number.
        /// </summary>
        /// <param name="windowId">The id of the window to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the window's order number was decreased.</returns>
        public bool DecreaseWindowOrderNumber(int windowId)
        {
            return UIBase.DecreaseItemOrderNumber(Windows, windowId);
        }

        /// <summary>
        /// Decrease Windows Order Number.
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
                if (button.IsVisible)
                {
                    // Check if any input devices are over top of any of the texts.
                    //for (var index = 0; index < ActiveInputDevices.Count; index++)
                    //{
                    //    var inputDevice = ActiveInputDevices[index];
                    //    button.CheckIsInFocus(new Rectangle((int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                    //                                        (int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                    //                                        inputDevice.Width,
                    //                                        inputDevice.Height));
                    //}
                }

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
                if (text.IsVisible)
                {
                    // Check if any input devices are over top of any of the texts.
                    //for (var index = 0; index < ActiveInputDevices.Count; index++)
                    //{
                    //    var inputDevice = ActiveInputDevices[index];
                    //    text.CheckIsInFocus(new Rectangle((int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                    //                                      (int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                    //                                      inputDevice.Width,
                    //                                      inputDevice.Height));
                    //}
                }

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
                if (menu.IsVisible)
                {
                    // Check if any input devices are over top of any of the menus.
                    //for (var index = 0; index < ActiveInputDevices.Count; index++)
                    //{
                    //    var inputDevice = ActiveInputDevices[index];
                    //    menu.CheckIsInFocus(new Rectangle((int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                    //                                      (int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                    //                                      inputDevice.Width,
                    //                                      inputDevice.Height));
                    //}
                }

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
                //window.ActiveInputDevices = ActiveInputDevices;

                if (AreWindowsMoving() == false)
                {
                    if (window.IsVisible)
                    {
                        // Check if any input devices are over top of any of the window contents.
                        //for (var index = 0; index < ActiveInputDevices.Count; index++)
                        //{
                        //    var inputDevice = ActiveInputDevices[index];

                        //    // Check Window focus.
                        //    window.CheckIsInFocus(inputDevice);

                        //    // Check Window contents focus.
                        //    window.ContentsCamera.CheckIsInFocus(new Rectangle((int) UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                        //                                                       (int) UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                        //                                                       inputDevice.Width,
                        //                                                       inputDevice.Height));
                        //}
                    }
                }

                if (CurrentWindowIdentifier == window.Name)
                {
                    //if (window.IsMoving == false &&
                    //    window.GetBorder("Top").IsVisible)
                    //{
                    //    window.IsMoving = IOMouse.LeftClickDownInside(window.GetBorder("Top").Rectangle);
                    //}
                    //else if (window.IsMoving &&
                    //         IOMouse.LeftClickHeld())
                    //{
                    //    window.Move(IOMouse.MovementDelta());
                    //}
                    //else
                    //{
                    //    window.IsMoving = false;
                    //}
                }

                if (CurrentWindowIdentifier == window.Name)
                {
                    if (IOKeyboard.KeyPress(Keys.D1))
                    {
                        IncreaseWindowOrderNumber(window.Name);
                    }

                    if (IOKeyboard.KeyPress(Keys.D2))
                    {
                        DecreaseWindowOrderNumber(window.Name);
                    }
                }

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

                    // Draw Window Contents.
                    //window.DrawContents(spriteBatch);
                }

                #endregion
            }
        }
    }
}