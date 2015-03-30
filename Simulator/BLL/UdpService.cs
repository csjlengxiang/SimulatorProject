using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL
{
    public class UdpService
    {
        UdpClient udp;
        IPEndPoint sendHost;
        SendService sendService = new SendService();
        public UdpService(string localIP, string localPort, string sendIP, string sendPort)
        {
            udp = new UdpClient(new IPEndPoint(IPAddress.Parse(localIP), Convert.ToInt32(localPort)));

            sendHost = new IPEndPoint(IPAddress.Parse(sendIP), Convert.ToInt32(sendPort));

            ThreadPool.QueueUserWorkItem(new WaitCallback((m) =>
                { 
                        IPEndPoint from = null;
                        while (true)
                        {
                            try
                            {
                                byte[] b = udp.Receive(ref from);
                                string str = Encoding.UTF8.GetString(b, 0, b.Length);
                                Console.WriteLine(str);
                                string[] strs = str.Split();
                                string sjh = strs[0];
                                string sh = strs[1];
                                string ch = strs[2];
                                string tp = strs[3];
                                if (tp == "加锁")
                                    sendService.sendOnce(sjh, sh + " 成功加在 " + ch + " 上");
                                else if (tp == "破锁")
                                    sendService.sendOnce(sjh, ch + " 的锁 " + sh + " 损坏，请检查锁状态");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    
                }
            ));
        }
        public void Send(string msg)
        {
            byte[] b = Encoding.UTF8.GetBytes(msg);
            udp.Send(b, b.Length, sendHost);
        }

    }
}
