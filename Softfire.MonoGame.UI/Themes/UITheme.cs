using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI.Themes
{
    /// <summary>
    /// A custom theme for easily modifying the UI.
    /// </summary>
    public class UITheme : IUIIdentifier
    {
        /// <summary>
        /// The theme's unique id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The theme's unique name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The theme's internal order number.
        /// </summary>
        private int _orderNumber;

        /// <summary>
        /// The theme's order number.
        /// </summary>
        public int OrderNumber
        {
            get => _orderNumber;
            set => _orderNumber = value >= 1 ? value : 0;
        }

        /// <summary>
        /// Is the theme currently active and being applied?
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Theme's font.
        /// </summary>
        private SpriteFont Font { get; }

        /// <summary>
        /// Theme's colors.
        /// </summary>
        private Dictionary<string, Color> Colors { get; }

        /// <summary>
        /// Theme's transparencies.
        /// </summary>
        public Dictionary<string, float> Transparencies { get; }

        /// <summary>
        /// A theme that is used to customize the UI.
        /// </summary>
        /// <param name="id">The theme's id. Intaken as an int.</param>
        /// <param name="name">The theme's name. Intaken as a string.</param>
        /// <param name="orderNumber">The theme's order number. Intaken as an int.</param>
        /// <param name="font">The theme's font to use. Intaken as a SpriteFont.</param>
        /// <param name="fontColor">The theme's font color to use. Intaken as a Color. Defualt is Color.Black.</param>
        /// <param name="backgroundColor">The theme's background color to use. Intaken as a Color. Defualt is Color.White.</param>
        /// <param name="highlightColor">The theme's highlight color to use. Intaken as a Color. Defualt is Color.AliceBlue.</param>
        /// <param name="outlineColor">The theme's outline color to use. Intaken as a Color. Defualt is Color.Black.</param>
        /// <param name="fontHighlightColor">The theme's font highlight color to use. Intaken as a Color. Defualt is Color.LightGray.</param>
        /// <param name="selectionColor">The theme's selection color to use. Intaken as a Color. Defualt is Color.CornflowerBlue.</param>
        /// <param name="fontTransparencyLevel">The theme's font transparency level. Intaken as a float.</param>
        /// <param name="backgroundTransparencyLevel">The theme's background transparency level. Intaken as a float.</param>
        /// <param name="highlightTransparencyLevel">The theme's highlight transparency level. Intaken as a float.</param>
        /// <param name="outlineTransparencyLevel">The theme's outline transparency level. Intaken as a float.</param>
        /// <param name="fontHighlightTransparencyLevel">The theme's font highlight transparency level. Intaken as a float.</param>
        /// <param name="selectionTransparencyLevel">The theme's selection transparency level. Intaken as a float.</param>
        public UITheme(int id, string name, int orderNumber, SpriteFont font, Color? fontColor,
                                                                              Color? backgroundColor,
                                                                              Color? highlightColor,
                                                                              Color? outlineColor,
                                                                              Color? fontHighlightColor,
                                                                              Color? selectionColor,
                                                                              float fontTransparencyLevel = 1f,
                                                                              float backgroundTransparencyLevel = 1f,
                                                                              float highlightTransparencyLevel = 0.75f,
                                                                              float outlineTransparencyLevel = 1f,
                                                                              float fontHighlightTransparencyLevel = 0.75f,
                                                                              float selectionTransparencyLevel = 0.75f)
        {
            Id = id;
            Name = name;
            Font = font;
            OrderNumber = orderNumber;

            Colors = new Dictionary<string, Color>(6)
            {
                { "Font", fontColor ?? Color.Black },
                { "Background", backgroundColor ?? Color.White },
                { "Highlight", highlightColor ?? Color.AliceBlue },
                { "Outline", outlineColor ?? Color.Black },
                { "FontHighlight", fontHighlightColor ?? Color.LightGray },
                { "Selection", selectionColor ?? Color.CornflowerBlue }
            };

            Transparencies = new Dictionary<string, float>(6)
            {
                { "Font", fontTransparencyLevel },
                { "Background", backgroundTransparencyLevel },
                { "Highlight", highlightTransparencyLevel },
                { "Outline", outlineTransparencyLevel },
                { "FontHighlight", fontHighlightTransparencyLevel },
                { "Selection", selectionTransparencyLevel }
            };
        }

        /// <summary>
        /// Applies the theme.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        /// <returns>Returns a boolean indicating whether the theme was applied.</returns>
        public bool Apply<T>(T uiBase) where T: UIBase
        {
            ApplyBackgroundColor(uiBase);
            ApplyBackgroundTransparency(uiBase);
            ApplyHighlightColor(uiBase);
            ApplyHighlightTransparency(uiBase);
            ApplyOutlineColor(uiBase);
            ApplyOutlineTransparency(uiBase);
            ApplySelectionColor(uiBase);
            ApplySelectionTransparency(uiBase);

            ApplyFont(uiBase);
            ApplyFontColor(uiBase);
            ApplyFontTransparency(uiBase);
            ApplyFontHighlightColor(uiBase);
            ApplyFontHighlightTransparency(uiBase);

            return IsActive = true;
        }

        /// <summary>
        /// Applies the theme's font.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyFont<T>(T uiBase) where T : UIBase
        {
            uiBase.Font = Font;
        }

        #region Colors

        /// <summary>
        /// Applies the theme's font color.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyFontColor<T>(T uiBase) where T : UIBase
        {
            uiBase.Colors["Font"] = Colors["Font"];
        }

        /// <summary>
        /// Applies the theme's background color.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyBackgroundColor<T>(T uiBase) where T : UIBase
        {
            uiBase.Colors["Background"] = Colors["Background"];
        }

        /// <summary>
        /// Applies the theme's highlight color.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyHighlightColor<T>(T uiBase) where T : UIBase
        {
            uiBase.Colors["Highlight"] = Colors["Highlight"];
        }

        /// <summary>
        /// Applies the theme's outline color.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyOutlineColor<T>(T uiBase) where T : UIBase
        {
            uiBase.Colors["Outline"] = Colors["Outline"];
        }

        /// <summary>
        /// Applies the theme's font highlight color.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyFontHighlightColor<T>(T uiBase) where T : UIBase
        {
            uiBase.Colors["FontHighlight"] = Colors["FontHighlight"];
        }

        /// <summary>
        /// Applies the theme's selection color.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplySelectionColor<T>(T uiBase) where T : UIBase
        {
            uiBase.Colors["Selection"] = Colors["Selection"];
        }

        #endregion

        #region Transparencies

        /// <summary>
        /// Applies the theme's font transparency.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyFontTransparency<T>(T uiBase) where T : UIBase
        {
            uiBase.Transparencies["Font"] = Transparencies["Font"];
        }

        /// <summary>
        /// Applies the theme's background transparency.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyBackgroundTransparency<T>(T uiBase) where T : UIBase
        {
            uiBase.Transparencies["Background"] = Transparencies["Background"];
        }

        /// <summary>
        /// Applies the theme's highlight transparency.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyHighlightTransparency<T>(T uiBase) where T : UIBase
        {
            uiBase.Transparencies["Highlight"] = Transparencies["Highlight"];
        }

        /// <summary>
        /// Applies the theme's outline transparency.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyOutlineTransparency<T>(T uiBase) where T : UIBase
        {
            uiBase.Transparencies["Outline"] = Transparencies["Outline"];
        }

        /// <summary>
        /// Applies the theme's font highlight transparency.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplyFontHighlightTransparency<T>(T uiBase) where T : UIBase
        {
            uiBase.Transparencies["FontHighlight"] = Transparencies["FontHighlight"];
        }

        /// <summary>
        /// Applies the theme's selection transparency.
        /// </summary>
        /// <typeparam name="T">Type UIBase.</typeparam>
        /// <param name="uiBase">An object of Type UIBase.</param>
        public void ApplySelectionTransparency<T>(T uiBase) where T : UIBase
        {
            uiBase.Transparencies["Selection"] = Transparencies["Selection"];
        }

        #endregion
    }
}