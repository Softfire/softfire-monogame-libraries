using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    public class UIFonts
    {
        /// <summary>
        /// UI Font Content Manager.
        /// </summary>
        private ContentManager UIContent { get; }

        /// <summary>
        /// Fonts.
        /// </summary>
        private Dictionary<string, SpriteFont> Fonts { get; }

        /// <summary>
        /// UIFonts Constructor.
        /// </summary>
        /// <param name="parentContentManager">Intakes the parent's ContentManager.</param>
        public UIFonts(ContentManager parentContentManager)
        {
            UIContent = parentContentManager;

            Fonts = new Dictionary<string, SpriteFont>();
        }

        /// <summary>
        /// Load Font.
        /// </summary>
        /// <param name="identifier">The font's unique identifier. Intakan as a string.</param>
        /// <param name="fontFilePath">The font's file path.</param>
        /// <returns>Returns a bool indicating whether the font was loaded.</returns>
        public bool LoadFont(string identifier, string fontFilePath)
        {
            var result = false;

            if (Fonts.ContainsKey(identifier) == false)
            {
                Fonts.Add(identifier, UIContent.Load<SpriteFont>(fontFilePath));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Unload Fonts.
        /// </summary>
        /// <param name="identifier">The font's unique identifier. Intakan as a string.</param>
        /// <returns>Returns a bool indicating whether the font was unloaded.</returns>
        public bool UnloadFont(string identifier)
        {
            var result = false;

            if (Fonts.ContainsKey(identifier))
            {
                Fonts.Remove(identifier);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Unload All Fonts.
        /// </summary>
        public void UnloadAllFonts()
        {
            UIContent.Unload();
        }

        /// <summary>
        /// Get Font.
        /// </summary>
        /// <param name="identifier">The font's unique identifier. Intakan as a string.</param>
        /// <returns>Returns the requested font or null if not found.</returns>
        public SpriteFont GetFont(string identifier)
        {
            SpriteFont font = null;

            if (Fonts.ContainsKey(identifier))
            {
                font = Fonts[identifier];
            }

            return font;
        }
    }
}