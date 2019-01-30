using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.NTWK
{
    /// <summary>
    /// Net Manager.
    /// Manages clients and servers.
    /// </summary>
    /// <typeparam name="T1">An instance of Type NetClient.</typeparam>
    /// <typeparam name="T2">An instance of Type NetServer.</typeparam>
    public class NetManager<T1, T2> where T1 : NetClient
                                    where T2 : NetServer
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private Logger Logger { get; }

        /// <summary>
        /// Clients.
        /// </summary>
        private Dictionary<string, T1> Clients { get; }

        /// <summary>
        /// Servers.
        /// </summary>
        private Dictionary<string, T2> Servers { get; }

        #region Enums

        /// <summary>
        /// Add Server Results.
        /// </summary>
        public enum AddServerResults
        {
            Failure,
            Success,
            DeniedIdentifierNullOrWhitespace,
            DeniedServerAlreadyExists
        }

        /// <summary>
        /// Add Client Results.
        /// </summary>
        public enum AddClientResults
        {
            Failure,
            Success,
            DeniedIdentifierNullOrWhitespace,
            DeniedClientAlreadyExists
        }

        /// <summary>
        /// Get Server Results.
        /// </summary>
        public enum GetServerResults
        {
            Failure,
            Success,
            DeniedIdentifierNullOrWhitespace,
            DeniedServerNotFound
        }

        /// <summary>
        /// Get Client Results.
        /// </summary>
        public enum GetClientResults
        {
            Failure,
            Success,
            DeniedIdentifierNullOrWhitespace,
            DeniedClientNotFound
        }

        /// <summary>
        /// Start Server Results.
        /// </summary>
        public enum StartServerResults
        {
            Failure,
            Success,
            DeniedIdentifierNullOrWhitespace,
            DeniedServerNotInProperState,
            DeniedServerNotFound
        }

        /// <summary>
        /// Start Client Results.
        /// </summary>
        public enum StartClientResults
        {
            Failure,
            Success,
            DeniedIdentifierNullOrWhitespace,
            DeniedClientNotInProperState,
            DeniedClientNotFound
        }

        /// <summary>
        /// Shutdown Server Results.
        /// </summary>
        public enum ShutdownServerResults
        {
            Failure,
            Success,
            DeniedIdentifierNullOrWhitespace,
            DeniedServerNotInProperState,
            DeniedServerNotFound
        }

        /// <summary>
        /// Shutdown Client Results.
        /// </summary>
        public enum ShutdownClientResults
        {
            Failure,
            Success,
            DeniedIdentifierNullOrWhitespace,
            DeniedClientNotInProperState,
            DeniedClientNotFound
        }

        /// <summary>
        /// Remove Server Results.
        /// </summary>
        public enum RemoveServerResults
        {
            Failure,
            Success,
            DeniedIdentifierNullOrWhitespace,
            DeniedServerNotInProperState,
            DeniedServerNotFound
        }

        /// <summary>
        /// Remove Client Rsults.
        /// </summary>
        public enum RemoveClientResults
        {
            Failure,
            Success,
            DeniedIdentifierNullOrWhitespace,
            DeniedClientNotInProperState,
            DeniedClientNotFound
        }

        #endregion

        /// <summary>
        /// Net Manager Constructor.
        /// </summary>
        public NetManager(string logFilePath = @"Config\Logs\NetManager")
        {
            Clients = new Dictionary<string, T1>();
            Servers = new Dictionary<string, T2>();
            Logger = new Logger(logFilePath);
        }

        #region Core Server Commands

        /// <summary>
        /// Add Server.
        /// </summary>
        /// <param name="identifier">A unique identifier. Used to Get, Start and Shutdown the server. Intaken as a <see cref="string"/>.</param>
        /// <param name="applicationIdentifier">Intakes a uniques string to identify the application. Used by clients and servers to connect.</param>
        /// <param name="ipAddress">Intakes an IPAddress that the client will bind to and listen to client responses.</param>
        /// <param name="port">Intakes a port number, as an int, to bind to.</param>
        /// <returns>Returns an enum of AddServerResults.</returns>
        public AddServerResults AddServer(string identifier, string applicationIdentifier, IPAddress ipAddress, int port)
        {
            try
            {
                #region Checks

                // Check identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding of server failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(AddServer)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Identifier was null or whitespace.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return AddServerResults.DeniedIdentifierNullOrWhitespace;
                }

                // Check if server already exists.
                if (Servers.ContainsKey(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding of server failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(AddServer)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Server already exists.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return AddServerResults.DeniedServerAlreadyExists;
                }

                #endregion

                // Add server.
                Servers.Add(identifier, (T2)new NetServer(applicationIdentifier, ipAddress, port));

                // Return success code.
                return AddServerResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(AddServer)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"IP/Port: {ipAddress}:{port}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return AddServerResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(AddServer)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}" +
                                         $"IP/Port: {ipAddress}:{port}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return AddServerResults.Failure;
        }

        /// <summary>
        /// Get Server.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T2.</param>
        /// <returns>Returns an enum or GetServerResults and a server of Type T2 or null.</returns>
        public (GetServerResults, T2) GetServer(string identifier)
        {
            try
            {
                #region Checks

                // Check identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Retrieval of server failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(GetServer)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Identifier was null or whitespace.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null.
                    return (GetServerResults.DeniedIdentifierNullOrWhitespace, null);
                }

                // Check if server exists.
                if (!Servers.ContainsKey(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Retrieval of server failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(GetServer)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Server not found.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null.
                    return (GetServerResults.DeniedServerNotFound, null);
                }

                #endregion

                // Return success code and server.
                return (GetServerResults.Success, Servers[identifier]);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(GetServer)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code and null.
                    return (GetServerResults.Failure, null);
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(GetServer)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code and null.
            return (GetServerResults.Failure, null);
        }

        /// <summary>
        /// Start Server.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T2.</param>
        /// <param name="startMessage">The start message. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns an enum of StartServerResults.</returns>
        public async Task<StartServerResults> StartServer(string identifier, string startMessage = "Starting server.")
        {
            try
            {
                #region Checks

                // Check identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Starting of server failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(StartServer)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Identifier was null or whitespace.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return StartServerResults.DeniedIdentifierNullOrWhitespace;
                }

                // Check if server exists.
                if (!Servers.ContainsKey(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Starting of server failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(StartServer)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Server not found.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return StartServerResults.DeniedServerNotFound;
                }

                // Check if server is stopped.
                if (Servers[identifier].Status != NetPeerStatus.Stopped)
                {
                    // Return failure code.
                    return StartServerResults.DeniedServerNotInProperState;
                }

                #endregion

                // Start server.
                await Servers[identifier].Start(startMessage);

                // Return success code.
                return StartServerResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(StartServer)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return StartServerResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(StartServer)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return StartServerResults.Failure;
        }

        /// <summary>
        /// Shutdown Server.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T2.</param>
        /// <param name="shutdownMessage">Intakes a string to be used as the shutdown message that will be sent to the server's clients.</param>
        /// <returns>Returns an enum of ShutdownServerResults..</returns>
        public async Task<ShutdownServerResults> ShutdownServer(string identifier, string shutdownMessage = "Shutting down server.")
        {
            try
            {
                #region Checks

                // Check identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Shutting down of server failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ShutdownServer)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Identifier was null or whitespace.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return ShutdownServerResults.DeniedIdentifierNullOrWhitespace;
                }

                // Check if server exists.
                if (!Servers.ContainsKey(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Shutting down of server failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ShutdownServer)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Server not found.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return ShutdownServerResults.DeniedServerNotFound;
                }

                // Check if server is stopped.
                if (Servers[identifier].Status != NetPeerStatus.Running)
                {
                    // Return failure code.
                    return ShutdownServerResults.DeniedServerNotInProperState;
                }

                #endregion

                // Start server.
                await Servers[identifier].Shutdown(shutdownMessage);

                // Return success code.
                return ShutdownServerResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(ShutdownServer)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return ShutdownServerResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(ShutdownServer)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return ShutdownServerResults.Failure;
        }

        /// <summary>
        /// Remove Server.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested object of Type T2.</param>
        /// <returns>Returns an enum of ResultsServerResults.</returns>
        public RemoveServerResults RemoveServer(string identifier)
        {
            try
            {
                #region Checks

                // Check identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removal of server failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveServer)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Identifier was null or whitespace.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return RemoveServerResults.DeniedIdentifierNullOrWhitespace;
                }

                // Check if server exists.
                if (!Servers.ContainsKey(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removal of server failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveServer)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Server not found.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return RemoveServerResults.DeniedServerNotFound;
                }

                // Check if server is stopped.
                if (Servers[identifier].Status != NetPeerStatus.Stopped)
                {
                    // Return failure code.
                    return RemoveServerResults.DeniedServerNotInProperState;
                }

                #endregion

                // Remove server.
                Servers.Remove(identifier);

                // Return success code.
                return RemoveServerResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(RemoveServer)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return RemoveServerResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(RemoveServer)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return RemoveServerResults.Failure;
        }

        #endregion

        #region Core Client Commands

        /// <summary>
        /// Add Client.
        /// </summary>
        /// <param name="identifier">A unique identifier. Used to Get, Start and Shutdown the client. Intaken as a <see cref="string"/>.</param>
        /// <param name="applicationIdentifier">Intakes a uniques string to identify the application. Used by clients and servers to connect.</param>
        /// <param name="ipAddress">Intakes an IPAddress that the client will bind to and listen to client responses.</param>
        /// <param name="port">Intakes a port number, as an int, to bind to.</param>
        /// <returns>Returns an enum of AddClientResults.</returns>
        public AddClientResults AddClient(string identifier, string applicationIdentifier, IPAddress ipAddress, int port)
        {
            try
            {
                #region Checks

                // Check identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding of client failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(AddClient)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Identifier was null or whitespace.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return AddClientResults.DeniedIdentifierNullOrWhitespace;
                }

                // Check if client already exists.
                if (Clients.ContainsKey(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding of client failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(AddClient)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Client already exists.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return AddClientResults.DeniedClientAlreadyExists;
                }

                #endregion

                // Add client.
                Clients.Add(identifier, (T1)new NetClient(applicationIdentifier, ipAddress, port));

                // Return success code.
                return AddClientResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(AddClient)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"IP/Port: {ipAddress}:{port}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return AddClientResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(AddServer)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}" +
                                         $"IP/Port: {ipAddress}:{port}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return AddClientResults.Failure;
        }

        /// <summary>
        /// Get Client.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T1.</param>
        /// <returns>Returns an enum of GetClientResults and a client of Type T1 or null.</returns>
        public (GetClientResults, T1) GetClient(string identifier)
        {
            try
            {
                #region Checks

                // Check identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Retrieval of client failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(GetClient)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Identifier was null or whitespace.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null.
                    return (GetClientResults.DeniedIdentifierNullOrWhitespace, null);
                }

                // Check if client exists.
                if (!Clients.ContainsKey(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Retrieval of client failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(GetClient)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Client not found.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null.
                    return (GetClientResults.DeniedClientNotFound, null);
                }

                #endregion

                // Return success code and client.
                return (GetClientResults.Success, Clients[identifier]);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(GetClient)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code and null.
                    return (GetClientResults.Failure, null);
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(GetClient)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code and null.
            return (GetClientResults.Failure, null);
        }

        /// <summary>
        /// Start Client.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T1.</param>
        /// <param name="startMessage">The start message. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns an enum of StartClientResults.</returns>
        public async Task<StartClientResults> StartClient(string identifier, string startMessage = "Starting client.")
        {
            try
            {
                #region Checks

                // Check identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Strating of client failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(StartClient)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Identifier was null or whitespace.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return StartClientResults.DeniedIdentifierNullOrWhitespace;
                }

                // Check if client exists.
                if (!Servers.ContainsKey(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Retrieval of client failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(StartClient)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Client not found.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return StartClientResults.DeniedClientNotFound;
                }

                // Check if client is stopped.
                if (Clients[identifier].Status != NetPeerStatus.Stopped)
                {
                    // Return failure code.
                    return StartClientResults.DeniedClientNotInProperState;
                }

                #endregion

                // Start client.
                await Clients[identifier].Start(startMessage);

                // Return success code.
                return StartClientResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(StartClient)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return StartClientResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(StartClient)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return StartClientResults.Failure;
        }

        /// <summary>
        /// Shutdown Client.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T1.</param>
        /// <param name="shutdownMessage">Intakes a string to be used as the shutdown message.</param>
        /// <returns>Returns an enum of ShutdownClientResults.</returns>
        public async Task<ShutdownClientResults> ShutdownClient(string identifier, string shutdownMessage = "Shutting down client.")
        {
            try
            {
                #region Checks

                // Check identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Shutting down of client failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ShutdownClient)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Identifier was null or whitespace.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return ShutdownClientResults.DeniedIdentifierNullOrWhitespace;
                }

                // Check if client exists.
                if (!Clients.ContainsKey(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Shutting down of client failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ShutdownClient)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Client not found.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return ShutdownClientResults.DeniedClientNotFound;
                }

                // Check if client is stopped.
                if (Clients[identifier].Status != NetPeerStatus.Running)
                {
                    // Return failure code.
                    return ShutdownClientResults.DeniedClientNotInProperState;
                }

                #endregion

                // Start client.
                await Clients[identifier].Shutdown(shutdownMessage);

                // Return success code.
                return ShutdownClientResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(ShutdownClient)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return ShutdownClientResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(ShutdownClient)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return ShutdownClientResults.Failure;
        }

        /// <summary>
        /// Remove Client.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested object of Type T1.</param>
        /// <returns>Returns an enum of RemoveClientResults.</returns>
        public RemoveClientResults RemoveClient(string identifier)
        {
            try
            {
                #region Checks

                // Check identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removal of client failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveClient)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Identifier was null or whitespace.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return RemoveClientResults.DeniedIdentifierNullOrWhitespace;
                }

                // Check if client exists.
                if (!Clients.ContainsKey(identifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removal of client failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveClient)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(identifier)}{Environment.NewLine}" +
                                                   $"Message: Client not found.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return RemoveClientResults.DeniedClientNotFound;
                }

                // Check if client is stopped.
                if (Clients[identifier].Status != NetPeerStatus.Stopped)
                {
                    // Return failure code.
                    return RemoveClientResults.DeniedClientNotInProperState;
                }

                #endregion

                // Remove client.
                Clients.Remove(identifier);

                // Return success code.
                return RemoveClientResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(RemoveClient)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return RemoveClientResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(RemoveClient)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return RemoveClientResults.Failure;
        }

        #endregion
    }
}