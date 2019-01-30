using System.Collections.Generic;
using Softfire.MonoGame.DB;

namespace Softfire.MonoGame.NTWK.Services.WCF
{
    /// <summary>
    /// MonoGame WCF.
    /// </summary>
    /// <typeparam name="T">An object of Type T.</typeparam>
    public class MonoGameWCF<T> : IMonoGameWCF<T>
    {
        /// <summary>
        /// Repository.
        /// </summary>
        private IRepository<T> _repository;

        /// <summary>
        /// MonoGame WCF Default Constructor.
        /// </summary>
        /// <param name="t">An object of Type t.</param>
        public MonoGameWCF(IRepository<T> t)
        {
            _repository = t;
        }

        /// <summary>
        /// Get Data.
        /// Returns all objects of Type T from the database.
        /// </summary>
        /// <returns>Returns an IEnumerable containing all objects of Type T in the database.</returns>
        public IEnumerable<T> GetData()
        {
            return _repository.GetData();
        }

        /// <summary>
        /// Create.
        /// Creates a new entry in the database from the supplied data.
        /// </summary>
        /// <param name="t">An object of Type t.</param>
        public void Create(T t)
        {
            _repository.Create(t);
        }

        /// <summary>
        /// Retrieve.
        /// Returns an object of Type T from the database.
        /// </summary>
        /// <param name="id">The object's unique Id. Used against the database. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the requested object of Type t otherwise null, if not found.</returns>
        public T Retrieve(int id)
        {
            return _repository.Retrieve(id);
        }

        /// <summary>
        /// Update.
        /// Updates an object of Type T in the database.
        /// </summary>
        /// <param name="id">The object's unique Id. Used against the database. Intaken as an <see cref="int"/>.</param>
        /// <param name="t">An object of Type t containing new data.</param>
        public void Update(int id, T t)
        {
            _repository.Update(id, t);
        }

        /// <summary>
        /// Delete.
        /// Flags the object for deletion.
        /// </summary>
        /// <param name="id">The object's unique Id. Used against the database. Intaken as an <see cref="int"/>.</param>
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}