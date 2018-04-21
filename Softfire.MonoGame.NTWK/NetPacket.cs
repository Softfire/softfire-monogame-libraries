namespace Softfire.MonoGame.NTWK
{
    public abstract class NetPacket : INetPacket
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; }
    }
}