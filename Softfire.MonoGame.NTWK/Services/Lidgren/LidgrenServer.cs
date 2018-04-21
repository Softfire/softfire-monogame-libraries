using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Softfire.MonoGame.IO.Encryption;
using Softfire.MonoGame.LOG;
using Softfire.MonoGame.NTWK.Services.Lidgren.Profiles;
using LidgrenNetServer = Lidgren.Network.NetServer;
using LidgrenNetPeerStatus = Lidgren.Network.NetPeerStatus;

namespace Softfire.MonoGame.NTWK.Services.Lidgren
{
    public class LidgrenServer : LidgrenNetServer, ILidgrenPeer
    {
        #region Fields and Properties

        /// <summary>
        /// Delta Time.
        /// Time since last update.
        /// </summary>
        protected double DeltaTime { get; set; }

        /// <summary>
        /// Elapsed Time.
        /// </summary>
        protected double ElapsedTime { get; set; }

        /// <summary>
        /// Server Id.
        /// </summary>
        public Guid Id { get; }
        
        /// <summary>
        /// Peer Client Profiles.
        /// </summary>
        public Dictionary<Guid, LidgrenPeerClientProfile> PeerClientProfiles { get; }

        /// <summary>
        /// Banned Ip Addresses.
        /// </summary>
        public Dictionary<IPAddress, NetBanIPAddress> BannedIpAddresses { get; }

        /// <summary>
        /// Mode.
        /// Mode dictates how the server will communicate.
        /// Internet = LAN and Internet clients can connect.
        /// LAN = LAN only clients can connect.
        /// Default is LAN.
        /// </summary>
        public Modes Mode { get; set; }

        /// <summary>
        /// Modes.
        /// Mode dictates how the server will communicate.
        /// Internet = LAN and Internet clients can connect.
        /// LAN = LAN only clients can connect.
        /// </summary>
        public enum Modes
        {
            Internet,
            LAN
        }

        /// <summary>
        /// Logger.
        /// </summary>
        protected internal Logger Logger { get; }

        /// <summary>
        /// Session Log.
        /// </summary>
        public List<KeyValuePair<LogTypes, string>> SessionLog { get; }

        /// <summary>
        /// Server Version.
        /// </summary>
        public double Version { get; set; }

        #endregion

        /// <summary>
        /// Network Server Constructor.
        /// NetPeerConfiguration set to defaults.
        /// See LidgrenNetCommon.SetPeerConfiguration() for defaults.
        /// </summary>
        /// <param name="applicationIdentifier">Intakes a uniques string to identify the application. Used by clients and servers to connect. Default is Softfire.MonoGame.NTWK.</param>
        /// <param name="ipAddress">Intakes an IPAddress that the client will bind to and listen to client responses. Default is GetLocalNetworkIPs()[0], the first available IP.</param>
        /// <param name="port">Intakes a port number, as an int, to bind to. Default is 16464.</param>
        /// <param name="logFilePath">Intakes a file path for logs relative to the calling application.</param>
        public LidgrenServer(string applicationIdentifier, IPAddress ipAddress, int port, string logFilePath = @"Config\Logs\Server") : base(LidgrenNetCommon.SetNetPeerConfiguration(applicationIdentifier, ipAddress, port).Item2)
        {
            Id = NetCommon.GenerateUniqueId();
            Mode = Modes.LAN;
            Logger = new Logger(logFilePath);

            SessionLog = new List<KeyValuePair<LogTypes, string>>();
            PeerClientProfiles = new Dictionary<Guid, LidgrenPeerClientProfile>();
            BannedIpAddresses = new Dictionary<IPAddress, NetBanIPAddress>();
        }

        #region Send Secure Connection Request

        /// <summary>
        /// Send Secure Conenction Request.
        /// Uses Asymmetric Encryption to create an initial handshake in which to setup a Symmetrically Encrypted Connection with the host.
        /// Sends the following:
        /// 1. Message Type as a byte.
        /// 2. Client Id (Guid) as a string.
        /// 3. RSA KeyParameter Modulus as a byte[].
        /// 4. RSA KeyParameter Exponent as a byte[].
        /// 5. Client Version as a double.
        /// </summary>
        /// <param name="outgoingMessageTypes">A byte array of a message types. This array should be associated to a defined enum indicating the order and data to expect.</param>
        /// <param name="rsaContainerIdentifier">The RSA Container Name. This is a unique identifier used to call the RSA Container. Intaken as a string.</param>
        /// <param name="recipientHostName">The ipV4 address or hostname to cconnect to.</param>
        /// <param name="recipientPort">The port number used to communicate with the host.</param>
        /// <returns>Returns an enum of SendSecureConnectionRequestResults.</returns>
        public SendSecureConnectionRequestResults SendSecureConnectionRequest(byte[] outgoingMessageTypes, string rsaContainerIdentifier, string recipientHostName, int recipientPort)
        {
            try
            {
                var ipV4Address = NetCommon.ResolveHostNameToIpV4(recipientHostName).First(address => address.AddressFamily == AddressFamily.InterNetwork);
                var ipEndPoint = new IPEndPoint(ipV4Address, recipientPort);

                return SendSecureConnectionRequest(outgoingMessageTypes, rsaContainerIdentifier, ipEndPoint);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is InvalidOperationException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is SocketException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(SendSecureConnectionRequest)}{Environment.NewLine}" +
                                                        $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                        $"IP/Port: {recipientHostName}:{recipientPort}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return SendSecureConnectionRequestResults.ExceptionThrownCheckLogs;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(SendSecureConnectionRequest)}{Environment.NewLine}" +
                                                $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                $"IP/Port: {recipientHostName}:{recipientPort}{Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return SendSecureConnectionRequestResults.UnknownErrorOccuredCheckLogs;

        }

        /// <summary>
        /// Send Secure Conenction Request.
        /// Uses Asymmetric Encryption to create an initial handshake in which to setup a Symmetrically Encrypted Connection with the host.
        /// Sends the following:
        /// 1. Message Type as a byte.
        /// 2. Client Id (Guid) as a string.
        /// 3. RSA KeyParameter Modulus as a byte[].
        /// 4. RSA KeyParameter Exponent as a byte[].
        /// 5. Client Version as a double.
        /// </summary>
        /// <param name="outgoingMessageTypes">A byte array of a message types. This array should be associated to a defined enum indicating the order and data to expect.</param>
        /// <param name="rsaContainerIdentifier">The RSA Container Name. This is a unique identifier used to call the RSA Container. Intaken as a string.</param>
        /// <param name="recipientIpEndPoint">Recipient IPEndPoint to connect to.</param>
        /// <returns>Returns an enum of SendSecureConnectionRequestResults.</returns>
        public SendSecureConnectionRequestResults SendSecureConnectionRequest(byte[] outgoingMessageTypes, string rsaContainerIdentifier, IPEndPoint recipientIpEndPoint)
        {
            try
            {
                #region Checks

                // Check if peer is running.
                if (Status != LidgrenNetPeerStatus.Running)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Sending of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SendSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(Status)}{Environment.NewLine}" +
                                                          $"Message: Peer was not running.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return SendSecureConnectionRequestResults.PeerIsNotRunning;
                }

                // Check ip address for ban.
                if (BannedIpAddresses.ContainsKey(recipientIpEndPoint.Address))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Sending of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SendSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(recipientIpEndPoint.Address)}{Environment.NewLine}" +
                                                          $"IP/Port: ({recipientIpEndPoint.Address}:{recipientIpEndPoint.Port}){Environment.NewLine}" +
                                                          $"Message: The sender's IP address is currently banned.{Environment.NewLine}",
                        useInlineLayout: false);

                    // Return failure code.
                    return SendSecureConnectionRequestResults.PeerIpAddressIsBanned;
                }

                // Check RSA Container Identifier for null or whitespace.
                if (string.IsNullOrWhiteSpace(rsaContainerIdentifier))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Sending of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SendSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(rsaContainerIdentifier)}{Environment.NewLine}" +
                                                          $"Message: RSA Container Identifier was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return SendSecureConnectionRequestResults.RsaContainerIdentifierIsNullOrWhitespace;
                }

                // Check Port validity.
                if (NetCommon.CheckPortValidity(recipientIpEndPoint.Port) == false)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Sending of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SendSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(recipientIpEndPoint.Port)}{Environment.NewLine}" +
                                                          $"Message: Port ({recipientIpEndPoint.Port}) was invalid.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return SendSecureConnectionRequestResults.PortIsInvalid;
                }

                #endregion

                #region Generate RSA KeyPair

                // Generate a new RSA Key Pair for communicating with the desired host.
                IOAsymmetricEncryption.GenerateAndStoreRsaKeyPair(rsaContainerIdentifier);
                var keyPair = IOAsymmetricEncryption.GetStoredRsaKeyPair(rsaContainerIdentifier);

                // Check RSA Key Pair Modulus length.
                if (keyPair.Modulus.Length == 0)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Sending of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SendSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(keyPair.Modulus)}{Environment.NewLine}" +
                                                          $"Message: RSA Key Pair Modulus was empty.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return SendSecureConnectionRequestResults.RsaKeyPairModulusLengthIsZero;
                }

                // Check RSA Key Pair Exponent length.
                if (keyPair.Exponent.Length == 0)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Sending of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SendSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(keyPair.Exponent)}{Environment.NewLine}" +
                                                          $"Message: RSA Key Pair Exponent was empty.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return SendSecureConnectionRequestResults.RsaKeyPairExponentLengthIsZero;
                }

                #endregion

                #region Send Request Message

                // Generate message.
                var netOuMsg = CreateMessage();

                // Add message type to message.
                netOuMsg.Write(outgoingMessageTypes);

                // Add peer id to message.
                netOuMsg.Write(Id.ToString());

                // Add Modulus to message.
                netOuMsg.Write(keyPair.Modulus);

                // Add Exponent to message.
                netOuMsg.Write(keyPair.Exponent);

                // Add peer version to message.
                netOuMsg.Write(Version);

                // Send message to recipient.
                SendUnconnectedMessage(netOuMsg, recipientIpEndPoint);

                #endregion

                return SendSecureConnectionRequestResults.RequestWasSent;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(SendSecureConnectionRequest)}{Environment.NewLine}" +
                                                 $"IP/Port: ({recipientIpEndPoint.Address}:{recipientIpEndPoint.Port}){Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return SendSecureConnectionRequestResults.ExceptionThrownCheckLogs;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(SendSecureConnectionRequest)}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return SendSecureConnectionRequestResults.UnknownErrorOccuredCheckLogs;
        }

        #endregion

        #region Receive Secure Connection Request

        /// <summary>
        /// Receive Secure Connection Request.
        /// Generates a symmetric encryption key for use between peers.
        /// </summary>
        /// <param name="peerId">Peer's id. Intaken as a Guid.</param>
        /// <param name="peerPublicKeyModulus">Peer's Asymmetric Encryption Public Key Modulus. Intaken as a byte[].</param>
        /// <param name="peerPublicKeyExponent">Peer's Asymmetric Encryption Public Key Exponent. Intaken as a byte[]</param>
        /// <param name="outgoingMessageType">A byte of a message type. This byte should be associated to a defined enum indicating what data to expect.</param>
        /// <param name="senderHostName">The sender's host name or IPv4 address as a string.</param>
        /// <param name="senderPort">The sender's port as an int.</param>
        /// <param name="peerVersion">Peer's version. Intaken as a double</param>
        /// <returns>Returns an enum of ReceiveSecureConnectionResults.</returns>
        public async Task<ReceiveSecureConnectionRequestResults> ReceiveSecureConnectionRequest(Guid peerId, byte[] peerPublicKeyModulus, byte[] peerPublicKeyExponent, byte outgoingMessageType, string senderHostName, int senderPort, double peerVersion)
        {
            try
            {
                var ipV4Address = NetCommon.ResolveHostNameToIpV4(senderHostName)[0];
                var ipEndPoint = new IPEndPoint(ipV4Address, senderPort);

                return await ReceiveSecureConnectionRequest(peerId, peerPublicKeyModulus, peerPublicKeyExponent, outgoingMessageType, ipEndPoint, peerVersion);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is SocketException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                        $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                        $"IP/Port: {senderHostName}:{senderPort}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return ReceiveSecureConnectionRequestResults.ExceptionThrownCheckLogs;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                $"IP/Port: {senderHostName}:{senderPort}{Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return ReceiveSecureConnectionRequestResults.UnknownErrorOccuredCheckLogs;
        }

        /// <summary>
        /// Receive Secure Connection Request.
        /// Generates a symmetric encryption key for use between peers.
        /// </summary>
        /// <param name="peerId">Peer's id. Intaken as a Guid.</param>
        /// <param name="peerPublicKeyModulus">Peer's Asymmetric Encryption Public Key Modulus. Intaken as a byte[].</param>
        /// <param name="peerPublicKeyExponent">Peer's Asymmetric Encryption Public Key Exponent. Intaken as a byte[]</param>
        /// <param name="outgoingMessageType">A byte of a message type. This byte should be associated to a defined enum indicating what data to expect.</param>
        /// <param name="senderIpEndPoint">Sender's IPEndPoint.</param>
        /// <param name="peerVersion">Peer's version. Intaken as a double</param>
        /// <returns>Returns an enum of ReceiveSecureConnectionResults.</returns>
        public async Task<ReceiveSecureConnectionRequestResults> ReceiveSecureConnectionRequest(Guid peerId, byte[] peerPublicKeyModulus, byte[] peerPublicKeyExponent, byte outgoingMessageType, IPEndPoint senderIpEndPoint, double peerVersion)
        {
            try
            {
                #region Checks

                // Check if peer is running.
                if (Status != LidgrenNetPeerStatus.Running)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(Status)}{Environment.NewLine}" +
                                                          $"Message: Peer was not running.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return ReceiveSecureConnectionRequestResults.PeerIsNotRunning;
                }

                // Check ip address for ban.
                if (BannedIpAddresses.ContainsKey(senderIpEndPoint.Address))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(senderIpEndPoint.Address)}{Environment.NewLine}" +
                                                          $"IP/Port: ({senderIpEndPoint.Address}:{senderIpEndPoint.Port}){Environment.NewLine}" +
                                                          $"Message: The sender's IP address is currently banned.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return ReceiveSecureConnectionRequestResults.PeerIpAddressIsBanned;
                }

                // Confirm proper parsing of peer id.
                if (peerId == Guid.Empty)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(peerId)}{Environment.NewLine}" +
                                                          $"Peer Id: ({peerId.ToString()}){Environment.NewLine}" +
                                                          $"Peer IP/Port: ({senderIpEndPoint.Address}:{senderIpEndPoint.Port}){Environment.NewLine}" +
                                                          $"Peer Version: ({peerVersion}){Environment.NewLine}" +
                                                          $"Message: Peer Id ({peerId}) was parsed incorrectly.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return ReceiveSecureConnectionRequestResults.PeerIdIsEmpty;
                }

                // Check client Public Key Modulus length.
                if (peerPublicKeyModulus.Length == 0)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(peerPublicKeyModulus)}{Environment.NewLine}" +
                                                          $"Peer Id: ({peerId.ToString()}){Environment.NewLine}" +
                                                          $"Peer IP/Port: ({senderIpEndPoint.Address}:{senderIpEndPoint.Port}){Environment.NewLine}" +
                                                          $"Peer Version: ({peerVersion}){Environment.NewLine}" +
                                                          $"Message: Peer RSA Public Key Modulus length is 0.{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return ReceiveSecureConnectionRequestResults.RsaKeyPairModulusLengthIsZero;
                }

                // Check client Public Key Exponent length.
                if (peerPublicKeyExponent.Length == 0)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of secure connection request failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(peerPublicKeyExponent)}{Environment.NewLine}" +
                                                          $"Peer Id: ({peerId.ToString()}){Environment.NewLine}" +
                                                          $"Peer IP/Port: ({senderIpEndPoint.Address}:{senderIpEndPoint.Port}){Environment.NewLine}" +
                                                          $"Peer Version: ({peerVersion}){Environment.NewLine}" +
                                                          $"Message: Peer RSA Public Key Exponent length is 0.{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return ReceiveSecureConnectionRequestResults.RsaKeyPairExponentLengthIsZero;
                }

                // Check for current peer profile.
                if (PeerClientProfiles.ContainsKey(peerId))
                {
                    Logger.Write(LogTypes.Info, $"Secure connection request denied.{Environment.NewLine}" +
                                                       $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(peerId)}{Environment.NewLine}" +
                                                       $"Peer Id: ({peerId.ToString()}){Environment.NewLine}" +
                                                       $"Peer IP/Port: ({senderIpEndPoint.Address}:{senderIpEndPoint.Port}){Environment.NewLine}" +
                                                       $"Peer Version: ({peerVersion}){Environment.NewLine}" +
                                                       $"Message: Peer is already connected.{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return ReceiveSecureConnectionRequestResults.PeerIsAlreadyConnected;
                }

                #endregion

                #region Process Request

                // Create New Peer Profile.
                var newPeer = new LidgrenPeerClientProfile(peerId, Configuration.AppIdentifier, senderIpEndPoint.Address, senderIpEndPoint.Port, peerVersion)
                {
                    AsymmetricEncryptionPublicKeyModulus = peerPublicKeyModulus,
                    AsymmetricEncryptionPublicKeyExponent = peerPublicKeyExponent,
                    SymmetricEncryptionKey = IOSymmetricEncryption.GenerateSymmetricKey()
                };

                // Add New Client.
                PeerClientProfiles.Add(peerId, newPeer);

                // Encrypt Encryption Key.
                var encryptedSymKey = await IOAsymmetricEncryption.EncryptAsync(
                    new RSAParameters
                    {
                        Modulus = newPeer.AsymmetricEncryptionPublicKeyModulus,
                        Exponent = newPeer.AsymmetricEncryptionPublicKeyExponent
                    }, newPeer.SymmetricEncryptionKey);

                // Check Encryption Key.
                if (encryptedSymKey == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Generation of Symmetric Encryption Key failed.{Environment.NewLine}" +
                                                        $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                        $"Variables: {nameof(encryptedSymKey)}{Environment.NewLine}" +
                                                        $"Client Id: {peerId.ToString()}{Environment.NewLine}" +
                                                        $"Client IP/Port: {senderIpEndPoint.Address}:{senderIpEndPoint.Port}{Environment.NewLine}" +
                                                        $"Client Version: {peerVersion}{Environment.NewLine}" +
                                                        $"Message: Encryption key generation failed.{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return ReceiveSecureConnectionRequestResults.SymmetricKeyWasNotFound;
                }

                #endregion

                #region Send Reply Message

                // Generate Message.
                var netOuMsg = CreateMessage();

                // Add message type to message.
                netOuMsg.Write(outgoingMessageType);

                // Add peer id to message.
                netOuMsg.Write(Id.ToString());

                // Add cipher text to message.
                netOuMsg.Write(encryptedSymKey);

                // Send reply message.
                SendUnconnectedMessage(netOuMsg, senderIpEndPoint);

                #endregion

                // Return success code.
                return ReceiveSecureConnectionRequestResults.RequestWasReceivedAndReplySent;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException ||
                    ex is FormatException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                        $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                        $"IP/Port: {senderIpEndPoint.Address}:{senderIpEndPoint.Port}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return ReceiveSecureConnectionRequestResults.ExceptionThrownCheckLogs;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                $"IP/Port: {senderIpEndPoint.Address}:{senderIpEndPoint.Port}{Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return ReceiveSecureConnectionRequestResults.UnknownErrorOccuredCheckLogs;
        }

        #endregion

        #region Encrypt and Send Data

        /// <summary>
        /// Send Encrypted Data.
        /// Encrypts and sends data to the indicated host using Symmetric Encryption.
        /// </summary>
        /// <param name="id">Recipient's id. Intaken as a Guid.</param>
        /// <param name="outgoingMessageTypes">A byte array of a message types. This array should be associated to a defined enum indicating the order and data to expect.</param>
        /// <param name="orderedPlainTextData">Data. Intaken as an IList{string}.</param>
        /// <param name="recipientHostName">Recipient's a host name. Intaken as a string.</param>
        /// <param name="recipientPort">Recipient's port. Intaken as an int.</param>
        /// <returns>Returns a result from SendEncryptedDataResults enum indicating whether the request was sent or an error occured.</returns>
        public async Task<SendEncryptedDataResults> SendEncryptedData(Guid id, byte[] outgoingMessageTypes, IList<string> orderedPlainTextData, string recipientHostName, int recipientPort)
        {
            try
            {
                var ipV4Address = NetCommon.ResolveHostNameToIpV4(recipientHostName)[0];
                var ipEndPoint = new IPEndPoint(ipV4Address, recipientPort);

                return await SendEncryptedData(id, outgoingMessageTypes, orderedPlainTextData, ipEndPoint);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is SocketException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"IP/Port: {recipientHostName}:{recipientPort}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.ExceptionThrownCheckLogs;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}" +
                                         $"IP/Port: {recipientHostName}:{recipientPort}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return SendEncryptedDataResults.UnknownErrorOccuredCheckLogs;
        }

        /// <summary>
        /// Send Encrypted Data.
        /// Encrypts and sends data to the indicated host using Symmetric Encryption.
        /// </summary>
        /// <param name="id">Recipient's id. Intaken as a Guid.</param>
        /// <param name="outgoingMessageTypes">A byte array of a message types. This array should be associated to a defined enum indicating the order and data to expect.</param>
        /// <param name="orderedPlainTextData">Data. Intaken as an IList{string}.</param>
        /// <param name="recipientIpEndPoint">Recipient's IPEndPoint.</param>
        /// <returns>Returns a result from SendEncryptedDataResults enum indicating whether the request was sent or an error occured.</returns>
        public async Task<SendEncryptedDataResults> SendEncryptedData(Guid id, byte[] outgoingMessageTypes, IList<string> orderedPlainTextData, IPEndPoint recipientIpEndPoint)
        {
            try
            {
                #region Checks

                // Check if client is running.
                if (Status != LidgrenNetPeerStatus.Running)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(Status)}{Environment.NewLine}" +
                                                   $"Message: Peer was not running.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.PeerIsNotRunning;
                }

                // Check ip address for ban.
                if (BannedIpAddresses.ContainsKey(recipientIpEndPoint.Address))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(recipientIpEndPoint.Address)}{Environment.NewLine}" +
                                                   $"IP/Port: ({recipientIpEndPoint.Address}:{recipientIpEndPoint.Port}){Environment.NewLine}" +
                                                   $"Message: The sender's IP address is currently banned.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.PeerIpAddressIsBanned;
                }

                // Confirm proper parsing of Id.
                if (id == Guid.Empty)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(id)}{Environment.NewLine}" +
                                                   $"Message: Peer ID ({id}) was parsed incorrectly.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.PeerIdIsEmpty;
                }

                // Check if Peer is already connected.
                if (PeerClientProfiles.ContainsKey(id) == false)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(id)}{Environment.NewLine}" +
                                                   $"Message: Peer ({id}) does not have a registered profile.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.PeerProfileWasNotFound;
                }

                // Gets the Peer Profile of the server.
                var peerServerProfile = PeerClientProfiles[id];

                // Check server Symmetric Encryption Key.
                if (peerServerProfile.SymmetricEncryptionKey == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(peerServerProfile.SymmetricEncryptionKey)}{Environment.NewLine}" +
                                                   $"Message: Peer ({recipientIpEndPoint.Address}:{recipientIpEndPoint.Port}) with Id ({id}) is missing a Symmetric Encryption Key.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.SymmetricKeyWasNotFound;
                }

                // Check Port validity.
                if (NetCommon.CheckPortValidity(recipientIpEndPoint.Port) == false)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of user data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(recipientIpEndPoint.Port)}{Environment.NewLine}" +
                                                   $"Message: Port ({recipientIpEndPoint.Port}) was invalid.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.PortIsInvalid;
                }

                #endregion

                #region Encrypt Data

                var cipherTextList = new List<string>();

                // Process data.
                foreach (var plainTextData in orderedPlainTextData)
                {
                    // Check for null or wtitespace.
                    if (string.IsNullOrWhiteSpace(plainTextData))
                    {
                        // Write to log.
                        Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                       $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(plainTextData)}{Environment.NewLine}" +
                                                       $"Message: Data was null or whitespace.{Environment.NewLine}",
                                                       useInlineLayout: false);

                        // Return failure code.
                        return SendEncryptedDataResults.PlainTextDataWasNullOrWhitespace;
                    }

                    // Encrypt text.
                    var cipherText = await IOSymmetricEncryption.EncryptAsync(plainTextData, peerServerProfile.SymmetricEncryptionKey);

                    // Check for null or wtitespace.
                    if (string.IsNullOrWhiteSpace(cipherText))
                    {
                        // Write to log.
                        Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                       $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(cipherText)}{Environment.NewLine}" +
                                                       $"Message: CipherText was null or whitespace.{Environment.NewLine}",
                                                       useInlineLayout: false);

                        // Return failure code.
                        return SendEncryptedDataResults.CipherTextDataWasNullOrWhitespace;
                    }

                    // Add encrypted text.
                    cipherTextList.Add(cipherText);
                }

                #endregion

                #region Send Data

                // Generate message.
                var netOuMsg = CreateMessage();

                // Add defined message types detailing what to expect when receiving the message.
                netOuMsg.Write(outgoingMessageTypes);

                // Add peer id to message.
                netOuMsg.Write(Id.ToString());

                // Add data to message.
                foreach (var cipherText in cipherTextList)
                {
                    netOuMsg.Write(cipherText);
                }

                // Send data.
                SendUnconnectedMessage(netOuMsg, recipientIpEndPoint);

                #endregion

                // Write to log.
                Logger.Write(LogTypes.Warning, $"Processed data successfully. Response sent using MessagetType: {outgoingMessageTypes}.{Environment.NewLine}" +
                                               $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}",
                                               useInlineLayout: false);

                // Return success code.
                return SendEncryptedDataResults.CipherTextDataSent;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                 $"IP/Port: ({recipientIpEndPoint.Address}:{recipientIpEndPoint.Port}){Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.ExceptionThrownCheckLogs;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return SendEncryptedDataResults.UnknownErrorOccuredCheckLogs;
        }

        #endregion

        #region Receive and Decrypt Data

        /// <summary> 
        /// Receive Encrypted Data.
        /// Decrypts data using Symmetric Encryption.
        /// </summary>
        /// <param name="peerId">Peer Id. Intaken as a Guid.</param>
        /// <param name="orderedCipherTextData">An object of Type IList{string} containing cipherText.</param>
        /// <param name="senderHostName">The sender's host name or IPv4 address as a string.</param>
        /// <param name="senderPort">The sender's port as an int.</param>
        /// <returns>Returns an enum of ReceiveEncryptedDataResults and an IDictionary{string, string} containing Plain Text data or null.</returns>
        public async Task<(ReceiveEncryptedDataResults, IDictionary<string, string>)> ReceiveEncryptedData(Guid peerId, IDictionary<string, string> orderedCipherTextData, string senderHostName, int senderPort)
        {
            try
            {
                var ipV4Address = NetCommon.ResolveHostNameToIpV4(senderHostName)[0];
                var ipEndPoint = new IPEndPoint(ipV4Address, senderPort);

                return await ReceiveEncryptedData(peerId, orderedCipherTextData, ipEndPoint);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is SocketException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"IP/Port: {senderHostName}:{senderPort}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code and null on failure.
                    return (ReceiveEncryptedDataResults.ExceptionThrownCheckLogs, null);
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(ReceiveSecureConnectionRequest)}{Environment.NewLine}" +
                                         $"Time: {DateTime.Now}{Environment.NewLine}" +
                                         $"IP/Port: {senderHostName}:{senderPort}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code and null on failure.
            return (ReceiveEncryptedDataResults.UnknownErrorOccuredCheckLogs, null);
        }

        /// <summary> 
        /// Receive Encrypted Data.
        /// Decrypts data using Symmetric Encryption.
        /// </summary>
        /// <param name="peerId">Peer Id. Intaken as a Guid.</param>
        /// <param name="orderedCipherTextData">An object of Type IList{string} containing cipherText.</param>
        /// <param name="senderIpEndPoint">Sender's IPEndPoint.</param>
        /// <returns>Returns an enum of ReceiveEncryptedDataResults and an IDictionary{string, string} containing Plain Text data or null.</returns>
        public async Task<(ReceiveEncryptedDataResults, IDictionary<string, string>)> ReceiveEncryptedData(Guid peerId, IDictionary<string, string> orderedCipherTextData, IPEndPoint senderIpEndPoint)
        {
            try
            {
                #region Checks

                // Check if client is running.
                if (Status != LidgrenNetPeerStatus.Running)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(Status)}{Environment.NewLine}" +
                                                   $"Message: Peer was not running.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null on failure.
                    return (ReceiveEncryptedDataResults.PeerIsNotRunning, null);
                }

                // Check ip address for ban.
                if (BannedIpAddresses.ContainsKey(senderIpEndPoint.Address))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(senderIpEndPoint.Address)}{Environment.NewLine}" +
                                                   $"IP/Port: ({senderIpEndPoint.Address}:{senderIpEndPoint.Port}){Environment.NewLine}" +
                                                   $"Message: The sender's IP address is currently banned.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return (ReceiveEncryptedDataResults.PeerIpAddressIsBanned, null);
                }

                // Check cipher text data list for null.
                if (orderedCipherTextData == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(orderedCipherTextData)}{Environment.NewLine}" +
                                                   $"Message: Cipher Text Data object was null.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null on failure.
                    return (ReceiveEncryptedDataResults.CipherTextDataWasNullOrWhitespace, null);
                }

                // Confirm proper parsing of peer id.
                if (peerId == Guid.Empty)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(peerId)}{Environment.NewLine}" +
                                                   $"Message: Id ({peerId}) was parsed incorrectly.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null on failure.
                    return (ReceiveEncryptedDataResults.PeerIdIsEmpty, null);
                }

                // Check if peer is already connected.
                if (PeerClientProfiles.ContainsKey(peerId) == false)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(peerId)}{Environment.NewLine}" +
                                                   $"Message: Peer ({peerId}) does not have a registered profile.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null on failure.
                    return (ReceiveEncryptedDataResults.PeerProfileWasNotFound, null);
                }

                // Gets the peer profile.
                var peerClientProfile = PeerClientProfiles[peerId];

                // Check peer Symmetric Encryption Key.
                if (peerClientProfile.SymmetricEncryptionKey == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(peerClientProfile.SymmetricEncryptionKey)}{Environment.NewLine}" +
                                                   $"Message: Peer ({senderIpEndPoint.Address}:{senderIpEndPoint.Port}) with Id ({peerId}) is missing a Symmetric Encryption Key.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null on failure.
                    return (ReceiveEncryptedDataResults.SymmetricKeyWasNotFound, null);
                }

                // Check Port validity.
                if (NetCommon.CheckPortValidity(senderIpEndPoint.Port) == false)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(senderIpEndPoint.Port)}{Environment.NewLine}" +
                                                   $"Message: Port ({senderIpEndPoint.Port}) was invalid.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null on failure.
                    return (ReceiveEncryptedDataResults.PortIsInvalid, null);
                }

                #endregion

                #region Process Data

                // Initialize returning list of data.
                IDictionary<string, string> orderedPlainTextData = new Dictionary<string, string>(orderedCipherTextData.Count);
                
                // Process data.
                foreach (var cipherTextData in orderedCipherTextData)
                {
                    // Check for null or whitespace.
                    if (string.IsNullOrWhiteSpace(cipherTextData.Value))
                    {
                        // Write to log.
                        Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                       $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(cipherTextData.Value)}{Environment.NewLine}" +
                                                       $"Message: Data was null or whitespace.{Environment.NewLine}",
                                                       useInlineLayout: false);

                        // Return failure code and null on failure.
                        return (ReceiveEncryptedDataResults.CipherTextDataWasNullOrWhitespace, null);
                    }

                    // Decrypt data.
                    var plainText = await IOSymmetricEncryption.DecryptAsync(cipherTextData.Value, peerClientProfile.SymmetricEncryptionKey);
                    
                    // Check for null or wtitespace.
                    if (string.IsNullOrWhiteSpace(plainText))
                    {
                        // Write to log.
                        Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                       $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(plainText)}{Environment.NewLine}" +
                                                       $"Message: Plain text with key ({cipherTextData.Key}) was null or whitespace.{Environment.NewLine}",
                                                       useInlineLayout: false);

                        // Return failure code and null on failure.
                        return (ReceiveEncryptedDataResults.PlainTextDataWasNullOrWhitespace, null);
                    }

                    // Add decrypted data.
                    orderedPlainTextData.Add(cipherTextData.Key, plainText);
                }

                #endregion

                // Write to log.
                Logger.Write(LogTypes.Info, $"Processed data successfully.{Environment.NewLine}" +
                                            $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                            $"Variables: {nameof(orderedPlainTextData)}{Environment.NewLine}",
                                            useInlineLayout: false);

                // Return result.
                return (ReceiveEncryptedDataResults.PlainTextDataReceived, orderedPlainTextData);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                 $"IP/Port: ({senderIpEndPoint.Address}:{senderIpEndPoint.Port}){Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code and null on failure.
                    return (ReceiveEncryptedDataResults.ExceptionThrownCheckLogs, null);
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code and null on failure.
            return (ReceiveEncryptedDataResults.UnknownErrorOccuredCheckLogs, null);
        }

        #endregion

        #region IP Commands

        /// <summary>
        /// Add IP Address Ban.
        /// </summary>
        /// <param name="ipAddress">The ip address to ban.</param>
        /// <param name="reason">The reason for the ban. Intaken as a string.</param>
        /// <param name="dateTime">The date/time of the ban. Intaken as DateTime.</param>
        /// <param name="expiryDateTime">The expiry date/time of the ban. Intaken as DateTime.</param>
        /// <returns>Returns a bool indicating whether the ip was added.</returns>
        public bool AddIpAddressBan(IPAddress ipAddress, string reason, DateTime dateTime, DateTime expiryDateTime)
        {
            try
            {
                // Check IPAddress for ban.
                if (CheckForIpAddressBan(ipAddress))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"IP Address ban failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddIpAddressBan)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(ipAddress)}{Environment.NewLine}" +
                                                          $"IP Address: {ipAddress}{Environment.NewLine}" +
                                                          $"Message: IP address is already banned.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return false;
                }

                // Check reason for null or whitespace.
                if (string.IsNullOrWhiteSpace(reason))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"IP Address ban failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddIpAddressBan)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(reason)}{Environment.NewLine}" +
                                                          $"IP Address: {ipAddress}{Environment.NewLine}" +
                                                          $"Message: Reason was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return false;
                }

                // Ban IP Address
                BannedIpAddresses.Add(ipAddress, new NetBanIPAddress(ipAddress, reason, dateTime, expiryDateTime));

                // Write to log.
                Logger.Write(LogTypes.Info, $"IP Address banned successfully.{Environment.NewLine}" +
                                                   $"Source: {nameof(AddIpAddressBan)}{Environment.NewLine}" +
                                                   $"Time: {dateTime}{Environment.NewLine}" +
                                                   $"Expiry: {expiryDateTime}{Environment.NewLine}" +
                                                   $"IP Address: {ipAddress} was banned.{Environment.NewLine}" +
                                                   $"Reason: {reason}{Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return true;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(AddIpAddressBan)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return false;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(AddIpAddressBan)}{Environment.NewLine}" +
                                                $"Time: {dateTime}{Environment.NewLine}" +
                                                $"Expiry: {expiryDateTime}{Environment.NewLine}" +
                                                $"IP Address: {ipAddress}{Environment.NewLine}" +
                                                $"Reason: {reason}{Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return false;
        }

        /// <summary>
        /// Check For IP Address Ban.
        /// </summary>
        /// <param name="ipAddress">The ip address to check.</param>
        /// <returns>Returns a bool indicating whether the ip is banned.</returns>
        public bool CheckForIpAddressBan(IPAddress ipAddress)
        {
            try
            {
                // Check for IPAddress ban.
                return BannedIpAddresses.ContainsKey(ipAddress);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(CheckForIpAddressBan)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return false;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(CheckForIpAddressBan)}{Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return false;
        }

        /// <summary>
        /// Remove IP Address Ban.
        /// </summary>
        /// <param name="ipAddress">The IPAddress to unban.</param>
        /// <returns>Returns a bool indicating whether the ip was removed.</returns>
        public bool RemoveIpAddressBan(IPAddress ipAddress)
        {
            try
            {
                // Check IPAddress for ban.
                if (CheckForIpAddressBan(ipAddress) == false)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"IP Address ban removal failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveIpAddressBan)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(ipAddress)}{Environment.NewLine}" +
                                                          $"IP Address: {ipAddress}{Environment.NewLine}" +
                                                          $"Message: IP address was not found.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return false;
                }

                // Remove IPAddress ban.
                BannedIpAddresses.Remove(ipAddress);

                // Write to log.
                Logger.Write(LogTypes.Info, $"IP Address ban removal succeeded.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveIpAddressBan)}{Environment.NewLine}" +
                                                   $"IP Address: {ipAddress}{Environment.NewLine}" +
                                                   $"Message: IP address was unbanned.{Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return true;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(RemoveIpAddressBan)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return false;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(RemoveIpAddressBan)}{Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return false;
        }

        #endregion

        #region Write To Logs

        /// <summary>
        /// Write To Logs.
        /// Writes to Session Log and Logger.
        /// </summary>
        /// <param name="logType">Type of log. Info, Error, Warning. Intaken as a string.</param>
        /// <param name="line">The line to log. Intaken as a string.</param>
        /// <param name="fileName">Intakes a file name as a string. Leave off extension. Default name is LOG and extension is .log.</param>
        /// <param name="useInlineLayout">Logger Only: Write to log in line.</param>
        public void WriteToLogs(LogTypes logType, string line, string fileName = "LOG", bool useInlineLayout = true)
        {
            SessionLog.Add(new KeyValuePair<LogTypes, string>(logType, $" - {line}"));
            Logger.Write(logType, line, fileName, useInlineLayout);
        }

        /// <summary>
        /// Write To Logs.
        /// Writes to Session Log and Logger.
        /// </summary>
        /// <param name="logType">Type of log. Info, Error, Warning. Intaken as a string.</param>
        /// <param name="lines">The lines to log. Intaken as an array of strings.</param>
        /// <param name="fileName">Intakes a file name as a string. Leave off extension. Default name is LOG and extension is .log.</param>
        /// <param name="useInlineLayout">Logger Only: Write to log in line.</param>
        public void WriteToLogs(LogTypes logType, string[] lines, string fileName = "LOG", bool useInlineLayout = true)
        {
            foreach (var line in lines)
            {
                SessionLog.Add(new KeyValuePair<LogTypes, string>(logType, $" - {line}"));
            }

            Logger.Write(logType, lines, fileName, useInlineLayout);
        }

        #endregion

        /// <summary>
        /// Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public virtual void Update(GameTime gameTime)
        {
            ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}