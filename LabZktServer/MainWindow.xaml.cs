using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        DispatcherTimer refreshListView = new DispatcherTimer();
        private static bool isRefreshing = true;
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
            refreshListView.Tick += RefreshListView_Tick;
            refreshListView.Interval = new TimeSpan(0, 0, 15);
            refreshListView.Start();
        }

        private void RefreshListView_Tick(object sender, EventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.Refresh();
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
                _server = null;
                handleNewClients = null;
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
            TcpClient tcpClient = null;
            string sData = "";
            try
            {
                tcpClient = (TcpClient)obj;
                StreamWriter sWriter = new StreamWriter(tcpClient.GetStream(), Encoding.Unicode);
                StreamReader sReader = new StreamReader(tcpClient.GetStream(), Encoding.Unicode);
                bool bClientConnected = true;
                sData = sReader.ReadLine();
                string[] split = sData.Split(':');
                try
                {
                    string path = envPath + "\\" + currentDirectory + "\\" + split[2] + split[1] + split[3] + ".log";
                    if (split[0] == pass)
                    {
                        client = new Client(Thread.CurrentThread, split[1], split[2], split[3], split[4], Convert.ToInt32(split[5]), path);
                        //Dispatcher.BeginInvoke(new Action(() => allClients.Add(client))).Wait();
                        allClients.Add(client);
                        Dispatcher.BeginInvoke(new Action(() => listView.Items.Refresh())).Wait();

                        if (!Directory.Exists(envPath + "\\" + currentDirectory))
                            Directory.CreateDirectory(envPath + "\\" + currentDirectory);
                        try
                        {
                            while (bClientConnected)
                            {
                                sData = sReader.ReadLine();
                                if (sData == "ping")
                                {
                                    sWriter.WriteLine("ping");
                                    sWriter.Flush();
                                }
                                else
                                {
                                    using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Append), Encoding.Unicode))
                                    {
                                        sw.Write(sData + "\n");
                                    }
                                    client.clientLog += (sData) + "\n";
                                    if (sData == "-1:EOT")
                                        bClientConnected = false;
                                    if(isRefreshing)
                                        Dispatcher.BeginInvoke(new Action(() => RefreshContent()));
                                }
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
                tcpClient.Close();
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
                button_Stop.Content = "||";
            }
            else
            {
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
