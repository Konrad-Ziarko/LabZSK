using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZKT
{
    class PMModel
    {
        public string filePath { get; set; } 
        public List<MicroOperation> List_MicroOps { get; set; }
        public bool isChanged { get; set; } 
        public DataGridView Grid_PM { get; set; }

        public  PMModel(ref List<MicroOperation> List_MicroOps)
        {
            this.List_MicroOps = List_MicroOps;
            filePath = @"\Env\~micro.zkt";
            isChanged = false;
        }

        public void TimerTick(DataGridView Grid_PM)
        {
            FileInfo fileInfo = new FileInfo(MainWindow.envPath + filePath);
            try
            {
                if (File.Exists(MainWindow.envPath + filePath))
                    fileInfo.Attributes = FileAttributes.Normal;
                fileInfo.Directory.Create();
            }
            catch (Exception)
            {
                fileInfo.Directory.Create();
                File.Delete(MainWindow.envPath + filePath);
            }
            finally
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(MainWindow.envPath + filePath, FileMode.Create)))
                {
                    bw.Write(Grid_PM.Columns.Count);
                    bw.Write(Grid_PM.Rows.Count);
                    foreach (DataGridViewRow row in Grid_PM.Rows)
                    {
                        for (int j = 0; j < Grid_PM.Columns.Count; ++j)
                        {
                            var val = row.Cells[j].Value;
                            bw.Write(true);
                            bw.Write(val.ToString());
                        }
                    }
                }
                uint crc = CRC.ComputeChecksum(File.ReadAllBytes(MainWindow.envPath + filePath));
                using (BinaryWriter bw = new BinaryWriter(File.Open(MainWindow.envPath + filePath, FileMode.Append)))
                {
                    bw.Write(crc);
                }
                fileInfo.Attributes = FileAttributes.Hidden;
            }
        }

        public void SaveTable(string fileName)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenRead(fileName)))
            {
                bw.Write(Grid_PM.Columns.Count);
                bw.Write(Grid_PM.Rows.Count);
                foreach (DataGridViewRow row in Grid_PM.Rows)
                {
                    for (int j = 0; j < Grid_PM.Columns.Count; ++j)
                    {
                        var val = row.Cells[j].Value;
                        bw.Write(true);
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
                List_MicroOps[i] = new MicroOperation(i, Grid_PM[1, i].Value.ToString(), Grid_PM[2, i].Value.ToString(),
                    Grid_PM[3, i].Value.ToString(), Grid_PM[4, i].Value.ToString(),
                    Grid_PM[5, i].Value.ToString(), Grid_PM[6, i].Value.ToString(),
                    Grid_PM[7, i].Value.ToString(), Grid_PM[8, i].Value.ToString(),
                    Grid_PM[9, i].Value.ToString(), Grid_PM[10, i].Value.ToString(),
                    Grid_PM[11, i].Value.ToString());
            }
            isChanged = false;
        }

        public void LoadTable(string fileName)
        {
            string[] split = fileName.Split('.');
            string extension = split[split.Length - 1];

            try
            {
                byte[] dataChunk = File.ReadAllBytes(fileName);
                if (dataChunk.Length >= 6814 && CRC.ComputeChecksum(File.ReadAllBytes(fileName)) == 0)
                    using (BinaryReader br = new BinaryReader(File.OpenRead(fileName)))
                    {
                        int n = br.ReadInt32();
                        int m = br.ReadInt32();
                        if (m == 256 && n == 12)
                        {
                            MicroOperation tmpMicroOperation;
                            string allMicroOpInRow = "";
                            string tmpString = "";
                            for (int i = 0; i < m; ++i)
                            {
                                for (int j = 0; j < n; ++j)
                                {
                                    if (br.ReadBoolean())
                                    {
                                        tmpString = br.ReadString();
                                        allMicroOpInRow += tmpString + " ";
                                        Grid_PM[j, i].Value = tmpString;
                                    }

                                    else
                                        br.ReadBoolean();
                                }
                                string[] attributes = allMicroOpInRow.Split(' ');
                                allMicroOpInRow = "";
                                tmpMicroOperation = new MicroOperation(attributes[0], attributes[1], attributes[2], attributes[3],
                                    attributes[4], attributes[5], attributes[6], attributes[7], attributes[8], attributes[9],
                                    attributes[10], attributes[11]);
                                List_MicroOps[i] = tmpMicroOperation;
                            }
                            isChanged = true;
                        }
                        else
                            MessageBox.Show("To nie jest plik z poprawnym mikroprogramem!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
                    }
                else if (Regex.Match(extension, @"[sS][aA][gG]").Success)
                    //naucz czytania plikow labsaga
                    //
                    ;
                else
                    MessageBox.Show("Wykryto niespójność pliku!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Wykryto niespójność pliku!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
            }
        }

        public void CloseForm()
        {
            for (int i = 0; i < 256; ++i)
            {
                List_MicroOps[i] = new MicroOperation(i, Grid_PM[1, i].Value.ToString(), Grid_PM[2, i].Value.ToString(),
            Grid_PM[3, i].Value.ToString(), Grid_PM[4, i].Value.ToString(),
            Grid_PM[5, i].Value.ToString(), Grid_PM[6, i].Value.ToString(),
            Grid_PM[7, i].Value.ToString(), Grid_PM[8, i].Value.ToString(),
            Grid_PM[9, i].Value.ToString(), Grid_PM[10, i].Value.ToString(),
            Grid_PM[11, i].Value.ToString());
            }
        }

        public void NewMicroOperation(string newMicroInstruction, string currentMicroInstruction)
        {
            Grid_PM.CurrentCell.Value = newMicroInstruction;
            if (Grid_PM.CurrentCell.ColumnIndex == 11 && (Grid_PM.CurrentCell.Value.ToString() == "" || Convert.ToInt32(Grid_PM.CurrentCell.Value) == 0))
                Grid_PM.CurrentCell.Value = "";
            if (newMicroInstruction == "" && Grid_PM.CurrentCell.ColumnIndex == 7)
            {
                string tmp = Grid_PM[4, Grid_PM.CurrentCell.RowIndex].Value.ToString();
                if (tmp == "ALA" || tmp == "ARA" || tmp == "LRQ" || tmp == "LLQ" || tmp == "LLA" || tmp == "LRA" || tmp == "LCA")
                    Grid_PM[4, Grid_PM.CurrentCell.RowIndex].Value = "";
            }
            else if (newMicroInstruction == "SHT")
            {
                int idxRow = Grid_PM.CurrentCell.RowIndex;
                int idxCol = Grid_PM.CurrentCell.ColumnIndex;
                Grid_PM[3, idxRow].Value = "";
                Grid_PM[5, idxRow].Value = "";
                Grid_PM[6, idxRow].Value = "";
                using (PMSubmit form_Radio = new PMSubmit(4, Grid_PM[4, idxRow].Value.ToString(), Grid_PM[7, idxRow].Value.ToString()))
                {
                    var result = form_Radio.ShowDialog();
                    if (result == DialogResult.OK)
                        newMicroInstruction = form_Radio.SelectedInstruction;
                    else
                        newMicroInstruction = currentMicroInstruction;
                    Grid_PM[4, idxRow].Value = newMicroInstruction;
                }
            }
        }
    }
}
