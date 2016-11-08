using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace LabZSKServer
{
    class Client
    {
        public Thread operatedBy { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string group { get; set; }
        public string iPAddress { get; set; }
        public int remotePort { get; set; }
        public List<string> clientLog;
        public DateTime whenWasClientConnected;
        public int switchCounter { get; set; }
        public int reconnectAttempts { get; set; }
        public string pathToLog { get; set; }
        public Client(Thread operatedBy, string name, string lastName, string group, string iPAddress, int remotePort, string pathToLog)
        {
            this.operatedBy = operatedBy;
            this.name = name;
            this.lastName = lastName;
            this.group = group;
            this.iPAddress = iPAddress;
            this.remotePort = remotePort;
            this.pathToLog = pathToLog;
            clientLog = new List<string>();
            whenWasClientConnected = DateTime.Now;
            reconnectAttempts = switchCounter = 0;
        }
    }
}
