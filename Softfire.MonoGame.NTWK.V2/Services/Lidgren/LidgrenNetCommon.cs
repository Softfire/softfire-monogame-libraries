using System;
using System.Net;
using System.Net.Sockets;
using LidgrenNetPeerConfiguration = Lidgren.Network.NetPeerConfiguration;

namespace Softfire.MonoGame.NTWK.V2.Services.Lidgren
{
    public static class LidgrenNetCommon
    {
        public enum SetNetPeerConfigurationResults
        {
            Failure,
            Success,
            DeniedApplicationIdentifierNullOrWhitespace,
            DeniedIpAddressNull,
            DeniedPortOutOfRange
        }

        /// <summary>
        /// Sets the NetPeerConfiguration used with a NetServer object.
        /// </summary>
        /// <param name="applicationIdentifier">Intakes the Application Identifier to define the NetServer/NetClient/NetPeer object and must be unique. Sets the thread name for the application as well. Connecting clients Application Identifiers must match this one to connect.</param>
        /// <param name="ipAddressToBind">Intakes an IPAddress to bind to.</param>
        /// <param name="port">Intakes a Port, as an int, that will be used for communication. Must be between 1025 and 65535.</param>
        /// <param name="maximumConnections">Intakes the maximum number of connections the server can accept. Default is 32. Clients can use the default.</param>
        /// <param name="receiveBufferSize">Intakes the Receive Buffer Size as an int. Default is 131071. Tweak for performance but default is good for most.</param>
        /// <param name="sendBufferSize">Intakes the Send Buffer Size as an int. Default is 131071. Tweak for performance but default is good for most.</param>
        /// <param name="connectionTimeOutInSeconds">Intakes an int to define the connection time out length. Default is 60 seconds.</param>
        /// <returns>Returns a new NetPeerConfiguration object to be used with a LidgrenServer or LidgrenClient.</returns>
        public static (SetNetPeerConfigurationResults, LidgrenNetPeerConfiguration) SetNetPeerConfiguration(string applicationIdentifier, IPAddress ipAddressToBind, int port,
                                                                                                            int maximumConnections = 32, int receiveBufferSize = 131071, int sendBufferSize = 131071,
                                                                                                            int connectionTimeOutInSeconds = 60)
        {
            try
            {
                #region Checks

                // Check for null of whitespace.
                if (!string.IsNullOrWhiteSpace(applicationIdentifier))
                {
                    // Write to log.
                    Logger.Write(@"Config\Logs\PeerConfiguration", LogTypes.Error, $"NetPeerConfiguration was not set.{Environment.NewLine}" +
                                                                                   $"Source: {nameof(SetNetPeerConfiguration)}{Environment.NewLine}" +
                                                                                   $"Variables: {nameof(applicationIdentifier)}{Environment.NewLine}" +
                                                                                   $"Message: Application Identifier was null or whitespace.{Environment.NewLine}",
                                                                                   useInlineLayout: false);

                    // Return failure code and null.
                    return (SetNetPeerConfigurationResults.DeniedApplicationIdentifierNullOrWhitespace, null);
                }

                // Check if null
                if (ipAddressToBind == null)
                {
                    // Write to log.
                    Logger.Write(@"Config\Logs\PeerConfiguration", LogTypes.Error, $"NetPeerConfiguration was not set.{Environment.NewLine}" +
                                                                                   $"Source: {nameof(SetNetPeerConfiguration)}{Environment.NewLine}" +
                                                                                   $"Variables: {nameof(ipAddressToBind)}{Environment.NewLine}" +
                                                                                   $"Message: IP Address was null.{Environment.NewLine}",
                                                                                   useInlineLayout: false);

                    // Return failure code and null.
                    return (SetNetPeerConfigurationResults.DeniedIpAddressNull, null);
                }

                // Check if in range.
                if (!NetCommon.IsPortValid(port))
                {
                    // Write to log.
                    Logger.Write(@"Config\Logs\PeerConfiguration", LogTypes.Error, $"NetPeerConfiguration was not set.{Environment.NewLine}" +
                                                                                   $"Source: {nameof(SetNetPeerConfiguration)}{Environment.NewLine}" +
                                                                                   $"Variables: {nameof(ipAddressToBind)}{Environment.NewLine}" +
                                                                                   $"Message: Port ({port}) was out of range.{Environment.NewLine}",
                                                                                   useInlineLayout: false);

                    // Return failure code and null.
                    return (SetNetPeerConfigurationResults.DeniedPortOutOfRange, null);
                }

                #endregion

                return (SetNetPeerConfigurationResults.Success, new LidgrenNetPeerConfiguration(applicationIdentifier)
                {
                    NetworkThreadName = $"{applicationIdentifier} - Network Thread",
                    MaximumConnections = maximumConnections,
                    ReceiveBufferSize = receiveBufferSize,
                    SendBufferSize = sendBufferSize,
                    LocalAddress = ipAddressToBind,
                    Port = port,
                    ConnectionTimeout = connectionTimeOutInSeconds,
                    UseMessageRecycling = true
                });
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is SocketException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(@"Config\Logs\PeerConfiguration", LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                                                   $"Source: {nameof(SetNetPeerConfiguration)}{Environment.NewLine}" +
                                                                                   $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                                                   $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code and null.
                    return (SetNetPeerConfigurationResults.Failure, null);
                }
            }

            // Write to log.
            Logger.Write(@"Config\Logs\PeerConfiguration", LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                                           $"Source: {nameof(SetNetPeerConfiguration)}{Environment.NewLine}" +
                                                                           $"Time: {DateTime.Now}{Environment.NewLine}",
                                                                           useInlineLayout: false);

            // Return failure code and null.
            return (SetNetPeerConfigurationResults.Failure, null);
        }
    }
}