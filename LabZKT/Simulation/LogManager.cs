using LabZKT.StaticClasses;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Trinet.Core.IO.Ntfs;

namespace LabZKT.Simulation
{
    /// <summary>
    /// Class for logs creation and management
    /// </summary>
    public class LogManager
    {
        #region ADS
        private const int GENERIC_WRITE = 1073741824;
        private const int FILE_SHARE_DELETE = 4;
        private const int FILE_SHARE_WRITE = 2;
        private const int FILE_SHARE_READ = 1;
        private const int OPEN_ALWAYS = 4;
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFileName,
                                                uint dwDesiredAccess,
                                                uint dwShareMode,
                                                IntPtr lpSecurityAttributes,
                                                uint dwCreationDisposition,
                                                uint dwFlagsAndAttributes,
                                                IntPtr hTemplateFile);
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);
        #endregion

        private MemoryStream inMemoryLog = new MemoryStream();
        private static readonly LogManager instance;
        private string LogFile = string.Empty;
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
            //a jak nie ma ADS? inne sposoby sprawdzenia integralnosci logu robic!
            bool toReturn = false;
            uint crcFromFile;
            byte[] arr = new byte[4];
            FileInfo fileInfo = new FileInfo(pathToLog);
            uint crc = CRC.ComputeChecksum(File.ReadAllBytes(pathToLog));
            if (fileInfo.AlternateDataStreamExists("crc"))
            {
                using (FileStream fs = fileInfo.GetAlternateDataStream("crc").OpenRead())
                {
                    fs.Read(arr, 0, 4);
                }
                crcFromFile = BitConverter.ToUInt32(arr, 0);
                if (crcFromFile == crc)
                    toReturn = true;
                else toReturn = false;
            }
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
                byte[] bytes = Translator.GetBytes(v, out len);
                inMemoryLog.Write(bytes, 0, len);

                using (BinaryWriter bw = new BinaryWriter(File.Open(LogFile, FileMode.Append), Encoding.UTF8))
                {
                    bw.Write(bytes);
                }
                uint crc = CRC.ComputeChecksum(File.ReadAllBytes(LogFile));
                if (!fileInfo.AlternateDataStreamExists("crc"))
                {
                    var stream = CreateFile(
                    LogFile + ":crc",
                    GENERIC_WRITE,
                    FILE_SHARE_WRITE,
                    IntPtr.Zero,
                    OPEN_ALWAYS,
                    0,
                    IntPtr.Zero);
                    if (stream != IntPtr.Zero)
                        CloseHandle(stream);
                }
                FileStream fs = fileInfo.GetAlternateDataStream("crc").OpenWrite();
                fs.Write(BitConverter.GetBytes(crc), 0, 4);
                fs.Close();
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
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(logFile, FileMode.Create), Encoding.UTF8))
                {
                }
                LogFile = logFile;
            }
            catch
            {
                MessageBox.Show("Wysątpił nieoczekiwany błąd podczas tworzenia logu!", "Ups", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Initialize new instance of 'In memory' log file
        /// </summary>
        public void clearInMemoryLog()
        {
            inMemoryLog = new MemoryStream();
        }
    }
}
