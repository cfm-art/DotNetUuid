using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace CfmArt.Uuid
{
    /// <summary>端末のMACアドレスを取得</summary>
    internal class MacAddress
        : CfmArt.Uuid.IMacAddressProvider
    {
        public MacAddressBytes Value { get => Address.Value; }
        
        private readonly Lazy<MacAddressBytes> Address;

        /// <summary>MACアドレスをそのまま利用するかハッシュ化するか</summary>
        internal bool Hashed = true;

        public MacAddress(bool hashed)
        {
            Hashed = hashed;
            Address = new Lazy<MacAddressBytes>(GetMacAddress);            
        }

        private MacAddressBytes GetMacAddress()
        {
            byte[] real = GetRealMacAddress();
            if (Hashed)
            {
                using var sha = new SHA1CryptoServiceProvider();
                var fake = sha.ComputeHash(real);
                fake[0] = 1;
                return fake;
            }
            return real;
        }

        /// <summary>MACアドレスの内最初の１つを取得</summary>
        private byte[] GetRealMacAddress()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            var up = interfaces.FirstOrDefault(i => i.OperationalStatus == OperationalStatus.Up);
            if (!(up is null))
            {
                // 稼働しているI/Fの最初
                var bytes = up.GetPhysicalAddress().GetAddressBytes();
                if (bytes.Any()) { return bytes; }
            }

            if (interfaces.Any())
            {
                // 稼働がないので適当に1つ
                var bytes = interfaces.Select(i => i.GetPhysicalAddress().GetAddressBytes())
                    .Where(m => m.Length > 0)
                    .FirstOrDefault();
                if (bytes != null) { return bytes; }
            }

            // I/Fが無いので乱数
            var random = new byte[6];
            new Random().NextBytes(random);
            random[0] = 1;
            return random;
        }
    }
}