using System;

namespace CfmArt.Uuid
{
    public class Uuid
    {
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
