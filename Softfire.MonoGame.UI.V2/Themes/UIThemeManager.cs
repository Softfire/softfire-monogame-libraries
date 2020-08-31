using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE.V2;

namespace Softfire.MonoGame.UI.V2.Themes
{
    /// <summary>
    /// A UI Theme Manager.
    /// </summary>
    public class UIThemeManager
    {
        /// <summary>
        /// The available themes for use.
        /// </summary>
        private List<UITheme> Themes { get; }

        /// <summary>
        /// The UI theme manager constructor.
        /// </summary>
        public UIThemeManager()
        {
            Themes = new List<UITheme>();
        }

        /// <summary>
        /// A theme that is used to customize the UI.
        /// </summary>
        /// <param name="name">The theme's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="font">The theme's font to use. Intaken as a <see cref="SpriteFont"/>.</param>
        /// <param name="fontColor">The theme's font color to use. Intaken as a Color.</param>
        /// <param name="backgroundColor">The theme's background color to use. Intaken as a Color.</param>
        /// <param name="highlightColor">The theme's highlight color to use. Intaken as a Color.</param>
        /// <param name="outlineColor">The theme's outline color to use. Intaken as a Color.</param>
        /// <param name="fontHighlightColor">The theme's font highlight color to use. Intaken as a Color.</param>
        /// <param name="selectionColor">The theme's selection color to use. Intaken as a Color.</param>
        /// <param name="fontTransparencyLevel">The theme's font transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="backgroundTransparencyLevel">The theme's background transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="highlightTransparencyLevel">The theme's highlight transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="outlineTransparencyLevel">The theme's outline transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="fontHighlightTransparencyLevel">The theme's font highlight transparency level. Intaken as a <see cref="float"/>.</param>
        /// <param name="selectionTransparencyLevel">The theme's selection transparency level. Intaken as a <see cref="float"/>.</param>
        public int AddTheme(string name, SpriteFont font,
                            Color fontColor,
                            Color backgroundColor,
                            Color highlightColor,
                            Color outlineColor,
                            Color fontHighlightColor,
                            Color selectionColor,
                            float backgroundTransparencyLevel = 1f,
                            float highlightTransparencyLevel= 0.25f,
                            float outlineTransparencyLevel = 1f,
                            float fontTransparencyLevel = 1f,
                            float fontHighlightTransparencyLevel = 0.75f,
                            float selectionTransparencyLevel = 0.75f)
        {
            var nextThemeId = 0;

            if (!CheckForTheme(name))
            {
                nextThemeId = Identities.GetNextValidObjectId<UITheme, UITheme>(Themes);

                if (!CheckForTheme(nextThemeId))
                {
                    Themes.Add(new UITheme(nextThemeId, name, font, fontColor, backgroundColor, highlightColor, outlineColor, fontHighlightColor, selectionColor,
                                           fontTransparencyLevel, backgroundTransparencyLevel, highlightTransparencyLevel,
                                           outlineTransparencyLevel, fontHighlightTransparencyLevel, selectionTransparencyLevel));
                }
            }

            return nextThemeId;
        }

        /// <summary>
        /// Applies the theme, by id, to the UI elements in the provided list.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="list">The list of UI elements to apply the theme.</param>
        /// <param name="themeId">The id of the theme to apply.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the theme has been applied.</returns>
        public bool ApplyTheme<T>(IEnumerable<T> list, int themeId) where  T : UIBase
        {
            var result = false;
            var theme = GetTheme(themeId);
            foreach (var uiBase in list)
            {
                result = theme?.Apply(uiBase) ?? false;
            }

            return result;
        }

        /// <summary>
        /// Applies the theme, by name, to the UI elements in the provided list.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="list">The list of UI elements to apply the theme.</param>
        /// <param name="themeName">The name of the theme to apply.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the theme has been applied.</returns>
        public bool ApplyTheme<T>(IEnumerable<T> list, string themeName) where T : UIBase
        {
            var result = false;
            var theme = GetTheme(themeName);
            foreach (var uiBase in list)
            {
                result = theme?.Apply(uiBase) ?? false;
            }

            return result;
        }

        /// <summary>
        /// Checks for a theme by id.
        /// </summary>
        /// <param name="themeId">The id of the theme to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the theme is present.</returns>
        public bool CheckForTheme(int themeId) => Identities.ObjectExists<UITheme, UITheme>(Themes, themeId);

        /// <summary>
        /// Checks for a theme by name.
        /// </summary>
        /// <param name="themeName">The name of the theme to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the theme is present.</returns>
        public bool CheckForTheme(string themeName) => Identities.ObjectExists<UITheme, UITheme>(Themes, themeName);

        /// <summary>
        /// Gets a theme by id.
        /// </summary>
        /// <param name="themeId">The id of the theme to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the theme with the specified id, if present, otherwise null.</returns>
        public UITheme GetTheme(int themeId) => Identities.GetObject<UITheme, UITheme>(Themes, themeId);

        /// <summary>
        /// Gets a theme by name.
        /// </summary>
        /// <param name="themeName">The name of the theme to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the theme with the specified name, if present, otherwise null.</returns>
        public UITheme GetTheme(string themeName) => Identities.GetObject<UITheme, UITheme>(Themes, themeName);

        /// <summary>
        /// Removes a theme by id.
        /// </summary>
        /// <param name="themeId">The id of the theme to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the theme was removed.</returns>
        public bool RemoveTheme(int themeId) => Identities.RemoveObject<UITheme, UITheme>(Themes, themeId);

        /// <summary>
        /// Removes a theme by name.
        /// </summary>
        /// <param name="themeName">The name of the theme to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the theme was removed.</returns>
        public bool RemoveTheme(string themeName) => Identities.RemoveObject<UITheme, UITheme>(Themes, themeName);
    }
}