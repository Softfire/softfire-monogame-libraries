using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        /// <param name="parent">The parent <see cref="Animation"/>.</param>
        /// <param name="id">The <see cref="Sprite2D"/>'s unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The <see cref="Sprite2D"/>'s unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="texturePath">The <see cref="Sprite2D"/>'s texture path. Relative to the Content's folder location. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The <see cref="Sprite2D"/>'s initial position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="width">The <see cref="Sprite2D"/>'s width. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The <see cref="Sprite2D"/>'s height. Intaken as a <see cref="float"/>.</param>
        /// <param name="isVisible">The <see cref="Sprite2D"/>'s visibility. Intaken as a <see cref="bool"/>.</param>
        protected Sprite2D(Animation parent, int id, string name, string texturePath, Vector2 position, int width, int height, bool isVisible = true) : base(parent, id, name, texturePath, position, width, height, isVisible)
        {
        }

        /// <summary>
        /// The <see cref="Sprite2D"/>'s update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame's <see cref="GameTime"/>.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// The <see cref="Sprite2D"/>'s draw method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a MonoGame <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform = default)
        {
            base.Draw(spriteBatch, transform);
        }
    }
}