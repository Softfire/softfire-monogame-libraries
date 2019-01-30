using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Softfire.MonoGame.NTWK
{
    public static class NetCommon
    {
        /// <summary>
        /// Get Network Interface Results.
        /// </summary>
        public enum GetNetworkInterfaceResults
        {
            Failure,
            Success,
            NoAvailableAdaptersFoundOnSystem,
            NoOperationalAdaptersFoundOnSystem
        }

        /// <summary>
        /// Get Subnet Mask Results.
        /// </summary>
        public enum GetSubnetMaskResults
        {
            Failure,
            Success
        }

        /// <summary>
        /// Get Broadcast Address Results.
        /// </summary>
        public enum GetBroadcastAddressResults
        {
            Failure,
            Success,
            IpAddressDoesNotMatchSubnetMask
        }

        /// <summary>
        /// Get Local Ipv4 Addresses.
        /// </summary>
        /// <returns>Returns an IEnumerable{IPAddress}.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<IPAddress> GetLocalIpv4Addresses()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();
        }

        /// <summary>
        /// Get Local Ipv6 Addresses.
        /// </summary>
        /// <returns>Returns an IEnumerable{IPAddress}.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<IPAddress> GetLocalIpv6Addresses()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetworkV6).ToList();
        }

        /// <summary>
        /// Get IPv4 Broadcast Address.
        /// </summary>
        /// <param name="ipv4Address">IPAddress to get the subnet mask of. IPv4.</param>
        /// <returns>Returns and enum of GetBroadcastAddressResults and an IPAddress or null.</returns>
        /// <exception cref="NetworkInformationException">This method throws a NetworkInformationException if System.NetworkInterface.GetAllNetworkInterfaces() fails to find any adapters.</exception>
        public static (GetBroadcastAddressResults, IPAddress) GetIpV4BroadcastAddress(IPAddress ipv4Address)
        {
            try
            {
                var ipAdressBytes = ipv4Address.GetAddressBytes();
                var subnetMaskBytes = GetIPv4SubnetMask(ipv4Address).Item2.GetAddressBytes();

                #region Checks

                // Check if given ip address is within the subnet.
                if (ipAdressBytes.Length != subnetMaskBytes.Length)
                {
                    // Return failure code and null.
                    return (GetBroadcastAddressResults.IpAddressDoesNotMatchSubnetMask, null);
                }

                #endregion

                // Initialize broadcastAddress byte array.
                var broadcastAddress = new byte[ipAdressBytes.Length];

                // Build address.
                for (var i = 0; i < broadcastAddress.Length; i++)
                {
                    broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
                }

                // Return success code and Boradcast Address.
                return (GetBroadcastAddressResults.Success, new IPAddress(broadcastAddress));
            }
            catch (Exception ex)
            {
                if (ex is NetworkInformationException)
                {
                    throw;
                }
            }

            // Return failure code and null.
            return (GetBroadcastAddressResults.Failure, null);
        }

        /// <summary>
        /// Get IPv4 Subnet Mask.
        /// </summary>
        /// <param name="ipv4Address">IPAddress to get the subnet mask of. IPv4.</param>
        /// <returns>Returns and enum of GetSubnetMaskResults and an IPAddress or null.</returns>
        /// <exception cref="NetworkInformationException">This method throws a NetworkInformationException if System.NetworkInterface.GetAllNetworkInterfaces() fails to find any adapters.</exception>
        public static (GetSubnetMaskResults, IPAddress) GetIPv4SubnetMask(IPAddress ipv4Address)
        {
            try
            {
                // Get available network adapters.
                var availableAdapters = GetNetworkInterfaces();
                
                #region Checks

                // Check if adapters were found on the system.
                if (availableAdapters.Item1 != GetNetworkInterfaceResults.Success)
                {
                    // Return failure code and null.
                    return (GetSubnetMaskResults.Failure, null);
                }

                #endregion

                // Find subnet mask of given ip address.
                foreach (var adapter in availableAdapters.Item2)
                {
                    foreach (var unicastAddress in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (ipv4Address.Equals(unicastAddress.Address) &&
                            ipv4Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return (GetSubnetMaskResults.Success, unicastAddress.IPv4Mask);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is NetworkInformationException)
                {
                    throw;
                }
            }

            // Return failure code and null.
            return (GetSubnetMaskResults.Failure, null);
        }

        /// <summary>
        /// Get MAC Address Bytes.
        /// </summary>
        /// <param name="networkInterface">The NetworkInterface in which to get in bytes.</param>
        /// <returns>Returns a byte array of the MAC address of the supplied NetworkInterface.</returns>
        public static byte[] GetMacAddressBytes(NetworkInterface networkInterface)
        {
            return networkInterface.GetPhysicalAddress().GetAddressBytes();
        }

        /// <summary>
        /// Get Netowrk Interfaces.
        /// </summary>
        /// <returns>Returns an enum of GetNetworkInterfaceResults and an IEnumerable of available network adapters otherwise null</returns>
        /// <exception cref="NetworkInformationException">This method throws a NetworkInformationException if System.NetworkInterface.GetAllNetworkInterfaces() fails to find any adapters.</exception>
        private static (GetNetworkInterfaceResults, IEnumerable<NetworkInterface>) GetNetworkInterfaces()
        {
            try
            {
                var adapters = NetworkInterface.GetAllNetworkInterfaces();

                #region Checks

                // Check if an adapter is available.
                if (adapters.Length < 1)
                {
                    // Return failure code and null.
                    return (GetNetworkInterfaceResults.NoAvailableAdaptersFoundOnSystem, null);
                }

                #endregion

                // IEnumerable of acative adapters.
                var activeAdapters = new List<NetworkInterface>();

                // Iterate available adapters for suitable selection.
                foreach (var adapter in adapters)
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up &&
                        adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                        adapter.NetworkInterfaceType != NetworkInterfaceType.Unknown &&
                        (adapter.Supports(NetworkInterfaceComponent.IPv4) || adapter.Supports(NetworkInterfaceComponent.IPv6)))
                    {
                        var adapterProperties = adapter.GetIPProperties();

                        foreach (var unicastAddress in adapterProperties.UnicastAddresses)
                        {
                            if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork ||
                                unicastAddress.Address.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                activeAdapters.Add(adapter);
                            }
                        }
                    }
                }

                #region Checks

                // Check if a suitable adapter was found.
                if (activeAdapters.Count == 0)
                {
                    // Return failure code and null.
                    return (GetNetworkInterfaceResults.NoOperationalAdaptersFoundOnSystem, null);
                }

                #endregion

                // Return success code and IEnumerable of available adapters.
                return (GetNetworkInterfaceResults.Success, activeAdapters);
            }
            catch (Exception ex)
            {
                if (ex is NetworkInformationException)
                {
                    throw;
                }
            }

            // Return failure code.
            return (GetNetworkInterfaceResults.Failure, null);
        }

        /// <summary>
        /// Resolve Host Name To IpV4.
        /// Resolves the host name to the associated IPAddresses.
        /// </summary>
        /// <param name="hostname">The host name to resolve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns an IEnumerable{IPAddress} of found addresses or null if hostname was unable to be resolved.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IList<IPAddress> ResolveHostNameToIpV4(string hostname)
        {
            return string.IsNullOrWhiteSpace(hostname) ? null : Dns.GetHostEntry(hostname).AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();
        }

        /// <summary>
        /// Resolve Host Name To IpV6.
        /// Resolves the host name to the associated IPAddresses.
        /// </summary>
        /// <param name="hostname">The host name to resolve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns an IEnumerable{IPAddress} of found addresses or null if hostname was unable to be resolved.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IList<IPAddress> ResolveHostNameToIpV6(string hostname)
        {
            return string.IsNullOrWhiteSpace(hostname) ? null : Dns.GetHostEntry(hostname).AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetworkV6).ToList();
        }

        /// <summary>
        /// Determines whether the port falls within the range of greater than 1024 and less than or equal to 65535.
        /// </summary>
        /// <param name="port">Intakes a port number as an int.</param>
        /// <returns>Returns a bool indicating whether the port is within range.</returns>
        public static bool IsPortValid(int port)
        {
            return port > 1024 && port <= 65535;
        }

        /// <summary>
        /// Generate Unique Id.
        /// </summary>
        /// <returns>Returns a unique Id as a Guid.</returns>
        public static Guid GenerateUniqueId()
        {
            return Guid.NewGuid();
        }
    }
}