using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Softfire.MonoGame.CORE;
using Softfire.MonoGame.CORE.V2;
using Softfire.MonoGame.CORE.V2.Input;

namespace Softfire.MonoGame.IO.V2
{
    /// <summary>
    /// A Mouse class for using an installed mouse.
    /// </summary>
    public partial class IOMouse : IMonoGameInputComponent
    {
        /// <summary>
        /// THe time between updates.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// The mouse's elapsed time.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// Holds all current Mouse and Cursor information.
        /// </summary>
        private MouseState MouseState { get; set; }

        /// <summary>
        /// The previous <see cref="MouseState"/>. Compared against the <see cref="MouseState"/> to determine actions.
        /// </summary>
        private MouseState PreviousMouseState { get; set; }

        #region Fields

        /// <summary>
        /// The mouse's internal scroll speed value.
        /// </summary>
        private float _scrollSpeed = 8f;

        #endregion

        #region Properties

        /// <summary>
        /// The mouse's width.
        /// </summary>
        public int Width { get; set; } = 1;

        /// <summary>
        /// The mouse's height.
        /// </summary>
        public int Height { get; set; } = 1;

        /// <summary>
        /// The mouse's position.
        /// </summary>
        public Vector2 Position { get; set; }
        
        /// <summary>
        /// The mouse's rectangle.
        /// </summary>
        public RectangleF Rectangle { get; private set; }
        
        /// <summary>
        /// The mouse's previous rectangle.
        /// </summary>
        public RectangleF PreviousRectangle { get; private set; }
        
        /// <summary>
        /// The mouse's current scroll speed value.
        /// </summary>
        public float ScrollSpeed
        {
            get => _scrollSpeed;
            set => _scrollSpeed = value > 0 ? value : 1;
        }

        /// <summary>
        /// The mouse's scroll direction.
        /// </summary>
        public ScrollDirections ScrollDirection { get; set; } = ScrollDirections.Vertical;
        
        /// <summary>
        /// The mouse's available buttons.
        /// </summary>
        public enum MouseButtons
        {
            /// <summary>
            /// Use for undefined buttons.
            /// </summary>
            None,
            /// <summary>
            /// The mouse's left click button.
            /// </summary>
            LeftButton,
            /// <summary>
            /// The mouse's middle click button.
            /// </summary>
            MiddleButton,
            /// <summary>
            /// The mouse's right click button
            /// </summary>
            RightButton,
            /// <summary>
            /// The mouse's button number one.
            /// </summary>
            ButtonOne,
            /// <summary>
            /// The mouse's button number two.
            /// </summary>
            ButtonTwo
        }

        #endregion

        #region Booleans

        /// <summary>
        /// Determines whether the mouse is visible.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        #endregion

        /// <summary>
        /// A mouse.
        /// </summary>
        public IOMouse()
        {
            Rectangle = new RectangleF(-1, -1, Width, Height);
            PreviousRectangle = new RectangleF(-1, -1, Width, Height);
        }

        #region Buttons

        /// <summary>
        /// Determines whether the mouse button is in a pressed state.
        /// </summary>
        /// <param name="mouseState">The MouseState to inspect for the button's current state.</param>
        /// <param name="button">The button to check if it is pressed. Intaken as a <see cref="MouseButtons"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        /// <exception cref="ArgumentException">An argument exception is thrown indicating what type of button was pressed and could not be handled.</exception>
        private static bool IsButtonDown(MouseState mouseState, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.None:
                    return false;
                case MouseButtons.LeftButton:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.MiddleButton:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                case MouseButtons.RightButton:
                    return mouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.ButtonOne:
                    return mouseState.XButton1 == ButtonState.Pressed;
                case MouseButtons.ButtonTwo:
                    return mouseState.XButton2 == ButtonState.Pressed;
                default:
                    throw new ArgumentException($"The mouse feature of type {button} was pressed and is not currently handled.");
            }
        }

        /// <summary>
        /// Determines whether the mouse button is in a released state.
        /// </summary>
        /// <param name="mouseState">The MouseState to inspect for the button's current state.</param>
        /// <param name="button">The button to check if it is released. Intaken as a <see cref="MouseButtons"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        /// <exception cref="ArgumentException">An argument exception is thrown indicating what type of button was pressed and could not be handled.</exception>
        private static bool IsButtonUp(MouseState mouseState, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.None:
                    return false;
                case MouseButtons.LeftButton:
                    return mouseState.LeftButton == ButtonState.Released;
                case MouseButtons.MiddleButton:
                    return mouseState.MiddleButton == ButtonState.Released;
                case MouseButtons.RightButton:
                    return mouseState.RightButton == ButtonState.Released;
                case MouseButtons.ButtonOne:
                    return mouseState.XButton1 == ButtonState.Released;
                case MouseButtons.ButtonTwo:
                    return mouseState.XButton2 == ButtonState.Released;
                default:
                    throw new ArgumentException($"The mouse feature of type {button} was pressed and is not currently handled.");
            }
        }

        /// <summary>
        /// Determines whether the mouse button is in an pressed state.
        /// </summary>
        /// <param name="button">The button to check if it is in a pressed state. Intaken as a <see cref="MouseButtons"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool ButtonPress(MouseButtons button) => IsButtonDown(MouseState, button) && IsButtonUp(PreviousMouseState, button);

        /// <summary>
        /// Determines whether the mouse button is in a released state.
        /// </summary>
        /// <param name="button">The button to check if it is in a released state. Intaken as a <see cref="MouseButtons"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool ButtonRelease(MouseButtons button) => IsButtonUp(MouseState, button) && IsButtonDown(PreviousMouseState, button);

        /// <summary>
        /// Determines whether the mouse button is in a held state.
        /// </summary>
        /// <param name="button">The button to check if it is in a held state. Intaken as a <see cref="MouseButtons"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a held state.</returns>
        public bool ButtonHeld(MouseButtons button) => IsButtonDown(MouseState, button) && IsButtonDown(PreviousMouseState, button);

        #endregion

        #region Left Click

        /// <summary>
        /// Determines whether the mouse button is in an pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool LeftClickPress() => ButtonPress(MouseButtons.LeftButton);

        /// <summary>
        /// Determines whether the mouse button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool LeftClickRelease() => ButtonRelease(MouseButtons.LeftButton);

        /// <summary>
        /// Determines whether the mouse button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the mouse button is being held down.</returns>
        public bool LeftClickHeld() => ButtonHeld(MouseButtons.LeftButton);

        #endregion

        #region Middle Click

        /// <summary>
        /// Determines whether the mouse button is in an pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool MiddleClickPress() => ButtonPress(MouseButtons.MiddleButton);
        
        /// <summary>
        /// Determines whether the mouse button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool MiddleClickRelease() => ButtonRelease(MouseButtons.MiddleButton);

        /// <summary>
        /// Determines whether the mouse button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the mouse button is being held down.</returns>
        public bool MiddleClickHeld() => ButtonHeld(MouseButtons.MiddleButton);

        #endregion

        #region Right Click

        /// <summary>
        /// Determines whether the mouse button is in an pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool RightClickPress() => ButtonPress(MouseButtons.RightButton);

        /// <summary>
        /// Determines whether the mouse button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool RightClickRelease() => ButtonRelease(MouseButtons.RightButton);

        /// <summary>
        /// Determines whether the mouse button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the mouse button is being held down.</returns>
        public bool RightClickHeld() => ButtonHeld(MouseButtons.RightButton);

        #endregion

        #region Button One

        /// <summary>
        /// Determines whether the mouse button is in an pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool ButtonOneClickPress() => ButtonPress(MouseButtons.ButtonOne);

        /// <summary>
        /// Determines whether the mouse button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool ButtonOneClickRelease() => ButtonRelease(MouseButtons.ButtonOne);

        /// <summary>
        /// Determines whether the mouse button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the mouse button is being held down.</returns>
        public bool ButtonOneClickHeld() => ButtonHeld(MouseButtons.ButtonOne);

        #endregion

        #region Button Two

        /// <summary>
        /// Determines whether the mouse button is in an pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool ButtonTwoClickPress() => ButtonPress(MouseButtons.ButtonTwo);

        /// <summary>
        /// Determines whether the mouse button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool ButtonTwoClickRelease() => ButtonRelease(MouseButtons.ButtonTwo);

        /// <summary>
        /// Determines whether the mouse button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the mouse button is being held down.</returns>
        public bool ButtonTwoClickHeld() => ButtonHeld(MouseButtons.ButtonTwo);

        #endregion

        #region Scrolling

        /// <summary>
        /// Scrolls the position in the provided <see cref="ScrollDirection"/> by the provided <see cref="ScrollSpeed"/>.
        /// </summary>
        /// <param name="position">The position to scroll. Intaken as a reference of <see cref="Vector2"/>.</param>
        /// <param name="direction">The direction to scroll. Intaken as a <see cref="ScrollDirection"/>.</param>
        /// <param name="speed">The speed at which to perform the scroll. Intaken as a <see cref="float"/>.</param>
        /// <returns>Returns a modified <see cref="Vector2"/> after applying any scrolling changes.</returns>
        public Vector2 Scroll(Vector2 position, ScrollDirections direction, float speed)
        {
            ScrollDirection = direction;
            ScrollSpeed = speed;

            return Scroll(position);
        }

        /// <summary>
        /// Scrolls the position in the set <see cref="ScrollDirection"/> by the set <see cref="ScrollSpeed"/>.
        /// </summary>
        /// <param name="position">The position to scroll. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns a modified <see cref="Vector2"/> after applying any scrolling changes.</returns>
        public Vector2 Scroll(Vector2 position)
        {
            if (CheckForUpwardScroll())
            {
                position.Y -= ScrollSpeed;
                return position;
            }

            if (CheckForDownwardScroll())
            {
                position.Y += ScrollSpeed;
                return position;
            }

            if (CheckForLeftwardScroll())
            {
                position.X -= ScrollSpeed;
                return position;
            }

            if (CheckForRightwardScroll())
            {
                position.X += ScrollSpeed;
                return position;
            }

            return position;
        }

        /// <summary>
        /// Checks the mouse's scroll index for a change indicating an upward scroll was performed.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the mouse's <see cref="ScrollDirection"/> is set to <see cref="ScrollDirections.Vertical"/>
        /// and whether the difference in scroll indexes indicates an upward scroll was performed</returns>
        public bool CheckForUpwardScroll() => ScrollDirection == ScrollDirections.Vertical && MouseState.ScrollWheelValue > PreviousMouseState.ScrollWheelValue;

        /// <summary>
        /// Checks the mouse's scroll index for a change indicating a downward scroll was performed.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the mouse's <see cref="ScrollDirection"/> is set to <see cref="ScrollDirections.Vertical"/>
        /// and whether the difference in scroll indexes indicates an downward scroll was performed</returns>
        public bool CheckForDownwardScroll() => ScrollDirection == ScrollDirections.Vertical && MouseState.ScrollWheelValue < PreviousMouseState.ScrollWheelValue;

        /// <summary>
        /// Checks the mouse's scroll index for a change indicating an leftward scroll was performed.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the mouse's <see cref="ScrollDirection"/> is set to <see cref="ScrollDirections.Horizontal"/>
        /// and whether the difference in scroll indexes indicates an leftward scroll was performed</returns>
        public bool CheckForLeftwardScroll() => ScrollDirection == ScrollDirections.Horizontal && MouseState.ScrollWheelValue > PreviousMouseState.ScrollWheelValue;

        /// <summary>
        /// Checks the mouse's scroll index for a change indicating an rightward scroll was performed.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the mouse's <see cref="ScrollDirection"/> is set to <see cref="ScrollDirections.Horizontal"/>
        /// and whether the difference in scroll indexes indicates an rightward scroll was performed</returns>
        public bool CheckForRightwardScroll() => ScrollDirection == ScrollDirections.Horizontal && MouseState.ScrollWheelValue < PreviousMouseState.ScrollWheelValue;

        #endregion

        /// <summary>
        /// Calculates and returns the movement deltas between each mouse state update.
        /// </summary>
        /// <returns>Returns the movement deltas of the last movement of the mouse as a <see cref="Vector2"/>.</returns>
        public Vector2 GetMovementDeltas() => new Vector2(MouseState.X - PreviousMouseState.X,
                                                                 MouseState.Y - PreviousMouseState.Y);

        /// <summary>
        /// The mouse's update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame <see cref="GameTime"/>.</param>
        public void Update(GameTime gameTime)
        {
            // Update elapsed time and delta time.
            ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            // Update mouse states.
            PreviousMouseState = MouseState;
            MouseState = Mouse.GetState();
            
            Rectangle = IsRemoteControlled
                ? new RectangleF(RemoteState.X, RemoteState.Y, Width, Height)
                : new RectangleF(MouseState.X, MouseState.Y, Width, Height);

            PreviousRectangle = IsRemoteControlled
                ? new RectangleF(PreviousRemoteState.X, PreviousRemoteState.Y, Width, Height)
                : new RectangleF(PreviousMouseState.X, PreviousMouseState.Y, Width, Height);
        }
    }
}