using LabZSK.StaticClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LabZSK.Simulation
{
    /// <summary>
    /// Class for logs creation and management
    /// </summary>
    public class LogManager
    {
        public static ConnectedClient _TcpClient;//jak zamykanie logu to przerywanie polaczenia
        private MemoryStream inMemoryLog = new MemoryStream();
        private static readonly LogManager instance;
        private string LogFile = string.Empty;
        private int currentLogLine = 0;
        public static bool isConnected { get; set; }
        private static List<Thread> allThreads = new List<Thread>();
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
                if (_TcpClient != null)
                {
                    Thread t = new Thread(() =>
                    {
                        try
                        {
                            _TcpClient.SendData(currentLogLine, v);
                        }
                        catch (ThreadAbortException)
                        {
                            
                        }
                        catch (Exception)
                        {
                            foreach (Thread th in allThreads)
                                th.Abort();
                            _TcpClient = null;
                            isConnected = false;
                        }
                    });
                    allThreads.Add(t);
                    t.Start();
                }
                else
                    isConnected = false;
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

        public void addTcpClient(string name, string lastName, string group, string ipAddress, string remotePort, string password)
        {
            _TcpClient = new ConnectedClient(name, lastName, group, ipAddress, remotePort, password);
            Thread t = new Thread(() =>
            {
                try
                {
                    _TcpClient.TryRegisterOnServer();
                }
                catch (ThreadAbortException)
                {

                }
                catch (Exception)
                {

                }
            });
            allThreads.Add(t);
            t.Start();
        }

        public void closeConnection()
        {
            if (_TcpClient != null)
            {
                Thread t = new Thread(() =>
                {
                    try
                    {
                        _TcpClient.SendData(-1, "EOT");

                    }
                    catch { }
                });
                t.Start();

                bool finished = t.Join(15000);
                if (!finished)
                    t.Abort();
            }
        }
        public class ConnectedClient
        {
            private TcpClient _client;
            private string name, lastName, group, ipAddress, remotePort, password;
            private StreamReader sReader;
            private StreamWriter sWriter;
            private string sData;
            public ConnectedClient(string name, string lastName, string group, string ipAddress, string remotePort, string password)
            {
                _client = new TcpClient();
                _client.Connect(ipAddress, Convert.ToInt32(remotePort));
                this.name = name;
                this.lastName = lastName;
                this.group = group;
                this.password = password;
                this.ipAddress = ipAddress;
                this.remotePort = remotePort;
                sReader = new StreamReader(_client.GetStream(), Encoding.Unicode);
                sWriter = new StreamWriter(_client.GetStream(), Encoding.Unicode);
                isConnected = true;
            }
            public void TryRegisterOnServer()
            {
                try
                {
                    sData = password + ":" + name + ":" + lastName + ":" + group + ":" + ipAddress + ":" + remotePort;
                    sWriter.WriteLine(sData);
                    sWriter.Flush();
                }
                catch (Exception)
                {
                    ;
                }
            }
            public void SendData(int rowIndex, string data)
            {
                sData = rowIndex + ":" + data;
                sWriter.WriteLine(sData);
                sWriter.Flush();
            }
            public void Disconnect()
            {
                _client.Close();
                //_TcpClient = null;///
            }
            public void HandleCommunication()
            {
                try
                {
                    int rowNum = 0;
                    while (isConnected)
                    {
                        //Console.Write("&gt; ");
                        //sData = Console.ReadLine();
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


                        //logManager musi dostac te metody, string są do niego wysłane i to on inicjuje wysyłanie pakietów

                        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        sData = rowNum++ + ":To jest log";
                        sWriter.WriteLine(sData);
                        sWriter.Flush();
                        Thread.Sleep(2000);
                        // String sDataIncomming = _sReader.ReadLine();
                    }
                }
                catch (Exception)
                {
                    isConnected = false;
                }
            }
        }

    }
}
