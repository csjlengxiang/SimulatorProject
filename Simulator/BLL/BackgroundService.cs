//#define debug
#define log
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
        //后台服务开启后，单例模式，既然是单例其实可以上来先优化好...
        GJService gjService = new GJService();
        JSService jsService = new JSService();
        JSLSService jslsService = new JSLSService();
        CZRYService czryService = new CZRYService();
        PSService psService = new PSService();
        PositionService positionService = new PositionService();
        DXService dxService = new DXService();
        csjSerialPort serialPortService;
        public BackgroundService(string com)
        {
            serialPortService = new csjSerialPort(Oper, com);
            serialPortService.Open();
        }

        public void Oper(string sp)
        {
            //解析数据
            GJ gj = gjService.LoadGJ(sp);

            //插入解析数据于数据库
            gjService.Insert(gj);
            gj.DWDD = positionService.GetNear(Convert.ToDouble(gj.JD), Convert.ToDouble(gj.WD));

            //根据轨迹点更新加锁表. 注意：加锁表需要存在
            JS js = null;
            string preZTBJ = null;

            jsService.UpdateByGJAndGetJS2(gj, ref preZTBJ, ref js);

            //加锁
            if (gj.DWZT == GJ.gjState.js.ToString())
            {
                //获得id
                //给手机sjh，发送: sh已经加在ch上
                //调用发出外网...再短信服务...
                string sjh = czryService.GetSJHFromID(js.HQHYYID);
                string sh = js.SH;
                string ch = js.CH;
                string str = sjh + " " + sh + " " + ch + " 加锁";
                LogService.Mess(str, @"c:\IntranetService");
                dxService.Insert(js.HQHYYID, gj.DWSJ, str, DX.dxlx.js.ToString());
                serialPortService.Send(str);
            }
            // 破锁，未预先确认破锁就破了
            else if (preZTBJ != JS.jsState.cs.ToString() && gj.DWZT == GJ.gjState.ps.ToString())
            {
                string sjh1 = czryService.GetSJHFromID(js.CZID);
                string sjh2 = czryService.GetSJHFromID(js.HYZRID);
                string sh = js.SH;
                string ch = js.CH;

                serialPortService.Send(sjh1 + " " + sh + " " + ch + " 破锁");
                serialPortService.Send(sjh2 + " " + sh + " " + ch + " 破锁");
                string str = sjh1 + " " + sh + " " + ch + " 破锁";
                LogService.Mess(str, @"c:\IntranetService");
                dxService.Insert(js.CZID, gj.DWSJ, str, DX.dxlx.ps.ToString());
                str = sjh2 + " " + sh + " " + ch + " 破锁";
                LogService.Mess(str, @"c:\IntranetService");
                dxService.Insert(js.HYZRID, gj.DWSJ, str, DX.dxlx.ps.ToString());
                //取出轨迹点，组合成历史记录... 

                string gjStr = gjService.GetGJStr(js.SBBH, js.JSSJ);

                //更新将其跟新为一个新的历史记录...

                jslsService.Insert(js, gjStr);

                //将破锁信息存储...补封操作更新新锁号信息，状态标记为加锁

                psService.Insert(gj.DWSJ, gj.DWDD);

            }
            //确认拆锁
            else if (preZTBJ == JS.jsState.cs.ToString() && gj.DWZT == GJ.gjState.ps.ToString())
            {
#if debug
                js.JSSJ = "2014/10/2 12:00:00";
#endif 
                //取出轨迹点，组合成历史记录... 

                string gjStr = gjService.GetGJStr(js.SBBH, js.JSSJ);

                //更新将其跟新为一个新的历史记录...

                jslsService.Insert(js, gjStr);

                // 直接删了...
                jsService.XiaoHao(js.SBBH);
            }
        }
    }
}
