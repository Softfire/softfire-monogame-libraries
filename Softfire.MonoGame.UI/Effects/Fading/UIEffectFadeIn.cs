using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Fading
{
    /// <summary>
    /// An effect to fade in the UI.
    /// </summary>
    public class UIEffectFadeIn : UIEffectBase
    {
        /// <summary>
        /// The transparency level to begin the transition.
        /// </summary>
        private float StartingTransparencyLevel { get; }

        /// <summary>
        /// The transparency level to end the transition.
        /// </summary>
        private float TargetTransparencyLevel { get; }

        /// <summary>
        /// A fade in effect.
        /// </summary>
        /// <param name="uiBase">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an int.</param>
        /// <param name="name">A unique name. Intaken as a string.</param>
        /// <param name="startingTransparencyLevel">The effect's starting transparency level. Intaken as a float.</param>
        /// <param name="targetTransparencyLevel">The effect's target transparency level. Intaken as a float.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a float. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a float. Default is 0f.</param>
        /// <param name="orderNumber">The effect's run order number. Intaken as an int. Default is 0.</param>
        public UIEffectFadeIn(UIBase uiBase, int id, string name, float startingTransparencyLevel = 0f, float targetTransparencyLevel = 1f, float durationInSeconds = 1f, float startDelayInSeconds = 0f, int orderNumber = 0) : base(uiBase, id, name, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            StartingTransparencyLevel = MathHelper.Clamp(startingTransparencyLevel, 0f, 1f);
            TargetTransparencyLevel = MathHelper.Clamp(targetTransparencyLevel, 0f, 1f);
            RateOfChange = (TargetTransparencyLevel - StartingTransparencyLevel) / DurationInSeconds;
        }

        /// <summary>
        /// Fades in the UI.
        /// </summary>
        /// <returns>Returns a bool indicating whether the fade was completed.</returns>
        protected override bool Action()
        {
            if (ElapsedTime >= StartDelayInSeconds)
            {
                ParentUIBase.Transparencies["Background"] += (float)RateOfChange * (float)DeltaTime;
            }

            // Correction for float calculations.
            if (ParentUIBase.Transparencies["Background"] >= TargetTransparencyLevel)
            {
                ParentUIBase.Transparencies["Background"] = TargetTransparencyLevel;
            }

            return ParentUIBase.Transparencies["Background"] >= TargetTransparencyLevel;
        }
    }
}