//#define debug
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using BLL;
using Model;
namespace TrackerRec
{
    public partial class TrackerRec : Form
    {
        UdpClient rec;
        public TrackerRec()
        {
            InitializeComponent();
            System.Environment.SetEnvironmentVariable("NLS_LANG", " SIMPLIFIED CHINESE_CHINA.ZHS16GBK");
            ThreadPool.QueueUserWorkItem(new WaitCallback((m) =>
                {
                    rec = new UdpClient(Convert.ToInt32(Properties.Resources.本地端口.ToString()));
                    IPEndPoint from = null;
                     
                    BackgroundService bService = new BackgroundService();
                    while (true)
                    {
                        byte[] b = rec.Receive(ref from);
                        string str = Encoding.UTF8.GetString(b, 0, b.Length);

                        ThreadPool.QueueUserWorkItem(new WaitCallback((sp) =>
                        {
                            bService.Oper((string)sp);
                        }), str);
                    }
                }));        }
        private void TrackerRec_Load(object sender, EventArgs e)
        {

        }
    }
}
