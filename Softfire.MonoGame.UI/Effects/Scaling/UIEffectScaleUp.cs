using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Scaling
{
    /// <summary>
    /// An effect to scale the UI up along the Y axis.
    /// </summary>
    public class UIEffectScaleUp : UIEffectBase
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
        /// An effect that scales the UI up along the Y axis.
        /// </summary>
        /// <param name="uiBase">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an int.</param>
        /// <param name="name">A unique name. Intaken as a string.</param>
        /// <param name="targetScale">The target scale (Y) to scale the UI by over the duration of time provided. Intaken as a Vector2.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a float. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a float. Default is 0f.</param>
        /// <param name="orderNumber">The effect's run order number. Intaken as an int. Default is 0.</param>
        public UIEffectScaleUp(UIBase uiBase, int id, string name, Vector2 targetScale, float durationInSeconds = 1, float startDelayInSeconds = 0, int orderNumber = 0) : base(uiBase, id, name, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            InitialScale = ParentUIBase.Scale;
            TargetScale = targetScale;
        }

        /// <summary>
        /// Scales the UI up.
        /// </summary>
        /// <returns>Returns a bool indicating whether the scaling was completed.</returns>
        protected override bool Action()
        {
            var scale = ParentUIBase.Scale;

            if (ElapsedTime >= StartDelayInSeconds)
            {
                RateOfChange = TargetScale.Y / DurationInSeconds;
                scale.Y -= (float)RateOfChange * (float)DeltaTime;
            }

            // Correction for float calculations.
            if (scale.Y <= InitialScale.Y - TargetScale.Y)
            {
                scale.Y = InitialScale.Y - TargetScale.Y;
            }

            ParentUIBase.Scale = scale;

            return ParentUIBase.Scale.Y <= InitialScale.Y - TargetScale.Y;
        }
    }
}