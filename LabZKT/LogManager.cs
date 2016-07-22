using System;
using System.IO;
using System.Text;

namespace LabZKT
{
    public class LogManager
    {
        private MemoryStream inMemoryLog = new MemoryStream();
        public LogManager()
        {
        }

        internal void addToMemory(string v, string logFile)
        {
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
                using (BinaryWriter bw = new BinaryWriter(File.Open(logFile + "crc", FileMode.Create)))
                {
                    bw.Write(crc);
                }

            }
            catch (UnauthorizedAccessException)
            {

            }
            FileInfo fileInfo = new FileInfo(logFile + "crc");
            fileInfo.Attributes = FileAttributes.Hidden;
        }

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
            inMemoryLog= new MemoryStream();
        }
    }
}
