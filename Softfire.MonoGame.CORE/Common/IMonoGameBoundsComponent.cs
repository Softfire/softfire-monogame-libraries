using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE.Common
{
    /// <summary>
    /// An interface for the dimensions of 2D objects.
    /// </summary>
    public interface IMonoGameBoundsComponent
    {
        /// <summary>
        /// The origin or center of the object.
        /// </summary>
        Vector2 Origin { get; }

        /// <summary>
        /// The rectangular boundary of an object.
        /// </summary>
        RectangleF Rectangle { get; }

        /// <summary>
        /// The extended rectangular boundary of an object.
        /// </summary>
        RectangleF ExtendedRectangle { get; }
    }
}