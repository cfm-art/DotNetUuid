using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using CfmArt.Uuid;

namespace Impl
{
    // Uuid Version 1.
    internal class UuidV1
    {
        /// <summary>被った時のシーケンス番号</summary>
        private static int Sequence_ { get; set; }
        /// <summary>シーケンス番号用の排他</summary>
        private static readonly object LockObject = new UuidV1();
        /// <summary>UUID v1のTickの基準となる日時</summary>
        private static readonly DateTimeOffset BaseTime = new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.FromHours(0));

        /// <summary>被り判定用</summary>
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
            var mac = Uuid.MacAddress.Value;
            var now = (DateTimeOffset.UtcNow - BaseTime).Ticks;
            var seq = Sequence(now);

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
            
            guid[10] = mac.B1;
            guid[11] = mac.B2;
            guid[12] = mac.B3;
            guid[13] = mac.B4;
            guid[14] = mac.B5;
            guid[15] = mac.B6;
            return new Guid(guid);
        }
    }
}
