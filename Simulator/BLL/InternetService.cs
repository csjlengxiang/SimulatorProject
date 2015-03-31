using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class InternetService
    {
        csjSerialPort mySerialPort;
        public InternetService(string com)
        {
            mySerialPort = new csjSerialPort(Oper, com);
            mySerialPort.Open();
        }
        public void Send(string str)
        {
            mySerialPort.Send(str);
        }
        private void Oper(string str)
        {
            //调用短信接口...

        }
    }
}
