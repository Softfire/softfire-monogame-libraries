using Lidgren.Network;

namespace Softfire.MonoGame.NTWK.Services.Lidgren
{
    public abstract class LidgrenPacket : NetPacket, ILidgrenPacket
    {
        /// <summary>
        /// Write To Packet.
        /// Use to write data into a NetOutgoingMessage in the Lidgren Library.
        /// </summary>
        /// <param name="netOutMsg">A NetOutgoingMessage. Data will be written to this message.</param>
        public abstract void WriteToPacket(NetOutgoingMessage netOutMsg);

        /// <summary>
        /// Read From Packet.
        /// Used to read data from a NetIncomingMessage in the Lidgren Library.
        /// </summary>
        /// <param name="netIncMsg">A NetIncomingMessage. Data will be read from this message.</param>
        public abstract void ReadFromPacket(NetIncomingMessage netIncMsg);
    }
}