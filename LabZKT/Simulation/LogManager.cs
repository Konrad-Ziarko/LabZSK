using LabZKT.StaticClasses;
using System;
using System.IO;
using System.Text;

namespace LabZKT.Simulation
{
    /// <summary>
    /// Static class for logs creation and management
    /// </summary>
    public class LogManager
    {
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
                FileInfo fileInfo;
                int len = 0;
                byte[] bytes = Translator.GetBytes(v, out len);
                inMemoryLog.Write(bytes, 0, len);

                using (BinaryWriter bw = new BinaryWriter(File.Open(logFile, FileMode.Append), Encoding.UTF8))
                {
                    bw.Write(bytes);
                }
                uint crc = CRC.ComputeChecksum(File.ReadAllBytes(logFile));

                try
                {
                    fileInfo = new FileInfo(logFile + "crc");
                    fileInfo.Attributes = FileAttributes.Normal;
                }
                catch (Exception)
                {
                    //No crc file
                }
                using (BinaryWriter bw = new BinaryWriter(File.Open(logFile + "crc", FileMode.Create)))
                {
                    bw.Write(crc);
                }
                fileInfo = new FileInfo(logFile + "crc");
                fileInfo.Attributes = FileAttributes.Hidden;
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
