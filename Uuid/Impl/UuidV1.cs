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

        static UuidV1()
        {
            Sequence_ = new Random().Next() & 0x3fff;
        }

        private UuidV1() {}


        static int Sequence
        {
            get
            {
                lock(LockObject)
                {
                    Sequence_ = (Sequence_ + 1) & 0x3fff;
                    return Sequence_;
                }
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
            var seq = Sequence;
            var mac = MacAddress.Value;

            return new Guid(
                (uint) (now & 0xffffffff),                      // 32bits
                (ushort) ((now >> 32) & 0xffff),                // 16bits
                (ushort) (((now >> 48) & 0x0fff) | 0x1000),     // 16bits
                (byte) ((seq >> 8) & 0x3f),       // 8bits (2/6)
                (byte) (seq & 0xff),       // 8bits
                mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);
        }
    }
}