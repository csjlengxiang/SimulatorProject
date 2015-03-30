//#define debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csjSerialPort;
namespace csjSerialPortTest
{
    class Program
    {
        static void Main(string[] args)
        {
#if debug
            string s = "132.2323234 32.323423";
            for (int i = 0; i < s.Length; i++)
                Console.WriteLine(i.ToString() + ":" + Convert.ToInt32(s[i]).ToString());
            byte[] b = Encoding.UTF8.GetBytes(s);

            string str = Guid.NewGuid().ToString();
               
            Console.WriteLine(str.Length);
            //Console.WriteLine(Encoding.Unicode.GetString(b));


#else
            csjSerialPort.csjSerialPort mySerialPort = new csjSerialPort.csjSerialPort("com6");
            
            mySerialPort.Open();
            mySerialPort.DataReceived += new csjSerialPortDataReceived(getBuffer);
            
#endif
            Console.ReadKey();
            mySerialPort.Close();
        }

        static long cnt = 0;
        static void getBuffer(byte[] b)
        {
            Console.WriteLine(b.Length.ToString() + " " + Encoding.UTF8.GetString(b));
        }
    }
}
