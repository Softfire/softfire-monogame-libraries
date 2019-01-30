using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// A manager for MonoGame fonts.
    /// </summary>
    public class UIFontManager
    {
        /// <summary>
        /// UI Font Content Manager.
        /// </summary>
        private ContentManager Content { get; }

        /// <summary>
        /// Fonts.
        /// </summary>
        private Dictionary<string, SpriteFont> Fonts { get; } = new Dictionary<string, SpriteFont>();

        /// <summary>
        /// UIFonts Constructor.
        /// </summary>
        /// <param name="parentContentManager">Intakes the parent's ContentManager.</param>
        public UIFontManager(ContentManager parentContentManager)
        {
            Content = new ContentManager(parentContentManager.ServiceProvider, "Content");
        }

        /// <summary>
        /// Load Font.
        /// </summary>
        /// <param name="identifier">The font's unique identifier. Intaken as a <see cref="string"/>.</param>
        /// <param name="fontFilePath">The font's file path.</param>
        /// <returns>Returns a bool indicating whether the font was loaded.</returns>
        public bool LoadFont(string identifier, string fontFilePath)
        {
            var result = false;

            if (!string.IsNullOrWhiteSpace(identifier) &&
                !string.IsNullOrWhiteSpace(fontFilePath) &&
                !Fonts.ContainsKey(identifier))
            {
                Fonts.Add(identifier, Content.Load<SpriteFont>(fontFilePath));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Unload Fonts.
        /// </summary>
        /// <param name="identifier">The font's unique identifier. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the font was unloaded.</returns>
        public bool UnloadFont(string identifier)
        {
            return !string.IsNullOrWhiteSpace(identifier) && Fonts.ContainsKey(identifier) && Fonts.Remove(identifier);
        }

        /// <summary>
        /// Unload All Fonts.
        /// </summary>
        public void UnloadAllFonts()
        {
            Content.Unload();
        }

        /// <summary>
        /// Get Font.
        /// </summary>
        /// <param name="identifier">The font's unique identifier. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns the requested font or null if not found.</returns>
        public SpriteFont GetFont(string identifier)
        {
            return !string.IsNullOrWhiteSpace(identifier) && Fonts.ContainsKey(identifier) ? Fonts[identifier] : null;
        }
    }
}