using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE.V2.Common;

namespace Softfire.MonoGame.UI.V2.Items
{
    /// <summary>
    /// The outline class used to draw around UI elements.
    /// </summary>
    public class UIOutline : IMonoGameIdentifierComponent
    {
        /// <summary>
        /// The outline's parent object.
        /// </summary>
        private UIBase Parent { get; }

        /// <summary>
        /// The outline's id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The outline's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Determines whether the outline is visible.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// The outline's internal thickness value.
        /// </summary>
        private int _thickness;

        /// <summary>
        /// The outline's thickness.
        /// </summary>
        public int Thickness
        {
            get => _thickness;
            set => _thickness = value > 0 ? value : 1;
        }

        /// <summary>
        /// The outline's color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The outline's transparency.
        /// </summary>
        public float Transparency { get; set; }

        /// <summary>
        /// The outline's draw texture.
        /// </summary>
        private Texture2D Texture { get; set; }

        /// <summary>
        /// The outline's available sides.
        /// </summary>
        public enum Sides
        {
            /// <summary>
            /// The top outline.
            /// </summary>
            Top,
            /// <summary>
            /// The right outline.
            /// </summary>
            Right,
            /// <summary>
            /// The bottom outline.
            /// </summary>
            Bottom,
            /// <summary>
            /// The left outline.
            /// </summary>
            Left
        }

        /// <summary>
        /// The outline's defined side.
        /// </summary>
        public Sides Side { get; }

        /// <summary>
        /// An outline for UI elements.
        /// </summary>
        /// <param name="parent">The parent object. Intaken as a derived type of <see cref="UIBase"/>.</param>
        /// <param name="id">The outline's id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The outline's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="side">The outline's location relative to the base. Intaken as a <see cref="Sides"/>.</param>
        /// <param name="thickness">The outline's thickness. Intaken as an <see cref="int"/>.</param>
        /// <param name="color">The outline's color. Intaken as a <see cref="Color"/>.</param>
        /// <param name="transparency">The outline's transparency level. Intaken as a <see cref="float"/>.</param>
        public UIOutline(UIBase parent, int id, string name, Sides side, int thickness = 1, Color? color = null, float transparency = 1.0f)
        {
            Parent = parent;
            Id = id;
            Name = name;
            Side = side;
            Thickness = thickness;
            Color = color ?? Color.Black;
            Transparency = transparency;
        }

        /// <summary>
        /// Loads the outline's content.
        /// </summary>
        public void LoadContent()
        {
            Texture = Parent.CreateTexture2D();
        }

        /// <summary>
        /// The outline's draw method.
        /// </summary>
        /// <param name="spriteBatch">A MonoGame <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public void Draw(SpriteBatch spriteBatch, Matrix transform)
        {
            if (IsVisible)
            {
                // Apply any transformations.
                var position = Vector2.Transform(new Vector2(Parent.Rectangle.X, Parent.Rectangle.Y), transform);

                switch (Side)
                {
                    case Sides.Top:
                        spriteBatch.Draw(Texture, new Vector2(position.X - Thickness, position.Y - Thickness), null,
                                         Color * Transparency, Parent.Transform.WorldRotation(), Vector2.Zero, new Vector2(Parent.Rectangle.Width + Thickness * 2, Thickness), SpriteEffects.None, 1);
                        break;
                    case Sides.Right:
                        spriteBatch.Draw(Texture, new Vector2(position.X + Parent.Rectangle.Width, position.Y - Thickness), null,
                                         Color * Transparency, Parent.Transform.WorldRotation(), Vector2.Zero, new Vector2(Thickness, Parent.Rectangle.Height + Thickness * 2), SpriteEffects.None, 1);
                        break;
                    case Sides.Bottom:
                        spriteBatch.Draw(Texture, new Vector2(position.X - Thickness, position.Y + Parent.Rectangle.Height), null,
                                         Color * Transparency, Parent.Transform.WorldRotation(), Vector2.Zero, new Vector2(Parent.Rectangle.Width + Thickness * 2, Thickness), SpriteEffects.None, 1);
                        break;
                    case Sides.Left:
                        spriteBatch.Draw(Texture, new Vector2(position.X - Thickness, position.Y - Thickness), null,
                                         Color * Transparency, Parent.Transform.WorldRotation(), Vector2.Zero, new Vector2(Thickness, Parent.Rectangle.Height + Thickness * 2), SpriteEffects.None, 1);
                        break;
                }
            }
        }
    }
}