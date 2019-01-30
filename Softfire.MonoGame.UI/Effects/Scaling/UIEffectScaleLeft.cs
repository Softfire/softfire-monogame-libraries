using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Scaling
{
    /// <summary>
    /// An effect to scale the UI to the left along the X axis.
    /// </summary>
    public class UIEffectScaleLeft : UIEffectBase
    {
        /// <summary>
        /// The target scale to reach along the X axis.
        /// </summary>
        private Vector2 TargetScale { get; }

        /// <summary>
        /// The initial scale of the UI.
        /// </summary>
        private Vector2 InitialScale { get; }

        /// <summary>
        /// The rate of change between calls.
        /// </summary>
        private double RateOfChange { get; set; }

        /// <summary>
        /// An effect that scales the UI to the left along the X axis.
        /// </summary>
        /// <param name="parent">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">A unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="targetScale">The target scale (X) to scale the UI by over the duration of time provided. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a <see cref="float"/>. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a <see cref="float"/>. Default is 0f.</param>
        public UIEffectScaleLeft(UIBase parent, int id, string name, Vector2 targetScale,
                                 float durationInSeconds = 1f, float startDelayInSeconds = 0f) : base(parent, id, name, durationInSeconds, startDelayInSeconds)
        {
            InitialScale = Parent.Transform.Scale;
            TargetScale = targetScale;
        }

        /// <summary>
        /// Scales the UI to the left.
        /// </summary>
        /// <returns>Returns a bool indicating whether the scaling was completed.</returns>
        protected override bool Action()
        {
            var scale = Parent.Transform.Scale;

            if (ElapsedTime >= StartDelayInSeconds)
            {
                RateOfChange = TargetScale.Y / DurationInSeconds;
                scale.X -= (float)RateOfChange * (float)DeltaTime;
            }

            // Correction for float calculations.
            if (scale.X <= InitialScale.X - TargetScale.X)
            {
                scale.X = InitialScale.X - TargetScale.X;
            }

            Parent.Transform.Scale = scale;

            return Parent.Transform.Scale.X <= InitialScale.X - TargetScale.X || ElapsedTime > DurationInSeconds + StartDelayInSeconds;
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