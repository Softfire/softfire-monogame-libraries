namespace Softfire.MonoGame.NTWK.V2
{
    public abstract class NetPacket : INetPacket
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; }
    }
}