﻿namespace Softfire.MonoGame.CORE.V2.Common
{
    /// <summary>
    /// An interface for the identification of objects.
    /// </summary>
    public interface IMonoGameIdentifierComponent
    {
        /// <summary>
        /// A unique id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// A unique name.
        /// </summary>
        string Name { get; }
    }
}
