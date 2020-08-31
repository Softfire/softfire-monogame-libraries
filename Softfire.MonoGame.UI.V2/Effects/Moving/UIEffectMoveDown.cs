using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Moving
{
    /// <summary>
    /// An effect to shift the UI downwards along the Y axis from it's current position.
    /// </summary>
    public class UIEffectMoveDown : UIEffectBase
    {
        /// <summary>
        /// Start Position.
        /// </summary>
        private Vector2 StartPosition { get; }

        /// <summary>
        /// Target Position.
        /// </summary>
        private Vector2 TargetPosition { get; }

        /// <summary>
        /// The rate of change between calls.
        /// </summary>
        private double RateOfChange { get; set; }

        /// <summary>
        /// An effect that moves the UI down along the Y axis.
        /// </summary>
        /// <param name="parent">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">A unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="startPosition">The effect's start position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="targetPosition">The effect's target position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a <see cref="float"/>. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a <see cref="float"/>. Default is 0f.</param>
        public UIEffectMoveDown(UIBase parent, int id, string name, Vector2 startPosition, Vector2 targetPosition,
                                float durationInSeconds = 1f, float startDelayInSeconds = 0f) : base(parent, id, name, durationInSeconds, startDelayInSeconds)
        {
            StartPosition = startPosition;
            TargetPosition = targetPosition;
        }

        /// <summary>
        /// Moves the UI down.
        /// </summary>
        /// <returns>Returns a bool indicating whether the move was completed.</returns>
        protected override bool Action()
        {
            var position = Parent.Transform.Position;

            if (ElapsedTime >= StartDelayInSeconds)
            {
                RateOfChange = (TargetPosition.Y - StartPosition.Y) / DurationInSeconds;
                position.Y += (float)RateOfChange * (float)DeltaTime;
            }

            // Correction for float calculations.
            if (position.Y >= TargetPosition.Y)
            {
                position.Y = TargetPosition.Y;
            }

            Parent.Transform.Position = position;

            return Parent.Transform.Position.Y >= TargetPosition.Y || ElapsedTime > DurationInSeconds + StartDelayInSeconds;
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