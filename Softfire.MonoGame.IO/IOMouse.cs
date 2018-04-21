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
        public static bool IsVisible { get; set; }

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
        public static States State { get; set; }

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
        /// Mouse Wheel Scroll Index.
        /// </summary>
        private static int ScrollIndex { get; set; }

        /// <summary>
        /// Previous Mouse Wheel Scroll Index.
        /// </summary>
        private static int PreviousScrollIndex { get; set; }

        /// <summary>
        /// Scroll Speed.
        /// </summary>
        public static ScrollSpeeds ScrollSpeed { get; set; }

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
        public static ScrollDirections ScrollDirection { get; set; }

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
            Left,
            Middle,
            Right,
            One,
            Two
        }

        /// <summary>
        /// IOMouse Constructor.
        /// </summary>
        static IOMouse()
        {
            IsVisible = false;
            UseCustomCursor = false;
            AreBoundariesEnforced = false;

            State = States.Idle;
            ScrollSpeed = ScrollSpeeds.Low;
            ScrollDirection = ScrollDirections.Vertical;
        }

        #region Left Click

        /// <summary>
        /// Left Click Down Method.
        /// Detects if a left click was performed.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool LeftClickDown()
        {
            State = States.Click;

            return PreviousMouseState.LeftButton == ButtonState.Released &&
                   MouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Left Click Up Method.
        /// Detects if a left click was released.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool LeftClickUp()
        {
            State = States.Click;

            return PreviousMouseState.LeftButton == ButtonState.Pressed &&
                   MouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Left Click Down Inside.
        /// Detects if a left click was performed inside the specified Rectangle.
        /// </summary>
        /// <param name="rectangle">Specified Rectangle in which to detect the left click.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool LeftClickDownInside(Rectangle rectangle)
        {
            State = States.Click;

            return rectangle.Contains(Rectangle) &&
                   PreviousMouseState.LeftButton == ButtonState.Released &&
                   MouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Left Click Up Inside.
        /// Detects if a left click was released inside the specified Rectangle.
        /// </summary>
        /// <param name="rectangle">Specified Rectangle in which to detect the left click.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool LeftClickUpInside(Rectangle rectangle)
        {
            State = States.Click;

            return rectangle.Contains(Rectangle) &&
                   PreviousMouseState.LeftButton == ButtonState.Pressed &&
                   MouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Left Click Held.
        /// Detects if a left click is being held.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool LeftClickHeld()
        {
            State = States.Held;

            return PreviousMouseState.LeftButton == ButtonState.Pressed &&
                   MouseState.LeftButton == ButtonState.Pressed;
        }

        #endregion

        #region Middle Click

        /// <summary>
        /// Middle Click Down Method.
        /// Detects if a middle click was performed.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool MiddleClickDown()
        {
            State = States.Click;

            return PreviousMouseState.MiddleButton == ButtonState.Released &&
                   MouseState.MiddleButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Middle Click Up Method.
        /// Detects if a middle click was released.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool MiddleClickUp()
        {
            State = States.Click;

            return PreviousMouseState.MiddleButton == ButtonState.Pressed &&
                   MouseState.MiddleButton == ButtonState.Released;
        }

        /// <summary>
        /// Middle Click Down Inside.
        /// Detects if a middle click was performed inside the specified Rectangle.
        /// </summary>
        /// <param name="rectangle">Specified Rectangle in which to detect the left click.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool MiddleClickDownInside(Rectangle rectangle)
        {
            State = States.Click;

            return rectangle.Contains(Rectangle) &&
                   PreviousMouseState.MiddleButton == ButtonState.Released &&
                   MouseState.MiddleButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Middle Click Up Inside.
        /// Detects if a middle click was released inside the specified Rectangle.
        /// </summary>
        /// <param name="rectangle">Specified Rectangle in which to detect the left click.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool MiddleClickUpInside(Rectangle rectangle)
        {
            State = States.Click;

            return rectangle.Contains(Rectangle) &&
                   PreviousMouseState.MiddleButton == ButtonState.Pressed &&
                   MouseState.MiddleButton == ButtonState.Released;
        }

        /// <summary>
        /// Middle Click Held.
        /// Detects if a middle click is being held.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool MiddleClickHeld()
        {
            State = States.Held;

            return PreviousMouseState.MiddleButton == ButtonState.Pressed &&
                   MouseState.MiddleButton == ButtonState.Pressed;
        }

        #endregion

        #region Right Click

        /// <summary>
        /// Right Click Down Method.
        /// Detects if a right click was performed.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool RightClickDown()
        {
            State = States.Click;

            return PreviousMouseState.RightButton == ButtonState.Released &&
                   MouseState.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Right Click Method.
        /// Detects if a right click was released.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool RightClickUp()
        {
            State = States.Click;

            return PreviousMouseState.RightButton == ButtonState.Pressed &&
                   MouseState.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// Right Click Down Inside.
        /// Detects if a right click was made inside the specified Rectangle.
        /// </summary>
        /// <param name="rectangle">Specified Rectangle in which to detect the right click.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool RightClickDownInside(Rectangle rectangle)
        {
            State = States.Click;

            return rectangle.Contains(Rectangle) &&
                   PreviousMouseState.RightButton == ButtonState.Released &&
                   MouseState.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Right Click Up Inside.
        /// Detects if a right click was released inside the specified Rectangle.
        /// </summary>
        /// <param name="rectangle">Specified Rectangle in which to detect the right click.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool RightClickUpInside(Rectangle rectangle)
        {
            State = States.Click;

            return rectangle.Contains(Rectangle) &&
                   PreviousMouseState.RightButton == ButtonState.Pressed &&
                   MouseState.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// Right Click and Held.
        /// Detects if a right click being held.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool RightClickHeld()
        {
            State = States.Held;

            return PreviousMouseState.RightButton == ButtonState.Pressed &&
                   MouseState.RightButton == ButtonState.Pressed;
        }

        #endregion

        #region Button One

        /// <summary>
        /// Button One Click Down Method.
        /// Detects if a button one click was performed.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool ButtonOneClickDown()
        {
            State = States.Click;

            return PreviousMouseState.XButton1 == ButtonState.Released &&
                   MouseState.XButton1 == ButtonState.Pressed;
        }

        /// <summary>
        /// Button One Click Up Method.
        /// Detects if a button one click was released.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool ButtonOneClickUp()
        {
            State = States.Click;

            return PreviousMouseState.XButton1 == ButtonState.Pressed &&
                   MouseState.XButton1 == ButtonState.Released;
        }

        /// <summary>
        /// Button One Down Inside.
        /// Detects if a button one click was performed inside the specified Rectangle.
        /// </summary>
        /// <param name="rectangle">Specified Rectangle in which to detect the button one click.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool ButtonOneClickDownInside(Rectangle rectangle)
        {
            State = States.Click;

            return rectangle.Contains(Rectangle) &&
                   PreviousMouseState.XButton1 == ButtonState.Released &&
                   MouseState.XButton1 == ButtonState.Pressed;
        }

        /// <summary>
        /// Button One Click Up Inside.
        /// Detects if a button one click was released inside the specified Rectangle.
        /// </summary>
        /// <param name="rectangle">Specified Rectangle in which to detect the button one click.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool ButtonOneClickUpInside(Rectangle rectangle)
        {
            State = States.Click;

            return rectangle.Contains(Rectangle) &&
                   PreviousMouseState.XButton1 == ButtonState.Pressed &&
                   MouseState.XButton1 == ButtonState.Released;
        }

        /// <summary>
        /// Button One Click Held.
        /// Detects if a button one click is being held.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool ButtonOneClickHeld()
        {
            State = States.Held;

            return PreviousMouseState.XButton1 == ButtonState.Pressed &&
                   MouseState.XButton1 == ButtonState.Pressed;
        }

        #endregion

        #region Button Two

        /// <summary>
        /// Button Two Click Down Method.
        /// Detects if a button two click was performed.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool ButtonTwoClickDown()
        {
            State = States.Click;

            return PreviousMouseState.XButton2 == ButtonState.Released &&
                   MouseState.XButton2 == ButtonState.Pressed;
        }

        /// <summary>
        /// Button Two Click Up Method.
        /// Detects if a button two click was released.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool ButtonTwoClickUp()
        {
            State = States.Click;

            return PreviousMouseState.XButton2 == ButtonState.Pressed &&
                   MouseState.XButton2 == ButtonState.Released;
        }

        /// <summary>
        /// Button Two Down Inside.
        /// Detects if a button two click was performed inside the specified Rectangle.
        /// </summary>
        /// <param name="rectangle">Specified Rectangle in which to detect the button two click.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool ButtonTwoClickDownInside(Rectangle rectangle)
        {
            State = States.Click;

            return rectangle.Contains(Rectangle) &&
                   PreviousMouseState.XButton2 == ButtonState.Released &&
                   MouseState.XButton2 == ButtonState.Pressed;
        }

        /// <summary>
        /// Button Two Click Up Inside.
        /// Detects if a button two click was released inside the specified Rectangle.
        /// </summary>
        /// <param name="rectangle">Specified Rectangle in which to detect the button two click.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool ButtonTwoClickUpInside(Rectangle rectangle)
        {
            State = States.Click;

            return rectangle.Contains(Rectangle) &&
                   PreviousMouseState.XButton2 == ButtonState.Pressed &&
                   MouseState.XButton2 == ButtonState.Released;
        }

        /// <summary>
        /// Button Two Click Held.
        /// Detects if a button two click is being held.
        /// </summary>
        /// <returns>Returns a boolean.</returns>
        public static bool ButtonTwoClickHeld()
        {
            State = States.Held;

            return PreviousMouseState.XButton2 == ButtonState.Pressed &&
                   MouseState.XButton2 == ButtonState.Pressed;
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
                    if (ScrollIndex > PreviousScrollIndex)
                    {
                        viewRectangle.Y -= (int)ScrollSpeed;
                    }
                    // Mouse wheel down to move down.
                    else if (ScrollIndex < PreviousScrollIndex)
                    {
                        viewRectangle.Y += (int)ScrollSpeed;
                    }

                    break;

                case ScrollDirections.Horizontal:
                    // Mouse wheel up to move left.
                    if (ScrollIndex > PreviousScrollIndex)
                    {
                        viewRectangle.X -= (int)ScrollSpeed;
                    }
                    // Mouse wheel down to move right.
                    else if (ScrollIndex < PreviousScrollIndex)
                    {
                        viewRectangle.X += (int)ScrollSpeed;
                    }

                    break;
            }

            return viewRectangle;
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
                if (ScrollIndex > PreviousScrollIndex)
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
                else if (ScrollIndex < PreviousScrollIndex)
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
        /// <param name="gameTime">Intakes a GameTime instance.</param>
        public static void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            PreviousMouseState = MouseState;
            MouseState = Mouse.GetState();

            PreviousScrollIndex = ScrollIndex;
            ScrollIndex = MouseState.ScrollWheelValue;

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