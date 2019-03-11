using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE.Common;
using Softfire.MonoGame.CORE.Input;
using Softfire.MonoGame.IO;
using Softfire.MonoGame.UI.Items;
using System.Collections.Generic;
using System.Linq;
using Softfire.MonoGame.CORE;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// A class for grouping UI elements together.
    /// </summary>
    public class UIGroup : IMonoGameIdentifierComponent, IMonoGameActiveComponent
    {
        /// <summary>
        /// Determines whether the group is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// The group's parent object.
        /// </summary>
        internal UIManager Manager { get; }

        /// <summary>
        /// The group's unique id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The group's unique name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The current tab order id.
        /// </summary>
        private int CurrentTabOrderId { get; set; } = 1;

        /// <summary>
        /// The group's UI elements.
        /// </summary>
        private IList<UIWindow> Children { get; }
        
        /// <summary>
        /// A group of UI elements.
        /// </summary>
        /// <param name="manager">The manager of this group. Intaken as a <see cref="UIManager"/>.</param>
        /// <param name="id">The group's unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The group's unique name. Intaken as a <see cref="string"/>.</param>
        public UIGroup(UIManager manager, int id, string name)
        {
            Id = id;
            Name = name;
            Manager = manager;
            Children = new List<UIWindow>();

            // Register movement event.
            // Registration order matters for call order.
            IOManager.InputMovementHandler += OnMove;
            IOManager.InputMovementHandler += OnBlur;
            IOManager.InputMovementHandler += OnFocus;

            // Register scrolling event.
            IOManager.InputScrollHandler += OnScroll;

            // Register input events.
            IOManager.InputPressHandler += OnTab;
            IOManager.InputPressHandler += OnPress;
            IOManager.InputReleaseHandler += OnRelease;
            IOManager.InputHeldHandler += OnHeld;
        }

        #region Windows

        /// <summary>
        /// Adds a window.
        /// </summary>
        /// <param name="name">The window's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The window's position offset from the center of the screen. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="viewWidth">The window's view width. Intaken as an <see cref="int"/>.</param>
        /// <param name="viewHeight">The window's view height. Intaken as an <see cref="int"/>.</param>
        /// <param name="worldWidth">the window's world width. Intaken as an <see cref="int"/>.</param>
        /// <param name="worldHeight">the window's world height. Intaken as an <see cref="int"/>.</param>
        /// <param name="borderThickness">The window's border thickness. Intaken as an <see cref="int"/>. Set to 0 to disable borders.</param>
        /// <param name="isVisible">The window's visibility. Intaken as a <see cref="bool"/>.</param>
        /// <returns>Returns the window id of the newly added window as an int.</returns>
        public int AddWindow(string name, Vector2 position = default, int viewWidth = 300, int viewHeight = 300,
                             int worldWidth = 0, int worldHeight = 150, int borderThickness = 10, bool isVisible = true)
        {
            var nextWindowId = 0;

            if (!WindowExists(name))
            {
                nextWindowId = Identities.GetNextValidObjectId<UIWindow, UIWindow>(Children);

                if (!WindowExists(nextWindowId))
                {
                    var newWindow = new UIWindow(this, null, nextWindowId, name, Manager.GetViewportDimensions().Center + position,
                                                 viewWidth, viewHeight, worldWidth, worldHeight, borderThickness, isVisible)
                    {
                        TabOrder = InputCommands.GetNextValidTabId<UIWindow, UIWindow>(Children)
                    };
                    newWindow.LoadContent();

                    Children.Add(newWindow);
                }
            }

            return nextWindowId;
        }

        /// <summary>
        /// Determines whether a window exists, by id.
        /// </summary>
        /// <param name="id">The id of the window to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the window exists.</returns>
        public bool WindowExists(int id) => Identities.ObjectExists<UIWindow, UIWindow>(Children, id);

        /// <summary>
        /// Determines whether a window exists, by name.
        /// </summary>
        /// <param name="name">The name of the window to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the window exists.</returns>
        public bool WindowExists(string name) => Identities.ObjectExists<UIWindow, UIWindow>(Children, name);

        /// <summary>
        /// Gets a window, by id.
        /// </summary>
        /// <param name="id">The id of the window to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a menu with the requested id, if present, otherwise null.</returns>
        public UIWindow GetWindow(int id) => Identities.GetObject<UIWindow, UIWindow>(Children, id);

        /// <summary>
        /// Gets a window, by name.
        /// </summary>
        /// <param name="name">The name of the window to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a window with the requested name, if present, otherwise null.</returns>
        public UIWindow GetWindow(string name) => Identities.GetObject<UIWindow, UIWindow>(Children, name);

        /// <summary>
        /// Removes a window, by id.
        /// </summary>
        /// <param name="id">The id of the window to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the window was removed.</returns>
        public bool RemoveWindow(int id) => Identities.RemoveObject<UIWindow, UIWindow>(Children, id);

        /// <summary>
        /// Removes a window, by name.
        /// </summary>
        /// <param name="name">The name of the window to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the window was removed.</returns>
        public bool RemoveWindow(string name) => Identities.RemoveObject<UIWindow, UIWindow>(Children, name);

        #endregion

        #region Events

        /// <summary>
        /// The subscription method to action when input changes.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnMove(object sender, InputEventArgs args)
        {
            foreach (var window in Children)
            {
                if (window.IsStateSet(FocusStates.IsHovered))
                {
                    window.CheckForFocus(Children, args, args.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Press ||
                                                         args.InputStates.GetState(InputMouseActionFlags.RightClick) == InputActionStateFlags.Press ||
                                                         args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                    window.OnMove(sender, args);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the window is scrolled.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnScroll(object sender, InputEventArgs args)
        {
            foreach (var window in Children)
            {
                if (window.IsStateSet(FocusStates.IsHovered))
                {
                    window.CheckForFocus(Children, args, args.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Press ||
                                                         args.InputStates.GetState(InputMouseActionFlags.RightClick) == InputActionStateFlags.Press ||
                                                         args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                    window.OnScroll(sender, args);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object gains focus.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnFocus(object sender, InputEventArgs args)
        {
            foreach (var window in Children)
            {
                window.OnFocus(sender, args);
            }
        }

        /// <summary>
        /// The subscription method to action when the object loses focus or blurs.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnBlur(object sender, InputEventArgs args)
        {
            foreach (var window in Children)
            {
                window.OnBlur(sender, args);
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects a press action.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnPress(object sender, InputEventArgs args)
        {
            foreach (var window in Children)
            {
                if (window.IsStateSet(FocusStates.IsHovered))
                {
                    window.CheckForFocus(Children, args, args.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Press ||
                                                         args.InputStates.GetState(InputMouseActionFlags.RightClick) == InputActionStateFlags.Press ||
                                                         args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                    window.OnPress(sender, args);
                }

                if (window.IsStateSet(FocusStates.IsFocused))
                {
                    // Set tab order to currently focused window.
                    CurrentTabOrderId = window.TabOrder;
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects a click has been released.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnRelease(object sender, InputEventArgs args)
        {
            foreach (var window in Children)
            {
                if (window.IsStateSet(FocusStates.IsHovered))
                {
                    window.CheckForFocus(Children, args, args.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Press ||
                                                         args.InputStates.GetState(InputMouseActionFlags.RightClick) == InputActionStateFlags.Press ||
                                                         args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                    window.OnRelease(sender, args);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects a click has been held.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnHeld(object sender, InputEventArgs args)
        {
            foreach (var window in Children)
            {
                window.OnHeld(sender, args);
            }
        }

        /// <summary>
        /// The subscription method to action when the mapped tabbing button has been pressed.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnTab(object sender, InputEventArgs args)
        {
            // If Tab is pressed.
            if (args.InputStates.GetState(InputMappableMovementCommandFlags.Tab) == InputActionStateFlags.Press ||
                args.InputStates.GetState(InputKeyboardCommandFlags.TabKey) == InputActionStateFlags.Press)
            {
                if (CurrentTabOrderId + 1 > Children.Count)
                {
                    CurrentTabOrderId = 1;
                }
                else
                {
                    CurrentTabOrderId++;
                }

                foreach (var window in Children)
                {
                    if (window.TabOrder == CurrentTabOrderId)
                    {
                        // Add focus.
                        window.AddState(FocusStates.IsFocused);

                        // Remove focus from other windows, if focused.
                        foreach (var child in Children)
                        {
                            if (!window.Equals(child))
                            {
                                child.RemoveState(FocusStates.IsFocused);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Update Method.
        /// </summary>
        /// <param name="gameTime">MonoGame's <see cref="GameTime"/>.</param>
        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                foreach (var window in Children.OrderBy(win => win.IsStateSet(FocusStates.IsFocused)))
                {
                    window.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// The group's draw method.
        /// </summary>
        /// <param name="spriteBatch">MonoGame's <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public void Draw(SpriteBatch spriteBatch, Matrix transform)
        {
            if (IsActive)
            {
                foreach (var window in Children.OrderBy(win => win.IsStateSet(FocusStates.IsFocused)))
                {
                    window.Draw(spriteBatch, transform);
                }
            }
        }
    }
}