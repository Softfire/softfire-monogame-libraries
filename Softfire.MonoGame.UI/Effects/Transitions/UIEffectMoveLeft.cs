﻿using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Transitions
{
    public class UIEffectMoveLeft : UIEffectBase
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
        /// UI Effect Move Left.
        /// Moves the UIBase left(-) on the X axis.
        /// </summary>
        /// <param name="uiBase">The Parent UIBase.</param>
        /// <param name="startPosition">UIBase start position. Intaken as a Vector2.</param>
        /// <param name="targetPosition">UIBase target position. Intaken as a Vector2.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float.</param>
        /// <param name="orderNumber">Intakes the Effect's run Order Number as an int.</param>
        public UIEffectMoveLeft(UIBase uiBase, Vector2 startPosition, Vector2 targetPosition, float durationInSeconds, float startDelayInSeconds, int orderNumber) : base(uiBase, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            StartPosition = startPosition;
            TargetPosition = targetPosition;
            RateOfChange = (StartPosition.X - TargetPosition.X) / DurationInSeconds;
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
                ParentUIBase.Position = new Vector2(ParentUIBase.Position.X - (float)RateOfChange * (float)DeltaTime, ParentUIBase.Position.Y);
            }

            // Correction for float calculations.
            if (ParentUIBase.Position.X <= TargetPosition.X)
            {
                ParentUIBase.Position = new Vector2(TargetPosition.X, ParentUIBase.Position.Y);
            }

            return ParentUIBase.Position.X <= TargetPosition.X;
        }
    }
}