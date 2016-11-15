﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LabZSK.Controls
{
    public partial class FastRichBox : RichTextBox
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams i_Params = base.CreateParams;
                try
                {
                    Win32.LoadLibrary("MsftEdit.dll");
                    i_Params.ClassName = "RichEdit50W";
                }
                catch { }
                return i_Params;
            }
        }
        public class Win32
        {
            [DllImport("kernel32.dll", EntryPoint = "LoadLibraryW", CharSet = CharSet.Unicode, SetLastError = true)]
            private static extern IntPtr LoadLibraryW(string s_File);

            public static IntPtr LoadLibrary(string s_File)
            {
                IntPtr h_Module = LoadLibraryW(s_File);
                if (h_Module != IntPtr.Zero)
                    return h_Module;

                int s32_Error = Marshal.GetLastWin32Error();
                throw new Win32Exception(s32_Error);
            }
        }
    }
}
