using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace testPost
{
    class Program
    {

        private static string PostData(string url, string param)
        {
            // 准备要POST的数据
            byte[] byData = Encoding.UTF8.GetBytes(param);

            // 设置发送的参数
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "POST";
            req.Timeout = 5000;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = byData.Length;

            // 提交数据
            Stream rs = req.GetRequestStream();
            rs.Write(byData, 0, byData.Length);
            rs.Close();
            

            // 取响应结果
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);

            try
            {
                Console.WriteLine(sr.ReadToEnd());
                return sr.ReadToEnd();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

            sr.Close();
            return null;
        }

        static void Main(string[] args)
        {
            while(true)
            {
                string postDate = Console.ReadLine();
                //if (postDate[0] == 'q') break;
                //sbbh=string&state=string&tim=string&jd=string&wd=string
                PostData("http://localhost:8081/DataService.asmx/insert", "sbbh=1&state=2&tim=2015/01/02 12:00:00&jd=4&wd=5");
                
            }
        }
    }
}
