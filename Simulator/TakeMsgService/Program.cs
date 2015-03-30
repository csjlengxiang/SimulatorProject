//#define debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
namespace TakeMsgService
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpService udpService = null;
#if debug
            udpService = new UdpService("127.0.0.1", "9000", "127.0.0.1", "8000");
#else
            udpService = new UdpService("182.168.100.50", "9000", "182.168.100.48", "8000");
#endif
            Console.WriteLine("udp接受、短信服务开启");
            Console.ReadKey();
            Console.WriteLine("再按就退出了");
            Console.ReadKey();
            Console.WriteLine("再按就退出了");
            Console.ReadKey();
            Console.WriteLine("再按就退出了");
            Console.ReadKey();
            Console.WriteLine("再按就退出了");
            Console.ReadKey();
            Console.WriteLine("再按就退出了");
            Console.ReadKey();
        }
    }
}
