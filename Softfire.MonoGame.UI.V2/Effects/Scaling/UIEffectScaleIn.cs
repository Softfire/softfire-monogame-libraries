using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.V2.Effects.Scaling
{
    /// <summary>
    /// An effect to scale the UI in along the X and YY axis.
    /// </summary>
    public class UIEffectScaleIn : UIEffectBase
    {
        /// <summary>
        /// The target scale to reach along the Y axis.
        /// </summary>
        private Vector2 TargetScale { get; }

        /// <summary>
        /// The initial scale of the UI.
        /// </summary>
        private Vector2 InitialScale { get; }

        /// <summary>
        /// The rate of change between calls.
        /// </summary>
        private Vector2 RateOfChange { get; set; }

        /// <summary>
        /// An effect that scales the UI in along the X and Y axis.
        /// </summary>
        /// <param name="parent">The ui element that will be affected. Intaken as a <see cref="UIBase"/>.</param>
        /// <param name="id">A unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">A unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="targetScale">The target scale (Y) to scale the UI by over the duration of time provided. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a <see cref="float"/>.</param>
        public UIEffectScaleIn(UIBase parent, int id, string name, Vector2 targetScale, float durationInSeconds = 1f, float startDelayInSeconds = 0f) : base(parent, id, name, durationInSeconds, startDelayInSeconds)
        {
            InitialScale = Parent.Transform.Scale;
            TargetScale = targetScale;
        }

        /// <summary>
        /// Scales the UI inwards.
        /// </summary>
        /// <returns>Returns a bool indicating whether the scaling was completed.</returns>
        protected override bool Action()
        {
            if ((TargetScale.X > 0 || TargetScale.Y > 0) &&
                ElapsedTime >= StartDelayInSeconds)
            {
                var scale = Parent.Transform.Scale;

                RateOfChange = new Vector2(TargetScale.X / DurationInSeconds, TargetScale.Y / DurationInSeconds);
                
                scale.X -= RateOfChange.X * (float)DeltaTime;
                scale.Y -= RateOfChange.Y * (float)DeltaTime;

                // Correction for float calculations.
                if (scale.X < TargetScale.X ||
                    scale.Y < TargetScale.Y)
                {
                    scale = TargetScale;
                }

                Parent.Transform.Scale = scale;
            }

            return Parent.Transform.Scale == TargetScale || ElapsedTime > DurationInSeconds + StartDelayInSeconds;
        }
    }
}