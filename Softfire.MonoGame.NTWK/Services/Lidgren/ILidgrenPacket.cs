using Lidgren.Network;

namespace Softfire.MonoGame.NTWK.Services.Lidgren
{
    interface ILidgrenPacket
    {
        /// <summary>
        /// Write To Packet.
        /// Use to write data into a NetOutgoingMessage in the Lidgren Library.
        /// </summary>
        /// <param name="netOutMsg">A NetOutgoingMessage. Data will be written to this message.</param>
        void WriteToPacket(NetOutgoingMessage netOutMsg);

        /// <summary>
        /// Read From Packet.
        /// Used to read data from a NetIncomingMessage in the Lidgren Library.
        /// </summary>
        /// <param name="netIncMsg">A NetIncomingMessage. Data will be read from this message.</param>
        void ReadFromPacket(NetIncomingMessage netIncMsg);
    }
}