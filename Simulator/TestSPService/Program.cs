using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestSPService
{
    class Program
    {
        static void Main(string[] args)
        {
            BLL.SPService spService = new BLL.SPService("com5");
            int cnt = 0;
            string str = "";
            while(true)
            {
                //string str = Console.ReadLine();
                //spService.Send(str);
                int len = str.Length;
                spService.Send(str);
                str += "1";
                //Thread.Sleep(1);
            }
        }
    }
}
