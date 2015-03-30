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
            //spService.Send(sp);
            bool suc = false;

            //解析数据
            GJ gj = null;
            suc = gjService.LoadGJ(sp, ref gj);

            if (!suc) return false;

            //插入解析数据于数据库

            //suc = gjService.Insert(gj);
            //if (!suc) return false;

            //开启加锁服务


            #region 可以合并成一个函数...
            //根据轨迹点更新加锁表. 注意：加锁表需要存在
            JS js = null;
            string preZTBJ = null;
            suc = jsService.UpdateByGJAndGetJS(gj, ref js, ref preZTBJ, gjService);

            if (!suc) return false;
            //throw new Exception("加锁表未建立...");

            //获取加锁信息
            //JS js = jsService.SelectBySHWithZT(gjService.GetGJ().SH);
            #endregion
#if debug
            js.ZTBJ = "3";
            gj.DWZT = "2";
#endif
            Console.WriteLine("statu : " + js.ZTBJ);
            //加锁
            if (js.ZTBJ == "1")
            {
                //获得id
                string sjh = czryService.GetSJHFromID(js.HQHYYID);
                string sh = js.SH;
                string ch = js.CH;

                Console.WriteLine("加锁：" + sjh + " " + sh + " " + ch);

                //给手机sjh，发送: sh已经加在ch上
                //调用发出外网...再短信服务...
                spService.Send(sjh + " " + sh + " " + ch + " 加锁");
            }
            // 破锁，报警
            else if (preZTBJ != "3" && gj.DWZT == "2")
            {

                string sjh = czryService.GetSJHFromID(js.HQHYYID);
                string sh = js.SH;
                string ch = js.CH;

                Console.WriteLine("中途破锁：" + sjh + " " + sh + " " + ch);

                spService.Send(sjh + " " + sh + " " + ch + " 破锁");
                //string sjh1 = czryService.GetSJHFromID(js.CZID);
                //string sjh2 = czryService.GetSJHFromID(js.HYZRID);
                //string sh = js.SH;
                //string ch = js.CH;
                //给手机sjh，发送: sh已经加在ch上
                //调用发出外网...再短信服务...
                //spService.Send(sjh1 + "," + sjh2 + "," + sh + "," + ch);

                //搬成历史记录，但是在线不销号...此处需要人工消号...
                //js.sbbh == gs.sbbh
                string gjStr = gjService.GetGJStr(js.SBBH, js.JSSJ);

                suc = jslsService.Insert(js, gjStr);

                if (!suc) return false;

            }
            //确认拆锁
            else if (preZTBJ == "3" && gj.DWZT == "2")
            {
#if debug
                js.JSSJ = "2014/10/2 12:00:00";
#endif
                Console.WriteLine("拆锁破锁");

                //取出轨迹点，组合成历史记录...
                //select t.dwsj,t.jd,t.wd from FDS_GJB t where t.sh='00001' and t.dwsj > to_date('2014/10/2 12:00:00','yyyy/mm/dd hh24:mi:ss') order by t.dwsj

                string gjStr = gjService.GetGJStr(js.SBBH, js.JSSJ);

                //更新将其跟新为一个新的历史记录...
                //insert into FDSGLXT_JSJLLSB t(JLH,QSCZID,ZDCZID,JIARYYHM,JIERYYHM,SH,SJH,CH,JSSJ,YJSPCH,ZTBJ,CSSJ,HPH,LSGJ) values('1100','z1','z2','r1','r2','s0001','1232131312','c0001',to_date('2014/12/18 11:59:00','yyyy/mm/dd hh24:mi:ss'),'pch','1',to_date('2014/12/19 11:59:00','yyyy/mm/dd hh24:mi:ss'),'hph','lsgj');
                //insert into FDSGLXT_JSJLLSB t(JLH,QSCZID,ZDCZID,JIARYYHM,JIERYYHM,SH,SJH,CH,JSSJ,YJSPCH,ZTBJ,CSSJ,HPH,LSGJ) values('1100','z1','z2','r1','r2','s0001','1232131312','c0001',to_date('2014/12/18 11:59:00','yyyy/mm/dd hh24:mi:ss'),'pch','1',to_date('2014/12/19 11:59:00','yyyy/mm/dd hh24:mi:ss'),'hph','lsgj');

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
