using System;
using Softfire.MonoGame.LOG.V2;

namespace Softfire.MonoGame.DB.V2
{
    public interface IDatabase : IDisposable
    {
        /// <summary>
        /// Logger.
        /// </summary>
        Logger Logger { get; }

        /// <summary>
        /// The server/host ip or name.
        /// </summary>
        string Server { get; }

        /// <summary>
        /// User Id.
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// Password.
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Database.
        /// </summary>
        string Database { get; }

        /// <summary>
        /// Port.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Connects to the configured database.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the connection was/is open.</returns>
        bool Connect();

        /// <summary>
        /// Closes the current connection.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the connection was/is closed.</returns>
        bool Close();
    }
}