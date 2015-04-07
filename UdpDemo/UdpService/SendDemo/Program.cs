using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                UdpService.UdpService.Send("lalal");
                Thread.Sleep(1000);
            }
        }
    }
}
