﻿using System.Collections.Generic;

namespace Softfire.MonoGame.NTWK.V2.Services.WCF
{
    /// <summary>
    /// MonoGameWCF Interface.
    /// </summary>
    /// <typeparam name="T">An object of Type T.</typeparam>
    [ServiceContract]
    public interface IMonoGameWCF<T>
    {
        /// <summary>
        /// Get Data.
        /// Returns all objects of Type T from the database.
        /// </summary>
        /// <returns>Returns an IEnumerable containing all objects of Type T in the database.</returns>
        [OperationContract]
        IEnumerable<T> GetData();

        /// <summary>
        /// Create.
        /// Creates a new entry in the database from the supplied data.
        /// </summary>
        /// <param name="t">An object of Type t.</param>
        [OperationContract]
        void Create(T t);

        /// <summary>
        /// Retrieve.
        /// Returns an object of Type T from the database.
        /// </summary>
        /// <param name="id">The object's unique Id. Used against the database. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the requested object of Type t otherwise null, if not found.</returns>
        [OperationContract]
        T Retrieve(int id);

        /// <summary>
        /// Update.
        /// Updates an object of Type T in the database.
        /// </summary>
        /// <param name="id">The object's unique Id. Used against the database. Intaken as an <see cref="int"/>.</param>
        /// <param name="t">An object of Type t containing new data.</param>
        [OperationContract]
        void Update(int id, T t);

        /// <summary>
        /// Delete.
        /// Flags the object for deletion.
        /// </summary>
        /// <param name="id">The object's unique Id. Used against the database. Intaken as an <see cref="int"/>.</param>
        [OperationContract]
        void Delete(int id);
    }
}