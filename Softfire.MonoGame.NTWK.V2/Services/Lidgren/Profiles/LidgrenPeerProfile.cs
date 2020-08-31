using System;
using System.Net;

namespace Softfire.MonoGame.NTWK.V2.Services.Lidgren.Profiles
{
    public abstract class LidgrenPeerProfile
    {
        /// <summary>
        /// Id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// IP Address.
        /// </summary>
        public IPAddress IpAddress { get; }

        /// <summary>
        /// Port.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// IP EndPoint.
        /// </summary>
        public IPEndPoint IpEndPoint { get; }

        /// <summary>
        /// Version.
        /// </summary>
        public double Version { get; }

        /// <summary>
        /// Ping.
        /// </summary>
        public double Ping { get; }

        /// <summary>
        /// Symmetric Encryption Key.
        /// </summary>
        public byte[] SymmetricEncryptionKey { get; set; }

        /// <summary>
        /// Asymmetric Encryption Public Key Modulus.
        /// </summary>
        public byte[] AsymmetricEncryptionPublicKeyModulus { get; set; }

        /// <summary>
        /// Asymmetric Encryption Public Key Exponent.
        /// </summary>
        public byte[] AsymmetricEncryptionPublicKeyExponent { get; set; }

        /// <summary>
        /// Refresh Attempts.
        /// </summary>
        public int RefreshAttempts { get; protected set; }

        /// <summary>
        /// Lidgren Peer Profile Constructor.
        /// </summary>
        /// <param name="id">A unique identifier. Intaken as a Guid.</param>
        /// <param name="name">The Peer's name. intaken as a string.</param>
        /// <param name="ipAddress">The Peer's IP Address.</param>
        /// <param name="port">The Peer's listening port. Intaken as an <see cref="int"/>.</param>
        /// <param name="version">The Peer's underlying version. Intaken as a <see cref="double"/>.</param>
        protected LidgrenPeerProfile(Guid id, string name, IPAddress ipAddress, int port, double version)
        {
            Id = id;
            Name = name;
            IpAddress = ipAddress;
            Port = port;
            Version = version;

            IpEndPoint = new IPEndPoint(IpAddress, Port);

            RefreshAttempts = 0;
        }

        /// <summary>
        /// Refresh.
        /// </summary>
        /// <returns>Returns a bool indicating whether Peer has been refeshed.</returns>
        public bool Refresh()
        {
            var result = true;

            RefreshAttempts++;

            if (RefreshAttempts >= 4)
            {
                result = false;
            }

            return result;
        }
    }
}