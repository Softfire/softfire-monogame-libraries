using System;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.DB
{
    internal interface IDatabase : IDisposable
    {
        /// <summary>
        /// Logger.
        /// </summary>
        Logger Logger { get; }

        /// <summary>
        /// Server.
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
        /// Open Connection.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the connaction was opened.</returns>
        bool OpenConnection();

        /// <summary>
        /// Close Connection.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the connaction was cloased.</returns>
        bool CloseConnection();
    }
}