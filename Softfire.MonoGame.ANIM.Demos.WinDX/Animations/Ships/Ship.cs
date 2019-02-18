using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.ANIM.Demos.WinDX.Animations.Ships
{
    /// <summary>
    /// A space ship class for use in the <see cref="Animation"/> demo.
    /// </summary>
    public class Ship : Animation
    {
        /// <summary>
        /// A 2D ship animation.
        /// </summary>
        /// <param name="parent">The parent <see cref="Animation"/>.</param>
        /// <param name="id">The <see cref="Ship"/>'s unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The <see cref="Ship"/>'s unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="texturePath">The <see cref="Ship"/>'s texture path. Relative to the Content's folder location. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The <see cref="Ship"/>'s initial position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="width">The <see cref="Ship"/>'s width. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The <see cref="Ship"/>'s height. Intaken as a <see cref="float"/>.</param>
        /// <param name="isVisible">The <see cref="Ship"/>'s visibility. Intaken as a <see cref="bool"/>.</param>
        public Ship(Animation parent, int id, string name, string texturePath, Vector2 position, int width, int height, bool isVisible = true) : base(parent, id, name, texturePath, position, width, height, isVisible)
        {

        }

        /// <summary>
        /// The <see cref="Ship"/>'s update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame's <see cref="GameTime"/>.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// The <see cref="Ship"/>'s draw method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a MonoGame <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform = default)
        {
            base.Draw(spriteBatch, transform);
        }
    }
}