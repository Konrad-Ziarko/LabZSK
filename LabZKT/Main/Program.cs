using LabZKT.Properties;
using LabZKT.Simulation;
using System;
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

        [STAThread]
        static void Main()
        {
            if (singleton.WaitOne(TimeSpan.Zero, true))
            {
                if (Settings.Default.FirstRun)
                {
                    MessageBox.Show("Aplikacja wykorzystuje Moje Dokumenty/Dokumenty jako domyślny katalog dla plików!", "LabZKT", MessageBoxButtons.OK);
                    Settings.Default.FirstRun = false;
                    Settings.Default.Save();
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new SplashScreen());
                SimController sc = new SimController();
            }
            else
            {
                MessageBox.Show("Aplikacja jest już uruchomina!", "LabZKT", MessageBoxButtons.OK);
            }
        }
    }
}
