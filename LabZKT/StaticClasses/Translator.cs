﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

namespace LabZSK.StaticClasses
{
    /// <summary>
    /// Static class with necessary descriptions
    /// </summary>
    public static class Translator
    {
        /// <summary>
        /// String array of complex operations mnemonics
        /// </summary>
        private static string[] DecodeComplexTable = new string[16] {"STP", "CMA", "ALA", "ARA", "LRQ", "LLQ", "LLA",
            "LRA", "LCA", "LAI", "LXI", "INX", "DEX", "CND", "ENI", "LDS"};
        /// <summary>
        /// String array of simple operations mnemonics
        /// </summary>
        private static string[] DecodeSimpleTable = new string[32] {"", "ADD", "SUB", "MUL", "DIV", "STQ", "STA", "STX",
            "LDA", "LDX", "STC", "TXA", "TMQ", "ADX", "SIO", "LIO", "UNB", "BAO", "BXP", "BXZ", "BXN", "TLD", "BAP",
            "BAZ", "BAN", "LOR", "LAND", "LNG", "EOR", "SRJ", "BDN", "NOP"};
        /// <summary>
        /// Dictionary holding binary value of simple and complex operations
        /// </summary>
        private static Dictionary<string, string> EncodeTable = new Dictionary<string, string>() { { "STP","0000" },
            {"CMA","0001" }, {"ALA","0010" } ,{"ARA","0011" }, {"LRQ","0100" }, {"LLQ","0101" }, {"LLA","0110" },
            {"LRA","0111" }, {"LCA","1000" }, {"LAI","1001" }, {"LXI","1010" }, {"INX","1011" }, {"DEX","1100" },
            {"CND","1101" }, {"ENI","1110" }, {"LDS","1111" },
            {"ADD","00001" }, {"SUB","00010" }, {"MUL","00011" }, {"DIV","00100" }, {"STQ","00101" }, {"STA","00110" },
            {"STX","00111" }, {"LDA","01000" }, {"LDX","01001" }, {"STC","01010" }, {"TXA","01011" }, {"TMQ","01100" },
            {"ADX","01101" }, {"SIO","01110" }, {"LIO","01111" }, {"UNB","10000" }, {"BAO","10001" }, {"BXP","10010" },
            {"BXZ","10011" }, {"BXN","10100" }, {"TLD","10101" }, {"BAP","10110" }, {"BAZ","10111" }, {"BAN","11000" },
            {"LOR","11001" }, {"LAND","11010" }, {"LNG","11011" }, {"EOR","11100" }, {"SRJ","11101" }, {"BDN","11110" },
            {"NOP","11111" }
        };

        /// <summary>
        /// Dictionary of description for microoperations
        /// </summary>
        internal static Dictionary<string, string> MicroOpDescriptions = new Dictionary<string, string>() {
            {"IXRE", "RI -> LALU" },{"ILK", "BUS -> LK" },{"IRAP", "BUS -> RAP" },{"IRAE","SUMA -> RAE" },
            {"IALU", "A -> LALU"},{"ILR","BUS -> LR" },{"IX","BUS -> X" },{"IBE","BUS -> RALU" },
            {"IRI","BUS -> RI" },{"IBI","BUS -> RAE" },{"IA","BUS -> A" },{"IMQ","BUS -> MQ" },
            {"IRR","BUS -> RR" },{"IRBP","BUS -> RBP" },{"SRBP","BUS -> RBP"},
            {"OXE", "X -> RALU"},{"OLR", "LR -> BUS" },{"ORR", "RR -> BUS"},{"ORAE", "RAE -> BUS"},
            {"OX", "X -> BUS"},{"ORI","RI -> BUS" },{"OA","A -> BUS" },{"OMQ","MQ -> BUS" },
            {"OBE","ALU -> BUS" },{"ORBP","RBP -> BUS" },{"NSI","LR+1 -> LR" },
            {"MUL","16 -> LK"},{"DIV","15 -> LK"},{"DLK","LK = LK-1"},
            {"SOFF","OFF = 1"},{"ROFF","OFF = 0"},{"SXRO","XRO = 1"},{"RXRO","XRO = 0"},
            {"DRI","RI = RI-1"},{"RA","A = 0"},{"RMQ","MQ = 0"},{"AQ15","NOT A0 -> MQ15"},
            {"RINT","INT = 0"},{"TAS","A >= 0"},{"TXS","RI >= 0"},{"TQ15","MQ15 = 0"},
            {"TAO","OFF = 0"},{"TXP","RI <= 0"},{"TXRO","RI = 0"},
            {"TAP","A <= 0"},{"TAZ","A = 0"},{"ADD","ALU = LALU + RALU"},{"SUB","ALU = LALU - RALU"},
            {"CMX","ALU = (NOT RALU)+1"},{"CMA","ALU = (NOT LALU)+1"},{"OR","ALU = LALU OR RALU"},{"AND","ALU = LALU AND RALU"},
            {"EOR","ALU = LALU XOR RALU"},{"NOTL","ALU = NOT LALU"},{"NOTR","ALU = NOT RALU"},{"L","ALU = LALU"},
            {"R","ALU = RALU"},{"INCL","ALU = LALU + 1"},{"INCR","ALU = RALU + 1"},{"DECL","ALU = LALU - 1"},
            {"DECR","ALU = RALU - 1"},{"ONE","ALU = 1"},{"ZERO","ALU = 0"},
        };

        #region Instruction Codes
        private static Dictionary<string, long> instCodeS1 = new Dictionary<string, long>()
        {
            {"", 0 },{"IXRE", 0x1 },{"OLR", 0x2 },{"ORR", 0x3 },{"ORAE", 0x4 },{"IALU", 0x5 },{"OXE", 0x6 },{"OX", 0x7 }
        };
        private static Dictionary<string, long> instCodeD1 = new Dictionary<string, long>()
        {
            {"", 0 },{"ILK", 0x1 },{"IRAP", 0x2 },{"OXE", 0x3 }
        };
        private static Dictionary<string, long> instCodeS2 = new Dictionary<string, long>()
        {
            {"", 0 },{"IRAE", 0x1 },{"ORR", 0x2 },{"ORI", 0x3 },{"ORAE", 0x4 },{"OA", 0x5 },{"OMQ", 0x6 },{"OX", 0x7 },
            {"OBE", 0x8 },{"IXRE", 0x9 },{"IALU", 0xA },{"OXE", 0xB }
        };
        private static Dictionary<string, long> instCodeD2 = new Dictionary<string, long>()
        {
            {"", 0 },{"ILR", 0x1 },{"IX", 0x2 },{"IBE", 0x3 },{"IRI", 0x4 },{"IBI", 0x5 },{"IA", 0x6 },{"IMQ", 0x7 },
            {"OXE", 0x8 },{"NSI", 0x9 },{"IAS", 0xA },{"SGN", 0xB }, {"ALA", 0xC }, {"ARA", 0xD },{"LRQ", 0xE },
            { "LLQ", 0xF },{"LLA", 0x10 },{"LRA", 0x11 },{"LCA", 0x12 }
        };
        private static Dictionary<string, long> instCodeS3 = new Dictionary<string, long>()
        {
            {"", 0 },{"ORI", 0x1 },{"OLR", 0x2 },{"OA", 0x3 },{"ORAE", 0x4 },{"OMQ", 0x5 },{"ORBP", 0x6 },{"OXE", 0x7 }
        };
        private static Dictionary<string, long> instCodeD3 = new Dictionary<string, long>()
        {
            {"", 0 },{"ILR", 0x1 },{"IX", 0x2 },{"IBE", 0x3 },{"IRI", 0x4 },{"IBI", 0x5 },{"IA", 0x6 },{"IMQ", 0x7 },
            {"OXE", 0x8 },{"NSI", 0x9 },{"IAS", 0xA },{"SGN", 0xB },{"IRR", 0xC },{"IRBP", 0xD },{"SRBP", 0xE }
        };
        private static Dictionary<string, long> instCodeC1 = new Dictionary<string, long>()
        {
            {"", 0 },{"CWC", 0x1 },{"RRC", 0x2 },{"MUL", 0x3 },{"DIV", 0x4 },{"SHT", 0x5 },{"IWC", 0x6 },{"END", 0x7 }
        };
        private static Dictionary<string, long> instCodeC2 = new Dictionary<string, long>()
        {
            {"", 0 },{"DLK", 0x1 },{"SOFF", 0x2 },{"ROFF", 0x3 },{"SXRO", 0x4 },{"RXRO", 0x5 },{"DRI", 0x6 },{"RA", 0x7 },
            { "RMQ", 0x8 }, {"AQ15", 0x9 },{"RINT", 0xA },{"OPC", 0xB },{"CEA", 0xC },{"ENI", 0xD }
        };
        private static Dictionary<string, long> instCodeTest = new Dictionary<string, long>()
        {
            {"", 0 },{"UNB", 0x1 },{"TINT", 0x2 },{"TIND", 0x3 },{"TAS", 0x4 },{"TXS", 0x5 },{"TQ15", 0x6 },{"TLK", 0x7 },
            {"TSD", 0x8 },{"TAO", 0x9 },{"TXP", 0xA },{"TXZ", 0xB },{"TXRO", 0xC },{"TAP", 0xD },{"TAZ", 0xE }
        };
        private static Dictionary<string, long> instCodeALU = new Dictionary<string, long>()
        {
            {"", 0 },{"ADD", 0x1 },{"SUB", 0x2 },{"CMX", 0x3 },{"CMA", 0x4 },{"OR", 0x5 },{"AND", 0x6 },{"EOR", 0x7 },
            {"NOTL", 0x8 },{"NOTR", 0x9 },{"L", 0xA },{"R", 0xB },{"INCL", 0xC },{"INCR", 0xD },{"DECL", 0xE },
            {"DECR", 0xF },{"ONE", 0x10 },{"ZERO", 0x11 }
        };
        #endregion

        /// <summary>
        /// Compute value of RBPS register.
        /// </summary>
        /// <param name="row">Selected row in micro op. memory</param>
        /// <returns>Selected row micro op. RBPS value</returns>
        public static long GetRbpsValue(DataGridViewRow row)
        {
            long rbps;
            try
            {
                rbps = (instCodeS1[row.Cells[1].Value.ToString()] << 45) + (instCodeD1[row.Cells[2].Value.ToString()] << 43)
                + (instCodeS2[row.Cells[3].Value.ToString()] << 39) + (instCodeD2[row.Cells[4].Value.ToString()] << 35)
                + (instCodeS3[row.Cells[5].Value.ToString()] << 32) + (instCodeD3[row.Cells[6].Value.ToString()] << 28)
                + (instCodeC1[row.Cells[7].Value.ToString()] << 25) + (instCodeC2[row.Cells[8].Value.ToString()] << 21)
                + (instCodeTest[row.Cells[9].Value.ToString()] << 16) + (instCodeALU[row.Cells[10].Value.ToString()] << 8);
            }
            catch
            {
                rbps = 0;
            }
            

                return rbps;
        }
        /// <summary>
        /// Get microoperation description
        /// </summary>
        /// <param name="microOpMnemo">String representing mnemonic of microoperation</param>
        /// <returns>String representing description</returns>
        public static string GetMicroOpDescription(string microOpMnemo)
        {
            string description = "";
            ResourceSet resourceSet = Operations.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
                if (entry.Key.ToString() == microOpMnemo)
                {
                    description = entry.Value.ToString();
                    break;
                }
            if (description == "")
                MicroOpDescriptions.TryGetValue(microOpMnemo, out description);
            return description;
        }
        public static string GetMicroOpExtendedDescription(string microOpMnemo)
        {
            string description = "";
            ResourceSet resourceSet = Operations.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
                if (entry.Key.ToString() == microOpMnemo)
                {
                    description = entry.Value.ToString();
                    break;
                }
            if (description == "")
                MicroOpDescriptions.TryGetValue(microOpMnemo, out description);
            return microOpMnemo.PadRight(8, ' ') + description;
        }
        /// <summary>
        /// Get instruction description
        /// </summary>
        /// <param name="instructionMnemo">String representing instruction mnemonic name</param>
        /// <returns>String representing description</returns>
        public static string GetInsrtuctionDescription(string instructionMnemo)
        {
            string description = "";
            ResourceSet resourceSet = Instructions.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
                if (entry.Key.ToString() == instructionMnemo)
                {
                    description = entry.Value.ToString();
                    break;
                }
            return description;
        }
        /// <summary>
        /// Get instruction description
        /// </summary>
        /// <param name="instructionMnemo">String representing instruction mnemonic name</param>
        /// <param name="binaryData">String representing binary value of instruction</param>
        /// <param name="instructionType">Value representing type of instruction</param>
        /// <returns>String representing description</returns>
        public static string GetInsrtuctionDescription(string instructionMnemo, string binaryData, int instructionType)
        {
            string description = "";
            string instructionCode = "";

            //description += instructionMnemo;
            if (instructionCode != null)
                if (instructionType == 2)
                {
                    description += " - " + Strings.simpleInstruction + "\n";
                    description += "OP = " + Convert.ToInt32(binaryData.Substring(0, 5), 2).ToString("X") + "h ("+instructionMnemo+")\n";
                    description += "X = " + binaryData.Substring(5, 1) + "\n";
                    description += "S = " + binaryData.Substring(6, 1) + "\n";
                    description += "I = " + binaryData.Substring(7, 1) + "\n";
                    description += "DA = " + Convert.ToInt32(binaryData.Substring(8, 8), 2).ToString("X") + "h\n";
                }
                else if (instructionType == 3)
                {
                    description += " - " + Strings.complexInstruction + "\n";
                    description += "AOP = " + Convert.ToInt32(binaryData.Substring(5, 4), 2).ToString("X") + "h (" + instructionMnemo + ")\n";
                    description += "N = " + Convert.ToInt32(binaryData.Substring(9, 7), 2).ToString("X") + "h\n";
                }

            return description;
        }
        /// <summary>
        /// Decode simple or complex instruciton
        /// </summary>
        /// <param name="instructionCode">String representing instruction value in binary</param>
        /// <returns>String representing instruction mnemonic name</returns>
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
        /// <summary>
        /// Convert string into bytes array
        /// </summary>
        /// <param name="str">input string</param>
        /// <param name="length">string length</param>
        /// <returns>bytes array for given string</returns>
        public static byte[] GetBytes(string str, out int length)
        {
            length = str.Length * sizeof(char);
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
