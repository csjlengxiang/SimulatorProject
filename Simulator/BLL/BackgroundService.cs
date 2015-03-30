//#define debug
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BackgroundService
    {
        //后台服务开启后，单例模式
        GJService gjService = new GJService();
        JSService jsService = new JSService();
        JSLSService jslsService = new JSLSService();
        CZRYService czryService = new CZRYService();
        SPService spService; // = new SPService("com5");

        public BackgroundService(SPService _spService)
        {
            spService = _spService;
        }

        public bool Oper(string sp)
        {
            bool suc = false;

            //解析数据
            GJ gj = null;
            suc = gjService.LoadGJ(sp, ref gj);

            if (!suc) return false;

            //插入解析数据于数据库

            suc = gjService.Insert(gj);

            if (!suc) return false;

            //开启加锁服务

            #region 可以合并成一个函数...
            //根据轨迹点更新加锁表. 注意：加锁表需要存在
            JS js = null;
            string preZTBJ = null;

            suc = jsService.UpdateByGJAndGetJS(gj, ref preZTBJ, ref js);

            if (!suc) return false;
            //throw new Exception("加锁表未建立...");
             
            #endregion
#if debug
            js.ZTBJ = "3";
            gj.DWZT = "2";
#endif 
            //加锁
            if (js.ZTBJ == "1")
            {
                //获得id
                string sjh = czryService.GetSJHFromID(js.HQHYYID);
                string sh = js.SH;
                string ch = js.CH;

                //给手机sjh，发送: sh已经加在ch上
                //调用发出外网...再短信服务...
                string str = sjh + " " + sh + " " + ch + " 加锁";
                LogService.Mess(str, @"c:\IntranetService");
                spService.Send(str);

            }
            // 破锁，报警
            else if (preZTBJ != "3" && gj.DWZT == "2")
            {
                string sjh1 = czryService.GetSJHFromID(js.CZID);
                string sjh2 = czryService.GetSJHFromID(js.HYZRID);
                string sh = js.SH;
                string ch = js.CH;

                spService.Send(sjh1 + " " + sh + " " + ch + " 破锁");
                spService.Send(sjh2 + " " + sh + " " + ch + " 破锁");
                string str = sjh1 + " " + sh + " " + ch + " 加锁";
                LogService.Mess(str, @"c:\IntranetService");
                str = sjh2 + " " + sh + " " + ch + " 加锁";
                LogService.Mess(str, @"c:\IntranetService");
            }
            //确认拆锁
            else if (preZTBJ == "3" && gj.DWZT == "2")
            {
#if debug
                js.JSSJ = "2014/10/2 12:00:00";
#endif 
                //取出轨迹点，组合成历史记录... 

                string gjStr = gjService.GetGJStr(js.SBBH, js.JSSJ);

                //更新将其跟新为一个新的历史记录...

                suc = jslsService.Insert(js, gjStr);

                if (!suc) return false;

                // 0 预加锁 1 加锁 2 破锁 3 销号 4 销号完毕
                ////////更新定位状态为4
                // 新版直接删了...
                suc = jsService.XiaoHao(js.SBBH);
                if (!suc) return false;
            }
            return suc;
        }
    }
}
