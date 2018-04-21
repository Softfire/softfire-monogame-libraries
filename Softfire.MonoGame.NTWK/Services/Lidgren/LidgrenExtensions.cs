using Lidgren.Network;
using LidgrenNetPeer = Lidgren.Network.NetPeer;

namespace Softfire.MonoGame.NTWK.Services.Lidgren
{
    public static class LidgrenExtensions
    {
        #region Send Message

        /// <summary>
        /// Send Message To All Connected.
        /// Sends an message to all active connections.
        /// </summary>
        /// <param name="netPeer">Intakes a NetPeer or a class derived from NetPeer.</param>
        /// <param name="netOutMsg">Intakes a NetOutgoingMessage.</param>
        /// <param name="netDeliveryMethod">Intakes a NetDeliveryMethod describing how the message will be sent.</param>
        public static void SendMessageToAllConnected(this LidgrenNetPeer netPeer, NetOutgoingMessage netOutMsg, NetDeliveryMethod netDeliveryMethod)
        {
            foreach (var connection in netPeer.Connections)
            {
                netPeer.SendMessage(netOutMsg, connection, netDeliveryMethod);
            }
        }

        /// <summary>
        /// Send Message To All Others Connected.
        /// Sends an message to all other active connections.
        /// </summary>
        /// <param name="netPeer">Intakes a NetPeer or a class derived from NetPeer.</param>
        /// <param name="netIncMsg">Intakes a NetIncomingMessage.</param>
        /// <param name="netOutMsg">Intakes a NetOutgoingMessage.</param>
        /// <param name="netDeliveryMethod">Intakes a NetDeliveryMethod describing how the message will be sent.</param>
        public static void SendMessageToAllOthersConnected(this LidgrenNetPeer netPeer, NetIncomingMessage netIncMsg, NetOutgoingMessage netOutMsg, NetDeliveryMethod netDeliveryMethod)
        {
            foreach (var connection in netPeer.Connections)
            {
                if (netIncMsg.SenderConnection != connection)
                {
                    netPeer.SendMessage(netOutMsg, connection, netDeliveryMethod);
                }
            }
        }

        #endregion
    }
}