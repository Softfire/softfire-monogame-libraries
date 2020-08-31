using Microsoft.Xna.Framework;
using Softfire.MonoGame.CORE;
using Softfire.MonoGame.CORE.V2;

namespace Softfire.MonoGame.IO.V2
{
    public partial class IOGamepad
    {
        #region Movement Deltas

        /// <summary>
        /// Calculates and returns the movement deltas between each thumb stick state update.
        /// </summary>
        /// <returns>Returns the movement deltas of the last movement of the left thumb stick as a <see cref="Vector2"/>.</returns>
        public Vector2 GetLeftThumbStickMovementDeltas() => new Vector2(GamepadState.ThumbSticks.Left.X - PreviousGamepadState.ThumbSticks.Left.X,
                                                                        GamepadState.ThumbSticks.Left.Y - PreviousGamepadState.ThumbSticks.Left.Y);

        /// <summary>
        /// Calculates and returns the movement deltas between each thumb stick state update.
        /// </summary>
        /// <returns>Returns the movement deltas of the last movement of the right thumb stick as a <see cref="Vector2"/>.</returns>
        public Vector2 GetRightThumbStickMovementDeltas() => new Vector2(GamepadState.ThumbSticks.Right.X - PreviousGamepadState.ThumbSticks.Right.X,
                                                                         GamepadState.ThumbSticks.Right.Y - PreviousGamepadState.ThumbSticks.Right.Y);

        /// <summary>
        /// Calculates and returns the thumb stick's bounding rectangle.
        /// </summary>
        /// <returns>Returns the thumb stick's bounding rectangle as a <see cref="RectangleF"/>.</returns>
        public RectangleF GetLeftThumbStickRectangle() => new RectangleF(GamepadState.ThumbSticks.Left.X, GamepadState.ThumbSticks.Left.Y, 1, 1);

        /// <summary>
        /// Calculates and returns the thumb stick's bounding rectangle.
        /// </summary>
        /// <returns>Returns the thumb stick's bounding rectangle as a <see cref="RectangleF"/>.</returns>
        public RectangleF GetRightThumbStickRectangle() => new RectangleF(GamepadState.ThumbSticks.Right.X, GamepadState.ThumbSticks.Right.Y, 1, 1);

        #endregion
    }
}