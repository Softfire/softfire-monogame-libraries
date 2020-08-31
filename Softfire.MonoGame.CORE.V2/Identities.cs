using System;
using System.Collections.Generic;
using System.Linq;
using Softfire.MonoGame.CORE.V2.Common;

namespace Softfire.MonoGame.CORE.V2
{
    /// <summary>
    /// A class of static methods for identifying objects.
    /// </summary>
    public static class Identities
    {
        #region Identity

        /// <summary>
        /// Retrieves the next valid id for an object of type TDerivative from the provided list.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to inspect to produce a valid id. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <returns>Returns a valid id for an object of type TDerivative as an <see cref="int"/>.</returns>
        public static int GetNextValidObjectId<TBase, TDerivative>(IList<TBase> list) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            var nextId = 1;
            while (list.Any(obj => obj is TDerivative && obj.Id == nextId))
            {
                nextId++;
            }

            return nextId;
        }

        /// <summary>
        /// Determines whether the object exists in the list of type TDerivative, by id.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IEnumerable{TBase}"/>.</param>
        /// <param name="id">The id of the object to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object exists in the list matching the provided id.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public static bool ObjectExists<TBase, TDerivative>(IEnumerable<TBase> list, int id) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            return list?.Any(obj => obj is TDerivative && obj.Id == id) ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Determines whether the object exists in the list of type TDerivative, by name.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IEnumerable{TBase}"/>.</param>
        /// <param name="name">The name of the object to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object exists in the list matching the provided name.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public static bool ObjectExists<TBase, TDerivative>(IEnumerable<TBase> list, string name) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            return list?.Any(obj => obj is TDerivative && obj.Name == name) ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Retrieves an object of type TDerivative, by id.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="id">The id of the object to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns an object of Type TDerivative, if present, otherwise null.</returns>
        public static TDerivative GetObject<TBase, TDerivative>(IList<TBase> list, int id) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            return ObjectExists<TBase, TDerivative>(list, id) ? list.FirstOrDefault(item => item.Id == id) as TDerivative : default;
        }

        /// <summary>
        /// Retrieves an object of type TDerivative, by name.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="name">The name of the object to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns an object of Type TDerivative, if present, otherwise null.</returns>
        public static TDerivative GetObject<TBase, TDerivative>(IList<TBase> list, string name) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            return ObjectExists<TBase, TDerivative>(list, name) ? list.FirstOrDefault(item => item.Name == name) as TDerivative : default;
        }

        /// <summary>
        /// Removes an object of type TDerivative, by id.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="id">The id of the object to remove. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object was removed.</returns>
        public static bool RemoveObject<TBase, TDerivative>(IList<TBase> list, int id) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            return list.Remove(GetObject<TBase, TDerivative>(list, id));
        }

        /// <summary>
        /// Removes an object of type TDerivative, by name.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="name">The name of the object to remove. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object was removed.</returns>
        public static bool RemoveObject<TBase, TDerivative>(IList<TBase> list, string name) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            return list.Remove(GetObject<TBase, TDerivative>(list, name));
        }

        /// <summary> 
        /// Reorders an object within the list, by id.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="id">The id of the object to move. Intaken as an <see cref="int"/>.</param>
        /// <param name="otherId">The id of the object to move above. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ReorderObject<TBase, TDerivative>(IList<TBase> list, int id, int otherId) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            // Temporarily store the object.
            var item = GetObject<TBase, TDerivative>(list, id);

            var otherItem = GetObject<TBase, TDerivative>(list, otherId);
            var otherItemIndex = list.IndexOf(otherItem);

            // Remove current object.
            RemoveObject<TBase, TDerivative>(list, id);

            // Update layer.
            list.Insert(otherItemIndex, item);
        }

        /// <summary> 
        /// Reorders an object within the list, by name.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="name">The name of the object to move. Intaken as a <see cref="string"/>.</param>
        /// <param name="otherId">The id of the object to move above. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ReorderObject<TBase, TDerivative>(IList<TBase> list, string name, int otherId) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            // Temporarily store the object.
            var item = GetObject<TBase, TDerivative>(list, name);

            var otherItem = GetObject<TBase, TDerivative>(list, otherId);
            var otherItemIndex = list.IndexOf(otherItem);

            // Remove current object.
            RemoveObject<TBase, TDerivative>(list, name);

            // Update layer.
            list.Insert(otherItemIndex, item);
        }

        /// <summary> 
        /// Reorders an object within the list, by id.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="id">The id of the object to move. Intaken as an <see cref="int"/>.</param>
        /// <param name="otherName">The name of the object to move above. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ReorderObject<TBase, TDerivative>(IList<TBase> list, int id, string otherName) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            // Temporarily store the object.
            var item = GetObject<TBase, TDerivative>(list, id);

            var otherItem = GetObject<TBase, TDerivative>(list, otherName);
            var otherItemIndex = list.IndexOf(otherItem);

            // Remove current object.
            RemoveObject<TBase, TDerivative>(list, id);

            // Update layer.
            list.Insert(otherItemIndex, item);
        }

        /// <summary> 
        /// Reorders an object within the list, by name.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="name">The name of the object to move. Intaken as a <see cref="string"/>.</param>
        /// <param name="otherName">The name of the object to move above. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ReorderObject<TBase, TDerivative>(IList<TBase> list, string name, string otherName) where TBase : IMonoGameIdentifierComponent where TDerivative : class, TBase
        {
            // Temporarily store the object.
            var item = GetObject<TBase, TDerivative>(list, name);

            var otherItem = GetObject<TBase, TDerivative>(list, otherName);
            var otherItemIndex = list.IndexOf(otherItem);

            // Remove current object.
            RemoveObject<TBase, TDerivative>(list, name);

            // Update layer.
            list.Insert(otherItemIndex, item);
        }

        #endregion

        #region Identity + Layer

        /// <summary>
        /// Retrieves the next valid id for an object of type TDerivative from the provided list on the provided layer.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to inspect to produce a valid id. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="layer">The layer to produce a valid id on. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a valid id for an object of type TDerivative as an <see cref="int"/>.</returns>
        public static int GetNextValidObjectId<TBase, TDerivative>(IList<TBase> list, int layer) where TBase : IMonoGameIdentifierComponent, IMonoGameLayerComponent where TDerivative : TBase
        {
            var nextId = 1;
            while (list.Any(obj => obj.Layer == layer && obj is TDerivative && obj.Id == nextId))
            {
                nextId++;
            }

            return nextId;
        }

        /// <summary>
        /// Determines whether the object exists in the list of type TDerivative on the provided layer, by id.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="id">The id of the object to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object exists.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public static bool ObjectExists<TBase, TDerivative>(IList<TBase> list, int layer, int id) where TBase : IMonoGameIdentifierComponent, IMonoGameLayerComponent where TDerivative : TBase
        {
            return list?.Any(obj => obj.Layer == layer && obj is TDerivative && obj.Id == id) ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Determines whether the object exists in the list of type TDerivative on the provided layer, by name.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The name of the object to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object exists.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public static bool ObjectExists<TBase, TDerivative>(IList<TBase> list, int layer, string name) where TBase : IMonoGameIdentifierComponent, IMonoGameLayerComponent where TDerivative : TBase
        {
            return list?.Any(obj => obj.Layer == layer && obj is TDerivative && obj.Name == name) ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Retrieves an object of type TDerivative on the provided layer, by id.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="id">The id of the object to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns an object of Type TDerivative, if present, otherwise null.</returns>
        public static TDerivative GetObject<TBase, TDerivative>(IList<TBase> list, int layer, int id) where TBase : IMonoGameIdentifierComponent, IMonoGameLayerComponent where TDerivative : TBase
        {
            return ObjectExists<TBase, TDerivative>(list, layer, id) ? (TDerivative)list.FirstOrDefault(item => item.Layer == layer && item.Id == id) : default;
        }

        /// <summary>
        /// Retrieves an object of type TDerivative on the provided layer, by name.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The name of the object to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns an object of Type TDerivative, if present, otherwise null.</returns>
        public static TDerivative GetObject<TBase, TDerivative>(IList<TBase> list, int layer, string name) where TBase : IMonoGameIdentifierComponent, IMonoGameLayerComponent where TDerivative : TBase
        {
            return ObjectExists<TBase, TDerivative>(list, layer, name) ? (TDerivative)list.FirstOrDefault(item => item.Layer == layer && item.Name == name) : default;
        }

        /// <summary>
        /// Removes an object of type TDerivative on the provided layer, by id.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="id">The id of the object to remove. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object was removed.</returns>
        public static bool RemoveObject<TBase, TDerivative>(IList<TBase> list, int layer, int id) where TBase : IMonoGameIdentifierComponent, IMonoGameLayerComponent where TDerivative : TBase
        {
            return list.Remove(GetObject<TBase, TDerivative>(list, layer, id));
        }

        /// <summary>
        /// Removes an object of type TDerivative on the provided layer, by name.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The name of the object to remove. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object was removed.</returns>
        public static bool RemoveObject<TBase, TDerivative>(IList<TBase> list, int layer, string name) where TBase : IMonoGameIdentifierComponent, IMonoGameLayerComponent where TDerivative : TBase
        {
            return list.Remove(GetObject<TBase, TDerivative>(list, layer, name));
        }

        /// <summary> 
        /// Modifies an object's layer, by id.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="id">The id of the object to change it's layer. Intaken as an <see cref="int"/>.</param>
        /// <param name="newLayer">The layer to place the object on. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ChangeObjectLayer<TBase, TDerivative>(IList<TBase> list, int layer, int id, int newLayer) where TBase : IMonoGameIdentifierComponent, IMonoGameLayerComponent where TDerivative : TBase
        {
            // Temporarily store the object.
            var item = GetObject<TBase, TDerivative>(list, layer, id);

            // Remove current object.
            RemoveObject<TBase, TDerivative>(list, layer, id);

            // Update layer.
            item.Layer = newLayer;

            // Add the object back to the list with it's new layer.
            list.Add(item);
        }

        /// <summary> 
        /// Modifies an object's layer, by name.
        /// </summary>
        /// <typeparam name="TBase">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="TDerivative">An object derived from type TBase.</typeparam>
        /// <param name="list">The list to check against for an object of type TDerivative. Intaken as a <see cref="IList{TBase}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The name of the object to change it's layer. Intaken as a <see cref="string"/>.</param>
        /// <param name="newLayer">The layer to place the object on. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ChangeObjectLayer<TBase, TDerivative>(IList<TBase> list, int layer, string name, int newLayer) where TBase : IMonoGameIdentifierComponent, IMonoGameLayerComponent where TDerivative : TBase
        {
            // Temporarily store the object.
            var item = GetObject<TBase, TDerivative>(list, layer, name);

            // Remove current object.
            RemoveObject<TBase, TDerivative>(list, layer, name);

            // Update layer.
            item.Layer = newLayer;

            // Add the object back to the list with it's new layer.
            list.Add(item);
        }

        #endregion
    }
}