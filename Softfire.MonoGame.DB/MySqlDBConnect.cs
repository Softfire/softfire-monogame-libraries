using System;
using MySql.Data.MySqlClient;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.DB
{
    /// <summary>
    /// MySQL DBConnect object with logging.
    /// </summary>
    public sealed class MySqlDBConnect : IDatabase
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
        /// Convert Zero DateTime.
        /// Returns System.DateTime.MinValue valued System.DateTime object for invalid values and a System.DateTime object for valid values.
        /// </summary>
        public bool ConvertZeroDateTime { get; }

        /// <summary>
        /// Integrated Security.
        /// </summary>
        public bool IntegratedSecurity { get; }

        /// <summary>
        /// Database.
        /// </summary>
        public string Database { get; }

        /// <summary>
        /// Port.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// SSL Mode.
        /// </summary>
        public SSLModes SSLMode { get; }

        /// <summary>
        /// SSL Modes.
        /// </summary>
        public enum SSLModes
        {
            Preferred,
            Required
        }

        /// <summary>
        /// Connection.
        /// </summary>
        public MySqlConnection Connection { get; }

        /// <summary>
        /// DB Coonect
        /// </summary>
        /// <param name="server"></param>
        /// <param name="userId">auth_windows</param>
        /// <param name="password"></param>
        /// <param name="database"></param>
        /// <param name="port"></param>
        /// <param name="convertZeroDateTime"></param>
        /// <param name="integratedSecurity"></param>
        /// <param name="sslMode"></param>
        /// <param name="logFilePath">Intakes a file path for logs relative to the calling application.</param>
        public MySqlDBConnect(string server, string userId = "", string password = "", string database = "", int port = 3306,
                              bool convertZeroDateTime = false, bool integratedSecurity = false, SSLModes sslMode = SSLModes.Preferred,
                              string logFilePath = @"Config\Logs\Database")
        {
            Server = server;
            UserId = userId;
            Password = password;
            Database = database;
            Port = port;

            ConvertZeroDateTime = convertZeroDateTime;
            IntegratedSecurity = integratedSecurity;
            SSLMode = sslMode;

            Connection = new MySqlConnection(ConnectionString());
            Logger = new Logger(logFilePath + $@"\{Server}\{Database}");
        }

        /// <summary>
        /// Connection String.
        /// Compiled and returned in the proper format used by MySQL.
        /// </summary>
        /// <returns>Returns a proper formatted connection string for MYSQL.</returns>
        private string ConnectionString()
        {
            return $"Server={Server};Port={Port};Database={Database};UID={UserId};Password={Password};ConvertZeroDateTime={ConvertZeroDateTime};IntegratedSecurity={IntegratedSecurity};SslMode={SSLMode.ToString()};";
        }

        /// <summary>
        /// Open Connection.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the connaction was opened.</returns>
        public bool OpenConnection()
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
            catch (MySqlException ex)
            {
                Logger.Write(LogTypes.Error, $"Error Code: {ex.Number}{Environment.NewLine}" +
                                                    $"Message: {ex.Message}", useInlineLayout: false);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Write(LogTypes.Error, ex.ToString(), useInlineLayout: false);
            }

            return result;
        }

        /// <summary>
        /// Close Connection.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the connaction was cloased.</returns>
        public bool CloseConnection()
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
            catch (MySqlException ex)
            {
                Logger.Write(LogTypes.Error, $"Error Code: {ex.Number}{Environment.NewLine}" +
                                                    $"Message: {ex.Message}", useInlineLayout: false);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Write(LogTypes.Error, ex.ToString(), useInlineLayout: false);
            }

            return result;
        }
        
        /// <summary>
        /// Dispose.
        /// Disposes the MySQLConnection.
        /// </summary>
        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}