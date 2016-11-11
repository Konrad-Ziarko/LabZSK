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
using System.Windows.Media;
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
        private bool isRefreshing = true;
        private TcpListener _server;
        private bool _isRunning;
        private Thread handleNewClients;
        private static string pass;
        private static List<Client> allClients = new List<Client>();
        private static List<Thread> allThreads = new List<Thread>();
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
                foreach (Thread t in allThreads)
                {
                    t.Abort();
                }
                allThreads = new List<Thread>();
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
                    if (textBox_Addres.Text == "")
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
                        _server.Server.ReceiveTimeout = _server.Server.ReceiveTimeout = 15000;
                        _server.Server.Ttl = 255;
                        textBox_Addres.Text = ipAddress;
                    }
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
                    allThreads.Add(t);
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
            Client client = null;
            try
            {
                TcpClient tcpClient = (TcpClient)obj;
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
                                client.clientLog+=(sData)+"\n";
                                if (sData == "-1:EOT")
                                    break;
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
            finally
            {
                client.foreColor = "Red";
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
            StartStopRefreshing(true);
            RefreshContent();
            refreshRichTextBox.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            RefreshContent();
        }

        private void RefreshContent()
        {
            textBox.Text = "";
            try
            {
                textBox.Text = allClients[listView.SelectedIndex].clientLog;
            }
            catch { }
            textBox.ScrollToEnd();
        }

        private void StartStopRefreshing(bool start)
        {
            isRefreshing = start;
            if (start)
            {
                
                refreshRichTextBox.Start();
                button_Stop.Content = "||";
            }
            else
            {
                refreshRichTextBox.Stop();
                button_Stop.Content = "|>";
            }
        }
        private void button_Stop_Click(object sender, RoutedEventArgs e)
        {
            StartStopRefreshing(!isRefreshing);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_server != null)
                _server.Stop();
            if (handleNewClients != null)
                handleNewClients.Abort();
            foreach (Thread t in allThreads)
                t.Abort();

        }
    }
}
