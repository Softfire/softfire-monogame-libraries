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
        /// Are Boundaries Enforced?
        /// </summary>
        private static bool AreBoundariesEnforced { get; set; }

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
        /// Gets whether the left mouse button is currently idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left mouse button is idle.</returns>
        public static bool LeftClickIdle()
        {
            return ButtonIdle(Buttons.Left);
        }

        /// <summary>
        /// Left Click Press.
        /// Gets whether the left mouse button is currently being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left mouse button was pressed.</returns>
        public static bool LeftClickPress()
        {
            return ButtonPress(Buttons.Left);
        }

        /// <summary>
        /// Left Click Release.
        /// Gets whether the left mouse button is currently not being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left mouse button was released.</returns>
        public static bool LeftClickRelease()
        {
            return ButtonRelease(Buttons.Left);
        }

        /// <summary>
        /// Left Click Held.
        /// Gets whether the left mouse button is currently being pressed and held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left mouse button is being held down.</returns>
        public static bool LeftClickHeld()
        {
            return ButtonHeld(Buttons.Left);
        }

        /// <summary>
        /// Left Click Press Inside.
        /// Gets whether the left mouse button was pressed inside the provided Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the left mouse button was pressed inside the supplied rectangle.</returns>
        public static bool LeftClickPressInside(Rectangle rectangle)
        {
            return (Rectangle.Intersects(rectangle) || rectangle.Contains(Rectangle)) &&
                   LeftClickPress();
        }

        /// <summary>
        /// Left Click Release Inside.
        /// Gets whether the left mouse button was released inside the provided Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the left mouse button was released inside the supplied rectangle.</returns>
        public static bool LeftClickReleaseInside(Rectangle rectangle)
        {
            return (Rectangle.Intersects(rectangle) || rectangle.Contains(Rectangle)) &&
                   LeftClickRelease();
        }

        #endregion

        #region Middle Click

        /// <summary>
        /// Middle Click Idle.
        /// Gets whether the middle mouse button is currently idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the middle mouse button is idle.</returns>
        public static bool MiddleClickIdle()
        {
            return ButtonIdle(Buttons.Middle);
        }

        /// <summary>
        /// Middle Click Press.
        /// Gets whether the middle mouse button is currently being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the middle mouse button was pressed.</returns>
        public static bool MiddleClickPress()
        {
            return ButtonPress(Buttons.Middle);
        }

        /// <summary>
        /// Middle Click Release.
        /// Gets whether the middle mouse button is currently not being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the middle mouse button was released.</returns>
        public static bool MiddleClickRelease()
        {
            return ButtonRelease(Buttons.Middle);
        }

        /// <summary>
        /// Middle Click Held.
        /// Gets whether the middle mouse button is currently being pressed and held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the middle mouse button is being held down.</returns>
        public static bool MiddleClickHeld()
        {
            return ButtonHeld(Buttons.Middle);
        }

        /// <summary>
        /// Middle Click Press Inside.
        /// Gets whether the middle mouse button was pressed inside the provided Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the middle mouse button was pressed inside the supplied rectangle.</returns>
        public static bool MiddleClickPressInside(Rectangle rectangle)
        {
            return (Rectangle.Intersects(rectangle) || rectangle.Contains(Rectangle)) &&
                   MiddleClickPress();
        }

        /// <summary>
        /// Middle Click Release Inside.
        /// Gets whether the middle mouse button was released inside the provided Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the middle mouse button was released inside the supplied rectangle.</returns>
        public static bool MiddleClickReleaseInside(Rectangle rectangle)
        {
            return (Rectangle.Intersects(rectangle) || rectangle.Contains(Rectangle)) &&
                   MiddleClickRelease();
        }

        #endregion

        #region Right Click

        /// <summary>
        /// Right Click Idle.
        /// Gets whether the right mouse button is currently idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right mouse button is idle.</returns>
        public static bool RightClickIdle()
        {
            return ButtonIdle(Buttons.Right);
        }

        /// <summary>
        /// Right Click Press.
        /// Gets whether the right mouse button is currently being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right mouse button was pressed.</returns>
        public static bool RightClickPress()
        {
            return ButtonPress(Buttons.Right);
        }

        /// <summary>
        /// Right Click Release.
        /// Gets whether the right mouse button is currently not being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right mouse button was released.</returns>
        public static bool RightClickRelease()
        {
            return ButtonRelease(Buttons.Right);
        }

        /// <summary>
        /// Right Click Held.
        /// Gets whether the right mouse button is currently being pressed and held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right mouse button is being held down.</returns>
        public static bool RightClickHeld()
        {
            return ButtonHeld(Buttons.Right);
        }

        /// <summary>
        /// Right Click Press Inside.
        /// Gets whether the right mouse button was pressed inside the provided Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the right mouse button was pressed inside the supplied rectangle.</returns>
        public static bool RightClickPressInside(Rectangle rectangle)
        {
            return (Rectangle.Intersects(rectangle) || rectangle.Contains(Rectangle)) &&
                   RightClickPress();
        }

        /// <summary>
        /// Right Click Release Inside.
        /// Gets whether the right mouse button was released inside the provided Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether the right mouse button was released inside the supplied rectangle.</returns>
        public static bool RightClickReleaseInside(Rectangle rectangle)
        {
            return (Rectangle.Intersects(rectangle) || rectangle.Contains(Rectangle)) &&
                   RightClickRelease();
        }

        #endregion

        #region Button One

        /// <summary>
        /// Button One Click Idle.
        /// Gets whether mouse button one is currently idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button one is idle.</returns>
        public static bool ButtonOneClickIdle()
        {
            return ButtonIdle(Buttons.One);
        }

        /// <summary>
        /// Button One Click Press.
        /// Gets whether mouse button one is currently being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button one was pressed.</returns>
        public static bool ButtonOneClickPress()
        {
            return ButtonPress(Buttons.One);
        }

        /// <summary>
        /// Button One Click Release.
        /// Gets whether mouse button one is currently not being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button one was released.</returns>
        public static bool ButtonOneClickRelease()
        {
            return ButtonRelease(Buttons.One);
        }

        /// <summary>
        /// Button One Click Held.
        /// Gets whether mouse button one is currently being pressed and held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button one is being held down.</returns>
        public static bool ButtonOneClickHeld()
        {
            return ButtonHeld(Buttons.One);
        }

        /// <summary>
        /// Button One Click Press Inside.
        /// Gets whether mouse button one was pressed inside the provided Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button one was pressed inside the supplied rectangle.</returns>
        public static bool ButtonOneClickPressInside(Rectangle rectangle)
        {
            return (Rectangle.Intersects(rectangle) || rectangle.Contains(Rectangle)) &&
                   ButtonOneClickPress();
        }

        /// <summary>
        /// Button One Click Release Inside.
        /// Gets whether mouse button one was released inside the provided Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button one was released inside the supplied rectangle.</returns>
        public static bool ButtonOneClickReleaseInside(Rectangle rectangle)
        {
            return (Rectangle.Intersects(rectangle) || rectangle.Contains(Rectangle)) &&
                   ButtonOneClickRelease();
        }

        #endregion

        #region Button Two

        /// <summary>
        /// Button Two Click Idle.
        /// Gets whether mouse button two is currently idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button two is idle.</returns>
        public static bool ButtonTwoClickIdle()
        {
            return ButtonIdle(Buttons.Two);
        }

        /// <summary>
        /// Button Two Click Press.
        /// Gets whether mouse button two is currently being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button two was pressed.</returns>
        public static bool ButtonTwoClickPress()
        {
            return ButtonPress(Buttons.Two);
        }

        /// <summary>
        /// Button Two Click Release.
        /// Gets whether mouse button two is currently not being pressed.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button two was released.</returns>
        public static bool ButtonTwoClickRelease()
        {
            return ButtonRelease(Buttons.Two);
        }

        /// <summary>
        /// Button Two Click Held.
        /// Gets whether mouse button two is currently being pressed and held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether mouse button two is being held down.</returns>
        public static bool ButtonTwoClickHeld()
        {
            return ButtonHeld(Buttons.Two);
        }

        /// <summary>
        /// Button Two Click Press Inside.
        /// Gets whether mouse button two was pressed inside the provided Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button two was pressed inside the supplied rectangle.</returns>
        public static bool ButtonTwoClickPressInside(Rectangle rectangle)
        {
            return (Rectangle.Intersects(rectangle) || rectangle.Contains(Rectangle)) &&
                   ButtonTwoClickPress();
        }

        /// <summary>
        /// Button Two Click Release Inside.
        /// Gets whether mouse button two was released inside the provided Rectangle.
        /// </summary>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>Returns a boolean indicating whether mouse button two was released inside the supplied rectangle.</returns>
        public static bool ButtonTwoClickReleaseInside(Rectangle rectangle)
        {
            return (Rectangle.Intersects(rectangle) || rectangle.Contains(Rectangle)) &&
                   ButtonTwoClickRelease();
        }

        #endregion

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
        /// Keeps Mouse Cursor within the provided rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to contain the mouse cursor.</param>
        public static void Boundaries(Rectangle rectangle)
        {
            AreBoundariesEnforced = true;

            var mousePosition = new Vector2(MouseState.X, MouseState.Y);

            // Left
            if (MouseState.X < 0)
            {
                mousePosition = new Vector2(0, MouseState.Y);
            }

            // Right
            if (MouseState.X > rectangle.Width)
            {
                mousePosition = new Vector2(rectangle.Width, MouseState.Y);
            }

            // Top
            if (MouseState.Y < 0)
            {
                mousePosition = new Vector2(MouseState.X, 0);
            }

            // Bottom
            if (MouseState.Y > rectangle.Height)
            {
                mousePosition = new Vector2(MouseState.X, rectangle.Height);
            }

            // Top Left
            if (MouseState.X < 0 && MouseState.Y < 0)
            {
                mousePosition = new Vector2(0, 0);
            }

            // Top Right
            if (MouseState.X > rectangle.Width && MouseState.Y < 0)
            {
                mousePosition = new Vector2(rectangle.Width, 0);
            }

            // Bottom Left
            if (MouseState.X < 0 && MouseState.Y > rectangle.Height)
            {
                mousePosition = new Vector2(0, rectangle.Height);
            }

            // Bottom Right
            if (MouseState.X > rectangle.Width && MouseState.Y > rectangle.Height)
            {
                mousePosition = new Vector2(rectangle.Width, rectangle.Height);
            }

            Position = mousePosition;
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

            Rectangle = new Rectangle(MouseState.X, MouseState.Y, 1, 1);

            if (!AreBoundariesEnforced)
            {
                Position = new Vector2(Rectangle.X, Rectangle.Y);
            }

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