using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpService.UdpService.Send("lalal", false, Rec);

            rrrConsole.ReadKey();
        }

        static void Rec(string msg)
        {
            Console.WriteLine(msg);
        }
    }
    
}
