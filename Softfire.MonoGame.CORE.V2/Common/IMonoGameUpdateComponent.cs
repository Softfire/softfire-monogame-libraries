using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE.V2.Common
{
    /// <summary>
    /// An interface for the updating of objects.
    /// </summary>
    public interface IMonoGameUpdateComponent
    {
        /// <summary>
        /// The object's update method.
        /// </summary>
        /// <param name="gameTime">Intakes a MonoGame <see cref="GameTime"/>.</param>
        void Update(GameTime gameTime);
    }
}
