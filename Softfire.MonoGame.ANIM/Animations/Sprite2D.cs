using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.ANIM.Animations
{
    /// <summary>
    /// A 2D sprite template.
    /// </summary>
    public abstract class Sprite2D : Animation
    {
        /// <summary>
        /// A 2D sprite animation.
        /// </summary>
        /// <param name="parent">The parent <see cref="Animation"/> with it's layer. Intaken as a (Layer, AnimationBase)</param>
        /// <param name="id">The sprite's unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The sprite's unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="texturePath">The sprite's texture path. Relative to Content's folder location. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The sprite's initial position. Intaken as a <see cref="Vector2"/>.</param>
        protected Sprite2D(Animation parent, int id, string name, string texturePath, Vector2 position) : base(parent, id, name, texturePath, position)
        {
        }
    }
}