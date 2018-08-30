using System;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Shifting
{
    /// <summary>
    /// An effect to shift the UI downwards along the Y axis from it's current position.
    /// </summary>
    public class UIEffectShiftDown : UIEffectBase
    {
        /// <summary>
        /// Distance to shift along the Y axis.
        /// </summary>
        private Vector2 ShiftVector { get; }

        /// <summary>
        /// The initial position to shift from.
        /// </summary>
        private Vector2 InitialPosition { get; set; }

        /// <summary>
        /// An effect that shifts the UI down along the Y axis.
        /// </summary>
        /// <param name="uiBase">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an int.</param>
        /// <param name="name">A unique name. Intaken as a string.</param>
        /// <param name="shiftVector">The shift vector (Y) to offset the UI by over the duration of time provided. Intaken as a Vector2.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a float. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a float. Default is 0f.</param>
        /// <param name="orderNumber">The effect's run order number. Intaken as an int. Default is 0.</param>
        public UIEffectShiftDown(UIBase uiBase, int id, string name, Vector2 shiftVector,
                                 float durationInSeconds = 1f, float startDelayInSeconds = 0f, int orderNumber = 0) : base(uiBase, id, name, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            ShiftVector = shiftVector;
        }

        /// <summary>
        /// Shifts the UI down.
        /// </summary>
        /// <returns>Returns a bool indicating whether the shift was completed.</returns>
        protected override bool Action()
        {
            var position = ParentUIBase.Position;

            if (Math.Abs(ElapsedTime) <= DeltaTime)
            {
                InitialPosition = position;
            }

            if (ElapsedTime >= StartDelayInSeconds)
            {
                RateOfChange = ShiftVector.Y / DurationInSeconds;
                position.Y += (float)RateOfChange * (float)DeltaTime;
            }

            // Correction for float calculations.
            if (position.Y >= InitialPosition.Y + ShiftVector.Y)
            {
                position.Y = InitialPosition.Y + ShiftVector.Y;
            }

            ParentUIBase.Position = position;

            return ParentUIBase.Position.Y >= InitialPosition.Y + ShiftVector.Y;
        }
    }
}