using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Impl
{
    // Uuid Version 1.
    internal class UuidV1
    {
        private static int Sequence_ { get; set; }
        private static readonly object LockObject = new UuidV1();
        private static readonly DateTimeOffset BaseTime = new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.FromHours(0));

        private static long OldTicks = 0L;

        static UuidV1()
        {
            Sequence_ = new Random().Next() & 0x3fff;
        }

        private UuidV1() {}


        static int Sequence(long tick)
        {
            lock(LockObject)
            {
                long old = OldTicks;
                OldTicks = tick;
                if (tick <= old)
                {
                    Sequence_ = (Sequence_ + 1) & 0x3fff;
                    return Sequence_;
                }
                return Sequence_;
            }
        }

        public static Guid Generate()
        {
            // 32bits: time low
            // 16bits: time mid
            // 4bits : 1
            // 12bits: time high
            // 2bits:  0?
            // 14bits: sequence
            //      2bits: 0
            //      6bits: seq
            //      8bits: seq
            // 48bits: mac-address
            var now = (DateTimeOffset.UtcNow - BaseTime).Ticks;
            var seq = Sequence(now);
            var mac = MacAddress.Value;

            byte[] guid = new byte[16];
            guid[0] = (byte) ((now >> 0) & 0xff);
            guid[1] = (byte) ((now >> 8) & 0xff);
            guid[2] = (byte) ((now >> 16) & 0xff);
            guid[3] = (byte) ((now >> 24) & 0xff);

            guid[4] = (byte) ((now >> 32) & 0xff);
            guid[5] = (byte) ((now >> 40) & 0xff);

            guid[6] = (byte) ((now >> 48) & 0xff);
            guid[7] = (byte) (((now >> 56) & 0x0f) | 0x10);

            guid[8] = (byte) ((seq >> 8) & 0x3f);
            guid[9] = (byte) (seq & 0xff);
            
            guid[10] = mac[0];
            guid[11] = mac[1];
            guid[12] = mac[2];
            guid[13] = mac[3];
            guid[14] = mac[4];
            guid[15] = mac[5];
            return new Guid(guid);
        }
    }
}
