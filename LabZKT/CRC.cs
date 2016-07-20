using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LabZKT
{
    public class CRC
    {
        const UInt32 polynomial = 0x04C11DB7;
        private static UInt32[] table = new UInt32[256];

        public static UInt32 ComputeChecksum(byte[] bytes)
        {
            UInt32 crc = 0;
            for (var i = 0; i < bytes.Length; i++)
                crc = (crc >> 8) ^ table[bytes[i] ^ crc & 0xff];
            return crc;
        }
        static CRC()
        {
            for (UInt32 i = 0; i < 256; i++)
            {
                UInt32 value = i;
                for (UInt32 j = 0; j < 8; j++)
                    if ((value & 1) == 1)
                        value = (value >> 1) ^ polynomial;
                    else
                        value >>= 1;
                table[i] = value;
            }
        }
    }
}
