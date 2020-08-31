using System;
using System.Collections.Concurrent;

namespace Softfire.MonoGame.NTWK.V2
{
    /// <summary>
    /// Net Packet Pool.
    /// Used to pool packets of NetPacket for reusability to reduce garbage collection.
    /// </summary>
    /// <typeparam name="T">An object of Type NetPacket.</typeparam>
    public class NetPacketPool<T> : INetPacketPool<T> where T : NetPacket
    {
        /// <summary>
        /// Packets.
        /// A ConcurrentBag of Type T where T is of Type NetPacket.
        /// </summary>
        private ConcurrentBag<T> Packets { get; }

        /// <summary>
        /// Type T's new constructor method. () => NewPacket().
        /// </summary>
        private Func<T> PacketGenerator { get; }

        /// <summary>
        /// Net Packet Pool.
        /// Used to pool packets of NetPacket for reusability to reduce garbage collection.
        /// </summary>
        /// <param name="packetGenerator">Type T's new constructor method. () => NewPacket().</param>
        public NetPacketPool(Func<T> packetGenerator)
        {
            Packets = new ConcurrentBag<T>();
            PacketGenerator = packetGenerator;
        }

        /// <summary>
        /// Seed Pool.
        /// </summary>
        /// <param name="poolSize">The amount of object to seed the pool with.</param>
        public void SeedPool(int poolSize)
        {
            if (Packets.Count < poolSize)
            {
                for (var i = 0; i < poolSize - Packets.Count; i++)
                {
                    Packets.Add(PacketGenerator());
                }
            }
        }

        /// <summary>
        /// Get Packet.
        /// Retrieves an object of Type T, if one is available, otherwise a new object of Type T.
        /// </summary>
        /// <returns>Returns an object of Type T.</returns>
        public T GetPacket()
        {
            return Packets.TryTake(out var item) ? item : PacketGenerator();
        }

        /// <summary>
        /// Recylce Packet.
        /// Adds the Packet into the pool.
        /// </summary>
        /// <param name="packet">An object of Type T.</param>
        public void RecyclePacket(T packet)
        {
            Packets.Add(packet);
        }
    }
}