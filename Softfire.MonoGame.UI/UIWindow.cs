using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.IO;
using Softfire.MonoGame.UI.Items;
using Softfire.MonoGame.UI.Menu;

namespace Softfire.MonoGame.UI
{
    public class UIWindow : UIBase
    {
        /// <summary>
        /// Internal border thickness.
        /// </summary>
        private int _borderThickness = 10;

        /// <summary>
        /// The parent group for this window.
        /// </summary>
        internal UIGroup ParentGroup { get; }

        /// <summary>
        /// The window's buttons. Use AddButton, RemoveButton and GetButton to modify UI Buttons.
        /// </summary>
        /// <see cref="AddButton(string)"/>
        /// <see cref="RemoveButton(int)"/>
        /// <see cref="RemoveButton(string)"/>
        /// <see cref="GetButton(int)"/>
        /// <see cref="GetButton(string)"/>
        public List<UIButton> Buttons { get; } = new List<UIButton>();

        /// <summary>
        /// The window's borders.
        /// </summary>
        public List<UIBorder> Borders { get; }

        /// <summary>
        /// The window's texts. Use AddText, RemoveText and GetText to modify UI Texts.
        /// </summary>
        /// <see cref="AddText(string, SpriteFont, string)"/>
        /// <see cref="RemoveText(int)"/>
        /// <see cref="RemoveText(string)"/>
        /// <see cref="GetText(int)"/>
        /// <see cref="GetText(string)"/>
        public List<UIText> Texts { get; } = new List<UIText>();

        /// <summary>
        /// The window's menus. Use AddMenu, RemoveMenu and GetMenu to modify UI Menus.
        /// </summary>
        /// <see cref="AddMenu(string, int, int)"/>
        /// <see cref="RemoveMenu(int)"/>
        /// <see cref="RemoveMenu(string)"/>
        /// <see cref="GetMenu(int)"/>
        /// <see cref="GetMenu(string)"/>
        public List<UIMenu> Menus { get; } = new List<UIMenu>();

        /// <summary>
        /// An extended rectangle of the window including its borders, if they are enabled.
        /// </summary>
        public Rectangle ExtendedRectangle => CalculateExtendedRectangle();

        /// <summary>
        /// The window's viewport. Displays only a portion of the WorldRectangle's viewing area unless it's of equal size.
        /// </summary>
        public Viewport ViewPort => new Viewport(Rectangle);

        /// <summary>
        /// The window's world Rectangle. The total viewing area.
        /// </summary>
        public Rectangle WorldRectangle { get; private set; }

        /// <summary>
        /// A UI window that has borders and can have buttons, texts and menus in them.
        /// They are scrollable and can have their width and height adjusted.
        /// </summary>
        /// <param name="parentGroup">The window's parent group. Intaken as a UIGroup.</param>
        /// <param name="id">The window's id. Intaken as an int.</param>
        /// <param name="name">The window's name. Intaken as a string.</param>
        /// <param name="position">The window's position. Intaken as a Vector2.</param>
        /// <param name="width">The window's width. Intaken as a float.</param>
        /// <param name="height">The window's height. Intaken as a float.</param>
        /// <param name="orderNumber">The window's order number. Used to sort the window. Intaken as an int.</param>
        /// <param name="worldRectangle">The window's world Rectangle. The total viewing area of the window. Intaken as a Rectangle.</param>
        /// <param name="borderThickness">The window's border thickness. Intaken as an int. Set to 0 to disable borders.</param>
        public UIWindow(UIGroup parentGroup, int id, string name, Vector2 position, int width, int height, int orderNumber,
                        Rectangle? worldRectangle = null, int borderThickness = 10) : base(id, name, position, width, height, orderNumber)
        {
            ParentGroup = parentGroup;
            WorldRectangle = worldRectangle ?? new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            borderThickness = borderThickness >= 0 ? borderThickness : 0;

            Borders = new List<UIBorder>(8)
            {
                new UIBorder(1, "Top", Vector2.Zero, width, borderThickness, 1),
                new UIBorder(2, "Right", Vector2.Zero, borderThickness, height, 2),
                new UIBorder(3, "Bottom", Vector2.Zero, width, borderThickness, 3),
                new UIBorder(4, "Left", Vector2.Zero, borderThickness, height, 4),
                new UIBorder(5, "TopLeft", Vector2.Zero, borderThickness, borderThickness, 5),
                new UIBorder(6, "TopRight", Vector2.Zero, borderThickness, borderThickness, 6),
                new UIBorder(7, "BottomLeft", Vector2.Zero, borderThickness, borderThickness, 7),
                new UIBorder(8, "BottomRight", Vector2.Zero, borderThickness, borderThickness, 8)
            };

            if (borderThickness == 0)
            {
                SetBorderVisibility(false);
            }

            SetWindowTransparency(Transparencies["Background"]);
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
                    var button = new UIButton(nextButtonId, buttonName, new Vector2(ParentGroup.ParentManager.GetViewportDimenions().Width / 2f, ParentGroup.ParentManager.GetViewportDimenions().Height / 2f), Width, Height / 4, nextButtonId);
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
                nextTextId = GetNextValidItemId(Texts);

                if (CheckForText(nextTextId) == false)
                {
                    var newText = new UIText(nextTextId, textName, font, text, nextTextId);
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
            return CheckItemById(Texts, textId);
        }

        /// <summary>
        /// Checks for text by name.
        /// </summary>
        /// <param name="textName">The name of the text to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the text is present.</returns>
        public bool CheckForText(string textName)
        {
            return CheckItemByName(Texts, textName);
        }

        /// <summary>
        /// Gets text by id.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve.</param>
        /// <returns>Returns a text with the requested id, if present, otherwise null.</returns>
        public UIText GetText(int textId)
        {
            return CheckForText(textId) ? GetItemById(Texts, textId) : default(UIText);
        }

        /// <summary>
        /// Gets text by name.
        /// </summary>
        /// <param name="textName">The name of the text to retrieve.</param>
        /// <returns>Returns a text with the requested name, if present, otherwise null.</returns>
        public UIText GetText(string textName)
        {
            return CheckForText(textName) ? GetItemByName(Texts, textName) : default(UIText);
        }

        /// <summary>
        /// Removes text by id.
        /// </summary>
        /// <param name="textId">The id of the text to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text was removed.</returns>
        public bool RemoveText(int textId)
        {
            return RemoveItemById(Texts, textId);
        }

        /// <summary>
        /// Removes text by name.
        /// </summary>
        /// <param name="textName">The name of the text to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text was removed.</returns>
        public bool RemoveText(string textName)
        {
            return RemoveItemByName(Texts, textName);
        }

        /// <summary>
        /// Increases text order number by id.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was increased.</returns>
        public bool IncreaseTextOrderNumber(int textId)
        {
            return IncreaseItemOrderNumber(Texts, textId);
        }

        /// <summary>
        /// Increases text order number by name.
        /// </summary>
        /// <param name="textName">The name of the text to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was increased.</returns>
        public bool IncreaseTextOrderNumber(string textName)
        {
            return IncreaseItemOrderNumber(Texts, textName);
        }

        /// <summary>
        /// Decreases text order number by id.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was decreased.</returns>
        public bool DecreaseTextOrderNumber(int textId)
        {
            return DecreaseItemOrderNumber(Texts, textId);
        }

        /// <summary>
        /// Decreases text order number by name.
        /// </summary>
        /// <param name="textName">The name of the text to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was decreased.</returns>
        public bool DecreaseTextOrderNumber(string textName)
        {
            return DecreaseItemOrderNumber(Texts, textName);
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
                nextMenuId = GetNextValidItemId(Texts);

                if (CheckForMenu(nextMenuId) == false)
                {
                    var newMenu = new UIMenu(ParentGroup, nextMenuId, menuName, new Vector2(ParentGroup.ParentManager.GetViewportDimenions().Width / 2f, ParentGroup.ParentManager.GetViewportDimenions().Height / 2f), width, height, nextMenuId);
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
            return CheckItemById(Menus, menuId);
        }

        /// <summary>
        /// Checks for menu by name.
        /// </summary>
        /// <param name="menuName">The name of the menu to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the menu is present.</returns>
        public bool CheckForMenu(string menuName)
        {
            return CheckItemByName(Menus, menuName);
        }

        /// <summary>
        /// Gets a menu ny id.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a menu with the requested id, if present, otherwise null.</returns>
        public UIMenu GetMenu(int menuId)
        {
            return CheckForMenu(menuId) ? GetItemById(Menus, menuId) : default(UIMenu);
        }

        /// <summary>
        /// Gets a menu by name.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a menu with the requested name, if present, otherwise null.</returns>
        public UIMenu GetMenu(string menuName)
        {
            return CheckForMenu(menuName) ? GetItemByName(Menus, menuName) : default(UIMenu);
        }

        /// <summary>
        /// Removes a menu by id.
        /// </summary>
        /// <param name="menuId">The id of the menu to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu was removed.</returns>
        public bool RemoveMenu(int menuId)
        {
            return RemoveItemById(Menus, menuId);
        }

        /// <summary>
        /// Removes a menu by name.
        /// </summary>
        /// <param name="menuName">The name of the menu to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu was removed.</returns>
        public bool RemoveMenu(string menuName)
        {
            return RemoveItemByName(Menus, menuName);
        }

        /// <summary>
        /// Increases a menu's order number by id.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was increased.</returns>
        public bool IncreaseMenuOrderNumber(int menuId)
        {
            return IncreaseItemOrderNumber(Menus, menuId);
        }

        /// <summary>
        /// Increases a menu's order number by name.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was increased.</returns>
        public bool IncreaseMenuOrderNumber(string menuName)
        {
            return IncreaseItemOrderNumber(Menus, menuName);
        }

        /// <summary>
        /// Decreases a menu's order number by id.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was decreased.</returns>
        public bool DecreaseMenuOrderNumber(int menuId)
        {
            return DecreaseItemOrderNumber(Menus, menuId);
        }

        /// <summary>
        /// Decreases a menu's order number by name.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was decreased.</returns>
        public bool DecreaseMenuOrderNumber(string menuName)
        {
            return DecreaseItemOrderNumber(Menus, menuName);
        }

        #endregion

        #region Borders

        /// <summary>
        /// Checks for border by id.
        /// </summary>
        /// <param name="borderId">The id of the border to search. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the border is present.</returns>
        public bool CheckForBorder(int borderId)
        {
            return CheckItemById(Borders, borderId);
        }

        /// <summary>
        /// Checks for border by name.
        /// </summary>
        /// <param name="borderName">The name of the border to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the border is present.</returns>
        public bool CheckForBorder(string borderName)
        {
            return CheckItemByName(Borders, borderName);
        }

        /// <summary>
        /// Gets a border ny id.
        /// </summary>
        /// <param name="borderId">The id of the border to retrieve. Intaken as an int.</param>
        /// <returns>Returns a border with the requested id, if present, otherwise null.</returns>
        public UIBorder GetBorder(int borderId)
        {
            return CheckForBorder(borderId) ? GetItemById(Borders, borderId) : default(UIBorder);
        }

        /// <summary>
        /// Gets a border by name.
        /// </summary>
        /// <param name="borderName">The name of the border to retrieve. Intaken as a string.</param>
        /// <returns>Returns a border with the requested name, if present, otherwise null.</returns>
        public UIBorder GetBorder(string borderName)
        {
            return CheckForBorder(borderName) ? GetItemByName(Borders, borderName) : default(UIBorder);
        }

        /// <summary>
        /// Sets all the window's borders to the provided thickness.
        /// </summary>
        /// <param name="borderThickness">The window's border thickness. Intaken as an int.</param>
        public void SetBorderThickness(int borderThickness)
        {
            SetBorderThickness(borderThickness, borderThickness, borderThickness, borderThickness);
            SetBorderThickness(new Vector2(borderThickness, borderThickness), new Vector2(borderThickness, borderThickness),
                               new Vector2(borderThickness, borderThickness), new Vector2(borderThickness, borderThickness));
        }

        /// <summary>
        /// Sets the window's individual main border thicknesses.
        /// </summary>
        /// <param name="top">The window's top border thickness. Intaken as an int.</param>
        /// <param name="right">The window's right border thickness. Intaken as an int.</param>
        /// <param name="bottom">The window's bottom border thickness. Intaken as an int.</param>
        /// <param name="left">The window's left border thickness. Intaken as an int.</param>
        public void SetBorderThickness(int top, int right, int bottom, int left)
        {
            GetBorder(1).Height = top;
            GetBorder(2).Width = right;
            GetBorder(3).Height = bottom;
            GetBorder(4).Width = left;
        }

        /// <summary>
        /// Sets the window's individual corner border thicknesses.
        /// </summary>
        /// <param name="topLeft">The window's top left border thickness. Intaken as a Vector2. The X axis is the border's width and the Y axis is the the border's height..</param>
        /// <param name="topRight">The window's top left border thickness. Intaken as a Vector2. The X axis is the border's width and the Y axis is the the border's height..</param>
        /// <param name="bottomRight">The window's top left border thickness. Intaken as a Vector2. The X axis is the border's width and the Y axis is the the border's height..</param>
        /// <param name="bottomLeft">The window's top left border thickness. Intaken as a Vector2. The X axis is the border's width and the Y axis is the the border's height..</param>
        /// <remarks>The window's corner border's thickness. Intaken as a Vector2. The X axis is the border's width and the Y axis is the the border's height.</remarks>
        public void SetBorderThickness(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft)
        {
            GetBorder(5).Width = (int)topLeft.X;
            GetBorder(5).Height = (int)topLeft.Y;
            GetBorder(6).Width = (int)topRight.X;
            GetBorder(6).Height = (int)topRight.Y;
            GetBorder(7).Width = (int)bottomRight.X;
            GetBorder(7).Height = (int)bottomRight.Y;
            GetBorder(8).Width = (int)bottomLeft.X;
            GetBorder(8).Height = (int)bottomLeft.Y;
        }

        /// <summary>
        /// Sets visibility for each border to that of the window.
        /// </summary>
        /// <see cref="UIBase.IsVisible"/>
        public void SetBorderVisibility()
        {
            foreach (var border in Borders)
            {
                border.IsVisible = IsVisible;
            }
        }

        /// <summary>
        /// Sets visibility for each border.
        /// </summary>
        /// <see cref="UIBase.IsVisible"/>
        public void SetBorderVisibility(bool visibility)
        {
            foreach (var border in Borders)
            {
                border.IsVisible = visibility;
            }
        }

        /// <summary>
        /// Sets individual border visibility.
        /// </summary>
        /// <param name="top">A boolean indicating whether the top border is visible.</param>
        /// <param name="right">A boolean indicating whether the right border is visible.</param>
        /// <param name="bottom">A boolean indicating whether the bottom border is visible.</param>
        /// <param name="left">A boolean indicating whether the left border is visible.</param>
        /// <param name="topLeft">A boolean indicating whether the top left corner border is visible.</param>
        /// <param name="topRight">A boolean indicating whether the top right corner border is visible.</param>
        /// <param name="bottomRight">A boolean indicating whether the bottom right corner border is visible.</param>
        /// <param name="bottomLeft">A boolean indicating whether the bottom left corner border is visible.</param>
        /// <see cref="UIBase.IsVisible"/>
        public void SetBorderVisibility(bool top, bool right, bool bottom, bool left,
                                        bool topLeft, bool topRight, bool bottomRight, bool bottomLeft)
        {
            GetBorder(1).IsVisible = top;
            GetBorder(2).IsVisible = right;
            GetBorder(3).IsVisible = bottom;
            GetBorder(4).IsVisible = left;
            GetBorder(5).IsVisible = topLeft;
            GetBorder(6).IsVisible = topRight;
            GetBorder(7).IsVisible = bottomRight;
            GetBorder(8).IsVisible = bottomLeft;
        }

        /// <summary>
        /// Updates the window's border positions.
        /// </summary>
        private async Task UpdateBorderPositions(GameTime gameTime)
        {
            var borderTop = GetBorder("Top");
            var borderRight = GetBorder("Right");
            var borderBottom = GetBorder("Bottom");
            var borderLeft = GetBorder("Left");
            var borderTopLeft = GetBorder("TopLeft");
            var borderTopRight = GetBorder("TopRight");
            var borderBottomRight = GetBorder("BottomRight");
            var borderBottomLeft = GetBorder("BottomLeft");

            // Main borders.
            borderTop.Position = new Vector2(borderTop.Position.X, -((HeightF / 2f) + borderTop.HeightF - borderTop.GetOutline("Bottom").Thickness));
            borderRight.Position = new Vector2((WidthF / 2f) + borderRight.WidthF - borderRight.GetOutline("Left").Thickness, borderRight.Position.Y);
            borderBottom.Position = new Vector2(borderBottom.Position.X, (HeightF / 2f) + borderBottom.HeightF - borderBottom.GetOutline("Top").Thickness);
            borderLeft.Position = new Vector2(-((WidthF / 2f) + borderLeft.WidthF - borderLeft.GetOutline("Right").Thickness), borderLeft.Position.Y);
            
            // Corner borders.
            borderTopLeft.Position = new Vector2(-((WidthF / 2f) + borderTopLeft.WidthF - borderTopLeft.GetOutline("Right").Thickness),
                                                 -((HeightF / 2f) + borderTopLeft.HeightF - borderTopLeft.GetOutline("Bottom").Thickness));
            borderTopRight.Position = new Vector2((WidthF / 2f) + borderTopRight.WidthF - borderTopRight.GetOutline("Left").Thickness,
                                                  -((HeightF / 2f) + borderTopRight.HeightF - borderTopRight.GetOutline("Bottom").Thickness));
            borderBottomRight.Position = new Vector2((WidthF / 2f) + borderBottomRight.WidthF - borderBottomRight.GetOutline("Left").Thickness,
                                                     (HeightF / 2f) + borderBottomRight.HeightF - borderBottomRight.GetOutline("Top").Thickness);
            borderBottomLeft.Position = new Vector2(-((WidthF / 2f) + borderBottomLeft.WidthF - borderBottomLeft.GetOutline("Right").Thickness),
                                                    (HeightF / 2f) + borderBottomLeft.HeightF - borderBottomLeft.GetOutline("Top").Thickness);

            foreach (var border in Borders)
            {
                border.ParentPosition = Position;
                await border.Update(gameTime);
            }
        }

        #endregion

        /// <summary>
        /// Used to move the window. Pass in an IO devices deltas to sync the window's movements to the IO device.
        /// </summary>
        /// <param name="deltas">The input device's deltas. Intaken as a Vector2.</param>
        /// <see cref="UIBase.IsMovable"/>
        /// <remarks>IsMovable must be set to true for this to work.</remarks>
        public void Move(Vector2 deltas)
        {
            if (IsMovable)
            {
                Position = new Vector2(Position.X + deltas.X, Position.Y + deltas.Y);
            }
        }

        /// <summary>
        /// Calculates an extended Rectangle that includes the window's borders.
        /// </summary>
        /// <returns>Returns a Rectangle that includes the window's borders as a Rectangle.</returns>
        private Rectangle CalculateExtendedRectangle()
        {
            var rectangle = Rectangle;
            var border = GetItemByName(Borders, "Top");

            if (border.IsVisible)
            {
                rectangle = new Rectangle(rectangle.X,
                                          rectangle.Y - ((GetItemByName(border.Outlines, "Bottom").Thickness * 2) + border.Height + (GetItemByName(border.Outlines, "Top").Thickness * 2)),
                                          rectangle.Width,
                                          rectangle.Height + ((GetItemByName(border.Outlines, "Bottom").Thickness * 2) + border.Height + (GetItemByName(border.Outlines, "Top").Thickness * 2)));
            }

            border = GetItemByName(Borders, "Right");

            if (border.IsVisible)
            {
                rectangle = new Rectangle(rectangle.X,
                                          rectangle.Y,
                                          rectangle.Width + ((GetItemByName(border.Outlines, "Left").Thickness * 2) + border.Width + (GetItemByName(border.Outlines, "Right").Thickness * 2)),
                                          rectangle.Height);
            }

            border = GetItemByName(Borders, "Bottom");

            if (border.IsVisible)
            {
                rectangle = new Rectangle(rectangle.X,
                                          rectangle.Y,
                                          rectangle.Width,
                                          rectangle.Height + ((GetItemByName(border.Outlines, "Top").Thickness * 2) + border.Height + (GetItemByName(border.Outlines, "Bottom").Thickness * 2)));
            }

            border = GetItemByName(Borders, "Left");

            if (border.IsVisible)
            {
                rectangle = new Rectangle(rectangle.X - ((GetItemByName(border.Outlines, "Right").Thickness * 2) + border.Width + (GetItemByName(border.Outlines, "Left").Thickness * 2)),
                                          rectangle.Y,
                                          rectangle.Width + ((GetItemByName(border.Outlines, "Right").Thickness * 2) + border.Width + (GetItemByName(border.Outlines, "Left").Thickness * 2)),
                                          rectangle.Height);
            }

            return rectangle;
        }

        /// <summary>
        /// Sets the window, borders and outlines transparency levels.
        /// </summary>
        /// <param name="transparencyLevel">The transparency level to set. Intaken as a float.</param>
        public void SetWindowTransparency(float transparencyLevel)
        {
            Transparencies["Background"] = transparencyLevel;
            Transparencies["Outline"] = transparencyLevel;

            foreach (var border in Borders)
            {
                border.Transparencies["Background"] = Transparencies["Background"];
                border.Transparencies["Outline"] = Transparencies["Outline"];

                border.SetOutlineTransparency();
            }
        }

        /// <summary>
        /// Sets the window's world Rectangle to that of the provided one if at least equal to the current Viewport.
        /// </summary>
        /// <param name="worldRectangle">The total view area of the window. Intaken as a Rectangle.</param>
        public void SetWorldRectangle(Rectangle worldRectangle)
        {
            if (worldRectangle.Width >= ViewPort.Width &&
                worldRectangle.Height >= ViewPort.Height)
            {
                WorldRectangle = worldRectangle;
            }
        }

        /// <summary>
        /// Draw Contents.
        /// Draws UIWindow Elements and Text.
        /// </summary>
        public void DrawContents(SpriteBatch contentsSpriteBatch, Viewport parentViewport)
        {
            //contentsSpriteBatch.GraphicsDevice.Viewport = ContentsCamera.Viewport;

            //contentsSpriteBatch.Begin(transformMatrix: ContentsCamera.Matrix);

            //foreach (var border in Borders.OrderBy(border => border.OrderNumber))
            //{
            //    border.IsVisible = IsVisible;
            //    border.Transparencies["Background"] = Transparencies["Background"];
            //    border.Draw(contentsSpriteBatch);
            //}

            //foreach (var button in Buttons.OrderBy(button => button.OrderNumber))
            //{
            //    if (button.GetText() != null)
            //    {
            //        button.GetText().IsVisible = IsVisible;
            //        button.GetText().ApplyEnhancedTextScaling(button.Rectangle, true);
            //    }

            //    button.IsVisible = IsVisible;
            //    button.Transparencies["Background"] = Transparencies["Background"];
            //    button.Draw(contentsSpriteBatch);
            //}

            //foreach (var text in Texts.OrderBy(text => text.OrderNumber))
            //{
            //    text.IsVisible = IsVisible;
            //    text.Transparencies["Background"] = Transparencies["Background"];
            //    text.ApplyEnhancedTextWrap(ContentsCamera.WorldViewRectangle);
            //    text.Draw(contentsSpriteBatch);
            //}

            //contentsSpriteBatch.End();

            //// Switch back to Parent Viewport.
            //contentsSpriteBatch.GraphicsDevice.Viewport = parentViewport;
        }

        /// <summary>
        /// Load Content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            foreach (var border in Borders)
            {
                border.LoadContent();
            }
        }

        /// <summary>
        /// UIWindow Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            await base.Update(gameTime);

            await UpdateBorderPositions(gameTime);

            foreach (var menu in Menus.OrderBy(menu => menu.OrderNumber))
            {
                await menu.Update(gameTime);
            }

            foreach (var button in Buttons.OrderBy(button => button.OrderNumber))
            {
                await button.Update(gameTime);
            }

            foreach (var text in Texts.OrderBy(text => text.OrderNumber))
            {
                await text.Update(gameTime);
            }
        }

        /// <summary>
        /// UIWindow Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (var border in Borders.OrderBy(border => border.OrderNumber))
            {
                border.Draw(spriteBatch);
            }

            foreach (var menu in Menus.OrderBy(menu => menu.OrderNumber))
            {
                menu.Draw(spriteBatch);
            }

            foreach (var button in Buttons.OrderBy(button => button.OrderNumber))
            {
                button.Draw(spriteBatch);
            }

            foreach (var text in Texts.OrderBy(text => text.OrderNumber))
            {
                text.Draw(spriteBatch);
            }
        }
    }
}