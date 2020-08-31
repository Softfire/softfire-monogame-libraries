using System.Collections.Generic;
using System.Net;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.NTWK.V2.Services.Lidgren
{
    public class LidgrenNetManager<T1, T2> where T1 : LidgrenClient
                                           where T2 : LidgrenServer
    {
        /// <summary>
        /// Clients.
        /// </summary>
        private Dictionary<string, T1> Clients { get; }

        /// <summary>
        /// Servers.
        /// </summary>
        private Dictionary<string, T2> Servers { get; }

        /// <summary>
        /// Lidgren Network Manager Constructor.
        /// </summary>
        protected LidgrenNetManager()
        {
            Clients = new Dictionary<string, T1>();
            Servers = new Dictionary<string, T2>();
        }

        #region Core Server Commands

        /// <summary>
        /// Add Server.
        /// </summary>
        /// <param name="identifier">A unique identifier. Used to Get, Start and Shutdown the server. Intaken as a <see cref="string"/>.</param>
        /// <param name="applicationIdentifier">Intakes a uniques string to identify the application. Used by clients and servers to connect. Default is Softfire.MonoGame.NTWK.</param>
        /// <param name="ipAddress">Intakes an IPAddress that the client will bind to and listen to client responses.</param>
        /// <param name="port">Intakes a port number, as an int, to bind to. Default is 16464.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Server was added or not.</returns>
        public bool AddServer(string identifier, string applicationIdentifier = null, IPAddress ipAddress = null, int port = 16464)
        {
            var result = false;

            if (!Servers.ContainsKey(identifier))
            {
                Servers.Add(identifier, (T2)new LidgrenServer(applicationIdentifier, ipAddress, port));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Server.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T2.</param>
        /// <returns>Returns an server of Type T2.</returns>
        public T2 GetServer(string identifier)
        {
            T2 result = null;

            if (Servers.ContainsKey(identifier))
            {
                result = Servers[identifier];
            }

            return result;
        }

        /// <summary>
        /// Start Server.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T2.</param>
        /// <returns>Returns a bool indicating whether the server was started successfully.</returns>
        public bool StartServer(string identifier)
        {
            var result = false;

            if (Servers.ContainsKey(identifier))
            {
                if (Servers[identifier].Status == LidgrenNetPeerStatus.NotRunning)
                {
                    Servers[identifier].Start();
                    result = Servers[identifier].Status == LidgrenNetPeerStatus.Starting ||
                             Servers[identifier].Status == LidgrenNetPeerStatus.Running;
                }
            }

            return result;
        }

        /// <summary>
        /// Shutdown Server.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T2.</param>
        /// <param name="shutdownMessage">Intakes a string to be used as the shutdown message that will be sent to the server's clients.</param>
        /// <returns>Returns a bool indicating whether the server was shutdown successfully.</returns>
        public bool ShutdownServer(string identifier, string shutdownMessage = "Shut down request registered.\nConnections will be terminated and socket(s) closed soon.")
        {
            var result = false;

            if (Servers.ContainsKey(identifier))
            {
                if (Servers[identifier].Status == LidgrenNetPeerStatus.Running)
                {
                    Servers[identifier].Shutdown(shutdownMessage);
                    result = Servers[identifier].Status == LidgrenNetPeerStatus.ShutdownRequested ||
                             Servers[identifier].Status == LidgrenNetPeerStatus.NotRunning;
                }
            }

            return result;
        }

        /// <summary>
        /// Remove Server.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested object of Type T2.</param>
        /// <returns>Returns a bool indicating whether the object of Type T2 was removed.</returns>
        public bool RemoveServer(string identifier)
        {
            var result = false;

            if (Servers.ContainsKey(identifier))
            {
                if (Servers[identifier].Status == LidgrenNetPeerStatus.NotRunning)
                {
                    result = Servers.Remove(identifier);
                }
            }

            return result;
        }

        #endregion

        #region Core Client Commands

        /// <summary>
        /// Add Client.
        /// </summary>
        /// <param name="identifier">A unique identifier. Used to Get, Start and Shutdown the client. Intaken as a <see cref="string"/>.</param>
        /// <param name="applicationIdentifier">Intakes a uniques string to identify the application. Used by clients and servers to connect. Default is Softfire.MonoGame.NTWK.</param>
        /// <param name="ipAddress">Intakes an IPAddress that the client will bind to and listen to client responses.</param>
        /// <param name="port">Intakes a port number, as an int, to bind to. Default is 16462.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Client was added or not.</returns>
        public bool AddClient(string identifier, string applicationIdentifier = null, IPAddress ipAddress = null, int port = 16462)
        {
            var result = false;

            if (!Clients.ContainsKey(identifier))
            {
                Clients.Add(identifier, (T1)new LidgrenClient(applicationIdentifier, ipAddress, port));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Client.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T1.</param>
        /// <returns>Returns an server of Type T1.</returns>
        public T1 GetClient(string identifier)
        {
            T1 result = null;

            if (Clients.ContainsKey(identifier))
            {
                result = Clients[identifier];
            }

            return result;
        }

        /// <summary>
        /// Start Client.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T1.</param>
        /// <returns>Returns a bool indicating whether the client was started successfully.</returns>
        public bool StartClient(string identifier)
        {
            var result = false;

            if (Clients.ContainsKey(identifier))
            {
                if (Clients[identifier].Status == LidgrenNetPeerStatus.NotRunning)
                {
                    Clients[identifier].Start();
                    result = Clients[identifier].Status == LidgrenNetPeerStatus.Starting ||
                             Clients[identifier].Status == LidgrenNetPeerStatus.Running;
                }
            }

            return result;
        }

        /// <summary>
        /// Shutdown Client.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested ser of Type T1.</param>
        /// <param name="shutdownMessage">Intakes a string to be used as the shutdown message.</param>
        /// <returns>Returns a bool indicating whether the client was shutdown successfully.</returns>
        public bool ShutdownClient(string identifier, string shutdownMessage = "Shut down request registered.\nConnections will be terminated and socket(s) closed soon.")
        {
            var result = false;

            if (Clients.ContainsKey(identifier))
            {
                if (Clients[identifier].Status == LidgrenNetPeerStatus.Running)
                {
                    Clients[identifier].Shutdown(shutdownMessage);
                    result = Clients[identifier].Status == LidgrenNetPeerStatus.ShutdownRequested ||
                             Clients[identifier].Status == LidgrenNetPeerStatus.NotRunning;
                }
            }

            return result;
        }

        /// <summary>
        /// Remove Client.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier used to find the requested object of Type T1.</param>
        /// <returns>Returns a bool indicating whether the object of Type T1 was removed.</returns>
        public bool RemoveClient(string identifier)
        {
            var result = false;

            if (Clients.ContainsKey(identifier))
            {
                if (Clients[identifier].Status == LidgrenNetPeerStatus.NotRunning)
                {
                    result = Clients.Remove(identifier);
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Network Manager Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public virtual void Update(GameTime gameTime)
        {
            // Update Client
            foreach (var client in Clients)
            {
                if (client.Value.Status == LidgrenNetPeerStatus.Running)
                {
                    client.Value.Update(gameTime);
                }
            }

            // Update Servers
            foreach (var server in Servers)
            {
                if (server.Value.Status == LidgrenNetPeerStatus.Running)
                {
                    server.Value.Update(gameTime);
                }
            }
        }
    }
}