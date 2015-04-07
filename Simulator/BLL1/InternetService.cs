using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL1
{
    public class InternetService
    {
        csjSerialPort mySerialPort;
        BackgroundService backgroundService;
        public InternetService(string com)
        {
            mySerialPort = new csjSerialPort(Oper, com);
            mySerialPort.Open();
            backgroundService = new BackgroundService();
        }
        public void Send(string str)
        {
            backgroundService.Oper(str);
            mySerialPort.Send(str);
        }
        private void Oper(string str)
        {
            //短信，调用短信接口
            //同步

        }
    }
}
