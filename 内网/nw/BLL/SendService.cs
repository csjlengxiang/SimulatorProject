﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BLL
{
    public class SendService
    {
        // 余额信息
        private struct BalanceResult
        {
            public int nResult;
            public long dwCorpId;
            public int nStaffId;
            public float fRemain;
        }

        // 接口地址
        private static readonly string POST_URL = "http://smsapi.c123.cn/OpenPlatform/OpenApi";

        //接口账号：1001@501117410001
        //接口 Key：0C5FBEC8E978E60C3791D093A1F34B08
        //通道组ID: 52

        // 接口帐号, 请换成你的帐号, 格式为 1001@+8位登录帐号+0001
        private static readonly string ACCOUNT = "1001@501117410001";

        // 接口密钥, 请换成你的帐号对应的接口密钥
        private static readonly string AUTHKEY = "0C5FBEC8E978E60C3791D093A1F34B08";

        // 通道组编号, 请换成你的帐号可以使用的通道组编号
        private static readonly uint CGID = 52;

        /************************************************************************/
        /* UrlEncode
        /* 对指定字符串进行URL标准化转码
        /************************************************************************/
        private string UrlEncode(string text, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byData = encoding.GetBytes(text);
            for (int i = 0; i < byData.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byData[i], 16));
            }
            return sb.ToString();
        }

        /************************************************************************/
        /* sendQuery
        /* 向指定的接口地址POST数据并返回响应数据
        /************************************************************************/
        private string sendQuery(string url, string param)
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
                return sr.ReadToEnd();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

            sr.Close();
            return null;
        }

        /************************************************************************/
        /* 分析返回的结果值
        /************************************************************************/
        private int parseResult(string sResult)
        {
            if (sResult != null)
            {
                try
                {
                    // 对返回值分析
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(sResult);
                    XmlElement root = xml.DocumentElement;
                    string sRet = root.GetAttribute("result");
                    return Convert.ToInt32(sRet);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return -100;
        }

        private BalanceResult parseBalanceResult(string sResult)
        {
            BalanceResult ret = new BalanceResult();
            int nRet = parseResult(sResult);
            ret.nResult = nRet;
            if (nRet < 0) return ret;

            try
            {
                // 对返回值分析
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sResult);
                XmlElement root = xml.DocumentElement;
                if (nRet > 0)
                {
                    XmlNode item = xml.SelectSingleNode("/xml/Item");
                    if (item != null)
                    {
                        ret.dwCorpId = Convert.ToInt64(item.Attributes["cid"].Value);
                        ret.nStaffId = Convert.ToInt32(item.Attributes["sid"].Value);
                        ret.fRemain = (float)Convert.ToDouble(item.Attributes["remain"].Value);
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return ret;
        }

        /************************************************************************/
        /* 取帐号余额
        /************************************************************************/
        private BalanceResult getBalance()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("action=getBalance&ac=");
            sb.Append(ACCOUNT);
            sb.Append("&authkey=");
            sb.Append(AUTHKEY);

            string sResult = sendQuery(POST_URL, sb.ToString());
            return parseBalanceResult(sResult);
        }

        /************************************************************************/
        /* 群发接口
        /* 手机号码, 如有多个使用逗号分隔, 支持1~3000个号码
        /* 内容, 500字以内 
        /************************************************************************/
        public int sendOnce(string mobile, string content, uint uCgid = 0, uint uCsid = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("action=sendOnce&ac=");
            sb.Append(ACCOUNT);
            sb.Append("&authkey=");
            sb.Append(AUTHKEY);
            sb.Append("&cgid=");
            sb.Append(uCgid > 0 ? uCgid : CGID);
            if (uCsid > 0)
            {
                sb.Append("&csid=");
                sb.Append(uCsid);
            }
            sb.Append("&m=");
            sb.Append(mobile);
            sb.Append("&c=");
            sb.Append(UrlEncode(content, Encoding.UTF8));

            string sResult = sendQuery(POST_URL, sb.ToString());
            return parseResult(sResult);
        }

        /************************************************************************/
        /* 一对一批量发送接口
        /* 手机号码, 如有多个使用逗号分隔, 支持2~100个号码
        /* 内容, 500字以内, 多个用{|}分隔
        /************************************************************************/
        private int sendBatch(string mobile, string content, uint uCgid = 0, uint uCsid = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("action=sendBatch&ac=");
            sb.Append(ACCOUNT);
            sb.Append("&authkey=");
            sb.Append(AUTHKEY);
            sb.Append("&cgid=");
            sb.Append(uCgid > 0 ? uCgid : CGID);
            if (uCsid > 0)
            {
                sb.Append("&csid=");
                sb.Append(uCsid);
            }
            sb.Append("&m=");
            sb.Append(mobile);
            sb.Append("&c=");
            sb.Append(UrlEncode(content, Encoding.UTF8));

            string sResult = sendQuery(POST_URL, sb.ToString());
            return parseResult(sResult);
        }

        /************************************************************************/
        /* 动态参数发送接口
        /* 手机号码, 如有多个使用逗号分隔, 支持2~100个号码
        /* 内容, 500字以内, 允许使用{p1}~{p10}指定最大10个动态参数
        /* 参数数组, 每个参数的值和号码数相同, 值之间用{|}分隔 
        /************************************************************************/
        private int sendParam(string mobile, string content, string[] param, uint uCgid = 0, uint uCsid = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("action=sendParam&ac=");
            sb.Append(ACCOUNT);
            sb.Append("&authkey=");
            sb.Append(AUTHKEY);

            for (int i = 0; i < param.Length; i++)
            {
                if (param[i] != null && param[i].Length > 0)
                {
                    sb.Append("&p");
                    sb.Append(i + 1);
                    sb.Append("=");
                    sb.Append(param[i]);
                }
            }

            sb.Append("&cgid=");
            sb.Append(uCgid > 0 ? uCgid : CGID);
            if (uCsid > 0)
            {
                sb.Append("&csid=");
                sb.Append(uCsid);
            }
            sb.Append("&m=");
            sb.Append(mobile);
            sb.Append("&c=");
            sb.Append(UrlEncode(content, Encoding.UTF8));

            string sResult = sendQuery(POST_URL, sb.ToString());
            return parseResult(sResult);
        }
    }
}
