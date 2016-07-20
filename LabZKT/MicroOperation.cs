using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabZKT
{
    public class MicroOperation
    {
        public string addr { get; set; }
        public string S1 { get; set; }
        public string D1 { get; set; }
        public string S2 { get; set; }
        public string D2 { get; set; }
        public string S3 { get; set; }
        public string D3 { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string Test { get; set; }
        public string ALU { get; set; }
        public string NA { get; set; }

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
