using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Coloring
{
    /// <summary>
    /// An effect to transition the UI's highlight color to another color.
    /// </summary>
    public class UIEffectHighlightColorGradient : UIEffectBase
    {
        /// <summary>
        /// The effect's target Color.
        /// </summary>
        private Color TargetColor { get; }

        /// <summary>
        /// An effect to transition a UI's highlight color.
        /// </summary>
        /// <param name="uiBase">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an int.</param>
        /// <param name="name">A unique name. Intaken as a string.</param>
        /// <param name="targetColor">The effect's target color. Intaken as a Color.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a float. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a float. Default is 0f.</param>
        /// <param name="orderNumber">The effect's run order number. Intaken as an int. Default is 0.</param>
        public UIEffectHighlightColorGradient(UIBase uiBase, int id, string name, Color targetColor,
                                              float durationInSeconds = 1f, float startDelayInSeconds = 0f, int orderNumber = 0) : base(uiBase, id, name, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            TargetColor = targetColor;
        }

        /// <summary>
        /// Transitions the UI's highlight color from the initial color to the target color.
        /// </summary>
        /// <returns>Returns a bool indicating whether the color was transitioned.</returns>
        protected override bool Action()
        {
            if (ElapsedTime >= StartDelayInSeconds)
            {
                RateOfChange += DeltaTime / DurationInSeconds;
                ParentUIBase.Colors["Highlight"] = Color.Lerp(ParentUIBase.Colors["Highlight"], TargetColor, (float)RateOfChange);
            }

            return ParentUIBase.Colors["Highlight"] == TargetColor;
        }
    }
}