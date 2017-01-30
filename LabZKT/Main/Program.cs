﻿using LabZSK.Properties;
using LabZSK.Simulation;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace LabZSK
{
    static class Program
    {
        static Mutex singleton = new Mutex(true, "LabZSK");

        private static string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZSK";
        [STAThread]
        static void Main(string[] args)
        {
            if (/*singleton.WaitOne(TimeSpan.Zero, true)*/ true)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Console.Out.Write(Settings.Default.RunIdx);
                Settings.Default.RunIdx += 1;
                Settings.Default.Save();

                string filename = string.Empty;
                if (args != null && args.Length == 1)
                    filename = args[0];
                else if (args != null && args.Length == 2)
                    filename = args[0]+"<*>"+args[1];
                Application.Run(new SplashScreen());
                Application.Run(new SimView(filename));
            }
            else
            {
                //MessageBox.Show(Strings.appAlreadyRunning, "LabZSK", MessageBoxButtons.OK);
            }
        }
    }
}
