using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Scaling
{
    /// <summary>
    /// An effect to scale the UI to the right along the X axis.
    /// </summary>
    public class UIEffectScaleRight : UIEffectBase
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
        /// An effect that scales the UI to the right along the X axis.
        /// </summary>
        /// <param name="uiBase">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an int.</param>
        /// <param name="name">A unique name. Intaken as a string.</param>
        /// <param name="targetScale">The target scale (X) to scale the UI by over the duration of time provided. Intaken as a Vector2.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a float. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a float. Default is 0f.</param>
        /// <param name="orderNumber">The effect's run order number. Intaken as an int. Default is 0.</param>
        public UIEffectScaleRight(UIBase uiBase, int id, string name, Vector2 targetScale,
                                  float durationInSeconds = 1f, float startDelayInSeconds = 0f, int orderNumber = 0) : base(uiBase, id, name, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            InitialScale = ParentUIBase.Scale;
            TargetScale = targetScale;

            RateOfChange = TargetScale.Y / DurationInSeconds;
        }

        /// <summary>
        /// Scales the UI to the right.
        /// </summary>
        /// <returns>Returns a bool indicating whether the scaling was completed.</returns>
        protected override bool Action()
        {
            var scale = ParentUIBase.Scale;

            if (ElapsedTime >= StartDelayInSeconds)
            {
                scale.X += (float)RateOfChange * (float)DeltaTime;
            }

            // Correction for float calculations.
            if (scale.X >= InitialScale.X + TargetScale.X)
            {
                scale.X = InitialScale.X + TargetScale.X;
            }

            ParentUIBase.Scale = scale;

            return ParentUIBase.Scale.X >= InitialScale.X + TargetScale.X;
        }
    }
}