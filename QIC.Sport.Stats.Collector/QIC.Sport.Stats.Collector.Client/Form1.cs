using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ML.Infrastructure.Config;
using ML.Infrastructure.IOC;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile;
using HtmlAgilityPack;
using QIC.Sport.Stats.Collector.BetRadar;
using QIC.Sport.Stats.Collector.BetRadar.Handle;
using QIC.Sport.Stats.Collector.BetRadar.Param;

namespace QIC.Sport.Stats.Collector.Client
{
    public partial class Form1 : Form
    {
        private TextBoxWriter tw;
        private IReptile reptile;
        public Form1()
        {
            InitializeComponent();
            tw = new TextBoxWriter(this.textBox1);

            //HandleProcessTest();

        }

        private void HandleProcessTest()
        {
            Console.SetOut(tw);
            var type = ConfigSingleton.CreateInstance().GetAppConfig<string>("ReptileType");

            IReptile r = IocUnity.GetService<IReptile>(type);

            var ph = new TeamHandle();

            var path = @"C:\Users\Gaushee\Desktop\555.txt";
            var txt = File.ReadAllText(path);
            var data = new BRData
            {
                Param = new TeamParam() { TeamId = "223423", PlayerId = "12334" },
                Html = txt
            };
            ph.Process(data);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Console.SetOut(tw);
            Console.WriteLine("Beginning ...");
            var type = ConfigSingleton.CreateInstance().GetAppConfig<string>("ReptileType");

            reptile = IocUnity.GetService<IReptile>(type);

            reptile.Start();

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            reptile.Stop();
        }
    }

    public class TextBoxWriter : System.IO.TextWriter
    {
        private TextBox tbox;
        delegate void VoidAction();

        public TextBoxWriter(TextBox box)
        {
            tbox = box;
        }

        public override void Write(string value)
        {
            VoidAction action = delegate
            {
                tbox.AppendText(value);
            };
            try
            {
                tbox.BeginInvoke(action);
            }
            catch (Exception)
            {

            }

        }

        public override void WriteLine(string value)
        {
            VoidAction action = delegate
            {
                tbox.AppendText(value + "\r\n");
            };
            try
            {
                tbox.BeginInvoke(action);
            }
            catch (Exception)
            {


            }

        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
