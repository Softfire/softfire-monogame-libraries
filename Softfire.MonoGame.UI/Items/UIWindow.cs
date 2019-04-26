using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE;
using Softfire.MonoGame.CORE.Common;
using Softfire.MonoGame.CORE.Input;
using Softfire.MonoGame.IO;
using System.Linq;

namespace Softfire.MonoGame.UI.Items
{
    /// <summary>
    /// A UI window for displaying text, buttons, menus and <see cref="MonoGameObject"/>s.
    /// </summary>
    public class UIWindow : UIBase
    {
        /// <summary>
        /// The window's camera.
        /// </summary>
        public IOCamera2D Camera { get; }

        /// <summary>
        /// The <see cref="RasterizerState"/> for drawing contents.
        /// </summary>
        private RasterizerState Rasterizer { get; }

        /// <summary>
        /// The border thickness for the window.
        /// </summary>
        private int BorderThickness { get; }

        /// <summary>
        /// The window's layers.
        /// </summary>
        private enum Layers
        {
            /// <summary>
            /// The base layer.
            /// </summary>
            Base,
            /// <summary>
            /// The border layer.
            /// </summary>
            Border,
            /// <summary>
            /// The text layer.
            /// </summary>
            Text,
            /// <summary>
            /// The button layer.
            /// </summary>
            Button,
            /// <summary>
            /// The content layer.
            /// </summary>
            Content,
            /// <summary>
            /// The overlay layer.
            /// </summary>
            Overlay
        }
        
        /// <summary>
        /// A UI window that has borders and can have buttons, texts and menus in them.
        /// They are scrollable and can have their width and height adjusted.
        /// </summary>
        /// <param name="group">The containing group. Intaken as a <see cref="UIGroup"/>.</param>
        /// <param name="parent">The parent object. Intaken as a <see cref="UIBase"/>.</param>
        /// <param name="id">The window's id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The window's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The window's position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="viewWidth">The window's view width. Intaken as an <see cref="int"/>.</param>
        /// <param name="viewHeight">The window's view height. Intaken as an <see cref="int"/>.</param>
        /// <param name="worldWidth">the window's world width that is to be extended beyond the viewWidth of the window. Intaken as an <see cref="int"/>.</param>
        /// <param name="worldHeight">the window's world height that is to be extended beyond the viewHeight of the window. Intaken as an <see cref="int"/>.</param>
        /// <param name="borderThickness">The window's border thickness. Intaken as an <see cref="int"/>. Set to 0 to disable borders.</param>
        /// <param name="isVisible">The window's visibility. Intaken as a <see cref="bool"/>.</param>
        public UIWindow(UIGroup group, UIBase parent, int id, string name, Vector2 position,
                        int viewWidth, int viewHeight, int worldWidth = 0, int worldHeight = 0,
                        int borderThickness = 10, bool isVisible = true) : base(parent, id, name, position, viewWidth, viewHeight, isVisible)
        {
            Group = group;
            Movement.SetBounds(GraphicsDevice.Viewport.Bounds);
            Camera = new IOCamera2D(GraphicsDevice, viewWidth, viewHeight, worldWidth, worldHeight);
            
            Rasterizer = new RasterizerState
            {
                MultiSampleAntiAlias = false,
                ScissorTestEnable = true
            };

            BorderThickness = borderThickness >= 0 ? borderThickness : 0;

            SetWindowTransparency(GetTransparency("Background").Level);
        }

        #region Events

        /// <summary>
        /// Pushes the ui element on to the <see cref="MonoGameObject.HoverStack"/>.
        /// </summary>
        /// <typeparam name="T">An object derivative of a <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="element">The ui element to push on to the <see cref="MonoGameObject.HoverStack"/>.</param>
        public override void RiseUp<T>(T element)
        {
            if (!element.IsStateSet(FocusStates.IsHovered))
            {
                element.AddState(FocusStates.IsHovered);
            }

            HoverStack.Push(element);
        }

        #endregion

        #region Texts

        /// <summary>
        /// Adds text.
        /// </summary>
        /// <param name="name">The text's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="font">The text's font. Intaken as a <see cref="SpriteFont"/>.</param>
        /// <param name="text">The text's text. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The text's position relative to the window's center.  Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="isVisible">The button's visibility. Intaken as a <see cref="bool"/>.</param>
        /// <returns>Returns the text id, if added, otherwise zero.</returns>
        /// <remarks>If text already exists with the provided name then a zero is returned indicating failure to add the text.</remarks>
        public int AddText(string name, SpriteFont font, string text, Vector2 position = default, bool isVisible = true)
        {
            var nextTextId = 0;

            if (!TextExists(name))
            {
                nextTextId = GetNextValidChildId<UIText>((int)Layers.Text);

                if (!TextExists(nextTextId))
                {
                    var newText = new UIText(this, nextTextId, name, font, text, position, isVisible)
                    {
                        Layer = (int)Layers.Text
                    };

                    newText.Transform.Parent = Transform;
                    newText.LoadContent();

                    Children.Add(newText);
                }
            }

            return nextTextId;
        }

        /// <summary>
        /// Determines whether text exists, by id.
        /// </summary>
        /// <param name="id">The id of the text to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the text exists.</returns>
        public bool TextExists(int id) => ChildExists<UIText>((int)Layers.Text, id);

        /// <summary>
        /// Determines whether text exists, by name.
        /// </summary>
        /// <param name="name">The name of the text to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the text exists.</returns>
        public bool TextExists(string name) => ChildExists<UIText>((int)Layers.Text, name);

        /// <summary>
        /// Retrieves text, by id.
        /// </summary>
        /// <param name="id">The id of the text to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns <see cref="UIText"/> with the requested id, if present, otherwise null.</returns>
        public UIText GetText(int id) => GetChild<UIText>((int)Layers.Text, id);

        /// <summary>
        /// Retrieves text, by name.
        /// </summary>
        /// <param name="name">The name of the text to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns <see cref="UIText"/> with the requested name, if present, otherwise null.</returns>
        public UIText GetText(string name) => GetChild<UIText>((int)Layers.Text, name);

        /// <summary>
        /// Removes text, by id.
        /// </summary>
        /// <param name="id">The id of the text to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the text was removed.</returns>
        public bool RemoveText(int id) => RemoveChild<UIText>((int)Layers.Text, id);

        /// <summary>
        /// Removes text, by name.
        /// </summary>
        /// <param name="name">The name of the text to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the text was removed.</returns>
        public bool RemoveText(string name) => RemoveChild<UIText>((int)Layers.Text, name);

        #endregion

        #region Buttons

        /// <summary>
        /// Adds a button.
        /// </summary>
        /// <param name="name">The button's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="isVisible">The button's visibility. Intaken as a <see cref="bool"/>.</param>
        /// <returns>Returns the button id, if added, otherwise zero.</returns>
        /// <remarks>If a button already exists with the provided name then a zero is returned indicating failure to add the button.</remarks>
        public int AddButton(string name, bool isVisible = true)
        {
            var nextButtonId = 0;

            if (!ButtonExists(name))
            {
                nextButtonId = GetNextValidChildId<UIButton>((int)Layers.Button);

                if (!ButtonExists(nextButtonId))
                {
                    var newButton = new UIButton(this, nextButtonId, name, Vector2.Zero, 60, 30, isVisible)
                    {
                        Layer = (int)Layers.Button
                    };

                    newButton.Transform.Parent = Transform;
                    newButton.LoadContent();

                    Children.Add(newButton);
                }
            }

            return nextButtonId;
        }

        /// <summary>
        /// Determines whether a button exists, by id.
        /// </summary>
        /// <param name="id">The id of the button to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the button exists.</returns>
        public bool ButtonExists(int id) => ChildExists<UIButton>((int)Layers.Button, id);

        /// <summary>
        /// Determines whether a button exists, by name.
        /// </summary>
        /// <param name="name">The name of the button to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the button exists.</returns>
        public bool ButtonExists(string name) => ChildExists<UIButton>((int)Layers.Button, name);

        /// <summary>
        /// Retrieves a button, by id.
        /// </summary>
        /// <param name="id">The id of the button to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a button with the requested id, if present, otherwise null.</returns>
        public UIButton GetButton(int id) => GetChild<UIButton>((int)Layers.Button, id);

        /// <summary>
        /// Retrieves a button, by name.
        /// </summary>
        /// <param name="name">The name of the button to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a button with the requested name, if present, otherwise null.</returns>
        public UIButton GetButton(string name) => GetChild<UIButton>((int)Layers.Button, name);

        /// <summary>
        /// Removes a button, by id.
        /// </summary>
        /// <param name="id">The id of the button to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button was removed.</returns>
        public bool RemoveButton(int id) => RemoveChild<UIButton>((int)Layers.Button, id);

        /// <summary>
        /// Removes a button, by name.
        /// </summary>
        /// <param name="name">The name of the button to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button was removed.</returns>
        public bool RemoveButton(string name) => RemoveChild<UIButton>((int)Layers.Button, name);

        #endregion

        #region Borders

        /// <summary>
        /// Adds a menu.
        /// </summary>
        /// <param name="name">The border's name. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns the border id of the newly added border as an int.</returns>
        /// <remarks>If a border already exists with the provided name then a zero is returned indicating failure to add the border.</remarks>
        private int AddBorder(string name)
        {
            var nextBorderId = 0;

            if (!ButtonExists(name))
            {
                nextBorderId = GetNextValidChildId<UIBorder>((int)Layers.Border);

                if (!ButtonExists(nextBorderId))
                {
                    var newBorder = new UIBorder(this, nextBorderId, name, Vector2.Zero, 10, 10)
                    {
                        Layer = (int) Layers.Border
                    };

                    newBorder.Transform.Parent = Transform;
                    newBorder.LoadContent();

                    Children.Add(newBorder);
                }
            }

            return nextBorderId;
        }

        /// <summary>
        /// Determines whether a border exists, by id.
        /// </summary>
        /// <param name="id">The id of the border to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the border exists.</returns>
        public bool BorderExists(int id) => ChildExists<UIBorder>((int)Layers.Border, id);

        /// <summary>
        /// Determines whether a border exists, by name.
        /// </summary>
        /// <param name="name">The name of the border to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the border exists.</returns>
        public bool BorderExists(string name) => ChildExists<UIBorder>((int)Layers.Border, name);

        /// <summary>
        /// Retrieves a border, by id.
        /// </summary>
        /// <param name="id">The id of the border to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a border with the requested id, if present, otherwise null.</returns>
        public UIBorder GetBorder(int id) => GetChild<UIBorder>((int)Layers.Border, id);

        /// <summary>
        /// Retrieves a border, by name.
        /// </summary>
        /// <param name="name">The name of the border to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a border with the requested name, if present, otherwise null.</returns>
        public UIBorder GetBorder(string name) => GetChild<UIBorder>((int)Layers.Border, name);

        /// <summary>
        /// Removes a border, by id.
        /// </summary>
        /// <param name="id">The id of the border to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the border was removed.</returns>
        public bool RemoveBorder(int id) => RemoveChild<UIBorder>((int)Layers.Border, id);

        /// <summary>
        /// Removes a border, by name.
        /// </summary>
        /// <param name="name">The name of the border to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the border was removed.</returns>
        public bool RemoveBorder(string name) => RemoveChild<UIBorder>((int)Layers.Border, name);

        /// <summary>
        /// Sets all the window's borders to the provided thickness.
        /// </summary>
        /// <param name="borderThickness">The window's border thickness. Intaken as an <see cref="int"/>.</param>
        public void SetBorderThickness(int borderThickness)
        {
            SetBorderThickness(borderThickness, borderThickness, borderThickness, borderThickness);
            SetBorderThickness(new Vector2(borderThickness, borderThickness), new Vector2(borderThickness, borderThickness),
                               new Vector2(borderThickness, borderThickness), new Vector2(borderThickness, borderThickness));
        }

        /// <summary>
        /// Sets the window's individual main border thicknesses.
        /// </summary>
        /// <param name="top">The window's top border thickness. Intaken as an <see cref="int"/>.</param>
        /// <param name="right">The window's right border thickness. Intaken as an <see cref="int"/>.</param>
        /// <param name="bottom">The window's bottom border thickness. Intaken as an <see cref="int"/>.</param>
        /// <param name="left">The window's left border thickness. Intaken as an <see cref="int"/>.</param>
        public void SetBorderThickness(int top, int right, int bottom, int left)
        {
            GetBorder(1).Size.Width = Size.Width;
            GetBorder(1).Size.Height = top;
            GetBorder(2).Size.Width = right;
            GetBorder(2).Size.Height = Size.Height;
            GetBorder(3).Size.Width = Size.Width;
            GetBorder(3).Size.Height = bottom;
            GetBorder(4).Size.Width = left;
            GetBorder(4).Size.Height = Size.Height;
        }

        /// <summary>
        /// Sets the window's individual corner border thicknesses.
        /// </summary>
        /// <param name="topLeft">The window's top left border thickness. Intaken as a <see cref="Vector2"/>. The X axis is the border's width and the Y axis is the the border's height..</param>
        /// <param name="topRight">The window's top left border thickness. Intaken as a <see cref="Vector2"/>. The X axis is the border's width and the Y axis is the the border's height..</param>
        /// <param name="bottomRight">The window's top left border thickness. Intaken as a <see cref="Vector2"/>. The X axis is the border's width and the Y axis is the the border's height..</param>
        /// <param name="bottomLeft">The window's top left border thickness. Intaken as a <see cref="Vector2"/>. The X axis is the border's width and the Y axis is the the border's height..</param>
        /// <remarks>The window's corner border's thickness. Intaken as a <see cref="Vector2"/>. The X axis is the border's width and the Y axis is the the border's height.</remarks>
        public void SetBorderThickness(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft)
        {
            GetBorder(5).Size.Width = (int)topLeft.X;
            GetBorder(5).Size.Height = (int)topLeft.Y;
            GetBorder(6).Size.Width = (int)topRight.X;
            GetBorder(6).Size.Height = (int)topRight.Y;
            GetBorder(7).Size.Width = (int)bottomRight.X;
            GetBorder(7).Size.Height = (int)bottomRight.Y;
            GetBorder(8).Size.Width = (int)bottomLeft.X;
            GetBorder(8).Size.Height = (int)bottomLeft.Y;
        }

        /// <summary>
        /// Sets visibility for each border to that of the window.
        /// </summary>
        public void SetBorderVisibility()
        {
            SetBorderVisibility(IsVisible);
        }

        /// <summary>
        /// Sets visibility for each border.
        /// </summary>
        public void SetBorderVisibility(bool visibility)
        {
            foreach (var component in Children)
            {
                if (component is UIBorder border)
                {
                    border.IsVisible = visibility;
                }
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
        /// Updates the border positions.
        /// </summary>
        private void UpdateBorderPositions()
        {
            var borderTop = GetBorder(1);
            var borderRight = GetBorder(2);
            var borderBottom = GetBorder(3);
            var borderLeft = GetBorder(4);

            var borderTopLeft = GetBorder(5);
            var borderTopRight = GetBorder(6);
            var borderBottomRight = GetBorder(7);
            var borderBottomLeft = GetBorder(8);

            if (borderTop.IsVisible)
            {
                borderTop.Transform.Position = new Vector2(0, -(ExtendedRectangle.Origin.Y - borderTop.ExtendedRectangle.Origin.Y));
            }

            if (borderRight.IsVisible)
            {
                borderRight.Transform.Position = new Vector2(ExtendedRectangle.Origin.X - borderRight.ExtendedRectangle.Origin.X, 0);
            }

            if (borderBottom.IsVisible)
            {
                borderBottom.Transform.Position = new Vector2(0, ExtendedRectangle.Origin.Y - borderBottom.ExtendedRectangle.Origin.Y);
            }

            if (borderLeft.IsVisible)
            {
                borderLeft.Transform.Position = new Vector2(-(ExtendedRectangle.Origin.X - borderLeft.ExtendedRectangle.Origin.X), 0);
            }

            if (borderTopLeft.IsVisible)
            {
                borderTopLeft.Transform.Position = new Vector2(-(ExtendedRectangle.Origin.X - borderTopLeft.ExtendedRectangle.Origin.X), -(ExtendedRectangle.Origin.Y - borderTopLeft.ExtendedRectangle.Origin.Y));
            }

            if (borderTopRight.IsVisible)
            {
                borderTopRight.Transform.Position = new Vector2(ExtendedRectangle.Origin.X - borderTopRight.ExtendedRectangle.Origin.X, -(ExtendedRectangle.Origin.Y - borderTopRight.ExtendedRectangle.Origin.Y));
            }

            if (borderBottomRight.IsVisible)
            {
                borderBottomRight.Transform.Position = new Vector2(ExtendedRectangle.Origin.X - borderBottomRight.ExtendedRectangle.Origin.X, ExtendedRectangle.Origin.Y - borderBottomRight.ExtendedRectangle.Origin.Y);
            }

            if (borderBottomLeft.IsVisible)
            {
                borderBottomLeft.Transform.Position = new Vector2(-(ExtendedRectangle.Origin.X - borderBottomLeft.ExtendedRectangle.Origin.X), ExtendedRectangle.Origin.Y - borderBottomLeft.ExtendedRectangle.Origin.Y);
            }
        }

        #endregion

        #region Content

        /// <summary>
        /// Adds content (non-ui) elements to the <see cref="UIWindow"/>.
        /// </summary>
        /// <typeparam name="T">An object derivative of a <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="content">The content to add. A derivative of <see cref="MonoGameObject"/>.</param>
        /// <returns>Returns the content's id, if the content has been added, otherwise zero.</returns>
        public int AddContent<T>(T content) where T : MonoGameObject
        {
            if (!ContentExists<T>(content.Name) &&
                !ContentExists<T>(content.Id))
            {
                content.Transform.Parent = Transform;
                Children.Add(content);

                return content.Id;
            }

            return 0;
        }

        /// <summary>
        /// Determines whether content exists, by id.
        /// </summary>
        /// <typeparam name="T">An object derivative of a <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="id">The id of the content to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the content exists.</returns>
        public bool ContentExists<T>(int id) where T : MonoGameObject => ChildExists<T>((int)Layers.Content, id);

        /// <summary>
        /// Determines whether content exists, by name.
        /// </summary>
        /// <typeparam name="T">An object derivative of a <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="name">The name of the content to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the content exists.</returns>
        public bool ContentExists<T>(string name) where T : MonoGameObject => ChildExists<T>((int)Layers.Content, name);

        /// <summary>
        /// Retrieves content, by id.
        /// </summary>
        /// <typeparam name="T">An object derivative of a <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="id">The id of the content to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns an object of type <see cref="T"/> with the requested id, if present, otherwise null.</returns>
        public T GetContent<T>(int id) where T : MonoGameObject => GetChild<T>((int)Layers.Content, id);

        /// <summary>
        /// Retrieves content, by name.
        /// </summary>
        /// <typeparam name="T">An object derivative of a <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="name">The content of the text to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns an object of type <see cref="T"/> with the requested name, if present, otherwise null.</returns>
        public T GetContent<T>(string name) where T : MonoGameObject => GetChild<T>((int)Layers.Content, name);

        /// <summary>
        /// Removes content, by id.
        /// </summary>
        /// <typeparam name="T">An object derivative of a <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="id">The id of the content to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the content was removed.</returns>
        public bool RemoveContent<T>(int id) where T : MonoGameObject => RemoveChild<T>((int)Layers.Content, id);

        /// <summary>
        /// Removes content, by name.
        /// </summary>
        /// <typeparam name="T">An object derivative of a <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="name">The name of the content to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the content was removed.</returns>
        public bool RemoveContent<T>(string name) where T : MonoGameObject => RemoveChild<T>((int)Layers.Content, name);

        #endregion

        /// <summary>
        /// Calculates an extended <see cref="RectangleF"/> that includes the window's outlines and borders.
        /// </summary>
        /// <returns>Returns a <see cref="RectangleF"/> that includes the window's outlines and borders.</returns>
        protected override RectangleF CalculateExtendedRectangle()
        {
            // Rectangle
            var rectangle = Rectangle;
            
            // Side Borders
            var borderTop = GetBorder(1);
            var borderRight = GetBorder(2);
            var borderBottom = GetBorder(3);
            var borderLeft = GetBorder(4);

            // Corner Borders
            var borderTopLeft = GetBorder(5);
            var borderTopRight = GetBorder(6);
            var borderBottomRight = GetBorder(7);
            var borderBottomLeft = GetBorder(8);

            // Top Borders
            if (borderTop.IsVisible)
            {
                rectangle.Y -= borderTop.ExtendedRectangle.Height;
                rectangle.Height += borderTop.ExtendedRectangle.Height;
            }
            else if (borderTopLeft.IsVisible)
            {
                rectangle.Y -= borderTopLeft.ExtendedRectangle.Height;
                rectangle.Height += borderTopLeft.ExtendedRectangle.Height;
            }
            else if (borderTopRight.IsVisible)
            {
                rectangle.Y -= borderTopRight.ExtendedRectangle.Height;
                rectangle.Height += borderTopRight.ExtendedRectangle.Height;
            }

            // Right Borders
            if (borderRight.IsVisible)
            {
                rectangle.Width += borderRight.ExtendedRectangle.Width;
            }
            else if (borderTopRight.IsVisible)
            {
                rectangle.Width += borderTopRight.ExtendedRectangle.Width;
            }
            else if (borderBottomRight.IsVisible)
            {
                rectangle.Width += borderBottomRight.ExtendedRectangle.Width;
            }

            // Bottom Borders
            if (borderBottom.IsVisible)
            {
                rectangle.Height += borderBottom.ExtendedRectangle.Height;
            }
            else if (borderBottomRight.IsVisible)
            {
                rectangle.Height += borderBottomRight.ExtendedRectangle.Height;
            }
            else if (borderBottomLeft.IsVisible)
            {
                rectangle.Height += borderBottomLeft.ExtendedRectangle.Height;
            }

            // Left Borders
            if (borderLeft.IsVisible)
            {
                rectangle.X -= borderLeft.ExtendedRectangle.Width;
                rectangle.Width += borderLeft.ExtendedRectangle.Width;
            }
            else if (borderTopLeft.IsVisible)
            {
                rectangle.X -= borderTopLeft.ExtendedRectangle.Width;
                rectangle.Width += borderTopLeft.ExtendedRectangle.Width;
            }
            else if (borderBottomLeft.IsVisible)
            {
                rectangle.X -= borderBottomLeft.ExtendedRectangle.Width;
                rectangle.Width += borderBottomLeft.ExtendedRectangle.Width;
            }

            return rectangle;
        }

        /// <summary>
        /// Sets the window, borders and outlines transparency levels.
        /// </summary>
        /// <param name="transparencyLevel">The transparency level to set. Intaken as a <see cref="float"/>.</param>
        public void SetWindowTransparency(float transparencyLevel)
        {
            GetTransparency("Background").Level = transparencyLevel;
            GetTransparency("Outline").Level = transparencyLevel;

            foreach (var component in Children)
            {
                if (component is UIBase element)
                {
                    element.GetTransparency("Background").Level = GetTransparency("Background").Level;
                    element.GetTransparency("Outline").Level = GetTransparency("Outline").Level;

                    element.SetOutlineTransparency();
                }
            }
        }

        /// <summary>
        /// The window's content loader.
        /// </summary>
        public override void LoadContent(ContentManager content = null)
        {
            // Add all side borders.
            AddBorder("Top");
            AddBorder("Right");
            AddBorder("Bottom");
            AddBorder("Left");

            // Add all corner borders.
            AddBorder("TopLeft");
            AddBorder("TopRight");
            AddBorder("BottomLeft");
            AddBorder("BottomRight");

            // Define the border's sizes.
            SetBorderThickness(BorderThickness == 0 ? 0 : BorderThickness);
            SetBorderVisibility(BorderThickness > 0);

            // Load Content.
            base.LoadContent();

            // Update their locations.
            UpdateBorderPositions();
        }

        /// <summary>
        /// The window's update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame <see cref="GameTime"/>.</param>
        public override void Update(GameTime gameTime)
        {
            if (IsStateSet(FocusStates.IsFocused) &&
                IsStateSet(FocusStates.IsHovered))
            {
                if (Camera.IsActive)
                {
                    if (!Camera.IsStateSet(FocusStates.IsFocused))
                    {
                        Camera.AddState(FocusStates.IsFocused);
                    }

                    if (!Camera.IsStateSet(FocusStates.IsHovered))
                    {
                        Camera.AddState(FocusStates.IsHovered);
                    }
                }

                // Move the window
                if (IsMovable)
                {
                    if (Events.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Held)
                    {
                        Movement.Move(Events.InputDeltas);
                    }
                }
            }
            else
            {
                Camera.RemoveState(FocusStates.IsFocused);
                Camera.RemoveState(FocusStates.IsHovered);
            }
            
            // Update the camera.
            Camera.Update(gameTime);
            
            foreach (var component in Children)
            {
                component?.Update(gameTime);
            }

            // Update the base.
            base.Update(gameTime);
        }

        /// <summary>
        /// The window's draw method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform = default)
        {
            if (IsVisible)
            {
                #region Draw Base and Borders

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

                // Draw base.
                base.Draw(spriteBatch, transform);

                // Draw borders.
                foreach (var component in Children.Where(element => element.Layer == (int)Layers.Border))
                {
                    component.Draw(spriteBatch, transform);
                }
                
                spriteBatch.End();

                #endregion

                #region Draw Window Contents

                // Save the original view for restoration later.
                var originalScissor = spriteBatch.GraphicsDevice.ScissorRectangle;

                // Apply the window's view.
                spriteBatch.GraphicsDevice.ScissorRectangle = (Rectangle)Rectangle;

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, rasterizerState: Rasterizer, transformMatrix: Camera.GetViewMatrix());

                // Draw the window's elements.
                foreach (var component in Children.Where(element => element.Layer != (int)Layers.Border))
                {
                    component.Draw(spriteBatch, transform);
                }

                spriteBatch.End();

                // Restore the original view to the graphics device.
                spriteBatch.GraphicsDevice.ScissorRectangle = originalScissor;

                #endregion
            }
        }
    }
}