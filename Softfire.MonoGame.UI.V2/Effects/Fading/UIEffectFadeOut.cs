using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.V2.Effects.Fading
{
    /// <summary>
    /// An effect to fade out the UI.
    /// </summary>
    public class UIEffectFadeOut : UIEffectBase
    {
        /// <summary>
        /// Starting Transparency Level.
        /// </summary>
        private float StartingTransparencyLevel { get; }

        /// <summary>
        /// Target Transparency Level.
        /// </summary>
        private float TargetTransparencyLevel { get; }

        /// <summary>
        /// The rate of change between calls.
        /// </summary>
        private double RateOfChange { get; set; }

        /// <summary>
        /// UI Effect Fade Out.
        /// </summary>
        /// <param name="parent">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">A unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="startingTransparencyLevel">The effect's starting transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="targetTransparencyLevel">The effect's target transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a <see cref="float"/>. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a <see cref="float"/>. Default is 0f.</param>
        public UIEffectFadeOut(UIBase parent, int id, string name, float startingTransparencyLevel = 1f, float targetTransparencyLevel = 0f,
                                                                   float durationInSeconds = 1f, float startDelayInSeconds = 0f) : base(parent, id, name, durationInSeconds, startDelayInSeconds)
        {
            StartingTransparencyLevel = MathHelper.Clamp(startingTransparencyLevel, 0f, 1f);
            TargetTransparencyLevel = MathHelper.Clamp(targetTransparencyLevel, 0f, 1f);
        }

        /// <summary>
        /// Fades out the UI.
        /// </summary>
        /// <returns>Returns a bool indicating whether the fade was completed.</returns>
        protected override bool Action()
        {
            if (ElapsedTime >= StartDelayInSeconds)
            {
                RateOfChange = (StartingTransparencyLevel - TargetTransparencyLevel) / DurationInSeconds;
                Parent.GetTransparency("Background").Level -= (float)RateOfChange * (float)DeltaTime;
            }

            // Correction for float calculations.
            if (Parent.GetTransparency("Background").Level <= TargetTransparencyLevel)
            {
                Parent.GetTransparency("Background").Level = TargetTransparencyLevel;
            }

            return Parent.GetTransparency("Background").Level <= TargetTransparencyLevel || ElapsedTime > DurationInSeconds + StartDelayInSeconds;
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