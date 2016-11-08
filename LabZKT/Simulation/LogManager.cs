using LabZSK.StaticClasses;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LabZSK.Simulation
{
    /// <summary>
    /// Class for logs creation and management
    /// </summary>
    public class LogManager
    {
        private MemoryStream inMemoryLog = new MemoryStream();
        private static readonly LogManager instance;
        private string LogFile = string.Empty;
        private int currentLogLine = 0;
        /// <summary>
        /// Initialize singleton instance
        /// </summary>
        public static LogManager Instance
        {
            get
            {
                return instance;
            }
        }
        private LogManager() { }
        static LogManager()
        {
            instance = new LogManager();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToLog">String representing path to log file</param>
        /// <returns>Boolean indicating wheter log is valid</returns>
        public bool checkLogIntegrity(string pathToLog)
        {
            bool toReturn = false;
            uint crc = CRC.ComputeChecksum(File.ReadAllBytes(pathToLog));
            if (crc == 0x0)
                toReturn = true;
            else toReturn = false;
            return toReturn;
        }
        /// <summary>
        /// Appends data to current log file
        /// </summary>
        /// <param name="v">String which will be appended to log file</param>
        public void addToMemory(string v)
        {
            if (LogFile != string.Empty)
            {
                FileInfo fileInfo = new FileInfo(LogFile);
                int len = 0;
                byte[] lineIndex = Translator.GetBytes("#!#" + currentLogLine++ + "#!#", out len);
                inMemoryLog.Write(lineIndex, 0, len);
                byte[] bytes = Translator.GetBytes(v, out len);
                inMemoryLog.Write(bytes, 0, len);

                FileStream fs = fileInfo.Open(FileMode.Open);
                //usuwanie poprzedniego CRC
                long bytesToDelete = 4;
                fs.SetLength(Math.Max(0, fileInfo.Length - bytesToDelete));
                fs.Close();
                //zapisywanie nowej porcji danych
                using (BinaryWriter bw = new BinaryWriter(File.Open(LogFile, FileMode.Append), Encoding.Unicode))
                {
                    bw.Write(bytes);
                }
                //dopisywanie nowego CRC
                uint crc = CRC.ComputeChecksum(File.ReadAllBytes(LogFile));
                using (BinaryWriter bw = new BinaryWriter(File.Open(LogFile, FileMode.Append), Encoding.Unicode))
                {
                    bw.Write(crc);
                }
            }
        }

        /// <summary>
        /// Get array of bytes from this stream
        /// </summary>
        /// <returns>Array of bytes</returns>
        internal byte[] GetBuffer()
        {
            return inMemoryLog.GetBuffer();
        }
        /// <summary>
        /// Initialize new log file
        /// </summary>
        /// <param name="logFile">String representing path to log file</param>
        public void createNewLog(string logFile)
        {
            currentLogLine = 0;
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(logFile, FileMode.Create), Encoding.Unicode))
                {
                }
                LogFile = logFile;
            }
            catch
            {
                MessageBox.Show(Strings.errorLogCreating, "Ups", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Initialize new instance of 'In memory' log file
        /// </summary>
        public void clearInMemoryLog()
        {
            inMemoryLog = new MemoryStream();
            currentLogLine = 0;
        }

        public void showInMemoryLog()
        {
            byte[] arr = new byte[inMemoryLog.Length];
            int len = Convert.ToInt32(inMemoryLog.Length);
            inMemoryLog.Seek(0, SeekOrigin.Begin);
            inMemoryLog.Read(arr, 0, len);
            MessageBox.Show(Encoding.Unicode.GetString(arr));
        }
    }
}
