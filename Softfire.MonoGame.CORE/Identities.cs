using System;
using System.Collections.Generic;
using System.Linq;
using Softfire.MonoGame.CORE.Common;

namespace Softfire.MonoGame.CORE
{
    /// <summary>
    /// A class of static methods for identifying objects.
    /// </summary>
    public static class Identities
    {
        #region Identity
        
        /// <summary>
        /// Retrieves the next valid id for an object of type T2 from the provided list.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to inspect to produce a valid id. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <returns>Returns a valid id for an object of type T2 as an <see cref="int"/>.</returns>
        public static int GetNextValidObjectId<T1, T2>(IList<T1> list) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            var nextId = 1;
            while (list.Any(obj => obj is T2 && obj.Id == nextId))
            {
                nextId++;
            }

            return nextId;
        }

        /// <summary>
        /// Determines whether the object exists in the list of type T2, by id.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="objectId">The id of the object to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object exists.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public static bool ObjectExists<T1, T2>(IList<T1> list, int objectId) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            return list?.Any(obj => obj is T2 && obj.Id == objectId) ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Determines whether the object exists in the list of type T2, by name.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="objectName">The name of the object to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object exists.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public static bool ObjectExists<T1, T2>(IList<T1> list, string objectName) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            return list?.Any(obj => obj is T2 && obj.Name == objectName) ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Retrieves an object of type T2, by id.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="objectId">The id of the object to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns an object of Type T2, if present, otherwise null.</returns>
        public static T2 GetObject<T1, T2>(IList<T1> list, int objectId) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            return ObjectExists<T1, T2>(list, objectId) ? (T2)list.FirstOrDefault(item => item.Id == objectId) : default;
        }

        /// <summary>
        /// Retrieves an object of type T2, by name.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="objectName">The name of the object to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns an object of Type T2, if present, otherwise null.</returns>
        public static T2 GetObject<T1, T2>(IList<T1> list, string objectName) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            return ObjectExists<T1, T2>(list, objectName) ? (T2)list.FirstOrDefault(item => item.Name == objectName) : default;
        }

        /// <summary>
        /// Removes an object of type T2, by id.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="objectId">The id of the object to remove. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object was removed.</returns>
        public static bool RemoveObject<T1, T2>(IList<T1> list, int objectId) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            return list.Remove(GetObject<T1, T2>(list, objectId));
        }

        /// <summary>
        /// Removes an object of type T2, by name.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="objectName">The name of the object to remove. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object was removed.</returns>
        public static bool RemoveObject<T1, T2>(IList<T1> list, string objectName) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            return list.Remove(GetObject<T1, T2>(list, objectName));
        }

        /// <summary> 
        /// Reorders an object within the list, by id.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="objectId">The id of the object to move. Intaken as an <see cref="int"/>.</param>
        /// <param name="otherObjectId">The id of the object to move above. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ReorderObject<T1, T2>(IList<T1> list, int objectId, int otherObjectId) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            // Temporarily store the object.
            var item = GetObject<T1, T2>(list, objectId);

            var otherItem = GetObject<T1, T2>(list, otherObjectId);
            var otherItemIndex = list.IndexOf(otherItem);

            // Remove current object.
            RemoveObject<T1, T2>(list, objectId);

            // Update layer.
            list.Insert(otherItemIndex, item);
        }

        /// <summary> 
        /// Reorders an object within the list, by name.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="objectName">The name of the object to move. Intaken as a <see cref="string"/>.</param>
        /// <param name="otherObjectId">The id of the object to move above. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ReorderObject<T1, T2>(IList<T1> list, string objectName, int otherObjectId) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            // Temporarily store the object.
            var item = GetObject<T1, T2>(list, objectName);

            var otherItem = GetObject<T1, T2>(list, otherObjectId);
            var otherItemIndex = list.IndexOf(otherItem);

            // Remove current object.
            RemoveObject<T1, T2>(list, objectName);

            // Update layer.
            list.Insert(otherItemIndex, item);
        }

        /// <summary> 
        /// Reorders an object within the list, by id.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="objectId">The id of the object to move. Intaken as an <see cref="int"/>.</param>
        /// <param name="otherObjectName">The name of the object to move above. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ReorderObject<T1, T2>(IList<T1> list, int objectId, string otherObjectName) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            // Temporarily store the object.
            var item = GetObject<T1, T2>(list, objectId);

            var otherItem = GetObject<T1, T2>(list, otherObjectName);
            var otherItemIndex = list.IndexOf(otherItem);

            // Remove current object.
            RemoveObject<T1, T2>(list, objectId);

            // Update layer.
            list.Insert(otherItemIndex, item);
        }

        /// <summary> 
        /// Reorders an object within the list, by name.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="objectName">The name of the object to move. Intaken as a <see cref="string"/>.</param>
        /// <param name="otherObjectName">The name of the object to move above. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ReorderObject<T1, T2>(IList<T1> list, string objectName, string otherObjectName) where T1 : IMonoGameIdentifierComponent where T2 : T1
        {
            // Temporarily store the object.
            var item = GetObject<T1, T2>(list, objectName);

            var otherItem = GetObject<T1, T2>(list, otherObjectName);
            var otherItemIndex = list.IndexOf(otherItem);

            // Remove current object.
            RemoveObject<T1, T2>(list, objectName);

            // Update layer.
            list.Insert(otherItemIndex, item);
        }

        #endregion

        #region Identity + Layer

        /// <summary>
        /// Retrieves the next valid id for an object of type T2 from the provided list on the provided layer.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to inspect to produce a valid id. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="layer">The layer to produce a valid id on. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a valid id for an object of type T2 as an <see cref="int"/>.</returns>
        public static int GetNextValidObjectId<T1, T2>(IList<T1> list, int layer) where T1 : IMonoGameIdentifierComponent, IMonoGameLayerComponent where T2 : T1
        {
            var nextId = 1;
            while (list.Any(obj => obj.Layer == layer && obj is T2 && obj.Id == nextId))
            {
                nextId++;
            }

            return nextId;
        }

        /// <summary>
        /// Determines whether the object exists in the list of type T2 on the provided layer, by id.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="objectId">The id of the object to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object exists.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public static bool ObjectExists<T1, T2>(IList<T1> list, int layer, int objectId) where T1 : IMonoGameIdentifierComponent, IMonoGameLayerComponent where T2 : T1
        {
            return list?.Any(obj => obj.Layer == layer && obj is T2 && obj.Id == objectId) ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Determines whether the object exists in the list of type T2 on the provided layer, by name.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="objectName">The name of the object to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object exists.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public static bool ObjectExists<T1, T2>(IList<T1> list, int layer, string objectName) where T1 : IMonoGameIdentifierComponent, IMonoGameLayerComponent where T2 : T1
        {
            return list?.Any(obj => obj.Layer == layer && obj is T2 && obj.Name == objectName) ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Retrieves an object of type T2 on the provided layer, by id.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="objectId">The id of the object to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns an object of Type T2, if present, otherwise null.</returns>
        public static T2 GetObject<T1, T2>(IList<T1> list, int layer, int objectId) where T1 : IMonoGameIdentifierComponent, IMonoGameLayerComponent where T2 : T1
        {
            return ObjectExists<T1, T2>(list, layer, objectId) ? (T2)list.FirstOrDefault(item => item.Layer == layer && item.Id == objectId) : default;
        }

        /// <summary>
        /// Retrieves an object of type T2 on the provided layer, by name.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="objectName">The name of the object to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns an object of Type T2, if present, otherwise null.</returns>
        public static T2 GetObject<T1, T2>(IList<T1> list, int layer, string objectName) where T1 : IMonoGameIdentifierComponent, IMonoGameLayerComponent where T2 : T1
        {
            return ObjectExists<T1, T2>(list, layer, objectName) ? (T2)list.FirstOrDefault(item => item.Layer == layer && item.Name == objectName) : default;
        }

        /// <summary>
        /// Removes an object of type T2 on the provided layer, by id.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="objectId">The id of the object to remove. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object was removed.</returns>
        public static bool RemoveObject<T1, T2>(IList<T1> list, int layer, int objectId) where T1 : IMonoGameIdentifierComponent, IMonoGameLayerComponent where T2 : T1
        {
            return list.Remove(GetObject<T1, T2>(list, layer, objectId));
        }

        /// <summary>
        /// Removes an object of type T2 on the provided layer, by name.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="objectName">The name of the object to remove. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object was removed.</returns>
        public static bool RemoveObject<T1, T2>(IList<T1> list, int layer, string objectName) where T1 : IMonoGameIdentifierComponent, IMonoGameLayerComponent where T2 : T1
        {
            return list.Remove(GetObject<T1, T2>(list, layer, objectName));
        }

        /// <summary> 
        /// Modifies an object's layer, by id.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="objectId">The id of the object to change it's layer. Intaken as an <see cref="int"/>.</param>
        /// <param name="newLayer">The layer to place the object on. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ChangeObjectLayer<T1, T2>(IList<T1> list, int layer, int objectId, int newLayer) where T1 : IMonoGameIdentifierComponent, IMonoGameLayerComponent where T2 : T1
        {
            // Temporarily store the object.
            var item = GetObject<T1, T2>(list, layer, objectId);

            // Remove current object.
            RemoveObject<T1, T2>(list, layer, objectId);

            // Update layer.
            item.Layer = newLayer;

            // Add the object back to the list with it's new layer.
            list.Add(item);
        }

        /// <summary> 
        /// Modifies an object's layer, by name.
        /// </summary>
        /// <typeparam name="T1">An object inheriting <see cref="IMonoGameIdentifierComponent"/> and <see cref="IMonoGameLayerComponent"/>.</typeparam>
        /// <typeparam name="T2">An object inheriting type T1.</typeparam>
        /// <param name="list">The list to check against for an object of type T2. Intaken as a <see cref="IList{T1}"/>.</param>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="objectName">The id of the object to change it's layer. Intaken as a <see cref="string"/>.</param>
        /// <param name="newLayer">The layer to place the object on. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the object's layer was changed.</returns>
        public static void ChangeObjectLayer<T1, T2>(IList<T1> list, int layer, string objectName, int newLayer) where T1 : IMonoGameIdentifierComponent, IMonoGameLayerComponent where T2 : T1
        {
            // Temporarily store the object.
            var item = GetObject<T1, T2>(list, layer, objectName);

            // Remove current object.
            RemoveObject<T1, T2>(list, layer, objectName);

            // Update layer.
            item.Layer = newLayer;

            // Add the object back to the list with it's new layer.
            list.Add(item);
        }

        #endregion
    }
}