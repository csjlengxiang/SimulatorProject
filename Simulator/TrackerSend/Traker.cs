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

namespace TrackerSend
{
    public partial class Traker : Form
    {
        UdpClient send;
        IPEndPoint host;
        public Traker()
        {
            InitializeComponent();

            send = new UdpClient(Convert.ToInt32(Properties.Resources.本地端口.ToString()));

            host = new IPEndPoint(IPAddress.Parse(Properties.Resources.服务端IP.ToString()), Convert.ToInt32(Properties.Resources.服务端端口.ToString()));

            textBox3.Text = Properties.Resources.锁号.ToString();
            textBox4.Text = Properties.Resources.手机号.ToString();

            //groupBox1.Text = Properties.Resources.锁号.ToString() + " " + Properties.Resources.手机号.ToString();
            //ThreadPool.QueueUserWorkItem(new WaitCallback((m) =>
            //    {
            //    }));
            comboBox1.Items.Add("加锁");
            comboBox1.Items.Add("途中定位");
            comboBox1.Items.Add("破锁");
            comboBox1.Items.Add("到站拆锁");
            comboBox1.SelectedIndex = 1;
        }

        private string getRandString()
        {
            return Guid.NewGuid().ToString();
             
            /*
            string ret = "";
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 32; i++)
            {
                ret += Convert.ToChar(r.Next(65, 91));
            }
            return ret;*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double j = -1,w = -1;
            try
            {
                string jw = textBox1.Text.ToString();
                if (jw.Contains(','))
                {
                     j = Convert.ToDouble(jw.Split(',')[0]);
                     w = Convert.ToDouble(jw.Split(',')[1]);
                }
            }
            catch
            {
                MessageBox.Show("经纬度输入有误!请格式:经度,纬度，如:100,100");
                return ;
            }
            string sj;
            try
            {
                sj = textBox2.Text.ToString();
                //year:mouth:day:hour:min


                //int year = Convert.ToInt32(sj.Split(':')[0]);
                //int mouth = Convert.ToInt32(sj.Split(':')[1]);
                //int day = Convert.ToInt32(sj.Split(':')[2]);
                //int hour = Convert.ToInt32(sj.Split(':')[3]);
                //int min = Convert.ToInt32(sj.Split(':')[4]);
            }
            catch
            {
                MessageBox.Show("时间输入有误!请格式如:2015:01:01:00:00");
                return;
            }
            string cd = getRandString() + " " + textBox3.Text.ToString() + " " + textBox4.Text.ToString() + " " + textBox5.Text.ToString() + " " + textBox6.Text.ToString() + " " + sj + " " + j.ToString() + " " + w.ToString() + " " + comboBox1.SelectedIndex.ToString();
            if (MessageBox.Show("发送内容为：" + cd, "发送确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                byte[] b = Encoding.UTF8.GetBytes(cd);
                send.Send(b, b.Length, host);
                //确定按钮的方法
            }
            else
            {
                //取消按钮的方法
            }

        }

        private void Traker_Leave(object sender, EventArgs e)
        {
            send.Close();
        }
    }
}
