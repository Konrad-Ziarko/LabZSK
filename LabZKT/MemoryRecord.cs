namespace LabZKT
{
    public class MemoryRecord
    {
        public string addr { get; set; }
        public string value { get; set; }
        public string hex { get; set; }
        public int typ { get; set; }

        public string OP { get; set; }
        public string AOP { get; set; }
        public string DA { get; set; }
        public string N { get; set; }
        public string XSI { get; set; }
        public string Mnemo { get; set; }
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
    }
}
