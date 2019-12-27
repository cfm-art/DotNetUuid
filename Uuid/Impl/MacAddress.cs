using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Impl
{
    internal class MacAddress
    {
        public static byte[] Value { get => Address.Value; }
        
        private static readonly Lazy<byte[]> Address = new Lazy<byte[]>(GetMacAddress);

        private static byte[] GetMacAddress()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            var up = interfaces.FirstOrDefault(i => i.OperationalStatus != OperationalStatus.Up);
            if (!(up is null))
            {
                return up.GetPhysicalAddress().GetAddressBytes();
            }
            else if (interfaces.Any())
            {
                return interfaces.First().GetPhysicalAddress().GetAddressBytes();
            }
            var random = new byte[6];
            new Random().NextBytes(random);
            return random;
        }
    }
}