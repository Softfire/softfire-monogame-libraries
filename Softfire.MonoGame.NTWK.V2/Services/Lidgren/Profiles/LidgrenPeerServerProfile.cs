using System;
using System.Net;

namespace Softfire.MonoGame.NTWK.V2.Services.Lidgren.Profiles
{
    public sealed class LidgrenPeerServerProfile : LidgrenPeerProfile
    {
        /// <summary>
        /// Is Private?
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Number of Connected Peers.
        /// Current.
        /// </summary>
        public int NumberOfConnectedPeers { get; }

        /// <summary>
        /// Maximum Number of Connected Peers.
        /// </summary>
        public int MaxNumberOfConnectedPeers { get; }

        /// <summary>
        /// Lidgren Peer Server Profile Constructor.
        /// </summary>
        /// <param name="id">A unique identifier. Intaken as a byte array.</param>
        /// <param name="name">The server's name. intaken as a string.</param>
        /// <param name="ipAddress">The server's IP Address.</param>
        /// <param name="port">The server's listening port. Intaken as an <see cref="int"/>.</param>
        /// <param name="version">The server's underlying version. Intaken as a <see cref="double"/>.</param>
        /// <param name="numberOfConnectedPeers">The number of connected peers for the server. Intaken as an <see cref="int"/>.</param>
        /// <param name="maxNumberOfConnectedPeers">The max number of connected peers for the server. Intaken as an <see cref="int"/>.</param>
        /// <param name="isPrivate">A bool indicting whether the server is private.</param>
        public LidgrenPeerServerProfile(Guid id, string name, IPAddress ipAddress, int port, double version, int numberOfConnectedPeers = 0, int maxNumberOfConnectedPeers = 32, bool isPrivate = false) : base(id, name, ipAddress, port, version)
        {
            NumberOfConnectedPeers = numberOfConnectedPeers;
            MaxNumberOfConnectedPeers = maxNumberOfConnectedPeers;
            IsPrivate = isPrivate;
        }
    }
}