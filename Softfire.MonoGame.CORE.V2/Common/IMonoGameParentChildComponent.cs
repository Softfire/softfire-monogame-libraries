using System.Collections.Generic;

namespace Softfire.MonoGame.CORE.V2.Common
{
    /// <summary>
    /// An interface for defining parent-child relationships.
    /// </summary>
    public interface IMonoGameParentChildComponent
    {
        /// <summary>
        /// The object's parent object.
        /// </summary>
        MonoGameObject Parent { get; }

        /// <summary>
        /// The object's child objects.
        /// </summary>
        IList<MonoGameObject> Children { get; }
    }
}