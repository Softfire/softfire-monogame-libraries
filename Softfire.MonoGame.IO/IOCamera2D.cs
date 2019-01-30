using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE;
using Softfire.MonoGame.CORE.Common;
using Softfire.MonoGame.CORE.Input;
using Softfire.MonoGame.CORE.Graphics.Views;

namespace Softfire.MonoGame.IO
{
    /// <summary>
    /// A 2D camera.
    /// </summary>
    public partial class IOCamera2D : IMonoGameUpdateComponent, IMonoGameFocusComponent
    {
        #region Booleans

        /// <summary>
        /// Determines whether the camera is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        #endregion

        #region Properties

        /// <summary>
        /// The camera's view.
        /// </summary>
        private ViewBase View { get; }

        /// <summary>
        /// The time between updates.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// The camera's internal position value.
        /// </summary>
        private Vector2 _position = Vector2.Zero;

        /// <summary>
        /// The camera's view position.
        /// </summary>
        public Vector2 Position
        {
            get => _position;
            set => _position = EnforceBoundaries(value);
        }
        
        /// <summary>
        /// The camera's rotation angle, in radians.
        /// </summary>
        public float RotationAngle { get; set; }

        /// <summary>
        /// The camera's view rectangle based on the <see cref="BoundingFrustum"/> of the camera.
        /// </summary>
        public RectangleF Bounds
        {
            get
            {
                var frustum = GetBoundingFrustum();
                var corners = frustum.GetCorners();
                var topLeft = corners[0];
                var bottomRight = corners[2];
                var width = bottomRight.X - topLeft.X;
                var height = bottomRight.Y - topLeft.Y;
                return new RectangleF(topLeft.X, topLeft.Y, width, height);
            }
        }

        /// <summary>
        /// The camera's origin.
        /// </summary>
        public Vector2 Origin => new Vector2(View.WorldWidth / 2f, View.WorldHeight / 2f);
        
        /// <summary>
        /// Determines whether the UI has focus.
        /// </summary>
        public FocusStates FocusState { get; set; }

        /// <summary>
        /// Input events used to interact with the UI.
        /// </summary>
        public InputEventArgs InputEvents { get; }

        #endregion

        /// <summary>
        /// A default 2D camera.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use. Intaken as a <see cref="GraphicsDevice"/>.</param>
        public IOCamera2D(GraphicsDevice graphicsDevice) : this(new ViewDefault(graphicsDevice))
        {

        }

        /// <summary>
        /// A scaled 2D camera.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use. Intaken as a <see cref="GraphicsDevice"/>.</param>
        /// <param name="worldWidth">The camera's world width. Intaken as an <see cref="int"/>.</param>
        /// <param name="worldHeight">The camera's world height. Intaken as an <see cref="int"/>.</param>
        public IOCamera2D(GraphicsDevice graphicsDevice, int worldWidth, int worldHeight) : this(new ViewScaled(graphicsDevice, worldWidth, worldHeight))
        {

        }

        /// <summary>
        /// A 2D camera for in-game windows.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use. Intaken as a <see cref="GraphicsDevice"/>.</param>
        /// <param name="viewWidth">The view's width. Intaken as an <see cref="int"/>.</param>
        /// <param name="viewHeight">The view's height. Intaken as an <see cref="int"/>.</param>
        /// <param name="worldWidth">The view's world width. Intaken as an <see cref="int"/>.</param>
        /// <param name="worldHeight">The view's world height. Intaken as an <see cref="int"/>.</param>
        public IOCamera2D(GraphicsDevice graphicsDevice, int viewWidth, int viewHeight, int worldWidth, int worldHeight) :
                          this(new ViewWindow(graphicsDevice, viewWidth, viewHeight, worldWidth, worldHeight))
        {

        }

        /// <summary>
        /// A 2D camera for the game client window.
        /// </summary>
        /// <param name="window">The game client window. Intaken as a <see cref="GameWindow"/>.</param>
        /// <param name="graphicsDevice">The graphics device to use. Intaken as a <see cref="GraphicsDevice"/>.</param>
        public IOCamera2D(GameWindow window, GraphicsDevice graphicsDevice) : this(new ViewGameClient(window, graphicsDevice))
        {

        }

        /// <summary>
        /// A 2D camera for use with the MonoGame framework.
        /// </summary>
        /// <param name="view">The view to use with the camera. Intaken as a derived type of <see cref="ViewBase"/>.</param>
        private IOCamera2D(ViewBase view)
        {
            View = view;

            InputEvents = InputEventArgs.Empty;

            // Register movement event.
            // Registration order matters for call order.
            IOManager.InputMovementHandler += OnMove;
            IOManager.InputMovementHandler += OnBlur;
            IOManager.InputMovementHandler += OnFocus;

            // Register scrolling event.
            IOManager.InputScrollHandler += OnScroll;

            // Register input events.
            IOManager.InputPressHandler += OnPress;
            IOManager.InputReleaseHandler += OnRelease;
            IOManager.InputHeldHandler += OnHeld;
        }

        #region Events

        /// <summary>
        /// The subscription method to action when input changes.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnMove(object sender, InputEventArgs args)
        {
            if (IsActive &&
                IsStateSet(FocusStates.IsHovered))
            {
                InputEvents.InputDeltas = args.InputDeltas;
                InputEvents.InputRectangle = args.InputRectangle;
                InputEvents.PlayerIndex = args.PlayerIndex;
            }
        }

        /// <summary>
        /// The subscription method to action when the window is scrolled.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnScroll(object sender, InputEventArgs args)
        {
            if (IsActive &&
                IsStateSet(FocusStates.IsHovered))
            {
                InputEvents.InputScrollVelocity = args.InputScrollVelocity;
                InputEvents.PlayerIndex = args.PlayerIndex;
                InputEvents.InputFlags.SetFlag(InputMouseActionFlags.ScrollUp, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollUp));
                InputEvents.InputFlags.SetFlag(InputMouseActionFlags.ScrollDown, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollDown));
                InputEvents.InputFlags.SetFlag(InputMouseActionFlags.ScrollLeft, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollLeft));
                InputEvents.InputFlags.SetFlag(InputMouseActionFlags.ScrollRight, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollRight));
            }
        }

        /// <summary>
        /// The subscription method to action when the camera gains focus.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnFocus(object sender, InputEventArgs args)
        {
            if (IsStateSet(FocusStates.IsHovered))
            {
                AreControlsEnabled = true;
            }
        }

        /// <summary>
        /// The subscription method to action when the object loses focus or blurs.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnBlur(object sender, InputEventArgs args)
        {
            if (!IsStateSet(FocusStates.IsHovered))
            {
                AreControlsEnabled = false;
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects a press action.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnPress(object sender, InputEventArgs args)
        {
            if (IsActive &&
                IsStateSet(FocusStates.IsHovered))
            {
                // Cycle through all flags.
                foreach (InputMappableCameraCommandFlags flag in Enum.GetValues(typeof(InputMappableCameraCommandFlags)))
                {
                    // Is the flag in a pressed state.
                    if (args.InputStates.GetState(flag) == InputActionStateFlags.Press)
                    {
                        // Add the press flag.
                        InputEvents.InputStates.SetState(flag, InputActionStateFlags.Press);
                    }
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
            if (IsActive &&
                IsStateSet(FocusStates.IsHovered))
            {
                // Cycle through all flags.
                foreach (InputMappableCameraCommandFlags flag in Enum.GetValues(typeof(InputMappableCameraCommandFlags)))
                {
                    // Is the flag in a released state.
                    if (args.InputStates.GetState(flag) == InputActionStateFlags.Release)
                    {
                        // Add the release flag.
                        InputEvents.InputStates.SetState(flag, InputActionStateFlags.Release);
                    }
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
            if (IsActive &&
                IsStateSet(FocusStates.IsHovered))
            {
                // Cycle through all flags.
                foreach (InputMappableCameraCommandFlags flag in Enum.GetValues(typeof(InputMappableCameraCommandFlags)))
                {
                    // Is the flag in a held state.
                    if (args.InputStates.GetState(flag) == InputActionStateFlags.Held)
                    {
                        // Add the held flag.
                        InputEvents.InputStates.SetState(flag, InputActionStateFlags.Held);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the state flag.
        /// </summary>
        /// <param name="state">The state flag to set.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetState(FocusStates state, bool result)
        {
            if (result)
            {
                AddState(state);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the state flag is set.
        /// </summary>
        /// <param name="state">The state flag to check.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the state flag is set.</returns>
        public bool IsStateSet(FocusStates state) => (FocusState & state) == state;

        /// <summary>
        /// Removes the state flag, if set.
        /// </summary>
        /// <param name="state">The state flag to remove if it is set.</param>
        public void RemoveState(FocusStates state)
        {
            if (IsStateSet(state))
            {
                FocusState &= ~state;
            }
        }

        /// <summary>
        /// Adds the state flag.
        /// </summary>
        /// <param name="state">The state flag to add.</param>
        public void AddState(FocusStates state) => FocusState |= state;

        /// <summary>
        /// Clears the state flags.
        /// </summary>
        public void ClearStates() => FocusState = 0;

        #endregion
        
        /// <summary>
        /// Enforces movement boundaries.
        /// </summary>
        /// <param name="value">The value to confine within the bounds. Intaken as a <see cref="Vector2"/>.</param>
        private Vector2 EnforceBoundaries(Vector2 value)
        {
            // Top
            value.Y = value.Y < 0 ? 0 : value.Y;

            // Right
            value.X = value.X > View.WorldWidth ? View.WorldWidth : value.X;

            // Bottom
            value.Y = value.Y > View.WorldHeight ? View.WorldHeight : value.Y;

            // Left
            value.X = value.X < 0 ? 0 : value.X;

            return value;
        }

        /// <summary>
        /// Transforms the world point (X, Y) to a screen point.
        /// </summary>
        /// <param name="x">The world point on the X axis to transform to it's screen equivalent. Intaken as a <see cref="float"/>.</param>
        /// <param name="y">The world point on the Y axis to transform to it's screen equivalent. Intaken as a <see cref="float"/>.</param>
        /// <returns>Returns the transformed point from the world to the screen as a <see cref="Vector2"/>.</returns>
        public Vector2 WorldToScreen(float x, float y) => WorldToScreen(new Vector2(x, y));

        /// <summary>
        /// Transforms the world point (X, Y) to a screen point.
        /// </summary>
        /// <param name="worldPosition">The position within the world to transform into the equivalent screen position. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns the transformed point from the world to the screen as a <see cref="Vector2"/>.</returns>
        public Vector2 WorldToScreen(Vector2 worldPosition) => Vector2.Transform(worldPosition + new Vector2(View.Viewport.X, View.Viewport.Y), GetViewMatrix());

        /// <summary>
        /// Transforms the screen point (X, Y) to a world point.
        /// </summary>
        /// <param name="x">The screen point on the X axis to transform to it's world equivalent. Intaken as a <see cref="float"/>.</param>
        /// <param name="y">The screen point on the Y axis to transform to it's screen equivalent. Intaken as a <see cref="float"/>.</param>
        /// <returns>Returns the transformed point from the screen to the world as a <see cref="Vector2"/>.</returns>
        public Vector2 ScreenToWorld(float x, float y) => ScreenToWorld(new Vector2(x, y));

        /// <summary>
        /// Transforms the screen point (X, Y) to a world point.
        /// </summary>
        /// <param name="screenPosition">The position within the screen to transform into the equivalent world position. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns the transformed point from the screen to the world as a <see cref="Vector2"/>.</returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition) => Vector2.Transform(screenPosition - new Vector2(View.Viewport.X, View.Viewport.Y), Matrix.Invert(GetViewMatrix()));

        /// <summary>
        /// Retrieves the camera's world view matrix, optionally inverted.
        /// </summary>
        /// <param name="returnInverted">Determines whether the matrix is returned as an inverted matrix.</param>
        /// <returns>Returns the camera's world view as a <see cref="Matrix"/>. Optionally inverted.</returns>
        public Matrix GetViewMatrix(bool returnInverted = false) => returnInverted ? Matrix.Invert(GetViewMatrix(Vector2.One)) : GetViewMatrix(Vector2.One);

        /// <summary>
        /// Retrieves the camera's world view matrix multiplied by the parallax factor.
        /// </summary>
        /// <param name="parallaxFactor">The value in which to multiply the camera's position by.</param>
        /// <returns>Returns a matrix with its position multiplied by the parallax factor.</returns>
        public Matrix GetViewMatrix(Vector2 parallaxFactor) => GetWorldViewMatrix(parallaxFactor) * View.GetScaleMatrix();

        /// <summary>
        /// Retrieves the camera's world view matrix.
        /// </summary>
        /// <returns>Returns the camera's world view as a <see cref="Matrix"/>.</returns>
        private Matrix GetWorldViewMatrix() => GetWorldViewMatrix(Vector2.One);

        /// <summary>
        /// Retrieves the camera's world view matrix multiplied by the parallax factor.
        /// </summary>
        /// <param name="parallaxFactor"></param>
        /// <returns>Returns a matrix with its position multiplied by the parallax factor.</returns>
        private Matrix GetWorldViewMatrix(Vector2 parallaxFactor) => Matrix.CreateTranslation(new Vector3(-Position * parallaxFactor, 0.0f)) *
                                                                     Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                                                                     Matrix.CreateRotationZ(RotationAngle) *
                                                                     Matrix.CreateScale(Zoom, Zoom, 1) *
                                                                     Matrix.CreateTranslation(new Vector3(Origin, 0.0f));

        /// <summary>
        /// Retrieves the projection view matrix of the provided matrix based on the camera's <see cref="Position"/>.
        /// </summary>
        /// <param name="viewMatrix">The view matrix to retrieve the projection view from. Intaken as a <see cref="Matrix"/>.</param>
        /// <returns>Returns the projection view from the provided view matrix based on the camera's <see cref="Position"/>.</returns>
        private Matrix GetProjectionViewMatrix(Matrix viewMatrix)
        {
            var projection = Matrix.CreateOrthographicOffCenter(View.Viewport.X, View.WorldWidth, View.WorldHeight, View.Viewport.Y, -1, 0);
            Matrix.Multiply(ref viewMatrix, ref projection, out projection);
            return projection;
        }

        /// <summary>
        /// Builds a <see cref="BoundingFrustum"/> using a combined matrix from the camera's view matrix, <see cref="GetViewMatrix(bool)"/>,
        /// and the camera's projection matrix, <see cref="GetProjectionViewMatrix(Matrix)"/>.
        /// </summary>
        /// <remarks>A frustum in computer graphics is generally a volume of 3D space,
        /// defined as the part of a rectangular pyramid that lies between two planes perpendicular to its center line.
        /// A frustum is often used to represent what a "camera" sees in your 3D space.</remarks>
        /// <returns>Returns a <see cref="BoundingFrustum"/> of the camera's view space.</returns>
        public BoundingFrustum GetBoundingFrustum() => new BoundingFrustum(GetProjectionViewMatrix(GetWorldViewMatrix()));

        /// <summary>
        /// Determines whether the <see cref="BoundingFrustum"/> contains the point in it's view.
        /// </summary>
        /// <param name="point">The point to check if it resides inside or intersects with the camera's view. Intaken as a <see cref="Point"/>.</param>
        /// <returns>Returns a <see cref="ContainmentType"/> describing the result.</returns>
        public ContainmentType Contains(Point point) => Contains(point.ToVector2());

        /// <summary>
        /// Determines whether the <see cref="BoundingFrustum"/> contains the vector in it's view.
        /// </summary>
        /// <param name="vector">The vector to check if it resides inside or intersects with the camera's view. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns a <see cref="ContainmentType"/> describing the result.</returns>
        public ContainmentType Contains(Vector2 vector) => GetBoundingFrustum().Contains(new Vector3(vector.X, vector.Y, 0));

        /// <summary>
        /// Determines whether the <see cref="BoundingFrustum"/> contains the provided rectangle in it's view.
        /// </summary>
        /// <param name="rectangle">The rectangle to check if it resides inside or intersects with the camera's view. Intaken as a <see cref="RectangleF"/>.</param>
        /// <returns>Returns a <see cref="ContainmentType"/> describing the result.</returns>
        public ContainmentType Contains(RectangleF rectangle)
        {
            var max = new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0.5f);
            var min = new Vector3(rectangle.X, rectangle.Y, 0.5f);
            var boundingBox = new BoundingBox(min, max);

            return GetBoundingFrustum().Contains(boundingBox);
        }
        
        /// <summary>
        /// Camera Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame <see cref="GameTime"/>.</param>
        public void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (IsStateSet(FocusStates.IsHovered))
            {
                Controls();
            }

            // Clears the events.
            InputEvents.Clear();
        }
    }
}