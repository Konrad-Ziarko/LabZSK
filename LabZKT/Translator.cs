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
