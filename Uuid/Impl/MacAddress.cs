using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace Impl
{
    internal class MacAddress
    {
        public static byte[] Value { get => Address.Value; }
        
        private static readonly Lazy<byte[]> Address = new Lazy<byte[]>(GetMacAddress);

        private static byte[] GetMacAddress()
        {
            byte[] real = GetRealMacAddress();
            using var sha = new SHA1CryptoServiceProvider();
            var fake = sha.ComputeHash(real);
            fake[0] = 1;
            return fake;
        }

        private static byte[] GetRealMacAddress()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            var up = interfaces.FirstOrDefault(i => i.OperationalStatus == OperationalStatus.Up);
            if (!(up is null))
            {
                var bytes = up.GetPhysicalAddress().GetAddressBytes();
                if (bytes.Any()) { return bytes; }
            }

            if (interfaces.Any())
            {
                var bytes = interfaces.Select(i => i.GetPhysicalAddress().GetAddressBytes())
                    .Where(m => m.Length > 0)
                    .FirstOrDefault();
                if (bytes != null) { return bytes; }
            }

            var random = new byte[6];
            new Random().NextBytes(random);
            random[0] = 1;
            return random;
        }
    }
}