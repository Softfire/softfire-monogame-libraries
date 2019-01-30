using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Softfire.MonoGame.IO.Encryption;
using Softfire.MonoGame.LOG;
using Softfire.MonoGame.NTWK.Services.Lidgren.Profiles;
using LidgrenNetClient = Lidgren.Network.NetClient;
using LidgrenNetPeerStatus = Lidgren.Network.NetPeerStatus;

namespace Softfire.MonoGame.NTWK.Services.Lidgren
{
    public class LidgrenClient : LidgrenNetClient, ILidgrenPeer
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
        /// Client Id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Peer Server Profiles.
        /// </summary>
        public Dictionary<Guid, LidgrenPeerServerProfile> PeerServerProfiles { get; }

        /// <summary>
        /// Mode.
        /// Mode dictates how the client will communicate.
        /// Internet = LAN and Internet clients can connect.
        /// LAN = LAN only clients can connect.
        /// Default is LAN.
        /// </summary>
        public Modes Mode { get; set; }

        /// <summary>
        /// Modes.
        /// Mode dictates how the client will communicate.
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
        /// Client Version.
        /// </summary>
        public double Version { get; set; }

        #endregion

        /// <summary>
        /// Network Client Constructor.
        /// NetPeerConfiguration set to defaults.
        /// See LidgrenNetCommon.SetPeerConfiguration() for defaults.
        /// </summary>
        /// <param name="applicationIdentifier">Intakes a uniques string to identify the application. Used by clients and servers to connect. Default is Softfire.MonoGame.NTWK.</param>
        /// <param name="ipAddress">Intakes an IPAddress that the client will bind to and listen to server responses. Default is GetLocalNetworkIPs()[0], the first available IP.</param>
        /// <param name="port">Intakes a port number, as an int, to bind to. Default is 16462.</param>
        /// <param name="logFilePath">Intakes a file path for logs relative to the calling application.</param>
        public LidgrenClient(string applicationIdentifier, IPAddress ipAddress, int port, string logFilePath = @"Config\Logs\Client") : base(LidgrenNetCommon.SetNetPeerConfiguration(applicationIdentifier, ipAddress, port).Item2)
        {
            Id = NetCommon.GenerateUniqueId();
            Mode = Modes.LAN;
            Logger = new Logger(logFilePath);

            PeerServerProfiles = new Dictionary<Guid, LidgrenPeerServerProfile>();
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
        /// <param name="rsaContainerIdentifier">The RSA Container Name. This is a unique identifier used to call the RSA Container. Intaken as a <see cref="string"/>.</param>
        /// <param name="recipientHostName">The ipV4 address or hostname to connect to.</param>
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
        /// <param name="rsaContainerIdentifier">The RSA Container Name. This is a unique identifier used to call the RSA Container. Intaken as a <see cref="string"/>.</param>
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
                if (!NetCommon.IsPortValid(recipientIpEndPoint.Port))
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

                #region Send Message

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

        #region Encrypt and Send Data

        /// <summary>
        /// Send Encrypted Data.
        /// Encrypts and sends data to the indicated host using Symmetric Encryption.
        /// </summary>
        /// <param name="peerId">Recipient's Peer Id. Intaken as a Guid.</param>
        /// <param name="outgoingMessageTypes">A byte array of a message types. This array should be associated to a defined enum indicating the order and data to expect.</param>
        /// <param name="orderedPlainTextData">Data. Intaken as an IList{string}.</param>
        /// <param name="recipientHostName">Recipient's host name as a string.</param>
        /// <param name="recipientPort">Recipient's port number as an int.</param>
        /// <returns>Returns a result from SendEncryptedDataResults enum indicating whether the request was sent or an error occured.</returns>
        public async Task<SendEncryptedDataResults> SendEncryptedData(Guid peerId, byte[] outgoingMessageTypes, IList<string> orderedPlainTextData, string recipientHostName, int recipientPort)
        {
            try
            {
                var ipV4Address = NetCommon.ResolveHostNameToIpV4(recipientHostName)[0];
                var ipEndPoint = new IPEndPoint(ipV4Address, recipientPort);

                return await SendEncryptedData(peerId, outgoingMessageTypes, orderedPlainTextData, ipEndPoint);
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
                                                 $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"IP/Port: {recipientHostName}:{recipientPort}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.ExceptionThrownCheckLogs;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
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
        /// <param name="peerId">Recipient's Peer Id. Intaken as a Guid.</param>
        /// <param name="outgoingMessageTypes">A byte array of a message types. This array should be associated to a defined enum indicating the order and data to expect.</param>
        /// <param name="orderedPlainTextData">Data. Intaken as an IList{string}.</param>
        /// <param name="recipientIpEndPoint">Recipient's IPEndPoint.</param>
        /// <returns>Returns a result from SendEncryptedDataResults enum indicating whether the request was sent or an error occured.</returns>
        public async Task<SendEncryptedDataResults> SendEncryptedData(Guid peerId, byte[] outgoingMessageTypes, IList<string> orderedPlainTextData, IPEndPoint recipientIpEndPoint)
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

                // Confirm proper parsing of Id.
                if (peerId == Guid.Empty)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of data failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(peerId)}{Environment.NewLine}" +
                                                          $"Message: Peer ID ({peerId}) was parsed incorrectly.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.PeerIdIsEmpty;
                }

                // Check if Peer is already connected.
                if (!PeerServerProfiles.ContainsKey(peerId))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of data failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(peerId)}{Environment.NewLine}" +
                                                          $"Message: Peer ({peerId}) does not have a registered profile.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.PeerProfileWasNotFound;
                }

                // Gets the Peer Profile of the server.
                var peerServerProfile = PeerServerProfiles[peerId];

                // Check server Symmetric Encryption Key.
                if (peerServerProfile.SymmetricEncryptionKey == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of data failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(peerServerProfile.SymmetricEncryptionKey)}{Environment.NewLine}" +
                                                          $"Message: Peer ({recipientIpEndPoint.Address}:{recipientIpEndPoint.Port}) with Id ({peerId}) is missing a Symmetric Encryption Key.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return SendEncryptedDataResults.SymmetricKeyWasNotFound;
                }

                // Check Port validity.
                if (!NetCommon.IsPortValid(recipientIpEndPoint.Port))
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
                for (var index = 0; index < orderedPlainTextData.Count; index++)
                {
                    var plaintTextData = orderedPlainTextData[index];

                    // Check for null or wtitespace.
                    if (string.IsNullOrWhiteSpace(plaintTextData))
                    {
                        // Write to log.
                        Logger.Write(LogTypes.Warning, $"Receiving of data failed.{Environment.NewLine}" +
                                                              $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                              $"Variables: {nameof(plaintTextData)}{Environment.NewLine} = ({plaintTextData})" +
                                                              $"Message: Data was null or whitespace.{Environment.NewLine}",
                                                              useInlineLayout: false);

                        // Return failure code.
                        return SendEncryptedDataResults.PlainTextDataWasNullOrWhitespace;
                    }

                    // Encrypt text.
                    var cipherText = await IOSymmetricEncryption.EncryptAsync(plaintTextData, peerServerProfile.SymmetricEncryptionKey);

                    // Check for null or wtitespace.
                    if (string.IsNullOrWhiteSpace(cipherText))
                    {
                        // Write to log.
                        Logger.Write(LogTypes.Warning, $"Receiving of user data failed.{Environment.NewLine}" +
                                                              $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                              $"Variables: {nameof(cipherText)}{Environment.NewLine} = ({cipherText})" +
                                                              $"Message: Cipher text with index ({index}) was null or whitespace.{Environment.NewLine}",
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

                // Add defined message type detailing what to expect to message.
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
        /// <param name="senderHostName">The sender's host name as a string.</param>
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
                                                 $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                 $"Time: {DateTime.Now}{Environment.NewLine}" +
                                                 $"IP/Port: {senderHostName}:{senderPort}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code and null on failure.
                    return (ReceiveEncryptedDataResults.ExceptionThrownCheckLogs, null);
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
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
                if (!PeerServerProfiles.ContainsKey(peerId))
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

                // Check cipher text data list for null.
                if (orderedCipherTextData == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Processing of data failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(orderedCipherTextData)}{Environment.NewLine}" +
                                                   $"Message: Cipher text data object was null.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code and null on failure.
                    return (ReceiveEncryptedDataResults.CipherTextDataWasNullOrWhitespace, null);
                }

                // Gets the peer profile.
                var peerClientProfile = PeerServerProfiles[peerId];

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
                    return (ReceiveEncryptedDataResults.PeerProfileWasNotFound, null);
                }

                // Check Port validity.
                if (!NetCommon.IsPortValid(senderIpEndPoint.Port))
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
                                                       $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
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
                                                       $"Source: {nameof(SendEncryptedData)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(plainText)}{Environment.NewLine} = ({plainText})" +
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

                // Return success code and ordered plain text data.
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

                    // Return null on failure.
                    return (ReceiveEncryptedDataResults.ExceptionThrownCheckLogs, null);
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(ReceiveEncryptedData)}{Environment.NewLine}",
                                         useInlineLayout: false);

            // Return null on failure.
            return (ReceiveEncryptedDataResults.UnknownErrorOccuredCheckLogs, null);
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