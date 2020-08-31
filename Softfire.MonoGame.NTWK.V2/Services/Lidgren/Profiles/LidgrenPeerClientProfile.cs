using System;
using System.Net;

namespace Softfire.MonoGame.NTWK.V2.Services.Lidgren.Profiles
{
    public sealed class LidgrenPeerClientProfile : LidgrenPeerProfile
    {
        /// <summary>
        /// Lidgren Peer Client Profile Constructor.
        /// </summary>
        /// <param name="id">A unique identifier. Intaken as a byte array.</param>
        /// <param name="name">The client's name. intaken as a string.</param>
        /// <param name="ipAddress">The client's IP Address.</param>
        /// <param name="port">The client's listening port. Intaken as an <see cref="int"/>.</param>
        /// <param name="version">The client's underlying version. Intaken as a <see cref="double"/>.</param>
        public LidgrenPeerClientProfile(Guid id, string name, IPAddress ipAddress, int port, double version) : base(id, name, ipAddress, port, version)
        {

        }
    }
}