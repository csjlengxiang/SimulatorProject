//#define hv
using DAL1;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL1
{
    class GJService
    {
        //private GJ gj = new GJ();
        private GJDAL gjDal = new GJDAL();

        public GJ LoadGJ(string str)
        {
            try
            {
                string[] strs = str.Split(' ');
                GJ gj = new GJ();
                gj.ID = Guid.NewGuid().ToString();
                gj.SBBH = strs[0];
                gj.DWZT = strs[1];
                gj.DWSJ = strs[2] + " " + strs[3];
                gj.JD = strs[4];
                gj.WD = strs[5];
                gj.DY = "5";
                //gj.DWDD = positionService.GetNear(Convert.ToDouble(gj.JD), Convert.ToDouble(gj.WD));
                return gj;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " 载入轨迹点失败!");
            }
        }
        /// <summary>
        /// 插入已load的gj点,若空则是已load
        /// </summary>
        /// <returns></returns>
        public void Insert(GJ gj)
        {
            try
            {
                string sql = string.Format("insert into FDS_GJB(ID,DWSJ,DWDD,DWDDID,JD,WD,DWZT,DY,SBBH) values('{0}',to_date('{1}','yyyy/mm/dd hh24:mi:ss'),'{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                        gj.ID, gj.DWSJ, gj.DWDD, gj.DWDDID, gj.JD, gj.WD, gj.DWZT, gj.DY, gj.SBBH);
                gjDal.Insert(sql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 通过设备编号获得轨迹，也就是说有几个加锁记录，查询就有几条记录
        /// </summary>
        /// <param name="sbbh"></param>
        /// <param name="jssj"></param>
        /// <returns></returns>
        public string GetGJStr(string sbbh, string jssj)
        {
            try
            {
                List<GJ> gjs = GetGJS(sbbh, jssj);
                int num = gjs.Count;
                string ret = "";
                for (int i = 0; i < num; i++)
                {
                    GJ gj = gjs[i];
                    ret += gj.DWSJ + ',' + gj.JD + ',' + gj.WD;
                    if (i != num - 1) ret += ';';
                }
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List <GJ> GetGJS(string sbbh, string jssj)
        {
            List<GJ> gjs = new List<GJ>();
            string sql;
#if hv
            sql = string.Format("select * from FDS_GJB_Rectify t where t.sbbh='{0}' and t.dwsj >= to_date('{1}','yyyy/mm/dd hh24:mi:ss') order by t.dwsj", sbbh, jssj);
            gjs = gjDal.SelectRectify(sql);
#endif
            sql = string.Format("select * from FDS_GJB t where t.sbbh='{0}' and t.dwsj >= to_date('{1}','yyyy/mm/dd hh24:mi:ss') order by t.dwsj", sbbh, jssj);
            gjs.AddRange(gjDal.Select(sql));
            return gjs;
        }
        public DataTable GetTable(string sbbh, string jssj)
        {
            List<GJ> gjs = GetGJS(sbbh, jssj);
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", System.Type.GetType("System.String"));
            dt.Columns.Add("DWSJ", System.Type.GetType("System.String"));
            dt.Columns.Add("JD", System.Type.GetType("System.String"));
            dt.Columns.Add("WD", System.Type.GetType("System.String"));
            dt.Columns.Add("DWZT", System.Type.GetType("System.String"));
            dt.Columns.Add("DWDD", System.Type.GetType("System.String"));
            dt.Columns.Add("DWDDID", System.Type.GetType("System.String"));
            dt.Columns.Add("DY", System.Type.GetType("System.String"));
            dt.Columns.Add("SBBH", System.Type.GetType("System.String"));
            foreach (GJ gj in gjs)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = gj.ID;
                dr["DWSJ"] = gj.DWSJ;
                dr["JD"] = gj.JD;
                dr["WD"] = gj.WD;
                dr["DWZT"] = gj.DWZT;
                dr["DWDD"] = gj.DWDD;
                dr["DWDDID"] = gj.DWDDID;
                dr["DY"] = gj.DY;
                dr["SBBH"] = gj.SBBH;
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }
}
