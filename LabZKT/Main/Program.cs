﻿using LabZKT.Properties;
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
                    Settings.Default.Save();
                }
                bool nMode = false;
                TimeSpan start = new TimeSpan(17, 0, 0);
                TimeSpan end = new TimeSpan(5, 0, 0);
                TimeSpan now = DateTime.Now.TimeOfDay;

                if (now >= start || now <= end)
                {
                    DialogResult dr = MessageBox.Show("Uruchomić w trybie nocnym?", "LabZKT", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                        nMode = true;
                }
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainWindow(nMode));
            }else
            {
                MessageBox.Show("Aplikacja jest już uruchomina!", "LabZKT", MessageBoxButtons.OK);
            }
        }
            
    }
}
