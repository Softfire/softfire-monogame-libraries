using System.Net;
using System.Threading.Tasks;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.NTWK
{
    /// <summary>
    /// Net Peer Status.
    /// </summary>
    public enum NetPeerStatus
    {
        Stopped,
        Starting,
        Running,
        Stopping
    }

    public class NetPeer : INetPeer
    {
        /// <summary>
        /// Logger.
        /// </summary>
        protected Logger Logger { get; }

        /// <summary>
        /// Configuration.
        /// </summary>
        public NetPeerConfiguration Configuration { get; }

        /// <summary>
        /// Status.
        /// Current Net Peer Status.
        /// </summary>
        public NetPeerStatus Status { get; }

        /// <summary>
        /// Start.
        /// </summary>
        /// <param name="message">A message to output to logger on Start.</param>
        /// <returns>Returns a Task.</returns>
        public async Task Start(string message = "Starting server.")
        {
            //TODO: Create a new thread and start the server.
        }

        /// <summary>
        /// Shutdown.
        /// </summary>
        /// <param name="message">A message to output to logger on Shutdown.</param>
        /// <returns>Returns a Task.</returns>
        public async Task Shutdown(string message = "Shutting down server.")
        {
            //TODO: Stop the server.
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public NetPeer(NetPeerConfiguration netConfiguration)
        {
            Configuration = netConfiguration;
            Logger = new Logger(Configuration.LogFilePath);
            Status = NetPeerStatus.Stopped;
        }

        /// <summary>
        /// Net Peer Constructor.
        /// </summary>
        /// <param name="identifier">A unique identifier. Intaken as a string.</param>
        /// <param name="ipAddress">The Ip Address to listen with.</param>
        /// <param name="port">The Port to listen on.</param>
        /// <param name="logFilePath">The path to store the NetPeer's log file.</param>
        public NetPeer(string identifier, IPAddress ipAddress, int port, string logFilePath = @"Config\Logs\Server")
        {
            Logger = new Logger(logFilePath);

            Status = NetPeerStatus.Stopped;
        }
    }
}