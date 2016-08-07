namespace LabZKT.StaticClasses
{
    public static class CRC
    {
        /// <summary>
        /// CRC-32 polynomal value used in computations
        /// </summary>
        const uint polynomial = 0x04C11DB7;
        /// <summary>
        /// Table with check value for each block of data
        /// </summary>
        private static uint[] table = new uint[256];
        /// <summary>
        /// Computes checksum for given block of data.
        /// </summary>
        /// <param name="bytes">Block of data</param>
        /// <returns>Check value/sum for given block of data</returns>
        public static uint ComputeChecksum(byte[] bytes)
        {
            uint crc = 0;
            for (var i = 0; i < bytes.Length; i++)
                crc = (crc >> 8) ^ table[bytes[i] ^ crc & 0xff];
            return crc;
        }
        /// <summary>
        /// Static constructor called only once. Fills 'table' with check sums.
        /// </summary>
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
