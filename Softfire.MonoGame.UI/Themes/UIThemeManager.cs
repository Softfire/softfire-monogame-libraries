using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI.Themes
{
    public class UIThemeManager
    {
        /// <summary>
        /// The available themes for use.
        /// </summary>
        private List<UITheme> Themes { get; } = new List<UITheme>();

        /// <summary>
        /// The UI theme manager constructor.
        /// </summary>
        public UIThemeManager()
        {
            
        }

        /// <summary>
        /// A theme that is used to customize the UI.
        /// </summary>
        /// <param name="name">The theme's name. Intaken as a string.</param>
        /// <param name="font">The theme's font to use. Intaken as a SpriteFont.</param>
        /// <param name="fontColor">The theme's font color to use. Intaken as a Color.</param>
        /// <param name="backgroundColor">The theme's background color to use. Intaken as a Color.</param>
        /// <param name="highlightColor">The theme's highlight color to use. Intaken as a Color.</param>
        /// <param name="outlineColor">The theme's outline color to use. Intaken as a Color.</param>
        /// <param name="fontHighlightColor">The theme's font highlight color to use. Intaken as a Color.</param>
        /// <param name="selectionColor">The theme's selection color to use. Intaken as a Color.</param>
        /// <param name="fontTransparencyLevel">The theme's font transparency level. Intaken as a float.</param>
        /// <param name="backgroundTransparencyLevel">The theme's background transparency level. Intaken as a float.</param>
        /// <param name="highlightTransparencyLevel">The theme's highlight transparency level. Intaken as a float.</param>
        /// <param name="outlineTransparencyLevel">The theme's outline transparency level. Intaken as a float.</param>
        /// <param name="fontHighlightTransparencyLevel">The theme's font highlight transparency level. Intaken as a float.</param>
        /// <param name="selectionTransparencyLevel">The theme's selection transparency level. Intaken as a float.</param>
        public int AddTheme(string name, SpriteFont font, Color fontColor, Color backgroundColor, Color highlightColor, Color outlineColor, Color fontHighlightColor, Color selectionColor,
                             float fontTransparencyLevel, float backgroundTransparencyLevel, float highlightTransparencyLevel, float outlineTransparencyLevel, float fontHighlightTransparencyLevel, float selectionTransparencyLevel)
        {
            var nextThemeId = UIBase.GetNextValidItemId(Themes);

            Themes.Add(new UITheme(nextThemeId, name, nextThemeId, font, fontColor, backgroundColor, highlightColor, outlineColor, fontHighlightColor, selectionColor,
                                   fontTransparencyLevel, backgroundTransparencyLevel, highlightTransparencyLevel, outlineTransparencyLevel, fontHighlightTransparencyLevel, selectionTransparencyLevel));

            return nextThemeId;
        }

        /// <summary>
        /// Applies the theme, by id, to the UI elements in the provided list.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="list">The list of UI elements to apply the theme.</param>
        /// <param name="themeId">The id of the theme to apply.</param>
        /// <returns>Returns a boolean indicating whether the theme has been applied.</returns>
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
        /// <returns>Returns a boolean indicating whether the theme has been applied.</returns>
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
        /// Gets a theme by id.
        /// </summary>
        /// <param name="themeId">The id of the theme to retrieve. Intaken as an int.</param>
        /// <returns>Returns the theme with the specified id, if present, otherwise null.</returns>
        public UITheme GetTheme(int themeId)
        {
            return UIBase.GetItemById(Themes, themeId);
        }

        /// <summary>
        /// Gets a theme by name.
        /// </summary>
        /// <param name="themeName">The name of the theme to retrieve. Intaken as an int.</param>
        /// <returns>Returns the theme with the specified name, if present, otherwise null.</returns>
        public UITheme GetTheme(string themeName)
        {
            return UIBase.GetItemByName(Themes, themeName);
        }

        /// <summary>
        /// Removes a theme by id.
        /// </summary>
        /// <param name="themeId">The id of the theme to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the theme was removed.</returns>
        public bool RemoveTheme(int themeId)
        {
            return UIBase.RemoveItemById(Themes, themeId);
        }

        /// <summary>
        /// Removes a theme by name.
        /// </summary>
        /// <param name="themeName">The name of the theme to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the theme was removed.</returns>
        public bool RemoveTheme(string themeName)
        {
            return UIBase.RemoveItemByName(Themes, themeName);
        }

        /// <summary>
        /// Increases a theme's order number by id.
        /// </summary>
        /// <param name="themeId">The id of the theme to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the theme's order number was increased.</returns>
        public bool IncreaseThemeOrderNumber(int themeId)
        {
            return UIBase.IncreaseItemOrderNumber(Themes, themeId);
        }

        /// <summary>
        /// Increases a theme's order number by name.
        /// </summary>
        /// <param name="themeName">The name of the theme to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the theme's order number was increased.</returns>
        public bool IncreaseThemeOrderNumber(string themeName)
        {
            return UIBase.IncreaseItemOrderNumber(Themes, themeName);
        }

        /// <summary>
        /// Decreases a theme's order number by id.
        /// </summary>
        /// <param name="themeId">The id of the theme to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the theme's order number was decreased.</returns>
        public bool DecreaseThemeOrderNumber(int themeId)
        {
            return UIBase.DecreaseItemOrderNumber(Themes, themeId);
        }

        /// <summary>
        /// Decreases a theme's order number by name.
        /// </summary>
        /// <param name="themeName">The name of the theme to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the theme's order number was decreased.</returns>
        public bool DecreaseThemeOrderNumber(string themeName)
        {
            return UIBase.DecreaseItemOrderNumber(Themes, themeName);
        }
    }
}