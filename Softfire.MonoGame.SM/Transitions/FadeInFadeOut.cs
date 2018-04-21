namespace Softfire.MonoGame.SM.Transitions
{
    public class FadeInFadeOut : Transition
    {
        /// <summary>
        /// Fade In.
        /// </summary>
        private FadeIn FadeIn { get; }

        /// <summary>
        /// Fade Out.
        /// </summary>
        private FadeOut FadeOut { get; set; }

        /// <summary>
        /// Fade Out Starting Transparency Level.
        /// </summary>
        private float FadeOutStartingTransparencyLevel { get; }

        /// <summary>
        /// Fade Out Target Transparency Level.
        /// </summary>
        private float FadeOutTargetTransparencyLevel { get; }

        /// <summary>
        /// Fade Out Duration In Seconds.
        /// </summary>
        private float FadeOutDurationInSeconds { get; }

        /// <summary>
        /// Fade Out Delay In Seconds.
        /// </summary>
        private float FadeOutDelayInSeconds { get; }

        /// <summary>
        /// Is Fade In Complete?
        /// </summary>
        private bool IsFadeInComplete { get; set; }

        /// <summary>
        /// Fade In Fade Out Constructor.
        /// </summary>
        /// <param name="state">The Parent State. Intakes a State.</param>
        /// <param name="fadeInStartingTransparencyLevel">Fade in starting transparency level. Intaken as a float.</param>
        /// <param name="fadeInTargetTransparencyLevel">Fade in target transparency level. Intaken as a float.</param>
        /// <param name="fadeInDurationInSeconds">Fade in transition duration in seconds. Intaken as a float.</param>
        /// <param name="fadeInStartDelayInSeconds">Fade in transition start delay in seconds. Intaken as a float.</param>
        /// <param name="fadeOutStartingTransparencyLevel">Fade out starting transparency level. Intaken as a float.</param>
        /// <param name="fadeOutTargetTransparencyLevel">Fade out target transparency level. Intaken as a float.</param>
        /// <param name="fadeOutDurationInSeconds">Fade out transition duration in seconds. Intaken as a float.</param>
        /// <param name="fadeOutStartDelayInSeconds">Fade out transition start delay in seconds. Intaken as a float.</param>
        /// <param name="orderNumber">Intakes the Transition's run Order Number as an int.</param>
        public FadeInFadeOut(State state, float fadeInStartingTransparencyLevel,
                                          float fadeInTargetTransparencyLevel,
                                          float fadeInDurationInSeconds,
                                          float fadeInStartDelayInSeconds,
                                          float fadeOutStartingTransparencyLevel,
                                          float fadeOutTargetTransparencyLevel,
                                          float fadeOutDurationInSeconds,
                                          float fadeOutStartDelayInSeconds,
                                          int orderNumber) : base(state, orderNumber: orderNumber)
        {
            FadeIn = new FadeIn(state, fadeInStartingTransparencyLevel, fadeInTargetTransparencyLevel, fadeInDurationInSeconds, fadeInStartDelayInSeconds, orderNumber);

            FadeOutStartingTransparencyLevel = fadeOutStartingTransparencyLevel;
            FadeOutTargetTransparencyLevel = fadeOutTargetTransparencyLevel;
            FadeOutDurationInSeconds = fadeOutDurationInSeconds;
            FadeOutDelayInSeconds = fadeOutStartDelayInSeconds;

            IsFadeInComplete = false;
        }

        /// <summary>
        /// Action.
        /// Defines Transition.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected override bool Action()
        {
            var result = false;

            if (IsFadeInComplete == false)
            {
                IsFadeInComplete = FadeIn.Run().Result;

                if (IsFadeInComplete)
                {
                    FadeOut = new FadeOut(ParentState, FadeOutStartingTransparencyLevel, FadeOutTargetTransparencyLevel, FadeOutDurationInSeconds, FadeOutDelayInSeconds, OrderNumber);
                }
            }
            else
            {
                result = FadeOut.Run().Result;
            }

            return result;
        }
    }
}