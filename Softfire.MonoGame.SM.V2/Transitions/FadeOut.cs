using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.SM.V2.Transitions
{
    public class FadeOut : Transition
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
        /// Fade In Constructor.
        /// </summary>
        /// <param name="state">The Parent State. Intakes a State.</param>
        /// <param name="startingTransparencyLevel">Starting transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="targetTransparencyLevel">Target transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="durationInSeconds">Transition duration in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="startDelayInSeconds">Transition start delay in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="orderNumber">Intakes the Transition's run Order Number as an int.</param>
        public FadeOut(State state, float startingTransparencyLevel = 1f, float targetTransparencyLevel = 0f, float durationInSeconds = 1f, float startDelayInSeconds = 0f, int orderNumber = 0) : base(state, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            StartingTransparencyLevel = MathHelper.Clamp(startingTransparencyLevel, 0f, 1f);
            TargetTransparencyLevel = MathHelper.Clamp(targetTransparencyLevel, 0f, 1f);
            RateOfChange = (StartingTransparencyLevel - TargetTransparencyLevel) / DurationInSeconds;
        }

        /// <summary>
        /// Action.
        /// Defines Transition.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected override bool Action()
        {
            if (ElapsedTime >= StartDelayInSeconds)
            { 
                ParentState.Transparency = MathHelper.Clamp(ParentState.Transparency - (float)RateOfChange * (float)DeltaTime, 0f, 1f);
            }

            // Correction for float calculations.
            if (ParentState.Transparency <= TargetTransparencyLevel)
            {
                ParentState.Transparency = TargetTransparencyLevel;
            }

            return ParentState.Transparency <= TargetTransparencyLevel;
        }
    }
}