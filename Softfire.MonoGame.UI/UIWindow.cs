using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.IO;

namespace Softfire.MonoGame.UI
{
    public class UIWindow : UIBase
    {
        /// <summary>
        /// Parent Group.
        /// </summary>
        internal UIGroup ParentGroup { private get; set; }

        /// <summary>
        /// TitleBar.
        /// </summary>
        public UIBorder TitleBar { get; }

        /// <summary>
        /// Has Borders?
        /// </summary>
        public bool HasBorders { get; set; }

        /// <summary>
        /// Border Color.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// UIWindow Border Thickness.
        /// </summary>
        public int BorderThickness { get; set; }

        /// <summary>
        /// Top Border.
        /// </summary>
        public UIBorder TopBorder { get; }

        /// <summary>
        /// Bottom Border.
        /// </summary>
        public UIBorder BottomBorder { get; }

        /// <summary>
        /// Left Border.
        /// </summary>
        public UIBorder LeftBorder { get; }

        /// <summary>
        /// Right Border.
        /// </summary>
        public UIBorder RightBorder { get; }

        /// <summary>
        /// Top Left Corner.
        /// </summary>
        public UIBorder TopLeftCorner { get; }

        /// <summary>
        /// Top Right Corner.
        /// </summary>
        public UIBorder TopRightCorner { get; }

        /// <summary>
        /// Bottom Left Corner.
        /// </summary>
        public UIBorder BottomLeftCorner { get; }

        /// <summary>
        /// Bottom Right Corner.
        /// </summary>
        public UIBorder BottomRightCorner { get; }

        /// <summary>
        /// UIWindow Active input Devices.
        /// </summary>
        public List<Rectangle> ActiveInputDevices { private get; set; }

        /// <summary>
        /// UIButtons.
        /// Use AddButton, RemoveButton and GetButton to modify UI Buttons.
        /// </summary>
        public Dictionary<int, UIButton> UIButtons { get; }

        /// <summary>
        /// UIBorders.
        /// Use AddBorder, RemoveBorder and GetBorder to modify UI Borders.
        /// </summary>
        public Dictionary<int, UIBorder> UIBorders { get; }

        /// <summary>
        /// UITexts.
        /// Use AddText, RemoveText and GetText to modify UI Texts.
        /// </summary>
        public Dictionary<int, UIText> UITexts { get; }

        /// <summary>
        /// Menu.
        /// </summary>
        public UIMenu Menu { get; private set; }

        /// <summary>
        /// UIWindows Camera2D.
        /// Used to scroll the view area of the window.
        /// </summary>
        public IOCamera2D ContentsCamera { get; }

        /// <summary>
        /// Window Rectangle
        /// A rectangle of the UIWindow including Borders and TitleBar, if they are enabled.
        /// </summary>
        public Rectangle WindowRectangle { get; private set; }

        /// <summary>
        /// UI Window Constructor.
        /// </summary>
        /// <param name="position">Intakes the UI's position as a Vector2.</param>
        /// <param name="width">Intakes the UI's width as a float.</param>
        /// <param name="height">Intakes the UI's height as a float.</param>
        /// <param name="orderNumber">Intakes an int that will be used to define the update/draw order. Update/Draw order is from lowest to highest.</param>
        /// <param name="color">Intakes the UI's base color as Color.</param>
        /// <param name="worldRectangle">Intakes a Rectangle describing the extended view of the UIWindow. Used by Camera2D.</param>
        /// <param name="hasBorders">Intakes a boolean indicating if the Window has Borders.</param>
        /// <param name="hasOutlines">Intakes a boolean indicating if the Window and it's Borders are outlined.</param>
        /// <param name="borderColor">Intakes the UI's border Color as a Color.</param>
        /// <param name="borderThickness">Intakes the UI's border thickness as an int.</param>
        /// <param name="isMovable">Intakes a boolean defining if the Window can be moved. Default is false.</param>
        /// <param name="textureFilePath">Intakes a texture's file path as a string.</param>
        /// <param name="isVisible">Indicates whether the UIBase is visible. Intaken as a bool.</param>
        public UIWindow(Vector2 position, int width, int height, int orderNumber, Color? color = null,
                                                                                  Rectangle? worldRectangle = null,
                                                                                  bool hasBorders = true,
                                                                                  bool hasOutlines = true,
                                                                                  Color? borderColor = null,
                                                                                  int borderThickness = 10,
                                                                                  bool isMovable = false,
                                                                                  string textureFilePath = null,
                                                                                  bool isVisible = false) : base(position, width, height, orderNumber, color, textureFilePath, isVisible)
        {
            HasBorders = hasBorders;
            HasOutlines = hasOutlines;
            BorderThickness = borderThickness;
            BorderColor = borderColor ?? Color.LightGray;
            IsMovable = isMovable;

            ContentsCamera = new IOCamera2D(new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height), worldRectangle ?? new Rectangle(0, 0, width, height));

            ActiveInputDevices = new List<Rectangle>();

            UIButtons = new Dictionary<int, UIButton>();
            UIBorders = new Dictionary<int, UIBorder>();
            UITexts = new Dictionary<int, UIText>();

            TopBorder = new UIBorder(new Vector2(), width, BorderThickness, orderNumber, BorderColor);
            RightBorder = new UIBorder(new Vector2(), BorderThickness, height, orderNumber, BorderColor);
            BottomBorder = new UIBorder(new Vector2(), width, BorderThickness, orderNumber, BorderColor);
            LeftBorder = new UIBorder(new Vector2(), BorderThickness, height, orderNumber, BorderColor);

            TitleBar = new UIBorder(new Vector2(), width + BorderThickness * 2 + OutlineThickness * 4, BorderThickness * 2, orderNumber, color);

            TopLeftCorner = new UIBorder(new Vector2(), BorderThickness + OutlineThickness * 2, BorderThickness + OutlineThickness * 2, orderNumber, BorderColor);
            TopRightCorner = new UIBorder(new Vector2(), BorderThickness + OutlineThickness * 2, BorderThickness + OutlineThickness * 2, orderNumber, BorderColor);
            BottomLeftCorner = new UIBorder(new Vector2(), BorderThickness + OutlineThickness * 2, BorderThickness + OutlineThickness * 2, orderNumber, BorderColor);
            BottomRightCorner = new UIBorder(new Vector2(), BorderThickness + OutlineThickness * 2, BorderThickness + OutlineThickness * 2, orderNumber, BorderColor);

            UpdateWindowProperties(position, width, height);

            #region Outline Settings

            OutlinesDictionary[Outlines.Top] = true;
            OutlinesDictionary[Outlines.Right] = true;
            OutlinesDictionary[Outlines.Bottom] = true;
            OutlinesDictionary[Outlines.Left] = true;

            TitleBar.OutlinesDictionary[Outlines.Top] = true;
            TitleBar.OutlinesDictionary[Outlines.Right] = true;
            TitleBar.OutlinesDictionary[Outlines.Bottom] = true;
            TitleBar.OutlinesDictionary[Outlines.Left] = true;

            TopLeftCorner.OutlinesDictionary[Outlines.Top] = true;
            TopLeftCorner.OutlinesDictionary[Outlines.Right] = false;
            TopLeftCorner.OutlinesDictionary[Outlines.Bottom] = false;
            TopLeftCorner.OutlinesDictionary[Outlines.Left] = true;

            TopBorder.OutlinesDictionary[Outlines.Top] = true;
            TopBorder.OutlinesDictionary[Outlines.Right] = false;
            TopBorder.OutlinesDictionary[Outlines.Bottom] = true;
            TopBorder.OutlinesDictionary[Outlines.Left] = false;

            TopRightCorner.OutlinesDictionary[Outlines.Top] = true;
            TopRightCorner.OutlinesDictionary[Outlines.Right] = true;
            TopRightCorner.OutlinesDictionary[Outlines.Bottom] = false;
            TopRightCorner.OutlinesDictionary[Outlines.Left] = false;

            RightBorder.OutlinesDictionary[Outlines.Top] = false;
            RightBorder.OutlinesDictionary[Outlines.Right] = true;
            RightBorder.OutlinesDictionary[Outlines.Bottom] = false;
            RightBorder.OutlinesDictionary[Outlines.Left] = true;

            BottomRightCorner.OutlinesDictionary[Outlines.Top] = false;
            BottomRightCorner.OutlinesDictionary[Outlines.Right] = true;
            BottomRightCorner.OutlinesDictionary[Outlines.Bottom] = true;
            BottomRightCorner.OutlinesDictionary[Outlines.Left] = false;

            BottomBorder.OutlinesDictionary[Outlines.Top] = true;
            BottomBorder.OutlinesDictionary[Outlines.Right] = false;
            BottomBorder.OutlinesDictionary[Outlines.Bottom] = true;
            BottomBorder.OutlinesDictionary[Outlines.Left] = false;

            BottomLeftCorner.OutlinesDictionary[Outlines.Top] = false;
            BottomLeftCorner.OutlinesDictionary[Outlines.Right] = false;
            BottomLeftCorner.OutlinesDictionary[Outlines.Bottom] = true;
            BottomLeftCorner.OutlinesDictionary[Outlines.Left] = true;

            LeftBorder.OutlinesDictionary[Outlines.Top] = false;
            LeftBorder.OutlinesDictionary[Outlines.Right] = true;
            LeftBorder.OutlinesDictionary[Outlines.Bottom] = false;
            LeftBorder.OutlinesDictionary[Outlines.Left] = true;

            #endregion
        }

        #region Buttons

        /// <summary>
        /// Add Button.
        /// Adds a Button, if it does not already exist.
        /// </summary>
        /// <param name="button">Intakes a new UIButton.</param>
        /// <returns>Returns a boolean indicating if the Button was added.</returns>
        public bool AddButton(UIButton button)
        {
            var result = false;

            if (UIButtons.ContainsKey(button.OrderNumber) == false)
            {
                button.LoadContent();
                UIButtons.Add(button.OrderNumber, button);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Remove Button.
        /// Removes the named Button, if it is present.
        /// </summary>
        /// <param name="buttonOrderNumber">The button's order number as an int.</param>
        /// <returns>Returns a boolean indicating if the Button was removed.</returns>
        public bool RemoveButton(int buttonOrderNumber)
        {
            var result = false;

            if (UIButtons.ContainsKey(buttonOrderNumber))
            {
                result = UIButtons.Remove(buttonOrderNumber);
            }

            return result;
        }

        /// <summary>
        /// Get Button.
        /// Get a specific Button by it's identifier, if it exists.
        /// </summary>
        /// <param name="buttonOrderNumber">The button's order number as an int.</param>
        /// <returns>Returns the Button with the specified identifier, if present, otherwise null.</returns>
        public UIButton GetButton(int buttonOrderNumber)
        {
            UIButton result = null;

            if (UIButtons.ContainsKey(buttonOrderNumber))
            {
                result = UIButtons[buttonOrderNumber];
            }

            return result;
        }

        #endregion

        #region Borders

        /// <summary>
        /// Add Border.
        /// Adds a Border, if it does not already exist.
        /// </summary>
        /// <param name="border">Intakes a new UIBorder.</param>
        /// <returns>Returns a boolean indicating if the Button was added.</returns>
        public bool AddBorder(UIBorder border)
        {
            var result = false;

            if (UIBorders.ContainsKey(border.OrderNumber) == false)
            {
                border.LoadContent();
                UIBorders.Add(border.OrderNumber, border);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Remove Border.
        /// Removes the named Border, if it is present.
        /// </summary>
        /// <param name="borderOrderNumber">The border's order number as an int.</param>
        /// <returns>Returns a boolean indicating if the Border was removed.</returns>
        public bool RemoveBorder(int borderOrderNumber)
        {
            var result = false;

            if (UIBorders.ContainsKey(borderOrderNumber))
            {
                result = UIBorders.Remove(borderOrderNumber);
            }

            return result;
        }

        /// <summary>
        /// Get Border.
        /// Get a specific Border by it's identifier, if it exists.
        /// </summary>
        /// <param name="borderOrderNumber">The border's order number as an int.</param>
        /// <returns>Returns the Border with the specified identifier, if present, otherwise null.</returns>
        public UIBorder GetBorder(int borderOrderNumber)
        {
            UIBorder result = null;

            if (UIBorders.ContainsKey(borderOrderNumber))
            {
                result = UIBorders[borderOrderNumber];
            }

            return result;
        }

        #endregion

        #region Texts

        /// <summary>
        /// Add Text.
        /// Adds Text, if it does not already exist.
        /// </summary>
        /// <param name="text">Intakes a new UI Text with a base class of UIBase.</param>
        /// <returns>Returns a boolean indicating if the Text was added.</returns>
        public bool AddText(UIText text)
        {
            var result = false;

            if (UITexts.ContainsKey(text.OrderNumber) == false)
            {
                text.LoadContent();
                UITexts.Add(text.OrderNumber, text);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Remove Text.
        /// Removes the named Text, if it is present.
        /// </summary>
        /// <param name="textOrderNumber">The text's order number as an int.</param>
        /// <returns>Returns a boolean indicating if the Text was removed.</returns>
        public bool RemoveText(int textOrderNumber)
        {
            var result = false;

            if (UITexts.ContainsKey(textOrderNumber))
            {
                result = UITexts.Remove(textOrderNumber);
            }

            return result;
        }

        /// <summary>
        /// Get Text.
        /// Get a specific Text by it's identifier, if it exists.
        /// </summary>
        /// <param name="textOrderNumber">The text's order number as an int.</param>
        /// <returns>Returns the Text with the specified identifier, if present, otherwise null.</returns>
        public UIText GetText(int textOrderNumber)
        {
            UIText result = null;

            if (UITexts.ContainsKey(textOrderNumber))
            {
                result = UITexts[textOrderNumber];
            }

            return result;
        }

        #endregion

        #region Menus

        /// <summary>
        /// Add Menu.
        /// Adds a new Menu.
        /// </summary>
        /// <returns>Returns a boolean indicating if the Menu was added.</returns>
        public bool AddMenu()
        {
            var result = false;

            if (Menu == null)
            {
                Menu = new UIMenu(Vector2.Zero, 1)
                {
                    ParentGroup = ParentGroup
                };

                Menu.LoadContent();

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Menu.
        /// </summary>
        /// <returns>Returns the Menu, if present, otherwise null.</returns>
        public UIMenu GetMenu()
        {
            UIMenu result = null;

            if (Menu != null)
            {
                result = Menu;
            }

            return result;
        }

        /// <summary>
        /// Remove Menu.
        /// Removes the Menu, if present.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the Menu was removed.</returns>
        public bool RemoveMenu()
        {
            var result = false;

            if (Menu != null)
            {
                Menu = null;
                result = true;
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Move Window Method.
        /// Used to move the window by detecting if the input device is present.
        /// </summary>
        /// <param name="inputRectangle">Intakes an Input Device's Rectangle.</param>
        public void Move(Rectangle inputRectangle)
        {
            if (IsMovable && TitleBar.IsVisible)
            {
                Position += new Vector2(inputRectangle.X - Position.X - TitleBar.Position.X, inputRectangle.Y - Position.Y - TitleBar.Position.Y);
            }
            else if (IsMovable && TopBorder.IsVisible)
            {
                Position += new Vector2(inputRectangle.X - Position.X - TopBorder.Position.X, inputRectangle.Y - Position.Y - TopBorder.Position.Y);
            }
        }

        /// <summary>
        /// Update Windows Properties.
        /// </summary>
        /// <param name="parentPosition">Intakes the Parent's Position as a Vector2.</param>
        /// <param name="parentWidth">Intakes the Parent's Width as an int.</param>
        /// <param name="parentHeight">Intakes the Parent's Height as an int.</param>
        private void UpdateWindowProperties(Vector2 parentPosition, int parentWidth, int parentHeight)
        {
            ContentsCamera.Viewport = new Viewport((int)parentPosition.X - (parentWidth / 2),
                                                   (int)parentPosition.Y - (parentHeight / 2),
                                                   parentWidth,
                                                   parentHeight);
            if (HasBorders)
            {
                TopBorder.IsVisible = IsVisible;
                RightBorder.IsVisible = IsVisible;
                BottomBorder.IsVisible = IsVisible;
                LeftBorder.IsVisible = IsVisible;
                TopLeftCorner.IsVisible = IsVisible;
                TopRightCorner.IsVisible = IsVisible;
                BottomRightCorner.IsVisible = IsVisible;
                BottomLeftCorner.IsVisible = IsVisible;
            }
            else
            {
                TopBorder.IsVisible = false;
                RightBorder.IsVisible = false;
                BottomBorder.IsVisible = false;
                LeftBorder.IsVisible = false;
                TopLeftCorner.IsVisible = false;
                TopRightCorner.IsVisible = false;
                BottomRightCorner.IsVisible = false;
                BottomLeftCorner.IsVisible = false;
            }

            if (HasOutlines)
            {
                TopBorder.HasOutlines = HasOutlines;
                TopBorder.ParentPosition = parentPosition;
                TopBorder.Position = new Vector2(0, -(((parentHeight + BorderThickness) / 2f) + (TopBorder.OutlineThickness * 4)));
                TopBorder.Width = parentWidth;
                TopBorder.Height = BorderThickness;

                RightBorder.HasOutlines = HasOutlines;
                RightBorder.ParentPosition = parentPosition;
                RightBorder.Position = new Vector2(((parentWidth + BorderThickness) / 2f) + (RightBorder.OutlineThickness * 4), 0);
                RightBorder.Width = BorderThickness;
                RightBorder.Height = parentHeight;

                BottomBorder.HasOutlines = HasOutlines;
                BottomBorder.ParentPosition = parentPosition;
                BottomBorder.Position = new Vector2(0, ((parentHeight + BorderThickness) / 2f) + (BottomBorder.OutlineThickness * 4));
                BottomBorder.Width = parentWidth;
                BottomBorder.Height = BorderThickness;

                LeftBorder.HasOutlines = HasOutlines;
                LeftBorder.ParentPosition = parentPosition;
                LeftBorder.Position = new Vector2(-(((parentWidth + BorderThickness) / 2f) + (LeftBorder.OutlineThickness * 4)), 0);
                LeftBorder.Width = BorderThickness;
                LeftBorder.Height = parentHeight;

                TitleBar.HasOutlines = HasOutlines;
                TitleBar.ParentPosition = parentPosition;
                TitleBar.Position = new Vector2(0, -(((parentHeight / 2f) + BorderThickness * 2f) + (TitleBar.OutlineThickness * 8)));
                TitleBar.Width = (parentWidth + (BorderThickness * 2)) + (TitleBar.OutlineThickness * 8);
                TitleBar.Height = BorderThickness * 2;

                TopLeftCorner.HasOutlines = HasOutlines;
                TopLeftCorner.ParentPosition = parentPosition;
                TopLeftCorner.Position = new Vector2(-((parentWidth + BorderThickness) / 2f + (TopLeftCorner.OutlineThickness * 3)), -((parentHeight + BorderThickness) / 2f + (TopLeftCorner.OutlineThickness * 3)));
                TopLeftCorner.Width = BorderThickness + (OutlineThickness * 2);
                TopLeftCorner.Height = BorderThickness + (OutlineThickness * 2);

                TopRightCorner.HasOutlines = HasOutlines;
                TopRightCorner.ParentPosition = parentPosition;
                TopRightCorner.Position = new Vector2((parentWidth + BorderThickness) / 2f + (TopRightCorner.OutlineThickness * 3), -((parentHeight + BorderThickness) / 2f + (TopRightCorner.OutlineThickness * 3)));
                TopRightCorner.Width = BorderThickness + (OutlineThickness * 2);
                TopRightCorner.Height = BorderThickness + (OutlineThickness * 2);

                BottomRightCorner.HasOutlines = HasOutlines;
                BottomRightCorner.ParentPosition = parentPosition;
                BottomRightCorner.Position = new Vector2((parentWidth + BorderThickness) / 2f + (BottomRightCorner.OutlineThickness * 3), (parentHeight + BorderThickness) / 2f + (BottomRightCorner.OutlineThickness * 3));
                BottomRightCorner.Width = BorderThickness + (OutlineThickness * 2);
                BottomRightCorner.Height = BorderThickness + (OutlineThickness * 2);

                BottomLeftCorner.HasOutlines = HasOutlines;
                BottomLeftCorner.ParentPosition = parentPosition;
                BottomLeftCorner.Position = new Vector2(-((parentWidth + BorderThickness) / 2f + (BottomLeftCorner.OutlineThickness * 3)), (parentHeight + BorderThickness) / 2f + (BottomLeftCorner.OutlineThickness * 3));
                BottomLeftCorner.Width = BorderThickness + (OutlineThickness * 2);
                BottomLeftCorner.Height = BorderThickness + (OutlineThickness * 2);
            }
            else
            {
                TopBorder.HasOutlines = HasOutlines;
                TopBorder.ParentPosition = parentPosition;
                TopBorder.Position = new Vector2(0, -(((parentHeight + BorderThickness) / 2f) + 2));
                TopBorder.Width = parentWidth;
                TopBorder.Height = BorderThickness;

                RightBorder.HasOutlines = HasOutlines;
                RightBorder.ParentPosition = parentPosition;
                RightBorder.Position = new Vector2(((parentWidth + BorderThickness) / 2f) + 2, 0);
                RightBorder.Width = BorderThickness;
                RightBorder.Height = parentHeight;

                BottomBorder.HasOutlines = HasOutlines;
                BottomBorder.ParentPosition = parentPosition;
                BottomBorder.Position = new Vector2(0, ((parentHeight + BorderThickness) / 2f) + 2);
                BottomBorder.Width = parentWidth;
                BottomBorder.Height = BorderThickness;

                LeftBorder.HasOutlines = HasOutlines;
                LeftBorder.ParentPosition = parentPosition;
                LeftBorder.Position = new Vector2(-(((parentWidth + BorderThickness) / 2f) + 2), 0);
                LeftBorder.Width = BorderThickness;
                LeftBorder.Height = parentHeight;

                TitleBar.HasOutlines = HasOutlines;
                TitleBar.ParentPosition = parentPosition;
                TitleBar.Position = new Vector2(0, -(parentHeight / 2f + BorderThickness * 2f + 4));
                TitleBar.Width = parentWidth + (BorderThickness * 2) + 4;
                TitleBar.Height = BorderThickness * 2;

                TopLeftCorner.HasOutlines = HasOutlines;
                TopLeftCorner.ParentPosition = parentPosition;
                TopLeftCorner.Position = new Vector2(-(((parentWidth + BorderThickness) / 2f) + 2), -(((parentHeight + BorderThickness) / 2f) + 2));
                TopLeftCorner.Width = BorderThickness;
                TopLeftCorner.Height = BorderThickness;

                TopRightCorner.HasOutlines = HasOutlines;
                TopRightCorner.ParentPosition = parentPosition;
                TopRightCorner.Position = new Vector2(((parentWidth + BorderThickness) / 2f) + 2, -(((parentHeight + BorderThickness) / 2f) + 2));
                TopRightCorner.Width = BorderThickness;
                TopRightCorner.Height = BorderThickness;

                BottomRightCorner.HasOutlines = HasOutlines;
                BottomRightCorner.ParentPosition = parentPosition;
                BottomRightCorner.Position = new Vector2(((parentWidth + BorderThickness) / 2f) + 2, ((parentHeight + BorderThickness) / 2f) + 2);
                BottomRightCorner.Width = BorderThickness;
                BottomRightCorner.Height = BorderThickness;

                BottomLeftCorner.HasOutlines = HasOutlines;
                BottomLeftCorner.ParentPosition = parentPosition;
                BottomLeftCorner.Position = new Vector2(-(((parentWidth + BorderThickness) / 2f) + 2), ((parentHeight + BorderThickness) / 2f) + 2);
                BottomLeftCorner.Width = BorderThickness;
                BottomLeftCorner.Height = BorderThickness;
            }

            if (Menu != null)
            {
                Menu.ParentPosition = parentPosition + Position;
            }
        }

        /// <summary>
        /// Calculate Window Rectangle.
        /// Calculates an extended Rectangle to that can be used for detection.
        /// Calculated with or without outlines, borders and TitleBar.
        /// </summary>
        /// <returns></returns>
        private void CalculateWindowRectangle()
        {
            var rectangle = Rectangle;

            if (HasOutlines)
            {
                if (TitleBar.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X, rectangle.Y - TitleBar.Height - TitleBar.OutlineThickness * 4, rectangle.Width, rectangle.Height + TitleBar.Height + TitleBar.OutlineThickness * 3);
                }

                if (TopBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X, rectangle.Y - TopBorder.Height - TopBorder.OutlineThickness * 4, rectangle.Width, rectangle.Height + TopBorder.Height + TopBorder.OutlineThickness * 4);
                }

                if (RightBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + RightBorder.Width + RightBorder.OutlineThickness * 4, rectangle.Height);
                }

                if (BottomBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height + BottomBorder.Height + BottomBorder.OutlineThickness * 4);
                }

                if (LeftBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X - LeftBorder.Width - LeftBorder.OutlineThickness * 4, rectangle.Y, rectangle.Width + LeftBorder.Width + LeftBorder.OutlineThickness * 4, rectangle.Height);
                }

                if (IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X - OutlineThickness, rectangle.Y - OutlineThickness, rectangle.Width + OutlineThickness, rectangle.Height + OutlineThickness);
                }
            }
            else if (HasOutlines == false &&
                     TitleBar.IsVisible)
            {
                rectangle = new Rectangle(rectangle.X, rectangle.Y - TitleBar.Height, rectangle.Width, rectangle.Height + TitleBar.Height);

                if (TopBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X, rectangle.Y - TopBorder.Height, rectangle.Width, rectangle.Height + TopBorder.Height);
                }

                if (RightBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + RightBorder.Width, rectangle.Height);
                }

                if (BottomBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height + BottomBorder.Height);
                }

                if (LeftBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X - LeftBorder.Width, rectangle.Y, rectangle.Width + LeftBorder.Width, rectangle.Height);
                }
            }
            else if (HasOutlines == false &&
                     TitleBar.IsVisible == false)
            {
                if (TopBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X, rectangle.Y - TopBorder.Height, rectangle.Width, rectangle.Height + TopBorder.Height);
                }

                if (RightBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + RightBorder.Width, rectangle.Height);
                }

                if (BottomBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height + BottomBorder.Height);
                }

                if (LeftBorder.IsVisible)
                {
                    rectangle = new Rectangle(rectangle.X - LeftBorder.Width, rectangle.Y, rectangle.Width + LeftBorder.Width, rectangle.Height);
                }
            }
            
            WindowRectangle = rectangle;
        }

        /// <summary>
        /// Set Window Transparency.
        /// </summary>
        /// <param name="transparencyLevel">Intakes a float to define the transparency level of all borders, titlebar and base.</param>
        public void SetWindowTransparency(float transparencyLevel)
        {
            TitleBar.Transparency = transparencyLevel;
            TopLeftCorner.Transparency = transparencyLevel;
            TopBorder.Transparency = transparencyLevel;
            TopRightCorner.Transparency = transparencyLevel;
            RightBorder.Transparency = transparencyLevel;
            BottomRightCorner.Transparency = transparencyLevel;
            BottomBorder.Transparency = transparencyLevel;
            BottomLeftCorner.Transparency = transparencyLevel;
            LeftBorder.Transparency = transparencyLevel;
            Transparency = transparencyLevel;

            if (Menu != null)
            {
                Menu.Transparency = Transparency;
            }
        }

        /// <summary>
        /// Draw Contents.
        /// Draws UIWindow Elements and Text.
        /// </summary>
        public void DrawContents(SpriteBatch contentsSpriteBatch, Viewport parentViewport)
        {
            contentsSpriteBatch.GraphicsDevice.Viewport = ContentsCamera.Viewport;

            contentsSpriteBatch.Begin(transformMatrix: ContentsCamera.Matrix);

            foreach (var border in UIBorders.OrderBy(border => border.Value.OrderNumber))
            {
                border.Value.IsVisible = IsVisible;
                border.Value.Transparency = Transparency;
                border.Value.Draw(contentsSpriteBatch);
            }

            foreach (var button in UIButtons.OrderBy(button => button.Value.OrderNumber))
            {
                button.Value.Text.ApplyEnhancedTextScaling(button.Value.Rectangle, true);
                button.Value.IsVisible = IsVisible;
                button.Value.Transparency = Transparency;
                button.Value.Draw(contentsSpriteBatch);
            }

            foreach (var text in UITexts.OrderBy(text => text.Value.OrderNumber))
            {
                text.Value.IsVisible = IsVisible;
                text.Value.Transparency = Transparency;
                text.Value.ApplyEnhancedTextWrap(ContentsCamera.WorldViewRectangle);
                text.Value.Draw(contentsSpriteBatch);
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
            TopBorder.LoadContent();
            RightBorder.LoadContent();
            BottomBorder.LoadContent();
            LeftBorder.LoadContent();
            TitleBar.LoadContent();
            TopLeftCorner.LoadContent();
            TopRightCorner.LoadContent();
            BottomLeftCorner.LoadContent();
            BottomRightCorner.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// UIWindow Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            UpdateWindowProperties(Position, Width, Height);
            SetWindowTransparency(Transparency);

            await TitleBar.Update(gameTime);
            await TopLeftCorner.Update(gameTime);
            await TopBorder.Update(gameTime);
            await TopRightCorner.Update(gameTime);
            await RightBorder.Update(gameTime);
            await BottomRightCorner.Update(gameTime);
            await BottomBorder.Update(gameTime);
            await BottomLeftCorner.Update(gameTime);
            await LeftBorder.Update(gameTime);

            foreach (var text in UITexts.OrderBy(text => text.Value.OrderNumber))
            {
                for (var index = 0; index < ActiveInputDevices.Count; index++)
                {
                    var inputDevice = ActiveInputDevices[index];
                    text.Value.CheckIsInFocus(new Rectangle((int) ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                            (int) ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                            inputDevice.Width,
                                                            inputDevice.Height));
                }

                await text.Value.Update(gameTime);
            }

            foreach (var button in UIButtons.OrderBy(button => button.Value.OrderNumber))
            {
                for (var index = 0; index < ActiveInputDevices.Count; index++)
                {
                    var inputDevice = ActiveInputDevices[index];
                    button.Value.CheckIsInFocus(new Rectangle((int) ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                              (int) ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                              inputDevice.Width,
                                                              inputDevice.Height));
                }

                await button.Value.Update(gameTime);
            }

            foreach (var border in UIBorders.OrderBy(border => border.Value.OrderNumber))
            {
                for (var index = 0; index < ActiveInputDevices.Count; index++)
                {
                    var inputDevice = ActiveInputDevices[index];
                    border.Value.CheckIsInFocus(new Rectangle((int) ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                              (int) ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                              inputDevice.Width,
                                                              inputDevice.Height));
                }

                await border.Value.Update(gameTime);
            }

            if (Menu != null)
            {
                for (var index = 0; index < ActiveInputDevices.Count; index++)
                {
                    var inputDevice = ActiveInputDevices[index];
                    Menu.CheckIsInFocus(new Rectangle((int)ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                      (int)ContentsCamera.GetScreenPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                      inputDevice.Width,
                                                      inputDevice.Height));
                }

                await Menu.Update(gameTime);
            }

            await base.Update(gameTime);

            CalculateWindowRectangle();

            ContentsCamera.Update(gameTime);
            ActiveInputDevices.Clear();
        }

        /// <summary>
        /// UIWindow Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            TopLeftCorner.Draw(spriteBatch);
            TopRightCorner.Draw(spriteBatch);
            BottomRightCorner.Draw(spriteBatch);
            BottomLeftCorner.Draw(spriteBatch);
            
            base.Draw(spriteBatch);
            
            TopBorder.Draw(spriteBatch);
            RightBorder.Draw(spriteBatch);
            BottomBorder.Draw(spriteBatch);
            LeftBorder.Draw(spriteBatch);
            
            TitleBar.Draw(spriteBatch);
        }
    }
}