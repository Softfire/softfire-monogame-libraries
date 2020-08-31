using System;
using System.Net;

namespace Softfire.MonoGame.NTWK.V2
{
    public sealed class NetBanIPAddress : NetBan
    {
        /// <summary>
        /// IP Address to Ban.
        /// </summary>
        public IPAddress IpAddress { get; }
        
        /// <summary>
        /// Lobby Ban.
        /// </summary>
        /// <param name="ipAddress">IPAddress to ban.</param>
        /// <param name="reason">Reason for ban.</param>
        /// <param name="dateTime">Date/Time of ban.</param>
        /// <param name="expiryDateTime">Expiry Date/Time for ban.</param>
        public NetBanIPAddress(IPAddress ipAddress, string reason, DateTime dateTime = new DateTime(), DateTime expiryDateTime = new DateTime()) : base(reason, dateTime, expiryDateTime)
        {
            IpAddress = ipAddress;
        }
    }
}