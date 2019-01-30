using System;
using System.Collections.Generic;

namespace Softfire.MonoGame.NTWK
{
    public class NetPacketManager
    {
        /// <summary>
        /// Packets.
        /// Contains a dictionary of registered packet types.
        /// </summary>
        private Dictionary<int, Type> Packets { get; }

        /// <summary>
        /// Packet Pools.
        /// Contains a dictionary of registered packet type pools.
        /// </summary>
        private Dictionary<Type, object> PacketPools { get; }

        /// <summary>
        /// Net Packet Manager.
        /// Used to register and provide access to packets.
        /// </summary>
        public NetPacketManager()
        {
            Packets = new Dictionary<int, Type>();
            PacketPools = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Register.
        /// Registers the packet type and creates a pool.
        /// </summary>
        /// <typeparam name="T">The packet type to register.</typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Register<T>() where T : NetPacket, new()
        {
            if (!PacketPools.ContainsKey(typeof(T)))
            {
                PacketPools.Add(typeof(T), new NetPacketPool<T>(() => new T()));
            }
        }

        /// <summary>
        /// Register.
        /// Registers the packet type and creates a pool.
        /// </summary>
        /// <typeparam name="T">The packet type to register.</typeparam>
        /// <param name="id">The id of the packet type to register.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Register<T>(int id) where T : NetPacket, new()
        {
            if (!Packets.ContainsKey(id) &&
                !PacketPools.ContainsKey(typeof(T)))
            {
                Packets.Add(id, typeof(T));
                PacketPools.Add(typeof(T), new NetPacketPool<T>(() => new T()));
            }
        }

        /// <summary>
        /// Register.
        /// Registers the packet type and creates a pool.
        /// </summary>
        /// <typeparam name="T">The packet type to register.</typeparam>
        /// <param name="id">The id of the packet type to register.</param>
        /// <param name="poolSize">The amount of objects to seed the pool with.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Register<T>(int id, int poolSize) where T : NetPacket, new()
        {
            if (!Packets.ContainsKey(id) &&
                !PacketPools.ContainsKey(typeof(T)))
            {
                Packets.Add(id, typeof(T));
                PacketPools.Add(typeof(T), new NetPacketPool<T>(() => new T()));
                SeedPool<T>(poolSize);
            }
        }

        /// <summary>
        /// Seed Pool.
        /// </summary>
        /// <typeparam name="T">The packet type to seed.</typeparam>
        /// <param name="poolSize">The amount of objects to seed the pool with.</param>
        public void SeedPool<T>(int poolSize) where T : NetPacket
        {
            if (PacketPools.ContainsKey(typeof(T)))
            {
                var pool = (NetPacketPool<T>)PacketPools[typeof(T)];
                pool.SeedPool(poolSize);
            }
        }

        /// <summary>
        /// Get Packet.
        /// Retrieves an object of Type T, if one is available, otherwise a new object of Type T.
        /// </summary>
        /// <typeparam name="T">The packet type to retrieve.</typeparam>
        /// <returns>Returns a packet of Type T.</returns>
        public T GetPacket<T>() where T : NetPacket, new()
        {
            if (!PacketPools.ContainsKey(typeof(T)))
            {
                Register<T>();
            }

            var pool = (NetPacketPool<T>)PacketPools[typeof(T)];
            return pool.GetPacket();
        }

        /// <summary>
        /// Get Packet.
        /// Retrieves an object of Type T, if one is available, otherwise a new object of Type T.
        /// </summary>
        /// <typeparam name="T">The packet type to retrieve.</typeparam>
        /// <param name="id">The packet id in which to retrieve.</param>
        /// <returns>Returns a packet of Type T.</returns>
        public T GetPacket<T>(int id) where T : NetPacket, new()
        {
            if (!PacketPools.ContainsKey(typeof(T)))
            {
                Register<T>(id);
            }

            var pool = (NetPacketPool<T>)PacketPools[typeof(T)];
            return pool.GetPacket();
        }

        /// <summary>
        /// Recylce Packet.
        /// Returns the packet back into the pool.
        /// </summary>
        /// <param name="packet">An object of Type T.</param>
        public void RecylePacket<T>(T packet) where T : NetPacket
        {
            var pool = (NetPacketPool<T>)PacketPools[typeof(T)];
            pool.RecyclePacket(packet);
        }
    }
}