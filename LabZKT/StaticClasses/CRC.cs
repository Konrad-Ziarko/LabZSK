namespace LabZSK.StaticClasses
{
    /// <summary>
    /// Class used for various CRC operations
    /// </summary>
    public static class CRC
    {
        /// <summary>
        /// CRC-32 polynomal value used in computations
        /// </summary>
        private const uint pmPolynomial = 0x04C11DB7;
        private const uint paoPolynomial = 0xEDB88320;
        private const uint logPolynomial = 0x82608EDB;
        /// <summary>
        /// Table with check value for each block of data
        /// </summary>
        private static uint[] pmTable = new uint[256];
        private static uint[] paoTable = new uint[256];
        private static uint[] logTable = new uint[256];
        /// <summary>
        /// Computes checksum for given block of data.
        /// </summary>
        /// <param name="bytes">Block of data to compute</param>
        /// <returns>Check value/sum for given block of data</returns>
        public static uint ComputePMChecksum(byte[] bytes)
        {
            uint crc = 0;
            for (var i = 0; i < bytes.Length; i++)
                crc = (crc >> 8) ^ pmTable[bytes[i] ^ crc & 0xff];
            return crc;
        }
        public static uint ComputePAOChecksum(byte[] bytes)
        {
            uint crc = 0;
            for (var i = 0; i < bytes.Length; i++)
                crc = (crc >> 8) ^ paoTable[bytes[i] ^ crc & 0xff];
            return crc;
        }
        public static uint ComputeLogChecksum(byte[] bytes)
        {
            uint crc = 0;
            for (var i = 0; i < bytes.Length; i++)
                crc = (crc >> 8) ^ logTable[bytes[i] ^ crc & 0xff];
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
                        value = (value >> 1) ^ pmPolynomial;
                    else
                        value >>= 1;
                pmTable[i] = value;
            }

            for (uint i = 0; i < 256; i++)
            {
                uint value = i;
                for (uint j = 0; j < 8; j++)
                    if ((value & 1) == 1)
                        value = (value >> 1) ^ paoPolynomial;
                    else
                        value >>= 1;
                paoTable[i] = value;
            }

            for (uint i = 0; i < 256; i++)
            {
                uint value = i;
                for (uint j = 0; j < 8; j++)
                    if ((value & 1) == 1)
                        value = (value >> 1) ^ logPolynomial;
                    else
                        value >>= 1;
                logTable[i] = value;
            }
        }
    }
}
