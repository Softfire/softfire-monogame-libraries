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
        /// UI Window Parent Group.
        /// </summary>
        internal UIGroup ParentGroup { get; }

        /// <summary>
        /// UI Window Border Color.
        /// </summary>
        public Color BorderColor { get; set; } = Color.LightGray;

        /// <summary>
        /// UI Window Border Thickness.
        /// </summary>
        public int BorderThickness { get; set; } = 10;

        /// <summary>
        /// UI Window Active input Devices.
        /// </summary>
        public List<Rectangle> ActiveInputDevices { private get; set; } = new List<Rectangle>();

        /// <summary>
        /// UI Window Buttons.
        /// Use AddButton, RemoveButton and GetButton to modify UI Buttons.
        /// </summary>
        public List<UIButton> Buttons { get; } = new List<UIButton>();

        /// <summary>
        /// UI Window Borders.
        /// Use AddBorder, RemoveBorder and GetBorder to modify UI Borders.
        /// </summary>
        public List<UIBorder> Borders { get; }

        /// <summary>
        /// UI Window Texts.
        /// Use AddText, RemoveText and GetText to modify UI Texts.
        /// </summary>
        public List<UIText> Texts { get; } = new List<UIText>();

        /// <summary>
        /// UI Window Menus.
        /// Use AddMenu, RemoveMenu and GetMenu to modify UI Menus.
        /// </summary>
        public List<UIMenu> Menus { get; } = new List<UIMenu>();

        /// <summary>
        /// UI Window Windows Camera2D.
        /// Used to scroll the view area of the window.
        /// </summary>
        public IOCamera2D ContentsCamera { get; }

        /// <summary>
        /// UI Window Extended Rectangle.
        /// A rectangle of the UIWindow including Borders, if they are enabled.
        /// </summary>
        public Rectangle ExtendedRectangle { get; private set; }

        /// <summary>
        /// UI Window.
        /// </summary>
        /// <param name="parentGroup">The parent group. Intaken as a UIGroup.</param>
        /// <param name="id">The window's id. Intaken as an int.</param>
        /// <param name="name">The window's name. Intaken as a string.</param>
        /// <param name="position">Intakes the UI's position as a Vector2.</param>
        /// <param name="width">Intakes the UI's width as a float.</param>
        /// <param name="height">Intakes the UI's height as a float.</param>
        /// <param name="orderNumber">Intakes an int that will be used to define the update/draw order. Update/Draw order is from lowest to highest.</param>
        /// <param name="worldRectangle">Intakes a Rectangle describing the extended view of the UIWindow. Used by Camera2D.</param>
        public UIWindow(UIGroup parentGroup, int id, string name, Vector2 position, int width, int height, int orderNumber, Rectangle? worldRectangle = null) : base(id, name, position, width, height, orderNumber)
        {
            ParentGroup = parentGroup;
            ContentsCamera = new IOCamera2D(new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height), worldRectangle ?? new Rectangle(0, 0, width, height));

            Borders = new List<UIBorder>(8)
            {
                new UIBorder(1, "Top", new Vector2(), width, BorderThickness, 1),
                new UIBorder(2, "Right", new Vector2(), BorderThickness, height, 2),
                new UIBorder(3, "Bottom", new Vector2(), width, BorderThickness, 3),
                new UIBorder(4, "Left", new Vector2(), BorderThickness, height, 4),
                new UIBorder(5, "TopLeft", new Vector2(), BorderThickness + GetItemById(Outlines, 1).Thickness * 2, BorderThickness + GetItemById(Outlines, 4).Thickness * 2, 5),
                new UIBorder(6, "TopRight", new Vector2(), BorderThickness + GetItemById(Outlines, 1).Thickness * 2, BorderThickness + GetItemById(Outlines, 2).Thickness * 2, 6),
                new UIBorder(7, "BottomLeft", new Vector2(), BorderThickness + GetItemById(Outlines, 3).Thickness * 2, BorderThickness + GetItemById(Outlines, 4).Thickness * 2, 7),
                new UIBorder(8, "BottomRight", new Vector2(), BorderThickness + GetItemById(Outlines, 3).Thickness * 2, BorderThickness + GetItemById(Outlines, 2).Thickness * 2, 8)
            };

            UpdateWindowProperties();
        }

        #region Buttons

        /// <summary>
        /// Add Button.
        /// </summary>
        /// <param name="name">The button's name. Intaken as a string.</param>
        /// <returns>Returns the button id of the newly added button as an int.</returns>
        public int AddButton(string name)
        {
            var nextButtonId = GetNextValidItemId(Buttons);

            var button = new UIButton(nextButtonId, name, new Vector2(ParentGroup.ParentManager.GetViewportDimenions().Width / 2f, ParentGroup.ParentManager.GetViewportDimenions().Height / 2f), Width, Height / 4, nextButtonId);
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

        /// <summary>
        /// Increase Button Order Number.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was increased.</returns>
        public bool IncreaseButtonOrderNumber(int buttonId)
        {
            return IncreaseItemOrderNumber(Buttons, buttonId);
        }

        /// <summary>
        /// Increase Button Order Number.
        /// </summary>
        /// <param name="buttonName">The name of the button to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was increased.</returns>
        public bool IncreaseButtonOrderNumber(string buttonName)
        {
            return IncreaseItemOrderNumber(Buttons, buttonName);
        }

        /// <summary>
        /// Decrease Button Order Number.
        /// </summary>
        /// <param name="buttonId">The id of the button to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the button's order number was decreased.</returns>
        public bool DecreaseButtonOrderNumber(int buttonId)
        {
            return DecreaseItemOrderNumber(Buttons, buttonId);
        }

        /// <summary>
        /// Decrease Button Order Number.
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
        /// <param name="name">The text's name. Intaken as a string.</param>
        /// <param name="font">The text's font. Intaken as a SpriteFont.</param>
        /// <param name="text">The text's text. Intaken as a string.</param>
        /// <returns>Returns the text id of the newly added text as an int.</returns>
        public int AddText(string name, SpriteFont font, string text)
        {
            var nextTextId = GetNextValidItemId(Texts);

            var newText = new UIText(nextTextId, name, font, text, nextTextId);
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
            return GetItemById(Texts, textId);
        }

        /// <summary>
        /// Get Text.
        /// </summary>
        /// <param name="textName">The name of the text to retrieve.</param>
        /// <returns>Returns a UIText with the requested name, if present, otherwise null.</returns>
        public UIText GetText(string textName)
        {
            return GetItemByName(Texts, textName);
        }

        /// <summary>
        /// Remove Text.
        /// </summary>
        /// <param name="textId">The id of the text to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text was removed.</returns>
        public bool RemoveText(int textId)
        {
            return RemoveItemById(Texts, textId);
        }

        /// <summary>
        /// Remove Text.
        /// </summary>
        /// <param name="textName">The name of the text to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text was removed.</returns>
        public bool RemoveText(string textName)
        {
            return RemoveItemByName(Texts, textName);
        }

        /// <summary>
        /// Increase Text Order Number.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was increased.</returns>
        public bool IncreaseTextOrderNumber(int textId)
        {
            return IncreaseItemOrderNumber(Texts, textId);
        }

        /// <summary>
        /// Increase Text Order Number.
        /// </summary>
        /// <param name="textName">The name of the text to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was increased.</returns>
        public bool IncreaseTextOrderNumber(string textName)
        {
            return IncreaseItemOrderNumber(Texts, textName);
        }

        /// <summary>
        /// Decrease Text Order Number.
        /// </summary>
        /// <param name="textId">The id of the text to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the text's order number was decreased.</returns>
        public bool DecreaseTextOrderNumber(int textId)
        {
            return DecreaseItemOrderNumber(Texts, textId);
        }

        /// <summary>
        /// Decrease Text Order Number.
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
        /// <param name="name">The menu's name. Intaken as a string.</param>
        /// <param name="width">The menu's width. Intaken as an int. Default is 100.</param>
        /// <param name="height">The menu's height. Intaken as an int. Default is 400.</param>
        /// <returns>Returns the menu id of the newly added menu as an int.</returns>
        public int AddMenu(string name, int width = 100, int height = 400)
        {
            var nextMenuId = GetNextValidItemId(Menus);

            var newMenu = new UIMenu(ParentGroup, nextMenuId, name, new Vector2(ParentGroup.ParentManager.GetViewportDimenions().Width / 2f, ParentGroup.ParentManager.GetViewportDimenions().Height / 2f), width, height, nextMenuId);
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
            return GetItemById(Menus, menuId);
        }

        /// <summary>
        /// Get Menu.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a UIMenu with the requested name, if present, otherwise null.</returns>
        public UIMenu GetMenu(string menuName)
        {
            return GetItemByName(Menus, menuName);
        }

        /// <summary>
        /// Remove Menu.
        /// </summary>
        /// <param name="menuId">The id of the menu to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu was removed.</returns>
        public bool RemoveMenu(int menuId)
        {
            return RemoveItemById(Menus, menuId);
        }

        /// <summary>
        /// Remove Menu.
        /// </summary>
        /// <param name="menuName">The name of the menu to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu was removed.</returns>
        public bool RemoveMenu(string menuName)
        {
            return RemoveItemByName(Menus, menuName);
        }

        /// <summary>
        /// Increase Menu Order Number.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was increased.</returns>
        public bool IncreaseMenuOrderNumber(int menuId)
        {
            return IncreaseItemOrderNumber(Menus, menuId);
        }

        /// <summary>
        /// Increase Menu Order Number.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was increased.</returns>
        public bool IncreaseMenuOrderNumber(string menuName)
        {
            return IncreaseItemOrderNumber(Menus, menuName);
        }

        /// <summary>
        /// Decrease Menu Order Number.
        /// </summary>
        /// <param name="menuId">The id of the menu to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was decreased.</returns>
        public bool DecreaseMenuOrderNumber(int menuId)
        {
            return DecreaseItemOrderNumber(Menus, menuId);
        }

        /// <summary>
        /// Decrease Menu Order Number.
        /// </summary>
        /// <param name="menuName">The name of the menu to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the menu's order number was decreased.</returns>
        public bool DecreaseMenuOrderNumber(string menuName)
        {
            return DecreaseItemOrderNumber(Menus, menuName);
        }

        #endregion

        /// <summary>
        /// Move Window Method.
        /// Used to move the window by detecting if the input device is present.
        /// </summary>
        /// <param name="deltas">The input device's deltas.</param>
        public void Move(Vector2 deltas)
        {
            if (IsMovable && GetItemByName(Borders, "Top").IsVisible)
            {
                Position = new Vector2(Position.X + deltas.X, Position.Y + deltas.Y);
            }
        }

        /// <summary>
        /// Update Windows Properties.
        /// </summary>
        private void UpdateWindowProperties()
        {
            //ContentsCamera.Viewport = new Viewport((int)Position.X - (Width / 2),
            //                                       (int)Position.Y - (Height / 2),
            //                                       Width,
            //                                       Height);

            #region Borders

            var border = GetItemByName(Borders, "Top");

            border.ParentPosition = Position;
            border.Position = new Vector2(0, -((Height / 2f) + BorderThickness - GetItemByName(border.Outlines, "Bottom").Thickness));
            border.Width = Width;
            border.Height = BorderThickness;

            border = GetItemByName(Borders, "Right");

            border.ParentPosition = Position;
            border.Position = new Vector2((Width / 2f) + BorderThickness - GetItemByName(border.Outlines, "Left").Thickness, 0);
            border.Width = BorderThickness;
            border.Height = Height;

            border = GetItemByName(Borders, "Bottom");

            border.ParentPosition = Position;
            border.Position = new Vector2(0, (Height / 2f) + BorderThickness - GetItemByName(border.Outlines, "Top").Thickness);
            border.Width = Width;
            border.Height = BorderThickness;

            border = GetItemByName(Borders, "Left");

            border.ParentPosition = Position;
            border.Position = new Vector2(-((Width / 2f) + BorderThickness - GetItemByName(border.Outlines, "Right").Thickness), 0);
            border.Width = BorderThickness;
            border.Height = Height;

            border = GetItemByName(Borders, "TopLeft");

            border.ParentPosition = Position;
            border.Position = new Vector2(-((Width / 2f) + BorderThickness - GetItemByName(border.Outlines, "Right").Thickness),
                                          -((Height / 2f) + BorderThickness - GetItemByName(border.Outlines, "Bottom").Thickness));
            border.Width = BorderThickness;
            border.Height = BorderThickness;

            border = GetItemByName(Borders, "TopRight");

            border.ParentPosition = Position;
            border.Position = new Vector2((Width / 2f) + BorderThickness - GetItemByName(border.Outlines, "Left").Thickness,
                                          -((Height / 2f) + BorderThickness - GetItemByName(border.Outlines, "Bottom").Thickness));
            border.Width = BorderThickness;
            border.Height = BorderThickness;

            border = GetItemByName(Borders, "BottomRight");

            border.ParentPosition = Position;
            border.Position = new Vector2((Width / 2f) + BorderThickness - GetItemByName(border.Outlines, "Left").Thickness,
                                          (Height / 2f) + BorderThickness - GetItemByName(border.Outlines, "Top").Thickness);
            border.Width = BorderThickness;
            border.Height = BorderThickness;

            border = GetItemByName(Borders, "BottomLeft");

            border.ParentPosition = Position;
            border.Position = new Vector2(-((Width / 2f) + BorderThickness - GetItemByName(border.Outlines, "Right").Thickness),
                                                    (Height / 2f) + BorderThickness - GetItemByName(border.Outlines, "Top").Thickness);
            border.Width = BorderThickness;
            border.Height = BorderThickness;

            #endregion
        }

        /// <summary>
        /// Calculate Extended Rectangle.
        /// Calculates an extended Rectangle that can be used for detection.
        /// </summary>
        private void CalculateExtendedRectangle()
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

            ExtendedRectangle = rectangle;
        }

        /// <summary>
        /// Set Window Transparency.
        /// </summary>
        /// <param name="transparencyLevel">Intakes a float to define the transparency level.</param>
        public void SetWindowTransparency(float transparencyLevel)
        {
            foreach (var border in Borders)
            {
                border.Transparencies["Background"] = transparencyLevel;
            }

            Transparencies["Background"] = transparencyLevel;
        }

        /// <summary>
        /// Draw Contents.
        /// Draws UIWindow Elements and Text.
        /// </summary>
        public void DrawContents(SpriteBatch contentsSpriteBatch, Viewport parentViewport)
        {
            contentsSpriteBatch.GraphicsDevice.Viewport = ContentsCamera.Viewport;

            contentsSpriteBatch.Begin(transformMatrix: ContentsCamera.Matrix);

            foreach (var border in Borders.OrderBy(border => border.OrderNumber))
            {
                border.IsVisible = IsVisible;
                border.Transparencies["Background"] = Transparencies["Background"];
                border.Draw(contentsSpriteBatch);
            }

            foreach (var button in Buttons.OrderBy(button => button.OrderNumber))
            {
                if (button.GetText() != null)
                {
                    button.GetText().IsVisible = IsVisible;
                    button.GetText().ApplyEnhancedTextScaling(button.Rectangle, true);
                }

                button.IsVisible = IsVisible;
                button.Transparencies["Background"] = Transparencies["Background"];
                button.Draw(contentsSpriteBatch);
            }

            foreach (var text in Texts.OrderBy(text => text.OrderNumber))
            {
                text.IsVisible = IsVisible;
                text.Transparencies["Background"] = Transparencies["Background"];
                text.ApplyEnhancedTextWrap(ContentsCamera.WorldViewRectangle);
                text.Draw(contentsSpriteBatch);
            }

            contentsSpriteBatch.End();

            // Switch back to Parent Viewport.
            contentsSpriteBatch.GraphicsDevice.Viewport = parentViewport;
        }

        /// <summary>
        /// Load Content.
        /// </summary>
        public override void LoadContent()
        {
            foreach (var border in Borders)
            {
                border.LoadContent();
            }

            base.LoadContent();
        }

        /// <summary>
        /// UIWindow Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            UpdateWindowProperties();
            SetWindowTransparency(Transparencies["Background"]);

            foreach (var border in Borders)
            {
                await border.Update(gameTime);
            }

            foreach (var text in Texts.OrderBy(text => text.OrderNumber))
            {
                for (var index = 0; index < ActiveInputDevices.Count; index++)
                {
                    var inputDevice = ActiveInputDevices[index];
                    text.CheckIsInFocus(new Rectangle((int)ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                      (int)ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                      inputDevice.Width,
                                                      inputDevice.Height));
                }

                await text.Update(gameTime);
            }

            foreach (var button in Buttons.OrderBy(button => button.OrderNumber))
            {
                for (var index = 0; index < ActiveInputDevices.Count; index++)
                {
                    var inputDevice = ActiveInputDevices[index];
                    button.CheckIsInFocus(new Rectangle((int)ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                        (int)ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                        inputDevice.Width,
                                                        inputDevice.Height));
                }

                await button.Update(gameTime);
            }

            foreach (var border in Borders.OrderBy(border => border.OrderNumber))
            {
                for (var index = 0; index < ActiveInputDevices.Count; index++)
                {
                    var inputDevice = ActiveInputDevices[index];
                    border.CheckIsInFocus(new Rectangle((int)ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                        (int)ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                        inputDevice.Width,
                                                        inputDevice.Height));
                }

                await border.Update(gameTime);
            }

            foreach (var menu in Menus.OrderBy(menu => menu.OrderNumber))
            {
                for (var index = 0; index < ActiveInputDevices.Count; index++)
                {
                    var inputDevice = ActiveInputDevices[index];
                    menu.CheckIsInFocus(new Rectangle((int)ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                      (int)ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                      inputDevice.Width,
                                                      inputDevice.Height));
                }

                await menu.Update(gameTime);
            }

            await base.Update(gameTime);

            CalculateExtendedRectangle();

            ContentsCamera.Update(gameTime);
            ActiveInputDevices.Clear();
        }

        /// <summary>
        /// UIWindow Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (var border in Borders)
            {
                border.Draw(spriteBatch);
            }
        }
    }
}