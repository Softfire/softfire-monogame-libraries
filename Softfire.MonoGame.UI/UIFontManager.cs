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
        /// Loads a <see cref="SpriteFont"/>. Call <see cref="GetFont(string)"/> to access the loaded font.
        /// </summary>
        /// <param name="identifier">The font's unique identifier. Intaken as a <see cref="string"/>.</param>
        /// <param name="fontFilePath">The font's file path. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns the loaded <see cref="SpriteFont"/> if the font was loaded, otherwise null.</returns>
        public SpriteFont LoadFont(string identifier, string fontFilePath)
        {
            SpriteFont font = null;

            if (!string.IsNullOrWhiteSpace(identifier) &&
                !string.IsNullOrWhiteSpace(fontFilePath) &&
                !Fonts.ContainsKey(identifier))
            {
                font = Content.Load<SpriteFont>(fontFilePath);
                Fonts.Add(identifier, font);
            }

            return font;
        }

        /// <summary>
        /// Unloads a <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="identifier">The font's unique identifier. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the font was unloaded.</returns>
        public bool UnloadFont(string identifier)
        {
            return !string.IsNullOrWhiteSpace(identifier) && Fonts.ContainsKey(identifier) && Fonts.Remove(identifier);
        }

        /// <summary>
        /// Unload all loaded <see cref="SpriteFont"/>s.
        /// </summary>
        public void UnloadAllFonts() => Content.Unload();

        /// <summary>
        /// Retrieves a <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="identifier">The font's unique identifier. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns the requested font, if found, otherwise null.</returns>
        public SpriteFont GetFont(string identifier) => !string.IsNullOrWhiteSpace(identifier) && Fonts.ContainsKey(identifier) ? Fonts[identifier] : null;
    }
}