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
        /// The rate of change between calls.
        /// </summary>
        private double RateOfChange { get; set; }

        /// <summary>
        /// An effect that shifts the UI down along the Y axis.
        /// </summary>
        /// <param name="parent">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">A unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="shiftVector">The shift vector (Y) to offset the UI by over the duration of time provided. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a <see cref="float"/>. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a <see cref="float"/>. Default is 0f.</param>
        public UIEffectShiftDown(UIBase parent, int id, string name, Vector2 shiftVector,
                                 float durationInSeconds = 1f, float startDelayInSeconds = 0f) : base(parent, id, name, durationInSeconds, startDelayInSeconds)
        {
            ShiftVector = shiftVector;
        }

        /// <summary>
        /// Shifts the UI down.
        /// </summary>
        /// <returns>Returns a bool indicating whether the shift was completed.</returns>
        protected override bool Action()
        {
            var position = Parent.Transform.Position;

            if (IsFirstRun)
            {
                InitialPosition = position;
                IsFirstRun = false;
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

            Parent.Transform.Position = position;

            return Parent.Transform.Position.Y >= InitialPosition.Y + ShiftVector.Y || ElapsedTime > DurationInSeconds + StartDelayInSeconds;
        }

        /// <summary>
        /// Resets the effect so it can be run again.
        /// </summary>
        protected internal override void Reset()
        {
            // Additional properties to reset.
            RateOfChange = 0;

            // Reset base properties.
            base.Reset();
        }
    }
}