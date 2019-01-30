using Microsoft.Xna.Framework;
using Softfire.MonoGame.CORE;
using Softfire.MonoGame.CORE.Common;

namespace Softfire.MonoGame.IO
{
    public partial class IOMouse
    {
        #region Remote Control

        /// <summary>
        /// Determines whether the mouse is remotely controlled.
        /// </summary>
        public bool IsRemoteControlled { get; set; }

        /// <summary>
        /// The remote device's state. Used to remotely control the mouse.
        /// </summary>
        private Vector2 RemoteState { get; set; }

        /// <summary>
        /// The remote device's previous state. Used to remotely control the mouse.
        /// </summary>
        private Vector2 PreviousRemoteState { get; set; }

        /// <summary>
        /// Updates the <see cref="RemoteState"/> with remote controller vectors to control the mouse.
        /// </summary>
        /// <param name="remoteVector">The vectors of the remote device.</param>
        public void RemoteControl(Vector2 remoteVector) => RemoteState += remoteVector;

        #endregion

        #region Rotation

        /// <summary>
        /// Rotates the position towards the mouse's cursor.
        /// </summary>
        /// <param name="position">An object's position. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns the rotated angle pointing towards the mouse's cursor.</returns>
        public double RotateTowardsCursor(Vector2 position) => Movement.RotateTowards(Position, position);

        /// <summary>
        /// Rotates the position away from the mouse's cursor.
        /// </summary>
        /// <param name="position">An object's position. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns the rotated angle pointing away from the mouse's cursor.</returns>
        public double RotateAwayFromCursor(Vector2 position) => Movement.RotateAway(Position, position);

        /// <summary>
        /// Rotates the position around the mouse's cursor.
        /// </summary>
        /// <param name="rotationAngle">The rotation angle at which to rotate around the mouse, in radians. Intaken as a <see cref="float"/>.</param>
        /// <param name="positionalOffset">The distance from the mouse to rotate at. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns the rotated angle pointing away from the mouse's cursor.</returns>
        public Vector2 RotateAroundCursor(float rotationAngle, Vector2 positionalOffset) => Movement.RotateAround(Position, new Vector2(Width / 2f, Height / 2f), rotationAngle, positionalOffset);

        #endregion

        #region Boundaries

        /// <summary>
        /// Checks the provided rectangle for interaction with mouse.
        /// </summary>
        /// <param name="bounds">The rectangle to check for intersection or containment.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the mouse is intersecting or is contained within the rectangle</returns>
        public bool CheckBounds(RectangleF bounds) => Rectangle.IntersectsWith(bounds) || bounds.Contains(Rectangle);

        /// <summary>
        /// The mouse's boundary enforcement method.
        /// </summary>
        /// <param name="container">The mouse's container's rectangle.</param>
        public void Boundaries(RectangleF container)
        {
            var mousePosition = new Vector2(MouseState.X, MouseState.Y);

            // Top
            if (mousePosition.Y < 0)
            {
                mousePosition.Y = 0;
            }

            // Right
            if (mousePosition.X > container.Width)
            {
                mousePosition.X = container.Width;
            }

            // Bottom
            if (mousePosition.Y > container.Height)
            {
                mousePosition.Y = container.Height;
            }

            // Left
            if (mousePosition.X < 0)
            {
                mousePosition.X = 0;
            }

            Position = mousePosition;
        }

        #endregion
    }
}