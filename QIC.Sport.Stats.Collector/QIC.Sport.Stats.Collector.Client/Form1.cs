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
        public Form1()
        {
            InitializeComponent();
            tw = new TextBoxWriter(this.textBox1);
            #region Test

            HandleProcessTest();

            //var path = @"C:\Users\Gaushee\Desktop\temp.txt";
            //var data = File.ReadAllText(path);
            //var h = new XmlHelper(data);
            //var xp4 = "//c";

            //var n = h.GetValue(xp4);

            //HtmlAgilityPack.HtmlDocument d = new HtmlAgilityPack.HtmlDocument();
            //d.LoadHtml(n);
            //var root = d.DocumentNode;

            //var ns1 = root.SelectSingleNode("//ul[@class=' jdlvl_2']");

            //var nn = ns1.SelectSingleNode("//ul[contains(@class,'jdlvl_3')]");


            //foreach (var state in ns1.ChildNodes)
            //{
            //    var s3 = state.SelectNodes("ul/li[@class='floater ']");

            //    var ss3 = state.SelectNodes("ul/li[contains(@class,'cc-')]");


            //    foreach (var s3e in s3)
            //    {
            //        var ss4 = s3e.SelectSingleNode("ul[@class=' jdlvl_4']");


            //        var s4 = s3e.SelectNodes("ul/li");
            //        if (s4 == null) continue;

            //        foreach (var s4e in s4)
            //        {
            //            var ss5 = s4e.SelectSingleNode("ul[@class=' jdlvl_5']");
            //            var s5 = s4e.SelectNodes("ul/li");
            //        }
            //    }
            //}

            #endregion

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
                Param = new TeamParam(){TeamId = "223423",PlayerId = "12334"},
                Html = txt
            };
            ph.Process(data);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Console.SetOut(tw);
            var type = ConfigSingleton.CreateInstance().GetAppConfig<string>("ReptileType");

            IReptile r = IocUnity.GetService<IReptile>(type);

            r.Start();

            Console.WriteLine("Start OK!");
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
