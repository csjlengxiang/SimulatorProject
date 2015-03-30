using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csjSerialPort;
using System.Threading;
namespace csjSerialPortTestSe
{
    class Program
    {
        static void Main(string[] args)
        {
            csjSerialPort.csjSerialPort mySerialPort = new csjSerialPort.csjSerialPort("com4");

            mySerialPort.Open();
            int cnt = 0;
            while(true)
            {
                byte[] b = new byte[10];
                for (int i=0;i<b.Length;i++)
                    b[i] = (byte)(cnt%100);
                mySerialPort.Send(b);
                Thread.Sleep(1);
                cnt++;
            }
            //mySerialPort.DataReceived += new csjSerialPortDataReceived(getBuffer);
        }
    }
}
