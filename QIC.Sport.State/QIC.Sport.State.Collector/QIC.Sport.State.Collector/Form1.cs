using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QIC.Sport.State.Collector
{
    public partial class Form1 : Form
    {
        private static int MaxLines = 15;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void ShowInfo(string info)
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (richTextBox1.Lines.Length >= MaxLines)
                {
                    richTextBox1.Clear();
                }
                richTextBox1.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff  " + info));
            });
        }
    }
}
