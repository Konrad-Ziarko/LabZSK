using LabZKT.Properties;
using LabZKT.Simulation;
using LabZKT.StaticClasses;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT
{
    static class Program
    {
        /// <summary>
        /// Mutex used to detect if application is already running.
        /// </summary>
        static Mutex singleton = new Mutex(true, "LABZKT");
        private static string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
        [STAThread]
        static void Main(string[] args)
        {
            if (singleton.WaitOne(TimeSpan.Zero, true))
            {
                if (Settings.Default.FirstRun)
                {
                    MessageBox.Show(Simulation.Strings.defaultDirectoryMessage, "LabZKT", MessageBoxButtons.OK);
                    Settings.Default.FirstRun = false;
                    Settings.Default.Save();
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new SplashScreen());
                if (File.Exists(envPath + @"\Env\LabZKT.ini"))
                {
                    //ustawienia ladowac
                    Settings.Default.Save();
                }
                string filename = string.Empty;
                filename = @"C:\Users\Konrad\Documents\LabZkt\PO\1.po";
                if (args != null && args.Length > 0)
                {
                    filename = args[0];
                }
                new SimController(filename);
                
            }
            else
            {
                MessageBox.Show(Simulation.Strings.appAlreadyRunning, "LabZKT", MessageBoxButtons.OK);
            }
        }
    }
}
