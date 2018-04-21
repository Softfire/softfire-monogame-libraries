using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Transitions
{
    public class UIEffectMoveUp : UIBaseEffect
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
        /// UI Effect Move Up.
        /// Moves the UIBase up(-) on the Y axis.
        /// </summary>
        /// <param name="uiBase">The Parent UIBase.</param>
        /// <param name="startPosition">UIBase start position. Intaken as a Vector2.</param>
        /// <param name="targetPosition">UIBase target position. Intaken as a Vector2.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float.</param>
        /// <param name="orderNumber">Intakes the Effect's run Order Number as an int.</param>
        public UIEffectMoveUp(UIBase uiBase, Vector2 startPosition, Vector2 targetPosition, float durationInSeconds, float startDelayInSeconds, int orderNumber) : base(uiBase, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            StartPosition = startPosition;
            TargetPosition = targetPosition;
            RateOfChange = (StartPosition.Y - TargetPosition.Y) / DurationInSeconds;
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
                ParentUIBase.Position = new Vector2(ParentUIBase.Position.X, ParentUIBase.Position.Y - (float)RateOfChange * (float)DeltaTime);
            }

            // Correction for float calculations.
            if (ParentUIBase.Position.Y <= TargetPosition.Y)
            {
                ParentUIBase.Position = new Vector2(ParentUIBase.Position.X, TargetPosition.Y);
            }

            return ParentUIBase.Position.Y <= TargetPosition.Y;
        }
    }
}