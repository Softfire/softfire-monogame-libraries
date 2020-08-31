namespace Softfire.MonoGame.SM.V2.Transitions
{
    public class RotateCounterClockwiseThenClockwise : Transition
    {
        /// <summary>
        /// Rotate Clockwise Transition.
        /// </summary>
        private RotateClockwise RotateClockwise { get; set;  }

        /// <summary>
        /// Rotate Counter Clockwise Transition.
        /// </summary>
        private RotateCounterClockwise RotateCounterClockwise { get; }

        /// <summary>
        /// Counter Clockwise Start Rotation Angle In Degrees.
        /// </summary>
        private double ClockwiseStartRotationAngleInDegrees { get; }

        /// <summary>
        /// Counter Clockwise Target Rotation Angle In Degrees.
        /// </summary>
        private double ClockwiseTargetRotationAngleInDegrees { get; }

        /// <summary>
        /// Counter Clockwise Rotation Duration In Seconds.
        /// </summary>
        private float ClockwiseRotationDurationInSeconds { get; }

        /// <summary>
        /// Counter Clockwise Rotation Delay In Seconds.
        /// </summary>
        private float ClockwiseRotationDelayInSeconds { get; }

        /// <summary>
        /// Is Clockwise Rotation Complete?
        /// </summary>
        private bool IsCounterClockwiseRotationComplete { get; set; }

        /// <summary>
        /// Rotate Clockwise Then Counter Clockwise Transition.
        /// </summary>
        /// <param name="state">The Parent State. Intakes a State.</param>
        /// <param name="counterClockwiseRotationStartAngleInDegrees">Counter clockwise rotation start angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <param name="counterClockwiseRotationTargetAngleInDegrees">Counter clockwise rotation target angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <param name="counterClockwiseRotationDurationInSeconds">Counter clockwise rotation transition duration in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="counterClockwiseRotationStartDelayInSeconds">Counter clockwise rotation transition start delay in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="clockwiseRotationStartAngleInDegrees">Clockwise rotation start angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <param name="clockwiseRotationTargetAngleInDegrees">Clockwise rotation target angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <param name="clockwiseRotationDurationInSeconds">Clockwise rotation transition duration in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="clockwiseRotationStartDelayInSeconds">Clockwise rotation transition start delay in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="orderNumber">Intakes the Transition's run Order Number as an int.</param>
        public RotateCounterClockwiseThenClockwise(State state, double counterClockwiseRotationStartAngleInDegrees,
                                                                double counterClockwiseRotationTargetAngleInDegrees,
                                                                float counterClockwiseRotationDurationInSeconds,
                                                                float counterClockwiseRotationStartDelayInSeconds,
                                                                double clockwiseRotationStartAngleInDegrees,
                                                                double clockwiseRotationTargetAngleInDegrees,
                                                                float clockwiseRotationDurationInSeconds,
                                                                float clockwiseRotationStartDelayInSeconds,
                                                                int orderNumber) : base(state, orderNumber: orderNumber)
        {
            RotateCounterClockwise = new RotateCounterClockwise(state, counterClockwiseRotationStartAngleInDegrees,
                                                                        counterClockwiseRotationTargetAngleInDegrees,
                                                                        counterClockwiseRotationDurationInSeconds,
                                                                        counterClockwiseRotationStartDelayInSeconds,
                                                                        orderNumber);

            ClockwiseStartRotationAngleInDegrees = clockwiseRotationStartAngleInDegrees;
            ClockwiseTargetRotationAngleInDegrees = clockwiseRotationTargetAngleInDegrees;
            ClockwiseRotationDurationInSeconds = clockwiseRotationDurationInSeconds;
            ClockwiseRotationDelayInSeconds = clockwiseRotationStartDelayInSeconds;

            IsCounterClockwiseRotationComplete = false;
        }

        /// <summary>
        /// Action.
        /// Defines Transition.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected override bool Action()
        {
            var result = false;

            if (!IsCounterClockwiseRotationComplete)
            {
                IsCounterClockwiseRotationComplete = RotateCounterClockwise.Run().Result;

                if (IsCounterClockwiseRotationComplete)
                {
                    RotateClockwise = new RotateClockwise(ParentState, ClockwiseStartRotationAngleInDegrees,
                                                                       ClockwiseTargetRotationAngleInDegrees,
                                                                       ClockwiseRotationDurationInSeconds,
                                                                       ClockwiseRotationDelayInSeconds,
                                                                       OrderNumber);
                }
            }
            else
            {
                result = RotateClockwise.Run().Result;
            }

            return result;
        }
    }
}