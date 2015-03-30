using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LogService
    {
        public static void Mess(string data, string path = @"c:\GpsMessges")
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                StreamWriter sw = File.AppendText(path + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                sw.WriteLine("【" + DateTime.Now.ToString() + "】" + data);
                sw.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
