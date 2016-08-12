using LabZKT.StaticClasses;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Trinet.Core.IO.Ntfs;

namespace LabZKT.Simulation
{
    /// <summary>
    /// Static class for logs creation and management
    /// </summary>
    public class LogManager
    {
        #region ADS
        public const int GENERIC_WRITE = 1073741824;
        public const int FILE_SHARE_DELETE = 4;
        public const int FILE_SHARE_WRITE = 2;
        public const int FILE_SHARE_READ = 1;
        public const int OPEN_ALWAYS = 4;
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(string lpFileName,
                                                uint dwDesiredAccess,
                                                uint dwShareMode,
                                                IntPtr lpSecurityAttributes,
                                                uint dwCreationDisposition,
                                                uint dwFlagsAndAttributes,
                                                IntPtr hTemplateFile);
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);
        #endregion
        private MemoryStream inMemoryLog = new MemoryStream();
        private static readonly LogManager instance;
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
        internal void addToMemory(string v, string logFile)
        {
            if (logFile != string.Empty)
            {
                FileInfo fileInfo = new FileInfo(logFile);
                int len = 0;
                byte[] bytes = Translator.GetBytes(v, out len);
                inMemoryLog.Write(bytes, 0, len);

                using (BinaryWriter bw = new BinaryWriter(File.Open(logFile, FileMode.Append), Encoding.UTF8))
                {
                    bw.Write(bytes);
                }
                uint crc = CRC.ComputeChecksum(File.ReadAllBytes(logFile));
                if (!fileInfo.AlternateDataStreamExists("crc"))
                {
                    var stream = CreateFile(
                    logFile + ":crc",
                    GENERIC_WRITE,
                    FILE_SHARE_WRITE,
                    IntPtr.Zero,
                    OPEN_ALWAYS,
                    0,
                    IntPtr.Zero);
                    if (stream != IntPtr.Zero)
                    {
                        CloseHandle(stream);
                    }
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

        internal void createNewLog(string logFile)
        {
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(logFile, FileMode.CreateNew), Encoding.UTF8))
                {
                }
            }
            catch (IOException)
            {
                File.Delete(logFile);
            }
        }
        internal void clearInMemoryLog()
        {
            inMemoryLog = new MemoryStream();
        }
    }
}
