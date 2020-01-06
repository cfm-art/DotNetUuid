
namespace CfmArt.Uuid
{
    public struct MacAddressBytes
    {
        public byte B1;
        public byte B2;
        public byte B3;
        public byte B4;
        public byte B5;
        public byte B6;

        public MacAddressBytes(
            byte b1, byte b2, byte b3,
            byte b4, byte b5, byte b6)
        {
            B1 = b1;
            B2 = b2;
            B3 = b3;
            B4 = b4;
            B5 = b5;
            B6 = b6;
        }

        public static implicit operator MacAddressBytes(byte[] bytes)
        {
            if (bytes.Length < 6) {
                throw new System.ArgumentException("Mac Address is required 6 octets.");
            }
            return new MacAddressBytes(
                bytes[0],
                bytes[1],
                bytes[2],
                bytes[3],
                bytes[4],
                bytes[5]);
        }
    }

    public interface IMacAddressProvider
    {
        MacAddressBytes Value { get; }
    }
}