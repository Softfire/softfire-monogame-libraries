using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.CORE.Common
{
    /// <summary>
    /// An interface for the drawing of objects.
    /// </summary>
    public interface IMonoGameDrawComponent
    {
        /// <summary>
        /// The object's draw method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a MonoGame <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        void Draw(SpriteBatch spriteBatch, Matrix transform);
    }
}
