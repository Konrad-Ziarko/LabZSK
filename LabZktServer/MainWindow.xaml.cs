using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabZktServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpListener _server;
        private bool _isRunning;
        Thread runningThread;
        public static string ipA = "";//do wywalenia
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
            {
                _server.Stop();
                runningThread.Abort();
                _isRunning = false;
                textBox_Addres.Text = "";
                button_Start.Content = "Nasłuchuj";
            }
            else
            {
                try
                {
                    string ipAddress = "";
                    foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                        if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet && item.OperationalStatus == OperationalStatus.Up)
                            foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    ipA = ipAddress = ip.Address.ToString();
                                    break;
                                }
                    _server = new TcpListener(IPAddress.Parse(ipAddress), Convert.ToInt32(textBox_Port.Text));
                    textBox_Addres.Text = ipAddress;
                    _server.Start();

                    _isRunning = true;
                    button_Start.Content = "Stop";

                    runningThread = new Thread(LoopClients);
                    runningThread.Start();
                }
                catch
                {
                    MessageBox.Show("Ustanowienie serwera zakończyło się niepowodzeniem!");
                }
            }
            
        }
        public void LoopClients()
        {
            try
            {
                while (_isRunning)
                {
                    TcpClient newClient = _server.AcceptTcpClient();
                    Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                    t.Start(newClient);
                }
            }catch(ThreadAbortException)
            { }
        }
        public void HandleClient(object obj)
        {
            try
            {
                TcpClient client = (TcpClient)obj;
                StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.Unicode);
                StreamReader sReader = new StreamReader(client.GetStream(), Encoding.Unicode);
                bool bClientConnected = true;
                string sData = null;
                while (bClientConnected)
                {
                    sData = sReader.ReadLine();

                    MessageBox.Show("Client: " + client.Client.RemoteEndPoint + "\n" + sData);

                    // to write something back.
                    // sWriter.WriteLine("Meaningfull things here");
                    // sWriter.Flush();
                }
            }
            catch (ThreadAbortException) { }
        }
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_Start_Copy_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                new ClientDemo(ipA, 1234);
            }).Start();
        }
        private class ClientDemo
        {
            private TcpClient _client;

            private StreamReader _sReader;
            private StreamWriter _sWriter;

            private bool _isConnected;

            public ClientDemo(string ipAddress, int portNum)
            {
                _client = new TcpClient();
                _client.Connect(ipAddress, portNum);

                HandleCommunication();
            }

            public void HandleCommunication()
            {
                _sReader = new StreamReader(_client.GetStream(), Encoding.Unicode);
                _sWriter = new StreamWriter(_client.GetStream(), Encoding.Unicode);

                _isConnected = true;
                string sData = "test";
                while (_isConnected)//while (_isConnected)
                {
                    //Console.Write("&gt; ");
                    //sData = Console.ReadLine();

                    // write data and make sure to flush, or the buffer will continue to 
                    // grow, and your data might not be sent when you want it, and will
                    // only be sent once the buffer is filled.
                    _sWriter.WriteLine(sData);
                    _sWriter.Flush();
                    Thread.Sleep(5000);
                    // if you want to receive anything
                    // String sDataIncomming = _sReader.ReadLine();
                }
            }
        }
    }
}
