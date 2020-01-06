using System;
using Xunit;

namespace Uuid
{
    public class UuidV1Test
    {
        [Fact]
        public void Generate()
        {
            // とりあえず目視確認。。。
            for (int i = 0; i < 4; ++i)
            {
                var v1 = CfmArt.Uuid.Uuid.V1.Generate();
                Console.WriteLine(v1.ToString());
                Guid.Parse(v1.ToString());
            }
        }
    }
}
