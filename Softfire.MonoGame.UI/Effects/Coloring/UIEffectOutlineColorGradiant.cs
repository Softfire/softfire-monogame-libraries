using System.Linq;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Coloring
{
    /// <summary>
    /// An effect to transition the UI's outline color to another color.
    /// </summary>
    public class UIEffectOutlineColorGradient : UIEffectBase
    {
        /// <summary>
        /// The effect's initial color.
        /// </summary>
        private Color InitialColor { get; set; }

        /// <summary>
        /// The effect's target Color.
        /// </summary>
        private Color TargetColor { get; }

        /// <summary>
        /// The rate of change between calls.
        /// </summary>
        private double RateOfChange { get; set; }

        /// <summary>
        /// An effect to transition a UI's outline color.
        /// </summary>
        /// <param name="parent">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">A unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="targetColor">The effect's target color. Intaken as a Color.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a <see cref="float"/>. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a <see cref="float"/>. Default is 0f.</param>
        public UIEffectOutlineColorGradient(UIBase parent, int id, string name, Color targetColor,
                                            float durationInSeconds = 1f, float startDelayInSeconds = 0f) : base(parent, id, name, durationInSeconds, startDelayInSeconds)
        {
            TargetColor = targetColor;
        }

        /// <summary>
        /// Transitions the UI's outline color from the initial color to the target color.
        /// </summary>
        /// <returns>Returns a bool indicating whether the color was transitioned.</returns>
        protected override bool Action()
        {
            if (IsFirstRun &&
                ElapsedTime >= StartDelayInSeconds)
            {
                InitialColor = Parent.Colors["Outline"];
                IsFirstRun = false;
            }

            if (ElapsedTime >= StartDelayInSeconds)
            {
                RateOfChange += DeltaTime / DurationInSeconds;

                foreach (var outline in Parent.Outlines)
                {
                    outline.Color = Color.Lerp(InitialColor, TargetColor, (float)RateOfChange);
                }
            }

            return Parent.Outlines.All(outline => outline.Color == TargetColor) && ElapsedTime > DurationInSeconds + StartDelayInSeconds;
        }

        /// <summary>
        /// Resets the effect so it can be run again.
        /// </summary>
        protected internal override void Reset()
        {
            // Additional properties to reset.
            RateOfChange = 0;

            // Reset base properties.
            base.Reset();
        }
    }
}