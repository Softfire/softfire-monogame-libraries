using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Softfire.MonoGame.NTWK.Services.Lidgren
{
    /// <summary>
    /// Secure Connection Request Results.
    /// </summary>
    public enum SendSecureConnectionRequestResults
    {
        PeerIsNotRunning,
        PeerIpAddressIsBanned,
        RsaContainerIdentifierIsNullOrWhitespace,
        PortIsInvalid,
        RsaKeyPairModulusLengthIsZero,
        RsaKeyPairExponentLengthIsZero,
        RequestWasSent,
        ExceptionThrownCheckLogs,
        UnknownErrorOccuredCheckLogs
    }

    /// <summary>
    /// Receive Secure Connection Request Results.
    /// </summary>
    public enum ReceiveSecureConnectionRequestResults
    {
        PeerIsNotRunning,
        PeerIpAddressIsBanned,
        PeerIdIsEmpty,
        RsaKeyPairModulusLengthIsZero,
        RsaKeyPairExponentLengthIsZero,
        PeerIsAlreadyConnected,
        SymmetricKeyWasNotFound,
        RequestWasReceivedAndReplySent,
        ExceptionThrownCheckLogs,
        UnknownErrorOccuredCheckLogs
    }

    /// <summary>
    /// Encrypted Data Results.
    /// </summary>
    public enum SendEncryptedDataResults
    {
        PeerIsNotRunning,
        PeerIpAddressIsBanned,
        PeerIdIsEmpty,
        PeerProfileWasNotFound,
        SymmetricKeyWasNotFound,
        PortIsInvalid,
        PlainTextDataWasNullOrWhitespace,
        CipherTextDataWasNullOrWhitespace,
        CipherTextDataSent,
        ExceptionThrownCheckLogs,
        UnknownErrorOccuredCheckLogs
    }

    /// <summary>
    /// Receive Encrypted Data Results.
    /// </summary>
    public enum ReceiveEncryptedDataResults
    {
        PeerIsNotRunning,
        PeerIpAddressIsBanned,
        PeerIdIsEmpty,
        PeerProfileWasNotFound,
        SymmetricKeyWasNotFound,
        PortIsInvalid,
        PlainTextDataWasNullOrWhitespace,
        CipherTextDataWasNullOrWhitespace,
        PlainTextDataReceived,
        ExceptionThrownCheckLogs,
        UnknownErrorOccuredCheckLogs
    }

    /// <summary>
    /// ILidgrenPeer Interface.
    /// </summary>
    public interface ILidgrenPeer
    {
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
        /// <param name="recipientHostName">The ipV4 address or hostname to connect to.</param>
        /// <param name="recipientPort">The port number used to communicate with the host.</param>
        /// <returns>Returns an enum of SecureConnectionRequestResults.</returns>
        SendSecureConnectionRequestResults SendSecureConnectionRequest(byte[] outgoingMessageTypes, string rsaContainerIdentifier, string recipientHostName, int recipientPort);

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
        /// <returns>Returns an enum of SecureConnectionRequestResults.</returns>
        SendSecureConnectionRequestResults SendSecureConnectionRequest(byte[] outgoingMessageTypes, string rsaContainerIdentifier, IPEndPoint recipientIpEndPoint);

        /// <summary>
        /// Send Encrypted Data.
        /// Encrypts and sends data to the indicated host using Symmetric Encryption.
        /// </summary>
        /// <param name="peerId">Recipient's Peer Id. Intaken as a Guid.</param>
        /// <param name="outgoingMessageTypes">A byte array of a message types. This array should be associated to a defined enum indicating the order and data to expect.</param>
        /// <param name="orderedPlainTextData">Data. Intaken as an IList{string}. Sent in order by index.</param>
        /// <param name="recipientHostName">Recipient's host name as a string.</param>
        /// <param name="recipientPort">Recipient's port number as an int.</param>
        /// <returns>Returns an enum of EncryptedDataResults.</returns>
        Task<SendEncryptedDataResults> SendEncryptedData(Guid peerId, byte[] outgoingMessageTypes, IList<string> orderedPlainTextData, string recipientHostName, int recipientPort);

        /// <summary>
        /// Send Encrypted Data.
        /// Encrypts and sends data to the indicated host using Symmetric Encryption.
        /// </summary>
        /// <param name="peerId">Recipient's Peer Id. Intaken as a Guid.</param>
        /// <param name="outgoingMessageTypes">A byte array of a message types. This array should be associated to a defined enum indicating the order and data to expect.</param>
        /// <param name="orderedPlainTextData">Data. Intaken as an IList{string}. Sent in order by index.</param>
        /// <param name="recipientIpEndPoint">Recipient IPEndPoint.</param>
        /// <returns>Returns an enum of EncryptedDataResults.</returns>
        Task<SendEncryptedDataResults> SendEncryptedData(Guid peerId, byte[] outgoingMessageTypes, IList<string> orderedPlainTextData, IPEndPoint recipientIpEndPoint);

        /// <summary> 
        /// Receive Encrypted Data.
        /// Decrypts data using Symmetric Encryption.
        /// </summary>
        /// <param name="peerId">Peer Id. Intaken as a Guid.</param>
        /// <param name="orderedCipherTextData">An object of Type IList{string} containing cipherText.</param>
        /// <param name="senderHostName">The sender's host name as a string.</param>
        /// <param name="senderPort">The sender's port as an int.</param>
        /// <returns>Returns an enum of ReceiveEncryptedDataResults and an IList{string} containing Plain Text data or null if an error occured.</returns>
        Task<(ReceiveEncryptedDataResults, IDictionary<string, string>)> ReceiveEncryptedData(Guid peerId, IDictionary<string, string> orderedCipherTextData, string senderHostName, int senderPort);

        /// <summary> 
        /// Receive Encrypted Data.
        /// Decrypts data using Symmetric Encryption.
        /// </summary>
        /// <param name="peerId">Peer Id. Intaken as a Guid.</param>
        /// <param name="orderedCipherTextData">An object of Type IList{string} containing cipherText.</param>
        /// <param name="senderIpEndPoint">Sender's IPEndPoint.</param>
        /// <returns>Returns an enum of ReceiveEncryptedDataResults and an IList{string} containing Plain Text data or null if an error occured.</returns>
        Task<(ReceiveEncryptedDataResults, IDictionary<string, string>)> ReceiveEncryptedData(Guid peerId, IDictionary<string, string> orderedCipherTextData, IPEndPoint senderIpEndPoint);
    }
}