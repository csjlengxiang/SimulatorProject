﻿//#define debug
#define log
using DAL;
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
        UdpService udpService;
        public BackgroundService(string com)
        {
            serialPortService = new csjSerialPort(Oper, com);
            serialPortService.Open();
#if debug
            //测短信
            dxService.Insert("jsrid", "2012/12/12 12:00:00", "neirong", "j");
            //Oper();
            //测破锁
            psService.Insert("2012/12/12 12:00:00", "dwddid");
#endif
            udpService = new UdpService("127.0.0.1", "8000", null, null, (msg) => {
                //Console.WriteLine(msg);
                serialPortService.Send(msg); 
            }); //写个闭包看起来不错swift
        }

        public void Oper(string sp = "")
        {
            try
            {
#if debug
            GJ gj = new GJ();
            gj.JD = "100";
            gj.WD = "100";
            gj.ID = Guid.NewGuid().ToString();
            gj.SBBH = "sbbh";
            gj.DWSJ = DateTime.Now.ToString();
            gj.DWZT = GJ.gjState.j.ToString();
            gj.DY = "dy";
#else
                GJ gj = gjService.LoadGJ(sp);
#endif
                //插入解析数据于数据库
                string stmp = "";
                gj.DWDDID = positionService.GetNear(Convert.ToDouble(gj.JD), Convert.ToDouble(gj.WD), ref stmp);
                gj.DWDD = stmp;
                gjService.Insert(gj);

                //根据轨迹点更新加锁表. 注意：加锁表需要存在
                JS js = null;
                string preZTBJ = null;
#if debug
#else
                jsService.UpdateByGJAndGetJS2(gj, ref preZTBJ, ref js);
#endif
                //加锁
                if (gj.DWZT == GJ.js)
                {
                    //获得id
                    //给手机sjh，发送: sh已经加在ch上
                    //调用发出外网...再短信服务...
                    string sjh = czryService.GetSJHFromID(js.HQHYYID);
                    string sh = js.SH;
                    string ch = js.CH;
                    string str = sjh + " " + sh + " " + ch + " j";
                    str = "6$" + str;
                    LogService.Mess(str, @"c:\IntranetService");
                    
                    dxService.Insert(js.HQHYYID, gj.DWSJ, str, DX.js);
                    serialPortService.Send(str);
                }
                // 破锁，未预先确认破锁就破了
                else if (preZTBJ != JS.cs && gj.DWZT == GJ.ps)
                {
                    string sh = js.SH;
                    string ch = js.CH;
                    try
                    {
                        string sjh1 = czryService.GetSJHFromID(js.CZID);
                        string str = sjh1 + " " + sh + " " + ch + " p";
                        str = "6$" + str;
                        serialPortService.Send(str);
                        
                        LogService.Mess(str, @"c:\IntranetService");
                        dxService.Insert(js.CZID, gj.DWSJ, str, DX.ps);
                    }
                    catch (Exception e)
                    {
                        LogService.Mess("继续：" + e.Message, @"c:\fwException");
                    }
                    try
                    {
                        string sjh2 = czryService.GetSJHFromID(js.HYZRID);
                        string str = sjh2 + " " + sh + " " + ch + " p";
                        str = "6$" + str;
                        serialPortService.Send(str); 
                        
                        LogService.Mess(str, @"c:\IntranetService");
                        dxService.Insert(js.HYZRID, gj.DWSJ, str, DX.ps);
                    }
                    catch (Exception e)
                    {
                        LogService.Mess("继续：" + e.Message, @"c:\fwException");
                    }

                    //取出轨迹点，组合成历史记录... 

                    string gjStr = gjService.GetGJStr(js.SBBH, js.JSSJ);

                    //更新将其跟新为一个新的历史记录...

                    jslsService.Insert(js, gjStr);

                    //将破锁信息存储...补封操作更新新锁号信息，状态标记为加锁

                    psService.Insert(gj.DWSJ, gj.DWDDID, gj.SBBH, gj.JD, gj.WD);

                }
                //确认拆锁
                else if (preZTBJ == JS.cs && gj.DWZT == GJ.ps)
                {
                    //取出轨迹点，组合成历史记录... 

                    string gjStr = gjService.GetGJStr(js.SBBH, js.JSSJ);

                    //更新将其跟新为一个新的历史记录...

                    jslsService.Insert(js, gjStr);

                    // 直接删了...
                    jsService.XiaoHao(js.SBBH);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                LogService.Mess(e.Message, @"c:\fwException");
            }
        }
    }
}
