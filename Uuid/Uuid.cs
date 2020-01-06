using System;

namespace CfmArt.Uuid
{
    public class Uuid
    {
        public static Guid Timestamp() => V1.Generate();
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
