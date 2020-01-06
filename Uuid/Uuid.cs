using System;

namespace CfmArt.Uuid
{
    /// <summary>各種UUIDの生成</summary>
    public class Uuid
    {
        /// <summary>MACアドレスを取得</summary>
        public static void SetMacAddressProvider(IMacAddressProvider provider) => MacAddress = provider;

        internal static IMacAddressProvider MacAddress { get; private set; } = new MacAddress(true);

        /// <summary>タイムスタンプベース (version 1)</summary>
        public static Guid Timestamp() => V1.Generate();
        /// <summary>ランダムベース (version 4)</summary>
        public static Guid Random() => V4.Generate();


        public class V1
        {
            public static Guid Generate() => Impl.UuidV1.Generate();
        }

        public class V4
        {
            public static Guid Generate() => Guid.NewGuid();
        }
    }
}
