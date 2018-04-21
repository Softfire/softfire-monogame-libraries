using System.Collections.Generic;
using System.Threading.Tasks;

namespace Softfire.MonoGame.DB
{
    /// <summary>
    /// IRepository.
    /// Implements sync and async CRUD methods.
    /// </summary>
    /// <typeparam name="T">Generic Type.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Get Data.
        /// </summary>
        /// <returns>Returns all data from the database.</returns>
        IEnumerable<T> GetData();

        /// <summary>
        /// Get Data Async.
        /// </summary>
        /// <returns>Returns all data from the database, asyncronously.</returns>
        Task<IEnumerable<T>> GetDataAsync();

        /// <summary>
        /// Create.
        /// Creates a new object entry in the database from the passed in object.
        /// </summary>
        /// <param name="t">Intacts a new object.</param>
        void Create(T t);

        /// <summary>
        /// Create Async.
        /// Creates a new object entry in the database from the passed in object, asyncronously.
        /// </summary>
        /// <param name="t">Intacts a new object.</param>
        Task CreateAsync(T t);

        /// <summary>
        /// Retrieve.
        /// Retrieves the requested object by Id.
        /// </summary>
        /// <param name="id">Intakes a object's Id as an int.</param>
        /// <returns>Returns the requested object or null if not found.</returns>
        T Retrieve(int id);

        /// <summary>
        /// Retrieve Async.
        /// Retrieves the requested object by Id, asyncronously.
        /// </summary>
        /// <param name="id">Intakes a object's Id as an int.</param>
        /// <returns>Returns the requested object or null if not found.</returns>
        Task<T> RetrieveAsync(int id);

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="id">Intakes a object's Id as an int.</param>
        /// <param name="t">Intakes a new object.</param>
        void Update(int id, T t);

        /// <summary>
        /// Update Async.
        /// </summary>
        /// <param name="id">Intakes a object's Id as an int.</param>
        /// <param name="t">Intakes a new object.</param>
        Task UpdateAsync(int id, T t);

        /// <summary>
        /// Delete.
        /// </summary>
        /// <param name="id">Intakes a object's Id as an int.</param>
        void Delete(int id);

        /// <summary>
        /// Delete Async.
        /// </summary>
        /// <param name="id">Intakes a object's Id as an int.</param>
        Task DeleteAsync(int id);
    }
}