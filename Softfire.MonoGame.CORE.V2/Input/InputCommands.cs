using System.Collections.Generic;
using System.Linq;
using Softfire.MonoGame.CORE.V2.Common;

namespace Softfire.MonoGame.CORE.V2.Input
{
    /// <summary>
    /// A set of input commands.
    /// </summary>
    public static class InputCommands
    {
        /// <summary>
        /// Retrieves the next valid tab id for an object of type T2 from the provided list.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameInputTabComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to inspect to produce a valid id. Intaken as a <see cref="IList{T}"/>.</param>
        /// <returns>Returns a valid id for an object of type T2 as an <see cref="int"/>.</returns>
        public static int GetNextValidTabId<T1, T2>(IList<T1> list) where T1 : IMonoGameInputTabComponent where T2 : T1
        {
            var nextTabId = 1;
            while (list.Any(obj => obj is T2 && obj.TabOrder == nextTabId))
            {
                nextTabId++;
            }

            return nextTabId;
        }

        /// <summary>
        /// Retrieves the next valid tab id for an object of type T2 from the provided list.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to inspect to produce a valid id. Intaken as a <see cref="IList{T}"/>.</param>
        /// <param name="layer">The layer to produce a valid id on. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a valid id for an object of type T2 as an <see cref="int"/>.</returns>
        public static int GetNextValidTabId<T1, T2>(IList<T1> list, int layer) where T1 : IMonoGameInputTabComponent, IMonoGameLayerComponent where T2 : T1
        {
            var nextTabId = 1;
            while (list.Any(obj => obj.Layer == layer && obj is T2 && obj.TabOrder == nextTabId))
            {
                nextTabId++;
            }

            return nextTabId;
        }
    }
}