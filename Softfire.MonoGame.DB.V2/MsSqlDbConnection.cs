using System;
using System.Data.SqlClient;
using Softfire.MonoGame.LOG.V2;

namespace Softfire.MonoGame.DB.V2
{
    /// <summary>
    /// MSSQL DBConnect object with logging.
    /// </summary>
    public sealed class MsSqlDbConnection : IDatabase
    {
        /// <summary>
        /// Logger.
        /// </summary>
        public Logger Logger { get; }

        /// <summary>
        /// Server.
        /// </summary>
        public string Server { get; }

        /// <summary>
        /// User Id.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Database.
        /// </summary>
        public string Database { get; }

        /// <summary>
        /// Port.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Connection.
        /// </summary>
        public SqlConnection Connection { get; }

        /// <summary>
        /// DB Connect
        /// </summary>
        /// <param name="server"></param>
        /// <param name="userId">auth_windows</param>
        /// <param name="password"></param>
        /// <param name="database"></param>
        /// <param name="port"></param>
        /// <param name="logFilePath">Intakes a file path for logs relative to the calling application.</param>
        public MsSqlDbConnection(string server, string userId = "", string password = "", string database = "", int port = 1433, string logFilePath = @"Config\Logs\Database")
        {
            Server = server;
            UserId = userId;
            Password = password;
            Database = database;
            Port = port;

            Connection = new SqlConnection(ConnectionString());
            Logger = new Logger(logFilePath + $@"\{Server}\{Database}");
        }

        /// <summary>
        /// A connection string, compiled and returned in the proper format used by MSSQL.
        /// </summary>
        /// <returns>Returns a proper formatted connection <see cref="string"/> for MSSQL.</returns>
        private string ConnectionString() => $"Server={Server},{Port};" +
                                             $"Database={Database};" +
                                             $"User Id={UserId};" +
                                             $"Password={Password};";

        /// <summary>
        /// Open Connection.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the connection was opened.</returns>
        public bool Connect()
        {
            var result = false;

            try
            {
                if (Connection != null)
                {
                    Connection.Open();
                    result = true;
                }
            }
            catch (SqlException ex)
            {
                Logger.Write(LogTypes.Error, $"Error Code: {ex.Number}{Environment.NewLine}" + $"Message: {ex.Message}", useInlineLayout: false);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Write(LogTypes.Error, ex.ToString(), useInlineLayout: false);
            }
            finally
            {
                Logger.Write(LogTypes.Info, "Connection to " + Server + ":" + Port + " result: " + result.ToString());
            }

            return result;
        }

        /// <summary>
        /// Close Connection.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the connection was closed.</returns>
        public bool Close()
        {
            var result = false;

            try
            {
                if (Connection != null)
                {
                    Connection.Close();
                    result = true;
                }
            }
            catch (SqlException ex)
            {
                Logger.Write(LogTypes.Error, $"Error Code: {ex.Number}{Environment.NewLine}" + $"Message: {ex.Message}", useInlineLayout: false);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Write(LogTypes.Error, ex.ToString(), useInlineLayout: false);
            }
            finally
            {
                Logger.Write(LogTypes.Info, "Connection to " + Server + ":" + Port + " closed.");
            }

            return result;
        }
        
        /// <summary>
        /// Disposes the SQLConnection.
        /// </summary>
        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}