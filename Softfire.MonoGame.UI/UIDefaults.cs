using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// UI Defaults.
    /// Stored attributes.
    /// </summary>
    public class UIDefaults
    {
        /// <summary>
        /// Index.
        /// </summary>
        public double Index { get; set; }

        /// <summary>
        /// Order Number.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Is Active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Is Visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Rectangle.
        /// </summary>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// Scale.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Rotation Angle.
        /// </summary>
        public float RotationAngle { get; set; }

        /// <summary>
        /// Draw Depth.
        /// </summary>
        public float DrawDepth { get; set; }

        /// <summary>
        /// Texts.
        /// </summary>
        public Dictionary<int, string> Texts { get; }

        /// <summary>
        /// Font.
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Selection Font.
        /// </summary>
        public SpriteFont SelectionFont { get; set; }

        /// <summary>
        /// Highlight Font.
        /// </summary>
        public SpriteFont HighlightFont { get; set; }

        /// <summary>
        /// Background Color.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Font Color.
        /// </summary>
        public Color FontColor { get; set; }

        /// <summary>
        /// Selection Color.
        /// </summary>
        public Color SelectionColor { get; set; }

        /// <summary>
        /// Highlight Color.
        /// </summary>
        public Color HighlightColor { get; set; }

        /// <summary>
        /// Outline Color.
        /// </summary>
        public Color OutlineColor { get; set; }

        /// <summary>
        /// Background Transparency.
        /// </summary>
        public float BackgroundTransparency { get; set; }

        /// <summary>
        /// Font Transparency.
        /// </summary>
        public float FontTransparency { get; set; }

        /// <summary>
        /// Selection Transparency.
        /// </summary>
        public float SelectionTransparency { get; set; }

        /// <summary>
        /// Highlight Transparency.
        /// </summary>
        public float HighlightTransparency { get; set; }

        /// <summary>
        /// Outline Thickness.
        /// </summary>
        public int OutlineThickness { get; set; }

        /// <summary>
        /// UI Defaults Constructor.
        /// </summary>
        public UIDefaults()
        {
            Texts = new Dictionary<int, string>();
        }
    }
}