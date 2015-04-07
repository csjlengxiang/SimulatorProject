using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UdpService
{
    public delegate void UdpServiceReciveDelege(string recStr);
    public class UdpService
    {
        static UdpClient udp;
        static IPEndPoint sendHost;
        static UdpServiceReciveDelege udpServiceRecive;
        public static void Send(string msg, bool isSend = true, UdpServiceReciveDelege _udpServiceRecive = null)
        {
            if (udp == null)
            {
                string localIP = "127.0.0.1";
                string localPort = "8001";
                string sendIP = "127.0.0.1";
                string sendPort = "8000";
                if (!isSend)
                {
                    string tmp = localPort;
                    localPort = sendPort;
                    sendPort = tmp;
                }
                udpServiceRecive = _udpServiceRecive;
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

                            if (udpServiceRecive != null)
                                udpServiceRecive(str);

                            //Console.WriteLine(str);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                ));
            }
            if (isSend)
            {
                byte[] _b = Encoding.UTF8.GetBytes(msg);
                udp.Send(_b, _b.Length, sendHost);
            }
             
        }

    }
}
