﻿namespace Softfire.MonoGame.CORE.V2.Input
{
    /// <summary>
    /// An interface for providing tabbing order to an object.
    /// </summary>
    public interface IMonoGameInputTabComponent
    {
        /// <summary>
        /// The tab order index for the object.
        /// </summary>
        int TabOrder { get; }
    }
}