namespace Softfire.MonoGame.NTWK.V2
{
    public interface INetPacketPool<T>
    {
        /// <summary>
        /// Seed Pool.
        /// </summary>
        /// <param name="poolSize">The amount of object to seed the pool with.</param>
        void SeedPool(int poolSize);

        /// <summary>
        /// Get Packet.
        /// Retrieves an object of Type T, if one is available, otherwise a new object of Type T.
        /// </summary>
        /// <returns>Returns an object of Type T.</returns>
        T GetPacket();

        /// <summary>
        /// Recylce Packet.
        /// Adds the Packet into the pool.
        /// </summary>
        /// <param name="packet">An object of Type T.</param>
        void RecyclePacket(T packet);
    }
}