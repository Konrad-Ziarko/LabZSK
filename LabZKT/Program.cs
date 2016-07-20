using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZKT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static System.Threading.Mutex singleton = new Mutex(true, "LabZKT");

        [STAThread]
        static void Main()
        {
            if (singleton.WaitOne(TimeSpan.Zero, true))
            {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainWindow());
            }else
            {
                MessageBox.Show("Aplikacja jest już uruchomina!", "LabZKT", MessageBoxButtons.OK);
            }
        }
            
    }
}
