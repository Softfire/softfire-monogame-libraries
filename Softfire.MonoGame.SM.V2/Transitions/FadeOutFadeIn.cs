namespace Softfire.MonoGame.SM.Transitions
{
    public class FadeOutFadeIn : Transition
    {
        /// <summary>
        /// Fade Out.
        /// </summary>
        private FadeOut FadeOut { get; }

        /// <summary>
        /// Fade In.
        /// </summary>
        private FadeIn FadeIn { get; set; }

        /// <summary>
        /// Fade In Starting Transparency Level.
        /// </summary>
        private float FadeInStartingTransparencyLevel { get; }

        /// <summary>
        /// Fade In Target Transparency Level.
        /// </summary>
        private float FadeInTargetTransparencyLevel { get; }

        /// <summary>
        /// Fade In Duration In Seconds.
        /// </summary>
        private float FadeInDurationInSeconds { get; }

        /// <summary>
        /// Fade In Delay In Seconds.
        /// </summary>
        private float FadeInDelayInSeconds { get; }

        /// <summary>
        /// Is Fade Out Complete?
        /// </summary>
        private bool IsFadeOutComplete { get; set; }

        /// <summary>
        /// Fade Out Fade In Constructor.
        /// </summary>
        /// <param name="state">The Parent State. Intakes a State.</param>
        /// <param name="fadeOutStartingTransparencyLevel">Fade out starting transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="fadeOutTargetTransparencyLevel">Fade out target transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="fadeOutDurationInSeconds">Fade out transition duration in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="fadeOutStartDelayInSeconds">Fade out transition start delay in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="fadeInStartingTransparencyLevel">Fade in starting transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="fadeInTargetTransparencyLevel">Fade in target transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="fadeInDurationInSeconds">Fade in transition duration in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="fadeInStartDelayInSeconds">Fade in transition start delay in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="orderNumber">Intakes the Transition's run Order Number as an int.</param>
        public FadeOutFadeIn(State state, float fadeOutStartingTransparencyLevel,
                                          float fadeOutTargetTransparencyLevel,
                                          float fadeOutDurationInSeconds,
                                          float fadeOutStartDelayInSeconds,
                                          float fadeInStartingTransparencyLevel,
                                          float fadeInTargetTransparencyLevel,
                                          float fadeInDurationInSeconds,
                                          float fadeInStartDelayInSeconds,
                                          int orderNumber) : base(state, orderNumber: orderNumber)
        {
            FadeOut = new FadeOut(state, fadeOutDurationInSeconds, fadeOutTargetTransparencyLevel, fadeOutStartingTransparencyLevel, fadeOutStartDelayInSeconds, orderNumber);

            FadeInDurationInSeconds = fadeInDurationInSeconds;
            FadeInTargetTransparencyLevel = fadeInTargetTransparencyLevel;
            FadeInStartingTransparencyLevel = fadeInStartingTransparencyLevel;
            FadeInDelayInSeconds = fadeInStartDelayInSeconds;

            IsFadeOutComplete = false;
        }

        /// <summary>
        /// Action.
        /// Defines Transition.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected override bool Action()
        {
            var result = false;

            if (!IsFadeOutComplete)
            {
                IsFadeOutComplete = FadeOut.Run().Result;

                if (IsFadeOutComplete)
                {
                    FadeIn = new FadeIn(ParentState, FadeInStartingTransparencyLevel, FadeInTargetTransparencyLevel, FadeInDurationInSeconds, FadeInDelayInSeconds, OrderNumber);
                }
            }
            else
            {
                result = FadeIn.Run().Result;
            }

            return result;
        }
    }
}