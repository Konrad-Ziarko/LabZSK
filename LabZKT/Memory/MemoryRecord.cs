using System;

namespace LabZKT.Memory
{
    /// <summary>
    /// Class representing operating memory record
    /// </summary>
    public class MemoryRecord
    {
        /// <summary>
        /// String representing record addres
        /// </summary>
        public string addr { get; private set; }
        /// <summary>
        /// String representing record value
        /// </summary>
        public string value { get; private set; }
        /// <summary>
        /// String representing record in hex
        /// </summary>
        public string hex { get; private set; }
        /// <summary>
        /// Value representing record type
        /// </summary>
        public int typ { get; private set; }
        /// <summary>
        /// String representing simple instruction
        /// </summary>
        public string OP { get; private set; }
        /// <summary>
        /// String representing complex instruction
        /// </summary>
        public string AOP { get; private set; }
        /// <summary>
        /// String representing simple instruction data field
        /// </summary>
        public string DA { get; private set; }
        /// <summary>
        /// String representing complex instruction data field
        /// </summary>
        public string N { get; private set; }
        /// <summary>
        /// String representing XSI bits
        /// </summary>
        public string XSI { get; private set; }
        /// <summary>
        /// Initialize instance of MemoryRecord class
        /// </summary>
        /// <param name="a">Addres value 0 to 255</param>
        /// <param name="v">String representing value of this record in binary</param>
        /// <param name="h">Hex representation</param>
        /// <param name="t">Type of record</param>
        public MemoryRecord(int a, string v, string h, int t)
        {
            addr = a.ToString();
            value = v;
            hex = h;
            typ = t;
            XSI = "000";
            if (t == 2)
            {
                OP = value.Substring(0,5);
                XSI = value.Substring(5, 3);
                DA = value.Substring(8, 8);
            }
            else if (t == 3)
            {
                AOP = value.Substring(5, 4);
                N = value.Substring(9, 7);
            }
        }
        /// <summary>
        /// Get value stored in memory record
        /// </summary>
        /// <returns>Int16 value representing memory record</returns>
        public short getInt16Value()
        {
            return Convert.ToInt16(this.hex, 16);
        }
    }
}
