namespace LabZKT.Memory
{
    public class MemoryRecord
    {
        public string addr { get; private set; }
        public string value { get; private set; }
        public string hex { get; private set; }
        public int typ { get; private set; }
        public string OP { get; private set; }
        public string AOP { get; private set; }
        public string DA { get; private set; }
        public string N { get; private set; }
        public string XSI { get; private set; }
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
