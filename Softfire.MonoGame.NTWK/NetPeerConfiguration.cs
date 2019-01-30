using System.Net;
using System.Threading;

namespace Softfire.MonoGame.NTWK
{
    public class NetPeerConfiguration
    {
        #region MTU

        /// <summary>
        /// Default MTU.
        /// </summary>
        public const int DefaultMtu = 1408;

        /// <summary>
        /// Maximum MTU.
        /// </summary>
        public int MaximumMtu { get; private set; }

        /// <summary>
        /// Is Auto Expanding Mtu.
        /// </summary>
        public bool IsAutoExpandingMtu { get; private set; }

        /// <summary>
        /// MTU Expansion Interval.
        /// </summary>
        public float MtuExpansionInterval { get; private set; }

        /// <summary>
        /// Maximum MTU Expansion Attempts.
        /// </summary>
        public int MaximumMtuExpansionAttempts { get; private set; }

        #endregion

        /// <summary>
        /// Log File Path.
        /// </summary>
        public string LogFilePath { get; private set; }

        /// <summary>
        /// Is Locked.
        /// Unable to be editted if locked.
        /// </summary>
        public bool IsLocked { get; private set; } = false;

        /// <summary>
        /// Is Accepting Connections.
        /// </summary>
        public bool IsAcceptingConnections { get; private set; } = false;

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// Thread Name.
        /// </summary>
        public Thread Thread { get; private set; }

        /// <summary>
        /// Thread Name.
        /// </summary>
        public string ThreadName => Thread?.Name;

        /// <summary>
        /// Local IP Address.
        /// </summary>
        public IPAddress LocalAddress { get; private set; }

        /// <summary>
        /// Broadcast IP Address.
        /// </summary>
        public IPAddress BroadcastAddress { get; private set; }

        /// <summary>
        /// Port.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Receive Buffer Size.
        /// </summary>
        public int ReceiveBufferSize { get; private set; } = 131071;

        /// <summary>
        /// Send Buffer Size.
        /// </summary>
        public int SendBufferSize { get; private set; } = 131071;

        /// <summary>
        /// Maximum Connections.
        /// </summary>
        public int MaximumConnections { get; private set; } = 32;

        /// <summary>
        /// Ping Interval.
        /// </summary>
        public float PingInterval { get; private set; } = 4.0f;

        /// <summary>
        /// Connection Time Out.
        /// </summary>
        public float ConnectionTimeOut { get; private set; } = 25.0f;

        /// <summary>
        /// Reconnection Interval.
        /// </summary>
        public float ReconnectionInterval { get; private set; } = 3.0f;

        /// <summary>
        /// Maximum Reconnection Attempts.
        /// </summary>
        public int MaximumReconnectionAttempts { get; private set; } = 5;

        /// <summary>
        /// Peer Type.
        /// </summary>
        public PeerTypes PeerType { get; private set; }

        /// <summary>
        /// Peer Types.
        /// </summary>
        public enum PeerTypes
        {
            Peer,
            Client,
            Server
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public NetPeerConfiguration()
        {
            Identifier = "Softfire.MonoGame.NTWK";
            LogFilePath = @"Config\Logs\";

        }

        /// <summary>
        /// Net Peer Configuration Constructor.
        /// </summary>
        /// <param name="identifier">A unique identifier. Intaken as a <see cref="string"/>.</param>
        /// <param name="logFilePath">The path to store the NetPeer's log file.</param>
        /// <param name="peerType">The Type of Peer. Used in LogFilePath to store logs.</param>
        public NetPeerConfiguration(string identifier = "Softfire.MonoGame.NTWK", string logFilePath = @"Config\Logs\", PeerTypes peerType = PeerTypes.Peer)
        {
            Identifier = identifier;

            if (logFilePath == @"Config\Logs\")
            {
                switch (peerType)
                {
                    case PeerTypes.Peer:
                        LogFilePath = logFilePath + "Peer";
                        break;
                    case PeerTypes.Client:
                        LogFilePath = logFilePath + "Client";
                        break;
                    case PeerTypes.Server:
                        LogFilePath = logFilePath + "Server";
                        break;
                }
            }
            else
            {
                LogFilePath = logFilePath;
            }

            PeerType = peerType;
        }

        /// <summary>
        /// Lock.
        /// Locks against edits.
        /// Only usable with an Unstarted/Stopped Thread.
        /// </summary>
        public void Lock()
        {
            if (Thread != null &&
                (Thread.ThreadState == ThreadState.Unstarted ||
                Thread.ThreadState == ThreadState.Stopped))
            {
                IsLocked = true;
            }
        }

        /// <summary>
        /// Unlock.
        /// Unlocks config for editting.
        /// Only usable with an Unstarted/Stopped Thread.
        /// </summary>
        public void UnLock()
        {
            if (Thread == null ||
                (Thread.ThreadState == ThreadState.Unstarted ||
                Thread.ThreadState == ThreadState.Stopped))
            {
                IsLocked = false;
            }
        }

        /// <summary>
        /// Set Identifier.
        /// </summary>
        /// <param name="identifier">A unique identifier. Peers must have matching identifiers to communicate.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Identifier was set.</returns>
        public bool SetIdentifier(string identifier)
        {
            var result = false;

            if (!IsLocked)
            {
                Identifier = identifier;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Local Address And Port.
        /// Can only be used if IsLocked is false.
        /// </summary>
        /// <param name="ipAddress">IPAddress to set to Local Address.</param>
        /// <param name="port">The port on which to listen.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Local Address and Port were set.</returns>
        public bool SetLocalAddressAndPort(IPAddress ipAddress, int port)
        {
            var result = false;

            if (!IsLocked)
            {
                LocalAddress = ipAddress;
                Port = port;
                BroadcastAddress = NetCommon.GetIpV4BroadcastAddress(ipAddress).Item2;

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Send Buffer Size.
        /// </summary>
        /// <param name="sendBufferSize">The receive buffer size.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Send Buffer was set.</returns>
        public bool SetSendBufferSize(int sendBufferSize)
        {
            var result = false;

            if (!IsLocked)
            {
                SendBufferSize = sendBufferSize;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Receive Buffer Size.
        /// </summary>
        /// <param name="receiveBufferSize">The receive buffer size.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Receive Buffer was set.</returns>
        public bool SetReceiveBufferSize(int receiveBufferSize)
        {
            var result = false;

            if (!IsLocked)
            {
                ReceiveBufferSize = receiveBufferSize;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Thread.
        /// </summary>
        /// <param name="thread">Intakes the Thread used by the NetPeer.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Thread was set.</returns>
        public bool SetThread(Thread thread)
        {
            var result = false;

            if (!IsLocked)
            {
                Thread = thread;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Maximum Connections.
        /// </summary>
        /// <param name="maximumConnections">The maximum connections allowed.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Maximum Connections were set.</returns>
        public bool SetMaximumConnections(int maximumConnections)
        {
            var result = false;

            if (!IsLocked)
            {
                MaximumConnections = maximumConnections;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Maximum MTU.
        /// Maximum Transmission Unit.
        /// </summary>
        /// <param name="maximumMtu">The maximum MTU size.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Maximum MTU was set.</returns>
        public bool SetMaximumMtu(int maximumMtu)
        {
            var result = false;

            if (!IsLocked)
            {
                MaximumMtu = maximumMtu;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Ping Interval.
        /// </summary>
        /// <param name="pingInterval">The amount of time between latency calculations.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Pint Interval was set.</returns>
        public bool SetPingInterval(float pingInterval)
        {
            var result = false;

            if (!IsLocked)
            {
                PingInterval = pingInterval;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Connection Time Out.
        /// </summary>
        /// <param name="connectionTimeOut">The time out period in which to drop connections if unreachable.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Connection Time Out was set.</returns>
        public bool SetConnectionTimeOut(int connectionTimeOut)
        {
            var result = false;

            if (!IsLocked)
            {
                ConnectionTimeOut = connectionTimeOut;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Reconnection Interval.
        /// </summary>
        /// <param name="reconnectionInterval">The time period in which to try reconnecting to peers.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Reconnection Interval was set.</returns>
        public bool SetReconnectionInterval(float reconnectionInterval)
        {
            var result = false;

            if (!IsLocked)
            {
                ReconnectionInterval = reconnectionInterval;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Maximum Reconnection Attempts
        /// </summary>
        /// <param name="maximumReconnectionAttempts">The maximum number of reconnection attempts when reconnecting to peers.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Maximum Reconnection Attempts were set.</returns>
        public bool SetMaximumReconnectionAttempts(int maximumReconnectionAttempts)
        {
            var result = false;

            if (!IsLocked)
            {
                MaximumReconnectionAttempts = maximumReconnectionAttempts;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Accepting Connections.
        /// </summary>
        /// <param name="isAcceptingConnections">A boolean indicating whether the peer is accepting connections.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the IsAcceptingConnections was set.</returns>
        public bool SetAcceptingConnections(bool isAcceptingConnections)
        {
            var result = false;

            if (!IsLocked)
            {
                IsAcceptingConnections = isAcceptingConnections;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set MTU Auto Expansion.
        /// </summary>
        /// <param name="isAutoExpandingMtu">A boolean indicating whether the MTU is auto expanding.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the IsAutoExpandingMtu was set.</returns>
        public bool SetMtuAutoExpansion(bool isAutoExpandingMtu)
        {
            var result = false;

            if (!IsLocked)
            {
                IsAutoExpandingMtu = isAutoExpandingMtu;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Maximum MTU Expansion Attempts
        /// </summary>
        /// <param name="maximumMtuExpansionAttempts">The maximum number of mtu expansion attempts.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Maximum MTU Expansion Attempts were set.</returns>
        public bool SetMaximumMtuExpansionAttempts(int maximumMtuExpansionAttempts)
        {
            var result = false;

            if (!IsLocked)
            {
                MaximumMtuExpansionAttempts = maximumMtuExpansionAttempts;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Set Mtu Expansion Interval.
        /// </summary>
        /// <param name="mtuExpansionInterval">The time period between MTU expansion attempts.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the MTU Expansion Interval was set.</returns>
        public bool SetMtuExpansionInterval(float mtuExpansionInterval)
        {
            var result = false;

            if (!IsLocked)
            {
                MtuExpansionInterval = mtuExpansionInterval;
                result = true;
            }

            return result;
        }
    }
}