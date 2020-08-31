using System;

namespace Softfire.MonoGame.NTWK.V2.Lobby
{
    public sealed class LobbyBanText : NetBan
    {
        /// <summary>
        /// Text to Ban.
        /// </summary>
        public string Text { get; }

        public LobbyBanText(string text, string reason, DateTime dateTime = new DateTime(), DateTime expiryDateTime = new DateTime()) : base(reason, dateTime, expiryDateTime)
        {
            Text = text;
        }
    }
}