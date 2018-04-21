using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Transitions
{
    public class UIEffectFadeOut : UIBaseEffect
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
        /// UI Effect Fade Out.
        /// </summary>
        /// <param name="uiBase">The Parent UIBase.</param>
        /// <param name="startingTransparencyLevel">Starting transparency level. Intaken as a float.</param>
        /// <param name="targetTransparencyLevel">Target transparency level. Intaken as a float.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float.</param>
        /// <param name="orderNumber">Intakes the Effect's run Order Number as an int.</param>
        public UIEffectFadeOut(UIBase uiBase, float startingTransparencyLevel = 1f, float targetTransparencyLevel = 0f, float durationInSeconds = 1.0f, float startDelayInSeconds = 0.0f, int orderNumber = 0) : base(uiBase, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            StartingTransparencyLevel = MathHelper.Clamp(startingTransparencyLevel, 0f, 1f);
            TargetTransparencyLevel = MathHelper.Clamp(targetTransparencyLevel, 0f, 1f);
            RateOfChange = (StartingTransparencyLevel - TargetTransparencyLevel) / DurationInSeconds;
        }

        /// <summary>
        /// Action.
        /// Defines Effect.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected override bool Action()
        {
            if (ElapsedTime >= StartDelayInSeconds)
            {
                ParentUIBase.Transparency = MathHelper.Clamp(ParentUIBase.Transparency - (float)RateOfChange * (float)DeltaTime, 0f, 1f);
            }

            // Correction for float calculations.
            if (ParentUIBase.Transparency <= TargetTransparencyLevel)
            {
                ParentUIBase.Transparency = TargetTransparencyLevel;
            }

            return ParentUIBase.Transparency <= TargetTransparencyLevel;
        }
    }
}