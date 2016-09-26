namespace LabZKT.MicroOperations
{
    /// <summary>
    /// Class representing single microoperation
    /// </summary>
    public class MicroOperation
    {
        /// <summary>
        /// Microoperation addres
        /// </summary>
        public string addr { get; private set; }
        /// <summary>
        /// Microoperation S1 field instruction
        /// </summary>
        public string S1 { get; private set; }
        /// <summary>
        /// Microoperation D1 field instruction
        /// </summary>
        public string D1 { get; private set; }
        /// <summary>
        /// Microoperation S2 field instruction
        /// </summary>
        public string S2 { get; private set; }
        /// <summary>
        /// Microoperation D2 field instruction
        /// </summary>
        public string D2 { get; private set; }
        /// <summary>
        /// Microoperation S3 field instruction
        /// </summary>
        public string S3 { get; private set; }
        /// <summary>
        /// Microoperation D3 field instruction
        /// </summary>
        public string D3 { get; private set; }
        /// <summary>
        /// Microoperation C1 field instruction
        /// </summary>
        public string C1 { get; private set; }
        /// <summary>
        /// Microoperation C2 field instruction
        /// </summary>
        public string C2 { get; private set; }
        /// <summary>
        /// Microoperation TEST field instruction
        /// </summary>
        public string Test { get; private set; }
        /// <summary>
        /// Microoperation ALU field instruction
        /// </summary>
        public string ALU { get; private set; }
        /// <summary>
        /// Microoperation NA field value
        /// </summary>
        public string NA { get; private set; }
        /// <summary>
        /// Initialize microoperation class instance
        /// </summary>
        /// <param name="adr">Microoperation addres</param>
        /// <param name="s1">Microoperation S1 filed instruction</param>
        /// <param name="d1">Microoperation D1 filed instruction</param>
        /// <param name="s2">Microoperation S2 filed instruction</param>
        /// <param name="d2">Microoperation D2 filed instruction</param>
        /// <param name="s3">Microoperation S3 filed instruction</param>
        /// <param name="d3">Microoperation D3 filed instruction</param>
        /// <param name="c1">Microoperation C1 filed instruction</param>
        /// <param name="c2">Microoperation C2 filed instruction</param>
        /// <param name="test">Microoperation TEST filed instruction</param>
        /// <param name="alu">Microoperation ALU filed instruction</param>
        /// <param name="na">Microoperation NA filed value</param>
        public MicroOperation(int adr, string s1, string d1, string s2, string d2, string s3, string d3, string c1, string c2, string test, string alu, string na)
        {
            addr = adr.ToString();
            S1 = s1;
            D1 = d1;
            S2 = s2;
            D2 = d2;
            S3 = s3;
            D3 = d3;
            C1 = c1;
            C2 = c2;
            Test = test;
            ALU = alu;
            NA = na;
        }
        /// <summary>
        /// Initialize microoperation class instance
        /// </summary>
        /// <param name="adr">Microoperation addres</param>
        /// <param name="s1">Microoperation S1 filed instruction</param>
        /// <param name="d1">Microoperation D1 filed instruction</param>
        /// <param name="s2">Microoperation S2 filed instruction</param>
        /// <param name="d2">Microoperation D2 filed instruction</param>
        /// <param name="s3">Microoperation S3 filed instruction</param>
        /// <param name="d3">Microoperation D3 filed instruction</param>
        /// <param name="c1">Microoperation C1 filed instruction</param>
        /// <param name="c2">Microoperation C2 filed instruction</param>
        /// <param name="test">Microoperation TEST filed instruction</param>
        /// <param name="alu">Microoperation ALU filed instruction</param>
        /// <param name="na">Microoperation NA filed value</param>
        public MicroOperation(string adr, string s1, string d1, string s2, string d2, string s3, string d3, string c1, string c2, string test, string alu, string na)
        {
            addr = adr;
            S1 = s1;
            D1 = d1;
            S2 = s2;
            D2 = d2;
            S3 = s3;
            D3 = d3;
            C1 = c1;
            C2 = c2;
            Test = test;
            ALU = alu;
            NA = na;
        }
    }
}
