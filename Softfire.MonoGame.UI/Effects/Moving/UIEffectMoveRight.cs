﻿using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Moving
{
    /// <summary>
    /// An effect to shift the UI to the right along the X axis from it's current position.
    /// </summary>
    public class UIEffectMoveRight : UIEffectBase
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
        /// An effect that moves the UI to the right along the X axis.
        /// </summary>
        /// <param name="uiBase">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an int.</param>
        /// <param name="name">A unique name. Intaken as a string.</param>
        /// <param name="startPosition">The effect's start position. Intaken as a Vector2.</param>
        /// <param name="targetPosition">The effect's target position. Intaken as a Vector2.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a float. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a float. Default is 0f.</param>
        /// <param name="orderNumber">The effect's run order number. Intaken as an int. Default is 0.</param>
        public UIEffectMoveRight(UIBase uiBase, int id, string name, Vector2 startPosition, Vector2 targetPosition,
                                 float durationInSeconds = 1f, float startDelayInSeconds = 0f, int orderNumber = 0) : base(uiBase, id, name, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            StartPosition = startPosition;
            TargetPosition = targetPosition;
            RateOfChange = (TargetPosition.X - StartPosition.X) / DurationInSeconds;
        }

        /// <summary>
        /// Moves the UI to the right.
        /// </summary>
        /// <returns>Returns a bool indicating whether the move was completed.</returns>
        protected override bool Action()
        {
            if (ElapsedTime >= StartDelayInSeconds)
            {
                ParentUIBase.Position = new Vector2(ParentUIBase.Position.X + (float)RateOfChange * (float)DeltaTime, ParentUIBase.Position.Y);
            }

            // Correction for float calculations.
            if (ParentUIBase.Position.X >= TargetPosition.X)
            {
                ParentUIBase.Position = new Vector2(TargetPosition.X, ParentUIBase.Position.Y);
            }

            return ParentUIBase.Position.X >= TargetPosition.X;
        }
    }
}