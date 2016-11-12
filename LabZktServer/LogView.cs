using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LabZSKServer
{
    public partial class LogView : Form
    {
        public LogView(string logText)
        {
            InitializeComponent();
            richTextBox1.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            richTextBox1.Text = logText;
            Regex regExp = new Regex(@"(={2}.+==|" + @"\n|" + Strings.mistake + @".+\s|" + Strings.canEditSettings + @"\s|" + Strings.notForStudents + @"\s)");
            foreach (Match match in regExp.Matches(richTextBox1.Text))
            {
                richTextBox1.Select(match.Index, match.Length);
                richTextBox1.SelectionColor = Color.Red;
            }
            regExp = new Regex(@"Auto:.+\s");
            foreach (Match match in regExp.Matches(richTextBox1.Text))
            {
                richTextBox1.Select(match.Index, match.Length);
                richTextBox1.SelectionColor = Color.MediumVioletRed;
            }
            regExp = new Regex(@"(" + Strings.registerHasChanged + @".+\s|" + Strings.microcodeHasChanged + @"\s|" + Strings.memHasChanged + @"\s)");
            foreach (Match match in regExp.Matches(richTextBox1.Text))
            {
                richTextBox1.Select(match.Index, match.Length);
                richTextBox1.SelectionColor = Color.OrangeRed;
            }
            regExp = new Regex(@"={6}.+=");
            foreach (Match match in regExp.Matches(richTextBox1.Text))
            {
                richTextBox1.Select(match.Index, match.Length);
                richTextBox1.SelectionColor = Color.Blue;
            }
            regExp = new Regex(@"(={8}.+=|Makro\s|Micro\s)");
            foreach (Match match in regExp.Matches(richTextBox1.Text))
            {
                richTextBox1.Select(match.Index, match.Length);
                richTextBox1.SelectionColor = Color.Green;
            }
            richTextBox1.Select(0, 0);
        }

    }
}
