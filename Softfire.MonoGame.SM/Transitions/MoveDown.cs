using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.SM.Transitions
{
    public class MoveDown : Transition
    {
        /// <summary>
        /// Start Position.
        /// </summary>
        private Vector2 StartPosition { get; }

        /// <summary>
        /// Target Position.
        /// </summary>
        private Vector2 TargetPosition { get; }

        /// <summary>
        /// Move Down.
        /// Moves the state up(+) on the Y axis.
        /// </summary>
        /// <param name="state">The Parent State. Intakes a State.</param>
        /// <param name="startPosition">Transition start position. Intaken as a Vector2.</param>
        /// <param name="targetPosition">Transition target position. Intaken as a Vector2.</param>
        /// <param name="durationInSeconds">Transition duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Transition start delay in seconds. Intaken as a float.</param>
        /// <param name="orderNumber">Intakes the Transition's run Order Number as an int.</param>
        public MoveDown(State state, Vector2 startPosition, Vector2 targetPosition, float durationInSeconds, float startDelayInSeconds, int orderNumber) : base(state, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            StartPosition = startPosition;
            TargetPosition = targetPosition;
            RateOfChange = (TargetPosition.Y - StartPosition.Y) / DurationInSeconds;
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
                ParentState.Position = new Vector2(ParentState.Position.X, ParentState.Position.Y + (float)RateOfChange * (float)DeltaTime);
            }

            // Correction for float calculations.
            if (ParentState.Position.Y >= TargetPosition.Y)
            {
                ParentState.Position = new Vector2(ParentState.Position.X, TargetPosition.Y);
            }

            return ParentState.Position.Y >= TargetPosition.Y;
        }
    }
}