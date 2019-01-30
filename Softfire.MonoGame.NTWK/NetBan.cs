using System;

namespace Softfire.MonoGame.NTWK
{
    public abstract class NetBan
    {
        /// <summary>
        /// Reason for Ban.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Date/Time of Ban.
        /// </summary>
        public DateTime DateTime { get; protected set; }
        
        /// <summary>
        /// Ban Expiry Date/Time.
        /// </summary>
        public DateTime ExpiryDateTime { get; protected set; }

        /// <summary>
        /// Lobby Ban.
        /// </summary>
        /// <param name="reason">The reason for the ban. Intaken as a <see cref="string"/>.</param>
        /// <param name="dateTime">The DateTime the ban occured.</param>
        /// <param name="expiryDateTime">The DateTime of when the ban expires.</param>
        protected NetBan(string reason, DateTime dateTime = new DateTime(), DateTime expiryDateTime = new DateTime())
        {
            Reason = reason;
            DateTime = dateTime;
            ExpiryDateTime = expiryDateTime;
        }
    }
}