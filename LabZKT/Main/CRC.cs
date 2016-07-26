namespace LabZKT
{
    public class CRC
    {
        const uint polynomial = 0x04C11DB7;
        private static uint[] table = new uint[256];

        public static uint ComputeChecksum(byte[] bytes)
        {
            uint crc = 0;
            for (var i = 0; i < bytes.Length; i++)
                crc = (crc >> 8) ^ table[bytes[i] ^ crc & 0xff];
            return crc;
        }
        static CRC()
        {
            for (uint i = 0; i < 256; i++)
            {
                uint value = i;
                for (uint j = 0; j < 8; j++)
                    if ((value & 1) == 1)
                        value = (value >> 1) ^ polynomial;
                    else
                        value >>= 1;
                table[i] = value;
            }
        }
    }
}
