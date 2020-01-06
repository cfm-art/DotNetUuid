using System;
using System.Linq;
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

        [Fact]
        public void GenerateParallel()
        {
            var list = Enumerable.Range(1, 400).AsParallel()
                .Select(_ => CfmArt.Uuid.Uuid.V1.Generate())
                .Select(guid => Guid.Parse(guid.ToString()))
                .ToList();
            
            var dist = list.Distinct();
            Assert.Equal(dist.Count(), list.Count());

            // list.ForEach(guid => Console.WriteLine(guid));
        }
    }
}
