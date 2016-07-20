using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabZKT
{
    public static class Translator
    {
        static string[] DecodeComplexTable = new string[16] {"STP", "CMA", "ALA", "ARA", "LRQ", "LLQ", "LLA",
            "LRA", "LCA", "LAI", "LXI", "INX", "DEX", "OND", "ENI", "LDS"};
        static string[] DecodeSimpleTable = new string[32] {"", "ADS", "SUS", "MUL", "DIV", "STQ", "STA", "STQ",
            "LDA", "LDX", "STC", "TXA", "TMQ", "ADX", "SIO", "LIO", "UNB", "BAQ", "BXP", "BXZ", "BXN", "TLD", "BAP",
            "BAZ", "BAN", "LOR", "LPR", "LNG", "EOR", "SRJ", "BDN", "NOP"};
        static Dictionary<string, string> EncodeTable = new Dictionary<string, string>() { { "STP","0000" },
            {"CMA","0001" }, {"ALA","0010" } ,{"ARA","0011" }, {"LRQ","0100" }, {"LLQ","0101" }, {"LLA","0110" },
            {"LRA","0111" }, {"LCA","1000" }, {"LAI","1001" }, {"LXI","1010" }, {"INX","1011" }, {"DEX","1100" },
            {"OND","1101" }, {"ENI","1110" }, {"LDS","1111" },
            {"ADS","00001" }, {"SUS","00010" }, {"MUL","00011" }, {"DIV","00100" }, {"STQ","00101" }, {"STA","00110" },
            {"STX","00111" }, {"LDA","01000" }, {"LDX","01001" }, {"STC","01010" }, {"TXA","01011" }, {"TMQ","01100" },
            {"ADX","01101" }, {"SIO","01110" }, {"LIO","01111" }, {"UNB","10000" }, {"BAQ","10001" }, {"BXP","10010" },
            {"BXZ","10011" }, {"BXN","10100" }, {"TLD","10101" }, {"BAP","10110" }, {"BAZ","10111" }, {"BAN","11000" },
            {"LOR","11001" }, {"LPR","11010" }, {"LNG","11011" }, {"EOR","11100" }, {"SRJ","11101" }, {"BDN","11110" },
            {"NOP","11111" }
        };

        static Dictionary<string, string> InstructionDescriptions = new Dictionary<string, string>() { { "STP","Stop dynamiczny" },
            {"CMA","Uzupełnienie A do 2" }, {"ALA","Przesunięcie arytmetyczne A w lewo" } ,{"ARA","Przesunięcie arytmetyczne A w prawo" },
            { "LRQ","Przesunięcie logiczne A || MQ w prawo" }, {"LLQ","Przesunięcie logiczne A || MQ w lewo" },
            { "LLA","Przesunięcie logiczne A w lewo" }, {"LRA","Przesunięcie lgoczine A w prawo" },
            { "LCA","Przesunięcie cykliczne A w lewo" }, {"LAI","Ładuj A bezpośrednio" },
            { "LXI","Ładuj RI bezpośrednio" }, {"INX","Zwiększ modyfikator RI" }, {"DEX","Zmniejsz modyfikator RO" },
            {"OND","Podłącz urządzenie" }, {"ENI","Zezwolenie na przerwania" }, {"LDS","Podaj status" },
            {"ADS","Dodawanie" }, {"SUS","Odejmowanie" }, {"MUL","Mnożenie" },
            { "DIV","Dzielenie" }, {"STQ","Zapamiętaj rejestr MQ" }, {"STA","Zapamiętaj akumulator A" },
            {"STX","Zapamiętaj modyfikator RI" }, {"LDA","Ładuj akumulator A" }, {"LDX","Ładuj modyfikator RI" },
            { "STC","Zapamiętaj LR" }, {"TXA","Prześlij RI do A" }, {"TMQ","Prześlij MQ do A" },
            {"ADX","Dodaj do RI" }, {"SIO","Start operacji WE-WY" }, {"LIO","Ładuj licznik słów WE-WY" },
            { "UNB","Skok bezwarunkowy" }, {"BAQ","Skocz jeśli nadmiar A" }, {"BXP","Skocz jeśli RI > 0" },
            {"BXZ","Skocz jeśli RI = 0" }, {"BXN","Skocz jeśli RI< 0" }, {"TLD","Skocz jeśli RI> 0 i RI = RI - 1" },
            { "BAP","Skocz jeśli A > 0" }, {"BAZ","Skocz jeśli A = 0" }, {"BAN","Skocz jeśli A< 0" },
            {"LOR","Suma logiczna A i komórki" }, {"LPR","Iloczyn logiczny A i komórki" }, {"LNG","Negacja logiczna A" },
            { "EOR","Różnica symetryczna A i komórki" }, {"SRJ","Skok ze śladem" }, {"BDN","Skok jeśli urządzenie niedostępne" },
            {"NOP","Nic nie rób" }
        };
        static Dictionary<string, string> MicroOpDescriptions = new Dictionary<string, string>() {
            {"IXRE", "RI -> LALU" },
            {"ILK", "BUS -> LK" },
            {"IRAP", "BUS -> RAP" },
            {"IRAE","SUMA -> RAE" },
            {"IALU", "A -> LALU"},
            {"ILR","BUS -> LR" },
            {"IX","BUS -> X" },
            {"IBE","BUS -> RALU" },
            {"IRI","BUS -> RI" },
            {"IBI","BUS -> RAE" },
            {"IA","BUS -> A" },
            {"IMQ","BUS -> MQ" },
            {"IAS","A0 -> ZNAK" },
            {"IRR","BUS -> RR" },
            {"IRBP","BUS -> RBP" },
            {"SRBP","BUS -> RBP"},

            {"OXE", "X -> RALU"},
            {"OLR", "LR -> BUS" },
            {"ORR", "RR -> BUS"},
            {"ORAE", "RAE -> BUS"},
            {"OX", "X -> BUS"},
            {"ORI","RI -> BUS" },
            {"OA","A -> BUS" },
            {"OMQ","MQ -> BUS" },
            {"OBE","ALU -> BUS" },
            {"ORB","RBP -> BUS" },

            {"NSI","LR+1 -> LR" },
            {"SGN","X0 -> ZNAK" },

            {"ALA","arytmetyczne A w lewo"},
            {"ARA","arytmetyczne A w prawo"},
            {"LRQ","logiczne A || MQ w prawo"},
            {"LLQ","logiczne A || MQ w lewo"},
            {"LLA","logiczne A w lewo"},
            {"LRA","logiczne A w prawo"},
            {"LCA","cykliczne A w lewo"},

            {"CWC","Rozpoczęcie CWC"},
            {"RRC","Rozpoczęcie RRC"},
            {"MUL","16 -> LK"},
            {"DIV","15 -> LK"},
            {"SHT","Operacja przesunięcia"},
            {"IWC","Rozpoczęcie IWC"},
            {"END","Koniec mikroprogramu"},

            {"DLK","LK = [LK]-1"},
            {"SOFF","OFF = 1"},
            {"ROFF","OFF = 0"},
            {"SXRO","XRO = 1"},
            {"RXRO","XRO = 0"},
            {"DRI","RI = RI-1"},
            {"RA","A = 0"},
            {"RMQ","MQ = 0"},
            {"AQ16","NOT A0 -> MQ16"},
            {"RINT","INT = 0"},
            {"OPC","OP lub AOP+32 -> RAPS"},
            {"CEA","Oblicz adres efektywny"},
            {"ENI","Odblokuj przerwania"},

            {"UNB","Zawsze pozytywny"},
            {"TINT","Brak przerwania"},
            {"TIND","Adresowanie pośrednie"},
            {"TAS","A >= 0"},
            {"TXS","RI >= 0"},
            {"TQ15","MQ15 = 0"},
            {"TCR","LK=0 jeśli SHT LK!=0"},
            {"TSD","ZNAK = 0"},
            {"TAO","OFF = 0"},
            {"TXP","RI < 0"},
            {"TXZ","BXZ i RI <> 0 lub TLD i RI = 0"},
            {"TXRO","XRO = 0"},
            {"TAP","A < 0"},
            {"TAZ","A = 0"},

            {"ADS","ALU = LALU + RALU"},
            {"SUS","ALU = LALU - RALU"},
            {"CMX","ALU = (NOT RALU)+1"},
            {"CMA","ALU = (NOT LALU)+1"},
            {"OR","ALU = LALU OR RALU"},
            {"AND","ALU = LALU AND RALU"},
            {"EOR","ALU = LALU XOR RALU"},
            {"NOTL","ALU = NOT LALU"},
            {"NOTR","ALU = NOT RALU"},
            {"L","ALU = LALU"},
            {"R","ALU = RALU"},
            {"INCL","ALU = LALU + 1"},
            {"INLK","ALU = RALU + 1"},
            {"DECL","ALU = LALU - 1"},
            {"DELK","ALU = RALU - 1"},
            {"ONE","ALU = 1"},
            {"ZERO","ALU = 0"},
        };
        private Dictionary<string, long> instCodeS1 = new Dictionary<string, long>()
        {
            {"", 0 },{"IXRE", 2 },{"OLR", 4 },{"ORR", 6 },{"ORAE", 8 },{"IALU", 0xA },{"OXE", 0xC },{"OX", 0xE }
        };
        private Dictionary<string, long> instCodeD1 = new Dictionary<string, long>()
        {
            {"", 0 },{"ILK", 8 },{"IRAP", 0x10 },{"OXE", 0x18 }
        };
        private Dictionary<string, long> instCodeS2 = new Dictionary<string, long>()
        {
            {"", 0 },{"IRAE", 0x8 },{"ORR", 0x10 },{"ORI", 0x18 },{"ORAE", 0x20 },{"OA", 0x28 },{"OMQ", 0x30 },{"OX", 0x38 },
            {"OBE", 0x40 },{"IXRE", 0x48 },{"IALU", 0x50 },{"OXE", 0x58 }
        };
        private Dictionary<string, long> instCodeD2 = new Dictionary<string, long>()
        {
            {"", 0 },{"ILR", 0x8 },{"IX", 0x10 },{"IBE", 0x18 },{"IRI", 0x20 },{"IBI", 0x28 },{"IA", 0x30 },{"IMQ", 0x38 },
            {"OXE", 0x40 },{"NSI", 0x48 },{"IAS", 0x50 },{"SGN", 0x58 },
            {"ALA", 0x8 },{"ARA", 0x10 },{"LRQ", 0x18 },{"LLQ", 0x20 },{"LLA", 0x28 },{"LRA", 0x30 },{"LCA", 0x38 }
        };
        private Dictionary<string, long> instCodeS3 = new Dictionary<string, long>()
        {
            {"", 0 },{"ORI", 1 },{"OLR", 2 },{"OA", 3 },{"ORAE", 4 },{"OMQ", 5 },{"ORB", 6 },{"OXE",7 }
        };
        private Dictionary<string, long> instCodeD3 = new Dictionary<string, long>()
        {
            {"", 0 },{"ILR", 0x1 },{"IX", 0x2 },{"IBE", 0x3 },{"IRI", 0x4 },{"IBI", 0x5 },{"IA", 0x6 },{"IMQ", 0x7 },
            {"OXE", 0x8 },{"NSI", 0x9 },{"IAS", 0xA },{"SGN", 0xB },{"IRR", 0xC },{"IRBP", 0xD },{"SRBP", 0xE }
        };
        private Dictionary<string, long> instCodeC1 = new Dictionary<string, long>()
        {
            {"", 0 },{"CWC", 2 },{"RRC", 4 },{"MUL", 6 },{"DIV", 8 },{"SHT", 10 },{"IWC", 12 },{"END", 14 }
        };
        private Dictionary<string, long> instCodeC2 = new Dictionary<string, long>()
        {
            {"", 0 },{"DLK", 2 },{"SOFF", 4 },{"ROFF", 6 },{"SXRO", 8 },{"RXRO", 10 },{"DRI", 12 },{"RA", 14 },
            { "RMQ", 16 }, {"AQ16", 18 },{"RINT", 20 },{"OPC", 22 },{"CEA", 24 },{"ENI", 26 }
        };
        private Dictionary<string, long> instCodeTest = new Dictionary<string, long>()
        {
            {"", 0 },{"UNB", 0x1 },{"TINT", 0x2 },{"TIND", 0x3 },{"TAS", 0x4 },{"TXS", 0x5 },{"TQ15", 0x6 },{"TCR", 0x7 },
            {"TSD", 0x8 },{"TAO", 0x9 },{"TXP", 0xA },{"TXZ", 0xB },{"TXRO", 0xC },{"TAP", 0xD },{"TAZ", 0xE }
        };
        private Dictionary<string, long> instCodeALU = new Dictionary<string, long>()
        {
            {"", 0 },{"ADS", 0x1 },{"SUS", 0x2 },{"CMX", 0x3 },{"CMA", 0x4 },{"OR", 0x5 },{"AND", 0x6 },{"EOR", 0x7 },
            {"NOTL", 0x8 },{"NOTR", 0x9 },{"L", 0xA },{"R", 0xB },{"INCL", 0xC },{"INLK", 0xD },{"DECL", 0xE },
            {"DELK", 0xF },{"INE", 0x10 },{"ZERO", 0x11 }
        };
        public static string GetMicroOpDescription(string microOpMnemo)
        {
            string description = "";
            MicroOpDescriptions.TryGetValue(microOpMnemo, out description);
            return description;
        }
        public static string GetInsrtuctionDescription(string instructionMnemo)
        {
            string description = "";
            InstructionDescriptions.TryGetValue(instructionMnemo, out description);
            return description;
        }
        public static string GetInsrtuctionDescription(string instructionMnemo, string binaryData, int instructionType)
        {
            string description = "";
            string instructionCode = "";

            description += instructionMnemo;
            if (instructionCode != null)
                if (instructionType == 2)
                {
                    description += " - ROZKAZ ZWYKŁY\n";
                    description += "OP = " + Convert.ToInt32(binaryData.Substring(0, 5), 2).ToString("X") + "h\n";
                    description += "X = " + binaryData.Substring(5, 1) + "\n";
                    description += "S = " + binaryData.Substring(6, 1) + "\n";
                    description += "I = " + binaryData.Substring(7, 1) + "\n";
                    description += "DA = " + Convert.ToInt32(binaryData.Substring(8, 8), 2).ToString("X") + "h\n";
                }
                else if (instructionType == 3)
                {
                    description += " - ROZKAZ ROZSZERZONY\n";
                    description += "AOP = " + Convert.ToInt32(binaryData.Substring(5, 4), 2).ToString("X") + "h\n";
                    description += "N = " + Convert.ToInt32(binaryData.Substring(9, 7), 2).ToString("X") + "h\n";
                }

            return description;
        }
        public static string EncodeInstruction(string binaryData)
        {
            string encodedInstruction = "";
            EncodeTable.TryGetValue(binaryData, out encodedInstruction);
            return encodedInstruction;
        }
        public static string DecodeInstruction(string instructionCode)
        {
            string decodedInstructionMnemo = "";
            int tmpInt = Convert.ToInt32(instructionCode, 2);
            if (instructionCode.Length == 4)
            {
                decodedInstructionMnemo = DecodeComplexTable[tmpInt];
            }
            else
                decodedInstructionMnemo = DecodeSimpleTable[tmpInt];
            return decodedInstructionMnemo;
        }
        public static byte[] GetBytes(string str, out int length)
        {
            length = str.Length * sizeof(char);
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
