using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Softfire.MonoGame.IO;

namespace Softfire.MonoGame.UI
{
    public class UIGroup
    {
        /// <summary>
        /// UIGroup Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Is Active?
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// UIGroup Camera.
        /// </summary>
        internal IOCamera2D UICamera { get; }

        /// <summary>
        /// UIGroup Default Viewport.
        /// The default viewport used to display the UIGroup's Windows.
        /// </summary>
        private Viewport DefaultViewport { get; }

        /// <summary>
        /// UIGroup Windows.
        /// </summary>
        public Dictionary<string, UIWindow> Windows { get; }

        /// <summary>
        /// UIGroup Menus.
        /// </summary>
        public Dictionary<string, UIMenu> Menus { get; }

        /// <summary>
        /// UIGroup Texts.
        /// </summary>
        public Dictionary<string, UIText> Texts { get; }

        /// <summary>
        /// UIGroup Textures.
        /// </summary>
        public Dictionary<string, UITexture> Textures { get; }

        /// <summary>
        /// UIGroup Active input Devices.
        /// </summary>
        public List<Rectangle> ActiveInputDevices { internal get; set; }

        /// <summary>
        /// UIGroup UIFrameBatch.
        /// Used to draw the UIBase's frame using the UIManager's UICamera.
        /// </summary>
        private SpriteBatch UIFrameBatch { get; }

        /// <summary>
        /// UIGroup UIContentBatch.
        /// Used to draw the UIBase's contents using it's internal IOCamera2D.
        /// </summary>
        private SpriteBatch UIContentsBatch { get; }

        /// <summary>
        /// Internal Order Number.
        /// </summary>
        private int _orderNumber;

        /// <summary>
        /// Order Number.
        /// Window will be updated/drawn in order from smallest to largest.
        /// </summary>
        public int OrderNumber
        {
            get => _orderNumber;
            set => _orderNumber = value >= 1 ? value : 0;
        }

        /// <summary>
        /// Current Internal Window Identifier.
        /// </summary>
        private string CurrentWindowIdentifier { get; set; }

        /// <summary>
        /// Current Internal Menu Identifier.
        /// </summary>
        private string CurrentMenuIdentifier { get; set; }

        /// <summary>
        /// Current Internal Text Identifier.
        /// </summary>
        private string CurrentTextIdentifier { get; set; }

        /// <summary>
        /// Current Internal Texture Identifier.
        /// </summary>
        private string CurrentTextureIdentifier { get; set; }

        /// <summary>
        /// UIGroup Constructor.
        /// </summary>
        /// <param name="name">The UIGroup Name. Intaken as a string.</param>
        /// <param name="orderNumber">Intakes the Group's Update/Draw Order Number as an int.</param>
        /// <param name="graphicsDevice">Intakes the UIGroup's parent GraphicsDevice.</param>
        public UIGroup(string name, int orderNumber, GraphicsDevice graphicsDevice)
        {
            Name = name;
            OrderNumber = orderNumber;

            DefaultViewport = graphicsDevice.Viewport;
            UICamera = new IOCamera2D(DefaultViewport.Bounds);
            UIFrameBatch = new SpriteBatch(graphicsDevice);
            UIContentsBatch = new SpriteBatch(graphicsDevice);

            Windows = new Dictionary<string, UIWindow>();
            Menus = new Dictionary<string, UIMenu>();
            Texts = new Dictionary<string, UIText>();
            Textures = new Dictionary<string, UITexture>();
            ActiveInputDevices = new List<Rectangle>();

            CurrentWindowIdentifier = string.Empty;

            IsActive = false;
        }

        #region Texts

        /// <summary>
        /// Add Text.
        /// Adds a new Text, if it does not already exist.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <param name="text">Intakes a new UIText.</param>
        /// <returns>Returns a boolean indicating if the Text was added.</returns>
        public bool AddText(string identifier, UIText text)
        {
            var result = false;

            if (Texts.ContainsKey(identifier) == false)
            {
                text.LoadContent();
                Texts.Add(identifier, text);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Text.
        /// Get a specific UIText by it's identifier, if it exists.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <returns>Returns the Text with the specified identifier, if present, otherwise null.</returns>
        public UIText GetText(string identifier)
        {
            UIText result = null;

            if (Texts.ContainsKey(identifier))
            {
                result = Texts[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Text.
        /// Removes the named Text, if it is present.
        /// </summary>
        /// <param name="identifier">Intakes a unique Identifier as a string.</param>
        /// <returns>Returns a boolean indicating whether the Text was removed.</returns>
        public bool RemoveText(string identifier)
        {
            var result = false;

            if (Texts.ContainsKey(identifier))
            {
                result = Texts.Remove(identifier);
            }

            return result;
        }

        #endregion

        #region Menus

        /// <summary>
        /// Add Menu.
        /// Adds a new Menu, if it does not already exist.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <param name="menu">Intakes a new UIMenu.</param>
        /// <returns>Returns a boolean indicating if the Menu was added.</returns>
        public bool AddMenu(string identifier, UIMenu menu)
        {
            var result = false;

            if (Menus.ContainsKey(identifier) == false)
            {
                menu.ParentGroup = this;
                menu.LoadContent();
                Menus.Add(identifier, menu);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Menu.
        /// Get a specific UIMenu by it's identifier, if it exists.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <returns>Returns the Menu with the specified identifier, if present, otherwise null.</returns>
        public UIMenu GetMenu(string identifier)
        {
            UIMenu result = null;

            if (Menus.ContainsKey(identifier))
            {
                result = Menus[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Menu.
        /// Removes the named Menu, if it is present.
        /// </summary>
        /// <param name="identifier">Intakes a unique Identifier as a string.</param>
        /// <returns>Returns a boolean indicating whether the Menu was removed.</returns>
        public bool RemoveMenu(string identifier)
        {
            var result = false;

            if (Menus.ContainsKey(identifier))
            {
                result = Menus.Remove(identifier);
            }

            return result;
        }

        #endregion

        #region Windows

        /// <summary>
        /// Add Window.
        /// Adds a new Window, if it does not already exist.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <param name="window">Intakes a new UIWindow.</param>
        /// <returns>Returns a boolean indicating if the Window was added.</returns>
        public bool AddWindow(string identifier, UIWindow window)
        {
            var result = false;

            if (Windows.ContainsKey(identifier) == false)
            {
                window.ParentGroup = this;
                window.LoadContent();
                Windows.Add(identifier, window);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Window.
        /// Get a specific UIWindow by it's identifier, if it exists.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <returns>Returns the Window with the specified identifier, if present, otherwise null.</returns>
        public UIWindow GetWindow(string identifier)
        {
            UIWindow result = null;

            if (Windows.ContainsKey(identifier))
            {
                result = Windows[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Window.
        /// Removes the named Window, if it is present.
        /// </summary>
        /// <param name="identifier">Intakes a unique Identifier as a string.</param>
        /// <returns>Returns a boolean indicating whether the Window was removed.</returns>
        public bool RemoveWindow(string identifier)
        {
            var result = false;

            if (Windows.ContainsKey(identifier))
            {
                result = Windows.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Reorder Window Up.
        /// </summary>
        /// <param name="identifier">Intakes a unique Identifier as a string.</param>
        /// <returns>Returns a boolean indicating whether the Window's Order Number was increased.</returns>
        public bool ReorderWindowUp(string identifier)
        {
            var result = false;
            UIWindow window;

            if ((window = GetWindow(identifier)) != null)
            {
                window.OrderNumber++;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Reorder Window Down.
        /// </summary>
        /// <param name="identifier">Intakes a unique Identifier as a string.</param>
        /// <returns>Returns a boolean indicating whether the Window's Order Number was decreased.</returns>
        public bool ReorderWindowDown(string identifier)
        {
            var result = false;
            UIWindow window;

            if ((window = GetWindow(identifier)) != null)
            {
                window.OrderNumber--;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Checks to see if any Windows are in motion.
        /// </summary>
        /// <returns>Returns a boolean indicating if Windows are currently in motion.</returns>
        private bool AreWindowsMoving()
        {
            return Windows.Count(win => win.Value.IsMoving) > 0;
        }

        #endregion

        #region Textures

        /// <summary>
        /// Add Texture.
        /// Adds a new Texture, if it does not already exist.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <param name="texture">Intakes a new UITexture.</param>
        /// <returns>Returns a boolean indicating if the Texture was added.</returns>
        public bool AddTexture(string identifier, UITexture texture)
        {
            var result = false;

            if (Texts.ContainsKey(identifier) == false)
            {
                texture.LoadContent();
                Textures.Add(identifier, texture);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Texture.
        /// Get a specific UITexture by it's identifier, if it exists.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <returns>Returns the Texture with the specified identifier, if present, otherwise null.</returns>
        public UITexture GetTexture(string identifier)
        {
            UITexture result = null;

            if (Texts.ContainsKey(identifier))
            {
                result = Textures[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Texture.
        /// Removes the named Texture, if it is present.
        /// </summary>
        /// <param name="identifier">Intakes a unique Identifier as a string.</param>
        /// <returns>Returns a boolean indicating whether the Texture was removed.</returns>
        public bool RemoveTexture(string identifier)
        {
            var result = false;

            if (Textures.ContainsKey(identifier))
            {
                result = Textures.Remove(identifier);
            }

            return result;
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
            #region Texts

            // Update Order is Ascending.
            // From lowest to highest.
            foreach (var text in Texts.OrderBy(text => text.Value.OrderNumber))
            {
                if (text.Value.IsVisible)
                {
                    // Check if any input devices are over top of any of the texts.
                    for (var index = 0; index < ActiveInputDevices.Count; index++)
                    {
                        var inputDevice = ActiveInputDevices[index];
                        text.Value.CheckIsInFocus(new Rectangle((int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                                (int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                                inputDevice.Width,
                                                                inputDevice.Height));
                    }
                }

                await text.Value.Update(gameTime);
            }

            // Get Identifier from Text that has Focus.
            // Will check in order from lowest to highest.
            foreach (var text in Texts.OrderBy(text => text.Value.OrderNumber))
            {
                if (text.Value.IsInFocus)
                {
                    CurrentTextIdentifier = text.Key;
                }
            }

            #endregion

            #region Menus

            // Update Order is Ascending.
            // From lowest to highest.
            foreach (var menu in Menus.OrderBy(menu => menu.Value.OrderNumber))
            {
                if (menu.Value.IsVisible)
                {
                    // Check if any input devices are over top of any of the menus.
                    for (var index = 0; index < ActiveInputDevices.Count; index++)
                    {
                        var inputDevice = ActiveInputDevices[index];
                        menu.Value.CheckIsInFocus(new Rectangle((int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                                (int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                                inputDevice.Width,
                                                                inputDevice.Height));
                    }
                }

                await menu.Value.Update(gameTime);
            }

            // Get Identifier from Menu that has Focus.
            // Will check in order from lowest to highest.
            foreach (var menu in Menus.OrderBy(menu => menu.Value.OrderNumber))
            {
                if (menu.Value.IsInFocus)
                {
                    CurrentMenuIdentifier = menu.Key;
                }
            }

            #endregion

            #region Windows

            // Update Order is Ascending.
            // From lowest to highest.
            foreach (var window in Windows.OrderBy(win => win.Value.OrderNumber))
            {
                window.Value.ActiveInputDevices = ActiveInputDevices;

                if (AreWindowsMoving() == false)
                {
                    if (window.Value.IsVisible)
                    {
                        // Check if any input devices are over top of any of the window contents.
                        for (var index = 0; index < ActiveInputDevices.Count; index++)
                        {
                            var inputDevice = ActiveInputDevices[index];
                            window.Value.ContentsCamera.CheckIsInFocus(new Rectangle((int) UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                                                     (int) UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                                                     inputDevice.Width,
                                                                                     inputDevice.Height));
                        }
                    }
                }

                if (CurrentWindowIdentifier == window.Key)
                {
                    if (window.Value.IsMoving == false &&
                        window.Value.TitleBar.IsVisible)
                    {
                        window.Value.IsMoving = IOMouse.LeftClickDownInside(window.Value.TitleBar.Rectangle);
                    }
                    else if (window.Value.IsMoving == false &&
                             window.Value.TopBorder.IsVisible)
                    {
                        window.Value.IsMoving = IOMouse.LeftClickDownInside(window.Value.TopBorder.Rectangle);
                    }
                    else if (window.Value.IsMoving &&
                             IOMouse.LeftClickHeld())
                    {
                        window.Value.Move(IOMouse.Rectangle);
                    }
                    else
                    {
                        window.Value.IsMoving = false;
                    }
                }

                if (CurrentWindowIdentifier == window.Key)
                {
                    if (IOKeyboard.KeyPress(Keys.D1))
                    {
                        ReorderWindowUp(window.Key);
                    }

                    if (IOKeyboard.KeyPress(Keys.D2))
                    {
                        ReorderWindowDown(window.Key);
                    }
                }

                await window.Value.Update(gameTime);
            }

            // Get Identifier from Window that has Focus.
            // Will check in order from lowest to highest.
            foreach (var window in Windows.OrderBy(win => win.Value.OrderNumber))
            {
                if (window.Value.IsInFocus)
                {
                    CurrentWindowIdentifier = window.Key;
                }
            }

            #endregion

            #region Textures

            // Update Order is Ascending.
            // From lowest to highest.
            foreach (var texture in Textures.OrderBy(texture => texture.Value.OrderNumber))
            {
                if (texture.Value.IsVisible)
                {
                    // Check if any input devices are over top of any of the textures.
                    for (var index = 0; index < ActiveInputDevices.Count; index++)
                    {
                        var inputDevice = ActiveInputDevices[index];
                        texture.Value.CheckIsInFocus(new Rectangle((int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                                   (int)UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                                   inputDevice.Width,
                                                                   inputDevice.Height));
                    }
                }

                await texture.Value.Update(gameTime);
            }

            // Get Identifier from Texture that has Focus.
            // Will check in order from lowest to highest.
            foreach (var texture in Textures.OrderBy(texture => texture.Value.OrderNumber))
            {
                if (texture.Value.IsInFocus)
                {
                    CurrentTextureIdentifier = texture.Key;
                }
            }

            #endregion

            UICamera.Update(gameTime);
            ActiveInputDevices.Clear();
        }

        /// <summary>
        /// Draw Method.
        /// Draws all windows and their contents.
        /// Draws are done is Ascending Order by OrderNumber.
        /// </summary>
        public void Draw()
        {
            if (IsActive)
            {
                // UI Frame Batch Begin with UICamera Matrix applied.
                UIFrameBatch.Begin(transformMatrix: UICamera.Matrix);

                #region Texts

                // Draw Order is Ascending.
                // Drawing from lowest to highest.
                foreach (var text in Texts.OrderBy(text => text.Value.OrderNumber))
                {
                    // Draw Text UIBase.
                    text.Value.Draw(UIFrameBatch);
                }

                #endregion

                #region Menus

                // Draw Order is Ascending.
                // Drawing from lowest to highest.
                foreach (var menu in Menus.OrderBy(menu => menu.Value.OrderNumber))
                {
                    // Draw Menu UIBase.
                    menu.Value.Draw(UIFrameBatch);
                }

                #endregion

                #region Windows

                // Draw Order is Ascending.
                // Drawing from lowest to highest.
                foreach (var window in Windows.OrderBy(win => win.Value.OrderNumber))
                {
                    // Draw Window UIBase.
                    window.Value.Draw(UIFrameBatch);

                    // Draw Window Contents.
                    window.Value.DrawContents(UIContentsBatch, DefaultViewport);
                }

                #endregion

                #region Textures

                // Draw Order is Ascending.
                // Drawing from lowest to highest.
                foreach (var texture in Textures.OrderBy(texture => texture.Value.OrderNumber))
                {
                    // Draw Text UIBase.
                    texture.Value.Draw(UIFrameBatch);
                }

                #endregion

                // End Frame Drawing.
                UIFrameBatch.End();

                #region Windows Contents

                // Draw Order is Ascending.
                // Drawing from lowest to highest.
                foreach (var window in Windows.OrderBy(win => win.Value.OrderNumber))
                {
                    // Draw Window Contents.
                    window.Value.DrawContents(UIContentsBatch, DefaultViewport);
                }

                #endregion
            }
        }
    }
}