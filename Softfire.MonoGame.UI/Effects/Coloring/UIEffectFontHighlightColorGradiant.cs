using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Coloring
{
    /// <summary>
    /// An effect to transition the UI's font highlight color to another color.
    /// </summary>
    public class UIEffectFontHighlightColorGradiant : UIEffectBase
    {
        /// <summary>
        /// The effect's initial color.
        /// </summary>
        private Color InitialColor { get; }

        /// <summary>
        /// The effect's target Color.
        /// </summary>
        private Color TargetColor { get; }

        /// <summary>
        /// An effect to transition a UI's font highlight color.
        /// </summary>
        /// <param name="uiBase">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an int.</param>
        /// <param name="name">A unique name. Intaken as a string.</param>
        /// <param name="targetColor">The effect's target color. Intaken as a Color.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a float. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a float. Default is 0f.</param>
        /// <param name="orderNumber">The effect's run order number. Intaken as an int. Default is 0.</param>
        public UIEffectFontHighlightColorGradiant(UIBase uiBase, int id, string name, Color targetColor,
                                                  float durationInSeconds = 1, float startDelayInSeconds = 0, int orderNumber = 0) : base(uiBase, id, name, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            InitialColor = ParentUIBase.Colors["FontHighlight"];
            TargetColor = targetColor;
        }

        /// <summary>
        /// Transitions the UI's font highlight color from the initial color to the target color.
        /// </summary>
        /// <returns>Returns a bool indicating whether the color was transitioned.</returns>
        protected override bool Action()
        {
            RateOfChange = ElapsedTime / DurationInSeconds;

            if (ElapsedTime >= StartDelayInSeconds)
            {
                ParentUIBase.Colors["FontHighlight"] = Color.Lerp(InitialColor, TargetColor, (float)RateOfChange);
            }

            return ParentUIBase.Colors["FontHighlight"] == TargetColor;
        }
    }
}