using LabZSK.Properties;
using LabZSK.Simulation;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LabZSK
{
    static class Program
    {
        static Mutex singleton = new Mutex(true, "LabZSK");
        private static string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZSK";
        [STAThread]
        static void Main(string[] args)
        {
            if (true/*singleton.WaitOne(TimeSpan.Zero, true)*/)
            {
                if (Settings.Default.FirstRun)
                {
                    MessageBox.Show(Strings.defaultDirectoryMessage, "LabZSK", MessageBoxButtons.OK);
                    Settings.Default.FirstRun = false;
                    Settings.Default.Save();
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (File.Exists(envPath + @"\Env\LabZSK.ini"))
                {
                    //ustawienia ladowac
                    Settings.Default.Save();
                }
                string filename = string.Empty;
                if (args != null && args.Length > 0)
                {
                    filename = args[0];
                }
                //Application.Run(new SplashScreen());
                Settings.Default.IsDevConsole = false;
                Application.Run(new SimView(filename));
            }
            else
            {
                MessageBox.Show(Strings.appAlreadyRunning, "LabZSK", MessageBoxButtons.OK);
            }
        }
    }
}
