namespace Softfire.MonoGame.SM.V2.Transitions
{
    public class RotateClockwiseThenCounterClockwise : Transition
    {
        /// <summary>
        /// Rotate Clockwise Transition.
        /// </summary>
        private RotateClockwise RotateClockwise { get; }

        /// <summary>
        /// Rotate Counter Clockwise Transition.
        /// </summary>
        private RotateCounterClockwise RotateCounterClockwise { get; set; }

        /// <summary>
        /// Counter Clockwise Start Rotation Angle In Degrees.
        /// </summary>
        private double CounterClockwiseStartRotationAngleInDegrees { get; }

        /// <summary>
        /// Counter Clockwise Target Rotation Angle In Degrees.
        /// </summary>
        private double CounterClockwiseTargetRotationAngleInDegrees { get; }

        /// <summary>
        /// Counter Clockwise Rotation Duration In Seconds.
        /// </summary>
        private float CounterClockwiseRotationDurationInSeconds { get; }

        /// <summary>
        /// Counter Clockwise Rotation Delay In Seconds.
        /// </summary>
        private float CounterClockwiseRotationDelayInSeconds { get; }

        /// <summary>
        /// Is Clockwise Rotation Complete?
        /// </summary>
        private bool IsClockwiseRotationComplete { get; set; }

        /// <summary>
        /// Rotate Clockwise Then Counter Clockwise Transition.
        /// </summary>
        /// <param name="state">The Parent State. Intakes a State.</param>
        /// <param name="clockwiseRotationStartAngleInDegrees">Clockwise rotation start angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <param name="clockwiseRotationTargetAngleInDegrees">Clockwise rotation target angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <param name="clockwiseRotationDurationInSeconds">Clockwise rotation transition duration in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="clockwiseRotationStartDelayInSeconds">Clockwise rotation transition start delay in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="counterClockwiseRotationStartAngleInDegrees">Counter clockwise rotation start angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <param name="counterClockwiseRotationTargetAngleInDegrees">Counter clockwise rotation target angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <param name="counterClockwiseRotationDurationInSeconds">Counter clockwise rotation transition duration in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="counterClockwiseRotationStartDelayInSeconds">Counter clockwise rotation transition start delay in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="orderNumber">Intakes the Transition's run Order Number as an int.</param>
        public RotateClockwiseThenCounterClockwise(State state, double clockwiseRotationStartAngleInDegrees,
                                                                double clockwiseRotationTargetAngleInDegrees,
                                                                float clockwiseRotationDurationInSeconds,
                                                                float clockwiseRotationStartDelayInSeconds,
                                                                double counterClockwiseRotationStartAngleInDegrees,
                                                                double counterClockwiseRotationTargetAngleInDegrees,
                                                                float counterClockwiseRotationDurationInSeconds,
                                                                float counterClockwiseRotationStartDelayInSeconds,
                                                                int orderNumber) : base(state, orderNumber: orderNumber)
        {
            RotateClockwise = new RotateClockwise(state, clockwiseRotationStartAngleInDegrees,
                                                         clockwiseRotationTargetAngleInDegrees,
                                                         clockwiseRotationDurationInSeconds,
                                                         clockwiseRotationStartDelayInSeconds,
                                                         orderNumber);

            CounterClockwiseStartRotationAngleInDegrees = counterClockwiseRotationStartAngleInDegrees;
            CounterClockwiseTargetRotationAngleInDegrees = counterClockwiseRotationTargetAngleInDegrees;
            CounterClockwiseRotationDurationInSeconds = counterClockwiseRotationDurationInSeconds;
            CounterClockwiseRotationDelayInSeconds = counterClockwiseRotationStartDelayInSeconds;

            IsClockwiseRotationComplete = false;
        }

        /// <summary>
        /// Action.
        /// Defines Transition.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected override bool Action()
        {
            var result = false;

            if (!IsClockwiseRotationComplete)
            {
                IsClockwiseRotationComplete = RotateClockwise.Run().Result;

                if (IsClockwiseRotationComplete)
                {
                    RotateCounterClockwise = new RotateCounterClockwise(ParentState, CounterClockwiseStartRotationAngleInDegrees,
                                                                                     CounterClockwiseTargetRotationAngleInDegrees,
                                                                                     CounterClockwiseRotationDurationInSeconds,
                                                                                     CounterClockwiseRotationDelayInSeconds,
                                                                                     OrderNumber);
                }
            }
            else
            {
                result = RotateCounterClockwise.Run().Result;
            }

            return result;
        }
    }
}