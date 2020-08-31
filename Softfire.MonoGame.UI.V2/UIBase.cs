using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE.V2;
using Softfire.MonoGame.CORE.V2.Common;
using Softfire.MonoGame.UI.V2.Effects;
using Softfire.MonoGame.UI.V2.Items;

namespace Softfire.MonoGame.UI.V2
{
    /// <summary>
    /// The base UI class.
    /// The root of all UI.
    /// </summary>
    public abstract class UIBase : MonoGameObject
    {
        /// <summary>
        /// The element's graphics device.
        /// </summary>
        internal static GraphicsDevice GraphicsDevice { get; set; }

        /// <summary>
        /// The containing group.
        /// </summary>
        protected UIGroup Group { get; set; }
        
        /// <summary>
        /// The element's effects manager.
        /// </summary>
        public UIEffectsManager EffectsManager { get; } = new UIEffectsManager();

        /// <summary>
        /// The element's textures.
        /// </summary>
        protected Dictionary<string, Texture2D> Textures { get; } = new Dictionary<string, Texture2D>();

        /// <summary>
        /// The element's colors.
        /// </summary>
        public Dictionary<string, Color> Colors { get; } = new Dictionary<string, Color>(6)
        {
            { "Background", Color.White },
            { "Highlight", Color.CornflowerBlue },
            { "Outline", Color.Black },
            { "Font", Color.Black },
            { "FontHighlight", Color.Teal },
            { "Selection", Color.Teal }
        };

        /// <summary>
        /// The element's transparency levels.
        /// </summary>
        private List<UITransparency> Transparencies { get; } = new List<UITransparency>(6)
        {
            new UITransparency(1, "Background", 1f),
            new UITransparency(2, "Highlight", 0.25f),
            new UITransparency(3, "Outline", 1f),
            new UITransparency(4, "Font", 1f ),
            new UITransparency(5, "FontHighlight", 0.75f),
            new UITransparency(6, "Selection", 0.75f)
        };

        /// <summary>
        /// The element's outlines.
        /// </summary>
        protected internal List<UIOutline> Outlines { get; }

        /// <summary>
        /// The element's padding.
        /// </summary>
        public UIPadding Paddings { get; } = new UIPadding();
        
        /// <summary>
        /// The element's sprite font.
        /// </summary>
        public SpriteFont Font { get; set; }
        
        /// <summary>
        /// Determines whether the background is visible.
        /// </summary>
        public bool IsBackgroundVisible { get; set; } = true;

        /// <summary>
        /// Everything a UI element should have in it's base.
        /// </summary>
        /// <param name="parent">The parent object. Intaken as a <see cref="MonoGameObject"/>.</param>
        /// <param name="id">The base's id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The base's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The base's position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="width">The base's width. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The base's height. Intaken as a <see cref="float"/>.</param>
        /// <param name="isVisible">The base's visibility. Intaken as a <see cref="bool"/>.</param>
        protected UIBase(MonoGameObject parent, int id, string name, Vector2 position, int width, int height, bool isVisible = true) : base(parent, id, name, position, width, height, isVisible)
        {
            Group = Parent is UIWindow window ? window.Group : null;

            IsHighlightable = true;

            // Outlines
            Outlines = new List<UIOutline>(4)
            {
                new UIOutline(this, 1, "Top", UIOutline.Sides.Top, 1, Colors["Outline"], GetTransparency("Outline").Level),
                new UIOutline(this, 2, "Right", UIOutline.Sides.Right, 1, Colors["Outline"], GetTransparency("Outline").Level),
                new UIOutline(this, 3, "Bottom", UIOutline.Sides.Bottom, 1, Colors["Outline"], GetTransparency("Outline").Level),
                new UIOutline(this, 4, "Left", UIOutline.Sides.Left, 1, Colors["Outline"], GetTransparency("Outline").Level)
            };
        }

        #region Transparencies
        
        /// <summary>
        /// Gets a transparency, by id.
        /// </summary>
        /// <param name="transparencyId">The id of the transparency to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the transparency with the specified id, if present, otherwise null.</returns>
        public UITransparency GetTransparency(int transparencyId) => Identities.GetObject<UITransparency, UITransparency>(Transparencies, transparencyId);

        /// <summary>
        /// Gets a transparency, by name.
        /// </summary>
        /// <param name="transparencyName">The name of the transparency to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns the transparency with the specified name, if present, otherwise null.</returns>
        public UITransparency GetTransparency(string transparencyName) => Identities.GetObject<UITransparency, UITransparency>(Transparencies, transparencyName);

        #endregion

        #region Outlines
        
        /// <summary>
        /// Gets an outline, by id.
        /// </summary>
        /// <param name="outlineId">The id of the outline to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the outline with the specified id, if present, otherwise null.</returns>
        public UIOutline GetOutline(int outlineId) => Identities.GetObject<UIOutline, UIOutline>(Outlines, outlineId);

        /// <summary>
        /// Gets an outline, by name.
        /// </summary>
        /// <param name="outlineName">The name of the outline to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns the outline with the specified name, if present, otherwise null.</returns>
        public UIOutline GetOutline(string outlineName) => Identities.GetObject<UIOutline, UIOutline>(Outlines, outlineName);

        /// <summary>
        /// Sets visibility for each outline to that of the base.
        /// </summary>
        public void SetOutlineVisibility()
        {
            foreach (var outline in Outlines)
            {
                outline.IsVisible = IsVisible;
            }
        }

        /// <summary>
        /// Sets visibility for each outline.
        /// </summary>
        public void SetOutlineVisibility(bool visibility)
        {
            foreach (var outline in Outlines)
            {
                outline.IsVisible = visibility;
            }
        }

        /// <summary>
        /// Sets individual outline visibility.
        /// </summary>
        /// <param name="top">A boolean indicating whether the top outline is visible.</param>
        /// <param name="right">A boolean indicating whether the right outline is visible.</param>
        /// <param name="bottom">A boolean indicating whether the bottom outline is visible.</param>
        /// <param name="left">A boolean indicating whether the left outline is visible.</param>
        public void SetOutlineVisibility(bool top, bool right, bool bottom, bool left)
        {
            Outlines[1].IsVisible = top;
            Outlines[2].IsVisible = right;
            Outlines[3].IsVisible = bottom;
            Outlines[4].IsVisible = left;
        }

        /// <summary>
        /// Sets all outline's color to that in Colors["Outline"].
        /// </summary>
        public void SetOutlineColor()
        {
            foreach (var outline in Outlines)
            {
                outline.Color = Colors["Outline"];
            }
        }

        /// <summary>
        /// Sets Colors["Outline"] to the provided color and applies the color to all outlines.
        /// </summary>
        /// <param name="color">The color to set in Colors["Outline"]. Intaken as a Color.</param>
        public void SetOutlineColor(Color color)
        {
            Colors["Outline"] = color;
            SetOutlineColor();
        }

        /// <summary>
        /// Sets individual outline color.
        /// </summary>
        /// <param name="top">The color for the top outline. Intaken as a Color.</param>
        /// <param name="right">The color for the right outline. Intaken as a Color.</param>
        /// <param name="bottom">The color for the bottom outline. Intaken as a Color.</param>
        /// <param name="left">The color for the left outline. Intaken as a Color.</param>
        public void SetOutlineColor(Color top, Color right, Color bottom, Color left)
        {
            Outlines[1].Color = top;
            Outlines[2].Color = right;
            Outlines[3].Color = bottom;
            Outlines[4].Color = left;
        }

        /// <summary>
        /// Sets all the outlines transparency level to the element's transparency level.
        /// </summary>
        public void SetOutlineTransparency()
        {
            foreach (var outline in Outlines)
            {
                outline.Transparency = GetTransparency("Outline").Level;
            }
        }

        /// <summary>
        /// Sets the element's transparency level to all outlines.
        /// </summary>
        /// <param name="transparencyLevel">The transparency level to set. Intaken as a <see cref="float"/>.</param>
        public void SetOutlineTransparency(float transparencyLevel)
        {
            GetTransparency("Outline").Level = transparencyLevel;
            SetOutlineTransparency();
        }

        /// <summary>
        /// Sets individual outline transparencies.
        /// </summary>
        /// <param name="top">The transparency level for the top outline. Intaken as a <see cref="float"/>.</param>
        /// <param name="right">The transparency level for the right outline. Intaken as a <see cref="float"/>.</param>
        /// <param name="bottom">The transparency level for the bottom outline. Intaken as a <see cref="float"/>.</param>
        /// <param name="left">The transparency level for the left outline. Intaken as a <see cref="float"/>.</param>
        public void SetOutlineTransparency(float top, float right, float bottom, float left)
        {
            Outlines[1].Transparency = top;
            Outlines[2].Transparency = right;
            Outlines[3].Transparency = bottom;
            Outlines[4].Transparency = left;
        }

        #endregion

        /// <summary>
        /// Calculates the element's rectangle.
        /// </summary>
        /// <returns>Returns the calculated rectangle.</returns>
        protected override RectangleF CalculateRectangle()
        {
            return new RectangleF(Transform.WorldPosition().X - Origin.X - Paddings.Left,
                                  Transform.WorldPosition().Y - Origin.Y - Paddings.Top,
                                  (Size.Width * Transform.Scale.X) + Paddings.Left + Paddings.Right,
                                  (Size.Height * Transform.Scale.Y) + Paddings.Top + Paddings.Bottom);
        }

        /// <summary>
        /// Calculates an <see cref="RectangleF"/> that includes the element's outlines.
        /// </summary>
        /// <returns>Returns an rectangle that includes the element's outlines as a <see cref="RectangleF"/>.</returns>
        protected override RectangleF CalculateExtendedRectangle()
        {
            // Rectangle
            var rectangle = Rectangle;

            // Outlines
            var outlineTop = GetOutline(1);
            var outlineRight = GetOutline(2);
            var outlineBottom = GetOutline(3);
            var outlineLeft = GetOutline(4);
            
            // Top Outline
            if (outlineTop.IsVisible)
            {
                rectangle.Y -= outlineTop.Thickness;
                rectangle.Height += outlineTop.Thickness;
            }

            // Right Outline
            if (outlineRight.IsVisible)
            {
                rectangle.Width += outlineRight.Thickness;
            }

            // Bottom Outline
            if (outlineBottom.IsVisible)
            {
                rectangle.Height += outlineBottom.Thickness;
            }

            // Left Outline
            if (outlineLeft.IsVisible)
            {
                rectangle.X -= outlineLeft.Thickness;
                rectangle.Width += outlineLeft.Thickness;
            }

            return rectangle;
        }

        /// <summary>
        /// Creates a new 2D texture.
        /// </summary>
        protected internal Texture2D CreateTexture2D()
        {
            var texture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData(new[] { Color.White });

            return texture;
        }
        
        /// <summary>
        /// The element's content loader.
        /// </summary>
        public override void LoadContent(ContentManager content = null)
        {
            Textures.Add("Background", CreateTexture2D());
            Textures.Add("Highlight", CreateTexture2D());

            foreach (var outline in Outlines)
            {
                outline.LoadContent();
            }

            base.LoadContent(content);
        }

        /// <summary>
        /// The element's update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame <see cref="GameTime"/>.</param>
        public override void Update(GameTime gameTime)
        {
            if (IsActive && IsVisible)
            {
                // Run any pending affects.
                Task.Run(() => EffectsManager.RunActiveEffects());
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// The element's draw method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform = default)
        {
            if (IsVisible)
            {
                if (IsBackgroundVisible)
                {
                    // Apply any transformations.
                    var position = Vector2.Transform(new Vector2(Rectangle.X, Rectangle.Y), transform);

                    // Draw Base.
                    spriteBatch.Draw(Textures["Background"], position, null,
                                     Colors["Background"] * GetTransparency("Background").Level, Transform.Rotation, Vector2.Zero,
                                     new Vector2(Rectangle.Width, Rectangle.Height), SpriteEffects.None, 1);
                }

                // Draw outlines.
                foreach (var outline in Outlines)
                {
                    outline.Draw(spriteBatch, transform);
                }

                // If Highlightable and has focus.
                if (IsHighlightable &&
                    IsStateSet(FocusStates.IsHovered))
                {
                    // Apply any transformations.
                    var position = Vector2.Transform(new Vector2(Rectangle.X, Rectangle.Y), transform);

                    // Highlight.
                    spriteBatch.Draw(Textures["Highlight"], position, null,
                                     Colors["Highlight"] * GetTransparency("Highlight").Level, Transform.WorldRotation(), Vector2.Zero,
                                     new Vector2(Rectangle.Width, Rectangle.Height), SpriteEffects.None, 1);
                }

                // Center dots
                spriteBatch.Draw(Textures["Background"], Transform.WorldPosition() - Vector2.One, null,
                                 Colors["Outline"] * GetTransparency("Outline").Level, Transform.Rotation, Vector2.Zero,
                                 new Vector2(2, 2), SpriteEffects.None, 1);
            }
        }
    }
}