using LabZKT.Properties;
using System;
using System.Collections.Specialized;
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
        static System.Threading.Mutex singleton = new Mutex(true, "LABZKT");

        [STAThread]
        static void Main()
        {
            if (singleton.WaitOne(TimeSpan.Zero, true))
            {
                if (Settings.Default.FirstRun)
                {
                    MessageBox.Show("Aplikacja wykorzystuje Moje Dokumenty/Dokumenty jako domyślny katalog dla plików!", "LabZKT", MessageBoxButtons.OK);
                    Settings.Default.FirstRun = false;
                    Settings.Default.LogFiles = new System.Collections.Specialized.StringCollection();
                    Settings.Default.Save();
                }
                StringCollection toDelete = new StringCollection();
                foreach (string s in Settings.Default.LogFiles)
                {
                    if (!File.Exists(s) && File.Exists(s + "crc"))
                    {
                        if (File.Exists(s + "crc"))
                            File.Delete(s + "crc");
                    }
                    else if (File.Exists(s) && !File.Exists(s + "crc"))
                        File.Delete(s);
                    else if (!File.Exists(s) && !File.Exists(s + "crc"))
                        toDelete.Add(s);
                }
                foreach (string s in toDelete)
                    Settings.Default.LogFiles.Remove(s);

                Settings.Default.Save();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SplashScreen());
                Application.Run(new MainWindow());
            }
            else
            {
                MessageBox.Show("Aplikacja jest już uruchomina!", "LabZKT", MessageBoxButtons.OK);
            }
        }

    }
}
