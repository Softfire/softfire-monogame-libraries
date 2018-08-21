using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Softfire.MonoGame.ANIM;

namespace Softfire.MonoGame.IO
{
    public static class IOMouse
    {
        /// <summary>
        /// Delta Time.
        /// Time between updates.
        /// </summary>
        private static double DeltaTime { get; set; }

        /// <summary>
        /// Elapsed Time.
        /// </summary>
        private static double ElapsedTime { get; set; }

        /// <summary>
        /// Cursor Visibility.
        /// Enables/Disables the visibility of the Cursor.
        /// </summary>
        public static bool IsVisible { get; set; } = true;

        /// <summary>
        /// Is In Use?
        /// </summary>
        public static bool IsInUse { get; private set; }

        /// <summary>
        /// Is Remote Controlled?
        /// Enable to allow another device to control the mouse.
        /// </summary>
        public static bool IsRemoteControlled { get; set; }

        /// <summary>
        /// Use Custom Cursor?
        /// Enable to use a custom cursor.
        /// </summary>
        public static bool UseCustomCursor { get; set; }

        /// <summary>
        /// Current Mouse State.
        /// Holds all Mouse and Cursor information.
        /// </summary>
        private static MouseState MouseState { get; set; }

        /// <summary>
        /// Previous Mouse State.
        /// Used to identify Mouse actions based on Previous Mouse State.
        /// </summary>
        private static MouseState PreviousMouseState { get; set; }

        /// <summary>
        /// Cursor Animation.
        /// </summary>
        public static Animation Cursor { get; set; }
        
        /// <summary>
        /// Mouse Rectangle.
        /// </summary>
        public static Rectangle Rectangle { get; private set; }

        /// <summary>
        /// Mouse Position.
        /// </summary>
        public static Vector2 Position { get; set; }

        /// <summary>
        /// Remote State.
        /// The remote device's state. Used to remotely control the mouse.
        /// </summary>
        private static Vector2 RemoteState { get; set; }

        /// <summary>
        /// Boundaries Rectangle.
        /// If defined, boundaries are enforced.
        /// </summary>
        private static Rectangle BoundariesRectangle { get; set; } = Rectangle.Empty;

        /// <summary>
        /// Cursor States.
        /// Used by Animation.
        /// </summary>
        public static States State { get; set; } = States.Idle;

        /// <summary>
        /// Cursor States.
        /// </summary>
        public enum States
        {
            Idle,
            Hover,
            Click,
            Held
        }

        /// <summary>
        /// Mouse Scroll Wheel Index.
        /// </summary>
        private static int ScrollWheelIndex { get; set; }

        /// <summary>
        /// Previous Mouse Scroll Wheel Index.
        /// </summary>
        private static int PreviousScrollWheelIndex { get; set; }

        /// <summary>
        /// Scroll Speed.
        /// </summary>
        public static ScrollSpeeds ScrollSpeed { get; set; } = ScrollSpeeds.Low;

        /// <summary>
        /// Scrolll Speeds.
        /// </summary>
        public enum ScrollSpeeds
        {
            Low = 3,
            MediumLow = 5,
            Medium = 7,
            MediumHigh = 9,
            High = 11
        }

        /// <summary>
        /// Scroll Direction.
        /// </summary>
        public static ScrollDirections ScrollDirection { get; set; } = ScrollDirections.Vertical;

        /// <summary>
        /// Scroll Directions.
        /// </summary>
        public enum ScrollDirections
        {
            Vertical,
            Horizontal
        }

        /// <summary>
        /// Buttons.
        /// </summary>
        public enum Buttons : byte
        {
            /// <summary>
            /// Use for undefined buttons.
            /// </summary>
            Undefined,
            /// <summary>
            /// Left Mouse Button.
            /// </summary>
            Left,
            /// <summary>
            /// Middle Mouse Button.
            /// </summary>
            Middle,
            /// <summary>
            /// Right Mouse Button.
            /// </summary>
            Right,
            /// <summary>
            /// Mouse Button One.
            /// </summary>
            One,
            /// <summary>
            /// Mouse Button Two.
            /// </summary>
            Two
        }

        #region Buttons

        /// <summary>
        /// Is Button Down?
        /// Gets whether given button is currently being pressed.
        /// </summary>
        /// <param name="mouseState">The MouseState to inspect for the button's current state.</param>
        /// <param name="button">The button to check if it is pressed.</param>
        /// <returns>Returns a boolean indicating whether the button is pressed.</returns>
        /// <exception cref="ArgumentException"></exception>
        private static bool IsButtonDown(this MouseState mouseState, Buttons button)
        {
            switch (button)
            {
                case Buttons.Undefined:
                    return false;
                case Buttons.Left:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case Buttons.Middle:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                case Buttons.Right:
                    return mouseState.RightButton == ButtonState.Pressed;
                case Buttons.One:
                    return mouseState.XButton1 == ButtonState.Pressed;
                case Buttons.Two:
                    return mouseState.XButton2 == ButtonState.Pressed;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Is Button Up?
        /// Gets whether given button is currently not being pressed.
        /// </summary>
        /// <param name="mouseState">The MouseState to inspect for the button's current state.</param>
        /// <param name="button">The button to check if it is released.</param>
        /// <returns>Returns a boolean indicating whether the button is released.</returns>
        /// <exception cref="ArgumentException"></exception>
        private static bool IsButtonUp(this MouseState mouseState, Buttons button)
        {
            switch (button)
            {
                case Buttons.Undefined:
                    return false;
                case Buttons.Left:
                    return mouseState.LeftButton == ButtonState.Released;
                case Buttons.Middle:
                    return mouseState.MiddleButton == ButtonState.Released;
                case Buttons.Right:
                    return mouseState.RightButton == ButtonState.Released;
                case Buttons.One:
                    return mouseState.XButton1 == ButtonState.Released;
                case Buttons.Two:
                    return mouseState.XButton2 == ButtonState.Released;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Button Idle.
        /// Gets whether given button is currently idle.
        /// </summary>
        /// <param name="button">The button to check if it is idle.</param>
        /// <returns>Returns a boolean indicating whether the button is idle.</returns>
        public static bool ButtonIdle(Buttons button)
        {
            return MouseState.IsButtonUp(button) && PreviousMouseState.IsButtonUp(button);
        }

        /// <summary>
        /// Button Press.
        /// Gets whether given button is currently being pressed.
        /// </summary>
        /// <param name="button">The button to check if it was pressed.</param>
        /// <returns>Returns a boolean indicating whether the button was pressed.</returns>
        public static bool ButtonPress(Buttons button)
        {
            return MouseState.IsButtonDown(button) && PreviousMouseState.IsButtonUp(button);
        }

        /// <summary>
        /// Button Release.
        /// Gets whether given button is currently not being pressed.
        /// </summary>
        /// <param name="button">The button to check if it was released.</param>
        /// <returns>Returns a boolean indicating whether the button was released.</returns>
        public static bool ButtonRelease(Buttons button)
        {
            return PreviousMouseState.IsButtonDown(button) && MouseState.IsButtonUp(button);
        }

        /// <summary>
        /// Button Held.
        /// Gets whether given button is currently being pressed and held.
        /// </summary>
        /// <param name="button">The button to check if it is being held down.</param>
        /// <returns>Returns a boolean indicating whether the button is being held down.</returns>
        public static bool ButtonHeld(Buttons button)
        {
            return MouseState.IsButtonDown(button) && PreviousMouseState.IsButtonDown(button);
        }

        #endregion

        #region Left Click

        /// <summary>
        /// Left Click Idle.
        /// Gets whether the left mouse button is idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left mouse button is idle.</returns>
        public static bool LeftClickIdle()
        {
            return ButtonIdle(Buttons.Left);
        }

        /// <summary>
        /// Left Click Idle.
        /// Gets whether the left mouse button is idle inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the left mouse button was was inside the supplied rectangle.</returns>
        public static bool LeftClickIdle(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && LeftClickIdle();
        }

        /// <summary>
        /// Left Click Press.
        /// Gets whether the left mouse button was pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left mouse button was pressed.</returns>
        public static bool LeftClickPress()
        {
            return ButtonPress(Buttons.Left);
        }

        /// <summary>
        /// Left Click Press.
        /// Gets whether the left mouse button was pressed inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the left mouse button was pressed inside the supplied rectangle.</returns>
        public static bool LeftClickPress(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && LeftClickPress();
        }

        /// <summary>
        /// Left Click Release.
        /// Gets whether the left mouse button was released.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left mouse button was released.</returns>
        public static bool LeftClickRelease()
        {
            return ButtonRelease(Buttons.Left);
        }

        /// <summary>
        /// Left Click Release.
        /// Gets whether the left mouse button was released inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the left mouse button was released inside the supplied rectangle.</returns>
        public static bool LeftClickRelease(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && LeftClickRelease();
        }

        /// <summary>
        /// Left Click Held.
        /// Gets whether the left mouse button is being held down.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left mouse button is being held down.</returns>
        public static bool LeftClickHeld()
        {
            return ButtonHeld(Buttons.Left);
        }

        /// <summary>
        /// Left Click Held.
        /// Gets whether the left mouse button was being held down inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the left mouse button was being held down inside the supplied rectangle.</returns>
        public static bool LeftClickHeld(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && LeftClickHeld();
        }

        #endregion

        #region Middle Click

        /// <summary>
        /// Middle Click Idle.
        /// Gets whether the middle mouse button is idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the middle mouse button is idle.</returns>
        public static bool MiddleClickIdle()
        {
            return ButtonIdle(Buttons.Middle);
        }

        /// <summary>
        /// Middle Click Idle.
        /// Gets whether the middle mouse button is idle inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the middle mouse button is idle inside the supplied rectangle.</returns>
        public static bool MiddleClickIdle(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && MiddleClickIdle();
        }

        /// <summary>
        /// Middle Click Press.
        /// Gets whether the middle mouse button is being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the middle mouse button was pressed.</returns>
        public static bool MiddleClickPress()
        {
            return ButtonPress(Buttons.Middle);
        }

        /// <summary>
        /// Middle Click Press.
        /// Gets whether the middle mouse button was pressed inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the middle mouse button was pressed inside the supplied rectangle.</returns>
        public static bool MiddleClickPress(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && MiddleClickPress();
        }

        /// <summary>
        /// Middle Click Release.
        /// Gets whether the middle mouse button was released.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the middle mouse button was released.</returns>
        public static bool MiddleClickRelease()
        {
            return ButtonRelease(Buttons.Middle);
        }

        /// <summary>
        /// Middle Click Release.
        /// Gets whether the middle mouse button was released inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the middle mouse button was released inside the supplied rectangle.</returns>
        public static bool MiddleClickRelease(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && MiddleClickRelease();
        }

        /// <summary>
        /// Middle Click Held.
        /// Gets whether the middle mouse button is being pressed and held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the middle mouse button is being held down.</returns>
        public static bool MiddleClickHeld()
        {
            return ButtonHeld(Buttons.Middle);
        }

        /// <summary>
        /// Middle Click Held.
        /// Gets whether the middle mouse button is being pressed and held inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the middle mouse button is being held down inside the supplied rectangle.</returns>
        public static bool MiddleClickHeld(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && MiddleClickHeld();
        }

        #endregion

        #region Right Click

        /// <summary>
        /// Right Click Idle.
        /// Gets whether the right mouse button is idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right mouse button is idle.</returns>
        public static bool RightClickIdle()
        {
            return ButtonIdle(Buttons.Right);
        }

        /// <summary>
        /// Right Click Idle.
        /// Gets whether the right mouse button is idle inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the right mouse button is idle inside the supplied rectangle.</returns>
        public static bool RightClickIdle(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && RightClickIdle();
        }

        /// <summary>
        /// Right Click Press.
        /// Gets whether the right mouse button is being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right mouse button was pressed.</returns>
        public static bool RightClickPress()
        {
            return ButtonPress(Buttons.Right);
        }

        /// <summary>
        /// Right Click Press.
        /// Gets whether the right mouse button was pressed inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the right mouse button was pressed inside the supplied rectangle.</returns>
        public static bool RightClickPress(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && RightClickPress();
        }

        /// <summary>
        /// Right Click Release.
        /// Gets whether the right mouse button was released.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right mouse button was released.</returns>
        public static bool RightClickRelease()
        {
            return ButtonRelease(Buttons.Right);
        }

        /// <summary>
        /// Right Click Release.
        /// Gets whether the right mouse button was released inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the right mouse button was released inside the supplied rectangle.</returns>
        public static bool RightClickRelease(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && RightClickRelease();
        }

        /// <summary>
        /// Right Click Held.
        /// Gets whether the right mouse button is being pressed and held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right mouse button is being held down.</returns>
        public static bool RightClickHeld()
        {
            return ButtonHeld(Buttons.Right);
        }

        /// <summary>
        /// Right Click Held.
        /// Gets whether the right mouse button is being pressed and held inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the right mouse button is being held down inside the supplied rectangle.</returns>
        public static bool RightClickHeld(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && RightClickHeld();
        }

        #endregion

        #region Button One

        /// <summary>
        /// Button One Idle.
        /// Gets whether mouse button one is idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button one is idle.</returns>
        public static bool ButtonOneClickIdle()
        {
            return ButtonIdle(Buttons.One);
        }

        /// <summary>
        /// Button One Idle.
        /// Gets whether mouse button one is idle inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button one is idle inside the supplied rectangle.</returns>
        public static bool ButtonOneClickIdle(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && ButtonOneClickIdle();
        }

        /// <summary>
        /// Button One Press.
        /// Gets whether mouse button one is being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button one was pressed.</returns>
        public static bool ButtonOneClickPress()
        {
            return ButtonPress(Buttons.One);
        }

        /// <summary>
        /// Button One Press.
        /// Gets whether mouse button one was pressed inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button one was pressed inside the supplied rectangle.</returns>
        public static bool ButtonOneClickPress(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && ButtonOneClickPress();
        }

        /// <summary>
        /// Button One Release.
        /// Gets whether mouse button one was released.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button one was released.</returns>
        public static bool ButtonOneClickRelease()
        {
            return ButtonRelease(Buttons.One);
        }

        /// <summary>
        /// Button One Release.
        /// Gets whether mouse button one was released inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button one was released inside the supplied rectangle.</returns>
        public static bool ButtonOneClickRelease(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && ButtonOneClickRelease();
        }

        /// <summary>
        /// Button One Held.
        /// Gets whether mouse button one is being pressed and held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button one is being held down.</returns>
        public static bool ButtonOneClickHeld()
        {
            return ButtonHeld(Buttons.One);
        }

        /// <summary>
        /// Button One Held.
        /// Gets whether mouse button one is being pressed and held inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button one is being held down inside the supplied rectangle.</returns>
        public static bool ButtonOneClickHeld(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && ButtonOneClickHeld();
        }

        #endregion

        #region Button Two

        /// <summary>
        /// Button Two Idle.
        /// Gets whether mouse button two is idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button two is idle.</returns>
        public static bool ButtonTwoClickIdle()
        {
            return ButtonIdle(Buttons.Two);
        }

        /// <summary>
        /// Button Two Idle.
        /// Gets whether mouse button two is idle inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button two is idle inside the supplied rectangle.</returns>
        public static bool ButtonTwoClickIdle(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && ButtonTwoClickIdle();
        }

        /// <summary>
        /// Button Two Press.
        /// Gets whether mouse button two is being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button two was pressed.</returns>
        public static bool ButtonTwoClickPress()
        {
            return ButtonPress(Buttons.Two);
        }

        /// <summary>
        /// Button Two Press.
        /// Gets whether mouse button two was pressed inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button two was pressed inside the supplied rectangle.</returns>
        public static bool ButtonTwoClickPress(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && ButtonTwoClickPress();
        }

        /// <summary>
        /// Button Two Release.
        /// Gets whether mouse button two was released.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button two was released.</returns>
        public static bool ButtonTwoClickRelease()
        {
            return ButtonRelease(Buttons.Two);
        }

        /// <summary>
        /// Button Two Release.
        /// Gets whether mouse button two was released inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button two was released inside the supplied rectangle.</returns>
        public static bool ButtonTwoClickRelease(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && ButtonTwoClickRelease();
        }

        /// <summary>
        /// Button Two Held.
        /// Gets whether mouse button two is being pressed and held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button two is being held down.</returns>
        public static bool ButtonTwoClickHeld()
        {
            return ButtonHeld(Buttons.Two);
        }

        /// <summary>
        /// Button Two Held.
        /// Gets whether mouse button two is being pressed and held inside the supplied Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button two is being held down inside the supplied rectangle.</returns>
        public static bool ButtonTwoClickHeld(Rectangle rectangle)
        {
            return CheckBounds(rectangle) && ButtonTwoClickHeld();
        }

        #endregion

        /// <summary>
        /// Check Bounds.
        /// Checks supplied Rectangle for interaction with Mouse.
        /// </summary>
        /// <param name="bounds">The Rectangle to check for intersection.</param>
        /// <returns>Returns a boolean indicating whether the Mouse is intersecting or is contained within the Rectangle</returns>
        public static bool CheckBounds(Rectangle bounds)
        {
            return Rectangle.Intersects(bounds) || bounds.Contains(Rectangle);
        }

        /// <summary>
        /// Hover.
        /// </summary>
        /// <param name="rectangle">Intakes a Rectangle to check whether the mouse is contained within.</param>
        /// <param name="hoverLength">Intakes a double to define the length of time the Mouse can hover before triggering. Default is 1.0 second.</param>
        /// <returns>Returns whether the Mouse is hovering over the Rectangle.</returns>
        public static bool Hover(Rectangle rectangle, double hoverLength = 1.0)
        {
            var result = false;

            if (rectangle.Contains(Rectangle))
            {
                ElapsedTime += DeltaTime;

                if (ElapsedTime >= hoverLength)
                {
                    State = States.Hover;
                    result = true;
                }
            }
            else
            {
                State = States.Idle;
                ElapsedTime = 0.0;
            }

            return result;
        }

        /// <summary>
        /// Scroll.
        /// </summary>
        /// <param name="viewRectangle">The view to scroll. Intaken as a Rectangle.</param>
        /// <returns>Returns a Rectangle modified by the Mouse Wheel.</returns>
        public static Rectangle Scroll(Rectangle viewRectangle)
        {
            switch (ScrollDirection)
            {
                case ScrollDirections.Vertical:
                    // Mouse wheel up to move up.
                    if (ScrollWheelIndex > PreviousScrollWheelIndex)
                    {
                        viewRectangle.Y -= (int)ScrollSpeed;
                    }
                    // Mouse wheel down to move down.
                    else if (ScrollWheelIndex < PreviousScrollWheelIndex)
                    {
                        viewRectangle.Y += (int)ScrollSpeed;
                    }

                    break;

                case ScrollDirections.Horizontal:
                    // Mouse wheel up to move left.
                    if (ScrollWheelIndex > PreviousScrollWheelIndex)
                    {
                        viewRectangle.X -= (int)ScrollSpeed;
                    }
                    // Mouse wheel down to move right.
                    else if (ScrollWheelIndex < PreviousScrollWheelIndex)
                    {
                        viewRectangle.X += (int)ScrollSpeed;
                    }

                    break;
            }

            return viewRectangle;
        }

        /// <summary>
        /// Movement Delta.
        /// </summary>
        /// <returns>Returns a Vector2 containing the movement deltas of the last movement of the mouse.</returns>
        public static Vector2 MovementDelta()
        {
            var deltaX = MouseState.X - PreviousMouseState.X;
            var deltaY = MouseState.Y - PreviousMouseState.Y;

            return new Vector2(deltaX, deltaY);
        }

        /// <summary>
        /// Remote Control.
        /// Update with controller vectors to control mouse.
        /// </summary>
        /// <param name="remoteVector">The vectors of the remote device.</param>
        public static void RemoteControl(Vector2 remoteVector)
        {
            RemoteState += remoteVector;
        }

        /// <summary>
        /// Menu Entry Selection By Detection.
        /// </summary>
        /// <param name="currentMenuEntryIndex">The current menu item index.</param>
        /// <param name="menuItemIndex">The menu item index of the current menu item.</param>
        /// <param name="menuEntryRectangle">The menu entry's rectangle.</param>
        /// <returns>Returns the menu item number of the current menu item if it contains the IOMouse's rectangle.</returns>
        public static int MenuEntrySelectionByDetection(int currentMenuEntryIndex, int menuItemIndex, Rectangle menuEntryRectangle)
        {
            return menuEntryRectangle.Contains(Rectangle) ? menuItemIndex : currentMenuEntryIndex;
        }

        /// <summary>
        /// Menu Entry Selection By Scroll.
        /// Scroll up or down to change menu entry.
        /// </summary>
        /// <param name="currentMenuEntryIndex">The current menu entry index. Intaken as a int.</param>
        /// <param name="firstEntry">The first menu entry in the menu part. Intaken as an int.</param>
        /// <param name="lastEntry">The last menu entry in the menu part. Intaken as an int.</param>
        /// <param name="firstMenu">A bool indicating whether this is the first menu in a series.</param>
        /// <param name="lastMenu">A bool indicating whether this is the last menu in a series.</param>
        /// <param name="isLoopingOnEntries">A bool indicating whether the menu index will return to the first or last entry upon exceeding the menu entry index.</param>
        /// <returns>Returns the selected menu entry index as per the scroll action taken.</returns>
        public static int MenuEntrySelectionByScroll(int currentMenuEntryIndex,
                                                     int firstEntry = 1, int lastEntry = 1,
                                                     bool firstMenu = true, bool lastMenu = true,
                                                     bool isLoopingOnEntries = true)
        {
            if (currentMenuEntryIndex >= firstEntry &&
                currentMenuEntryIndex <= lastEntry)
            {
                // Mouse wheel up.
                if (ScrollWheelIndex > PreviousScrollWheelIndex)
                {
                    currentMenuEntryIndex--;

                    if (firstMenu)
                    {
                        if (currentMenuEntryIndex < firstEntry)
                        {
                            currentMenuEntryIndex = isLoopingOnEntries == false ? firstEntry : lastEntry;
                        }
                    }
                }
                // Mouse wheel down.
                else if (ScrollWheelIndex < PreviousScrollWheelIndex)
                {
                    currentMenuEntryIndex++;

                    if (lastMenu)
                    {
                        if (currentMenuEntryIndex > lastEntry)
                        {
                            currentMenuEntryIndex = isLoopingOnEntries == false ? lastEntry : firstEntry;
                        }
                    }
                }
            }
            else if (currentMenuEntryIndex < firstEntry)
            {
                currentMenuEntryIndex = firstEntry;
            }
            else if (currentMenuEntryIndex > lastEntry)
            {
                currentMenuEntryIndex = lastEntry;
            }

            return currentMenuEntryIndex;
        }

        /// <summary>
        /// Boundaries.
        /// Keeps Mouse Cursor within the BoundariesRectangle.
        /// </summary>
        private static Vector2 Boundaries()
        {
            var mousePosition = new Vector2(MouseState.X, MouseState.Y);

            // Left
            if (MouseState.X < 0)
            {
                mousePosition = new Vector2(0, MouseState.Y);
            }

            // Right
            if (MouseState.X > BoundariesRectangle.Width)
            {
                mousePosition = new Vector2(BoundariesRectangle.Width, MouseState.Y);
            }

            // Top
            if (MouseState.Y < 0)
            {
                mousePosition = new Vector2(MouseState.X, 0);
            }

            // Bottom
            if (MouseState.Y > BoundariesRectangle.Height)
            {
                mousePosition = new Vector2(MouseState.X, BoundariesRectangle.Height);
            }

            // Top Left
            if (MouseState.X < 0 && MouseState.Y < 0)
            {
                mousePosition = new Vector2(0, 0);
            }

            // Top Right
            if (MouseState.X > BoundariesRectangle.Width && MouseState.Y < 0)
            {
                mousePosition = new Vector2(BoundariesRectangle.Width, 0);
            }

            // Bottom Left
            if (MouseState.X < 0 && MouseState.Y > BoundariesRectangle.Height)
            {
                mousePosition = new Vector2(0, BoundariesRectangle.Height);
            }

            // Bottom Right
            if (MouseState.X > BoundariesRectangle.Width && MouseState.Y > BoundariesRectangle.Height)
            {
                mousePosition = new Vector2(BoundariesRectangle.Width, BoundariesRectangle.Height);
            }

            return mousePosition;
        }

        public static void SetBoundaries(Rectangle bounds)
        {
            BoundariesRectangle = bounds;
        }

        public static void ResetBoundaries()
        {
            SetBoundaries(Rectangle.Empty);
        }

        /// <summary>
        /// Rotates the position towards the Cursor.
        /// </summary>
        /// <param name="position">An object's position. Intaken as a Vector2.</param>
        /// <returns>Returns the rotated angle pointing towards the cursor.</returns>
        public static float RotateTowardsCursor(Vector2 position)
        {
            return -(float) Math.Atan2(position.X - Position.X, position.Y - Position.Y);
        }

        /// <summary>
        /// Mouse Update Method.
        /// Update MouseStates.
        /// </summary>
        /// <param name="gameTime">Intakes a MonoGame GameTime instance.</param>
        public static void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            PreviousMouseState = MouseState;
            MouseState = Mouse.GetState();

            PreviousScrollWheelIndex = ScrollWheelIndex;
            ScrollWheelIndex = MouseState.ScrollWheelValue;

            IsInUse = MouseState != PreviousMouseState;

            Rectangle = IsRemoteControlled ? new Rectangle((int)RemoteState.X, (int)RemoteState.Y, 1, 1) : new Rectangle(MouseState.X, MouseState.Y, 1, 1);

            Position = BoundariesRectangle != Rectangle.Empty ? Boundaries() : new Vector2(Rectangle.X, Rectangle.Y);

            if (UseCustomCursor &&
                Cursor != null)
            {
                // TODO: Correct
                //Cursor.SetAction(State.ToString());
                Cursor.Position = Position;

                Cursor.Update(gameTime);
            }
        }

        /// <summary>
        /// Mouse Draw Method
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible &&
                UseCustomCursor)
            {
                Cursor?.Draw(spriteBatch);
            }
        }
    }
}