namespace Softfire.MonoGame.SM.V2.Transitions
{
    public class RotateClockwise : Transition
    {
        /// <summary>
        /// Start Rotation Angle.
        /// </summary>
        private double StartRotationAngle { get; }

        /// <summary>
        /// Target Rotation Angle.
        /// </summary>
        private double TargetRotationAngle { get; }

        /// <summary>
        /// Rotate Clockkwise.
        /// </summary>
        /// <param name="state">The Parent State. Intakes a State.</param>
        /// <param name="startRotationAngleInDegrees">Start angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <param name="targetRotationAngleInDegrees">Target angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <param name="durationInSeconds">Transition duration in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="startDelayInSeconds">Transition start delay in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="orderNumber">Intakes the Transition's run Order Number as an int.</param>
        public RotateClockwise(State state, double startRotationAngleInDegrees, double targetRotationAngleInDegrees, float durationInSeconds, float startDelayInSeconds, int orderNumber) : base(state, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            StartRotationAngle = startRotationAngleInDegrees;
            TargetRotationAngle = ConvertToRadians(targetRotationAngleInDegrees);
            RateOfChange = (TargetRotationAngle - StartRotationAngle) / DurationInSeconds;
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
                ParentState.RotationAngle += RateOfChange * DeltaTime;
            }

            // Correction for float calculations.
            if (ParentState.RotationAngle >= TargetRotationAngle)
            {
                ParentState.RotationAngle = TargetRotationAngle;
            }

            return ParentState.RotationAngle >= TargetRotationAngle;
        }
    }
}