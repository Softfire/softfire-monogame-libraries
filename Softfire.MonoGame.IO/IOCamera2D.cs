using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Softfire.MonoGame.IO
{
    public class IOCamera2D
    {
        /// <summary>
        /// Camera Viewport.
        /// </summary>
        public Viewport Viewport { get; set; }

        /// <summary>
        /// Game Delta Time.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// Is In Focus?
        /// Set to true if this IOCamera2D has focus.
        /// </summary>
        public bool IsInFocus { get; set; }

        /// <summary>
        /// Are Camera Controls Enabled.
        /// Used to trigger camera controls.
        /// </summary>
        public bool AreControlsEnabled { get; set; }

        /// <summary>
        /// Allow Camera Zoom?
        /// Enable/Disable Camera Zoom.
        /// </summary>
        public bool AllowZoom { get; set; }

        /// <summary>
        /// Camera Position.
        /// Translated in the Camera's Matrix.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// World View Rectangle.
        /// The Area in which the camera can travel.
        /// Default is Rectangle(0, 0, Viewport.Width, Viewport.Height).
        /// </summary>
        public Rectangle WorldViewRectangle { get; set; }

        /// <summary>
        /// World View Internal Width.
        /// </summary>
        private int _worldViewWidth;

        /// <summary>
        /// World View Width.
        /// </summary>
        public int WorldViewWidth
        {
            get => _worldViewWidth;
            set => _worldViewWidth = value < Viewport.Width ? Viewport.Width : value;
        }

        /// <summary>
        /// World View Internal Height.
        /// </summary>
        private int _worldViewHeight;

        /// <summary>
        /// World View Height.
        /// </summary>
        public int WorldViewHeight
        {
            get => _worldViewHeight;
            set => _worldViewHeight = value < Viewport.Height ? Viewport.Height : value;
        }

        /// <summary>
        /// Camera Rotation Angle.
        /// Translated in the Camera's Matrix.
        /// </summary>
        public float RotationAngle { get; set; }
        
        /// <summary>
        /// Camera Matrix.
        /// Transforms view to WorldView.
        /// CreateTranslation(new Vector3(-Position, 0)) inverses the position to follow it.
        /// CreateRotationZ(Rotation) applies any rotation.
        /// CreateScale(Zoom, Zoom, 1) scales the view.
        /// </summary>
        public Matrix Matrix => Matrix.Identity *
                                Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                                Matrix.CreateRotationX(RotationAngle) *
                                Matrix.CreateScale(Zoom, Zoom, 1);

        /// <summary>
        /// Camera Scroll Speed.
        /// </summary>
        public float ScrollSpeed { get; set; } = ScrollSpeeds.Low;

        /// <summary>
        /// Camera Scroll Speeds.
        /// </summary>
        public struct ScrollSpeeds
        {
            public const float Low = 0.25f;
            public const float Medium = 0.50f;
            public const float High = 1.00f;
            public const float Ultra = 2.00f;
        }

        /// <summary>
        /// Camera Zoom Minimum.
        /// </summary>
        private float ZoomMinimum { get; set; }

        /// <summary>
        /// Camera Zoom Maximum.
        /// </summary>
        private float ZoomMaximum { get; set; }

        /// <summary>
        /// Camera Zoom.
        /// </summary>
        private float _zoom;

        /// <summary>
        /// Camera Zoom.
        /// </summary>
        private float Zoom
        {
            get => _zoom;
            set {
                    if (value > ZoomMaximum)
                    {
                        _zoom = ZoomMaximum;
                    }
                    else if (value < ZoomMinimum)
                    {
                        _zoom = ZoomMinimum;
                    }
                    else
                    {
                        _zoom = value;
                    }
                }
        }

        /// <summary>
        /// Camera Zoom Increment.
        /// </summary>
        public float ZoomIncrement { get; set; }

        /// <summary>
        /// Camera Zoom Increments.
        /// </summary>
        public struct ZoomIncrements
        {
            public const float Tenth = 0.10f;
            public const float Quarter = 0.25f;
            public const float Half = 0.50f;
            public const float One = 1.00f;
        }
        
        /// <summary>
        /// Camera Constructor.
        /// </summary>
        /// <param name="viewRectangle">Intakes the Camera's viewing area as a Rectangle.</param>
        /// <param name="worldRectangle">Intakes a Rectangle of the World.</param>
        /// <param name="rotation">Intakes an int to define the starting rotation angle in radinas. 0.0f is the default.</param>
        /// <param name="allowZoom">Inatkes a boolean indicating whether to enable/disable zoom functionality.</param>
        /// <param name="zoomMinimum">Intakes the minimum zoom level as a float. 0.25f is the default.</param>
        /// <param name="zoomMaximum">Intakes the maximum zoom level as a float. 3.0f is the default.</param>
        public IOCamera2D(Rectangle viewRectangle, Rectangle worldRectangle = new Rectangle(), float rotation = 0.0f, bool allowZoom = false, float zoomMinimum = 0.10f, float zoomMaximum = 2.0f)
        {
            Viewport = new Viewport(viewRectangle);

            WorldViewWidth = worldRectangle.Width <= Viewport.Width ? Viewport.Width : worldRectangle.Width;
            WorldViewHeight = worldRectangle.Height <= Viewport.Height ? Viewport.Height : worldRectangle.Height;

            RotationAngle = rotation;
            
            AllowZoom = allowZoom;
            ZoomMinimum = zoomMinimum;
            ZoomMaximum = zoomMaximum;
            Zoom = 1.0f;
            ZoomIncrement = ZoomIncrements.Tenth;
        }

        /// <summary>
        /// Controls.
        /// Enable AreControlsEnabled To Use.
        /// </summary>
        private void Controls()
        {
            if (IsInFocus)
            {
                if (IOKeyboard.KeyHeld(Keys.LeftControl) &&
                    IOKeyboard.KeyPress(Keys.NumPad0))
                {
                    AreControlsEnabled = !AreControlsEnabled;
                }

                if (AreControlsEnabled)
                {
                    // Up
                    if (IOKeyboard.KeyHeld(Keys.LeftControl) &&
                        IOKeyboard.KeyHeld(Keys.NumPad8))
                    {
                        Position -= new Vector2(0, ScrollSpeed) * (float)DeltaTime;
                    }

                    // Down
                    if (IOKeyboard.KeyHeld(Keys.LeftControl) &&
                        IOKeyboard.KeyHeld(Keys.NumPad2))
                    {
                        Position += new Vector2(0, ScrollSpeed) * (float)DeltaTime;
                    }

                    // Left
                    if (IOKeyboard.KeyHeld(Keys.LeftControl) &&
                        IOKeyboard.KeyHeld(Keys.NumPad4))
                    {
                        Position -= new Vector2(ScrollSpeed, 0) * (float)DeltaTime;
                    }

                    // Right
                    if (IOKeyboard.KeyHeld(Keys.LeftControl) &&
                        IOKeyboard.KeyHeld(Keys.NumPad6))
                    {
                        Position += new Vector2(ScrollSpeed, 0) * (float)DeltaTime;
                    }

                    if (AllowZoom)
                    {
                        if (IOKeyboard.KeyPress(Keys.Add))
                        {
                            Zoom += ZoomIncrement;
                        }

                        if (IOKeyboard.KeyPress(Keys.Subtract))
                        {
                            Zoom -= ZoomIncrement;
                        }
                    }

                    if (IOKeyboard.KeyPress(Keys.Escape))
                    {
                        AreControlsEnabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Boundaries.
        /// </summary>
        private void Boundaries()
        {
            // Top
            if (Position.Y < 0)
            {
                Position = new Vector2(Position.X, 0);
            }

            // Right
            if (Position.X > WorldViewWidth - Viewport.Width)
            {
                Position = new Vector2(WorldViewWidth - Viewport.Width, Position.Y);
            }

            // Bottom
            if (Position.Y > WorldViewHeight - Viewport.Height)
            {
                Position = new Vector2(Position.X, WorldViewHeight - Viewport.Height);
            }

            // Left
            if (Position.X < 0)
            {
                Position = new Vector2(0, Position.Y);
            }
        }

        /// <summary>
        /// Set Focus Target.
        /// </summary>
        /// <param name="focusTargetPosition">Intakes a target position to focus at the camera's center as a Vector2.</param>
        public void SetFocusTarget(Vector2 focusTargetPosition)
        {
            Position = focusTargetPosition - new Vector2(Viewport.Width / Zoom / 2f, Viewport.Height / Zoom / 2f);
        }

        /// <summary>
        /// Get Screen Position.
        /// </summary>
        /// <param name="worldPosition">Intakes a Vector2 defining the location within the underlying World Space.</param>
        /// <returns>Returns a Vector2 defining the location on the screen that corresponds with the World position given.</returns>
        public Vector2 GetScreenPosition(Vector2 worldPosition)
        {
            return worldPosition - new Vector2(Viewport.X, Viewport.Y) - Position;
        }

        /// <summary>
        /// Get World Position.
        /// </summary>
        /// <param name="screenPosition">Intakes a Vector2 defining the location on the Screen.</param>
        /// <returns>Returns a Vector2 defining the location in World Space that corresponds with the screen position given.</returns>
        public Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            return screenPosition + new Vector2(Viewport.X, Viewport.Y) + Position;
        }

        /// <summary>
        /// Check if input rectangle is within the Viewport's bounds.
        /// </summary>
        /// <param name="rectangle">The input rectangle to check against the Viewport's bounds.</param>
        public void CheckIsInFocus(Rectangle rectangle)
        {
            IsInFocus = Viewport.Bounds.Contains(rectangle);
        }

        /// <summary>
        /// Camera Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            WorldViewRectangle = new Rectangle(Viewport.X, Viewport.Y, WorldViewWidth, WorldViewHeight);

            Controls();
            Boundaries();
        }

        /// <summary>
        /// Camera Draw Method.
        /// Draws Camera Overlays when Camera Controllers are enabled.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (AreControlsEnabled)
            {
                //TODO: Create Camera Overlays for Zoom and Directional Controls.
            }
        }
    }
}