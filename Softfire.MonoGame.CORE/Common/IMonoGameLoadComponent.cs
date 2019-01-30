using Microsoft.Xna.Framework.Content;

namespace Softfire.MonoGame.CORE.Common
{
    /// <summary>
    /// An interface for the loading of an object's assets.
    /// </summary>
    public interface IMonoGameLoadComponent
    {
        /// <summary>
        /// The object's load content method.
        /// </summary>
        void LoadContent(ContentManager content = null);
    }
}