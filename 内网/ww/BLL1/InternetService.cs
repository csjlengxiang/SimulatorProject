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
            try
            {
                string[] strs = str.Split('$');

                if (strs.Length == 1)
                {
                    LogService.Mess(strs[0]);
                    return;
                }

                string type = strs[0];
                string ctx = strs[1];
                switch (type)
                {
                    case "1":
                        backgroundService.jsService.YJS(ctx);
                        break;
                    case "2":
                        backgroundService.jsService.YJSQR(ctx);
                        break;
                    case "3":
                        backgroundService.jsService.YJSQX(ctx);
                        break;
                    case "4":
                        backgroundService.jsService.BF(ctx);
                        break;
                    case "5":
                        backgroundService.jsService.CS(ctx);
                        break;
                    case "6":
                        /////////////短信////////////

                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                LogService.Mess(e.Message, @"c:\dxservice");
            }

        }
    }
}
