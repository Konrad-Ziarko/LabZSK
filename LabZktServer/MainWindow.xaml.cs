using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace LabZSKServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Serwer może generować klucze RSA i rozpoczynać od wysłania klucza publicznego
        private static string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZSKSerwer\\";
        private static string currentDirectory;
        DispatcherTimer refreshRichTextBox = new DispatcherTimer();
        private TcpListener _server;
        private bool _isRunning;
        private Thread handleNewClients;
        private static string pass;
        private static List<Client> allClients = new List<Client>();
        public MainWindow()
        {
            InitializeComponent();
            listView.ItemsSource = allClients;
            refreshRichTextBox.Tick += dispatcherTimer_Tick;
            refreshRichTextBox.Interval = new TimeSpan(0, 0, 3);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
            {
                _server.Stop();
                handleNewClients.Abort();
                _isRunning = false;
                textBox_Addres.Text = "";
                button_Start.Content = "Nasłuchuj";
            }
            else
            {
                pass = textBox_Pass.Text;
                currentDirectory = DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss");
                try
                {
                    string ipAddress = "";
                    foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                        if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet && item.OperationalStatus == OperationalStatus.Up)
                            foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    ipAddress = ip.Address.ToString();
                                    break;
                                }
                    _server = new TcpListener(IPAddress.Parse(ipAddress), Convert.ToInt32(textBox_Port.Text));
                    textBox_Addres.Text = ipAddress;
                    _server.Start();

                    _isRunning = true;
                    button_Start.Content = "Stop";

                    handleNewClients = new Thread(LoopClients);
                    handleNewClients.Start();
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
            }
            catch (ThreadAbortException)
            {
                //server wyłączony, przestań szukać klientów
            }
        }
        public void HandleClient(object obj)
        {
            try
            {
                TcpClient tcpClient = (TcpClient)obj;
                Client client;
                StreamWriter sWriter = new StreamWriter(tcpClient.GetStream(), Encoding.Unicode);
                StreamReader sReader = new StreamReader(tcpClient.GetStream(), Encoding.Unicode);
                bool bClientConnected = true;
                string sData = sReader.ReadLine();
                string[] split = sData.Split(':');
                try
                {
                    string path = envPath + "\\" + currentDirectory + "\\" + split[2] + split[1] + split[3] + ".log";
                    if (split[0] == pass)
                    {
                        client = new Client(Thread.CurrentThread, split[1], split[2], split[3], split[4], Convert.ToInt32(split[5]), path);
                        allClients.Add(client);
                        Dispatcher.BeginInvoke(new Action(() => listView.Items.Refresh())).Wait();

                        if (!Directory.Exists(envPath + "\\" + currentDirectory))
                            Directory.CreateDirectory(envPath + "\\" + currentDirectory);
                        try
                        {
                            while (bClientConnected)
                            {
                                sData = sReader.ReadLine();
                                using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Append), Encoding.Unicode))
                                {
                                    sw.Write(sData);
                                }
                                client.clientLog.Add(sData);
                                // sWriter.WriteLine("");
                                // sWriter.Flush();
                            }
                        }
                        catch (IOException)
                        {
                            //host zerwał połączenie
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    ;//pierwsza paczka danych jest nie poprawna
                }
            }
            catch (ThreadAbortException)
            {
                ;//serwer przerwał połączenie
            }
        }
        private void textBox_Pass_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox_Pass.Text = Regex.Replace(textBox_Pass.Text, @":", "");
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refreshRichTextBox.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Paragraph p;
            FlowDocument document = new FlowDocument();
            foreach (string logLine in allClients[listView.SelectedIndex].clientLog)
            {
                p = new Paragraph(new Run(logLine));
                p.FontSize = 14;
                p.TextAlignment = TextAlignment.Left;
                document.Blocks.Add(p);
            }
            richTextBox.Document = document;
            richTextBox.ScrollToEnd();
        }
    }
}
