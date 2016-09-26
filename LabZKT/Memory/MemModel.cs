using LabZKT.StaticClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LabZKT.Memory
{
    /// <summary>
    /// Provides and computes data for memory records
    /// </summary>
    public class MemModel
    {
        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
        /// <summary>
        /// Boolean representing whether model was changed
        /// </summary>
        public bool isChanged { get; set; }
        /// <summary>
        /// String representing path to file with MemoryRecords list
        /// </summary>
        public string filePath { get; set; } 
        /// <summary>
        /// List of MemoryRecords
        /// </summary>
        public List<MemoryRecord> List_Memory { get; set; }
        /// <summary>
        /// DataGrid of MemoryRecords
        /// </summary>
        public DataGridView Grid_Mem { get; set; }
        /// <summary>
        /// Initialize instance of model class
        /// </summary>
        /// <param name="List_Memory"></param>
        public MemModel(ref List<MemoryRecord> List_Memory)
        {
            this.List_Memory = List_Memory;
            filePath = @"\Env\~mem.zkt";
            isChanged = false;
        }
        /// <summary>
        /// Load all records from internal list to DataGridView object
        /// </summary>
        public void LoadMemory()
        {
            foreach (MemoryRecord row in List_Memory)
                Grid_Mem.Rows.Add(row.addr, row.value, row.hex, row.typ);
        }

        internal void TimerTick(DataGridView Grid_Mem)
        {
            FileInfo fileInfo = new FileInfo(envPath + filePath);
            try
            {
                if (File.Exists(envPath + filePath))
                    fileInfo.Attributes = FileAttributes.Normal;
                fileInfo.Directory.Create();
            }
            catch (Exception)
            {
                fileInfo.Directory.Create();
                File.Delete(envPath + filePath);
            }
            finally
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(envPath + filePath, FileMode.Create)))
                {
                    bw.Write(Grid_Mem.Columns.Count);
                    bw.Write(Grid_Mem.Rows.Count);
                    foreach (DataGridViewRow row in Grid_Mem.Rows)
                    {
                        for (int j = 0; j < Grid_Mem.Columns.Count; ++j)
                        {
                            var val = row.Cells[j].Value;
                            bw.Write(true);
                            if (j == 3 && val.ToString() == "")
                                bw.Write("0");
                            else
                                bw.Write(val.ToString());
                        }
                    }
                }
                uint crc = CRC.ComputeChecksum(File.ReadAllBytes(envPath + filePath));
                using (BinaryWriter bw = new BinaryWriter(File.Open(envPath + filePath, FileMode.Append)))
                {
                    bw.Write(crc);
                }
                fileInfo.Attributes = FileAttributes.Hidden;
            }
        }
        /// <summary>
        /// Save internal memoryrecords list to file
        /// </summary>
        /// <param name="fileName">String representing path to file</param>
        public void SaveTable(string fileName)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                bw.Write(Grid_Mem.Columns.Count);
                bw.Write(Grid_Mem.Rows.Count);
                foreach (DataGridViewRow row in Grid_Mem.Rows)
                {
                    for (int j = 0; j < Grid_Mem.Columns.Count; ++j)
                    {
                        var val = row.Cells[j].Value;
                        bw.Write(true);
                        if (j == 3 && val.ToString() == "")
                            bw.Write("0");
                        else
                            bw.Write(val.ToString());
                    }
                }
            }
            uint crc = CRC.ComputeChecksum(File.ReadAllBytes(fileName));
            using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Append)))
            {
                bw.Write(crc);
            }
            for (int i = 0; i < 256; ++i)
            {
                List_Memory[i] = new MemoryRecord(i, (string)Grid_Mem.Rows[i].Cells[1].Value, (string)Grid_Mem.Rows[i].Cells[2].Value,
                    Convert.ToInt16(Grid_Mem.Rows[i].Cells[3].Value));
            }
            isChanged = false;
        }
        /// <summary>
        /// Load memoryrecords from file
        /// </summary>
        /// <param name="fileName">String representing path to file</param>
        public void LoadTable(string fileName)
        {
            string[] split = fileName.Split('.');
            string extension = split[split.Length - 1];
            try
            {
                byte[] dataChunk = File.ReadAllBytes(fileName);
                if (dataChunk.Length >= 2974 && CRC.ComputeChecksum(File.ReadAllBytes(fileName)) == 0 && Regex.Match(extension, @"[pP][oO]").Success)
                    using (BinaryReader br = new BinaryReader(File.OpenRead(fileName)))
                    {
                        int n = br.ReadInt32();
                        int m = br.ReadInt32();
                        if (m == 256 && n == 4)
                        {
                            MemoryRecord tmpMemory;
                            string singleMemoryRecord = "";
                            string tmpString = "";
                            for (int i = 0; i < m; ++i)
                            {
                                for (int j = 0; j < n; ++j)
                                {
                                    if (br.ReadBoolean())
                                    {
                                        tmpString = br.ReadString();
                                        singleMemoryRecord += tmpString + " ";
                                        Grid_Mem.Rows[i].Cells[j].Value = tmpString;
                                    }
                                    else
                                        br.ReadBoolean();
                                }
                                string[] attributes = singleMemoryRecord.Split(' ');
                                singleMemoryRecord = "";
                                tmpMemory = new MemoryRecord(Convert.ToInt16(attributes[0]), attributes[1], attributes[2], Convert.ToInt16(attributes[3]));
                                List_Memory[i] = tmpMemory;
                            }
                            isChanged = true;
                        }
                        else
                            MessageBox.Show("To nie jest plik z poprawnym mikroprogramem!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
                    }
                else if (Regex.Match(extension, @"[sS][aA][gG]").Success)
                //naucz czytania plikow labsaga
                //
                {;
                }
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                MessageBox.Show("Wykryto niespójność pliku!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Save all changes on form close
        /// </summary>
        public void CloseForm()
        {
            for (int i = 0; i < 256; ++i)
            {
                List_Memory[i] = new MemoryRecord(i, Grid_Mem.Rows[i].Cells[1].Value.ToString(),
                    Grid_Mem.Rows[i].Cells[2].Value.ToString(), Convert.ToInt16(Grid_Mem.Rows[i].Cells[3].Value));
            }
        }

    }
}
