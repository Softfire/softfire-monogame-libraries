using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI.Effects.Highlighting
{
    public class UIEffectHighlight : UIEffectBase
    {
        /// <summary>
        /// Highlight Texture.
        /// </summary>
        private Texture2D Texture { get; }

        /// <summary>
        /// Highlight Color.
        /// </summary>
        private Color Color { get;  }

        /// <summary>
        /// Highlight Transparency Level.
        /// </summary>
        private float TransparencyLevel { get; }

        public UIEffectHighlight(UIBase uiBase, Texture2D highlightTexture, Color highlightColor, float highlightTransparencyLevel,
                                 float durationInSeconds = 1, float startDelayInSeconds = 0, int orderNumber = 0) : base(uiBase, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            Texture = highlightTexture;
            Color = highlightColor;
            TransparencyLevel = highlightTransparencyLevel;
        }

        protected override bool Action()
        {
            RateOfChange = ElapsedTime / DurationInSeconds;

            if (ElapsedTime >= StartDelayInSeconds)
            {
                ParentUIBase.IsHighlighting = true;
                ParentUIBase.Textures["Highlight"] = Texture;
                ParentUIBase.Colors["Highlight"] = Color;
                ParentUIBase.Transparencies["Highlight"] = TransparencyLevel;
            }

            return ParentUIBase.IsHighlighting;
        }
    }
}