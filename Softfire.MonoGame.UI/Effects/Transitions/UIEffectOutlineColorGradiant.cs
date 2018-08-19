﻿using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Transitions
{
    public class UIEffectOutlineColorGradiant : UIEffectBase
    {
        /// <summary>
        /// Initial Color.
        /// </summary>
        private Color InitialColor { get; }

        /// <summary>
        /// Target Color.
        /// </summary>
        private Color TargetColor { get; }

        /// <summary>
        /// UI Effect Outline Color Gradiant.
        /// </summary>
        /// <param name="uiBase">The Parent UIBase.</param>
        /// <param name="initialColor">Initial Color. Intaken as a Color.</param>
        /// <param name="targetColor">Target Color. Intaken as a Color.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float.</param>
        /// <param name="orderNumber">Intakes the Effect's run Order Number as an int.</param>
        public UIEffectOutlineColorGradiant(UIBase uiBase, Color initialColor, Color targetColor, float durationInSeconds = 1, float startDelayInSeconds = 0, int orderNumber = 0) : base(uiBase, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            InitialColor = initialColor;
            TargetColor = targetColor;
        }

        /// <summary>
        /// Action.
        /// Defines Effect.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected override bool Action()
        {
            var result = false;
            RateOfChange = ElapsedTime / DurationInSeconds;

            if (ElapsedTime >= StartDelayInSeconds)
            {
                foreach (var outline in ParentUIBase.Outlines)
                {
                    outline.Color = Color.Lerp(InitialColor, TargetColor, (float)RateOfChange);
                    result = outline.Color == TargetColor;
                }
            }

            return result;
        }
    }
}
